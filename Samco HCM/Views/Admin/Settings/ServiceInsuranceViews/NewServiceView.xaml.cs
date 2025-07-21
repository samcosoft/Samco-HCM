using System;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM_Shared;

namespace Samco_HCM.Views.Settings.ServiceInsuranceViews;

/// <summary>
/// Interaction logic for NewServiceView.xaml
/// </summary>
public partial class NewServiceView : IDisposable
{
    private readonly Session _session1 = new();
    private readonly HealthServices _healthServices;
    public NewServiceView()
    {
        InitializeComponent();
        _healthServices = new HealthServices(_session1);
        MainInfoGrp.DataContext = _healthServices;
        if (SamcoSoftShared.LoadedSettings?.PersonnelRole != null)
            foreach (var itm in SamcoSoftShared.LoadedSettings?.PersonnelRole!)
            {
                RoleSelBx.Items.Add(new ComboBoxEditItem { Content = itm });
            }
        EquipCombo.ItemsSource = new XPCollection<Equipments>(_session1);
    }

    public NewServiceView(int selSvcId)
    {
        InitializeComponent();
        if (SamcoSoftShared.LoadedSettings?.PersonnelRole != null)
            foreach (var itm in SamcoSoftShared.LoadedSettings?.PersonnelRole!)
            {
                RoleSelBx.Items.Add(new ComboBoxEditItem { Content = itm });
            }

        _healthServices = _session1.GetObjectByKey<HealthServices>(selSvcId);
        MainInfoGrp.DataContext = _healthServices;
        DefEquipGrp.Visibility = Visibility.Visible;
        EquipGrid.ItemsSource = _healthServices.DefEquips;
        EquipCombo.ItemsSource = new XPCollection<Equipments>(_session1);
        RoleSelBx.Text = _healthServices.ProviderRole;
        //ImageBx.EditValue = _healthServices.image;

        var cbxItem = ServiceTypeBx.Items.Cast<ComboBoxEditItem>()
            .Where(itm => itm.Tag.Equals(_healthServices.group)).ToList();

        if (cbxItem.Any())
        {
            ServiceTypeBx.SelectedItem = cbxItem.First();
        }

        //Add parent combobox data for emergency box
        AddParentBoxItem(_healthServices.group);
        if (_healthServices.parent != null)
        //Select appropriate parentBx item
        {
            var parentItem = ParentBx.Items.Cast<ComboBoxEditItem>()
                .Where(itm => itm.Tag.Equals(_healthServices.parent.Oid)).ToList();

            if (parentItem.Count != 0) ParentBx.SelectedItem = parentItem.First();
        }
    }

    private void AddParentBoxItem(string groupName)
    {
        ParentBx.Items.Clear();
        var serviceList = _session1.Query<HealthServices>().Where(x => x.parent == null && x.group == groupName).ToList();

        foreach (var service in serviceList)
        {
            ParentBx.Items.Add(new ComboBoxEditItem { Content = service.name, Tag = service.Oid });
        }
    }


    private void ServiceNameBx_OnValidate(object sender, ValidationEventArgs e)
    {
        if (e.Value == null)
        {
            e.SetError("وارد نمودن نام خدمت الزامیست.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
        }
        else
        {
            e.SetError(null);
        }
    }

    private void ServiceTypeBx_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
    {
        AddParentBoxItem(((ComboBoxEditItem)ServiceTypeBx.SelectedItem).Tag.ToString());
    }

    private void RoleSelBx_OnValidate(object sender, ValidationEventArgs e)
    {
        if (e.Value == null)
        {
            e.SetError("انتخاب حداقل یک نقش الزامی است.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
        }
        else
        {
            e.SetError(null);
        }
    }

    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (ServiceNameBx.HasValidationError || RoleSelBx.HasValidationError)
        {
            MainNotify.ShowError("خطا در ویرایش خدمت", "لطفاً اطلاعات خدمت را به درستی وارد نمایید.");
            return;
        }
        _healthServices.group = ((ComboBoxEditItem)ServiceTypeBx.SelectedItem).Tag.ToString();
        //if (ImageBx.EditValue != null)
        //{
        //    if (ImageBx.EditValue is BitmapImage)
        //    {
        //        _healthServices.image = ((BitmapImage)ImageBx.EditValue).StreamSource, IO.MemoryStream).ToArray

        //    }
        //    Else
        //    SelService.image = CType(imageBx.EditValue, Byte())
        //    End If
        //}
        //else
        //{
        //    _healthServices.image = null;
        //}
        HealthServices parent = null;
        if (ParentBx.SelectedItem != null)
        {
            parent = _session1.GetObjectByKey<HealthServices>(
                ((ComboBoxEditItem)ParentBx.SelectedItem).Tag.ToString());
        }

        //Validation
        var oldItem  = _session1.Query<HealthServices>()
            .FirstOrDefault(x => x.name == _healthServices.name && ReferenceEquals(x.parent, parent));
        if (_healthServices.Oid < 0)
        {
            if (oldItem != null)
            {
                MainNotify.ShowError("خطا در ثبت خدمت", "این خدمت قبلاً در این بخش ثبت شده است.");
                return;
            }
        }
        else
        {
            if (oldItem != null && oldItem.Oid != _healthServices.Oid)
            {
                MainNotify.ShowError("خطا در ثبت خدمت", "این خدمت قبلاً در این بخش ثبت شده است.");
                return;
            }
        }

        _healthServices.parent = parent;
        _healthServices.ProviderRole = RoleSelBx.Text;
        _healthServices.Save();
        ((WinUIDialogWindow)Parent).DialogResult = true;
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        ((WinUIDialogWindow)Parent).DialogResult = false;
    }

    private void AddEquipBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (EquipCombo.SelectedItem == null || EquipCount.HasValidationError)
        {
            MainNotify.ShowError("خطا در ثبت تجهیز", "لطفاً از لیست بالا یک مورد را انتخاب کنید.");
            return;
        }

        if (_healthServices.DefEquips.Any(x => x.EquipName.Oid == ((Equipments)EquipCombo.SelectedItem).Oid))
        {
            MainNotify.ShowError("خطا در افزودن تجهیز",
                "این تجهیز قبلاً اضافه شده است. برای ویرایش تعداد آن از لیست بالا آن را تغییر دهید.");
            return;

        }

        var newEquip = new DefEquip(_session1)
        {
            EquipName = (Equipments)EquipCombo.SelectedItem,
            Count = int.Parse(EquipCount.Text),
            Service = _healthServices
        };
        newEquip.Save();
    }

    private void DelEquipBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (EquipGrid.SelectedItem == null)
        {
            MainNotify.ShowError("خطا در حذف تجهیز", "لطفاً از لیست بالا یک مورد را انتخاب کنید.");
            return;
        }
        ((DefEquip)EquipGrid.SelectedItem).Delete();
    }

    public void Dispose()
    {
        _session1?.Dispose();
    }
}