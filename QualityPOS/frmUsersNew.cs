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
using QualityPOS.Objects;
using QualityPOS.Objects.DTO;

namespace QualityPOS
{
    public partial class frmUsersNew : Form
    {
        UserManager _userManager = new UserManager();

        public frmUsersNew()
        {
            InitializeComponent();
        }

        private void frmUsersNew_Load(object sender, EventArgs e)
        {
            LoadUserRoles();
        }

        private async void LoadUserRoles()
        {
            cboUserRole.Items.Clear();

            if (Global.User.UserRoleID == (int)UserRoleEnum.Manager)
            {
                cboUserRole.Items.Add("Employee");
            }
            else
            {
                var roles = await _userManager.GetUserRoles();
                foreach (var role in roles)
                {
                    cboUserRole.Items.Add(role.RoleName);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmUsersNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            var userDTO = new UserDTO()
            {
                FirstName = txtFirstname.Text,
                LastName  = txtLastname.Text,
                Password = txtPassword.Text,
                ReTypePassword = txtRepassword.Text,
                Username = txtUsername.Text,
                UserRoleStr = cboUserRole.Text
            };

            var result = await _userManager.Add(userDTO);
            if (result.IsSuccess)
            {
                MessageBox.Show($"User { txtFirstname.Text } { txtLastname.Text} added!");
                Close();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }
    }
}
