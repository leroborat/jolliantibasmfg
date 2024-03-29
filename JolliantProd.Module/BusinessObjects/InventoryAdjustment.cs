﻿using System;
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
using System.Diagnostics;

namespace JolliantProd.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class InventoryAdjustment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventoryAdjustment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            InventoryDate = DateTime.Now;
            AdjustmentType = AdjustmentTypeEnum.SingleProduct;
            Status = StatusEnum.Draft;
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
        }


        string createdBy;
        DateTime createdDate;
        AdjustmentTypeEnum adjustmentType;
        WarehouseLocation inventoriedLocation;
        DateTime inventoryDate;
        string inventoryReferenceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize), RuleRequiredField()]
        public string InventoryReferenceName
        {
            get => inventoryReferenceName;
            set => SetPropertyValue(nameof(InventoryReferenceName), ref inventoryReferenceName, value);
        }

        public enum StatusEnum
        {
            Draft,
            Validated
        }

        StatusEnum status;
        [RuleRequiredField()]
        public StatusEnum Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }



        public enum AdjustmentTypeEnum
        {
            AllProducts,
            SingleProduct
        }

        [RuleRequiredField()]
        public AdjustmentTypeEnum AdjustmentType
        {
            get => adjustmentType;
            set => SetPropertyValue(nameof(AdjustmentType), ref adjustmentType, value);
        }

        [RuleRequiredField()]
        public DateTime InventoryDate
        {
            get => inventoryDate;
            set => SetPropertyValue(nameof(InventoryDate), ref inventoryDate, value);
        }


        public DateTime CreatedDate
        {
            get => createdDate;
            set => SetPropertyValue(nameof(CreatedDate), ref createdDate, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CreatedBy
        {
            get => createdBy;
            set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value);
        }



        public WarehouseLocation InventoriedLocation
        {
            get => inventoriedLocation;
            set {

                SetPropertyValue(nameof(InventoriedLocation), ref inventoriedLocation, value);
                if (!IsSaving && !IsLoading && AdjustmentType == AdjustmentTypeEnum.AllProducts)
                {
                    Session.Delete(InventoryAdjustmentLines);
                    // Clear Lines
                    // Get Distinct Products
                    var distinctProducts = new XPQuery<StockTransfer>(Session).Where(x => x.DestinationLocation == InventoriedLocation)
                        .Select(x => x.Product).Distinct();

                    foreach (Product item in distinctProducts)
                    {
                        Debug.WriteLine(item.ProductName);
                        // Query Stock Transfer by product destination = location
                        
                        if (item.Tracking == Product.TrackingEnum.NoTracking)
                        {
                            var TotalIn = new XPQuery<StockTransfer>(Session)
                                .Where(x => x.Product == item && x.DestinationLocation == InventoriedLocation)
                                .Select(x => x.Quantity).Sum();
                            var TotalOut = new XPQuery<StockTransfer>(Session)
                                .Where(x => x.Product == item && x.SourceLocation == InventoriedLocation)
                                .Select(x => x.Quantity).Sum();
                            var balance = TotalIn - TotalOut;
                            if (balance != 0)
                            {
                                InventoryAdjustmentLine ial = new InventoryAdjustmentLine(Session);
                                ial.Product = item;
                                ial.TheoreticalQuantity = balance;
                                InventoryAdjustmentLines.Add(ial);
                            }
                        }
                        else if (item.Tracking == Product.TrackingEnum.TrackByLot)
                        {

                            var DistinctLot = new XPQuery<StockTransfer>(Session)
                                .Where(x => x.Product == item && x.DestinationLocation == InventoriedLocation)
                                .Select(x => x.Lot)
                                .Where(y => y.StockOnHand > 0)
                                .Distinct();

                            foreach (Lot lot in DistinctLot)
                            {
                               

                                var TotalIn = new XPQuery<StockTransfer>(Session)
                                    .Where(x => x.Product == item && x.DestinationLocation == InventoriedLocation)
                                    .Where(x => x.Lot == lot)
                                    .Select(x => x.Quantity).Sum();

                                var TotalOut = new XPQuery<StockTransfer>(Session)
                                    .Where(x => x.Product == item && x.SourceLocation == InventoriedLocation)
                                    .Where(x => x.Lot == lot)
                                    .Select(x => x.Quantity).Sum();

                                var balance = TotalIn - TotalOut;

                                if (balance != 0)
                                {
                                    InventoryAdjustmentLine ial = new InventoryAdjustmentLine(Session);
                                    ial.Product = item;
                                    ial.TheoreticalQuantity = balance;
                                    ial.LotNumber = lot;
                                    InventoryAdjustmentLines.Add(ial);
                                }
                            }
                        }

                        // if Product is tracked by Lot query distinct lot
                        // Sub Query by lot source and destination
                        // Add Product if more than 0
                        //If not tracked by lot, add product
                    }

                }

            }


        }

        [Association("InventoryAdjustment-InventoryAdjustmentLines"), DevExpress.Xpo.Aggregated()]
        public XPCollection<InventoryAdjustmentLine> InventoryAdjustmentLines
        {
            get
            {
                return GetCollection<InventoryAdjustmentLine>(nameof(InventoryAdjustmentLines));
            }
        }
    }

    public class InventoryAdjustmentLine : BaseObject
    {
        public InventoryAdjustmentLine(Session session) : base(session)
        { }



        double theoreticalQuantity;
        double realQuantity;

        Lot lotNumber;
        Product product;
        InventoryAdjustment inventoryAdjustment;

        [Association("InventoryAdjustment-InventoryAdjustmentLines")]
        public InventoryAdjustment InventoryAdjustment
        {
            get => inventoryAdjustment;
            set => SetPropertyValue(nameof(InventoryAdjustment), ref inventoryAdjustment, value);
        }

        [RuleRequiredField()]
        public Product Product
        {
            get => product;
            set {

                SetPropertyValue(nameof(Product), ref product, value);
                if (!IsSaving && !IsLoading && InventoryAdjustment != null )
                {
                    if (InventoryAdjustment.AdjustmentType == InventoryAdjustment.AdjustmentTypeEnum.SingleProduct)
                    {
                        Debug.WriteLine("I Shouldn't be here");
                        if (Product.Tracking == Product.TrackingEnum.NoTracking)
                        {
                            var stList = new XPQuery<StockTransfer>(Session);
                            var stockTransfersDest = from p in stList
                                                     where p.Product == Product &&
                                                     p.DestinationLocation == InventoryAdjustment.InventoriedLocation
                                                     select p;
                            // Query Stock Transfer by source location

                            var stockTransfersSource = from p in stList
                                                       where p.Product == Product &&
                                                       p.SourceLocation == InventoryAdjustment.InventoriedLocation
                                                       select p;

                            var TotalIn = stockTransfersDest.Sum(x => x.Quantity);
                            var TotalOut = stockTransfersSource.Sum(x => x.Quantity);
                            TheoreticalQuantity = TotalIn - TotalOut;
                        }
                    }
                }            
            }
        }

        [RuleRequiredField()]
        public Lot LotNumber
        {
            get => lotNumber;
            set {
                SetPropertyValue(nameof(LotNumber), ref lotNumber, value);
                if (!IsSaving && !IsLoading && InventoryAdjustment != null)
                {
                    if (InventoryAdjustment.AdjustmentType == InventoryAdjustment.AdjustmentTypeEnum.SingleProduct)
                    {
                        var stList = new XPQuery<StockTransfer>(Session);
                        var stockTransfersDest = from p in stList
                                                 where p.Product == Product &&
                                                 p.DestinationLocation == InventoryAdjustment.InventoriedLocation
                                                 select p;
                        // Query Stock Transfer by source location

                        var stockTransfersSource = from p in stList
                                                   where p.Product == Product &&
                                                   p.SourceLocation == InventoryAdjustment.InventoriedLocation
                                                   select p;

                        var LotInList = from a in stockTransfersDest
                                        where a.Lot == LotNumber
                                        select a;

                        var TotalIn = LotInList.Sum(x => x.Quantity);

                        var LotOutList = from a in stockTransfersSource
                                         where a.Lot == LotNumber
                                         select a;

                        var TotalOut = LotOutList.Sum(x => x.Quantity);
                        TheoreticalQuantity = TotalIn - TotalOut;
                    }
                }
            }
        }



        
        public double TheoreticalQuantity
        {
            get => theoreticalQuantity;
            set => SetPropertyValue(nameof(TheoreticalQuantity), ref theoreticalQuantity, value);
        }



        public double RealQuantity
        {
            get => realQuantity;
            set => SetPropertyValue(nameof(RealQuantity), ref realQuantity, value);
        }


    }
}