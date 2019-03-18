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
                    var thisPL = new XPCollection<VendorPriceList>(Session).Where(
                            x => x.Vendor == Vendor && x.FromDate < DateTime.Now
                            && x.ToDate > DateTime.Now
                            );


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


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Terms
        {
            get => terms;
            set => SetPropertyValue(nameof(Terms), ref terms, value);
        }

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
                subTotal = Total / Convert.ToDecimal(1.12);
                subTotal = Math.Round(subTotal, 2, MidpointRounding.AwayFromZero);
                return subTotal;
            }
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




    }

    public class PurchaseOrderLine : BaseObject
    {
        public PurchaseOrderLine(Session session) : base(session)
        { }


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
                        var thisPL = new XPCollection<VendorPriceList>(Session).Where(
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
                    }
                }
            }
        }


        public decimal Price
        {
            get => price;
            set => SetPropertyValue(nameof(Price), ref price, value);
        }

        
        [PersistentAlias(nameof(lineTotal))]
        public decimal LineTotal
        {
            get {
                lineTotal = Convert.ToDecimal(Quantity) * Price;
                return lineTotal; }
        }
        

        public UnitOfMeasure PurchaseUOM
        {
            get => purchaseUOM;
            set => SetPropertyValue(nameof(PurchaseUOM), ref purchaseUOM, value);
        }


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