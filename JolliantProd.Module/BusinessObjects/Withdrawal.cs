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
using AggregatedAttribute = DevExpress.Xpo.AggregatedAttribute;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Withdrawal : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Withdrawal(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            RequestedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
        }


        StatusEnum status;
        string transferredBy;
        string requestedBy;
        WarehouseLocation location;
        string seriesName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SeriesName
        {
            get => seriesName;
            set => SetPropertyValue(nameof(SeriesName), ref seriesName, value);
        }


        public WarehouseLocation Location
        {
            get => location;
            set
            {
                SetPropertyValue(nameof(Location), ref location, value);
                if (!IsLoading && !IsSaving)
                {
                    SeriesName = Location.DisplayName + "/" + "WITHDRAW/" + Location.NextWithdrawal;
                    Location.NextWithdrawal += 1;
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RequestedBy
        {
            get => requestedBy;
            set => SetPropertyValue(nameof(RequestedBy), ref requestedBy, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TransferredBy
        {
            get => transferredBy;
            set => SetPropertyValue(nameof(TransferredBy), ref transferredBy, value);
        }

        [Association("Withdrawal-WithdrawalLines"), Aggregated()]
        public XPCollection<WithdrawalLine> WithdrawalLines
        {
            get
            {
                return GetCollection<WithdrawalLine>(nameof(WithdrawalLines));
            }
        }

        public enum StatusEnum
        {
            New,
            Validated,
            Cancelled
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }
    }

    public class WithdrawalLine : BaseObject
    {
        public WithdrawalLine(Session session) : base(session)
        { }


        UnitOfMeasure uOM;
        [Persistent(nameof(ProcessedQuantity))]
        double processedQuantity;

        double demandQuantity;
        Product product;
        Withdrawal withdrawal;

        [Association("Withdrawal-WithdrawalLines")]
        public Withdrawal Withdrawal
        {
            get => withdrawal;
            set => SetPropertyValue(nameof(Withdrawal), ref withdrawal, value);
        }


        public Product Product
        {
            get => product;
            set { SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving)
                {
                    UOM = Product.UOM;
                    //Set Default Lots
                }
            }
        }


        public double DemandQuantity
        {
            get => demandQuantity;
            set => SetPropertyValue(nameof(DemandQuantity), ref demandQuantity, value);
        }


        [PersistentAlias(nameof(processedQuantity))]
        public double ProcessedQuantity
        {
            get {
                processedQuantity = WithdrawalLineLots.Select(x => x.DoneQuantity).Sum();
                return processedQuantity; }
        }

        
        public UnitOfMeasure UOM
        {
            get => uOM;
            set => SetPropertyValue(nameof(UOM), ref uOM, value);
        }

        [Association("WithdrawalLine-WithdrawalLineLots"), Aggregated()]
        public XPCollection<WithdrawalLineLot> WithdrawalLineLots
        {
            get
            {
                return GetCollection<WithdrawalLineLot>(nameof(WithdrawalLineLots));
            }
        }
    }


    [RuleCombinationOfPropertiesIsUnique("UniqueWithdrawalLot", DefaultContexts.Save, "Lot, WithdrawalLine")]
    public class WithdrawalLineLot : BaseObject
    {
        public WithdrawalLineLot(Session session) : base(session)
        { }


        double doneQuantity;
        Lot lot;
        WithdrawalLine withdrawalLine;

        [Association("WithdrawalLine-WithdrawalLineLots")]
        public WithdrawalLine WithdrawalLine
        {
            get => withdrawalLine;
            set => SetPropertyValue(nameof(WithdrawalLine), ref withdrawalLine, value);
        }


        public Lot Lot
        {
            get => lot;
            set => SetPropertyValue(nameof(Lot), ref lot, value);
        }

        
        public double DoneQuantity
        {
            get => doneQuantity;
            set => SetPropertyValue(nameof(DoneQuantity), ref doneQuantity, value);
        }
    }
}