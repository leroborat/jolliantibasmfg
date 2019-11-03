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
    public class AuditUsageReport : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AuditUsageReport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        DateTime toDate;
        DateTime fromDate;

        [RuleRequiredField()]
        public DateTime FromDate
        {
            get => fromDate;
            set => SetPropertyValue(nameof(FromDate), ref fromDate, value);
        }

        
        [RuleRequiredField()]
        public DateTime ToDate
        {
            get => toDate;
            set => SetPropertyValue(nameof(ToDate), ref toDate, value);
        }

        [Association("AuditUsageReport-AuditUsageLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<AuditUsageLine> AuditUsageLines
        {
            get
            {
                return GetCollection<AuditUsageLine>(nameof(AuditUsageLines));
            }
        }


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    public class AuditUsageLine : BaseObject
    {
        public AuditUsageLine(Session session) : base(session)
        { }


        AuditDataItemPersistent aDIP;
        AuditUsageReport auditUsageReport;

        [Association("AuditUsageReport-AuditUsageLines")]
        public AuditUsageReport AuditUsageReport
        {
            get => auditUsageReport;
            set => SetPropertyValue(nameof(AuditUsageReport), ref auditUsageReport, value);
        }

        
        public AuditDataItemPersistent ADIP
        {
            get => aDIP;
            set => SetPropertyValue(nameof(ADIP), ref aDIP, value);
        }


    }
}