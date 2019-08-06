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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class KitchenPlan : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public KitchenPlan(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SeriesName
        {
            get => seriesName;
            set => SetPropertyValue(nameof(SeriesName), ref seriesName, value);
        }

        WarehouseLocation stockLocation;
        string processedBy;
        StatusEnum status;
        ShiftEnum shift;
        DateTime date;
        string seriesName;
        Warehouse warehouse;
        [RuleRequiredField()]
        public Warehouse Warehouse
        {
            get => warehouse;
            set
            {
                SetPropertyValue(nameof(Warehouse), ref warehouse, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    Warehouse.NextKitchenPlan += 1;
                    Session.Save(Warehouse);
                    SeriesName = Warehouse.WarehouseName + "-KITCHENPLAN-" + Warehouse.NextKitchenPlan;
                }
            }
        }

        [RuleRequiredField()]
        public WarehouseLocation StockLocation
        {
            get => stockLocation;
            set => SetPropertyValue(nameof(StockLocation), ref stockLocation, value);
        }


        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        public enum ShiftEnum
        {
            AM,
            PM
        }

        [RuleRequiredField()]
        public ShiftEnum Shift
        {
            get => shift;
            set => SetPropertyValue(nameof(Shift), ref shift, value);
        }

        public enum StatusEnum
        {
            Draft,
            Planned,
            InProgress,
            Done
        }


        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("KitchenPlan-Withdrawals"), DevExpress.Xpo.Aggregated()]
        public XPCollection<Withdrawal> Withdrawals
        {
            get
            {
                return GetCollection<Withdrawal>(nameof(Withdrawals));
            }
        }

        [Association("KitchenPlan-KitchenPlanLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<KitchenPlanLine> KitchenPlanLines
        {
            get
            {
                return GetCollection<KitchenPlanLine>(nameof(KitchenPlanLines));
            }
        }

        [Association("KitchenPlan-ProductionBatches")]
        public XPCollection<ProductionBatch> ProductionBatches
        {
            get
            {
                return GetCollection<ProductionBatch>(nameof(ProductionBatches));
            }
        }

        [Association("KitchenPlan-ProductionFinishedGoods"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ProductionFinishedGood> ProductionFinishedGoods
        {
            get
            {
                return GetCollection<ProductionFinishedGood>(nameof(ProductionFinishedGoods));
            }
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProcessedBy
        {
            get => processedBy;
            set => SetPropertyValue(nameof(ProcessedBy), ref processedBy, value);
        }

        [Action(Caption = "Confirm", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void PlanMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Planned;
            foreach (KitchenPlanLine line in KitchenPlanLines)
            {
                for (int i = 0; i < line.Batches; i++)
                {
                    ProductionBatch pb = new ProductionBatch(Session);
                    pb.Component = line.ItemName;
                    pb.KitchenPlan = line.KitchenPlan;
                    ProductionBatches.Add(pb);
                }
            }
            Session.Save(this);
            Session.CommitTransaction();
        }

        [Action(Caption = "Mark Done", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void DoneMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            Status = StatusEnum.Done;
            ProcessedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            
            WarehouseLocation warehouseLocation;
            warehouseLocation = Session.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Production"));
            if (warehouseLocation == null)
            {
                warehouseLocation = new WarehouseLocation(Session);
                warehouseLocation.LocationName = "Production";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.Production;
                Session.Save(warehouseLocation);
            }

           
            foreach (ProductionFinishedGood item in ProductionFinishedGoods)
            {
                StockTransfer st = new StockTransfer(Session);
                st.DestinationLocation = StockLocation;
                st.SourceLocation = warehouseLocation;

                Lot newLot = new Lot(Session);
                newLot.Product = item.FinishedGood;
                newLot.LotCode = item.LotNumber;
                newLot.ExpirationDate = item.ExpirationDate;
                newLot.InternalReference = SeriesName;
                st.Lot = newLot;
                st.Quantity = item.Quantity;
                st.UOM = item.FinishedGood.UOM;
                st.Product = item.FinishedGood;
                st.Reference = "Moved FG to Stock: " + SeriesName;
                Session.Save(This);
            }

            Session.CommitTransaction();
        }

    }

    public class KitchenPlanLine : BaseObject
    {
        public KitchenPlanLine(Session session) : base(session)
        { }


        KitchenPlan kitchenPlan;
        string remarks;
        int batches;
        Product itemName;

        
        [Association("KitchenPlan-KitchenPlanLines")]
        public KitchenPlan KitchenPlan
        {
            get => kitchenPlan;
            set => SetPropertyValue(nameof(KitchenPlan), ref kitchenPlan, value);
        }

        [RuleRequiredField()]
        public Product ItemName
        {
            get => itemName;
            set => SetPropertyValue(nameof(ItemName), ref itemName, value);
        }


        public int Batches
        {
            get => batches;
            set => SetPropertyValue(nameof(Batches), ref batches, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Remarks
        {
            get => remarks;
            set => SetPropertyValue(nameof(Remarks), ref remarks, value);
        }

    }
}