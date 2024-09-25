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
    public class AssessmentReport : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AssessmentReport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
        }

        FinalStatusEnum status;
        string remarks;
        string methodOfInspection;
        string assessedBy;
        DateTime date;
        Equipment equipment;

        [Association("Equipment-AssessmentReports")]
        public Equipment Equipment
        {
            get => equipment;
            set => SetPropertyValue(nameof(Equipment), ref equipment, value);
        }


        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AssessedBy
        {
            get => assessedBy;
            set => SetPropertyValue(nameof(AssessedBy), ref assessedBy, value);
        }


        [Size(300)]
        public string MethodOfInspection
        {
            get => methodOfInspection;
            set => SetPropertyValue(nameof(MethodOfInspection), ref methodOfInspection, value);
        }


        [Size(1000)]
        public string Remarks
        {
            get => remarks;
            set => SetPropertyValue(nameof(Remarks), ref remarks, value);
        }

        public enum FinalStatusEnum
        {
            Operational,
            NotOperational
        }

        
        public FinalStatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }



    }
}