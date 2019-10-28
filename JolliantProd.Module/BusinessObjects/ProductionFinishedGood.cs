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
    public class ProductionFinishedGood : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductionFinishedGood(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        [Persistent(nameof(CostPerFG))]
        decimal costPerFG;
        [Persistent(nameof(TotalFGCost))]
        decimal totalFGCost;
        DateTime expirationDate;
        string lotNumber;
        UnitOfMeasure uOM;
        double quantity;
        KitchenPlan kitchenPlan;
        Product finishedGood;



        [Association("KitchenPlan-ProductionFinishedGoods")]
        public KitchenPlan KitchenPlan
        {
            get => kitchenPlan;
            set => SetPropertyValue(nameof(KitchenPlan), ref kitchenPlan, value);
        }

        [RuleRequiredField()]
        public Product FinishedGood
        {
            get => finishedGood;
            set
            {
                SetPropertyValue(nameof(FinishedGood), ref finishedGood, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    UOM = FinishedGood.UOM;
                }

            }
        }


        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize), VisibleInDetailView(false), VisibleInListView(false)]
        public string LotNumber
        {
            get => lotNumber;
            set => SetPropertyValue(nameof(LotNumber), ref lotNumber, value);
        }


        [RuleRequiredField()]
        public DateTime ExpirationDate
        {
            get => expirationDate;
            set => SetPropertyValue(nameof(ExpirationDate), ref expirationDate, value);
        }


        [PersistentAlias(nameof(totalFGCost))]
        public decimal TotalFGCost
        {
            get
            {
                totalFGCost = PFGLines.Select(x => x.Cost).Sum();
                return totalFGCost;
            }
        }

        
        [PersistentAlias(nameof(costPerFG))]
        public decimal CostPerFG
        {
            get {
                if (Quantity != 0)
                {
                    costPerFG = TotalFGCost / Convert.ToDecimal(Quantity);
                }
                
                return costPerFG; }
        }
        




        [Association("ProductionFinishedGood-PFGLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<PFGLine> PFGLines
        {
            get
            {
                return GetCollection<PFGLine>(nameof(PFGLines));
            }
        }


    }

    public class PFGLine : BaseObject
    {
        public PFGLine(Session session) : base(session)
        { }


        [Persistent(nameof(Cost))]
        decimal cost;
        [Persistent(nameof(Component))]
        string component;
        double weight;
        ProductionBatch kitchenCode;
        ProductionFinishedGood pFG;

        [Association("ProductionFinishedGood-PFGLines")]
        public ProductionFinishedGood PFG
        {
            get => pFG;
            set => SetPropertyValue(nameof(PFG), ref pFG, value);
        }

        [RuleRequiredField()]
        public ProductionBatch KitchenCode
        {
            get => kitchenCode;
            set
            {
                SetPropertyValue(nameof(KitchenCode), ref kitchenCode, value);

                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    Weight = KitchenCode.CookedWeight;
                }
            }
        }


        [PersistentAlias(nameof(component))]
        public string Component
        {
            get
            {
                try
                {
                    component = KitchenCode.Component.ProductName;
                }
                catch (Exception)
                {


                }
                return component;
            }
        }



        public double Weight
        {
            get => weight;
            set => SetPropertyValue(nameof(Weight), ref weight, value);
        }

        
        [PersistentAlias(nameof(cost))]
        public decimal Cost
        {
            get {
                if (KitchenCode != null)
                {
                    cost = KitchenCode.CostPerWeight * Convert.ToDecimal(Weight);
                }
                
                return cost; }
        }
        

    }
}