using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QualityPOS.Objects;
using QualityPOS.Manager;

namespace QualityPOS
{
    public partial class frmLogin : Form
    {
        UserManager _userManager = new UserManager();
        StoreManager _storeManager = new StoreManager();

        public frmLogin()
        {
            InitializeComponent();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var user = new User() { Username = txtUsername.Text, Password = txtPassword.Text };
            user = await _userManager.Login(user);

            if (user == null)
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Global.User = user;
                Global.Store = await _storeManager.GetLastOpenStore();
                if (Global.Store == null)
                    Global.Store = new Store();
                //show main form
                frmMain frm = new frmMain();
                frm.Show();
                //Global.frmProducts.Hide();
                //Global.frmStore.Hide();
                //Global.frmSales.Show();
                Hide();
            }
        }
    }
}
