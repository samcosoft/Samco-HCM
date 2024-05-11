using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;
using Samco_HCM.Classes;
using Samco_HCM.Views.Settings;
using Samco_HCM_Shared;
using Samco_HCM_Shared.Classes;

namespace Samco_HCM.Views;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class Dashboard
{
    public Dashboard()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        RenderTiles();
        base.OnNavigatedTo(e);
    }

    private void RenderTiles()
    {
        MainTilLayout.Children.Clear();
        //Add dentistry
        var session1 = new Session();
        if (session1.Query<HealthServices>().Any() == false && File.Exists(Environment.CurrentDirectory + "\\Template.mdb"))
        {
            if (WinUIMessageBox.Show(this, "در حال حاضر هیچ خدمتی تعریف نشده است. آیا مایل به تعریف تعدادی خدمت پیش‌فرض هستید؟", "تعریف خدمات پیش‌فرض", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Window) == MessageBoxResult.Yes)
            {
                var tempDataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AccessConnectionProvider.GetConnectionString(Environment.CurrentDirectory + "\\Template.mdb"), DevExpress.Xpo.DB.AutoCreateOption.SchemaOnly);
                using var tempSession = new UnitOfWork(tempDataLayer);
                var cloneHelp = new CloneHelper(session1);
                var healthList = tempSession.Query<HealthServices>().ToList();
                var newService = cloneHelp.Clone(healthList[0], true);
                newService.Save();
            }
        }

        //Add Dentistry
        if (!session1.Query<HealthServices>().Any(x => x.name.Contains("دندانپزشک")))
        {
            //Create default Dentistry service
            var dentSvc = new HealthServices(session1) { name = "دندانپزشک", group = "visit", image = (byte[])Resources["Dentist"] };
            dentSvc.Save();
        }

        //Load tiles
        var recTileList = session1.Query<HealthServices>().Where(x => x.group == "visit").ToList();

        foreach (var itm in recTileList.Where(itm => itm.parent is null))
        {
            //Create items for visit
            var newVisitTile = SamcoAdd.GetTile(itm);
            if (itm.HealthServicesCollection.Count > 0)
            {
                newVisitTile.Click += MaxiTiles_Click;
            }
            if (MainTilLayout.Children.Count == 0) TileLayoutControl.SetGroupHeader(newVisitTile, "ویزیت");
            MainTilLayout.Children.Add(newVisitTile);
        }

        //Create emergency Tile
        var emergentTile = SamcoAdd.GetTile("خدمات اورژانس", TileSize.Small, "Emergency.samco", true, "خدمات دیگر");
        Navigation.SetNavigateTo(emergentTile, new Emergency());
        MainTilLayout.Children.Add(emergentTile);

        //Add Laboratory
        if (!session1.Query<HealthServices>().Any(x => x.name.Contains("آزمایشگاه")))
        {
            //Create default laboratory service
            var labSvc = new HealthServices(session1) { name = "آزمایشگاه", group = "other", image = (byte[])Resources["Laboratory"] };
            labSvc.Save();
        }

        //Add other items
        recTileList = session1.Query<HealthServices>().Where(x => x.group == "other").ToList();
        foreach (var itm in recTileList.Where(itm => itm.parent is null))
        {
            //Create items for extra
            var newVisitTile = SamcoAdd.GetTile(itm);
            if (itm.HealthServicesCollection.Count > 0)
            {
                newVisitTile.Click += MaxiTiles_Click;
            }
            MainTilLayout.Children.Add(newVisitTile);
        }

        //Add Network tiles (Professional Edition)
        //if (SamcoAdd.License.GetLicenseEdition() == "Professional Edition")
        //{
        //    if (session1.Query<HealthServices>().Any(x => x.name.Contains("دندانپزشک")))
        //    {
        //        //Create Dentistry Tile
        //        var dentTile = SamcoAdd.GetTile("دندانپزشکی (شبکه)", TileSize.Small, Resources["Dentist"], true, "خدمات شبکه");
        //        dentTile.Click += MaxiTiles_Click;
        //        MainTilLayout.Children.Add(dentTile);
        //    }
        //    if (session1.Query<HealthServices>().Any(x => x.name.Contains("آزمایشگاه")))
        //    {
        //        //Create Laboratory Tile
        //        var labTile = SamcoAdd.GetTile("آزمایشگاه (شبکه)", TileSize.Small, Resources["Laboratory"]);
        //        labTile.Click += MaxiTiles_Click;
        //        MainTilLayout.Children.Add(labTile);
        //    }
        //}

        //Create statistics Tiles
        var dailyStat = SamcoAdd.GetTile("آمار روزانه", TileSize.Small, "DailyStatIcon.samco", true, "آمار");
        Navigation.SetNavigateTo(dailyStat, new Statistics());
        Navigation.SetNavigationParameter(dailyStat, "daily");
        MainTilLayout.Children.Add(dailyStat);

        var allStat = SamcoAdd.GetTile("آمار کلی", TileSize.Small, "AllStatIcon.samco");
        Navigation.SetNavigateTo(allStat, new Statistics());
        Navigation.SetNavigationParameter(allStat, "all");
        MainTilLayout.Children.Add(allStat);

        if (SamcoAdd.UserIsAdmin())
        {
            var editBil = SamcoAdd.GetTile("ویرایش قبض", TileSize.Small, "BillEditIcon.samco", true, "ویرایش اطلاعات");
            Navigation.SetNavigateTo(editBil, new BillEditorView());
            MainTilLayout.Children.Add(editBil);

            editBil = SamcoAdd.GetTile("ویرایش اطلاعات بیماران", TileSize.Large, "BillEditIcon.samco");
            Navigation.SetNavigateTo(editBil, new PatientEditorView());
            MainTilLayout.Children.Add(editBil);

            //Create settings tiles
            var insuranceService = SamcoAdd.GetTile("تنظیمات بیمه و خدمات", TileSize.Large, "ServiceInsuranceIcon.samco", true, "تنظیمات");
            Navigation.SetNavigateTo(insuranceService, new ServiceInsuranceView());
            MainTilLayout.Children.Add(insuranceService);

            var personnelEdit = SamcoAdd.GetTile("تنظیمات پرسنل", TileSize.Large, "ServiceInsuranceIcon.samco");
            Navigation.SetNavigateTo(personnelEdit, new PersonnelEditorView());
            MainTilLayout.Children.Add(personnelEdit);

            var databaseSetting = SamcoAdd.GetTile("تنظیمات پایگاه داده و سیستم", TileSize.Large, "DatabaseIcon.samco");
            Navigation.SetNavigateTo(databaseSetting, new SettingView());
            MainTilLayout.Children.Add(databaseSetting);
        }
        var userSetting = SamcoAdd.GetTile("تنظیمات کاربری", TileSize.Small, "UserSettingIcon.samco", !SamcoAdd.UserIsAdmin(), "تنظیمات");
        Navigation.SetNavigateTo(userSetting, new UserView());
        MainTilLayout.Children.Add(userSetting);
        ((MainWindow)Application.Current.MainWindow)!.WaitIndic.IsSplashScreenShown = false;

        //Load personnel
        if (SamcoSoftShared.LoadedSettings != null && SamcoSoftShared.LoadedSettings.PersonnelRole != null && SamcoAdd.ShiftList.Count == 0)
        {
            var personnelSelector = new WinUIDialogWindow("انتخاب پرسنل شیفت")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new PersonnelSelectorView(),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };

            personnelSelector.ShowDialog();
        }
    }

    private void MaxiTiles_Click(object sender, EventArgs e)
    {
        var selTile = (Tile)sender;
        selTile.IsMaximized = !selTile.IsMaximized;
    }
}