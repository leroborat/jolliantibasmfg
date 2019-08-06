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
    [DefaultClassOptions, XafDefaultProperty("KitchenCode")]
    public class ProductionBatch : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductionBatch(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        StatusEnum status;
        DateTime time;
        double cookedWeight;
        string kitchenCode;
        Product component;

        KitchenPlan kitchenPlan;

        [Association("KitchenPlan-ProductionBatches"), RuleRequiredField()]
        public KitchenPlan KitchenPlan
        {
            get => kitchenPlan;
            set => SetPropertyValue(nameof(KitchenPlan), ref kitchenPlan, value);
        }



        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string KitchenCode
        {
            get => kitchenCode;
            set => SetPropertyValue(nameof(KitchenCode), ref kitchenCode, value);
        }


        public Product Component
        {
            get => component;
            set => SetPropertyValue(nameof(Component), ref component, value);
        }


        [RuleRequiredField()]
        public double CookedWeight
        {
            get => cookedWeight;
            set => SetPropertyValue(nameof(CookedWeight), ref cookedWeight, value);
        }

        [RuleRequiredField()]
        public DateTime Time
        {
            get => time;
            set => SetPropertyValue(nameof(Time), ref time, value);
        }

        [Association("ProductionBatch-BatchConsumptions"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BatchConsumption> BatchConsumptions
        {
            get
            {
                return GetCollection<BatchConsumption>(nameof(BatchConsumptions));
            }
        }

        [Association("ProductionBatch-BatchRoutes"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BatchRoute> BatchRoutes
        {
            get
            {
                return GetCollection<BatchRoute>(nameof(BatchRoutes));
            }
        }

        public enum StatusEnum
        {
            Draft,
            Validated
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Action(Caption = "Validate", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void ValidateStatus ()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Validated;
            Session.Save(this);
        }

    }

    public class BatchConsumption : BaseObject
    {
        public BatchConsumption(Session session) : base(session)
        { }


        [Persistent(nameof(QuantityConsumed))]
        double quantityConsumed;
        UnitOfMeasure uOM;
        Product itemConsumed;
        ProductionBatch productionBatch;

        [Association("ProductionBatch-BatchConsumptions")]
        public ProductionBatch ProductionBatch
        {
            get => productionBatch;
            set => SetPropertyValue(nameof(ProductionBatch), ref productionBatch, value);
        }


        public Product ItemConsumed
        {
            get => itemConsumed;
            set
            {

                SetPropertyValue(nameof(ItemConsumed), ref itemConsumed, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    UOM = ItemConsumed.ProductionUOM;
                }
            }
        }


        
        [PersistentAlias(nameof(quantityConsumed))]
        public double QuantityConsumed
        {
            get {
                quantityConsumed = BatchConsumptionLines.Select(x => x.Quantity).Sum();
                return quantityConsumed; }
        }
        


        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
        }

        [Association("BatchConsumption-BatchConsumptionLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BatchConsumptionLine> BatchConsumptionLines
        {
            get
            {
                return GetCollection<BatchConsumptionLine>(nameof(BatchConsumptionLines));
            }
        }

    }

    public class BatchConsumptionLine : BaseObject
    {
        public BatchConsumptionLine(Session session) : base(session)
        { }


        double quantity;
        Lot lotCode;
        BatchConsumption batchConsumption;

        [Association("BatchConsumption-BatchConsumptionLines")]
        public BatchConsumption BatchConsumption
        {
            get => batchConsumption;
            set => SetPropertyValue(nameof(BatchConsumption), ref batchConsumption, value);
        }


        public Lot LotCode
        {
            get => lotCode;
            set => SetPropertyValue(nameof(LotCode), ref lotCode, value);
        }

        
        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


    }

    public class BatchRoute : BaseObject
    {
        public BatchRoute(Session session) : base(session)
        { }


        ProductionBatch productionBatch;
        DateTime endTime;
        DateTime startTime;
        WorkCenter workCenter;

        [RuleRequiredField()]
        public WorkCenter WorkCenter
        {
            get => workCenter;
            set => SetPropertyValue(nameof(WorkCenter), ref workCenter, value);
        }


        public DateTime StartTime
        {
            get => startTime;
            set => SetPropertyValue(nameof(StartTime), ref startTime, value);
        }


        public DateTime EndTime
        {
            get => endTime;
            set => SetPropertyValue(nameof(EndTime), ref endTime, value);
        }

        
        [Association("ProductionBatch-BatchRoutes")]
        public ProductionBatch ProductionBatch
        {
            get => productionBatch;
            set => SetPropertyValue(nameof(ProductionBatch), ref productionBatch, value);
        }
    }
}