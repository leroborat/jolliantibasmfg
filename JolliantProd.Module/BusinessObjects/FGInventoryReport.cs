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

    public class FGInventoryReport : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FGInventoryReport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        DateTime date;
        Product product;
        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [Association("FGInventoryReport-FGInventoryLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<FGInventoryLine> FGInventoryReportLines
        {
            get
            {
                return GetCollection<FGInventoryLine>(nameof(FGInventoryReportLines));
            }
        }

        [Action(Caption = "Generate Report", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void ActionMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            var lotList = new XPQuery<StockTransfer>(Session)
                .Where(x => x.Product.CanBeSold == true)
                .Where(x => x.Product == Product)
                .Where(x => x.Date.Date == Date.Date)
                .Where(x => x.SourceLocation.DisplayName == "Taytay/Stock" || x.DestinationLocation.DisplayName == "Taytay/Stock")
                .Select(x => x.Lot).Distinct();

            foreach (var item in lotList)
            {
                var y = new FGInventoryLine(Session);
                y.FGInventoryReport = this;
                y.Lot = item;

            }

            Session.Save(this);

        }
    }

    public class FGInventoryLine : BaseObject
    {
        public FGInventoryLine(Session session) : base(session)
        { }


        FGInventoryReport fGInventoryReport;
        [Persistent(nameof(StockOnHand))]
        double stockOnHand;
        double withdrawQuantity;
        double receivedQuantity;
        double beginningQuantity;
        Lot lot;

        public Lot Lot
        {
            get => lot;
            set { SetPropertyValue(nameof(Lot), ref lot, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                 
                    

                    var PreviousQuantityReceived = new XPQuery<StockTransfer>(Session)
                    .Where(x => x.Product.CanBeSold == true)
                    .Where(x => x.Product == FGInventoryReport.Product)
                    .Where(x => x.Date.Date < FGInventoryReport.Date.Date)
                    .Where(x => x.DestinationLocation.DisplayName == "Taytay/Stock")
                    .Where(x => x.Lot == Lot)
                    .Select(x => x.Quantity).Sum();

                    var PreviousWithdrawnQuantity = new XPQuery<StockTransfer>(Session)
                    .Where(x => x.Product.CanBeSold == true)
                    .Where(x => x.Product == FGInventoryReport.Product)
                    .Where(x => x.Date.Date < FGInventoryReport.Date.Date)
                    .Where(x => x.DestinationLocation.DisplayName == "Taytay/Stock")
                    .Where(x => x.Lot == Lot)
                    .Select(x => x.Quantity).Sum();

                    BeginningQuantity = PreviousQuantityReceived - PreviousWithdrawnQuantity;

                    ReceivedQuantity = new XPQuery<StockTransfer>(Session)
                    .Where(x => x.Product.CanBeSold == true)
                    .Where(x => x.Product == FGInventoryReport.Product)
                    .Where(x => x.Date.Date == FGInventoryReport.Date.Date)
                    .Where(x => x.DestinationLocation.DisplayName == "Taytay/Stock")
                    .Where(x => x.Lot == Lot)
                    .Select(x => x.Quantity).Sum();

                    WithdrawQuantity = new XPQuery<StockTransfer>(Session)
                   .Where(x => x.Product.CanBeSold == true)
                   .Where(x => x.Product == FGInventoryReport.Product)
                   .Where(x => x.Date.Date == FGInventoryReport.Date.Date)
                   .Where(x => x.SourceLocation.DisplayName == "Taytay/Stock")
                   .Where(x => x.Lot == Lot)
                   .Select(x => x.Quantity).Sum();
                }
            }
        }


        public double BeginningQuantity
        {
            get => beginningQuantity;
            set => SetPropertyValue(nameof(BeginningQuantity), ref beginningQuantity, value);
        }



        public double ReceivedQuantity
        {
            get => receivedQuantity;
            set => SetPropertyValue(nameof(ReceivedQuantity), ref receivedQuantity, value);
        }


        public double WithdrawQuantity
        {
            get => withdrawQuantity;
            set => SetPropertyValue(nameof(WithdrawQuantity), ref withdrawQuantity, value);
        }


        [PersistentAlias(nameof(stockOnHand))]
        public double StockOnHand
        {
            get
            {
                stockOnHand = BeginningQuantity +  ReceivedQuantity - WithdrawQuantity;
                return stockOnHand;
            }
        }

        
        [Association("FGInventoryReport-FGInventoryLines")]
        public FGInventoryReport FGInventoryReport
        {
            get => fGInventoryReport;
            set => SetPropertyValue(nameof(FGInventoryReport), ref fGInventoryReport, value);
        }

    }
}