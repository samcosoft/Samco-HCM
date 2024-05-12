using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Navigation;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Views;
using Samco_HCM_Shared;
using LogicNP.CryptoLicensing;

namespace Samco_HCM.Classes
{
    internal static class SamcoAdd
    {
        internal static bool OffShift;
        internal static Dictionary<string, List<int>> ShiftList = new();
        internal static CryptoLicense License;

        internal static bool UserIsAdmin()
        {
            return SamcoSoftShared.CurrentUser?.Group is "A" or "O";
        }

        #region Licensing

        internal static bool ValidateLicense(out bool isTrial)
        {
            isTrial = true;
            License = new CryptoLicense
            {
                LicenseServiceURL = "https://demo.samcosoft.ir/activation2/Service.asmx",
                LicenseServiceSettingsFilePath = "%AppDomainAppPath%App_Data\\Samco HCM.xml",
                ValidationKey = "AMAAMACFmfVL+4ktVtTILxQjYT+LChwE13ed4BnmXP8vg0xPGmjWd1tuu3qAmIwjH5sEK30DAAEAAQ==",
                // Load license from the registry
                StorageMode = LicenseStorageMode.ToFile
            };

            if (string.IsNullOrEmpty(SamcoSoftShared.LoadedSettings!.ApplicationLicense))
            {
                //Add Trial license
                License.LicenseCode = "lgIAAPkTn7Dao9oBHgAYAENvbXBhbnk9I1Bob25lPSNBZGRyZXNzPVRsetzMZkJSMBWNKU6G0cT7tLpIMhHIkKXFOT/w/6ik6gRtLPMLQfIlQb/E+RtM7g==";
                SamcoSoftShared.LoadedSettings!.ApplicationLicense = "lgIAAPkTn7Dao9oBHgAYAENvbXBhbnk9I1Bob25lPSNBZGRyZXNzPVRsetzMZkJSMBWNKU6G0cT7tLpIMhHIkKXFOT/w/6ik6gRtLPMLQfIlQb/E+RtM7g==";
                License.Save();
            }
            else
            {
                License.LicenseCode = SamcoSoftShared.LoadedSettings!.ApplicationLicense;
            }

            // Validate the license using .Status property
            if (License.Status == LicenseStatus.Valid)
            {
                isTrial = License.IsEvaluationLicense();
                return true;
            }

            return false;
        }
        internal static bool ActivateBySerial(string serial, out string message, out bool isTrial)
        {
            isTrial = true;
            message = string.Empty;
            var result = License.GetLicenseFromSerial(serial);
            if (result != SerialValidationResult.Success)
            {
                message = result switch

                {
                    SerialValidationResult.Failed => "خطا در فعالسازی مجوز.",
                    SerialValidationResult.NotASerial => "سریال وارد شده نا معتبر است.",
                    _ => string.Empty
                };
                message += License.GetAllStatusExceptionsAsString();
                return false;
            }

            if (License.Status == LicenseStatus.Valid)
            {
                isTrial = License.IsEvaluationLicense();
                SamcoSoftShared.LoadedSettings!.ApplicationLicense = License.LicenseCode;
                License.Save();
                return true;
            }

            return false;
        }

        internal static bool ActivateByLicenseCode(string licenseCode, out string message)
        {
            message = string.Empty;
            License.LicenseCode = licenseCode;

            if (License.Status == LicenseStatus.Valid)
            {
                SamcoSoftShared.LoadedSettings!.ApplicationLicense = License.LicenseCode;
                License.Save();
                return true;
            }

            message = License.GetAllStatusExceptionsAsString();
            return false;
        }

        internal static bool DeactivateLicense()
        {
            License.Deactivate();
            License.Save();
            SamcoSoftShared.LoadedSettings!.ApplicationLicense = string.Empty;
            SamcoSoftShared.LoadedSettings!.Save();
            return true;
        }

        public static bool CheckForActiveConnection()
        {
            var myPing = new Ping();
            var reply = myPing.Send("demo.samcosoft.ir", 1000);
            if (reply is { Status: IPStatus.Success })
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Tile Control

        internal static Tile GetTile(HealthServices serviceInfo, bool isFirst = false, string groupText = null, bool isReserveTile = false)
        {
            var newTile = new Tile { Header = serviceInfo.name, Size = serviceInfo.name.Length > 17 ? TileSize.Large : TileSize.Small, Background = GetTileRandomColor(), Content = GetTileIcon(serviceInfo.image), UseLayoutRounding = true, Tag = serviceInfo.Oid };
            Navigation.SetNavigationParameter(newTile, serviceInfo.Oid);
            FlowLayoutControl.SetIsFlowBreak(newTile, isFirst);
            if (groupText is not null)
                TileLayoutControl.SetGroupHeader(newTile, groupText);
            if (isReserveTile == false)
            {
                if (!serviceInfo.HealthServicesCollection.Any())
                {
                    //TODO:Change Line
                    Navigation.SetNavigateTo(newTile, new Insurance());
                }
                else
                {
                    // Tile has children
                    newTile.MaximizedContentTemplate = GetTileChildren(serviceInfo);
                }
            }
            return newTile;
        }

        internal static Tile GetTile(string headerString, TileSize tilSize, string imageSource, bool isFirst = false, string groupText = null)
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

        private static DataTemplate GetTileChildren(HealthServices serviceInfo)
        {
            var retTemplate = TemplateGenerator.CreateDataTemplate(() => new MaximizedTile(serviceInfo.HealthServicesCollection.ToList()));
            //var maxTile = new FrameworkElementFactory(typeof(MaximizedTile));
            //maxTile.SetValue(MaximizedTile.HealthServicesList, serviceInfo.HealthServicesCollection.ToList());


            //var parentGroup = new FrameworkElementFactory(typeof(LayoutGroup));
            //parentGroup.SetValue(LayoutGroup.OrientationProperty, Orientation.Vertical);
            //foreach (var child in serviceInfo.HealthServicesCollection)
            //{
            //    var newBtn = new FrameworkElementFactory(typeof(SimpleButton));
            //    newBtn.SetValue(ContentControl.ContentProperty, child.name);
            //    newBtn.SetValue(Navigation.NavigateToProperty, "Insurance");
            //    newBtn.SetValue(Navigation.NavigationParameterProperty, child.Oid);
            //    // layoutParent.SetValue(LayoutGroup.HeaderProperty, "انتخاب خدمت")
            //    parentGroup.AppendChild(newBtn);
            //}
            //// layoutParent.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch)
            //var scrollControllPan = new FrameworkElementFactory(typeof(ScrollViewer));
            //scrollControllPan.AppendChild(parentGroup);
            //var mainGroup = new FrameworkElementFactory(typeof(LayoutGroup));
            //mainGroup.SetValue(LayoutGroup.OrientationProperty, Orientation.Vertical);
            //mainGroup.AppendChild(scrollControllPan);
            //var newLabel = new FrameworkElementFactory(typeof(Label));
            //newLabel.SetValue(ContentProperty, "برای کوچک کردن کاشی روی این قسمت دوباره کلیک کنید.");
            //newLabel.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            //newLabel.SetValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
            //newLabel.SetValue(MarginProperty, new Thickness(0, 0, 0, 30));
            //newLabel.SetValue(FontFamilyProperty, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#IRANYekanRD"));
            //newLabel.SetValue(FontSizeProperty, 14);
            //newLabel.SetValue(ForegroundProperty, new SolidColorBrush(Colors.White));
            //mainGroup.AppendChild(newLabel);

            // retTemplate.VisualTree = layoutParent
            //retTemplate.VisualTree = mainGrid;
            return retTemplate;
        }
        internal static Image GetTileIcon(string imageSrc)
        {
            if (string.IsNullOrEmpty(imageSrc))
                return default;
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
                return default;
            }
        }

        internal static Image GetTileIcon(byte[] imageSrc)
        {
            if (imageSrc is null)
                return default;
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

        internal static ImageSource GetImageSource(byte[] imageSrc)
        {
            if (imageSrc is null)
                return default;
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

        #region Queue Control

        internal static int GetNobat(int selServiceId)
        {
            using var session1 = new Session();
            var todayVisits = session1.Query<Visits>().Where(vis => vis.service.Oid == selServiceId && vis.visitDate.Date == DateTime.Today);
            return todayVisits.Count();
        }

        internal static int GetNobat(HealthServices selService, DateTime selDate)
        {
            using var session1 = new Session();
            var resDate = session1.Query<Reserves>().Where(res => res.Service.Oid == selService.Oid && res.AdmitDate == selDate);
            return resDate.Count();
        }

        internal static int GetNobat(int selServiceId, DateTime selDate)
        {
            using var session1 = new Session();
            var resDate = session1.Query<Reserves>().Where(res => res.Service.Oid == selServiceId && res.AdmitDate == selDate);
            return resDate.Count();
        }


        #endregion
    }
}
