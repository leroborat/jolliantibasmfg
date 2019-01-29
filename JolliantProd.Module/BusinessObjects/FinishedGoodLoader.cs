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
    public class FinishedGoodLoader : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FinishedGoodLoader(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string referenceName;
        WarehouseLocation to;
        WarehouseLocation from;
        Product product;
        DateTime lotExpirationDate;

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize),RuleRequiredField()]
        public string ReferenceName
        {
            get => referenceName;
            set => SetPropertyValue(nameof(ReferenceName), ref referenceName, value);
        }

        [RuleRequiredField()]
        public DateTime LotExpirationDate
        {
            get => lotExpirationDate;
            set => SetPropertyValue(nameof(LotExpirationDate), ref lotExpirationDate, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation From
        {
            get => from;
            set => SetPropertyValue(nameof(From), ref from, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation To
        {
            get => to;
            set => SetPropertyValue(nameof(To), ref to, value);
        }

    }

}