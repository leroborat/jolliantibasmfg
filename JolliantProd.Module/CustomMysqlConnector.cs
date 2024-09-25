using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System;
using System.Data;
using DevExpress.Xpo.DB;
using MySql.Data.MySqlClient; // Ensure you have the MySql.Data package installed
using System.Threading;
using DevExpress.Persistent.Base;



namespace JolliantProd.Module
{
    public class CustomMySqlConnectionProvider : MySqlConnectionProvider
    {
        public CustomMySqlConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption)
            : base(connection, autoCreateOption) { }

        public override string ComposeSafeColumnName(string columnName)
        {
            return base.ComposeSafeColumnName(columnName).ToLower(Thread.CurrentThread.CurrentCulture);
        }

        public override string ComposeSafeTableName(string tableName)
        {
            return base.ComposeSafeTableName(tableName).ToLower(Thread.CurrentThread.CurrentCulture);
        }

        public override string ComposeSafeConstraintName(string constraintName)
        {
            return base.ComposeSafeConstraintName(constraintName).ToLower(Thread.CurrentThread.CurrentCulture);
        }

        public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            IDbConnection connection = new MySqlConnection(connectionString);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return new CustomMySqlConnectionProvider(connection, autoCreateOption);
        }

        public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption)
        {
            return new CustomMySqlConnectionProvider(connection, autoCreateOption);
        }

        public static string GetConnectionString(string database, string userid, string password)
        {
            return $"Server=myServerAddress;Database={database};Uid={userid};Pwd={password};";
        }

        public new static void Register()
        {
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
            DataStoreBase.RegisterDataStoreProvider(typeof(MySqlConnection).FullName, CreateProviderFromConnection);
        }

        public new const string XpoProviderTypeString = "MyCustomMySql";
    }

}
