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
            IsActive = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        int odooID;
        string vendorPrefix;
        bool isActive;
        StatusEnum status;
        PaymentTerm defaultPaymentTerm;
        string paymentTerms;
        string tINNumber;
        bool vATVendor;
        int nextIn;
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


        [Size(SizeAttribute.DefaultStringMappingFieldSize), VisibleInListView(false), RuleRequiredField()]
        public string VendorPrefix
        {
            get => vendorPrefix;
            set => SetPropertyValue(nameof(VendorPrefix), ref vendorPrefix, value);
        }


        public bool VATVendor
        {
            get => vATVendor;
            set => SetPropertyValue(nameof(VATVendor), ref vATVendor, value);
        }


        public bool IsActive
        {
            get => isActive;
            set => SetPropertyValue(nameof(IsActive), ref isActive, value);
        }


        public PaymentTerm DefaultPaymentTerm
        {
            get => defaultPaymentTerm;
            set => SetPropertyValue(nameof(DefaultPaymentTerm), ref defaultPaymentTerm, value);
        }

        public enum StatusEnum
        {
            ForApproval,
            Approved,
            Disapproved
        }


        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        //[Size(SizeAttribute.DefaultStringMappingFieldSize)]
        //public string PaymentTerms
        //{
        //    get => paymentTerms;
        //    set => SetPropertyValue(nameof(PaymentTerms), ref paymentTerms, value);
        //}


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TINNumber
        {
            get => tINNumber;
            set => SetPropertyValue(nameof(TINNumber), ref tINNumber, value);
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


        public int NextIn
        {
            get => nextIn;
            set => SetPropertyValue(nameof(NextIn), ref nextIn, value);
        }



        [Association("Vendor-VendorPriceLists"), Aggregated()]
        public XPCollection<VendorPriceList> VendorPriceList
        {
            get
            {
                return GetCollection<VendorPriceList>(nameof(VendorPriceList));
            }
        }

        [Action(Caption = "Set As Approved", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void ApprovedVendor()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Approved;
        }

        [Action(Caption = "Set As Dispproved", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void DisApprovedVendor()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Disapproved;
        }

        private XPCollection<AuditDataItemPersistent> auditTrail;
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }

        [VisibleInDetailView(false), VisibleInListView(false)]
        public int OdooID
        {
            get => odooID;
            set => SetPropertyValue(nameof(OdooID), ref odooID, value);
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