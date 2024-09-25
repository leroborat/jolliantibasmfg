using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JolliantProd.Module.BusinessObjects
{
 
    [RuleObjectExists("AnotherSingletonExists", DefaultContexts.Save, "True", InvertResult = true,
    CustomMessageTemplate = "Another Singleton already exists.")]
    [RuleCriteria("CannotDeleteSingleton", DefaultContexts.Delete, "False",
    CustomMessageTemplate = "Cannot delete Singleton.")]
    public class Odoo : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Odoo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string status;
        string databaseName;
        bool isEnabled;
        string password;
        string username;
        string server;


        public bool IsEnabled
        {
            get => isEnabled;
            set => SetPropertyValue(nameof(IsEnabled), ref isEnabled, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField()]
        public string Server
        {
            get => server;
            set => SetPropertyValue(nameof(Server), ref server, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField()]
        public string DatabaseName
        {
            get => databaseName;
            set => SetPropertyValue(nameof(DatabaseName), ref databaseName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField()]
        public string Username
        {
            get => username;
            set => SetPropertyValue(nameof(Username), ref username, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [VisibleInListView(false)]
        [RuleRequiredField()]
        public string Password
        {
            get => password;
            set => SetPropertyValue(nameof(Password), ref password, value);
        }


        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        public async Task<Type> ConnectOdoo()
        {

            var config = new OdooConfig(
               apiUrl: Server,
               dbName: DatabaseName,
               userName: Username,
               password: Password
               );

            var odooClient = new OdooClient(config);
            var versionResult = await odooClient.GetVersionAsync();
            var loginResult = await odooClient.LoginAsync();

            Console.WriteLine("@@@@@@@@@@@@");

            Console.WriteLine(loginResult.Succeed);
            if (loginResult.Succeed)
            {
                Status = "Intergation Successful!";
            } else
            {
                Status = "Failed! " + loginResult.Message;
            }

            return null;

        }
        [Action(Caption = "Test Odoo Connection", ImageName = "Attention", AutoCommit = true)]
        public void OdooTestActionMethod()
        {
            ConnectOdoo();
        }
    }
}