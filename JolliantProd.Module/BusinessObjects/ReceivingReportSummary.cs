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
    public class ReceivingReportSummary : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReceivingReportSummary(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        DateTime to;
        DateTime from;

        [RuleRequiredField()]
        public DateTime From
        {
            get => from;
            set => SetPropertyValue(nameof(From), ref from, value);
        }


        [RuleRequiredField()]
        public DateTime To
        {
            get => to;
            set => SetPropertyValue(nameof(To), ref to, value);
        }

        [Association("ReceivingReportSummary-RRSummaryLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<RRSumarryLine> RRSummaryLines
        {
            get
            {
                return GetCollection<RRSumarryLine>(nameof(RRSummaryLines));
            }
        }


        //[Action(Caption = "Generate Report", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void GenerateReport()
        //{
        //    Session.Delete(RRSummaryLines);
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    var myList = new XPQuery<ReceivedLine>(Session).Where(x => x.Receiving.ActualDeliveryDate >= From)
        //        .Where(x => x.Receiving.ActualDeliveryDate <= To)
        //        .Where(x => x.Receiving.Status == Receiving.StatusEnum.Validated);


        //    foreach (var item in myList)
        //    {
        //        var myItem = new RRSumarryLine(Session);
        //        myItem.ReceivingReportSummary = this;
        //        myItem.ReceivedLine = item;
        //    }
        //}
    }

    public class RRSumarryLine : BaseObject
    {
        public RRSumarryLine(Session session) : base(session)
        { }


        decimal pricePerUnit;
        ReceivedLine receivedLine;
        ReceivingReportSummary receivingReportSummary;

        [Association("ReceivingReportSummary-RRSummaryLines")]
        public ReceivingReportSummary ReceivingReportSummary
        {
            get => receivingReportSummary;
            set => SetPropertyValue(nameof(ReceivingReportSummary), ref receivingReportSummary, value);
        }


        public ReceivedLine ReceivedLine
        {
            get => receivedLine;
            set { SetPropertyValue(nameof(ReceivedLine), ref receivedLine, value);
                if (!IsLoading & !IsSaving & !IsDeleted)
                {
                    try
                    {
                        PricePerUnit = ReceivedLine.Receiving.PurchaseOrder.PurchaseOrderLines
                            .Where(x => x.Product == ReceivedLine.Product).First().PricePerUnit;
                    }
                    catch (Exception)
                    {

                        PricePerUnit = 0;
                    }
                    
                }
            }
        }

        
        public decimal PricePerUnit
        {
            get => pricePerUnit;
            set => SetPropertyValue(nameof(PricePerUnit), ref pricePerUnit, value);
        }



    }
}