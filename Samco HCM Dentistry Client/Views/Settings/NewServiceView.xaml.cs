using System.Linq;
using System.Windows;
using DentData;
using DevExpress.Xpf.Editors;
using DevExpress.Xpo;
using Samco_HCM_Dentistry_Client.Classes;

namespace Samco_HCM_Dentistry_Client.Views;

/// <summary>
/// Interaction logic for NewServiceView.xaml
/// </summary>
public partial class NewServiceView
{
    private readonly Session _session1 = new();
    private readonly Services _dentalServices;
    public NewServiceView()
    {
        InitializeComponent();

        _dentalServices = new Services(_session1);
        DataContext = _dentalServices;
    }

    public NewServiceView(int selSvcId)
    {
        InitializeComponent();
        _dentalServices = _session1.GetObjectByKey<Services>(selSvcId);
        DataContext = _dentalServices;
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


    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ServiceNameBox.HasValidationError)
        {
            MainNotify.ShowError("خطا در ویرایش خدمت", "لطفاً نام خدمت را وارد نمایید.");
            return;
        }

        //validation

        var oldService = _session1.Query<Services>().FirstOrDefault(x => x.name == _dentalServices.name);
        if (oldService != null && oldService.Oid != _dentalServices.Oid)
        {
            MainNotify.ShowError("خطا در ویرایش خدمت", "این خدمت قبلاً ثبت شده است.");
            return;
        }


        _dentalServices.Save();
        MainNotify.ShowSuccess("ذخیره موفقیت آمیز", "خدمت با موفقیت ذخیره شد.");
    }
}