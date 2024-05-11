using System;
using System.Linq;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpo;
using HCMData;
using Samco_HCM.Classes;
using NavigationEventArgs = DevExpress.Xpf.WindowsUI.Navigation.NavigationEventArgs;

namespace Samco_HCM.Views
{
    /// <summary>
    /// Interaction logic for Emergency.xaml
    /// </summary>
    public partial class Emergency
    {
        public Emergency()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainTilLayout.Children.Clear();
            using var session1 = new Session();
            var emergencyTileList = session1.Query<HealthServices>().Where(x => x.group == "emerg").ToList();
            if (emergencyTileList.Count == 0)
                return;

            foreach (var itm in emergencyTileList.Where(itm => itm.parent is null))
            {
                //Create items for visit
                var newVisitTile = SamcoAdd.GetTile(itm);
                if (itm.HealthServicesCollection.Count > 0)
                {
                    newVisitTile.Click += MaxiTiles_Click;
                }
                if (MainTilLayout.Children.Count == 0) TileLayoutControl.SetGroupHeader(newVisitTile, "انتخاب خدمت");
                MainTilLayout.Children.Add(newVisitTile);
            }
            base.OnNavigatedTo(e);
        }
        private void MaxiTiles_Click(object sender, EventArgs e)
        {
            var selTile = (Tile)sender;
            selTile.IsMaximized = !selTile.IsMaximized;
        }

    }
}
