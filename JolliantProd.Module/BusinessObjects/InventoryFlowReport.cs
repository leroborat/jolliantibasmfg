using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventoryFlowReport : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventoryFlowReport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        WarehouseLocation location;
        DateTime date;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        [XafDisplayName("Inventory Check Reason")]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [RuleRequiredField()]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        [Association("InventoryFlowReport-InventoryFlowLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<InventoryFlowLine> InventoryFlowLines
        {
            get
            {
                return GetCollection<InventoryFlowLine>(nameof(InventoryFlowLines));
            }
        }

        double GetInflowQuantity(Product product)
        {

            return new XPQuery<StockTransfer>(Session)
                .Where(x => x.Date.Date == Date.Date)
                .Where(x => x.Product == product)
                .Where(x => x.DestinationLocation == Location)
                .Select(x => x.Quantity).Sum();

        }

        double GetOutflowQuantity(Product item)
        {
            return new XPQuery<StockTransfer>(Session)
                .Where(x => x.Date.Date == Date.Date)
                .Where(x => x.Product == item)
                .Where(x => x.SourceLocation == Location)
                .Select(x => x.Quantity).Sum();
        }


        double GetStockOnHand(Product item)
        {
            var totalIn =  new XPQuery<StockTransfer>(Session)
               .Where(x => x.Date.Date <= Date.Date)
               .Where(x => x.Product == item)
               .Where(x => x.DestinationLocation == Location)
               .Select(x => x.Quantity).Sum();

            var totalOut = new XPQuery<StockTransfer>(Session)
               .Where(x => x.Date.Date <= Date.Date)
               .Where(x => x.Product == item)
               .Where(x => x.SourceLocation == Location)
               .Select(x => x.Quantity).Sum();

            return totalIn - totalOut;

        }


        [Action(Caption = "Generate Report", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void GenerateReportActionMethod()
        {
            while (InventoryFlowLines.Count > 0)
            {
                InventoryFlowLines.Remove(InventoryFlowLines[0]);
            }

            var myItems = new XPQuery<StockTransfer>(Session)
                .Where(x => x.Date.Date == Date.Date)
                .Where(x => x.DestinationLocation == Location || x.SourceLocation == Location)
                .Select(x => x.Product)
                .Distinct();

            foreach (var item in myItems)
            {
                Console.WriteLine(item);
                InventoryFlowLine ifl = new InventoryFlowLine(Session);
                ifl.Product = item;
                ifl.Inflow = GetInflowQuantity(item);
                ifl.Outflow = GetOutflowQuantity(item);
                ifl.StockOnHand = GetStockOnHand(item);
                InventoryFlowLines.Add(ifl);
            }

   
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).

        }


    }

    public class InventoryFlowLine : BaseObject
    {
        public InventoryFlowLine(Session session) : base(session)
        { }


        InventoryFlowReport inventoryFlowReport;
        double stockOnHand;
        double outflow;
        double inflow;
        Product product;

        
        [Association("InventoryFlowReport-InventoryFlowLines")]
        public InventoryFlowReport InventoryFlowReport
        {
            get => inventoryFlowReport;
            set => SetPropertyValue(nameof(InventoryFlowReport), ref inventoryFlowReport, value);
        }

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public double Inflow
        {
            get => inflow;
            set => SetPropertyValue(nameof(Inflow), ref inflow, value);
        }


        public double Outflow
        {
            get => outflow;
            set => SetPropertyValue(nameof(Outflow), ref outflow, value);
        }

        
        public double StockOnHand
        {
            get => stockOnHand;
            set => SetPropertyValue(nameof(StockOnHand), ref stockOnHand, value);
        }


    }
}