using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QualityPOS.Manager;

namespace QualityPOS
{
    public partial class frmStoreCurrentUser : Form
    {
        private StoreManager _storeManager = new StoreManager();
        private SaleManager _saleManager = new SaleManager();

        public frmStoreCurrentUser()
        {
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to close the store?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await _storeManager.CloseStore(Global.Store);
                Global.Store = new Objects.Store();
                Close();
            }
        }

        private void frmStoreCurrentUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private async void frmStoreCurrentUser_Load(object sender, EventArgs e)
        {
            lblName.Text = $@"{ Global.User.FirstName } { Global.User.LastName }";
            lblStart.Text = $@"{ Convert.ToDateTime(Global.Store.DateOpen).ToString("MMM dd, yyyy hh:mmtt") }";
            var totalSales = await _saleManager.GetTotalSaleByStoreID(Global.Store.StoreID);
            lblTotalSales.Text = totalSales.ToString("C2").Replace("$", "");
        }
    }
}
