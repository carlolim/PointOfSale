using QualityPOS.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using QualityPOS.Manager;

namespace QualityPOS
{
    public partial class frmStore : Form
    {
        private StoreManager _storeManager = new StoreManager();
        private UserManager _userManager = new UserManager();
        private DateTime FilterDateFrom { get; set; }
        private DateTime FilterDateTo { get; set; }

        public frmStore()
        {
            InitializeComponent();
            FilterDateFrom = DateTime.Now;
            FilterDateTo = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Global.Store.StoreID == 0)
            {
                MessageBox.Show("Please open store first");
                return;
            }

            using(frmStoreAddProduct frm = new frmStoreAddProduct())
            {
                frm.ShowDialog();
                UpdateCurrentStoreUser();
                PopulateListview();
            }
        }

        private void frmStore_Load(object sender, EventArgs e)
        {
            UpdateCurrentStoreUser();
            PopulateListview();
        }

        private async void PopulateListview(string search = "")
        {

            var items = await _storeManager.GetStoreProductsByStoreID(Global.Store.StoreID);

            gridControl1.DataSource = null;
            foreach (GridColumn column in gridView1.Columns)
            {
                GridSummaryItem item = column.SummaryItem;
                if (item != null)
                    column.Summary.Remove(item);
            }
            if (items.Any())
            {
                var source = items.Select(m => new FrmStoreGridItem
                {
                    Quantity = m.Quantity,
                    Name = m.Name,
                    Id = m.StoreProductID,
                    Left = m.UnitsLeft,
                    Price = m.SalesPrice,
                    Sold = m.UnitsSold,
                    TotalPriceLeft = m.TotalPriceLeft,
                    TotalPriceSold = m.TotalPriceSold
                });

                gridControl1.DataSource = source;
                GridColumnSummaryItem item3 = new GridColumnSummaryItem(SummaryItemType.Sum, "TotalPriceSold", "{0:N}");
                GridColumnSummaryItem item5 = new GridColumnSummaryItem(SummaryItemType.Sum, "TotalPriceLeft", "{0:N}");
                gridView1.Columns["TotalPriceSold"].Summary.Add(item3);
                gridView1.Columns["TotalPriceLeft"].Summary.Add(item5);
            }
            //if (!string.IsNullOrWhiteSpace(search))
            //{
            //    items.RemoveAll(m => !m.Name.ToLower().Contains(search.ToLower()));
            //}

            //listView1.Items.Clear();
            //if (items.Any())
            //{
            //    foreach(var item in items)
            //    {
            //        var lvi = new ListViewItem(item.StoreProductID.ToString());

            //        lvi.SubItems.Add(item.Name);
            //        lvi.SubItems.Add(item.Code);
            //        lvi.SubItems.Add(item.SalesPrice.ToString("C2").Replace("$",""));
            //        lvi.SubItems.Add(item.Quantity.ToString());
            //        lvi.SubItems.Add(item.UnitsSold.ToString());
            //        lvi.SubItems.Add(item.TotalPriceSold.ToString("C2").Replace("$", ""));
            //        lvi.SubItems.Add(item.UnitsLeft.ToString());
            //        lvi.SubItems.Add(item.TotalPriceLeft.ToString("C2").Replace("$", ""));

            //        listView1.Items.Add(lvi);
            //    }
            //}
        }

        private async void UpdateCurrentStoreUser()
        {
            var lastOpenStore = await _storeManager.GetLastOpenStore();
            if(lastOpenStore != null)
            {
                var user = _userManager.GetByID(lastOpenStore.UserID);
                if (user != null)
                {
                    lblCurrentStoreUser.Text = $@"Current store user: { user.FirstName } { user.LastName }";
                    return;
                }
            }
            lblCurrentStoreUser.Text = $@"Open Store";
        }

        private async void lblCurrentStoreUser_Click(object sender, EventArgs e)
        {
            if(lblCurrentStoreUser.Text == "Open Store")
            {
                if (MessageBox.Show("Are you sure you want to open the store?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var store = await _storeManager.OpenStore(Global.User);
                    if (store != null)
                    {
                        Global.Store = store;
                        UpdateCurrentStoreUser();
                        PopulateListview();
                    }
                }
            }
            else
            {
                using (frmStoreCurrentUser frm = new frmStoreCurrentUser())
                {
                    frm.ShowDialog();
                    UpdateCurrentStoreUser();
                    PopulateListview();
                }
            }
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!gridControl1.IsPrintingAvailable)
            {
                MessageBox.Show("Printing not available", "Error");
                return;
            }

            gridView1.ShowPrintPreview();
        }
    }

    public class FrmStoreGridItem
    {
        [Display(Order = -1)]
        public int Id { get; set; }
        [Editable(false)]
        public string Name { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Price { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double Quantity { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double Sold { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal TotalPriceSold { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double Left { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal TotalPriceLeft { get; set; }
    }
}
