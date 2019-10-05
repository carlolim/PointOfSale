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
    public partial class frmProductsEdit : Form
    {
        private readonly ProductManager _productManager = new ProductManager();
        private readonly BrandManager _brandManager = new BrandManager();
        private readonly CategoryManager _categoryManager = new CategoryManager();
        private readonly ProductAuditManager _productAuditManager = new ProductAuditManager();
        private ProductDTO selectedProduct { get; set; }
        public int ProductID { get; set; }

        public frmProductsEdit()
        {
            InitializeComponent();
        }

        private void frmProductsEdit_Load(object sender, EventArgs e)
        {
            LoadBrandCategory();
            SetSelectedProduct();
        }

        private async void SetSelectedProduct()
        {
            selectedProduct = await _productManager.GetDTOByIDAsync(ProductID);
            if(selectedProduct != null)
            {
                txtAvailableStock.Text = selectedProduct.AvailableStockStr;
                txtCode.Text = selectedProduct.Code;
                txtCost.Text = selectedProduct.CostStr;
                txtMarkup.Text = selectedProduct.MarkUpStr;
                txtName.Text = selectedProduct.Name;
                txtSalesPrice.Text = selectedProduct.SalesPriceStr;
                cboBrand.Text = selectedProduct.Brand;
                cboCategory.Text = selectedProduct.Category;
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
                ProductID = ProductID,
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

            var result = await _productManager.Update(product);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //TODO: move added quantity sa product audit table. update to, dapat ata walang update quantity sa product?
                MessageBox.Show($@"Product { txtName.Text } updated.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private async void LoadBrandCategory()
        {
            var brands = await _brandManager.GetAll();
            var categories = await _categoryManager.GetAll();

            cboBrand.Items.Clear();
            cboCategory.Items.Clear();

            foreach (var brand in brands)
            {
                cboBrand.Items.Add(brand.BrandName);
            }

            foreach (var cat in categories)
            {
                cboCategory.Items.Add(cat.CategoryName);
            }
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmNewBrandCategory frm = new frmNewBrandCategory())
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
