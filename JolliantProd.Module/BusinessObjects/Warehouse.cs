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
    public class Warehouse : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Warehouse(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            WarehouseLocation warehouseLocation = new WarehouseLocation(Session);
            warehouseLocation.LocationName = "Stock";
            warehouseLocation.Warehouse = this;
            WarehouseLocations.Add(warehouseLocation);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string warehouseName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string WarehouseName
        {
            get => warehouseName;
            set => SetPropertyValue(nameof(WarehouseName), ref warehouseName, value);
        }

        [Association("Warehouse-WarehouseLocations")]
        public XPCollection<WarehouseLocation> WarehouseLocations
        {
            get
            {
                return GetCollection<WarehouseLocation>(nameof(WarehouseLocations));
            }
        }
    }

    [DefaultClassOptions]
    public class WarehouseLocation : BaseObject
    {
        public WarehouseLocation(Session session) : base(session)
        { }


        int nextWithdrawal;
        int nextIn;
        LocationTypeEnum locationType;
        Warehouse warehouse;
        [Persistent(nameof(DisplayName))]
        string displayName;
        WarehouseLocation parent;
        string locationName;



        [Association("Warehouse-WarehouseLocations")]
        public Warehouse Warehouse
        {
            get => warehouse;
            set => SetPropertyValue(nameof(Warehouse), ref warehouse, value);
        }
        [PersistentAlias(nameof(displayName))]
        public string DisplayName
        {
            get
            {

                if (LocationType == LocationTypeEnum.Internal)
                {
                    if (Parent != null && Warehouse != null)
                    {
                        displayName = Parent.DisplayName + "/" + LocationName;
                    }
                    else if (Parent == null && Warehouse != null)
                    {
                        displayName = Warehouse.WarehouseName + "/" + LocationName;
                    }
                }
                else if (LocationType == LocationTypeEnum.CustomerLocation || LocationType == LocationTypeEnum.VendorLocation)
                {
                    if (Parent != null)
                    {
                        displayName = Parent.DisplayName + "/" + LocationName;
                    }
                    else
                    {
                        displayName = "Partner Locations" + "/" + LocationName;
                    }
                }
                else
                {
                    if (Parent != null)
                    {
                        displayName = Parent.DisplayName + "/" + LocationName;
                    }
                    else
                    {
                        displayName = "Virtual" + "/" + LocationName;
                    }
                }

                return displayName;
            }
        }


        public int NextIn
        {
            get => nextIn;
            set => SetPropertyValue(nameof(NextIn), ref nextIn, value);
        }

        
        public int NextWithdrawal
        {
            get => nextWithdrawal;
            set => SetPropertyValue(nameof(NextWithdrawal), ref nextWithdrawal, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string LocationName
        {
            get => locationName;
            set => SetPropertyValue(nameof(LocationName), ref locationName, value);
        }


        public WarehouseLocation Parent
        {
            get => parent;
            set => SetPropertyValue(nameof(Parent), ref parent, value);
        }

        public enum LocationTypeEnum
        {
            Internal,
            CustomerLocation,
            VendorLocation,
            InventoryLoss,
            Production
        }

        [RuleRequiredField()]
        public LocationTypeEnum LocationType
        {
            get => locationType;
            set => SetPropertyValue(nameof(LocationType), ref locationType, value);
        }

    }
}