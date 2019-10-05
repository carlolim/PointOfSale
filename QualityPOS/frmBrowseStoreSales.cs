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
using QualityPOS.Objects.DTO;

namespace QualityPOS
{
    public partial class frmBrowseStoreSales : Form
    {
        public int StoreId { get; set; }

        private readonly SaleManager _saleManager = new SaleManager();
        private readonly StoreManager _storeManager = new StoreManager();
        private readonly UserManager _userManager = new UserManager();
        private List<SaleDTO> _transactions = new List<SaleDTO>();

        public frmBrowseStoreSales()
        {
            InitializeComponent();
        }

        private void frmBrowseStoreSales_Load(object sender, EventArgs e)
        {
            LoadTransactions();
        }


        private async void LoadTransactions()
        {
            var store = await _storeManager.GetById(StoreId);
            if (store != null)
            {
                var user = _userManager.GetByID(store.UserID);
                lblStoreUser.Text = $@"Store user: { user.FirstName } {user.LastName }";
                lblStoreDate.Text = $@"{Convert.ToDateTime(store.DateOpen).ToString("MMM. dd, yyyy hh:mm tt")}";

                _transactions = await _saleManager.GetTransactionsByStoreID(StoreId);
                gridControl1.DataSource = null;
                foreach (GridColumn column in gridView1.Columns)
                {
                    GridSummaryItem item = column.SummaryItem;
                    if (item != null)
                        column.Summary.Remove(item);
                }
                if (_transactions.Any())
                {
                    var source = _transactions.Select(m => new GridItem()
                    {
                        Items = m.Items,
                        GrossSales = m.Total,
                        NetSales = m.Net,
                        AmountPaid = m.AmountPaid,
                        Change = m.Change,
                        Date = m.DateTime,
                        Id = m.SaleID,
                        Time = m.DateTime
                    });

                    gridControl1.DataSource = source;
                    GridColumnSummaryItem item1 = new GridColumnSummaryItem(SummaryItemType.Sum, "GrossSales", "{0:N}");
                    GridColumnSummaryItem item2 = new GridColumnSummaryItem(SummaryItemType.Sum, "NetSales", "{0:N}");
                    gridView1.Columns["GrossSales"].Summary.Add(item1);
                    gridView1.Columns["NetSales"].Summary.Add(item2);

                }
                //foreach (SaleDTO transaction in _transactions)
                //{
                //    ListViewItem lvi = new ListViewItem(transaction.SaleID.ToString());
                //    lvi.SubItems.Add(transaction.DateStr);
                //    lvi.SubItems.Add(transaction.TimeStr);
                //    lvi.SubItems.Add(transaction.Items);
                //    lvi.SubItems.Add(transaction.Total.ToString("C2").Replace("$", ""));
                //    lvi.SubItems.Add(transaction.Net.ToString("C2").Replace("$", ""));
                //    lvi.SubItems.Add(transaction.AmountPaid.ToString("C2").Replace("$", ""));
                //    lvi.SubItems.Add(transaction.Change.ToString("C2").Replace("$", ""));
                //    lvi.SubItems.Add($@"{ transaction.Firstname } { transaction.Lastname }");

                //    lvwSales.Items.Add(lvi);
                //    _net += transaction.Net;
                //    _gross += transaction.Total;
                //}

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

    public class GridItem
    {
        [Display(Order = -1)]
        public int Id { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "MMM dd, yyyy")]
        public DateTime Date { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "hh:mm tt")]
        public DateTime Time { get; set; }
        [Editable(false)]
        public string Items { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal GrossSales { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal NetSales { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double AmountPaid { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double Change { get; set; }
    }
}
