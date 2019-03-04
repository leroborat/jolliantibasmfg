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

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField(), RuleUniqueValue()]
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
                XPCollection<StockTransfer> collection = new XPCollection<StockTransfer>(Session);
                //Get In
                var a = from st in collection
                        where (st.DestinationLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                        st.Product == Product && st.Lot == this)
                        select st;

                double StockIn = 0;

                foreach (StockTransfer item in a)
                {
                    StockIn += item.Quantity;
                }

                //Get Out

                var b = from st in collection
                        where (st.SourceLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal &&
                        st.Product == Product && st.Lot == this)
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