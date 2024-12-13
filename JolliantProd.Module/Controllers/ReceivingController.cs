using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ReceivingController : ViewController
    {
        public ReceivingController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction assignSeriesAction = new SimpleAction(
            this, "AssignSeriesAction", PredefinedCategory.Edit)
            {
                Caption = "Assign Series",
                ConfirmationMessage = "Are you sure?",
                ImageName = "Attention",                
            };

            // Attach the action's Execute event handler
            assignSeriesAction.Execute += AssignSeriesAction_Execute;
        }

        private void AssignSeriesAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var currentObject = View.CurrentObject as Receiving;
            if (currentObject != null)
            {
                if (currentObject.Series == null)
                {
                    // Generate the new Series
                    int nextIn = currentObject.StorageLocation.NextIn;
                    string newSeries;

                    // Loop until we find a unique series
                    do
                    {
                        // Generate the new Series based on the current NextIn value
                        newSeries = currentObject.StorageLocation.Warehouse.WarehouseName + "-IN-" + nextIn;

                        // Check if the series already exists in another Receiving object
                        var existingReceiving = ObjectSpace.FindObject<Receiving>(CriteriaOperator.Parse("Series == ?", newSeries));

                        if (existingReceiving == null)
                        {
                            // If no existing series is found, break the loop
                            break;
                        }

                        // If the series is already taken, increment NextIn and try again
                        nextIn += 1;

                    } while (true);

                    // Once a unique series is found, update the StorageLocation and the Series
                    currentObject.StorageLocation.NextIn = nextIn + 1; // Increment NextIn after assigning the new series
                    currentObject.Series = newSeries;

                    // Save changes to the StorageLocation and the current object
                    ObjectSpace.CommitChanges();

                }
            }

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.ObjectSpace.Committing += ObjectSpace_Committing;
            // Perform various tasks depending on the target View.
        }

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            if (((Receiving)View.CurrentObject).FirstModifiedOn == DateTime.MinValue)
            {
                ((Receiving)View.CurrentObject).FirstModifiedOn = DateTime.Now;
            }
            ((Receiving)View.CurrentObject).LastModifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).EmployeeName;
            ((Receiving)View.CurrentObject).LastModifiedOn = DateTime.Now;
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
    }
}
