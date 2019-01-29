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
    public class Variant : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Variant(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        Company company;
        [Persistent(nameof(DisplayName))]
        string displayName;
        SupplierCode suppCode;
        string area;


        [PersistentAlias(nameof(displayName))]
        public string DisplayName
        {
            get
            {
                displayName = SuppCode + " - " + Area;
                return displayName;
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string Area
        {
            get => area;
            set => SetPropertyValue(nameof(Area), ref area, value);
        }

        [RuleRequiredField()]
        public SupplierCode SuppCode
        {
            get => suppCode;
            set => SetPropertyValue(nameof(SuppCode), ref suppCode, value);
        }

        [RuleRequiredField()]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }

        [Association("Variant-VariantLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<VariantLine> VariantLines
        {
            get
            {
                return GetCollection<VariantLine>(nameof(VariantLines));
            }
        }
    }

    public class VariantLine : BaseObject
    {
        public VariantLine(Session session) : base(session)
        { }

        Variant variant;
        decimal salesPrice;
        Product product;

        
        [Association("Variant-VariantLines")]
        public Variant Variant
        {
            get => variant;
            set => SetPropertyValue(nameof(Variant), ref variant, value);
        }

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        public decimal SalesPrice
        {
            get => salesPrice;
            set => SetPropertyValue(nameof(SalesPrice), ref salesPrice, value);
        }
    }
}