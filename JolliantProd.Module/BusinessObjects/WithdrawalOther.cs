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
    public class WithdrawalOther : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public WithdrawalOther(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Series = "WDRAWOT-" + (new XPQuery<WithdrawalOther>(Session).Count() + 1 ).ToString();
            Date = DateTime.Now;
            RequestedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        StatusEnum status;
        string processedBy;
        string requestedBy;
        DateTime date;
        WarehouseLocation location;
        string series;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Series
        {
            get => series;
            set => SetPropertyValue(nameof(Series), ref series, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }


        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RequestedBy
        {
            get => requestedBy;
            set => SetPropertyValue(nameof(RequestedBy), ref requestedBy, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProcessedBy
        {
            get => processedBy;
            set => SetPropertyValue(nameof(ProcessedBy), ref processedBy, value);
        }

        public enum StatusEnum
        {
            Draft,
            Submitted,
            Done
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("WithdrawalOther-WithdrawalOtherLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<WithdrawalOtherLine> WithdrwalOtherLines
        {
            get
            {
                return GetCollection<WithdrawalOtherLine>(nameof(WithdrwalOtherLines));
            }
        }

        [Action(Caption = "Submit", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void SubmitActionMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Submitted;
        }

        [Action(Caption = "Validate", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        public void ValidateActionMethod()
        {
            Session.Save(this);
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = StatusEnum.Done;
            var ProdLocation = new XPQuery<WarehouseLocation>(Session)
                .Where(x => x.LocationName == "Production").First();
            foreach (var item in WithdrwalOtherLines)
            {
                var xTransfer = new StockTransfer(Session);
                xTransfer.Date = DateTime.Now;
                xTransfer.DestinationLocation = ProdLocation;
                xTransfer.Product = item.Item;
                xTransfer.Quantity = item.Quantity;
                xTransfer.Reference = "Other Withdrawal: " + Series;
                xTransfer.SourceLocation = Location;
                xTransfer.UOM = item.UOM;
                Session.Save(xTransfer);
            }

            ProcessedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            Session.Save(this);
            
        }
    }

    public class WithdrawalOtherLine : BaseObject
    {
        public WithdrawalOtherLine(Session session) : base(session)
        { }



        UnitOfMeasure uOM;
        [Persistent(nameof(AvailableQuantity))]
        double availableQuantity;
        double quantity;
        Product item;
        WithdrawalOther withdrawalOther;

        [Association("WithdrawalOther-WithdrawalOtherLines")]
        public WithdrawalOther WithdrawalOther
        {
            get => withdrawalOther;
            set => SetPropertyValue(nameof(WithdrawalOther), ref withdrawalOther, value);
        }

        [RuleRequiredField()]
        public Product Item
        {
            get => item;
            set { SetPropertyValue(nameof(Item), ref item, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    if (Item != null)
                    {
                        Quantity = 1;
                        UOM = Item.UOM;
                    }
                    
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


        [PersistentAlias(nameof(availableQuantity))]
        public double AvailableQuantity
        {
            get {
                if (Item != null)
                {
                    availableQuantity = Item.StockOnHand;
                }
                
                return availableQuantity; }
        }
        

    }
}