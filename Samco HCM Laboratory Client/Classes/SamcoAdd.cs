using DevExpress.Data.Filtering;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Navigation;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using HCMData;
using LabData;
using Samco_HCM_Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        if (!SamcoSoftShared.LoadedSettings!.ActiveClient || RemoteDatalayer is null)
        {
            ((MainWindow)Application.Current.MainWindow!).Resources["ConnectionState"] = "کاربری محلی";
            return false;
        }
        try
        {
            var dataStore = XpoDefault.GetConnectionProvider(RemoteDatalayer.Connection.ConnectionString, AutoCreateOption.None);
            ((MainWindow)Application.Current.MainWindow!).Resources["ConnectionState"] = dataStore != null ? "متصل" : "قطع ارتباط";
            ((MainWindow)Application.Current.MainWindow!).ServerConnectBtn.IsVisible = dataStore != null;
            return dataStore != null;
        }
        catch (Exception)
        {
            ((MainWindow)Application.Current.MainWindow!).Resources["ConnectionState"] = "قطع ارتباط";
            ((MainWindow)Application.Current.MainWindow!).ServerConnectBtn.IsVisible = true;
            return false;
        }
    }

    internal static void UpdateBillsData()
    {
        if (CheckNetwork() == false) return;
        try
        {
            using var session1 = new Session(XpoDefault.DataLayer);
            using var remoteSession = new Session(RemoteDatalayer);
            var unpaidVisit = session1.Query<LabVisits>().Where(x => !x.paid).ToList();
            foreach (var bill in unpaidVisit)
            {
                var remoteBill = remoteSession.FindObject<RemoteBill>(new BinaryOperator("RemoteBillId", bill.Oid));
                if (remoteBill != null)
                {
                    if (remoteBill.IsPaid == false)
                    {
                        bill.paid = true;
                        bill.Save();
                        remoteBill.Delete();
                    }
                    else
                    {
                        remoteBill.Price = bill.Price;
                        remoteBill.Save();
                    }
                }
                else
                {
                    //Create Bill
                    var newRemoteBill = new RemoteBill(remoteSession)
                    {
                        RemoteBillId = bill.Oid,
                        Price = bill.Price,
                        ServiceType = "Lab",
                        Name = bill.Patient.Name,
                        MelliCode = bill.Patient.MelliCode,
                        InsId = bill.insType.serverID
                    };
                    newRemoteBill.Save();
                }
            }
        }
        catch (Exception ex)
        {
            MainNotify.ShowError("خطأ", $"خطای پایگاه داده: {ex.Message}");
        }
    }

    ////TODO: Only for Gahvareh
    //internal static int GetTestPrice(LabVisits visit)
    //{
    //    var sumPrice = visit.Price;
    //    if (!visit.isFree)
    //    {
    //        var fanniPrice = Math.Round((double)(visit.insType.fanniPrice * visit.insType.franchises / 10000));
    //        sumPrice += (int)fanniPrice;
    //        return sumPrice;
    //    }

    //    return 0;
    //}
    #endregion

    #region General

    internal static void PrintVisualTreeRecursive(ref List<DependencyObject> controlList, DependencyObject parentCtl, Type contType)
    {
        var count = VisualTreeHelper.GetChildrenCount(parentCtl);
        if (count > 0)
        {
            for (var n = 0; n <= count - 1; n++)
            {
                var child = VisualTreeHelper.GetChild(parentCtl, n);
                if (child.GetType() == contType)
                    controlList.Add(child);
                PrintVisualTreeRecursive(ref controlList, child, contType);
            }
        }
        else if (parentCtl.GetType() == contType)
            controlList.Add(parentCtl);
    }


    #endregion
}