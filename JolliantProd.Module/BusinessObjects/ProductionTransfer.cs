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
            ProcessedDate = DateTime.Now;
        }

        WarehouseLocation location;
        StatusEnum status;
        string processedBy;
        DateTime processedDate;
        WithdrawalRequest withdrawalRequest;

        [Association("WithdrawalRequest-ProductionTransfers"), RuleRequiredField()]
        public WithdrawalRequest WithdrawalRequest
        {
            get => withdrawalRequest;
            set
            {
                SetPropertyValue(nameof(WithdrawalRequest), ref withdrawalRequest, value);
                if (!IsLoading && !IsSaving && !IsDeleted && ProductionTransferLines.Count <= 0)
                {
                    Location = WithdrawalRequest.FromLocation;
                    foreach (var item in WithdrawalRequest.WithdrawalRequestLines)
                    {
                        if (item.ProcessedQuantity < item.StockingQuantity)
                        {
                            var thisProductLine = new ProductionTransferLine(Session);
                            thisProductLine.Product = item.Product;
                            ProductionTransferLines.Add(thisProductLine);
                        }
                    }
                    Session.Save(this);
                }
            }
        }

        
        public WarehouseLocation Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }


        public DateTime ProcessedDate
        {
            get => processedDate;
            set => SetPropertyValue(nameof(ProcessedDate), ref processedDate, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProcessedBy
        {
            get => processedBy;
            set => SetPropertyValue(nameof(ProcessedBy), ref processedBy, value);
        }

        [Association("ProductionTransfer-ProductionTransferLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ProductionTransferLine> ProductionTransferLines
        {
            get
            {
                return GetCollection<ProductionTransferLine>(nameof(ProductionTransferLines));
            }
        }

        public enum StatusEnum
        {
            Draft,
            Validated,
            Cancelled
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }
    }

    public class ProductionTransferLine : BaseObject
    {
        public ProductionTransferLine(Session session) : base(session)
        { }


        [Persistent(nameof(ProcessedStockingQuantity))]
        double processedStockingQuantity;
        [Persistent(nameof(ProductionQuantity))]
        double productionQuantity;
        ProductionTransfer productionTransfer;
        UnitOfMeasure productionUOM;

        UnitOfMeasure stockingUOM;

        Product product;

        public Product Product
        {
            get => product;
            set
            {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    StockingUOM = Product.UOM;
                    ProductionUOM = Product.ProductionUOM;
                }
            }
        }


        
        [PersistentAlias(nameof(processedStockingQuantity))]
        public double ProcessedStockingQuantity
        {
            get {
                processedStockingQuantity = ProductionTransferLineLots.Select(x => x.StockingQuantity).Sum();
                return processedStockingQuantity; }
        }
        


        public UnitOfMeasure StockingUOM
        {
            get => stockingUOM;
            set => SetPropertyValue(nameof(StockingUOM), ref stockingUOM, value);
        }


        
        [PersistentAlias(nameof(productionQuantity))]
        public double ProductionQuantity
        {
            get {
                try
                {
                    productionQuantity = ProcessedStockingQuantity * Product.UOMRatioProduction;
                }
                catch (Exception)
                {

                }
                
                return productionQuantity; }
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
            set { SetPropertyValue(nameof(ProductionTransfer), ref productionTransfer, value);
            }
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

        [RuleRequiredField()]
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