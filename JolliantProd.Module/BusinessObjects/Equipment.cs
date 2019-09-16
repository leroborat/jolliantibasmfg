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
    public class Equipment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Equipment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        decimal cost;
        string serialNumber;
        string model;
        Vendor vendor;
        string equipmentDescription;
        string location;
        DateTime assignedDate;
        string usedByName;
        UsedByEnum usedBy;
        EquipmentCategory equipmentCategory;
        string equipmentName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string EquipmentName
        {
            get => equipmentName;
            set => SetPropertyValue(nameof(EquipmentName), ref equipmentName, value);
        }


        public EquipmentCategory EquipmentCategory
        {
            get => equipmentCategory;
            set => SetPropertyValue(nameof(EquipmentCategory), ref equipmentCategory, value);
        }

        public enum UsedByEnum
        {
            Department,
            Employee,
            Other
        }


        public UsedByEnum UsedBy
        {
            get => usedBy;
            set => SetPropertyValue(nameof(UsedBy), ref usedBy, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string UsedByName
        {
            get => usedByName;
            set => SetPropertyValue(nameof(UsedByName), ref usedByName, value);
        }


        public DateTime AssignedDate
        {
            get => assignedDate;
            set => SetPropertyValue(nameof(AssignedDate), ref assignedDate, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }


        [Size(500)]
        public string EquipmentDescription
        {
            get => equipmentDescription;
            set => SetPropertyValue(nameof(EquipmentDescription), ref equipmentDescription, value);
        }


        public Vendor Vendor
        {
            get => vendor;
            set => SetPropertyValue(nameof(Vendor), ref vendor, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Model
        {
            get => model;
            set => SetPropertyValue(nameof(Model), ref model, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SerialNumber
        {
            get => serialNumber;
            set => SetPropertyValue(nameof(SerialNumber), ref serialNumber, value);
        }

        
        public decimal Cost
        {
            get => cost;
            set => SetPropertyValue(nameof(Cost), ref cost, value);
        }

        [Association("Equipment-MaintenanceRequests")]
        public XPCollection<MaintenanceRequest> MaintenaceRequests
        {
            get
            {
                return GetCollection<MaintenanceRequest>(nameof(MaintenaceRequests));
            }
        }



    }

    [DefaultClassOptions]
    public class EquipmentCategory : BaseObject
    {
        public EquipmentCategory(Session session) : base(session)
        { }


        string categoryName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string CategoryName
        {
            get => categoryName;
            set => SetPropertyValue(nameof(CategoryName), ref categoryName, value);
        }

    }

    [DefaultClassOptions()]
    public class MaintenanceRequest : BaseObject

    {
        public MaintenanceRequest(Session session) : base(session)
        { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            RequestDate = DateTime.Now;
        }


        StageEnum status;
        string internalNotes;
        PriorityEnum priority;
        DateTime scheduledDate;
        TypeEnum type;
        DateTime closeDate;
        string responsible;
        DateTime requestDate;
        string createdBy;
        Equipment equipment;
        string descriptionName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string DescriptionName
        {
            get => descriptionName;
            set => SetPropertyValue(nameof(DescriptionName), ref descriptionName, value);
        }


        [Association("Equipment-MaintenanceRequests")]
        public Equipment Equipment
        {
            get => equipment;
            set => SetPropertyValue(nameof(Equipment), ref equipment, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CreatedBy
        {
            get => createdBy;
            set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value);
        }


        public DateTime RequestDate
        {
            get => requestDate;
            set => SetPropertyValue(nameof(RequestDate), ref requestDate, value);
        }


        public DateTime ScheduledDate
        {
            get => scheduledDate;
            set => SetPropertyValue(nameof(ScheduledDate), ref scheduledDate, value);
        }


        public DateTime CloseDate
        {
            get => closeDate;
            set => SetPropertyValue(nameof(CloseDate), ref closeDate, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Responsible
        {
            get => responsible;
            set => SetPropertyValue(nameof(Responsible), ref responsible, value);
        }

        public enum TypeEnum
        {
            Corrective,
            Preventive
        }

        [RuleRequiredField()]
        public TypeEnum Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }


        public enum PriorityEnum
        {
            Low,
            Medium,
            High
        }


        public PriorityEnum Priority
        {
            get => priority;
            set => SetPropertyValue(nameof(Priority), ref priority, value);
        }

        public enum StageEnum
        {
            New,
            InProgress,
            Repaired,
            Scrapped
        }

        
        public StageEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [Size(1000)]
        public string InternalNotes
        {
            get => internalNotes;
            set => SetPropertyValue(nameof(InternalNotes), ref internalNotes, value);
        }

    }
}