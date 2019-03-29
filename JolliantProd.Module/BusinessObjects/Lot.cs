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
    [DefaultClassOptions, XafDefaultProperty("LotCode")]
    public class Lot : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Lot(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }


        TripLine tripLine;
        [Persistent(nameof(StockOnHand))]
        double stockOnHand;
        DateTime expirationDate;
        string internalReference;
        Product product;
        string lotCode;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string LotCode
        {
            get => lotCode;
            set => SetPropertyValue(nameof(LotCode), ref lotCode, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InternalReference
        {
            get => internalReference;
            set => SetPropertyValue(nameof(InternalReference), ref internalReference, value);
        }


        public DateTime ExpirationDate
        {
            get => expirationDate;
            set => SetPropertyValue(nameof(ExpirationDate), ref expirationDate, value);
        }



        [PersistentAlias(nameof(stockOnHand))]
        public double StockOnHand
        {
            get
            {
                var StockIn = new XPQuery<StockTransfer>(Session)
               .Where(x => x.DestinationLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                        x.Product == Product && x.Lot == this).Select(x => x.Quantity).Sum();

                var StockOut = new XPQuery<StockTransfer>(Session)
               .Where(x => x.SourceLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                        x.Product == Product && x.Lot == this).Select(x => x.Quantity).Sum();
                //Get Out

                

                //Get Actual
                stockOnHand = StockIn - StockOut;
                return stockOnHand;
            }
        }

        public void UpdateStockOnHand(bool forceChangeEvents)
        {
            if (forceChangeEvents)
            {
                OnChanged("StockOnHand");
            }
        }


        [Association("TripLine-Lots")]
        public TripLine TripLine
        {
            get => tripLine;
            set => SetPropertyValue(nameof(TripLine), ref tripLine, value);
        }
    }
}