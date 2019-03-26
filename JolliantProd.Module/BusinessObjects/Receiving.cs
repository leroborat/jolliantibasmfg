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
    public class Receiving : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Receiving(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        StatusEnum status;
        string series;
        string processedBy;
        WarehouseLocation storageLocation;
        Vendor vendor;
        DateTime actualDeliveryDate;
        DateTime expectedDeliveryDate;
        PurchaseOrder purchaseOrder;

        [Association("PurchaseOrder-Receivings")]
        public PurchaseOrder PurchaseOrder
        {
            get => purchaseOrder;
            set {
                SetPropertyValue(nameof(PurchaseOrder), ref purchaseOrder, value);
                if (!IsLoading && !IsSaving)
                {
                    Vendor = PurchaseOrder.Vendor;
                    ExpectedDeliveryDate = PurchaseOrder.DeliveryDate;
                    if (StorageLocation == null)
                    {
                        StorageLocation = PurchaseOrder?.DeliveryLocation;
                    }
                    
                    foreach (PurchaseOrderLine item in PurchaseOrder.PurchaseOrderLines)
                    {
                        ReceivedLine rl = new ReceivedLine(Session);
                        rl.Receiving = this;
                        if (item.ReceivedQuantity < item.Quantity)
                        {
                            
                            rl.Product = item.Product;
                            rl.Demand = item.Quantity - item.ReceivedQuantity;
                            ReceivedLines.Add(rl);
                        }
                    }
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Series
        {
            get => series;
            set => SetPropertyValue(nameof(Series), ref series, value);
        }

        [RuleRequiredField()]
        public Vendor Vendor
        {
            get => vendor;
            set => SetPropertyValue(nameof(Vendor), ref vendor, value);
        }


        public DateTime ExpectedDeliveryDate
        {
            get => expectedDeliveryDate;
            set => SetPropertyValue(nameof(ExpectedDeliveryDate), ref expectedDeliveryDate, value);
        }

        [RuleRequiredField()]
        public DateTime ActualDeliveryDate
        {
            get => actualDeliveryDate;
            set => SetPropertyValue(nameof(ActualDeliveryDate), ref actualDeliveryDate, value);
        }

        [RuleRequiredField()]
        public WarehouseLocation StorageLocation
        {
            get => storageLocation;
            set
            {
                SetPropertyValue(nameof(StorageLocation), ref storageLocation, value);
                if (!IsLoading && !IsSaving)
                {
                    try
                    {
                        StorageLocation.NextIn += 1;
                        Series = StorageLocation.Warehouse.WarehouseName + "-IN-" + StorageLocation.NextIn;
                    }
                    catch (Exception)
                    {
                        
                    }
                    

                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProcessedBy
        {
            get => processedBy;
            set => SetPropertyValue(nameof(ProcessedBy), ref processedBy, value);
        }

        [Association("Receiving-ReceivedLines"), Aggregated()]
        public XPCollection<ReceivedLine> ReceivedLines
        {
            get
            {
                return GetCollection<ReceivedLine>(nameof(ReceivedLines));
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

    public class ReceivedLine : BaseObject
    {
        public ReceivedLine(Session session) : base(session)
        { }

        
        [Association("Receiving-ReceivedLines")]
        public Receiving Receiving
        {
            get => receiving;
            set => SetPropertyValue(nameof(Receiving), ref receiving, value);
        }

        string vendorLotCode;
        DateTime lotExpiry;
        double stockingQuantityReceived;
        UnitOfMeasure storageUOM;
        double purchaseQuantityReceived;
        Lot lot;
        UnitOfMeasure purchaseUOM;
        double demand;
        Receiving receiving;
        Product product;
        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set
            {
                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsLoading && !IsSaving)
                {
                    PurchaseUOM = Product.PurchaseUOM;
                    StorageUOM = Product.UOM;
                    if (Product.Tracking != Product.TrackingEnum.NoTracking)
                    {
                        Lot lot = new Lot(Session);
                        lot.InternalReference = Receiving?.Series;
                        lot.LotCode = Product?.InternalReference + "-" + DateTime.Now.ToString("MMddyyyy") +
                            "-" + Receiving?.Vendor?.VendorName + "-" + Receiving?.Vendor?.NextIn;
                        try
                        {
                            Receiving.Vendor.NextIn += 1;
                        }
                        catch (Exception)
                        { }
                        lot.LotCode = (lot.LotCode.Replace(" ", string.Empty)).ToUpper();
                        lot.Product = Product;
                        Lot = lot;
                    }
                }
            }
        }


        public double Demand
        {
            get => demand;
            set => SetPropertyValue(nameof(Demand), ref demand, value);
        }


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


        public DateTime LotExpiry
        {
            get => lotExpiry;
            set => SetPropertyValue(nameof(LotExpiry), ref lotExpiry, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VendorLotCode
        {
            get => vendorLotCode;
            set => SetPropertyValue(nameof(VendorLotCode), ref vendorLotCode, value);
        }


        public double PurchaseQuantityReceived
        {
            get => purchaseQuantityReceived;
            set
            {
                SetPropertyValue(nameof(PurchaseQuantityReceived), ref purchaseQuantityReceived, value);
                if (!IsLoading && !IsSaving)
                {
                    if (StorageUOM == PurchaseUOM.ReferenceMeasure)
                    {
                        StockingQuantityReceived = PurchaseQuantityReceived * PurchaseUOM.Ratio;
                    }
                }
            }
        }


        
        public double StockingQuantityReceived
        {
            get => stockingQuantityReceived;
            set => SetPropertyValue(nameof(StockingQuantityReceived), ref stockingQuantityReceived, value);
        }


        public UnitOfMeasure StorageUOM
        {
            get => storageUOM;
            set => SetPropertyValue(nameof(StorageUOM), ref storageUOM, value);
        }





    }
}