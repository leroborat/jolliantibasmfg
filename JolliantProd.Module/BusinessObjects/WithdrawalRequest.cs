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
    public class WithdrawalRequest : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public WithdrawalRequest(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        
        [Association("ManufacturingOrder-WithdrawalRequests"), RuleRequiredField()]
        public ManufacturingOrder ManufacturingOrder
        {
            get => manufacturingOrder;
            set => SetPropertyValue(nameof(ManufacturingOrder), ref manufacturingOrder, value);
        }


        StatusEnum status;
        ManufacturingOrder manufacturingOrder;
        string processedBy;
        string requestedBy;
        WarehouseLocation fromLocation;
        string seriesName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SeriesName
        {
            get => seriesName;
            set => SetPropertyValue(nameof(SeriesName), ref seriesName, value);
        }


        public WarehouseLocation FromLocation
        {
            get => fromLocation;
            set
            {
                SetPropertyValue(nameof(FromLocation), ref fromLocation, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    SeriesName = "PRODWITH-" + (FromLocation.NextWithdrawal + 1).ToString();
                    FromLocation.NextWithdrawal += 1;
                    Session.Save(FromLocation);
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RequestedBy
        {
            get => requestedBy;
            set => SetPropertyValue(nameof(RequestedBy), ref requestedBy, value);
        }

        public enum StatusEnum
        {
            InProgress,
            Cancelled,
            Completed
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("WithdrawalRequest-WithdrawalRequestLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<WithdrawalRequestLine> WithdrawalRequestLines
        {
            get
            {
                return GetCollection<WithdrawalRequestLine>(nameof(WithdrawalRequestLines));
            }
        }

        [Association("WithdrawalRequest-ProductionTransfers"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ProductionTransfer> ProductionTransfers
        {
            get
            {
                return GetCollection<ProductionTransfer>(nameof(ProductionTransfers));
            }
        }

       

    }

    public class WithdrawalRequestLine : BaseObject
    {
        public WithdrawalRequestLine(Session session) : base(session)
        { }


        
        [Association("WithdrawalRequest-WithdrawalRequestLines")]
        public WithdrawalRequest WithdrawalRequest
        {
            get => withdrawalRequest;
            set => SetPropertyValue(nameof(WithdrawalRequest), ref withdrawalRequest, value);
        }
        
        UnitOfMeasure stockingUOM;
        double stockingQuantity;
        [Persistent(nameof(ProcessedQuantity))]
        double processedQuantity;
        UnitOfMeasure productionUOM;
        double demand;
        WithdrawalRequest withdrawalRequest;
        Product product;

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public double Demand
        {
            get => demand;
            set => SetPropertyValue(nameof(Demand), ref demand, value);
        }


        public UnitOfMeasure ProductionUOM
        {
            get => productionUOM;
            set => SetPropertyValue(nameof(ProductionUOM), ref productionUOM, value);
        }


        public double StockingQuantity
        {
            get => stockingQuantity;
            set => SetPropertyValue(nameof(StockingQuantity), ref stockingQuantity, value);
        }


        public UnitOfMeasure StockingUOM
        {
            get => stockingUOM;
            set => SetPropertyValue(nameof(StockingUOM), ref stockingUOM, value);
        }

        [PersistentAlias(nameof(processedQuantity))]
        public double ProcessedQuantity
        {
            get
            {
                processedQuantity = (from c in WithdrawalRequest.ProductionTransfers
                                     where c.Status == ProductionTransfer.StatusEnum.Validated
                                     from a in c.ProductionTransferLines
                                     where a.Product == Product
                                     select a.ProcessedStockingQuantity).Sum();
                return processedQuantity;
            }
        }

        

    }
}