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
    public class BillOfMaterial : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BillOfMaterial(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ReferenceName = "Standard";
            Quantity = 1;
        }


        ProductionRoute productionRoute;
        double quantity;
        Product product;
        string referenceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string ReferenceName
        {
            get => referenceName;
            set => SetPropertyValue(nameof(ReferenceName), ref referenceName, value);
        }


        [Association("Product-BillOfMaterials")]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

        [RuleRequiredField()]
        public ProductionRoute ProductionRoute
        {
            get => productionRoute;
            set => SetPropertyValue(nameof(ProductionRoute), ref productionRoute, value);
        }

        [Association("BillOfMaterial-BomComponents"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BomComponent> BomComponents
        {
            get
            {
                return GetCollection<BomComponent>(nameof(BomComponents));
            }
        }
    }

    public class BomComponent : BaseObject
    {
        public BomComponent(Session session) : base(session)
        { }


        RouteOperation consumedInRouteOperation;
        UnitOfMeasure productionUOM;
        double quantity;
        Product component;
        BillOfMaterial billOfMaterial;

        [Association("BillOfMaterial-BomComponents")]
        public BillOfMaterial BillOfMaterial
        {
            get => billOfMaterial;
            set => SetPropertyValue(nameof(BillOfMaterial), ref billOfMaterial, value);
        }


        public Product Component
        {
            get => component;
            set { SetPropertyValue(nameof(Component), ref component, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    ProductionUOM = Component.ProductionUOM;
                }
            }
        }


        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


        [RuleRequiredField()]
        public UnitOfMeasure ProductionUOM
        {
            get => productionUOM;
            set => SetPropertyValue(nameof(ProductionUOM), ref productionUOM, value);
        }

        
        
        public RouteOperation ConsumedInRouteOperation
        {
            get => consumedInRouteOperation;
            set => SetPropertyValue(nameof(ConsumedInRouteOperation), ref consumedInRouteOperation, value);
        }

    }
}