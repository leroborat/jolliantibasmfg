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
    public class UnservedLine : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UnservedLine(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string unservedReason;
        double quantity;
        Product product;
        SalesOrder salesOrder;

        [Association("SalesOrder-UnservedLines")]
        public SalesOrder SalesOrder
        {
            get => salesOrder;
            set => SetPropertyValue(nameof(SalesOrder), ref salesOrder, value);
        }


        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string UnservedReason
        {
            get => unservedReason;
            set => SetPropertyValue(nameof(UnservedReason), ref unservedReason, value);
        }


    }
}