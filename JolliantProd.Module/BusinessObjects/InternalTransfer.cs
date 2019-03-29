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
    public class InternalTransfer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InternalTransfer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TransferDate = DateTime.Now;
            Status = StatusEnum.Draft;
        }


        StatusEnum status;
        DateTime transferDate;
        WarehouseLocation destinationLocation;
        WarehouseLocation sourceLocation;
        string referenceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string ReferenceName
        {
            get => referenceName;
            set => SetPropertyValue(nameof(ReferenceName), ref referenceName, value);
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


        public DateTime TransferDate
        {
            get => transferDate;
            set => SetPropertyValue(nameof(TransferDate), ref transferDate, value);
        }

        public enum StatusEnum
        {
            Draft,
            Done
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("InternalTransfer-InternalTransferLines"), Aggregated()]
        public XPCollection<InternalTransferLine> InternalTransferLines
        {
            get
            {
                return GetCollection<InternalTransferLine>(nameof(InternalTransferLines));
            }
        }

    }

    public class InternalTransferLine : BaseObject
    {
        public InternalTransferLine(Session session) : base(session)
        { }

        
        [Association("InternalTransfer-InternalTransferLines")]
        public InternalTransfer InternalTransfer
        {
            get => internalTransfer;
            set => SetPropertyValue(nameof(InternalTransfer), ref internalTransfer, value);
        }

        [Persistent(nameof(AvailableQuantity))]
        double availableQuantity;
        double quantityDone;
        Lot lotNumber;
        InternalTransfer internalTransfer;
        Product product;
        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public Lot LotNumber
        {
            get => lotNumber;
            set => SetPropertyValue(nameof(LotNumber), ref lotNumber, value);
        }

        
        [PersistentAlias(nameof(availableQuantity))]
        public double AvailableQuantity
        {
            get {
                availableQuantity = 0;

                if (InternalTransfer != null && InternalTransfer.SourceLocation.LocationType == WarehouseLocation.LocationTypeEnum.Internal)
                {
                    if (Product.Tracking == Product.TrackingEnum.TrackByLot ||
                        Product.Tracking == Product.TrackingEnum.TrackBySerial)
                    {
                        if (LotNumber != null && InternalTransfer.SourceLocation != null)
                        {
                            var TotalIn = new XPQuery<StockTransfer>(Session)
                                .Where(x => x.DestinationLocation == InternalTransfer.SourceLocation &&
                                           x.Lot == LotNumber).Select(x => x.Quantity).Sum();

                            var TotalOut = new XPQuery<StockTransfer>(Session)
                                .Where(x => x.SourceLocation == InternalTransfer.SourceLocation &&
                                           x.Lot == LotNumber).Select(x => x.Quantity).Sum();

                            availableQuantity = TotalIn - TotalOut;
                        }
                    }
                    else
                    {
                        var smoves = new XPQuery<StockTransfer>(Session);
                        var TotalIn = (from a in smoves
                                       where a.DestinationLocation == InternalTransfer.SourceLocation
                                       select a.Quantity).Sum();

                        var TotalOut = (from a in smoves
                                        where a.SourceLocation == InternalTransfer.SourceLocation
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
        public double QuantityDone
        {
            get => quantityDone;
            set => SetPropertyValue(nameof(QuantityDone), ref quantityDone, value);
        }
    }
}