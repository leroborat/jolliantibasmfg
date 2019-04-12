using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using AggregatedAttribute = DevExpress.Xpo.AggregatedAttribute;
using System.Diagnostics;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions, XafDefaultProperty("PurchaseOrderNumber")]
    public class PurchaseOrder : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PurchaseOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            PurchaseOrderDate = DateTime.Now;
            DeliveryDate = DateTime.Now.AddDays(3);
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField()]
        public Company Company
        {
            get => company;
            set { SetPropertyValue(nameof(Company), ref company, value);
                if (!IsSaving && !IsLoading)
                {
                    PurchaseOrderNumber = "PO-" + DateTime.Now.Year.ToString() + "-"+ Company.NextPONumber;
                    Company.NextPONumber += 1;
                }
            }
        }


        PaymentTerm paymentTerm;
        bool vATApplies;
        WarehouseLocation deliveryLocation;
        [Persistent(nameof(VAT))]
        decimal vAT;
        [Persistent(nameof(SubTotal))]
        decimal subTotal;
        [Persistent(nameof(Total))]
        decimal total;
        string approvedBy;
        string createdBy;
        StatusEnum status;
        string terms;
        DateTime deliveryDate;
        DateTime purchaseOrderDate;
        string deliveryAddress;
        Vendor vendor;
        Company company;
        string purchaseOrderNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PurchaseOrderNumber
        {
            get => purchaseOrderNumber;
            set => SetPropertyValue(nameof(PurchaseOrderNumber), ref purchaseOrderNumber, value);
        }

        [RuleRequiredField()]
        public Vendor Vendor
        {
            get => vendor;
            set
            {
                SetPropertyValue(nameof(Vendor), ref vendor, value);


                if (!IsSaving && !IsLoading)
                {
                    VATApplies = Vendor.VATVendor;
                    var thisPL = new XPQuery<VendorPriceList>(Session).Where(
                            x => x.Vendor == Vendor && x.FromDate < DateTime.Now
                            && x.ToDate > DateTime.Now
                            );

                    PaymentTerm = Vendor.DefaultPaymentTerm;

                    if (thisPL.Count() != 0)
                    {
                        foreach (PurchaseOrderLine item in PurchaseOrderLines)
                        {
                            var thisProductLine = thisPL.First().VendorPricelistLines.Where(
                           x => x.Product == item.Product);

                            if (thisProductLine.Count() != 0)
                            {
                                item.Price = thisProductLine.First().Price;
                            }
                        }

                    }
                }
            }
        }


        public DateTime PurchaseOrderDate
        {
            get => purchaseOrderDate;
            set => SetPropertyValue(nameof(PurchaseOrderDate), ref purchaseOrderDate, value);
        }

        [RuleRequiredField()]
        public DateTime DeliveryDate
        {
            get => deliveryDate;
            set => SetPropertyValue(nameof(DeliveryDate), ref deliveryDate, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DeliveryAddress
        {
            get => deliveryAddress;
            set => SetPropertyValue(nameof(DeliveryAddress), ref deliveryAddress, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation DeliveryLocation
        {
            get => deliveryLocation;
            set => SetPropertyValue(nameof(DeliveryLocation), ref deliveryLocation, value);
        }

        
        public PaymentTerm PaymentTerm
        {
            get => paymentTerm;
            set => SetPropertyValue(nameof(PaymentTerm), ref paymentTerm, value);
        }


        //[Size(SizeAttribute.DefaultStringMappingFieldSize)]
        //public string Terms
        //{
        //    get => terms;
        //    set => SetPropertyValue(nameof(Terms), ref terms, value);
        //}

        [Association("PurchaseOrder-PurchaseOrderLines"), Aggregated()]
        public XPCollection<PurchaseOrderLine> PurchaseOrderLines
        {
            get
            {
                return GetCollection<PurchaseOrderLine>(nameof(PurchaseOrderLines));
            }
        }

        public enum StatusEnum
        {
            New,
            Approved,
            Declined
        }


        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CreatedBy
        {
            get => createdBy;
            set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ApprovedBy
        {
            get => approvedBy;
            set => SetPropertyValue(nameof(ApprovedBy), ref approvedBy, value);
        }


        [PersistentAlias(nameof(total))]
        public decimal Total
        {
            get
            {
                total = PurchaseOrderLines.Sum(x => x.LineTotal);
                return total;
            }
        }


        [PersistentAlias(nameof(subTotal))]
        public decimal SubTotal
        {
            get
            {
                if (VATApplies == false)
                {
                    subTotal = 0;
                } else
                {
                    subTotal = Total / Convert.ToDecimal(1.12);
                    subTotal = Math.Round(subTotal, 2, MidpointRounding.AwayFromZero);
                }
                
                return subTotal;
            }
        }

        
        public bool VATApplies
        {
            get => vATApplies;
            set => SetPropertyValue(nameof(VATApplies), ref vATApplies, value);
        }


        [PersistentAlias(nameof(vAT))]
        public decimal VAT
        {
            get {
                vAT = Total - SubTotal;
                return vAT; }
        }

        [Association("PurchaseOrder-Receivings")]
        public XPCollection<Receiving> Receivings
        {
            get
            {
                return GetCollection<Receiving>(nameof(Receivings));
            }
        }

        [Association("PurchaseOrder-ReceivingReturns")]
        public XPCollection<ReceivingReturn> ReceivingReturns
        {
            get
            {
                return GetCollection<ReceivingReturn>(nameof(ReceivingReturns));
            }
        }



    }

    public class PurchaseOrderLine : BaseObject
    {
        public PurchaseOrderLine(Session session) : base(session)
        { }


        double discount;
        [Persistent(nameof(ReceivedQuantity))]
        double receivedQuantity;
        [Persistent(nameof(LineTotal))]
        decimal lineTotal;
        decimal price;
        PurchaseOrder purchaseOrder;
        UnitOfMeasure stockingUOM;
        double stockingQuantity;
        UnitOfMeasure purchaseUOM;
        double quantity;
        Product product;

        [Association("PurchaseOrder-PurchaseOrderLines")]
        public PurchaseOrder PurchaseOrder
        {
            get => purchaseOrder;
            set => SetPropertyValue(nameof(PurchaseOrder), ref purchaseOrder, value);
        }

        public Product Product
        {
            get => product;
            set
            {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving)
                {
                    if (PurchaseOrder?.Vendor != null)
                    {
                        var thisPL = new XPQuery<VendorPriceList>(Session).Where(
                            x => x.Vendor == PurchaseOrder.Vendor && x.FromDate < DateTime.Now
                            && x.ToDate > DateTime.Now
                            );
                        Debug.WriteLine(thisPL.Count());
                        if (thisPL.Count() != 0)
                        {
                            var thisProductLine = thisPL.First().VendorPricelistLines.Where(
                                x => x.Product == Product);

                            if (thisProductLine.Count() != 0)
                            {
                                Price = thisProductLine.First().Price;
                            }
                        }
                    }
                    PurchaseUOM = Product.PurchaseUOM;
                    StockingUOM = Product.UOM;
                }
            }
        }


        public double Quantity
        {
            get => quantity;
            set
            {
                SetPropertyValue(nameof(Quantity), ref quantity, value);
                if (!IsLoading && !IsSaving)
                {
                    if (StockingUOM == PurchaseUOM.ReferenceMeasure)
                    {
                        StockingQuantity = Quantity * PurchaseUOM.Ratio;
                    } else if (StockingUOM == PurchaseUOM)
                    {
                        StockingQuantity = Quantity;
                    }
                }
            }
        }


        public decimal Price
        {
            get => price;
            set => SetPropertyValue(nameof(Price), ref price, value);
        }

        
        public double Discount
        {
            get => discount;
            set => SetPropertyValue(nameof(Discount), ref discount, value);
        }


        [PersistentAlias(nameof(lineTotal))]
        public decimal LineTotal
        {
            get
            {
                lineTotal = Convert.ToDecimal(Quantity) * (Price - (Price *  Convert.ToDecimal(Discount / 100)));
                return lineTotal;
            }
        }


        public UnitOfMeasure PurchaseUOM
        {
            get => purchaseUOM;
            set => SetPropertyValue(nameof(PurchaseUOM), ref purchaseUOM, value);
        }

        
        [PersistentAlias(nameof(receivedQuantity))]
        public double ReceivedQuantity
        {
            get {
                receivedQuantity = (from c in PurchaseOrder.Receivings
                                    where c.Status == Receiving.StatusEnum.Validated
                                    from a in c.ReceivedLines
                                    where a.Product == Product
                                    select a.PurchaseQuantityReceived).Sum();

                var retQ = (from c in PurchaseOrder.ReceivingReturns
                            where c.Status == ReceivingReturn.StatusEnum.Validated
                            from a in c.ReceivingReturnLines
                            where a.Product == Product
                            select a.Quantity).Sum();

                return receivedQuantity - retQ; }
        }


        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.GreaterThan, "0")]
        public double StockingQuantity
        {
            get => stockingQuantity;
            set => SetPropertyValue(nameof(StockingQuantity), ref stockingQuantity, value);
        }


        public UnitOfMeasure StockingUOM
        {
            get => stockingUOM;
            set => SetPropertyValue(nameof(StockingUOM), ref stockingUOM, value);
        }
    }
}