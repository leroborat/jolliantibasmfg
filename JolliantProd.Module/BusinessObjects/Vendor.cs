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
    public class Vendor : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Vendor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string contactPerson;
        string internalNotes;
        string website;
        string email;
        string mobileNumber;
        string phoneNumber;
        string address;
        string vendorName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string VendorName
        {
            get => vendorName;
            set => SetPropertyValue(nameof(VendorName), ref vendorName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ContactPerson
        {
            get => contactPerson;
            set => SetPropertyValue(nameof(ContactPerson), ref contactPerson, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string MobileNumber
        {
            get => mobileNumber;
            set => SetPropertyValue(nameof(MobileNumber), ref mobileNumber, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Website
        {
            get => website;
            set => SetPropertyValue(nameof(Website), ref website, value);
        }

        
        [Size(SizeAttribute.Unlimited)]
        public string InternalNotes
        {
            get => internalNotes;
            set => SetPropertyValue(nameof(InternalNotes), ref internalNotes, value);
        }

        [Association("Vendor-VendorPriceLists"), Aggregated()]
        public XPCollection<VendorPriceList> VendorPriceList
        {
            get
            {
                return GetCollection<VendorPriceList>(nameof(VendorPriceList));
            }
        }
    }

    public class VendorPriceList : BaseObject
    {
        public VendorPriceList(Session session) : base(session)
        { }


        DateTime toDate;
        DateTime fromDate;
        Vendor vendor;

        [Association("Vendor-VendorPriceLists")]
        public Vendor Vendor
        {
            get => vendor;
            set => SetPropertyValue(nameof(Vendor), ref vendor, value);
        }

        [RuleRequiredField()]
        public DateTime FromDate
        {
            get => fromDate;
            set => SetPropertyValue(nameof(FromDate), ref fromDate, value);
        }

        [RuleRequiredField()]
        public DateTime ToDate
        {
            get => toDate;
            set => SetPropertyValue(nameof(ToDate), ref toDate, value);
        }

        [Association("VendorPriceList-VendorPricelistLines"), Aggregated()]
        public XPCollection<VendorPricelistLine> VendorPricelistLines
        {
            get
            {
                return GetCollection<VendorPricelistLine>(nameof(VendorPricelistLines));
            }
        }
    }

    public class VendorPricelistLine : XPObject
    {
        public VendorPricelistLine(Session session) : base(session)
        { }


        decimal price;
        Product product;
        VendorPriceList vendorPriceList;

        [Association("VendorPriceList-VendorPricelistLines")]
        public VendorPriceList VendorPriceList
        {
            get => vendorPriceList;
            set => SetPropertyValue(nameof(VendorPriceList), ref vendorPriceList, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        
        public decimal Price
        {
            get => price;
            set => SetPropertyValue(nameof(Price), ref price, value);
        }

    }
}