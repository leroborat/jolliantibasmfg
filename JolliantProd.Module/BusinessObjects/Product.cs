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


        double uOMRatioProduction;
        UnitOfMeasure productionUOM;
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

        
        public bool CanBeManufactured
        {
            get => canBeManufactured;
            set => SetPropertyValue(nameof(CanBeManufactured), ref canBeManufactured, value);
        }
        bool canBeManufactured;
        

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


        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
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

        [RuleRequiredField()]
        public UnitOfMeasure ProductionUOM
        {
            get => productionUOM;
            set => SetPropertyValue(nameof(ProductionUOM), ref productionUOM, value);
        }

        
        public double UOMRatioProduction
        {
            get => uOMRatioProduction;
            set => SetPropertyValue(nameof(UOMRatioProduction), ref uOMRatioProduction, value);
        }



        [PersistentAlias(nameof(stockOnHand))]
        public double StockOnHand
        {
            get
            {

                

                var StockIn = new XPQuery<StockTransfer>(Session)
                .Where(
                x => x.DestinationLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                x.Product == this
                ).Select(x => x.Quantity).Sum();

                var StockOut = new XPQuery<StockTransfer>(Session).Where(
                    x => x.SourceLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                    x.Product == this
                    ).Select(x => x.Quantity).Sum();

                stockOnHand = StockIn - StockOut;
                return stockOnHand;
            }
        }

        
        
        public double ReorderingLevel
        {
            get => reorderingLevel;
            set => SetPropertyValue(nameof(ReorderingLevel), ref reorderingLevel, value);
        }
        double reorderingLevel;
        
        

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

        [Association("Product-BillOfMaterials"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BillOfMaterial> BillOfMaterials
        {
            get
            {
                return GetCollection<BillOfMaterial>(nameof(BillOfMaterials));
            }
        }


    }
}