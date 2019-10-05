using QualityPOS.Manager;
using QualityPOS.Objects.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QualityPOS
{
    public partial class frmUsers : Form
    {
        UserManager _userManager = new UserManager();
        public int SelectedUserID { get; set; }

        public frmUsers()
        {
            InitializeComponent();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            PopulateListView();
            SelectedUserID = 0;
        }

        private async void PopulateListView(string search = "")
        {
            var users = new List<UserDTO>();

            if (Global.User.UserRoleID == (int)UserRoleEnum.Manager)
            {
                users = await _userManager.GetAllByRole(UserRoleEnum.Employee);
            }
            else
            {
                users = await _userManager.GetAll();
            }

            if (!string.IsNullOrWhiteSpace(search))
                users.RemoveAll(m => !($@"{ m.FirstName } { m.LastName }").ToLower().Contains(search.ToLower()));

            listView1.Items.Clear();

            foreach(var user in users)
            {
                ListViewItem lvi = new ListViewItem(user.UserID.ToString());

                string lastLogin = string.Empty;
                lastLogin = user.LastLogin == null ? string.Empty : Convert.ToDateTime(user.LastLogin).ToString("MMM dd, yyyy hh:mmtt");

                lvi.SubItems.Add(user.FirstName);
                lvi.SubItems.Add(user.LastName);
                lvi.SubItems.Add(user.Username);
                lvi.SubItems.Add(lastLogin);
                lvi.SubItems.Add(user.UserRole.RoleName);

                listView1.Items.Add(lvi);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PopulateListView(textBox1.Text);
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedUserID = 0;
            if (listView1.SelectedItems.Count > 0)
                SelectedUserID = Convert.ToInt32(listView1.SelectedItems[0].Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(SelectedUserID == 0)
            {
                MessageBox.Show("Select a user to edit");
                return;
            }

            using(frmUsersEdit frm = new frmUsersEdit())
            {
                frm.UserID = SelectedUserID;
                frm.ShowDialog();
                PopulateListView();
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (SelectedUserID == 0)
            {
                MessageBox.Show("Select a user to delete");
                return;
            }

            if(MessageBox.Show("Are you sure you want to delete this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(SelectedUserID == 1)
                {
                    MessageBox.Show("You cannot delete main admin user");
                    return;
                }

                var result = await _userManager.Delete(SelectedUserID);
                MessageBox.Show(result.Message);
                PopulateListView();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (frmUsersNew frm = new frmUsersNew())
            {
                frm.ShowDialog();
                PopulateListView();
            }
        }
    }
}
