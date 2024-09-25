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
    public class WorkOrder : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public WorkOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        StatusEnum status;
        WorkCenter workCenter;
        DateTime plannedDateTo;
        DateTime actualEndDate;
        DateTime actualStartDate;
        DateTime plannedDate;
        ManufacturingOrder manufacturingOrder;

        [Association("ManufacturingOrder-WorkOrders")]
        public ManufacturingOrder ManufacturingOrder
        {
            get => manufacturingOrder;
            set => SetPropertyValue(nameof(ManufacturingOrder), ref manufacturingOrder, value);
        }


        public DateTime PlannedDate
        {
            get => plannedDate;
            set => SetPropertyValue(nameof(PlannedDate), ref plannedDate, value);
        }


        public DateTime PlannedDateTo
        {
            get => plannedDateTo;
            set => SetPropertyValue(nameof(PlannedDateTo), ref plannedDateTo, value);
        }


        public DateTime ActualStartDate
        {
            get => actualStartDate;
            set => SetPropertyValue(nameof(ActualStartDate), ref actualStartDate, value);
        }


        public DateTime ActualEndDate
        {
            get => actualEndDate;
            set => SetPropertyValue(nameof(ActualEndDate), ref actualEndDate, value);
        }


        public WorkCenter WorkCenter
        {
            get => workCenter;
            set => SetPropertyValue(nameof(WorkCenter), ref workCenter, value);
        }

        public enum StatusEnum
        {
            Ready,
            Finished,
            Cancelled
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }



    }
}