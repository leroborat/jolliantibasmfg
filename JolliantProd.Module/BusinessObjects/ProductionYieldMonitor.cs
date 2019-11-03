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
    
    public class ProductionYieldMonitor : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductionYieldMonitor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        [Persistent(nameof(Variance))]
        double variance;
        Product product;
        ProductionBatch productionBatch;

        [Association("ProductionBatch-ProductionYieldMonitors")]
        public ProductionBatch ProductionBatch
        {
            get => productionBatch;
            set => SetPropertyValue(nameof(ProductionBatch), ref productionBatch, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        
        [PersistentAlias(nameof(variance))]
        public double Variance
        {
            get {
                if (PYMLines.Count > 1)
                {
                    variance = PYMLines.OrderByDescending(x => x.TransferredWeight).Select(x => x.TransferredWeight).First() -
                        PYMLines.OrderByDescending(x => x.TransferredWeight).Select(x => x.TransferredWeight).Last();

                }
                return variance; }
        }
        

        [Association("ProductionYieldMonitor-PYMLines")]
        public XPCollection<PYMLIne> PYMLines
        {
            get
            {
                return GetCollection<PYMLIne>(nameof(PYMLines));
            }
        }



    }

    public class PYMLIne : BaseObject
    {
        public PYMLIne(Session session) : base(session)
        { }


        double transferredWeight;
        WorkCenter to;
        WorkCenter from;
        ProductionYieldMonitor productionYieldMonitor;

        [Association("ProductionYieldMonitor-PYMLines")]
        public ProductionYieldMonitor ProductionYieldMonitor
        {
            get => productionYieldMonitor;
            set => SetPropertyValue(nameof(ProductionYieldMonitor), ref productionYieldMonitor, value);
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

        
        public double TransferredWeight
        {
            get => transferredWeight;
            set => SetPropertyValue(nameof(TransferredWeight), ref transferredWeight, value);
        }


    }
}