using System;
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
                using (var stream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.Read();
                        do
                        {
                            while (reader.Read())
                            {
                                Lot lot = ObjectSpace.FindObject<Lot>(new BinaryOperator("LotCode", reader.GetValue(0)));
                                if (lot == null)
                                {
                                    lot = ObjectSpace.CreateObject<Lot>();
                                    lot.LotCode = Convert.ToString(reader.GetValue(0));
                                    lot.Product = ((FinishedGoodLoader)View.CurrentObject).Product;
                                    lot.InternalReference = finishedGoodLoader.ReferenceName;

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
                                    } else
                                    {
                                        lot.ExpirationDate = ((FinishedGoodLoader)View.CurrentObject).LotExpirationDate;
                                    }
                
                                    ObjectSpace.CommitChanges();

                                    StockTransfer stockTransfer = ObjectSpace.CreateObject<StockTransfer>();
                                    stockTransfer.Reference = finishedGoodLoader.ReferenceName;
                                    stockTransfer.SourceLocation = finishedGoodLoader.From;
                                    stockTransfer.DestinationLocation = finishedGoodLoader.To;
                                    stockTransfer.Product = finishedGoodLoader.Product;
                                    stockTransfer.Quantity = (Double)reader.GetValue(1);
                                    stockTransfer.Lot = lot;
                                    ObjectSpace.CommitChanges();
                                }
                                Debug.WriteLine(reader.GetValue(0));
                                Debug.WriteLine(reader.GetValue(1));
                            }
                        } while (reader.NextResult());
                    }

                    ShowSuccess("Finished Loading. Check Stock Moves");
                }
                
                View.ObjectSpace.Refresh();
            }
        }
    }
}
