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
    public class MaterialRequest : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MaterialRequest(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            RequestDate = DateTime.Now;
            RequestedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string lastModifiedBy;
        DateTime lastModifiedDate;
        DateTime deliveryDate;
        Vendor vendor;
        PurchaseOrder purchaseOrder;
        RequestStatusEnum status;
        string note;
        string requestedBy;
        DateTime requestDate;

        public DateTime RequestDate
        {
            get => requestDate;
            set => SetPropertyValue(nameof(RequestDate), ref requestDate, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RequestedBy
        {
            get => requestedBy;
            set => SetPropertyValue(nameof(RequestedBy), ref requestedBy, value);
        }

        [RuleRequiredField()]
        public Vendor Vendor
        {
            get => vendor;
            set => SetPropertyValue(nameof(Vendor), ref vendor, value);
        }

        [RuleRequiredField()]
        public DateTime DeliveryDate
        {
            get => deliveryDate;
            set => SetPropertyValue(nameof(DeliveryDate), ref deliveryDate, value);
        }


        [Size(500)]
        public string Note
        {
            get => note;
            set => SetPropertyValue(nameof(Note), ref note, value);
        }

        public enum RequestStatusEnum
        {
            New,
            Approved,
            Denied
        }


        public RequestStatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        public DateTime LastModifiedDate
        {
            get => lastModifiedDate;
            set => SetPropertyValue(nameof(LastModifiedDate), ref lastModifiedDate, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LastModifiedBy
        {
            get => lastModifiedBy;
            set => SetPropertyValue(nameof(LastModifiedBy), ref lastModifiedBy, value);
        }


        public PurchaseOrder PurchaseOrder
        {
            get => purchaseOrder;
            set => SetPropertyValue(nameof(PurchaseOrder), ref purchaseOrder, value);
        }

        [Association("MaterialRequest-MaterialRequestLines"),Aggregated()]
        public XPCollection<MaterialRequestLine> MaterialRequestLines
        {
            get
            {
                return GetCollection<MaterialRequestLine>(nameof(MaterialRequestLines));
            }
        }
    }

    public class MaterialRequestLine : BaseObject
    {
        public MaterialRequestLine(Session session) : base(session)
        { }


        UnitOfMeasure stockingUOM;
        double stockingQuantity;
        UnitOfMeasure purchaseUOM;
        double quantity;
        Product product;
        MaterialRequest materialRequest;

        [Association("MaterialRequest-MaterialRequestLines")]
        public MaterialRequest MaterialRequest
        {
            get => materialRequest;
            set => SetPropertyValue(nameof(MaterialRequest), ref materialRequest, value);
        }


        public Product Product
        {
            get => product;
            set { SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving)
                {
                    PurchaseUOM = Product.PurchaseUOM;
                    StockingUOM = Product.UOM;
                }
            }
        }


        public double Quantity
        {
            get => quantity;
            set { SetPropertyValue(nameof(Quantity), ref quantity, value);
                if (!IsLoading && !IsSaving)
                {
                    if (StockingUOM == PurchaseUOM.ReferenceMeasure)
                    {
                        StockingQuantity = Quantity * PurchaseUOM.Ratio;
                    } else if (StockingUOM == PurchaseUOM)
                    {
                        StockingQuantity = Quantity;
                    }
                }
            }
        }


        public UnitOfMeasure PurchaseUOM
        {
            get => purchaseUOM;
            set => SetPropertyValue(nameof(PurchaseUOM), ref purchaseUOM, value);
        }

        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.GreaterThan, "0")]
        public double StockingQuantity
        {
            get => stockingQuantity;
            set => SetPropertyValue(nameof(StockingQuantity), ref stockingQuantity, value);
        }

        
        public UnitOfMeasure StockingUOM
        {
            get => stockingUOM;
            set => SetPropertyValue(nameof(StockingUOM), ref stockingUOM, value);
        }
    }
}