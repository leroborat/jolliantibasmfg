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
    public class InventoryReturn : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventoryReturn(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = StatusEnum.Draft;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        StatusEnum status;
        WarehouseLocation returnLocation;
        SalesOrder salesOrder;
        string returnReferenceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string ReturnReferenceName
        {
            get => returnReferenceName;
            set => SetPropertyValue(nameof(ReturnReferenceName), ref returnReferenceName, value);
        }

        public enum StatusEnum
        {
            Draft,
            Done
        }

        [RuleRequiredField()]
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [Association("SalesOrder-InventoryReturns")]
        public SalesOrder SalesOrder
        {
            get => salesOrder;
            set => SetPropertyValue(nameof(SalesOrder), ref salesOrder, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation ReturnLocation
        {
            get => returnLocation;
            set => SetPropertyValue(nameof(ReturnLocation), ref returnLocation, value);
        }

        [Association("InventoryReturn-InventoryReturnLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<InventoryReturnLine> InventoryReturnLines
        {
            get
            {
                return GetCollection<InventoryReturnLine>(nameof(InventoryReturnLines));
            }
        }
    }

    public class InventoryReturnLine : BaseObject
    {
        public InventoryReturnLine(Session session) : base(session)
        { }


        double quantity;
        Lot lotName;
        Product product;
        InventoryReturn inventoryReturn;

        [Association("InventoryReturn-InventoryReturnLines")]
        public InventoryReturn InventoryReturn
        {
            get => inventoryReturn;
            set => SetPropertyValue(nameof(InventoryReturn), ref inventoryReturn, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        [RuleRequiredField()]
        public Lot LotName
        {
            get => lotName;
            set => SetPropertyValue(nameof(LotName), ref lotName, value);
        }

        [RuleRequiredField()]
        public double Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


    }
}