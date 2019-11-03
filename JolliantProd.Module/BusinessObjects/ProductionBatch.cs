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
            Batches = 1;
        }


        double pieces;
        double basins;
        double batches;
        [Persistent(nameof(CostPerWeight))]
        decimal costPerWeight;
        [Persistent(nameof(BatchTotalCost))]
        decimal batchTotalCost;
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



        [Size(SizeAttribute.DefaultStringMappingFieldSize), NonCloneable()]
        public string KitchenCode
        {
            get => kitchenCode;
            set => SetPropertyValue(nameof(KitchenCode), ref kitchenCode, value);
        }


        public Product Component
        {
            get => component;
            set
            {
                SetPropertyValue(nameof(Component), ref component, value);

                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    try
                    {
                        var toConsume = Component.BillOfMaterials.First();
                        foreach (var item in toConsume.BomComponents)
                        {
                            var nline = new BatchConsumption(Session);
                            nline.ProductionBatch = this;
                            nline.ItemConsumed = item.Component;
                            BatchConsumptions.Add(nline);
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
            }
        }


        public double Batches
        {
            get => batches;
            set
            {
                SetPropertyValue(nameof(Batches), ref batches, value);
                if (!IsLoading & !IsSaving & !IsDeleted)
                {
                    foreach (var item in BatchConsumptions)
                    {
                        try
                        {
                            if (Batches != 0)
                            {
                                var myBom = Component.BillOfMaterials.First()
                                .BomComponents.Where(x => x.Component == item.ItemConsumed).First();

                                item.QuantityConsumed = myBom.Quantity * Batches;
                            }
                            
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }


        public double Basins
        {
            get => basins;
            set { SetPropertyValue(nameof(Basins), ref basins, value);
                if (!IsLoading & !IsSaving & !IsDeleted)
                {
                    foreach (var item in BatchConsumptions)
                    {
                        try
                        {
                            if (Basins != 0)
                            {
                                var myBom = Component.BillOfMaterials.First()
                                .BomComponents.Where(x => x.Component == item.ItemConsumed).First();

                                item.QuantityConsumed = myBom.Quantity * Basins;
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        
        public double Pieces
        {
            get => pieces;
            set { SetPropertyValue(nameof(Pieces), ref pieces, value);
                if (!IsLoading & !IsSaving & !IsDeleted)
                {
                    foreach (var item in BatchConsumptions)
                    {
                        try
                        {
                            if (Pieces != 0)
                            {
                                var myBom = Component.BillOfMaterials.First()
                                .BomComponents.Where(x => x.Component == item.ItemConsumed).First();

                                item.QuantityConsumed = myBom.Quantity * Pieces;
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }

            }
        }


        [RuleRequiredField()]
        public double CookedWeight
        {
            get => cookedWeight;
            set => SetPropertyValue(nameof(CookedWeight), ref cookedWeight, value);
        }


        [PersistentAlias(nameof(batchTotalCost))]
        public decimal BatchTotalCost
        {
            get
            {
                batchTotalCost = BatchConsumptions.Select(x => x.Cost).Sum() +
                    BatchRoutes.Select(x => x.Cost).Sum();
                return batchTotalCost;
            }
        }

        
        [PersistentAlias(nameof(costPerWeight))]
        public decimal CostPerWeight
        {
            get {
                if (CookedWeight != 0)
                {
                    costPerWeight = BatchTotalCost / Convert.ToDecimal(CookedWeight);
                }
                
                return costPerWeight; }
        }
        


        
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

        [Association("ProductionBatch-ProductionYieldMonitors"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ProductionYieldMonitor> ProductionYieldMonitors
        {
            get
            {
                return GetCollection<ProductionYieldMonitor>(nameof(ProductionYieldMonitors));
            }
        }

        [Association("ProductionBatch-BatchTransmittals"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BatchTransmittal> BatchTransmittals
        {
            get
            {
                return GetCollection<BatchTransmittal>(nameof(BatchTransmittals));
            }
        }

        private XPCollection<AuditDataItemPersistent> auditTrail;

        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }

        public enum StatusEnum
        {
            Draft,
            Validated
        }

        [NonCloneable()]
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


        [Persistent(nameof(Cost))]
        decimal cost;
        [Persistent(nameof(ConsumptionVariance))]
        double consumptionVariance;
        double actualConsumed;
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
                    try
                    {
                        var myBom = ProductionBatch.Component.BillOfMaterials.First()
                            .BomComponents.Where(x => x.Component == ItemConsumed).First();

                        QuantityConsumed = myBom.Quantity;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }




        public double QuantityConsumed
        {
            get => quantityConsumed;
            set => SetPropertyValue(nameof(QuantityConsumed), ref quantityConsumed, value);
        }


        public double ActualConsumed
        {
            get => actualConsumed;
            set => SetPropertyValue(nameof(ActualConsumed), ref actualConsumed, value);
        }

        
        [PersistentAlias(nameof(cost))]
        public decimal Cost
        {
            get {
                try
                {
                    var cpu = new XPQuery<WithdrawalLine>(Session)
                    .Where(x => x.Withdrawal.KitchenPlan == ProductionBatch.KitchenPlan)
                    .Where(x => x.Product == ItemConsumed)
                    .FirstOrDefault();

                    if (QuantityConsumed >= ActualConsumed)
                    {
                        cost = cpu.ProductionPerUnitCost * Convert.ToDecimal(QuantityConsumed);
                    } else
                    {
                        cost = cpu.ProductionPerUnitCost * Convert.ToDecimal(ActualConsumed);
                    }
                    
                }
                catch (Exception)
                {

                }
                
                  
                return cost; }
        }
        
        


        [PersistentAlias(nameof(consumptionVariance))]
        public double ConsumptionVariance
        {
            get {
                consumptionVariance = ActualConsumed - QuantityConsumed;
                return consumptionVariance; }
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


        [Persistent(nameof(Cost))]
        decimal cost;
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

        
        [PersistentAlias(nameof(cost))]
        public decimal Cost
        {
            get {
                try
                {
                    cost = Convert.ToDecimal((EndTime - StartTime).TotalHours) * WorkCenter.CostPerHour;
                }
                catch (Exception)
                {

                }
                
                return cost; }
        }
        

        [Association("ProductionBatch-BatchRoutes")]
        public ProductionBatch ProductionBatch
        {
            get => productionBatch;
            set => SetPropertyValue(nameof(ProductionBatch), ref productionBatch, value);
        }
    }

    public class BatchTransmittal : BaseObject
    {
        public BatchTransmittal(Session session) : base(session)
        { }


        [Persistent(nameof(TotalWeight))]
        double totalWeight;
        WorkCenter to;
        WorkCenter from;
        ProductionBatch productionBatch;

        [Association("ProductionBatch-BatchTransmittals")]
        public ProductionBatch ProductionBatch
        {
            get => productionBatch;
            set => SetPropertyValue(nameof(ProductionBatch), ref productionBatch, value);
        }


        [RuleRequiredField()]
        public WorkCenter From
        {
            get => from;
            set => SetPropertyValue(nameof(From), ref from, value);
        }

        [RuleRequiredField()]
        public WorkCenter To
        {
            get => to;
            set => SetPropertyValue(nameof(To), ref to, value);
        }

        
        [PersistentAlias(nameof(totalWeight))]
        public double TotalWeight
        {
            get {
                totalWeight = BatchTransmittalLines.Select(x => x.TotalUnits).Sum();
                return totalWeight; }
        }
        


        [Association("BatchTransmittal-BatchTransmittalLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BatchTransmittalLine> BatchTransmittalLines
        {
            get
            {
                return GetCollection<BatchTransmittalLine>(nameof(BatchTransmittalLines));
            }
        }



    }

    public class BatchTransmittalLine : BaseObject
    {
        public BatchTransmittalLine(Session session) : base(session)
        { }


        string remarks;
        double totalUnits;
        string description;
        string itemCode;
        BatchTransmittal batchTransmittal;

        [Association("BatchTransmittal-BatchTransmittalLines")]
        public BatchTransmittal BatchTransmittal
        {
            get => batchTransmittal;
            set => SetPropertyValue(nameof(BatchTransmittal), ref batchTransmittal, value);
        }



        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string ItemCode
        {
            get => itemCode;
            set => SetPropertyValue(nameof(ItemCode), ref itemCode, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [RuleRequiredField()]
        public double TotalUnits
        {
            get => totalUnits;
            set => SetPropertyValue(nameof(TotalUnits), ref totalUnits, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Remarks
        {
            get => remarks;
            set => SetPropertyValue(nameof(Remarks), ref remarks, value);
        }


    }
}