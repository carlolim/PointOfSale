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

namespace QualityPOS
{
    public partial class frmNewBrandCategory : Form
    {
        public bool IsBrand { get; set; }
        BrandManager _brandManager = new BrandManager();
        CategoryManager _categoryManager = new CategoryManager();

        public frmNewBrandCategory()
        {
            InitializeComponent();
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.Focus();
                MessageBox.Show("Name is required");
                return;
            }

            Result result = new Result();

            if (IsBrand)
            {
                Brand brand = new Brand()
                {
                    BrandName = txtName.Text,
                    IsDeleted = false,
                    UserCreatedID = Global.User.UserID,
                    DateCreated = DateTime.Now
                };

                result = await _brandManager.Add(brand);
            }
            else
            {
                Category category = new Category()
                {
                    CategoryName = txtName.Text,
                    IsDeleted= false,
                    DateCreated = DateTime.Now,
                    UserCreatedID = Global.User.UserID
                };
                result = await _categoryManager.Add(category);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show("Added successfully!");
                Close();
            }
            else
            {
                MessageBox.Show("An error occured");
            }
        }

        private void frmNewBrandCategory_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
