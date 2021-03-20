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

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Customer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Customer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        int odooDatabaseID;
        string odooID;
        string businessStyle;
        string tinNumber;
        string contactNumber;
        string emailAddress;
        string billingAddress;
        string customerName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string CustomerName
        {
            get => customerName;
            set => SetPropertyValue(nameof(CustomerName), ref customerName, value);
        }


        //[Size(SizeAttribute.DefaultStringMappingFieldSize)]
        //public string OdooID
        //{
        //    get => odooID;
        //    set => SetPropertyValue(nameof(OdooID), ref odooID, value);
        //}

        
        public int OdooDatabaseID
        {
            get => odooDatabaseID;
            set => SetPropertyValue(nameof(OdooDatabaseID), ref odooDatabaseID, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TinNumber
        {
            get => tinNumber;
            set => SetPropertyValue(nameof(TinNumber), ref tinNumber, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BusinessStyle
        {
            get => businessStyle;
            set => SetPropertyValue(nameof(BusinessStyle), ref businessStyle, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BillingAddress
        {
            get => billingAddress;
            set => SetPropertyValue(nameof(BillingAddress), ref billingAddress, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ContactNumber
        {
            get => contactNumber;
            set => SetPropertyValue(nameof(ContactNumber), ref contactNumber, value);
        }

        [Association("Customer-CustomerAddresses"), DevExpress.Xpo.Aggregated()]
        public XPCollection<CustomerDeliveryAddress> CustomerAddresses
        {
            get
            {
                return GetCollection<CustomerDeliveryAddress>(nameof(CustomerAddresses));
            }
        }

        [Association("Customer-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }
    }

    [XafDefaultProperty("DeliveryAddress")]
    public class CustomerDeliveryAddress : BaseObject
    {
        public CustomerDeliveryAddress(Session session) : base(session)
        { }



        Customer customer;
        string deliveryAddress;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DeliveryAddress
        {
            get => deliveryAddress;
            set => SetPropertyValue(nameof(DeliveryAddress), ref deliveryAddress, value);
        }

        
        [Association("Customer-CustomerAddresses")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

    }
}