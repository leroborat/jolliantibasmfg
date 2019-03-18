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
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using AggregatedAttribute = DevExpress.Xpo.AggregatedAttribute;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions, XafDefaultProperty("PurchaseOrderNumber")]
    public class SalesOrder : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SalesOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CurrentUser = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            Status = StatusEnum.Draft;
        }


        [Persistent(nameof(TotalInvoicedAmount))]
        decimal totalInvoicedAmount;
        [Persistent(nameof(TotalSalesNetOfUnserved))]
        decimal totalSalesNetOfUnserved;
        [Persistent(nameof(TotalUnservedQuantity))]
        double totalUnservedQuantity;
        [Persistent(nameof(TotalUnservedAmount))]
        decimal totalUnservedAmount;
        [Persistent(nameof(TotalQuantitiesInvoiced))]
        double totalQuantitiesInvoiced;
        CustomerDeliveryAddress deliveryAddress;
        Customer customer;
        [Persistent(nameof(TotalQuantitiesReturned))]
        double totalQuantitiesReturned;
        Company company;
        OrderTypeEnum type;
        [Persistent(nameof(TotalQuantitiesDelivered))]
        double totalQuantitiesDelivered;
        [Persistent(nameof(TotalQuantity))]
        double totalQuantity;
        [Persistent(nameof(TotalSales))]
        decimal totalSales;
        StatusEnum status;
        DateTime purchaseOrderDate;
        string currentUser;
        Variant supplierCode;
        DateTime scheduledDeliveryDate;
        string purchaseOrderNumber;

        [RuleRequiredField()]
        public Customer Customer
        {
            get => customer;
            set
            {
                SetPropertyValue(nameof(Customer), ref customer, value);
                if (!IsLoading && !IsSaving)
                {
                    //DeliveryAddress = Customer.CustomerAddresses.First();
                }
            }
        }

        [RuleRequiredField()]
        public CustomerDeliveryAddress DeliveryAddress
        {
            get => deliveryAddress;
            set => SetPropertyValue(nameof(DeliveryAddress), ref deliveryAddress, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string PurchaseOrderNumber
        {
            get => purchaseOrderNumber;
            set => SetPropertyValue(nameof(PurchaseOrderNumber), ref purchaseOrderNumber, value);
        }


        [RuleRequiredField()]
        public DateTime PurchaseOrderDate
        {
            get => purchaseOrderDate;
            set => SetPropertyValue(nameof(PurchaseOrderDate), ref purchaseOrderDate, value);
        }

        public DateTime ScheduledDeliveryDate
        {
            get => scheduledDeliveryDate;
            set => SetPropertyValue(nameof(ScheduledDeliveryDate), ref scheduledDeliveryDate, value);
        }

        [RuleRequiredField()]
        public Variant SupplierCode
        {
            get => supplierCode;
            set
            {
                SetPropertyValue(nameof(SupplierCode), ref supplierCode, value);
                if (!IsLoading && !IsSaving)
                {
                    Session.Delete(SalesOrderLines);
                    foreach (VariantLine item in SupplierCode.VariantLines)
                    {
                        SalesOrderLine sl = new SalesOrderLine(Session);
                        sl.Product = item.Product;
                        sl.UnitPrice = item.SalesPrice;
                        sl.Quantity = 1;
                        SalesOrderLines.Add(sl);
                    }
                    DeliveryAddress = SupplierCode.DeliveryAddress;
                    Company = SupplierCode.Company;
                }
            }
        }

        [RuleRequiredField()]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }
        //Removed Aggregated
        [Association("SalesOrder-SalesOrderLines"), Aggregated()]
        public XPCollection<SalesOrderLine> SalesOrderLines
        {
            get
            {
                return GetCollection<SalesOrderLine>(nameof(SalesOrderLines));
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CurrentUser
        {
            get => currentUser;
            set => SetPropertyValue(nameof(CurrentUser), ref currentUser, value);
        }

        public enum StatusEnum
        {
            Draft,
            Confirmed
        }


        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        public enum OrderTypeEnum
        {
            Daily,
            Additional,
            Allocation
        }

        [RuleRequiredField()]
        public OrderTypeEnum Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }


        [PersistentAlias(nameof(totalSales))]
        public decimal TotalSales
        {
            get
            {
                totalSales = 0;
                foreach (SalesOrderLine salesOrderLine in SalesOrderLines)
                {
                    totalSales += salesOrderLine.Amount;
                }
                return totalSales;
            }
        }


        [PersistentAlias(nameof(totalSalesNetOfUnserved))]
        public decimal TotalSalesNetOfUnserved
        {
            get
            {
                totalSalesNetOfUnserved = TotalSales - TotalUnservedAmount;
                return totalSalesNetOfUnserved;
            }
        }



        [PersistentAlias(nameof(totalQuantity))]
        public double TotalQuantity
        {
            get
            {
                totalQuantity = 0;
                foreach (SalesOrderLine salesOrderLine in SalesOrderLines)
                {
                    totalQuantity += salesOrderLine.Quantity;
                }
                return totalQuantity;
            }
        }


        [PersistentAlias(nameof(totalQuantitiesDelivered))]
        public double TotalQuantitiesDelivered
        {
            get
            {
                totalQuantitiesDelivered = (from c in SalesOrderLines
                                            select c.QuantityDelivered).Sum();

                return totalQuantitiesDelivered;
            }
        }


        [PersistentAlias(nameof(totalQuantitiesReturned))]
        public double TotalQuantitiesReturned
        {
            get
            {

                totalQuantitiesReturned = (from c in SalesOrderLines
                                           select c.QuantityReturned).Sum();

                return totalQuantitiesReturned;
            }
        }


        [PersistentAlias(nameof(totalUnservedQuantity))]
        public double TotalUnservedQuantity
        {
            get
            {
                totalUnservedQuantity = SalesOrderLines.Select(x => x.QuantityUnserved).Sum();
                return totalUnservedQuantity;
            }
        }


        [PersistentAlias(nameof(totalUnservedAmount))]
        public decimal TotalUnservedAmount
        {
            get
            {
                totalUnservedAmount = SalesOrderLines.Select(x => x.UnservedAmount).Sum();
                return totalUnservedAmount;
            }
        }



        [PersistentAlias(nameof(totalQuantitiesInvoiced))]
        public double TotalQuantitiesInvoiced
        {
            get
            {

                totalQuantitiesInvoiced = 0;
                foreach (Invoice item in Invoices)
                {

                    foreach (InvoiceLine invoiceLine in item.InvoiceLines)
                    {
                        totalQuantitiesInvoiced += invoiceLine.Quantity;
                    }

                }
                return totalQuantitiesInvoiced;
            }
        }

        
        [PersistentAlias(nameof(totalInvoicedAmount))]
        public decimal TotalInvoicedAmount
        {
            get {
                totalInvoicedAmount = Invoices.Select(x => x.TotalSales).Sum();
                return totalInvoicedAmount; }
        }
        




        //Removed Aggregated
        [Association("SalesOrder-Trips"), Aggregated()]
        public XPCollection<Trip> Trips
        {
            get
            {
                return GetCollection<Trip>(nameof(Trips));
            }
        }

        [Association("SalesOrder-InventoryReturns")]
        public XPCollection<InventoryReturn> InventoryReturns
        {
            get
            {
                return GetCollection<InventoryReturn>(nameof(InventoryReturns));
            }
        }

        //Removed Aggregated
        [Association("SalesOrder-Invoices"), Aggregated()]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }

        //Removed Aggregated
        [Association("SalesOrder-UnservedLines"), Aggregated()]
        public XPCollection<UnservedLine> UnservedLines
        {
            get
            {
                return GetCollection<UnservedLine>(nameof(UnservedLines));
            }
        }



    }
    //[RuleCombinationOfPropertiesIsUnique("UniqueSOL", DefaultContexts.Save, "SalesOrder, Product")]
    public class SalesOrderLine : BaseObject
    {
        public SalesOrderLine(Session session) : base(session)
        { }


        [Persistent(nameof(OutboundAmount))]
        decimal outboundAmount;
        [Persistent(nameof(UnservedAmount))]
        decimal unservedAmount;
        [Persistent(nameof(QuantityUnserved))]
        double quantityUnserved;
        [Persistent(nameof(QuantityInvoiced))]
        double quantityInvoiced;
        [Persistent(nameof(QuantityReturned))]
        double quantityReturned;
        [Persistent(nameof(QuantityDelivered))]
        double quantityDelivered;
        [Persistent(nameof(Amount))]
        decimal amount;
        decimal unitPrice;
        double quantity;
        Product product;
        SalesOrder salesOrder;

        [Association("SalesOrder-SalesOrderLines")]
        public SalesOrder SalesOrder
        {
            get => salesOrder;
            set => SetPropertyValue(nameof(SalesOrder), ref salesOrder, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


        [PersistentAlias(nameof(quantityDelivered))]
        public double QuantityDelivered
        {
            get
            {
                quantityDelivered = 0;
                foreach (Trip item in SalesOrder.Trips)
                {
                    if (item.TripStatus == Trip.TripStatusEnum.Validated)
                    {
                        foreach (TripLine tripLine in item.TripLines)
                        {
                            if (tripLine.Product == Product)
                            {
                                quantityDelivered += tripLine.QuantityDone;
                            }
                        }
                    }
                }
                return quantityDelivered - QuantityReturned;
            }
        }


        [PersistentAlias(nameof(quantityReturned))]
        public double QuantityReturned
        {
            get
            {
                quantityReturned = 0;
                foreach (InventoryReturn item in SalesOrder.InventoryReturns)
                {
                    foreach (InventoryReturnLine inventoryReturnLine in item.InventoryReturnLines)
                    {
                        if (inventoryReturnLine.Product == Product)
                        {
                            quantityReturned += inventoryReturnLine.Quantity;
                        }
                    }
                }


                return quantityReturned;
            }
        }


        [PersistentAlias(nameof(quantityInvoiced))]
        public double QuantityInvoiced
        {
            get
            {
                quantityInvoiced = (from c in SalesOrder.Invoices
                                    from x in c.InvoiceLines
                                    where x.Product == Product
                                    select x.Quantity).Sum();
                return quantityInvoiced;
            }
        }


        [PersistentAlias(nameof(quantityUnserved))]
        public double QuantityUnserved
        {
            get
            {
                quantityUnserved = (from c in SalesOrder.UnservedLines
                                    where c.Product == Product
                                    select c.Quantity).Sum();
                return quantityUnserved;
            }
        }




        [RuleRequiredField()]
        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }


        [PersistentAlias(nameof(amount))]
        public decimal Amount
        {
            get
            {
                amount = UnitPrice * Convert.ToDecimal(Quantity);
                return amount;
            }
        }


        [PersistentAlias(nameof(unservedAmount))]
        public decimal UnservedAmount
        {
            get
            {
                unservedAmount = UnitPrice * Convert.ToDecimal(QuantityUnserved);
                return unservedAmount;
            }
        }

        
        [PersistentAlias(nameof(outboundAmount))]
        public decimal OutboundAmount
        {
            get {
                outboundAmount = Amount - UnservedAmount;
                return outboundAmount; }
        }
        


    }
}