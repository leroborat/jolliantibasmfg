using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using JolliantProd.Module.BusinessObjects;
using System.Collections.Generic;
using System.Diagnostics;

namespace JolliantProd.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {

            base.UpdateDatabaseAfterUpdateSchema();

            Employee sampleUser = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", "User"));
            if(sampleUser == null) {
                sampleUser = ObjectSpace.CreateObject<Employee>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            Employee userAdmin = ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<Employee>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
            }
			// If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
			userAdmin.Roles.Add(adminRole);

            PermissionPolicyRole RNDRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "RND"));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "RND";
            }
            

            WarehouseLocation warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Customers"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Customers";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.CustomerLocation;
            }
            ObjectSpace.CommitChanges();

            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Vendors"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Vendors";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.VendorLocation;
            }

            ObjectSpace.CommitChanges(); //This line persists created object(s).

            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Production"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Production";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.Production;
            }

            ObjectSpace.CommitChanges();

            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Scrapped"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Scrapped";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.InventoryLoss;
            }

            ObjectSpace.CommitChanges();

            warehouseLocation = ObjectSpace.FindObject<WarehouseLocation>(new BinaryOperator("LocationName", "Adjustment"));
            if (warehouseLocation == null)
            {
                warehouseLocation = ObjectSpace.CreateObject<WarehouseLocation>();
                warehouseLocation.LocationName = "Adjustment";
                warehouseLocation.LocationType = WarehouseLocation.LocationTypeEnum.InventoryLoss;
            }

            ObjectSpace.CommitChanges();


            UnitOfMeasureCategory  unitOfMeasureCategory = ObjectSpace.FindObject<UnitOfMeasureCategory>(new BinaryOperator("Name", "Unit"));
            if (unitOfMeasureCategory == null)
            {
                unitOfMeasureCategory = ObjectSpace.CreateObject<UnitOfMeasureCategory>();
                unitOfMeasureCategory.Name = "Unit";
            }

            UnitOfMeasure unitMeasure = ObjectSpace.FindObject<UnitOfMeasure>(new BinaryOperator("UOMName", "PC(s)"));
            if (unitMeasure == null)
            {
                unitMeasure = ObjectSpace.CreateObject<UnitOfMeasure>();
                unitMeasure.UOMName = "PC(s)";
                unitMeasure.UnitOfMeasureCategory = unitOfMeasureCategory;
            }

            PermissionPolicyRole purchaseManageRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Purchase Manager"));
            if (purchaseManageRole == null)
            {
                purchaseManageRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                purchaseManageRole.Name = "Purchase Manager";
            }

            PermissionPolicyRole purchaseUser = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Purchase User"));
            if (purchaseUser == null)
            {
                purchaseUser = ObjectSpace.CreateObject<PermissionPolicyRole>();
                purchaseUser.Name = "Purchase User";
            }

            ObjectSpace.CommitChanges();


        }


        public override void UpdateDatabaseBeforeUpdateSchema() {

            Debug.WriteLine("Cleaning duplicates before schema update");
            UpdateDuplicateSeries();
            FixDuplicateWithdrawalSeriesNames();
            FixDuplicateWithdrawalOtherSeries();

            base.UpdateDatabaseBeforeUpdateSchema();
            

        }

        private void FixDuplicateWithdrawalOtherSeries()
        {
            // Get all WithdrawalOther objects
            IList<WithdrawalOther> allWithdrawalOthers = ObjectSpace.GetObjects<WithdrawalOther>();

            // Find duplicates based on Series
            var duplicateGroups = allWithdrawalOthers
                .GroupBy(w => w.Series)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateGroups)
            {
                bool isFirst = true;
                int suffix = 1;

                foreach (var withdrawalOther in group)
                {
                    if (isFirst)
                    {
                        isFirst = false; // Keep the first duplicate unchanged
                        continue;
                    }

                    // Append a unique suffix to the duplicate Series
                    withdrawalOther.Series = withdrawalOther.Series + "-" + suffix;
                    suffix++;

                    // Save the changes
                    ObjectSpace.CommitChanges();
                }
            }

            // Commit all changes after processing duplicates
            ObjectSpace.CommitChanges();
        }


        private void FixDuplicateWithdrawalSeriesNames()
        {
            // Get all Withdrawal objects
            IList<Withdrawal> allWithdrawals = ObjectSpace.GetObjects<Withdrawal>();

            // Find duplicates based on SeriesName
            var duplicateGroups = allWithdrawals
                .GroupBy(w => w.SeriesName)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateGroups)
            {
                bool isFirst = true;
                int suffix = 1;

                foreach (var withdrawal in group)
                {
                    if (isFirst)
                    {
                        isFirst = false; // Keep the first duplicate unchanged
                        continue;
                    }

                    // Append a unique suffix to the duplicate SeriesName
                    withdrawal.SeriesName = withdrawal.SeriesName + "-" + suffix;
                    suffix++;

                    // Save the changes
                    ObjectSpace.CommitChanges();
                }
            }

            // Commit all changes after processing duplicates
            ObjectSpace.CommitChanges();
        }


        private void UpdateDuplicateSeries()
        {
            // Find all Receiving objects
            IList<Receiving> allReceivings = ObjectSpace.GetObjects<Receiving>();

            // Group by Series to find duplicates
            var duplicateGroups = allReceivings
                .GroupBy(r => r.Series)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateGroups)
            {
                bool isFirst = true;
                int decrement = 1;

                foreach (var receiving in group)
                {
                    if (isFirst)
                    {
                        isFirst = false; // Keep the first duplicate unchanged
                        continue;
                    }

                    // Append "-1", "-2", etc., to the duplicate series
                    receiving.Series = receiving.Series + "-" + decrement;
                    decrement++;

                    ObjectSpace.CommitChanges();
                }
            }

            ObjectSpace.CommitChanges(); // Ensure all updates are committed
        }

        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);                
            }
            return defaultRole;
        }


        


    }


}
