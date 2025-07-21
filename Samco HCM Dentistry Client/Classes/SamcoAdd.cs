using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace Samco_HCM_Dentistry_Client.Classes;

internal static class SamcoAdd
{
    internal static bool OffShift; 
    internal static bool OnlineClient;

    public static bool LoadDatabase(string connectionString, ref string errorMessage)
    {
        //Check For database Update
        var databaseAssemblies = System.Reflection.Assembly.GetExecutingAssembly();
        try
        {
            var dbDataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
            using var session1 = new Session(dbDataLayer);
            session1.UpdateSchema(databaseAssemblies);
            session1.CreateObjectTypeRecords(databaseAssemblies);
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }

        //Try to Connect to database
        var dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(System.Reflection.Assembly.GetExecutingAssembly());
        var store = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
        try
        {
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dict, store);
            XpoDefault.Session = null;
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }
}