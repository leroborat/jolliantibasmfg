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
    public class UnitOfMeasure : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UnitOfMeasure(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        UnitOfMeasure referenceMeasure;
        double ratio;
        UnitOfMeasureCategory unitOfMeasureCategory;
        string uOMName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string UOMName
        {
            get => uOMName;
            set => SetPropertyValue(nameof(UOMName), ref uOMName, value);
        }

        [RuleRequiredField()]
        public UnitOfMeasureCategory UnitOfMeasureCategory
        {
            get => unitOfMeasureCategory;
            set => SetPropertyValue(nameof(UnitOfMeasureCategory), ref unitOfMeasureCategory, value);
        }


        public double Ratio
        {
            get => ratio;
            set => SetPropertyValue(nameof(Ratio), ref ratio, value);
        }

        
        public UnitOfMeasure ReferenceMeasure
        {
            get => referenceMeasure;
            set => SetPropertyValue(nameof(ReferenceMeasure), ref referenceMeasure, value);
        }



    }

    public class UnitOfMeasureCategory : XPObject
    {
        public UnitOfMeasureCategory(Session session) : base(session)
        { }


        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize),RuleRequiredField()]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
    }
}