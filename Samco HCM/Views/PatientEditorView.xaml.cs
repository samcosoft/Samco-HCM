using System;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI.Navigation;
using Samco_HCM.Classes;

namespace Samco_HCM.Views
{
    /// <summary>
    /// Interaction logic for PatientEditorView.xaml
    /// </summary>
    public partial class PatientEditorView : IDisposable
    {
        private Session _session1;
        private PatientInfo _selPat;
        public PatientEditorView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _session1 = new Session();
            LoadData();
            base.OnNavigatedTo(e);
        }

        private void LoadData()
        {
            PatientGrid.ItemsSource = new XPServerCollectionSource(_session1, _session1.GetClassInfo<PatientInfo>()) { AllowEdit = true };
            VisitGrid.ItemsSource = new XPServerCollectionSource(_session1, _session1.GetClassInfo<Visits>());
            ReserveGrid.ItemsSource = new XPCollection<Reserves>(_session1);
        }

        private void PatientGrid_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (PatientGrid.SelectedItem is null)
                return;
            _selPat = (PatientInfo)PatientGrid.SelectedItem;
            VisitGrid.FilterCriteria = CriteriaOperator.Parse("patient.Oid = ?", _selPat.Oid);
            ReserveGrid.FilterCriteria = CriteriaOperator.Parse("Patient.Oid = ?", _selPat.Oid);
        }
        //private void PatientAddBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var patUi = new WinUIDialogWindow { Content = new PatientEditView() };
        //    patUi.ShowDialog();
        //    PatientGrid.ItemsSource = new XPServerCollectionSource(_session1, _session1.GetClassInfo<PatientInfo>()) { AllowEdit = true };
        //}

        private void PatientDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selPat is null)
            {
                MainNotify.ShowWarning("خطا در حذف بیمار", "لطفاً از لیست بالا یک نفر را انتخاب کنید.");
                return;
            }

            if (WinUIMessageBox.Show(this, "حذف اطلاعات بیمار", $"آیا از حذف اطلاعات بیمار {_selPat.name} مطمئنید؟", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Adorner) == MessageBoxResult.No) return;

            _selPat.Delete();
            LoadData();
        }

        private void BillDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (VisitGrid.SelectedItem is null)
            {
                WinUIMessageBox.Show(this, "لطفاً از لیست بالا یک ویزیت را انتخاب کنید.", "خطا در حذف قبض", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Window);
                return;
            } ((Visits)VisitGrid.SelectedItem).Delete();
            VisitGrid.ItemsSource = new XPServerCollectionSource(_session1, _session1.GetClassInfo<Visits>());
        }

        private void ReserveDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ReserveGrid.SelectedItem is null)
            {
                WinUIMessageBox.Show(this, "لطفاً از لیست بالا یک رزرو را انتخاب کنید.", "خطا در حذف رزرو", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, FloatingMode.Window);
                return;
            } ((Reserves)ReserveGrid.SelectedItem).Delete();
            ReserveGrid.ItemsSource = new XPCollection<Reserves>(_session1);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _session1?.Dispose();
            base.OnNavigatedFrom(e);
        }

        public void Dispose()
        {
            _session1?.Dispose();
        }
    }
}
