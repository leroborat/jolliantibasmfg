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
    public class SalesCategory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SalesCategory(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        [Persistent(nameof(DisplayName))]
        string displayName;
        SalesCategory parent;
        string categoryName;



        
        [PersistentAlias(nameof(displayName))]
        public string DisplayName
        {
            get {
                if (Parent != null)
                {
                    displayName = Parent.DisplayName + "/" + CategoryName;
                } else
                {
                    displayName = CategoryName;
                }
                return displayName; }
        }
        

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CategoryName
        {
            get => categoryName;
            set => SetPropertyValue(nameof(CategoryName), ref categoryName, value);
        }

        
        public SalesCategory Parent
        {
            get => parent;
            set => SetPropertyValue(nameof(Parent), ref parent, value);
        }
    }
}