using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Navigation;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using HCMData;
using Samco_HCM_Shared;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Samco_HCM_Laboratory_Client.Classes;

internal static class SamcoAdd
{
    internal static bool OffShift;
    internal static bool OnlineClient;
    internal static IDataLayer? RemoteDatalayer;
    internal static bool UserIsAdmin()
    {
        return SamcoSoftShared.CurrentUser?.Group is "A" or "O";
    }
    public static bool LoadDatabase(string connectionString, ref string? errorMessage)
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

    #region Tile Control

    internal static Tile GetTile(HealthServices serviceInfo, bool isFirst = false, string? groupText = null)
    {
        var newTile = new Tile { Header = serviceInfo.name, Size = serviceInfo.name.Length > 17 ? TileSize.Large : TileSize.Small, Background = GetTileRandomColor(), Content = GetTileIcon(serviceInfo.image), UseLayoutRounding = true, Tag = serviceInfo.Oid };
        Navigation.SetNavigationParameter(newTile, serviceInfo.Oid);
        FlowLayoutControl.SetIsFlowBreak(newTile, isFirst);
        if (groupText is not null)
            TileLayoutControl.SetGroupHeader(newTile, groupText);
        return newTile;
    }

    internal static Tile GetTile(string headerString, TileSize tilSize, string imageSource, bool isFirst = false, string? groupText = null)
    {
        var newTile = new Tile { Header = headerString, Size = tilSize, Background = GetTileRandomColor(), Content = GetTileIcon(imageSource), UseLayoutRounding = true };
        FlowLayoutControl.SetIsFlowBreak(newTile, isFirst);
        if (groupText is not null)
            TileLayoutControl.SetGroupHeader(newTile, groupText);
        return newTile;
    }

    // For insurance list on Insurance page
    internal static TileBarItem GetTile(insuranceType insuranceInfo)
    {
        var newTile = new TileBarItem
        {
            Content = insuranceInfo.name,
            Size = DevExpress.Xpf.Navigation.Internal.TileSize.Auto,
            Background = GetTileRandomColor(),
            TileGlyph = GetImageSource(insuranceInfo.image),
            Width = 170,
            Foreground = new SolidColorBrush(Colors.White),
            HorizontalContentAlignment = HorizontalAlignment.Right,
            Tag = insuranceInfo.Oid,
            UseLayoutRounding = true
        };
        return newTile;
    }

    internal static Image? GetTileIcon(string imageSrc)
    {
        if (string.IsNullOrEmpty(imageSrc))
            return null;
        try
        {
            var bim = new BitmapImage(new Uri($"pack://application:,,,/Images/{imageSrc}"));
            var result = new Image { Source = bim, Margin = new Thickness(15, 15, 15, 50) };
            RenderOptions.SetBitmapScalingMode(result, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(result, EdgeMode.Aliased);
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    internal static Image? GetTileIcon(byte[]? imageSrc)
    {
        if (imageSrc is null)
            return null;
        var memStr = new MemoryStream(imageSrc);
        var bim = new BitmapImage();
        bim.BeginInit();
        memStr.Seek(0L, SeekOrigin.Begin);
        bim.StreamSource = memStr;
        bim.EndInit();
        var result = new Image { Source = bim, Margin = new Thickness(15, 15, 15, 50) };
        RenderOptions.SetBitmapScalingMode(result, BitmapScalingMode.HighQuality);
        RenderOptions.SetEdgeMode(result, EdgeMode.Aliased);
        return result;
    }

    internal static ImageSource? GetImageSource(byte[]? imageSrc)
    {
        if (imageSrc is null)
            return null;
        var memStr = new MemoryStream(imageSrc);
        var bim = new BitmapImage();
        bim.BeginInit();
        memStr.Seek(0L, SeekOrigin.Begin);
        bim.StreamSource = memStr;
        bim.EndInit();
        RenderOptions.SetBitmapScalingMode(bim, BitmapScalingMode.HighQuality);
        RenderOptions.SetEdgeMode(bim, EdgeMode.Aliased);
        var targetBitmap = new TransformedBitmap(bim, new ScaleTransform(0.3, 0.3));
        return targetBitmap;
    }

    private static SolidColorBrush GetTileRandomColor()
    {
        var rnd = new Random();
        return new SolidColorBrush(Color.FromRgb(Convert.ToByte(rnd.Next(0, 180)), Convert.ToByte(rnd.Next(0, 180)), Convert.ToByte(rnd.Next(0, 180))));
    }
    #endregion

    #region Network

    internal static bool CheckNetwork()
    {
        if (RemoteDatalayer is null)
            return false;
        try
        {
            var dataStore = XpoDefault.GetConnectionProvider(RemoteDatalayer.Connection.ConnectionString, AutoCreateOption.None);
            return dataStore != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion
}