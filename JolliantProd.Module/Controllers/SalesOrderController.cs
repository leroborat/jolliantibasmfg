using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using JolliantProd.Module.BusinessObjects;

namespace JolliantProd.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SalesOrderController : ViewController
    {
        public SalesOrderController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void SalesOrderController_Activated(object sender, EventArgs e)
        {
            //if (((SalesOrder)View.CurrentObject).Status == SalesOrder.StatusEnum.Confirmed)
            //{
            //    View.AllowEdit["SalesOrderController"] = false;
            //}

        }

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            ((SalesOrder)View.CurrentObject).Status = SalesOrder.StatusEnum.Confirmed;
            //((SalesOrder)View.CurrentObject).Status = SalesOrder.StatusEnum.Confirmed;
            //View.AllowEdit["SalesOrderController"] = false;
        }

        private void simpleAction2_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
           

            Trip trip = ((Trip)View.CurrentObject);
            trip.Save();
            trip.TripStatus = Trip.TripStatusEnum.Validated;

            foreach (TripLine item in trip.TripLines)
            {
                foreach (TripLineDetail tripLineDetail in item.TripLineDetails)
                {
                    StockTransfer stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                    stockTransfer.SourceLocation = tripLineDetail.From;
                    stockTransfer.DestinationLocation = trip.DestinationLocation;
                    stockTransfer.Quantity = tripLineDetail.QuantityDone;
                    stockTransfer.Product = item.Product;
                    if (tripLineDetail.LotCode != null)
                    {
                        stockTransfer.Lot = tripLineDetail.LotCode;
                    }

                    stockTransfer.Reference = "Delivery: " + trip.DisplayName + " " + trip.PONumber;
                    ObjectSpace.CommitChanges();
                }          
            }
        }

        private void ValidateInventoryAdjustmentAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var iaView = ((InventoryAdjustment)View.CurrentObject);
            iaView.Status = InventoryAdjustment.StatusEnum.Validated;
            iaView.Save();
            WarehouseLocation warehouseLocation;
            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Adjustment"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Adjustment";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.InventoryLoss;
                ObjectSpace.CommitChanges();
            }

            foreach (InventoryAdjustmentLine item in iaView.InventoryAdjustmentLines)
            {
                if (item.TheoreticalQuantity < item.RealQuantity)
                {
                    double difference = item.RealQuantity - item.TheoreticalQuantity;
                    var stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                    stockTransfer.Reference = iaView.InventoryReferenceName;
                    stockTransfer.SourceLocation = warehouseLocation;
                    stockTransfer.DestinationLocation = iaView.InventoriedLocation;
                    stockTransfer.Product = item.Product;
                    if (item.LotNumber != null)
                    {
                        stockTransfer.Lot = item.LotNumber;
                    }
                    stockTransfer.Quantity = difference;
                    ObjectSpace.CommitChanges();
                } else if (item.TheoreticalQuantity > item.RealQuantity)
                {
                    double difference = item.TheoreticalQuantity - item.RealQuantity;
                    var stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                    stockTransfer.Reference = iaView.InventoryReferenceName;
                    stockTransfer.SourceLocation = iaView.InventoriedLocation;
                    stockTransfer.DestinationLocation = warehouseLocation;
                    stockTransfer.Product = item.Product;
                    if (item.LotNumber != null)
                    {
                        stockTransfer.Lot = item.LotNumber;
                    }
                    stockTransfer.Quantity = difference;
                    ObjectSpace.CommitChanges();
                }
            }
        }

        private void ValidateInventoryReturnAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Customers"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Customers";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.CustomerLocation;
                ObjectSpace.CommitChanges();
            }
            var thisView = (InventoryReturn)View.CurrentObject;
            thisView.Status = InventoryReturn.StatusEnum.Done;
            foreach (InventoryReturnLine item in thisView.InventoryReturnLines)
            {
                var stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                stockTransfer.Reference = thisView.ReturnReferenceName;
                stockTransfer.SourceLocation = warehouseLocation;
                stockTransfer.DestinationLocation = thisView.ReturnLocation;
                stockTransfer.Product = item.Product;
                if (item.LotName != null)
                {
                    stockTransfer.Lot = item.LotName;
                }
                stockTransfer.Quantity = item.Quantity;
                ObjectSpace.CommitChanges();
            }
        }

        private void ValidateScrapAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var iaView = ((InventoryScrap)View.CurrentObject);
            iaView.Save();
            iaView.Status = InventoryScrap.StatusEnum.Validated;
            WarehouseLocation warehouseLocation;
            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Scrapped"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Scrapped";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.InventoryLoss;
                ObjectSpace.CommitChanges();
            }

            foreach (InventoryScrapLine item in iaView.InventoryScrapLines)
            {
                var stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                stockTransfer.Reference = iaView.ReferenceName;
                stockTransfer.SourceLocation = iaView.InventoryLocation;
                stockTransfer.DestinationLocation = warehouseLocation;
                stockTransfer.Product = item.Product;
                if (item.Lot != null)
                {
                    stockTransfer.Lot = item.Lot;
                }
                stockTransfer.Quantity = item.QuantityToScrap;
                ObjectSpace.CommitChanges();
            }
        }

        private void ValidateInternalTransferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var iaView = ((InternalTransfer)View.CurrentObject);
            iaView.Save();
            iaView.Status = InternalTransfer.StatusEnum.Done;
           

            foreach (InternalTransferLine item in iaView.InternalTransferLines)
            {
                var stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                stockTransfer.Reference = iaView.ReferenceName;
                stockTransfer.SourceLocation = iaView.SourceLocation;
                stockTransfer.DestinationLocation = iaView.DestinationLocation;
                stockTransfer.Product = item.Product;
                if (item.LotNumber != null)
                {
                    stockTransfer.Lot = item.LotNumber;
                }
                stockTransfer.Quantity = item.QuantityDone;
                ObjectSpace.CommitChanges();
            }
        }

        private void GenerateInvoiceLinesAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((Invoice)View.CurrentObject);
            ObjectSpace.Delete(thisView.InvoiceLines);
            ObjectSpace.CommitChanges();
            thisView.Save();
            if (thisView.Trips.Count != 0)
            {
                foreach (Trip item in thisView.Trips)
                {
                    foreach (TripLine tripLine in item.TripLines)
                    {
                        var line = from c in thisView.InvoiceLines
                                   where c.Product == tripLine.Product
                                   select c;

                        InvoiceLine invoiceLine;
                        // Get Sales Information
                        var sline = (from c in tripLine.Trip.SalesOrder.SalesOrderLines
                                     where c.Product == tripLine.Product
                                     select c
                                     ).First();


                        if (line.Count() != 0)
                        {
                            invoiceLine = line.First();
                            invoiceLine.Quantity += tripLine.QuantityDone;
                        } else
                        {
                            invoiceLine = ObjectSpace.CreateObject<InvoiceLine>();
                            invoiceLine.Product = tripLine.Product;
                            invoiceLine.Quantity = tripLine.QuantityDone;
                            invoiceLine.UnitPrice = sline.UnitPrice;
                            invoiceLine.Invoice = tripLine.Trip.Invoice;
                        }

                        

                        ObjectSpace.CommitChanges();
                       
                    }
                }
                thisView.Save();
                ObjectSpace.Refresh();
            }
        }

        private void ValidateInvoiceAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ((Invoice)View.CurrentObject).Status = Invoice.StatusEnum.Open;
            
        }

        private void ValidateCustomerPaymentAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((CustomerPayment)View.CurrentObject);
            thisView.Save();
            thisView.Status = CustomerPayment.StatusEnum.Validated;

            foreach (PaymentAllocationLine item in thisView.PaymentAllocationLines)
            {
                if (item.Invoice.OpenAmount <= 0)
                {
                    item.Invoice.Status = Invoice.StatusEnum.Paid;
                }
            }

            ObjectSpace.CommitChanges();
        }

        private void GenerateTripLinesAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = (TripLine)View.CurrentObject;
            ObjectSpace.Delete(thisView.TripLineDetails);
            ObjectSpace.CommitChanges();
            foreach (Lot item in thisView.Lots)
            {
                TripLineDetail lineDetail = ObjectSpace.CreateObject<TripLineDetail>();
                lineDetail.From = thisView.Location;
                lineDetail.LotCode = item;
                thisView.TripLineDetails.Add(lineDetail);
                ObjectSpace.CommitChanges();
            }
        }
    }
}
