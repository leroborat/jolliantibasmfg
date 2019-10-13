using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ExcelDataReader;
using JolliantProd.Module.BusinessObjects;

namespace JolliantProd.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class LoadBachTransmittalLineController : ViewController
    {
        public LoadBachTransmittalLineController()
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

        private void BatchTransmittalAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var batchTransmittal = (BatchTransmittal)View.CurrentObject;
            batchTransmittal.Save();
            batchTransmittal.ProductionBatch.KitchenPlan.Save();


            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Session currentSession = ((XPObjectSpace)ObjectSpace).Session;
                UnitOfWork session = new UnitOfWork(currentSession.DataLayer);

                currentSession.Save(batchTransmittal.ProductionBatch.KitchenPlan);
                currentSession.CommitTransaction();

                //var thisBatchTransfer = session.FindObject<BatchTransmittal>(new BinaryOperator("Oid", batchTransmittal.Oid));
                //Debug.WriteLine(batchTransmittal.Oid);

                using (var stream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                //Debug.WriteLine(thisBatchTransfer);
                                var myLine = new BatchTransmittalLine(currentSession);
                                try
                                {
                                    myLine.ItemCode = Convert.ToString(reader.GetValue(0));
                                }
                                catch (Exception){}

                                try
                                {
                                    myLine.Description = Convert.ToString(reader.GetValue(1));
                                }
                                catch (Exception) { }

                                try
                                {
                                    myLine.TotalUnits = (Double)reader.GetValue(2);
                                }
                                catch (Exception) { }

                                try
                                {
                                    myLine.Remarks = Convert.ToString(reader.GetValue(3));
                                }
                                catch (Exception) { }

                                myLine.BatchTransmittal = batchTransmittal;
                            }
                        } while (reader.NextResult());
                    }
                    //currentSession.CommitChanges();
                    
                }
                batchTransmittal.Save();
                View.ObjectSpace.Refresh();
            }
        }
    }
}
