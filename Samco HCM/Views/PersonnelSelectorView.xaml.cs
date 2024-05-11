using System;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using HandyControl.Tools.Extension;
using HCMData;
using Samco_HCM.Classes;
using Samco_HCM_Shared;

namespace Samco_HCM.Views
{
    /// <summary>
    /// Interaction logic for PersonnelSelectorView.xaml
    /// </summary>
    public partial class PersonnelSelectorView
    {
        private readonly Session _session1 = new();

        public PersonnelSelectorView()
        {
            InitializeComponent();
            foreach (var itm in SamcoSoftShared.LoadedSettings!.PersonnelRole!)
            {
                var personList = _session1.Query<Personnel>().Where(x => x.Role == itm).ToList();
                if (personList.Count != 0)
                {
                    var newGrp = new LayoutGroup { Header = itm, View = LayoutGroupView.GroupBox, AnimateScrolling = true, MaxHeight = 500 };
                    newGrp.Children.Add(new ListBoxEdit { ItemsSource = personList, ShowCustomItems = true, DisplayMember = "Name", ValueMember = "Oid", StyleSettings = new CheckedListBoxEditStyleSettings(), MaxHeight = 500 });
                    FlowList.Children.Add(newGrp);
                }
            }
            try
            {
                RealUserBx.Text = string.IsNullOrEmpty(SamcoSoftShared.CurrentUser!.RealName) ? SamcoSoftShared.CurrentUser!.Username : SamcoSoftShared.CurrentUser!.RealName;
            }
            catch (Exception)
            {
                // ignored
            }
        }
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            SamcoAdd.ShiftList.Clear();
            var groupList = FlowList.Children.OfType<LayoutGroup>();
            foreach (var grp in groupList)
            {
                var selList = grp.Children[1].CastTo<ListBoxEdit>();
                SamcoAdd.ShiftList.Add(grp.Header.ToString()!, selList.SelectedItems.Cast<Personnel>().Select(x => x.Oid).ToList());
            }
            ((WinUIDialogWindow)Parent).DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ((WinUIDialogWindow)Parent).DialogResult = false;
        }
    }
}
