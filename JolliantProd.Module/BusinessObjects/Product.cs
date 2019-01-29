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
    public class Product : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Product(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Tracking = TrackingEnum.TrackByLot;
        }


        TrackingEnum tracking;
        [Persistent(nameof(StockOnHand))]
        double stockOnHand;
        UnitOfMeasure purchaseUOM;
        UnitOfMeasure uOM;
        decimal cost;
        decimal salesPrice;
        string barcode;
        string internalReference;
        SalesCategory salesCategory;
        ProductTypeEnum productType;
        bool canBePurchased;
        bool canBeSold;
        string productName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string ProductName
        {
            get => productName;
            set => SetPropertyValue(nameof(ProductName), ref productName, value);
        }


        public bool CanBeSold
        {
            get => canBeSold;
            set => SetPropertyValue(nameof(CanBeSold), ref canBeSold, value);
        }


        public bool CanBePurchased
        {
            get => canBePurchased;
            set => SetPropertyValue(nameof(CanBePurchased), ref canBePurchased, value);
        }

        public enum ProductTypeEnum
        {
            Storable,
            Consumable,
            Service
        }


        public ProductTypeEnum ProductType
        {
            get => productType;
            set => SetPropertyValue(nameof(ProductType), ref productType, value);
        }

        [RuleRequiredField()]
        public SalesCategory SalesCategory
        {
            get => salesCategory;
            set => SetPropertyValue(nameof(SalesCategory), ref salesCategory, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InternalReference
        {
            get => internalReference;
            set => SetPropertyValue(nameof(InternalReference), ref internalReference, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Barcode
        {
            get => barcode;
            set => SetPropertyValue(nameof(Barcode), ref barcode, value);
        }


        public decimal SalesPrice
        {
            get => salesPrice;
            set => SetPropertyValue(nameof(SalesPrice), ref salesPrice, value);
        }


        public decimal Cost
        {
            get => cost;
            set => SetPropertyValue(nameof(Cost), ref cost, value);
        }

        [RuleRequiredField()]
        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
        }

        [RuleRequiredField()]
        public UnitOfMeasure PurchaseUOM
        {
            get => purchaseUOM;
            set => SetPropertyValue(nameof(PurchaseUOM), ref purchaseUOM, value);
        }


        [PersistentAlias(nameof(stockOnHand))]
        public double StockOnHand
        {
            get
            {
                XPCollection<StockTransfer> collection = new XPCollection<StockTransfer>(Session);
                //Get In
                var a = from st in collection
                        where (st.DestinationLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                        st.Product == this)
                        select st;

                double StockIn = 0;

                foreach (StockTransfer item in a)
                {
                    StockIn += item.Quantity;
                }

                //Get Out

                var b = from st in collection
                        where (st.SourceLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                        st.Product == this)
                        select st;

                double StockOut = 0;

                foreach (StockTransfer item in b)
                {
                    StockOut += item.Quantity;
                }

                //Get Actual
                stockOnHand = StockIn - StockOut;
                return stockOnHand;
            }
        }

        public enum TrackingEnum
        {
            NoTracking,
            TrackByLot,
            TrackBySerial
        }

        [RuleRequiredField()]
        public TrackingEnum Tracking
        {
            get => tracking;
            set => SetPropertyValue(nameof(Tracking), ref tracking, value);
        }


    }
}