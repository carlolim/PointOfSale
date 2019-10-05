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
using DevExpress.XtraGrid.Views.Grid;
using QualityPOS.Manager;
using QualityPOS.Objects.DTO;

namespace QualityPOS
{
    public partial class frmBrowseStore : Form
    {
        private readonly StoreManager _storeManager = new StoreManager();

        private DateTime? dateFrom { get; set; }
        private DateTime? dateTo { get; set; }
        private List<StoreSummaryDTO> stores = new List<StoreSummaryDTO>();
        private int selectedStoreId = 0;

        public frmBrowseStore()
        {
            InitializeComponent();
        }

        private void frmBrowseSales_Load(object sender, EventArgs e)
        {
            GetStoresByDateRange();
        }

        private async void GetStoresByDateRange()
        {
            stores = await _storeManager.GetStoresByDateRange(this.dateFrom, this.dateTo);
            selectedStoreId = 0;
            gridControl1.DataSource = null;
            foreach (GridColumn column in gridView1.Columns)
            {
                GridSummaryItem item = column.SummaryItem;
                if (item != null)
                    column.Summary.Remove(item);
            }
            if (stores.Any())
            {
                var source = stores.Select(m => new BrowseStoreGridItem
                {
                    StoreId = m.StoreId,
                    User = m.Firstname + " " + m.Lastname,
                    NetSales = m.NetSales,
                    GrossSales = m.TotalSales,
                    DateOpened = m.DateOpen
                });
                gridControl1.DataSource = source;
                GridColumnSummaryItem item1 = new GridColumnSummaryItem(SummaryItemType.Sum, "GrossSales", "{0:N}");
                gridView1.Columns["GrossSales"].Summary.Add(item1);
                GridColumnSummaryItem item2 = new GridColumnSummaryItem(SummaryItemType.Sum, "NetSales", "{0:N}");
                gridView1.Columns["NetSales"].Summary.Add(item2);
            }
            //foreach (var store in stores)
            //{
            //    ListViewItem lvi = new ListViewItem(store.StoreId.ToString());
            //    lvi.SubItems.Add(store.DateOpen.ToString("MMM. dd hh:mm tt"));
            //    lvi.SubItems.Add(store.Firstname + " " + store.Lastname);
            //    lvi.SubItems.Add($@"{ store.TotalSales.ToString("C2").Replace("$", "") }");
            //    lvi.SubItems.Add($@"{ store.NetSales.ToString("C2").Replace("$", "") }");
            //    lvwStores.Items.Add(lvi);
            //}

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            dateFrom = dtpFrom.DateTime;
            dateTo = dtpTo.DateTime;
            GetStoresByDateRange();
            selectedStoreId = 0;

        }
        private void gridControl1_Click(object sender, EventArgs e)
        {
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            GridView gridView = gridControl1.FocusedView as GridView;
            BrowseStoreGridItem row = (BrowseStoreGridItem)gridView.GetRow(gridView.FocusedRowHandle);
            selectedStoreId = row.StoreId;
            using (frmBrowseStoreSales frm = new frmBrowseStoreSales())
            {
                frm.StoreId = selectedStoreId;
                frm.ShowDialog();
            }
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

    public class BrowseStoreGridItem
    {
        [Display(Order = -1)]
        public int StoreId { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "MMM dd, yyyy hh:mm tt")]
        public DateTime DateOpened { get; set; }
        [Editable(false)]
        public string User { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal GrossSales { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal NetSales { get; set; }
    }
}
