using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using HCMData;
using Samco_HCM_Shared.Classes;

namespace Samco_HCM_Shared;

public static class SamcoSoftShared
{
    public static HCMSettings? LoadedSettings;
    public static BitmapImage? LoadImage(byte[]? imageData)
    {
        if (imageData == null || imageData.Length == 0) return null;

        var image = new BitmapImage();
        using (var mem = new MemoryStream(imageData))
        {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }

        image.Freeze();
        return image;
    }

    public static bool LoadDatabase(string? connectionString, bool justConnect, ref string? errorMessage)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            errorMessage = "Connection string is null or empty.";
            return false;
        }
        if (!justConnect)
        {//Check For database Update
            var databaseAssemblies = Assembly.GetExecutingAssembly();
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
        }

        //Try to Connect to database
        var dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(Assembly.GetExecutingAssembly());
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

    public static bool LoadRemoteDatabase(string? connectionString, Assembly remoteAssembly, bool justConnect, ref string? errorMessage, out IDataLayer? dataLayer)
    {
        dataLayer = null;
        if (string.IsNullOrEmpty(connectionString))
        {
            errorMessage = "Connection string is null or empty.";
            return false;
        }
        if (!justConnect)
        {//Check For database Update
            try
            {
                var dbDataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
                using var session1 = new Session(dbDataLayer);
                session1.UpdateSchema(remoteAssembly);
                session1.CreateObjectTypeRecords(remoteAssembly);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        //Try to Connect to database
        var dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(remoteAssembly);
        var store = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
        try
        {
            dataLayer = new ThreadSafeDataLayer(dict, store);
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }

    public static LoggedUser? CurrentUser;

    #region Setting Manager Codes

    public static void WriteToJsonFile<T>(string filePath, T objectToWrite)
    {
        var jsonString = JsonSerializer.Serialize(objectToWrite, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, jsonString);
    }

    public static T? ReadFromJsonFile<T>(string filePath)
    {

        var jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    #endregion

}
public class BackColorMaker
{
    public static Brush BackColor
    {
        get
        {
            var retColor = new Color();
            var rnd = new Random();
            retColor.A = 255;
            retColor.R = (byte)rnd.Next(200);
            retColor.G = (byte)rnd.Next(200);
            retColor.B = (byte)rnd.Next(200);
            var retBrush = new SolidColorBrush(retColor);
            return retBrush;
        }
    }
}

public class LoggedUser
{
    public int Oid { get; set; }
    public string? Username { get; set; }
    public string? Group { get; set; }
    public string? RealName { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public byte[]? Avatar { get; set; }

    public LoggedUser()
    {
    }

    public LoggedUser(Users selUser)
    {
        Oid = selUser.Oid;
        Username = selUser.username;
        Group = selUser.group;
        RealName = selUser.realname;
        LastLoginTime = selUser.lastLogin;
        Avatar = selUser.photo;
    }
}