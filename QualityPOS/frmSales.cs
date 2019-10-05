using QualityPOS.Manager;
using QualityPOS.Objects.DTO;
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

namespace QualityPOS
{
    public partial class frmSales : Form
    {
        private readonly SaleManager _saleManager = new SaleManager();
        private readonly StoreManager _storeManager = new StoreManager();
        private readonly UserManager _userManager = new UserManager();
        private List<SaleDTO> _transactions = new List<SaleDTO>();

        public int SaleId { get; set; }

        public frmSales()
        {
            InitializeComponent();
        }

        private void frmSales_Load(object sender, EventArgs e)
        {
            ProcessAccessLevel();
            LoadTransactions();
            UpdateCurrentStoreUser();
        }

        private async void LoadTransactions()
        {
            if (Global.Store.StoreID == 0)
                _transactions = await _saleManager.GetTransactions();
            else
                _transactions = await _saleManager.GetTransactionsByStoreID(Global.Store.StoreID);

            //listView1.Items.Clear();
            lblTotal.Text = string.Empty;
            decimal subtotal = 0;
            gridControl1.DataSource = null;
            foreach (GridColumn column in gridView1.Columns)
            {
                GridSummaryItem item = column.SummaryItem;
                if (item != null)
                    column.Summary.Remove(item);
            }
            if (_transactions.Any())
            {
                var source = _transactions.Select(m => new SalesGridItem()
                {
                    Items = m.Items,
                    Total = m.Total,
                    User = m.Firstname + " " + m.Lastname,
                    AmountPaid = m.AmountPaid,
                    Change = m.Change,
                    Date = m.DateTime,
                    Id = m.SaleID,
                    Time = m.DateTime
                });

                gridControl1.DataSource = source;
                GridColumnSummaryItem item1 = new GridColumnSummaryItem(SummaryItemType.Sum, "Total", "{0:N}");
                gridView1.Columns["Total"].Summary.Add(item1);
            }
            //foreach (SaleDTO transaction in _transactions)
            //{
            //    ListViewItem lvi = new ListViewItem(transaction.SaleID.ToString());

            //    lvi.SubItems.Add(transaction.DateStr);
            //    lvi.SubItems.Add(transaction.TimeStr);
            //    lvi.SubItems.Add(transaction.Items);
            //    lvi.SubItems.Add(transaction.Total.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(transaction.AmountPaid.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(transaction.Change.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add($@"{ transaction.Firstname } { transaction.Lastname }");

            //    listView1.Items.Add(lvi);

            //    subtotal += transaction.Total;
            //}
            subtotal = _transactions.Sum(m => m.Total);
            lblTotal.Text = $@"Total Sales: { subtotal.ToString("C2").Replace("$", "") }";

        }

        private void frmSales_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Global.Store.StoreID == 0)
            {
                MessageBox.Show("Please open store first");
                return;
            }

            using (frmSalesNewSale frm = new frmSalesNewSale())
            {
                frm.ShowDialog();
                LoadTransactions();
            }
        }

        private async void lblCurrentStoreUser_Click(object sender, EventArgs e)
        {
            if (lblCurrentStoreUser.Text == "Open Store")
            {
                if (MessageBox.Show("Are you sure you want to open the store?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var store = await _storeManager.OpenStore(Global.User);
                    if (store != null)
                    {
                        Global.Store = store;
                        UpdateCurrentStoreUser();
                        LoadTransactions();
                    }
                }
            }
            else
            {
                using (frmStoreCurrentUser frm = new frmStoreCurrentUser())
                {
                    frm.ShowDialog();
                    UpdateCurrentStoreUser();
                    LoadTransactions();
                }
            }
        }

        private async void UpdateCurrentStoreUser()
        {
            var lastOpenStore = await _storeManager.GetLastOpenStore();
            if (lastOpenStore != null)
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

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SaleId == 0)
            {
                MessageBox.Show("Please select a transaction to edit");
                return;
            }
            using (frmSalesEditSale frm = new frmSalesEditSale())
            {
                frm.SaleID = SaleId;
                frm.Transaction = _transactions.FirstOrDefault(m => m.SaleID == SaleId);
                frm.ShowDialog();
                LoadTransactions();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (SaleId == 0)
            {
                MessageBox.Show("Please select a transaction to edit");
                return;
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

        private void button10_Click(object sender, EventArgs e)
        {
            using (frmBrowseStore frm = new frmBrowseStore())
            {
                frm.ShowDialog();
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            GridView gridView = gridControl1.FocusedView as GridView;
            SalesGridItem row = (SalesGridItem) gridView.GetRow(gridView.FocusedRowHandle);
            SaleId = row.Id;
        }

        void ProcessAccessLevel()
        {
            if (Global.User.UserRoleID == (int)UserRoleEnum.Employee)
            {
                btnSalesReport.Enabled = false;
            }
            else if(Global.User.UserRoleID == (int)UserRoleEnum.Manager)
            {
                btnNewSale.Enabled = false;
                btnExchange.Enabled = false;
            }
        }
    }

    public class SalesGridItem
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
        public decimal Total { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double AmountPaid { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public double Change { get; set; }
        [Editable(false)]
        public string User { get; set; }
    }
}
