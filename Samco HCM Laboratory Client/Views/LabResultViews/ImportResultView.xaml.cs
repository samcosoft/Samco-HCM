using System.Linq;
using DevExpress.Xpf.Dialogs;
using Samco_HCM_Shared;
using System.Windows;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpo;
using LabData;
using Samco_HCM_Laboratory_Client.Classes;

namespace Samco_HCM_Laboratory_Client.Views.LabResultViews
{
    /// <summary>
    /// Interaction logic for ImportResultView.xaml
    /// </summary>
    public partial class ImportResultView
    {
        public ImportResultView()
        {
            InitializeComponent();

            //Load data from the database
            ColumnCodes.DataContext = SamcoSoftShared.LoadedSettings;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            ProcBtn.IsEnabled = false;
            ConfBtn.IsEnabled = false;

            var openFile = new DXOpenFileDialog
            {
                Title = "انتخاب فایل داده",
                Filter = "Excel files (*.xls, *.xlsx)|*.xls*"
            };
            if (!openFile.ShowDialog().GetValueOrDefault(false)) return;

            if (!ExcelPan.LoadDocument(openFile.FileName))
            {
                MainNotify.ShowError("خطا در خواندن فایل", "فایل قابل خواندن نیست");
                return;
            }

            ProcBtn.IsEnabled = true;
        }

        private void ProcBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CodeColBx.Text) || string.IsNullOrEmpty(NameColBx.Text) ||
                string.IsNullOrEmpty(IdColBx.Text) || string.IsNullOrEmpty(ResultColBx.Text))
            {
                MainNotify.ShowWarning("خطا در خواندن اطلاعات", "لطفا تمام فیلدها را پر کنید");
                return;
            }

            if (ExcelPan.SelectedCell == null)
            {
                MainNotify.ShowWarning("خطا در خواندن اطلاعات", "لطفا اولین سلول اطلاعات را انتخاب کنید");
                return;
            }

            var currentWorksheet = ExcelPan.Document.Worksheets.ActiveWorksheet;
            var firstRow = ExcelPan.SelectedCell.TopRowIndex;
            var visitIdCol = currentWorksheet.Columns[IdColBx.Text].Index;
            var namIdCol = currentWorksheet.Columns[NameColBx.Text].Index;

            for (var i = firstRow; i < currentWorksheet.Rows.LastUsedIndex; i++)
            {
                var visitIdText = currentWorksheet.Rows[i][visitIdCol].Value.ToString();
                if (string.IsNullOrEmpty(visitIdText) || !int.TryParse(visitIdText, out var visitId)) continue;

                //Get visit information from database
                using Session session1 = new();
                var visit = session1.GetObjectByKey<LabVisits>(visitId);

                if (visit == null) continue;

                currentWorksheet.Rows[i][namIdCol].Value = visit.Patient.Name;
            }
            MainNotify.ShowSuccess("بررسی اطلاعات", "اطلاعات با موفقیت بررسی شدند");
            ConfBtn.IsEnabled = true;
        }

        private void ConfBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //Show warning
            if (WinUIMessageBox.Show("آیا از صحت اطلاعات مطمئنید؟ اطلاعات در حال ذخیره در پایگاه داده هستند.", "ثبت اطلاعات", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == MessageBoxResult.No) return;
            //Save data to the database

            var currentWorksheet = ExcelPan.Document.Worksheets.ActiveWorksheet;
            var firstRow = ExcelPan.SelectedCell.TopRowIndex;
            var visitIdCol = currentWorksheet.Columns[IdColBx.Text].Index;
            var codeCol = currentWorksheet.Columns[CodeColBx.Text].Index;
            var resultCol = currentWorksheet.Columns[ResultColBx.Text].Index;

            for (var i = firstRow; i < currentWorksheet.Rows.LastUsedIndex; i++)
            {
                var visitIdText = currentWorksheet.Rows[i][visitIdCol].Value.ToString();
                if (string.IsNullOrEmpty(visitIdText) || !int.TryParse(visitIdText, out var visitId)) continue;

                //Get visit information from database
                using Session session1 = new();
                var visit = session1.GetObjectByKey<LabVisits>(visitId);

                if (visit == null) continue;

                var testCode = currentWorksheet.GetCellValue(codeCol, i).TextValue;
                if (string.IsNullOrEmpty(testCode)) continue;

                var machineCode = session1.FindObject<TestName>(new BinaryOperator(nameof(TestName.MachineCode), testCode));
                if (machineCode == null)
                {
                    WinUIMessageBox.Show(
                        $"برای کد دستگاه {testCode} آزمایشی تعریف نشده است. مقدار این کد در پایگاه داده ذخیره نخواهد شد.",
                        "آزمایش تعریف نشده", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    continue;
                }
               
                //Get labTest
                var testCard = visit.TestCards.FirstOrDefault(x => x.TestName.Oid == machineCode.Oid);
                if (testCard == null)
                {
                    WinUIMessageBox.Show(
                        $"برای بیمار {visit.Patient.Name} آزمایش {machineCode.name} تعریف نشده است. مقدار این کد در پایگاه داده ذخیره نخواهد شد.",
                        "آزمایش تعریف نشده", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None,
                        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    continue;
                }

                //Set result
                testCard.Result = currentWorksheet.GetCellValue(resultCol, i).TextValue;
                testCard.Save();
            }
            MainNotify.ShowSuccess("ذخیره اطلاعات", "اطلاعات با موفقیت ذخیره شدند");
            SamcoSoftShared.LoadedSettings!.Save();
        }
    }
}
