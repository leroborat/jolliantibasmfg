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
    public class ReceivingReturn : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReceivingReturn(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ReturnDate = DateTime.Now;
            ProcessedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
        }

        WarehouseLocation fromLocation;
        string processedBy;
        StatusEnum status;
        DateTime returnDate;
        PurchaseOrder purchaseOrder;

        [Association("PurchaseOrder-ReceivingReturns"), RuleRequiredField()]
        public PurchaseOrder PurchaseOrder
        {
            get => purchaseOrder;
            set { SetPropertyValue(nameof(PurchaseOrder), ref purchaseOrder, value);
                if (!IsLoading && !IsSaving)
                {
                    var receivings = PurchaseOrder.Receivings.Where(
                        x => x.Status == Receiving.StatusEnum.Validated
                        );

                    foreach (Receiving item in receivings)
                    {
                        foreach (ReceivedLine receivedLine in item.ReceivedLines)
                        {
                            var rline = new ReceivingReturnLine(Session);
                            rline.ReceivingReturn = this;
                            rline.Product = receivedLine.Product;
                            rline.Quantity = receivedLine.PurchaseQuantityReceived;
                            rline.Lot = receivedLine.Lot;
                        }
                    }
                }
            }
        }


        public DateTime ReturnDate
        {
            get => returnDate;
            set => SetPropertyValue(nameof(ReturnDate), ref returnDate, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation FromLocation
        {
            get => fromLocation;
            set => SetPropertyValue(nameof(FromLocation), ref fromLocation, value);
        }

        [Association("ReceivingReturn-ReceivingReturnLines"), Aggregated()]
        public XPCollection<ReceivingReturnLine> ReceivingReturnLines
        {
            get
            {
                return GetCollection<ReceivingReturnLine>(nameof(ReceivingReturnLines));
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


        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProcessedBy
        {
            get => processedBy;
            set => SetPropertyValue(nameof(ProcessedBy), ref processedBy, value);
        }

    }

    public class ReceivingReturnLine : BaseObject
    {
        public ReceivingReturnLine(Session session) : base(session)
        { }

        double stockingQuantity;
        double quantity;
        Product product;
        ReceivingReturn receivingReturn;

        [Association("ReceivingReturn-ReceivingReturnLines")]
        public ReceivingReturn ReceivingReturn
        {
            get => receivingReturn;
            set => SetPropertyValue(nameof(ReceivingReturn), ref receivingReturn, value);
        }


        public Product Product
        {
            get => product;
            set { SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving)
                {
                    PurchaseUOM = Product.PurchaseUOM;
                    StorageUOM = Product.UOM;
                }
            }
        }


        public double Quantity
        {
            get => quantity;
            set { SetPropertyValue(nameof(Quantity), ref quantity, value);
                if (!IsLoading && !IsSaving)
                {
                    if (StorageUOM == PurchaseUOM.ReferenceMeasure)
                    {
                        StockingQuantity = Quantity * PurchaseUOM.Ratio;
                    } else if (StorageUOM == PurchaseUOM)
                    {
                        StockingQuantity = Quantity;
                    }
                }
            }
        }

        Lot lot;
        UnitOfMeasure purchaseUOM;

        public UnitOfMeasure PurchaseUOM
        {
            get => purchaseUOM;
            set => SetPropertyValue(nameof(PurchaseUOM), ref purchaseUOM, value);
        }

        public Lot Lot
        {
            get => lot;
            set => SetPropertyValue(nameof(Lot), ref lot, value);
        }


        UnitOfMeasure storageUOM;

        
        public double StockingQuantity
        {
            get => stockingQuantity;
            set => SetPropertyValue(nameof(StockingQuantity), ref stockingQuantity, value);
        }


        public UnitOfMeasure StorageUOM
        {
            get => storageUOM;
            set => SetPropertyValue(nameof(StorageUOM), ref storageUOM, value);
        }


    }
}