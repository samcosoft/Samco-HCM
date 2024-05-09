using System;
using System.Windows;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using Samco_HCM.Classes;
using Samco_HCM_Shared;

namespace Samco_HCM.Views.Settings.ServiceInsuranceViews
{
    /// <summary>
    /// Interaction logic for NewRoleView.xaml
    /// </summary>
    public partial class NewRoleView : IDisposable
    {
        public string SelRole;
        private readonly Session _session1 = new();
        private bool _disposed;

        public NewRoleView()
        {
            InitializeComponent();
        }

        public NewRoleView(string selRole)
        {
            InitializeComponent();
            SelRole = selRole;
            RoleBox.EditValue = SelRole;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _session1?.Dispose();
            }
            _disposed = true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RoleBox.Text))
            {
                MainNotify.ShowError("خطا در افزودن نقش", "لطفاً نام نقش را وارد کنید.");
                return;
            }

            if (string.IsNullOrEmpty(SelRole))
            {
                if (SamcoSoftShared.LoadedSettings!.PersonnelRole!.Contains(RoleBox.Text) == false)
                {
                    SamcoSoftShared.LoadedSettings!.PersonnelRole!.Add(RoleBox.Text);
                    ((WinUIDialogWindow)Parent).DialogResult = true;
                }
                else
                {
                    MainNotify.ShowWarning("خطا در ثبت نقش", "این نقش قبلاً ثبت شده است.");
                }
            }
            else
            {
                if (SelRole == RoleBox.Text)
                {
                    ((WinUIDialogWindow)Parent).DialogResult = true;
                    return;
                }

                if (SamcoSoftShared.LoadedSettings!.PersonnelRole!.Contains(RoleBox.Text) == false)
                {
                    SamcoSoftShared.LoadedSettings!.PersonnelRole!.Remove(SelRole);
                    SamcoSoftShared.LoadedSettings!.PersonnelRole!.Add(RoleBox.Text);
                    ((WinUIDialogWindow)Parent).DialogResult = true;
                }
                else
                {
                    MainNotify.ShowWarning("خطا در ثبت نقش", "این نقش قبلاً ثبت شده است.");
                }
            }
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ((WinUIDialogWindow)Parent).DialogResult = false;
        }
    }
}
