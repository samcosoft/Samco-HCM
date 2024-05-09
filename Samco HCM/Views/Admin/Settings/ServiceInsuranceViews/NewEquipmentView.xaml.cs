using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;

namespace Samco_HCM.Views.Settings.ServiceInsuranceViews
{
    /// <summary>
    /// Interaction logic for NewEquipmentView.xaml
    /// </summary>
    public partial class NewEquipmentView : IDisposable
    {
        private readonly Session _session1 = new();
        private readonly Equipments _selEquipments;
        public NewEquipmentView()
        {
            InitializeComponent();
            _selEquipments =  new Equipments(_session1);
            DataContext = _selEquipments;
        }

        public NewEquipmentView(Equipments selEquipments)
        {
            _selEquipments = _session1.GetObjectByKey<Equipments>(selEquipments.Oid);
            DataContext = _selEquipments;
        }
        private void EquipBx_OnValidate(object sender, ValidationEventArgs e)
        {
            e.SetError(e.Value == null ? "نام وسیله الزامی است." : default(object));
        }

        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ((WinUIDialogWindow)Parent).DialogResult = false;
        }

        private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (EquipBx.HasValidationError)
            {
                MainNotify.ShowError("خطا در ثبت تجهیز", "لطفاً نام تجهیز را وارد کنید.");
                return;
            }
            //Check name not added before
            var prevEquipment = _session1.Query<Equipments>().Where(x => x.Name == EquipBx.Text).ToList();
            if (prevEquipment.Any(x=>x.Oid!=_selEquipments.Oid))
            {
                MainNotify.ShowError("خطا در ثبت تجهیز", "تجهیزی قبلاً با همین نام ثبت شده است.");
                return;
            }
            _selEquipments.Save();
            ((WinUIDialogWindow)Parent).DialogResult = true;
        }

        public void Dispose()
        {
            _session1?.Dispose();
        }
    }
}
