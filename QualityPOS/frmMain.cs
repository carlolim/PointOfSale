using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QualityPOS
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ProcessAccessLevel();
            frmSales frm = new frmSales();
            frm.MdiParent = this;
            frm.Show();
            
        }

        private void tileSales_ItemClick(object sender, TileItemEventArgs e)
        {
            frmSales frm = new frmSales();
            frm.MdiParent = this;
            frm.Show();
        }

        private void tileStore_ItemClick(object sender, TileItemEventArgs e)
        {
            frmStore frm = new frmStore();
            frm.MdiParent = this;
            frm.Show();
        }

        private void tileStockRoom_ItemClick(object sender, TileItemEventArgs e)
        {
            frmProducts frm = new frmProducts();
            frm.MdiParent = this;
            frm.Show();
        }

        private void tileUsers_ItemClick(object sender, TileItemEventArgs e)
        {
            frmUsers frm = new frmUsers();
            frm.MdiParent = this;
            frm.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        void ProcessAccessLevel()
        {
            if (Global.User.UserRoleID == (int)UserRoleEnum.Employee)
            {
                tileStore.Enabled = false;
                tileStockRoom.Enabled = false;
                tileUsers.Enabled = false;
            }
        }

        private void tileMyAccount_ItemClick(object sender, TileItemEventArgs e)
        {
            frmMyAccount frm = new frmMyAccount();
            frm.MdiParent = this;
            frm.Show();
        }

        private void tileLogout_ItemClick(object sender, TileItemEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}