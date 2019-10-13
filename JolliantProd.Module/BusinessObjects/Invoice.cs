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

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions, XafDefaultProperty("InvoiceNumber")]
    public class Invoice : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Invoice(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            InvoiceDate = DateTime.Now;
            Status = StatusEnum.Draft;
        }

        SupplierCode suppCode;
        string trip;
        [Persistent(nameof(OpenAmount))]
        decimal openAmount;
        Company company;
        [Persistent(nameof(VAT))]
        decimal vAT;
        [Persistent(nameof(DueLessVat))]
        decimal dueLessVat;
        [Persistent(nameof(TotalAmountDue))]
        decimal totalAmountDue;
        [Persistent(nameof(DistributionAllowance))]
        decimal distributionAllowance;
        double distributionAllowancePercentage;
        [Persistent(nameof(TotalSales))]
        decimal totalSales;
        [Persistent(nameof(PurchaseOrderNumber))]
        string purchaseOrderNumber;
        SalesOrder salesOrder;
        StatusEnum status;
        DateTime dueDate;
        PaymentTerm paymentTerm;
        DateTime invoiceDate;
        string businessStyle;
        string tIN;
        string customerBillingAddress;
        Customer customer;
        string invoiceNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string InvoiceNumber
        {
            get => invoiceNumber;
            set => SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value);
        }


        [Association("Customer-Invoices"), RuleRequiredField()]
        public Customer Customer
        {
            get => customer;
            set
            {
                SetPropertyValue(nameof(Customer), ref customer, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    CustomerBillingAddress = Customer.BillingAddress;
                    TIN = Customer.TinNumber;
                    BusinessStyle = Customer.BusinessStyle;
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CustomerBillingAddress
        {
            get => customerBillingAddress;
            set => SetPropertyValue(nameof(CustomerBillingAddress), ref customerBillingAddress, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TIN
        {
            get => tIN;
            set => SetPropertyValue(nameof(TIN), ref tIN, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BusinessStyle
        {
            get => businessStyle;
            set => SetPropertyValue(nameof(BusinessStyle), ref businessStyle, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Trip
        {
            get => trip;
            set => SetPropertyValue(nameof(Trip), ref trip, value);
        }

        
        public SupplierCode SuppCode
        {
            get => suppCode;
            set => SetPropertyValue(nameof(SuppCode), ref suppCode, value);
        }


        public DateTime InvoiceDate
        {
            get => invoiceDate;
            set => SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value);
        }


        public PaymentTerm PaymentTerm
        {
            get => paymentTerm;
            set
            {
                SetPropertyValue(nameof(PaymentTerm), ref paymentTerm, value);
                if (!IsLoading && !IsSaving)
                {
                    DueDate = InvoiceDate.AddDays(PaymentTerm.Days);
                }
            }
        }


        public DateTime DueDate
        {
            get => dueDate;
            set => SetPropertyValue(nameof(DueDate), ref dueDate, value);
        }

        public enum StatusEnum
        {
            Draft,
            Open,
            Paid
        }


        public StatusEnum Status
        {
            get {
                return status; }
            set
            {
                SetPropertyValue(nameof(Status), ref status, value);
               
            }
        }

        
        [PersistentAlias(nameof(openAmount))]
        public decimal OpenAmount
        {
            get {
                openAmount = TotalAmountDue - (new XPQuery<PaymentAllocationLine>(Session).
                            Where(rbd => rbd.Invoice == this).Select(rbd => rbd.AllocatedAmount).Sum());
                
                return openAmount; }
        }
        


        [Association("SalesOrder-Invoices")]
        public SalesOrder SalesOrder
        {
            get => salesOrder;
            set
            {
                SetPropertyValue(nameof(SalesOrder), ref salesOrder, value);
                if (!IsSaving && !IsLoading && !IsDeleted)
                {
                    Customer = SalesOrder.Customer;
                    Company = SalesOrder.Company;
                    if (InvoiceLines.Count <= 0)
                    {
                        foreach (SalesOrderLine item in SalesOrder.SalesOrderLines)
                        {
                            InvoiceLine il = new InvoiceLine(Session);
                            il.Product = item.Product;
                            il.UnitPrice = item.UnitPrice;
                            InvoiceLines.Add(il);
                        }
                    }
                    
                }
            }
        }

        [RuleRequiredField()]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }


        [PersistentAlias(nameof(purchaseOrderNumber))]
        public string PurchaseOrderNumber
        {
            get
            {
                if (SalesOrder != null)
                {
                    purchaseOrderNumber = SalesOrder.PurchaseOrderNumber;
                }
                return purchaseOrderNumber;
            }
        }


        [PersistentAlias(nameof(totalSales))]
        public decimal TotalSales
        {
            get
            {
                totalSales = (from c in InvoiceLines
                              select c.SubTotal).Sum();
                return totalSales;
            }
        }


        public double DistributionAllowancePercentage
        {
            get => distributionAllowancePercentage;
            set => SetPropertyValue(nameof(DistributionAllowancePercentage), ref distributionAllowancePercentage, value);
        }


        [PersistentAlias(nameof(distributionAllowance))]
        public decimal DistributionAllowance
        {
            get
            {
                distributionAllowance = (TotalSales * Convert.ToDecimal((DistributionAllowancePercentage / 100)));
                return distributionAllowance;
            }
        }


        [PersistentAlias(nameof(totalAmountDue))]
        public decimal TotalAmountDue
        {
            get
            {
                totalAmountDue = TotalSales - DistributionAllowance;
                return totalAmountDue;
            }
        }


        [PersistentAlias(nameof(dueLessVat))]
        public decimal DueLessVat
        {
            get
            {
                dueLessVat = TotalAmountDue / Convert.ToDecimal(1.12);
                dueLessVat = Math.Round(dueLessVat, 2, MidpointRounding.AwayFromZero);
                return dueLessVat;
            }
        }

        
        [PersistentAlias(nameof(vAT))]
        public decimal VAT
        {
            get {
                vAT = TotalAmountDue - DueLessVat;
                return vAT; }
        }


        [Association("Invoice-Trips")]
        public XPCollection<Trip> Trips
        {
            get
            {
                return GetCollection<Trip>(nameof(Trips));
            }
        }


        [Association("Invoice-InvoiceLines"), Aggregated()]
        public XPCollection<InvoiceLine> InvoiceLines
        {
            get
            {
                return GetCollection<InvoiceLine>(nameof(InvoiceLines));
            }
        }

       

    }

    public class InvoiceLine : BaseObject
    {
        public InvoiceLine(Session session) : base(session)
        { }

        
        [Association("Invoice-InvoiceLines"), RuleRequiredField()]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        Invoice invoice;
        [Persistent(nameof(SubTotal))]
        decimal subTotal;
        double quantity;
        decimal unitPrice;
        string description;
        Product product;

        public Product Product
        {
            get => product;
            set {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving)
                {
                    Description = Product.ProductName;
                }
            }
        }


        [Size(SizeAttribute.Unlimited), RuleRequiredField()]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [RuleRequiredField()]
        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }

        [RuleRequiredField()]
        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


        
        [PersistentAlias(nameof(subTotal))]
        public decimal SubTotal
        {
            get {
                subTotal = UnitPrice * Convert.ToDecimal(Quantity);
                return subTotal; }
        }
        

    }

    
}