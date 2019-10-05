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
    public partial class frmUsersEdit : Form
    {
        UserManager _userManager = new UserManager();
        public int UserID { get; set; }
        User _selectedUser { get; set; }

        public frmUsersEdit()
        {
            InitializeComponent();
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

        private async void GetUserData()
        {
            var user = _userManager.GetByID(UserID);
            _selectedUser = user;
            if(user != null)
            {
                var userRole = await _userManager.GetUserRoleByID(user.UserRoleID);

                txtFirstname.Text = user.FirstName;
                txtLastname.Text = user.LastName;
                txtUsername.Text = user.Username;
                cboUserRole.Text = userRole.RoleName;
            }

        }

        private void frmUsersEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private void frmUsersEdit_Load(object sender, EventArgs e)
        {
            LoadUserRoles();
            GetUserData();
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            var userDTO = new UserDTO()
            {
                UserID = UserID,
                FirstName = txtFirstname.Text,
                LastName = txtLastname.Text,
                Password = txtPassword.Text,
                ReTypePassword = txtRepassword.Text,
                Username = txtUsername.Text,
                UserRoleStr = cboUserRole.Text,
                DateModified = DateTime.Now,
                UserModifiedID = Global.User.UserID,
                DateCreated = _selectedUser.DateCreated,
                LastLogin = _selectedUser.LastLogin,
                UserCreatedID = _selectedUser.UserCreatedID
            };

            var result = await _userManager.Update(userDTO);
            if (result.IsSuccess)
            {
                MessageBox.Show($"User { txtFirstname.Text } { txtLastname.Text} updated!");
                Close();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }
    }
}
