﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
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
using ExcelDataReader;
using System.Diagnostics;
using JolliantProd.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using OfficeOpenXml;

namespace JolliantProd.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class LoadVariantController : ViewController
    {
        public LoadVariantController()
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

        private void LoadVariantAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((Variant)View.CurrentObject);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        do
                        {
                            while (reader.Read())
                            {
                                Product product = ObjectSpace.FindObject<Product>(new BinaryOperator("ProductName", reader.GetValue(0)));
                                if (product != null)
                                {
                                    VariantLine vLine = ObjectSpace.CreateObject<VariantLine>();
                                    vLine.Product = product;
                                    vLine.SalesPrice = Convert.ToDecimal((reader.GetValue(1)).ToString());

                                    thisView.VariantLines.Add(vLine);
                                    ObjectSpace.CommitChanges();
                                    Debug.WriteLine("Here 1");
                                    Debug.WriteLine(reader.GetValue(0));
                                    Debug.WriteLine(reader.GetValue(1));
                                }
                               
                            }
                        } while (reader.NextResult());
                    }
                }
                Debug.WriteLine("Here 3");
                thisView.Save();
                View.ObjectSpace.Refresh();
            }
        }

        private void LoadProductAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        do
                        {
                            while (reader.Read())
                            {
                                Product product = ObjectSpace.FindObject<Product>(new BinaryOperator("ProductName", reader.GetValue(0)));
                                if (product == null)
                                {
                                    product = ObjectSpace.CreateObject<Product>();
                                    product.ProductName = (String)reader.GetValue(0);
                                    product.CanBeSold = true;
                                    product.ProductType = Product.ProductTypeEnum.Storable;
                                    product.Tracking = Product.TrackingEnum.TrackByLot;
                                    SalesCategory category = ObjectSpace.FindObject<SalesCategory>(new BinaryOperator("CategoryName", reader.GetValue(1)));
                                    if (category != null)
                                    {
                                        product.SalesCategory = category;
                                    } else {
                                        ShowError("Category " + reader.GetValue(1) + " not found. Skipping: " + reader.GetValue(0));
                                        product.Delete();
                                        continue;
                                    }
                                    UnitOfMeasure unitOfMeasure = ObjectSpace.FindObject<UnitOfMeasure>(new BinaryOperator("UOMName", "PC(s)"));
                                    if (unitOfMeasure != null)
                                    {
                                        product.UOM = unitOfMeasure;
                                        product.PurchaseUOM = unitOfMeasure;
                                    } else
                                    {
                                        ShowError("UOM  PC(s) not found. Please create first. " + product);
                                        product.Delete();
                                        continue;
                                    }

                                    ObjectSpace.CommitChanges();
                                }
                                Debug.WriteLine(reader.GetValue(0));
                                Debug.WriteLine(reader.GetValue(1));
                            }
                        } while (reader.NextResult());
                    }
                }

                View.ObjectSpace.Refresh();
            }
        }

        private void ShowError(string v)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 5000;
            options.Message = v;
            options.Type = InformationType.Error;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = "Error";
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void ShowSuccess(string v)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 5000;
            options.Message = v;
            options.Type = InformationType.Success;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = "Success";
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void LoadFgAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var finishedGoodLoader = (FinishedGoodLoader)View.CurrentObject;
            finishedGoodLoader.Save();
            

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Session currentSession= ((XPObjectSpace)ObjectSpace).Session;
                UnitOfWork session = new UnitOfWork(currentSession.DataLayer);
                var thisProduct = ((FinishedGoodLoader)View.CurrentObject).Product;
                var sessProduct = session.FindObject<Product>(new BinaryOperator("Oid", thisProduct.Oid));
                var sessFrom = session.FindObject<WarehouseLocation>(new BinaryOperator("Oid", finishedGoodLoader.From.Oid));
                var sessTo = session.FindObject<WarehouseLocation>(new BinaryOperator("Oid", finishedGoodLoader.To.Oid));
                var thisKP = session.FindObject <KitchenPlan>(new BinaryOperator("Oid", finishedGoodLoader.KitchenPlan.Oid));
                var lotList = new List<Lot>();

                var thisReference = finishedGoodLoader.ReferenceName;

                using (var stream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        do
                        {
                            while (reader.Read())
                            {
                                
                                Lot lot = new Lot(session);
                                //if (lot == null)
                                //{
                                //    lot = new Lot(session);
                                //}
                                lot.LotCode = Convert.ToString(reader.GetValue(0));
                                lot.Product = sessProduct;
                                lot.InternalReference = thisReference;
                                lot.KitchenPlan = thisKP;

                                if (lot.Product.SalesCategory.CategoryName == "Hotta Rice")
                                {
                                    try
                                    {
                                        DateTime newED = DateTime.ParseExact(lot.LotCode.Substring(lot.LotCode.Length - 6), "MMddyy",
                                            System.Globalization.CultureInfo.InvariantCulture);
                                        lot.ExpirationDate = newED;
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                else
                                {
                                    lot.ExpirationDate = ((FinishedGoodLoader)View.CurrentObject).LotExpirationDate;
                                }

                                lot.Save();
                                lotList.Add(lot);

                                StockTransfer stockTransfer = new StockTransfer(session);
                                stockTransfer.Reference = finishedGoodLoader.ReferenceName;
                                stockTransfer.SourceLocation = sessFrom;
                                stockTransfer.DestinationLocation = sessTo;
                                stockTransfer.Product = sessProduct;
                                stockTransfer.Quantity = (Double)reader.GetValue(1);
                                stockTransfer.Lot = lot;
                                stockTransfer.Save();
                                stockTransfer.Lot.UpdateStockOnHand(true);
                                stockTransfer.Lot.Save();
                                Debug.WriteLine(stockTransfer.Lot.StockOnHand);

                            }
                        } while (reader.NextResult());
                    }
                    session.CommitChanges();
                    //foreach (Lot item in lotList)
                    //{
                    //    item.UpdateStockOnHand(true);
                    //}
                    //session.CommitChanges();
                    ShowSuccess("Finished Loading. Check Stock Moves");
                }
                
                View.ObjectSpace.Refresh();
            }
        }

        private void UpdateLotCountDB_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var LotCollection = ObjectSpace.GetObjects<Lot>();
            foreach (Lot item in LotCollection)
            {
                item.UpdateStockOnHand(true);
                item.Save();
                Debug.WriteLine(item.LotCode);
            }
            ObjectSpace.CommitChanges();

        }

        private void CustomPOSummary_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
           
        }

        private void LoadPurchaseableProduct_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        do
                        {
                            while (reader.Read())
                            {
                                Product product = ObjectSpace.FindObject<Product>(new BinaryOperator("ProductName", reader.GetValue(0)));
                                if (product == null)
                                {
                                    product = ObjectSpace.CreateObject<Product>();
                                    product.ProductName = (String)reader.GetValue(0);
                                    product.CanBePurchased = true;
                                    product.ProductType = Product.ProductTypeEnum.Storable;
                                    product.Tracking = Product.TrackingEnum.TrackByLot;
                                    product.InternalReference = reader.GetValue(4).ToString();
                                    SalesCategory category = ObjectSpace.FindObject<SalesCategory>(new BinaryOperator("CategoryName", reader.GetValue(1)));
                                    if (category != null)
                                    {
                                        product.SalesCategory = category;
                                    }
                                    else
                                    {
                                        ShowError("Category " + reader.GetValue(1) + " not found. Skipping: " + reader.GetValue(0));
                                        product.Delete();
                                        continue;
                                    }
                                    UnitOfMeasure unitOfMeasure = ObjectSpace.FindObject<UnitOfMeasure>(new BinaryOperator("UOMName", reader.GetValue(2)));
                                    if (unitOfMeasure != null)
                                    {
                                        product.UOM = unitOfMeasure;
                                        product.PurchaseUOM = unitOfMeasure;
                                    }
                                    else
                                    {
                                        ShowError("UOM " + reader.GetValue(2) + " not found. Skipping: " + reader.GetValue(0));
                                        product.Delete();
                                        continue;
                                    }

                                    UnitOfMeasure unitOfMeasure2 = ObjectSpace.FindObject<UnitOfMeasure>(new BinaryOperator("UOMName", reader.GetValue(3)));
                                    if (unitOfMeasure2 != null)
                                    {
                                        product.ProductionUOM = unitOfMeasure2;
                                    }

                                    else
                                    {
                                        ShowError("UOM " + reader.GetValue(3) + " not found. Skipping: " + reader.GetValue(0));
                                        product.Delete();
                                        continue;
                                    }

                                    ObjectSpace.CommitChanges();
                                }
                                Debug.WriteLine(reader.GetValue(0));
                                Debug.WriteLine(reader.GetValue(1));
                            }
                        } while (reader.NextResult());
                    }
                }

                View.ObjectSpace.Refresh();
                ShowSuccess("Finished Loading.");
            }
        }

        void GenerateCostRecords(string fileName, WorkCenter prep1, WorkCenter prep2, int value, ProductionBatch pb)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    BatchTransmittal bt = ObjectSpace.CreateObject<BatchTransmittal>();
                    bt.ProductionBatch = pb;
                    bt.From = prep1;
                    bt.To = prep2;
                    do
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(value) == null)
                            {
                                continue;
                            }
                            BatchTransmittalLine btl = ObjectSpace.CreateObject<BatchTransmittalLine>();
                            btl.BatchTransmittal = bt;
                            btl.ItemCode = reader.GetValue(0).ToString();
                            btl.Description = reader.GetValue(1).ToString();
                            btl.Remarks = "Loaded from Importer";
                            btl.TotalUnits = reader.GetDouble(value);

                            ObjectSpace.CommitChanges();


                        }
                    } while (reader.NextResult());
                    ObjectSpace.CommitChanges();
                }
            }
        }
        private void GinilingAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var thisView = ((ProductionBatch)View.CurrentObject);
            WorkCenter prep1 = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Prep 1"));
            if (prep1 == null)
            {
                prep1 = ObjectSpace.CreateObject<WorkCenter>();
                prep1.Name = "Prep 1";
            }

            WorkCenter prep2 = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Prep 2"));
            if (prep2 == null)
            {
                prep2 = ObjectSpace.CreateObject<WorkCenter>();
                prep2.Name = "Prep 2";
            }

            WorkCenter kitchen = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Kitchen"));
            if (kitchen == null)
            {
                kitchen = ObjectSpace.CreateObject<WorkCenter>();
                kitchen.Name = "Kitchen";
            }

            WorkCenter precooling = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Pre-Cooling"));
            if (precooling == null)
            {
                precooling = ObjectSpace.CreateObject<WorkCenter>();
                precooling.Name = "Pre-Cooling";
            }

            WorkCenter cooling = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Cooling"));
            if (cooling == null)
            {
                cooling = ObjectSpace.CreateObject<WorkCenter>();
                cooling.Name = "Cooling";
            }

            WorkCenter storage = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Storage"));
            if (storage == null)
            {
                storage = ObjectSpace.CreateObject<WorkCenter>();
                storage.Name = "Storage";
            }

            WorkCenter packingTS = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Packing-TopSeal"));
            if (packingTS == null)
            {
                packingTS = ObjectSpace.CreateObject<WorkCenter>();
                packingTS.Name = "Packing-TopSeal";
            }

            WorkCenter packingJumbo = ObjectSpace.FindObject<WorkCenter>(new BinaryOperator("Name", "Packing-Jumbo"));
            if (packingJumbo == null)
            {
                packingJumbo = ObjectSpace.CreateObject<WorkCenter>();
                packingJumbo.Name = "Packing-Jumbo";
            }






            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                GenerateCostRecords(openFileDialog1.FileName, prep1, prep2, 2, thisView);
                GenerateCostRecords(openFileDialog1.FileName, prep2, kitchen, 3, thisView);
                GenerateCostRecords(openFileDialog1.FileName, kitchen, precooling, 4, thisView);
                GenerateCostRecords(openFileDialog1.FileName, precooling, cooling, 5, thisView);
                GenerateCostRecords(openFileDialog1.FileName, cooling, storage, 6, thisView);
                GenerateCostRecords(openFileDialog1.FileName, storage, packingTS, 7, thisView);
                GenerateCostRecords(openFileDialog1.FileName, storage, packingJumbo, 8, thisView);


                View.ObjectSpace.Refresh();
                ShowSuccess("Finished Loading.");
            }
        }
    }
}
