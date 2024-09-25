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
    [DefaultClassOptions]
    public class CustomerPayment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CustomerPayment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            var list = new XPCollection<CustomerPayment>(Session);
            ReferenceName = "PAYMENT/IN/" + Convert.ToString(list.Count + 1);
            PaymentDate = DateTime.Now;
        }


        StatusEnum status;
        string checkBank;
        DateTime checkDate;
        string checkNumber;
        [Persistent(nameof(TotalAllocated))]
        decimal totalAllocated;
        Company company;
        DateTime paymentDate;
        string oRNumber;
        decimal paymentAmount;
        string referenceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField(), RuleUniqueValue()]
        public string ReferenceName
        {
            get => referenceName;
            set => SetPropertyValue(nameof(ReferenceName), ref referenceName, value);
        }

        [RuleRequiredField()]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [RuleRequiredField()]
        public decimal PaymentAmount
        {
            get => paymentAmount;
            set => SetPropertyValue(nameof(PaymentAmount), ref paymentAmount, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CheckNumber
        {
            get => checkNumber;
            set => SetPropertyValue(nameof(CheckNumber), ref checkNumber, value);
        }


        public DateTime CheckDate
        {
            get => checkDate;
            set => SetPropertyValue(nameof(CheckDate), ref checkDate, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CheckBank
        {
            get => checkBank;
            set => SetPropertyValue(nameof(CheckBank), ref checkBank, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ORNumber
        {
            get => oRNumber;
            set => SetPropertyValue(nameof(ORNumber), ref oRNumber, value);
        }

        [RuleRequiredField()]
        public DateTime PaymentDate
        {
            get => paymentDate;
            set => SetPropertyValue(nameof(PaymentDate), ref paymentDate, value);
        }

        public enum StatusEnum
        {
            Draft,
            Validated
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [PersistentAlias(nameof(totalAllocated)),
            RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.LessThanOrEqual,
            "PaymentAmount", ParametersMode.Expression)]
        public decimal TotalAllocated
        {
            get {
                totalAllocated = PaymentAllocationLines.Sum(ds => ds.AllocatedAmount);
                return totalAllocated; }
        }

        [Association("CustomerPayment-PaymentAllocationLines"), Aggregated()]
        public XPCollection<PaymentAllocationLine> PaymentAllocationLines
        {
            get
            {
                return GetCollection<PaymentAllocationLine>(nameof(PaymentAllocationLines));
            }
        }

    }

    //[RuleCombinationOfPropertiesIsUnique("UniqueInvoiceinPA", DefaultContexts.Save, "Invoice, CustomerPayment")]
    public class PaymentAllocationLine : BaseObject
    {
        public PaymentAllocationLine(Session session) : base(session)
        { }

        
        [Association("CustomerPayment-PaymentAllocationLines")]
        public CustomerPayment CustomerPayment
        {
            get => customerPayment;
            set => SetPropertyValue(nameof(CustomerPayment), ref customerPayment, value);
        }


        CustomerPayment customerPayment;
        [Persistent(nameof(InvoiceBalance))]
        decimal invoiceBalance;
        decimal allocatedAmount;
        [Persistent(nameof(AmountDue))]
        decimal amountDue;
        Invoice invoice;

        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }


        [PersistentAlias(nameof(amountDue))]
        public decimal AmountDue
        {
            get
            {
                amountDue = Invoice.TotalAmountDue;
                return amountDue;
            }
        }

        [RuleRequiredField(),
            RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.LessThanOrEqual,
            "AmountDue", ParametersMode.Expression)]
        public decimal AllocatedAmount
        {
            get => allocatedAmount;
            set => SetPropertyValue(nameof(AllocatedAmount), ref allocatedAmount, value);
        }

        
        [PersistentAlias(nameof(invoiceBalance))]
        public decimal InvoiceBalance
        {
            get {
                invoiceBalance = AmountDue - AllocatedAmount;
                return invoiceBalance; }
        }
        



    }
}