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
    public partial class frmProductsNew : Form
    {
        private readonly ProductManager _productManager = new ProductManager();
        private readonly BrandManager _brandManager = new BrandManager();
        private readonly CategoryManager _categoryManager = new CategoryManager();
        private readonly ProductAuditManager _productAuditManager = new ProductAuditManager();

        public frmProductsNew()
        {
            InitializeComponent();
        }

        private void chkSalesPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesPrice.Checked)
            {
                txtMarkup.Text = string.Empty;
                txtSalesPrice.Text = string.Empty;
            }

            lblMarkup.Visible = chkSalesPrice.Checked;
            txtMarkup.Visible = chkSalesPrice.Checked;
            lblReqMarkup.Visible = chkSalesPrice.Checked;

            txtSalesPrice.Enabled = !chkSalesPrice.Checked;
        }

        private void chkCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCode.Checked)
                txtCode.Text = string.Empty;

            txtCode.Enabled = !chkCode.Checked;
        }

        private void frmProductsNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            var brand = await _brandManager.GetByName(cboBrand.Text);
            var category = await _categoryManager.GetByName(cboCategory.Text);

            if (brand == null)
                brand = new Brand();
            if (category == null)
                category = new Category();

            ProductDTO product = new ProductDTO()
            {
                AutomaticCode = chkCode.Checked,
                AvailableStockStr = txtAvailableStock.Text,
                Brand = Convert.ToString(brand.BrandID),
                Category = Convert.ToString(category.CategoryID),
                Code = txtCode.Text,
                CostStr = txtCost.Text,
                MarkUpStr = txtMarkup.Text,
                Name = txtName.Text,
                SalesPriceStr = txtSalesPrice.Text,
                AutomaticSalesPrice = chkSalesPrice.Checked
            };

            var result = await _productManager.Add(product);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                ProductAudit productAudit = new ProductAudit()
                {
                    Quantity =  Convert.ToDecimal(txtAvailableStock.Text),
                    UserCreatedID =  Global.User.UserID,
                    ProductID =  result.ID,
                    DateCreated = DateTime.Now
                };
                await _productAuditManager.Add(productAudit);
                MessageBox.Show($@"Product { txtName.Text } added.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void ComputeSalesPrice()
        {
            if (chkSalesPrice.Checked)
            {
                if (!string.IsNullOrWhiteSpace(txtMarkup.Text) && !string.IsNullOrWhiteSpace(txtCost.Text))
                {
                    double outDouble = 0.0;
                    if (double.TryParse(txtMarkup.Text, out outDouble) && double.TryParse(txtCost.Text, out outDouble))
                    {
                        txtSalesPrice.Text = Convert.ToString(Convert.ToDouble(txtCost.Text) + (Convert.ToDouble(txtCost.Text) * Convert.ToDouble(txtMarkup.Text)));
                    }
                }
            }
        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {
            ComputeSalesPrice();
        }

        private void txtMarkup_TextChanged(object sender, EventArgs e)
        {
            ComputeSalesPrice();
        }

        private void frmProductsNew_Load(object sender, EventArgs e)
        {
            LoadBrandCategory();
        }

        private async void LoadBrandCategory()
        {
            var brands = await _brandManager.GetAll();
            var categories = await _categoryManager.GetAll();

            cboBrand.Items.Clear();
            cboCategory.Items.Clear();

            foreach(var brand in brands)
            {
                cboBrand.Items.Add(brand.BrandName);
            }

            foreach(var cat in categories)
            {
                cboCategory.Items.Add(cat.CategoryName);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using(frmNewBrandCategory frm = new frmNewBrandCategory())
            {
                frm.IsBrand = false;
                frm.ShowDialog();
                LoadBrandCategory();
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmNewBrandCategory frm = new frmNewBrandCategory())
            {
                frm.IsBrand = true;
                frm.ShowDialog();
                LoadBrandCategory();
            }
        }
    }
}
