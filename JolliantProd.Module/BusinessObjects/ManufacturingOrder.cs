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
    public class ManufacturingOrder : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ManufacturingOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }


        [RuleRequiredField()]
        public Company Company
        {
            get => company;
            set { SetPropertyValue(nameof(Company), ref company, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    SeriesName = "MO-" + (Company.NextMONumber + 1).ToString();
                    Company.NextMONumber += 1;
                    Session.Save(Company);
                }
            }
        }
        DateTime productionStartDateTime;
        StatusEnum status;
        ProductionRoute routing;
        BillOfMaterial billOfMaterial;
        double quantityToProduce;
        Product product;
        Company company;
        string seriesName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SeriesName
        {
            get => seriesName;
            set => SetPropertyValue(nameof(SeriesName), ref seriesName, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public DateTime ProductionStartDateTime
        {
            get => productionStartDateTime;
            set => SetPropertyValue(nameof(ProductionStartDateTime), ref productionStartDateTime, value);
        }

        [RuleRequiredField()]
        public double QuantityToProduce
        {
            get => quantityToProduce;
            set => SetPropertyValue(nameof(QuantityToProduce), ref quantityToProduce, value);
        }

        [RuleRequiredField()]
        public BillOfMaterial BillOfMaterial
        {
            get => billOfMaterial;
            set
            {
                SetPropertyValue(nameof(BillOfMaterial), ref billOfMaterial, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    Routing = BillOfMaterial.ProductionRoute;
                }
            }
        }


        public ProductionRoute Routing
        {
            get => routing;
            set => SetPropertyValue(nameof(Routing), ref routing, value);
        }

        [Association("ManufacturingOrder-ConsumedMaterials"), DevExpress.Xpo.Aggregated()]
        public XPCollection<ConsumedMaterial> ConsumedMaterials
        {
            get
            {
                return GetCollection<ConsumedMaterial>(nameof(ConsumedMaterials));
            }
        }

        public enum StatusEnum
        {
            Draft,
            Planned,
            InProgress,
            Done
        }

        
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("ManufacturingOrder-FinishedGoods"), Aggregated()]
        public XPCollection<FinishedGood> FinishedGoods
        {
            get
            {
                return GetCollection<FinishedGood>(nameof(FinishedGoods));
            }
        }

        [Association("ManufacturingOrder-WorkOrders")]
        public XPCollection<WorkOrder> WorkOrders
        {
            get
            {
                return GetCollection<WorkOrder>(nameof(WorkOrders));
            }
        }

        [Association("ManufacturingOrder-WithdrawalRequests"), Aggregated()]
        public XPCollection<WithdrawalRequest> WithdrawalRequests
        {
            get
            {
                return GetCollection<WithdrawalRequest>(nameof(WithdrawalRequests));
            }
        }
    }

    public class ConsumedMaterial : BaseObject
    {
        public ConsumedMaterial(Session session) : base(session)
        { }


        
        [Association("ManufacturingOrder-ConsumedMaterials")]
        public ManufacturingOrder ManufacturingOrder
        {
            get => manufacturingOrder;
            set => SetPropertyValue(nameof(ManufacturingOrder), ref manufacturingOrder, value);
        }
        [Persistent(nameof(QuantityAvailable))]
        double quantityAvailable;
        double consumed;
        double toConsume;
        UnitOfMeasure unitOfMeasure;
        ManufacturingOrder manufacturingOrder;
        Product product;

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public UnitOfMeasure UnitOfMeasure
        {
            get => unitOfMeasure;
            set => SetPropertyValue(nameof(UnitOfMeasure), ref unitOfMeasure, value);
        }

        
        [PersistentAlias(nameof(quantityAvailable))]
        public double QuantityAvailable
        {
            get { return quantityAvailable; }
        }
        


        public double ToConsume
        {
            get => toConsume;
            set => SetPropertyValue(nameof(ToConsume), ref toConsume, value);
        }

        
        public double Consumed
        {
            get => consumed;
            set => SetPropertyValue(nameof(Consumed), ref consumed, value);
        }

    }

    public class FinishedGood : BaseObject
    {
        public FinishedGood(Session session) : base(session)
        { }


        double quantity;
        string lotCode;
        UnitOfMeasure unitOfMeasure;
        Product product;
        ManufacturingOrder manufacturingOrder;

        [Association("ManufacturingOrder-FinishedGoods")]
        public ManufacturingOrder ManufacturingOrder
        {
            get => manufacturingOrder;
            set => SetPropertyValue(nameof(ManufacturingOrder), ref manufacturingOrder, value);
        }


        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public UnitOfMeasure UnitOfMeasure
        {
            get => unitOfMeasure;
            set => SetPropertyValue(nameof(UnitOfMeasure), ref unitOfMeasure, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LotCode
        {
            get => lotCode;
            set => SetPropertyValue(nameof(LotCode), ref lotCode, value);
        }

        
        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

    }
}