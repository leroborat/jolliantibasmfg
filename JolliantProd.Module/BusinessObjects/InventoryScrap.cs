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
using AggregatedAttribute = DevExpress.Xpo.AggregatedAttribute;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class InventoryScrap : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventoryScrap(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = StatusEnum.Draft;
        }


        StatusEnum status;
        WarehouseLocation inventoryLocation;
        string referenceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string ReferenceName
        {
            get => referenceName;
            set => SetPropertyValue(nameof(ReferenceName), ref referenceName, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation InventoryLocation
        {
            get => inventoryLocation;
            set => SetPropertyValue(nameof(InventoryLocation), ref inventoryLocation, value);
        }

        public enum StatusEnum
        {
            Draft,
            Validated
        }

        [RuleRequiredField()]
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("InventoryScrap-InventoryScrapLines"), Aggregated()]
        public XPCollection<InventoryScrapLine> InventoryScrapLines
        {
            get
            {
                return GetCollection<InventoryScrapLine>(nameof(InventoryScrapLines));
            }
        }
    }

    public class InventoryScrapLine : BaseObject
    {
        public InventoryScrapLine(Session session) : base(session)
        { }


        double quantityToScrap;
        [Persistent(nameof(AvailableQuantity))]
        double availableQuantity;
        Lot lot;
        Product product;
        InventoryScrap inventoryScrap;

        [Association("InventoryScrap-InventoryScrapLines")]
        public InventoryScrap InventoryScrap
        {
            get => inventoryScrap;
            set => SetPropertyValue(nameof(InventoryScrap), ref inventoryScrap, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public Lot Lot
        {
            get => lot;
            set => SetPropertyValue(nameof(Lot), ref lot, value);
        }


        [PersistentAlias(nameof(availableQuantity))]
        public double AvailableQuantity
        {
            get {
                availableQuantity = 0;
                if (InventoryScrap != null && Product != null)
                {
                    if (Product.Tracking == Product.TrackingEnum.TrackByLot ||
                        Product.Tracking == Product.TrackingEnum.TrackBySerial)
                    {
                        if (Lot != null && InventoryScrap.InventoryLocation != null)
                        {
                            var smoves = new XPQuery<StockTransfer>(Session);
                            var TotalIn = (from a in smoves
                                           where a.DestinationLocation == InventoryScrap.InventoryLocation &&
                                           a.Lot == Lot
                                           select a.Quantity).Sum();

                            var TotalOut = (from a in smoves
                                            where a.SourceLocation == InventoryScrap.InventoryLocation &&
                                            a.Lot == Lot
                                            select a.Quantity).Sum();
                            availableQuantity = TotalIn - TotalOut;
                        }
                    }
                    else
                    {
                        var smoves = new XPQuery<StockTransfer>(Session);
                        var TotalIn = (from a in smoves
                                       where a.DestinationLocation == InventoryScrap.InventoryLocation
                                       select a.Quantity).Sum();

                        var TotalOut = (from a in smoves
                                        where a.SourceLocation == InventoryScrap.InventoryLocation
                                        select a.Quantity).Sum();
                        availableQuantity = TotalIn - TotalOut;
                    }
                }
                return availableQuantity;
            }
        }

        [RuleRequiredField(),
         RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.LessThanOrEqual,
            "AvailableQuantity", ParametersMode.Expression)
            ]
        public double QuantityToScrap
        {
            get => quantityToScrap;
            set => SetPropertyValue(nameof(QuantityToScrap), ref quantityToScrap, value);
        }


    }

}