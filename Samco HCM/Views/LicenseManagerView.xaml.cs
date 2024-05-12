using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Samco_HCM.Classes;

namespace Samco_HCM.Views;

/// <summary>
/// Interaction logic for LicenseManagerView.xaml
/// </summary>
public partial class LicenseManagerView
{
    private int _pageIndex;
    public LicenseManagerView()
    {
        InitializeComponent();

        //Set Settings
        if (SamcoAdd.License.IsEvaluationLicense())
        {
            if (!SamcoAdd.License.IsEvaluationExpired())
            {
                StatusText.Text = "آزمایشی";
                StatusBorder.Background = new SolidColorBrush(Colors.DarkOrange);
                var style = SamcoAdd.License.RemainingUsageDays switch
                {
                    < 10 => FindResource("ProgressBarDangerWave") as Style,
                    < 20 => FindResource("ProgressBarWarningWave") as Style,
                    < 31 => FindResource("ProgressBarSuccessWave") as Style,
                    _ => FindResource("ProgressBarPrimaryWave") as Style
                };
                StatusBar.Style = style;
                StatusBar.Value = SamcoAdd.License.RemainingUsageDays;
            }
            else
            {
                TrialButton.IsEnabled = false;
                StatusText.Text = "منقضی";
                StatusBorder.Background = new SolidColorBrush(Colors.Red);
                StatusBar.Style = FindResource("ProgressBarDangerWave") as Style;
                StatusBar.Value = 0;
            }
            StatusBar.Text = $"{SamcoAdd.License.RemainingUsageDays} روز";
        }
        else
        {
            //Valid License
            TrialButton.IsEnabled = false;
            ActivationButton.IsEnabled = false;
            DeactivateButton.IsEnabled = true;
            StatusText.Text = "معتبر";
            StatusBorder.Background = new SolidColorBrush(Colors.Green);
            StatusBar.Style = FindResource("ProgressBarSuccessWave") as Style;
            StatusBar.Text = "نامحدود";
            StatusBar.Value = 30;
            ActivationWizard.AllowNext = false;
        }

        ErrorMessageBx.Text = string.Empty;
        ActiveErrorGroup.Visibility = Visibility.Collapsed;
        LicenseCodeErrorBox.Visibility = Visibility.Collapsed;
        LicenseErrorBox.Text = string.Empty;
    }

    private void OpenSiteButton(object sender, RoutedEventArgs e)
    {
        const string url = "https://samcosoft.ir";
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
    }

    private void ActivationWizard_OnNext(object sender, CancelEventArgs e)
    {
        switch (_pageIndex)
        {
            case 0:
                {
                    //Welcome Page
                    if (TrialButton.IsChecked.GetValueOrDefault())
                    {
                        ((DXDialogWindow)Parent).DialogResult = true;
                        return;
                    }
                    //Goto activation pane
                    _pageIndex = 1;
                    break;
                }
            case 1:
                {
                    //Validate info
                    if (string.IsNullOrEmpty(SerialBox.Text))
                    {
                        MainNotify.ShowWarning("خطا در فعالسازی", "لطفاً اطلاعات خواسته شده را وارد کنید.");
                        e.Cancel = true;
                        return;
                    }
                    if (SamcoAdd.CheckForActiveConnection() == false)
                    {
                        MainNotify.ShowWarning("خطا در فعالسازی", "برای فعالسازی ارتباط اینترنتی لازم است.");
                        e.Cancel = true;
                        return;
                    }

                    if (SamcoAdd.ActivateBySerial(SerialBox.Text, out var message, out _))
                    {
                        //Activation successful
                        ActivationWizard.AllowCancel = false;
                        ActivationWizard.AllowBack = false;
                        _pageIndex = 3;
                    }
                    else
                    {
                        ErrorMessageBx.Text = message;
                        ActiveErrorGroup.Visibility = Visibility.Visible;
                    }
                    break;
                }
            //Offline Activation
            case 2:
                {
                    if (string.IsNullOrEmpty(LicenseBox.Text))
                    {
                        MainNotify.ShowWarning("خطا در فعالسازی", "لطفاً اطلاعات مجوز را وارد کنید.");
                        e.Cancel = true;
                        return;
                    }

                    if (SamcoAdd.ActivateByLicenseCode(LicenseBox.Text, out var message))
                    {
                        //Activation successful
                        ActivationWizard.AllowCancel = false;
                        ActivationWizard.AllowBack = false;
                        _pageIndex = 3;
                    }
                    else
                    {
                        LicenseCodeErrorBox.Visibility = Visibility.Visible;
                        LicenseErrorBox.Text = message;
                    }
                    break;
                }
        }
        ActivationWizard.SelectedIndex = _pageIndex;
        e.Cancel = true;
    }

    private void ActivationWizard_OnBack(object sender, CancelEventArgs e)
    {
        _pageIndex = _pageIndex switch
        {
            //Activation with serial
            1 =>
                //Goto Welcome Page
                0,
            //Activation with license
            2 =>
                //Goto Activation Page
                0,
            _ => _pageIndex
        };
        ActivationWizard.SelectedIndex = _pageIndex;
        e.Cancel = true;
    }

    private void OfflineButtonClick(object sender, RoutedEventArgs e)
    {
        //Offline activation
        MachineCode.Text = SamcoAdd.License.GetLocalMachineCodeAsString();
        _pageIndex = 2;
        ActivationWizard.SelectedIndex = _pageIndex;
    }

    private void ActivationWizard_OnFinish(object sender, CancelEventArgs e)
    {
        ((DXDialogWindow)Parent).DialogResult = true;
    }

    private void DeactivateButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (WinUIMessageBox.Show(this, "آیا از غیر فعالسازی مجوز برنامه مطمئنید؟ توجه کنید که برای استفاده مجدد از برنامه باید آن را فعال نمایید.",
               "غیر فعال سازی", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading, FloatingMode.Window) == MessageBoxResult.No) return;
        if (SamcoAdd.DeactivateLicense())
        {
            Application.Current.Shutdown();
        }
    }
}