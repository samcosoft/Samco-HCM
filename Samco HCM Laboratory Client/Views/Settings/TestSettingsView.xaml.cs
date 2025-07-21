using DevExpress.Data.Filtering;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LabData;
using Samco_HCM_Laboratory_Client.Classes;
using Samco_HCM_Laboratory_Client.Views.Settings.TestSettingsViews;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Samco_HCM_Laboratory_Client.Views.Settings
{
    /// <summary>
    /// Interaction logic for TestSettingsView.xaml
    /// </summary>
    public partial class TestSettingsView : IDisposable
    {
        private readonly Session _session1 = new();

        public TestSettingsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!SamcoAdd.CheckNetwork())
                MainNotify.ShowWarning("خطای شبکه", "ارتباط با سرور برقرار نیست. امکان دریافت اطلاعات بیمه ها وجود ندارد.");
            else
            {
                //Update insurance data
                try
                {
                    using var remoteSession = new Session(SamcoAdd.RemoteDatalayer);
                    var insList = remoteSession.Query<HCMData.insuranceType>().ToList();
                    if (insList.Count == 0)
                    {
                        MainNotify.ShowWarning("خطای دریافت اطلاعات بیمه", "بیمه ای در پذیرش تعریف نشده است.");
                        return;
                    }

                    foreach (var insuranceType in insList)
                    {
                        var existInsurance = _session1.Query<LabInsuranceType>()
                            .FirstOrDefault(i => i.name == insuranceType.name);
                        if (existInsurance == null)
                        {

                            var newInsurance = new LabInsuranceType
                            {
                                name = insuranceType.name,
                                serverID = insuranceType.Oid

                            };
                            _session1.Save(newInsurance);
                        }
                        else
                        {
                            existInsurance.serverID = insuranceType.Oid;
                            _session1.Save(existInsurance);
                        }
                    }
                    //Delete insurances that no longer exist
                    var existingInsurances = _session1.Query<LabInsuranceType>().ToList();
                    foreach (var insurance in existingInsurances.Where(insurance => insList.All(i => i.name != insurance.name)))
                    {
                        _session1.Delete(insurance);
                    }

                }
                catch (Exception ex)
                {
                    MainNotify.ShowError("خطای اتصال به پایگاه داده", ex.Message);
                    return;
                }
            }
            LoadData();
        }

        private void LoadData()
        {
            InsList.ItemsSource = null;
            TestGroup.ItemsSource = null;
            TestList.ItemsSource = null;
            PriceListGrid.ItemsSource = null;

            InsList.ItemsSource = new XPCollection<LabInsuranceType>(_session1);
            TestGroup.ItemsSource = new XPCollection<TestGroup>(_session1, CriteriaOperator.Parse("name != ?", null), new SortProperty("printOrder", SortingDirection.Ascending));
            TestList.ItemsSource = new XPCollection<TestName>(_session1, CriteriaOperator.Parse("parent Is null"), new SortProperty("printOrder", SortingDirection.Ascending));
            PriceListGrid.ItemsSource = new XPCollection<TestPrice>(_session1, CriteriaOperator.Parse("testName.parent Is null"));
            ((XPCollection<TestName>)TestSubList.ItemsSource)?.Reload();
            TempGroup.ItemsSource = new XPCollection<TestTemplate>(_session1);
            TempEditGroup.DataContext = new TestTemplate(_session1);
        }

        #region InsuranseGroup

        private void InsList_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (InsList.SelectedItem is LabInsuranceType selectedInsurance)
            {
                InsEditBtn.DataContext = selectedInsurance;
                InsFanniBtn.DataContext = selectedInsurance;
            }

        }

        private void InsSvBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (InsEditBtn.DataContext == null) return;
            var selectedInsurance = (LabInsuranceType)InsEditBtn.DataContext;

            if (string.IsNullOrWhiteSpace(InsEditBtn.Text))
            {
                MainNotify.ShowWarning("خطا در ثبت اطلاعات", "لطفاْ درصد فرانشیز بیمه را به صورت عددی بین 0 تا 100 وارد کنید.");
                return;
            }
            selectedInsurance.franchises = Convert.ToInt16(InsEditBtn.Text);
            _session1.Save(selectedInsurance);
        }

        private void InsDelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (InsEditBtn.DataContext == null) return;
            var selectedInsurance = (LabInsuranceType)InsEditBtn.DataContext;
            selectedInsurance.franchises = 0;
            _session1.Save(selectedInsurance);
        }

        private void FanniSvBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (InsFanniBtn.DataContext == null) return;
            var selectedInsurance = (LabInsuranceType)InsFanniBtn.DataContext;

            if (string.IsNullOrWhiteSpace(InsFanniBtn.Text))
            {
                MainNotify.ShowWarning("خطا در ثبت اطلاعات", "لطفاْ حق فنی را به ریال وارد کنید.");
                return;
            }
            selectedInsurance.fanniPrice = Convert.ToInt32(InsFanniBtn.Text);
            _session1.Save(selectedInsurance);
        }

        private void FanniDelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (InsFanniBtn.DataContext == null) return;
            var selectedInsurance = (LabInsuranceType)InsFanniBtn.DataContext;
            selectedInsurance.fanniPrice = 0;
            _session1.Save(selectedInsurance);
        }
        #endregion

        #region TestGroup

        private void TestGroup_OnSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (TestGroup.SelectedItem is TestGroup selectedGroup)
                TestGroupEditBtn.DataContext = selectedGroup;
        }

        private void NewGroupBtn(object sender, RoutedEventArgs e)
        {
            TestGroupEditBtn.DataContext = new TestGroup(_session1);
        }
        private void SaveGroupBtn(object sender, RoutedEventArgs e)
        {
            if (TestGroupEditBtn.DataContext is TestGroup selectedGroup)
            {
                //Check validation
                if (string.IsNullOrWhiteSpace(TestGroupEditBtn.Text))
                {
                    MainNotify.ShowWarning("خطا در ثبت اطلاعات", "لطفاْ نام گروه آزمایش را وارد کنید.");
                    return;
                }
                var existGroup = _session1.FindObject<TestGroup>(CriteriaOperator.Parse("name = ?", TestGroupEditBtn.Text));
                if (existGroup != null && existGroup.Oid != selectedGroup.Oid)
                {
                    MainNotify.ShowWarning("خطا در ثبت اطلاعات", "یک گروه با این نام وجود دارد.");
                    return;
                }

                selectedGroup.name = TestGroupEditBtn.Text;
                selectedGroup.printOrder = _session1.Query<TestGroup>().Count();
                _session1.Save(selectedGroup);
                TestGroupEditBtn.DataContext = new TestGroup(_session1);
                LoadData();
            }
        }
        private void DeleteGroupBtn(object sender, RoutedEventArgs e)
        {
            if (TestGroup.SelectedItem is TestGroup selectedGroup)
            {
                _session1.Delete(selectedGroup);
            }
        }
        private void GroupUpBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestGroup.SelectedItem == null) return;
            var selectedGroup = (TestGroup)TestGroup.SelectedItem;
            if (selectedGroup.printOrder > 1)
                selectedGroup.printOrder -= 1;
            _session1.Save(selectedGroup);

            if (TestGroup.SelectedIndex > 0)
            {
                var previousGroup = (TestGroup)((DevExpress.Xpf.Editors.Popups.EditorListBox)TestGroup.EditCore).Items[TestGroup.SelectedIndex - 1]!;
                previousGroup.printOrder += 1;
                _session1.Save(previousGroup);
            }
        }

        private void GroupDownBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestGroup.SelectedItem == null) return;
            var selectedGroup = (TestGroup)TestGroup.SelectedItem;
            var maxCount = _session1.Query<TestGroup>().Count();
            if (selectedGroup.printOrder < maxCount)
                selectedGroup.printOrder += 1;
            _session1.Save(selectedGroup);

            if (TestGroup.SelectedIndex < maxCount - 1)
            {
                var previousGroup = (TestGroup)((DevExpress.Xpf.Editors.Popups.EditorListBox)TestGroup.EditCore).Items[TestGroup.SelectedIndex + 1]!;
                previousGroup.printOrder -= 1;
                _session1.Save(previousGroup);
            }
        }
        #endregion

        #region TestName

        private void NewTestBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var newWindows = new WinUIDialogWindow("تعریف آزمایش جدید")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new NewTestView(),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newWindows.ShowDialog() == true) LoadData();
        }
        private void EditTestBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedItem == null)
            {
                MainNotify.ShowWarning("خطا", "لطفاً یک تست را انتخاب کنید.");
                return;
            }
            var newWindows = new WinUIDialogWindow("تعریف آزمایش جدید")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new NewTestView((int)TestList.EditValue),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newWindows.ShowDialog() == true) LoadData();
        }
        private void DelTestBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedItem is null)
            {
                MainNotify.ShowError("خطا در حذف آزمایش", "لطفاً یک مورد را انتخاب نمایید.");
                return;
            }
            // Show confirmation
            var selTest = _session1.GetObjectByKey<TestName>(TestList.EditValue);
            if (WinUIMessageBox.Show(this, $"آیا از حذف آزمایش {selTest.name} مطمئن هستید؟ تمامی اطلاعات مربوط به این آزمایش و زیرمجموعه های آن نیز حذف خواهند شد.", "تأیید حذف آزمایش", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
            {
                selTest.Delete();
                LoadData();
            }
        }
        private void TestList_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            TestSubList.ItemsSource = null;
            if (e.NewValue == null) return;
            var selTest = _session1.GetObjectByKey<TestName>((int)e.NewValue);
            if (selTest.TestNameCollection.Any()) TestSubList.ItemsSource = new XPCollection<TestName>(_session1, CriteriaOperator.Parse("parent = ?", selTest),
                new SortProperty("printOrder", SortingDirection.Ascending));
        }

        private void TestUpBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedItem == null) return;
            var selectedTest = (TestName)TestList.SelectedItem;
            if (selectedTest.printOrder > 1)
                selectedTest.printOrder -= 1;
            _session1.Save(selectedTest);

            if (TestList.SelectedIndex > 0)
            {
                var previousTest = (TestName)((DevExpress.Xpf.Editors.Popups.EditorListBox)TestList.EditCore).Items[TestList.SelectedIndex - 1]!;
                previousTest.printOrder += 1;
                _session1.Save(previousTest);
            }
        }

        private void TestDownBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedItem == null) return;
            var selectedTest = (TestName)TestList.SelectedItem;
            var maxCount = _session1.Query<TestName>().Count(x => x.parent == null);
            if (selectedTest.printOrder < maxCount)
                selectedTest.printOrder += 1;
            _session1.Save(selectedTest);

            if (TestList.SelectedIndex < maxCount - 1)
            {
                var previousTest = (TestName)((DevExpress.Xpf.Editors.Popups.EditorListBox)TestList.EditCore).Items[TestList.SelectedIndex + 1]!;
                previousTest.printOrder -= 1;
                _session1.Save(previousTest);
            }
        }

        private void NewSubBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedItem is null)
            {
                MainNotify.ShowError("خطا در افزودن زیر مجموعه آزمایش", "لطفاً یک آزمایش را انتخاب نمایید.");
                return;
            }

            var selTest = (TestName)TestList.SelectedItem;
            var newWindows = new WinUIDialogWindow($"تعریف زیرمجموعه برای {selTest.name}")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new NewTestView(selTest),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newWindows.ShowDialog() == true) LoadData();
        }

        private void EditSubBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestSubList.SelectedItem is null)
            {
                MainNotify.ShowError("خطا در ویرایش زیر مجموعه آزمایش", "لطفاً یک آزمایش را انتخاب نمایید.");
                return;
            }
            var selTest = (TestName)TestSubList.SelectedItem;
            var newWindows = new WinUIDialogWindow($"تعریف زیرمجموعه برای {selTest.parent.name}")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new NewTestView(selTest.Oid),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newWindows.ShowDialog() == true) LoadData();
        }

        private void DelSubBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestSubList.SelectedItem is null)
            {
                MainNotify.ShowError("خطا در حذف زیر مجموعه", "لطفاً یک مورد را انتخاب نمایید.");
                return;
            }
            // Show confirmation
            var selTest = _session1.GetObjectByKey<TestName>(TestSubList.EditValue);
            if (WinUIMessageBox.Show(this, $"آیا از حذف زیر مجموعه {selTest.name} مطمئن هستید؟", "تأیید حذف آزمایش", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
            {
                selTest.Delete();
                LoadData();
            }
        }

        private void TestSubUpBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestSubList.SelectedItem == null) return;
            var selectedTest = (TestName)TestSubList.SelectedItem;
            if (selectedTest.printOrder > 1)
                selectedTest.printOrder -= 1;
            _session1.Save(selectedTest);

            if (TestSubList.SelectedIndex > 0)
            {
                var previousTest = (TestName)((DevExpress.Xpf.Editors.Popups.EditorListBox)TestSubList.EditCore).Items[TestSubList.SelectedIndex - 1]!;
                previousTest.printOrder += 1;
                _session1.Save(previousTest);
            }
        }

        private void TestSubDownBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (TestSubList.SelectedItem == null) return;
            var selectedTest = (TestName)TestSubList.SelectedItem;
            var maxCount = selectedTest.parent.TestNameCollection.Count;
            if (selectedTest.printOrder < maxCount)
                selectedTest.printOrder += 1;
            _session1.Save(selectedTest);

            if (TestSubList.SelectedIndex < maxCount - 1)
            {
                var previousTest = (TestName)((DevExpress.Xpf.Editors.Popups.EditorListBox)TestSubList.EditCore).Items[TestSubList.SelectedIndex + 1]!;
                previousTest.printOrder -= 1;
                _session1.Save(previousTest);
            }
        }
        #endregion

        #region Price Editor

        private void EditPriceBtn_Click(object sender, RoutedEventArgs e)
        {
            var newWindows = new WinUIDialogWindow("ویرایش تعرفه ها")
            {
                FlowDirection = FlowDirection.RightToLeft,
                Content = new NewPriceView(),
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Vazirmatn"),
                FontSize = 16
            };
            if (newWindows.ShowDialog() == true) LoadData();
        }

        private void DeletePriceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PriceListGrid.SelectedItem is null)
            {
                MainNotify.ShowError("خطا در حذف تعرفه", "لطفاً یک مورد را انتخاب نمایید.");
                return;
            }
            // Show confirmation
            var selPrice = (TestPrice)PriceListGrid.SelectedItem;
            if (WinUIMessageBox.Show(this, $"آیا از حذف تعرفه بیمه {selPrice.insType.name} برای آزمایش {selPrice.testName.name} مطمئن هستید؟", "تأیید حذف تعرفه", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
            {
                selPrice.Delete();
                LoadData();
            }
        }
        #endregion

        #region TestTemplates

        private void TempGroup_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            TestListToken.Clear();
            if (TempGroup.SelectedItem == null) return;
            var selTemp = (TestTemplate)TempGroup.SelectedItem;
            TempEditGroup.DataContext = selTemp;
            foreach (var selTempTestName in selTemp.TestNames)
            {
                TestListToken.SelectedItems.Add(selTempTestName);
            }

            TempShortBx.Text = ((Key)selTemp.ShortKey).ToString();
        }

        private void NewTemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            TestListToken.Clear();
            TempEditGroup.DataContext = new TestTemplate(_session1);
        }

        private void SaveTemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            var template = (TestTemplate)TempEditGroup.DataContext;
            if (string.IsNullOrEmpty(TestTempBtn.Text))
            {
                MainNotify.ShowWarning("خطا در ذخیره نمونه", "نام نمونه را وارد کنید.");
                return;
            }

            template.Name = TestTempBtn.Text;
            template.Save();

            foreach (var result in TestListToken.SelectedItems.Cast<TestName>()) template.TestNames.Add(result);
            template.Save();

            TempGroup.ItemsSource = new XPCollection<TestTemplate>(_session1);
        }

        private void DeleteTemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TempGroup.SelectedItem == null)
            {
                MainNotify.ShowWarning("خطا در حذف نمونه", "لطفاً یک نمونه را انتخاب کنید.");
                return;
            }
            var selTemp = (TestTemplate)TempGroup.SelectedItem;
            if (WinUIMessageBox.Show(this, $"آیا از حذف نمونه {selTemp.Name} مطمئن هستید؟", "تأیید حذف نمونه", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
            {
                selTemp.Delete();
                TempGroup.ItemsSource = new XPCollection<TestTemplate>(_session1);
            }
        }

        private void ShortcutKey_KeyDown(object sender, KeyEventArgs e)
        {
            var template = (TestTemplate)TempEditGroup.DataContext;
            if (e.Key == Key.Escape)
            {
                template.ShortKey = 0;
                TempShortBx.Text =string.Empty;
                return;
            }

            template.ShortKey = (byte)e.Key;
            TempShortBx.Text = e.Key.ToString();
        }

        #endregion

        public void Dispose()
        {
            _session1.Dispose();
        }

    }
}
