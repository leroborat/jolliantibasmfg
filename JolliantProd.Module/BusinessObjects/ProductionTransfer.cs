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
    public class ProductionTransfer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductionTransfer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        DateTime processedDate;
        WithdrawalRequest withdrawalRequest;

        [Association("WithdrawalRequest-ProductionTransfers"), RuleRequiredField()]
        public WithdrawalRequest WithdrawalRequest
        {
            get => withdrawalRequest;
            set => SetPropertyValue(nameof(WithdrawalRequest), ref withdrawalRequest, value);
        }

        
        public DateTime ProcessedDate
        {
            get => processedDate;
            set => SetPropertyValue(nameof(ProcessedDate), ref processedDate, value);
        }

        [Association("ProductionTransfer-ProductionTransferLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ProductionTransferLine> ProductionTransferLines
        {
            get
            {
                return GetCollection<ProductionTransferLine>(nameof(ProductionTransferLines));
            }
        }
    }

    public class ProductionTransferLine : BaseObject
    {
        public ProductionTransferLine(Session session) : base(session)
        { }


        ProductionTransfer productionTransfer;
        UnitOfMeasure productionUOM;
        double productionQuantity;
        UnitOfMeasure stockingUOM;
        double processedStockingQuantity;
        Product product;

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public double ProcessedStockingQuantity
        {
            get => processedStockingQuantity;
            set => SetPropertyValue(nameof(ProcessedStockingQuantity), ref processedStockingQuantity, value);
        }


        public UnitOfMeasure StockingUOM
        {
            get => stockingUOM;
            set => SetPropertyValue(nameof(StockingUOM), ref stockingUOM, value);
        }


        public double ProductionQuantity
        {
            get => productionQuantity;
            set => SetPropertyValue(nameof(ProductionQuantity), ref productionQuantity, value);
        }


        public UnitOfMeasure ProductionUOM
        {
            get => productionUOM;
            set => SetPropertyValue(nameof(ProductionUOM), ref productionUOM, value);
        }

        
        [Association("ProductionTransfer-ProductionTransferLines")]
        public ProductionTransfer ProductionTransfer
        {
            get => productionTransfer;
            set => SetPropertyValue(nameof(ProductionTransfer), ref productionTransfer, value);
        }


        [Association("ProductionTransferLine-ProductionTransferLineLots"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ProductionTransferLineLot> ProductionTransferLineLots
        {
            get
            {
                return GetCollection<ProductionTransferLineLot>(nameof(ProductionTransferLineLots));
            }
        }
    }

    public class ProductionTransferLineLot : BaseObject
    {
        public ProductionTransferLineLot(Session session) : base(session)
        { }


        double stockingQuantity;
        Lot lot;
        ProductionTransferLine productionTransferLine;

        [Association("ProductionTransferLine-ProductionTransferLineLots")]
        public ProductionTransferLine ProductionTransferLine
        {
            get => productionTransferLine;
            set => SetPropertyValue(nameof(ProductionTransferLine), ref productionTransferLine, value);
        }


        public Lot Lot
        {
            get => lot;
            set => SetPropertyValue(nameof(Lot), ref lot, value);
        }

        
        public double StockingQuantity
        {
            get => stockingQuantity;
            set => SetPropertyValue(nameof(StockingQuantity), ref stockingQuantity, value);
        }
    }
}