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
    public class StockTransfer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public StockTransfer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
        }


        UnitOfMeasure uOM;
        DateTime date;
        Lot lot;
        double quantity;
        Product product;
        WarehouseLocation destinationLocation;
        WarehouseLocation sourceLocation;
        string reference;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string Reference
        {
            get => reference;
            set => SetPropertyValue(nameof(Reference), ref reference, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation SourceLocation
        {
            get => sourceLocation;
            set => SetPropertyValue(nameof(SourceLocation), ref sourceLocation, value);
        }


        [RuleRequiredField()]
        public WarehouseLocation DestinationLocation
        {
            get => destinationLocation;
            set => SetPropertyValue(nameof(DestinationLocation), ref destinationLocation, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsSaving && !IsDeleted && !IsLoading)
                {
                    UOM = Product.UOM;
                }
            }
        }


        [RuleRequiredField()]
        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

        
        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
        }

        [RuleRequiredField()]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        public Lot Lot
        {
            get => lot;
            set {

                SetPropertyValue(nameof(Lot), ref lot, value);
            }
        }


    }
}