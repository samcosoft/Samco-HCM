using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.WindowsUI.Navigation;
using Samco_HCM_Laboratory_Client.Classes;
using System.Windows;
using Samco_HCM_Laboratory_Client.Views.Settings;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;

namespace Samco_HCM_Laboratory_Client.Views;

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
        //Load tiles

        var admissionTile =SamcoAdd.GetTile("پذیرش", TileSize.Small, "ReceptionIcon.samco", true, "آزمایشگاه");
        Navigation.SetNavigateTo(admissionTile, new NewVisitView());
        MainTilLayout.Children.Add(admissionTile);

        var resultTile = SamcoAdd.GetTile("ثبت نتایج", TileSize.Small, "LabResultIcon.samco");
        Navigation.SetNavigateTo(resultTile, new LabResultView());
        MainTilLayout.Children.Add(resultTile);

        //Create statistics Tiles
        var allStat = SamcoAdd.GetTile("آمار", TileSize.Small, "AllStatIcon.samco");
        //Navigation.SetNavigateTo(allStat, new Statistics());
        MainTilLayout.Children.Add(allStat);

        if (SamcoAdd.UserIsAdmin())
        {
            //Create settings tiles
            var insuranceService = SamcoAdd.GetTile("تنظیمات بیمه و آزمایشات", TileSize.Large, "ServiceInsuranceIcon.samco", true, "تنظیمات");
            Navigation.SetNavigateTo(insuranceService, new TestSettingsView());
            MainTilLayout.Children.Add(insuranceService);

            var machineSetting = SamcoAdd.GetTile("تنظیمات کدهای دستگاه", TileSize.Large, "ServiceInsuranceIcon.samco");
            Navigation.SetNavigateTo(machineSetting, new TestCodeSettings());
            MainTilLayout.Children.Add(machineSetting);

            var databaseSetting = SamcoAdd.GetTile("تنظیمات پایگاه داده و سیستم", TileSize.Large, "DatabaseIcon.samco");
            Navigation.SetNavigateTo(databaseSetting, new SettingView());
            MainTilLayout.Children.Add(databaseSetting);
        }
        var userSetting = SamcoAdd.GetTile("تنظیمات کاربری", TileSize.Small, "UserSettingIcon.samco", !SamcoAdd.UserIsAdmin(), "تنظیمات");
        //Navigation.SetNavigateTo(userSetting, new UserView());
        MainTilLayout.Children.Add(userSetting);
        // ReSharper disable once AssignNullToNotNullAttribute
        ((MainWindow)Application.Current.MainWindow).WaitIndic.IsSplashScreenShown = false;
    }
}