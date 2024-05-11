using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM_Shared;

namespace Samco_HCM.Views
{
    /// <summary>
    /// Interaction logic for PersonnelEditorView.xaml
    /// </summary>
    public partial class PersonnelEditorView: IDisposable
    {
        private readonly Session _session1 = new();
        public PersonnelEditorView()
        {
            InitializeComponent();
            DataContext= this;
        }

        public List<string> AvailableRoles
        {
            get => SamcoSoftShared.LoadedSettings!.PersonnelRole;
            set => SamcoSoftShared.LoadedSettings!.PersonnelRole = value;
        }
        public XPCollection<Personnel> PersonnelList => new(_session1);

        private void TableView_ValidateRowDeletion(object sender, DevExpress.Xpf.Grid.GridValidateRowDeletionEventArgs e)
        {
            _session1.Delete(e.Rows);
        }

        public void Dispose()
        {
            _session1?.Dispose();
        }
    }
}
