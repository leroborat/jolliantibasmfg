using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using System.Diagnostics;
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
    public class Withdrawal : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Withdrawal(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            RequestedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            WithdrawalDate = DateTime.Now;
        }


        KitchenPlan kitchenPlan;
        DateTime withdrawalDate;
        StatusEnum status;
        string transferredBy;
        string requestedBy;
        WarehouseLocation location;
        string seriesName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SeriesName
        {
            get => seriesName;
            set => SetPropertyValue(nameof(SeriesName), ref seriesName, value);
        }

        [RuleRequiredField()]
        public DateTime WithdrawalDate
        {
            get => withdrawalDate;
            set => SetPropertyValue(nameof(WithdrawalDate), ref withdrawalDate, value);
        }


        public WarehouseLocation Location
        {
            get => location;
            set
            {
                SetPropertyValue(nameof(Location), ref location, value);
                if (!IsLoading && !IsSaving)
                {
                    SeriesName = Location.DisplayName + "/" + "WITHDRAW/" + Location.NextWithdrawal;
                    Location.NextWithdrawal += 1;
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RequestedBy
        {
            get => requestedBy;
            set => SetPropertyValue(nameof(RequestedBy), ref requestedBy, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TransferredBy
        {
            get => transferredBy;
            set => SetPropertyValue(nameof(TransferredBy), ref transferredBy, value);
        }

        [Association("Withdrawal-WithdrawalLines"), Aggregated()]
        public XPCollection<WithdrawalLine> WithdrawalLines
        {
            get
            {
                return GetCollection<WithdrawalLine>(nameof(WithdrawalLines));
            }
        }

        public enum StatusEnum
        {
            New,
            Validated,
            Cancelled,
            Submitted
        }


        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        
        [Association("KitchenPlan-Withdrawals"), RuleRequiredField()]
        public KitchenPlan KitchenPlan
        {
            get => kitchenPlan;
            set {
                SetPropertyValue(nameof(KitchenPlan), ref kitchenPlan, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    Location = KitchenPlan.StockLocation;
                    //GET ALL Allocations wherein kitchen plan = this
                    var allocations = new XPQuery<ComponentAllotment>(Session).Where(
                        x => x.KitchenPlanLine.KitchenPlan == KitchenPlan);
                    var materialList = allocations.Select(x => x.Material).Distinct();
                    //GET Distinct Materials
                    foreach (var item in materialList)
                    {
                        //Get Quantity for each material
                        double allocCount = allocations.Where(x => x.Material == item).Select(x => x.Quantity).Sum();
                        WithdrawalLine withdrawalLine = new WithdrawalLine(Session);
                        withdrawalLine.Product = item;
                        if (item.UOM != item.ProductionUOM)
                        {
                            allocCount = allocCount / item.UOMRatioProduction;
                        }
                        withdrawalLine.DemandQuantity = 0;
                        WithdrawalLines.Add(withdrawalLine);
                    }
                    //Get Quantity for each material
                    //Convert to Storage UOM
                    //Generate entries
                }
            }
        }

        [Action(Caption = "Submit", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void ActionMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Submitted;
            Session.Save(this);
        }
    }

    public class WithdrawalLine : BaseObject
    {
        public WithdrawalLine(Session session) : base(session)
        { }


        double productionQuantity;
        [Persistent(nameof(ProductionPerUnitCost))]
        decimal productionPerUnitCost;
        [Persistent(nameof(RawCost))]
        decimal rawCost;

        UnitOfMeasure productionUOM;
        UnitOfMeasure uOM;
        [Persistent(nameof(ProcessedQuantity))]
        double processedQuantity;

        double demandQuantity;
        Product product;
        Withdrawal withdrawal;

        [Association("Withdrawal-WithdrawalLines")]
        public Withdrawal Withdrawal
        {
            get => withdrawal;
            set => SetPropertyValue(nameof(Withdrawal), ref withdrawal, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set
            {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    UOM = Product.UOM;
                    ProductionUOM = Product.ProductionUOM;
                    //Set Default Lots
                }
            }
        }


        public double DemandQuantity
        {
            get => demandQuantity;
            set => SetPropertyValue(nameof(DemandQuantity), ref demandQuantity, value);
        }


        [PersistentAlias(nameof(processedQuantity))]
        public double ProcessedQuantity
        {
            get
            {
                processedQuantity = WithdrawalLineLots.Select(x => x.DoneQuantity).Sum();
                return processedQuantity;
            }
        }


        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
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

        [Association("WithdrawalLine-WithdrawalLineLots"), Aggregated()]
        public XPCollection<WithdrawalLineLot> WithdrawalLineLots
        {
            get
            {
                return GetCollection<WithdrawalLineLot>(nameof(WithdrawalLineLots));
            }
        }


        [PersistentAlias(nameof(rawCost))]
        public decimal RawCost
        {
            get
            {
                if (WithdrawalLineLots.Count() !=0)
                {
                    rawCost = WithdrawalLineLots.Select(x => x.Lot).Sum(y => y.CostPerUnit) / Convert.ToDecimal(WithdrawalLineLots.Count());
                    rawCost = rawCost * Convert.ToDecimal(ProcessedQuantity);
                }
                
                return rawCost;
            }
        }

        
        [PersistentAlias(nameof(productionPerUnitCost))]
        public decimal ProductionPerUnitCost
        {
            get {
                if (ProductionQuantity != 0)
                {
                    productionPerUnitCost = RawCost / Convert.ToDecimal(ProductionQuantity);
                }
                
                return productionPerUnitCost; }
        }
        
        

    }


    [RuleCombinationOfPropertiesIsUnique("UniqueWithdrawalLot", DefaultContexts.Save, "Lot, WithdrawalLine")]
    public class WithdrawalLineLot : BaseObject
    {
        public WithdrawalLineLot(Session session) : base(session)
        { }


        [Persistent(nameof(LotAge))]
        int lotAge;
        double doneQuantity;
        Lot lot;
        WithdrawalLine withdrawalLine;

        [Association("WithdrawalLine-WithdrawalLineLots")]
        public WithdrawalLine WithdrawalLine
        {
            get => withdrawalLine;
            set => SetPropertyValue(nameof(WithdrawalLine), ref withdrawalLine, value);
        }


        public Lot Lot
        {
            get => lot;
            set => SetPropertyValue(nameof(Lot), ref lot, value);
        }


        public double DoneQuantity
        {
            get => doneQuantity;
            set { SetPropertyValue(nameof(DoneQuantity), ref doneQuantity, value);

                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    if (WithdrawalLine != null)
                    {
                        if (WithdrawalLine.ProductionUOM != WithdrawalLine.UOM)
                        {
                            WithdrawalLine.ProductionQuantity = WithdrawalLine.ProcessedQuantity * WithdrawalLine.Product.UOMRatioProduction;
                        }
                        else
                        {
                            WithdrawalLine.ProductionQuantity = WithdrawalLine.ProcessedQuantity;
                        }
                    }
                }
            }
        }

        
        [PersistentAlias(nameof(lotAge))]
        public int LotAge
        {
            get {
                try
                {
                    if (Lot != null)
                    {
                        var age = new XPQuery<StockTransfer>(Session)
                            .Where(x => x.DestinationLocation == WithdrawalLine.Withdrawal.Location)
                            .Where(x => x.Product == WithdrawalLine.Product)
                            .Where(x => x.Lot == Lot)
                            .Select(x => x.Date).First();

                        Console.WriteLine(age);
                        lotAge = (WithdrawalLine.Withdrawal.WithdrawalDate - age).Days;
                        Console.WriteLine(lotAge);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                    lotAge = 0;
                }
                
                return lotAge; }
        }
        
    }
}