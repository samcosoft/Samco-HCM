using DevExpress.Xpo;
using System;
using System.Windows;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Editors;
using HCMData;
using DevExpress.Xpf.WindowsUI;
using Samco_HCM.Classes;

namespace Samco_HCM.Views.Settings.ServiceInsuranceViews
{
    public partial class NewInsuranceView : IDisposable
    {
        public insuranceType SelInsurance;
        private readonly Session _session1 = new();
        private bool _disposed;


        public NewInsuranceView()
        {
            InitializeComponent();
            SelInsurance = new insuranceType(_session1);
            DataContext = SelInsurance;
        }

        public NewInsuranceView(int oid)
        {
            InitializeComponent();
            SelInsurance = _session1.GetObjectByKey<insuranceType>(oid);
            DataContext = SelInsurance;
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

        private void InsuranceName_Validate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.SetError("وارد نمودن نام بیمه الزامیست.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                return;
            }

            if (SelInsurance.Oid < 0 && _session1.FindObject<insuranceType>(new BinaryOperator(nameof(insuranceType.name), e.Value.ToString())) != null)
            {
                e.SetError("یک بیمه با نام مشابه وجود دارد.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                return;
            }
            e.SetError(null);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InsuranceName.HasValidationError)
            {
                MainNotify.ShowError("خطای ثبت بیمه", "وارد نمودن نام بیمه الزامیست.");
                return;
            }
            SelInsurance.Save();
            ((WinUIDialogWindow)Parent).DialogResult = true;
        }

        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ((WinUIDialogWindow)Parent).DialogResult = false;
        }
    }
}
