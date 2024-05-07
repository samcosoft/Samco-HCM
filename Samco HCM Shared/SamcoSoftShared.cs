using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using System;
using System.IO;
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

        //Try Connect to database
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

    public static LoggedUser? CurrentUser;
    public static bool ClosedDay { get; set; }

    #region Setting Manager Codes

    public static void WriteToJsonFile<T>(string filePath, T objectToWrite)
    {
        var jsonString = JsonSerializer.Serialize(objectToWrite);
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

public class LoggedUser(Users selUser)
{
    public int Oid { get; set; } = selUser.Oid;
    public string? Username { get; set; } = selUser.username;
    public string? Group { get; set; } = selUser.group;
    public string? RealName { get; set; } = selUser.realname;
    public DateTime? LastLoginTime { get; set; } = selUser.lastLogin;

    public byte[] Avatar { get; set; } = selUser.photo;
}