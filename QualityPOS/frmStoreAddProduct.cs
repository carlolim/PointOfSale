using QualityPOS.Objects;
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
using QualityPOS.Manager;
using QualityPOS.Extensions;

namespace QualityPOS
{
    public partial class frmStoreAddProduct : Form
    {
        private ProductDTO _selectedProductDTO;
        private List<ProductDTO> _suggestions;
        private ProductManager _productManager;
        private StoreManager _storeManager;

        public frmStoreAddProduct()
        {
            InitializeComponent();

            _productManager = new ProductManager();
            _storeManager = new StoreManager();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                _selectedProductDTO = null;
                SetLabelText(string.Empty);
            }
        }

        private async void frmStoreAddProduct_Load(object sender, EventArgs e)
        {
            SetLabelText(string.Empty);
            _suggestions = await _productManager.GetListDTOAsync();
            if (_suggestions.Count() > 0)
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                collection.AddRange(_suggestions.Select(m => m.Name).ToArray());
                txtName.AutoCompleteCustomSource = collection;
            }
        }

        private void SetSelectedItem(ProductDTO selected)
        {
            _selectedProductDTO = selected;
            lblAvailableStock.Text = selected.AvailableStockStr;
            lblBrand.Text = selected.Brand;
            lblCategory.Text = selected.Category;
            lblCode.Text = selected.Code;
            lblCost.Text = selected.CostStr;
            //lblMarkup.Text = selected.MarkUpStr;
            lblName.Text = selected.Name;
            lblSalesPrice.Text = selected.SalesPriceStr;

            txtName.Text = selected.Name;
        }

        private void SetLabelText(string txt)
        {
            lblAvailableStock.Text = txt;
            lblBrand.Text = txt;
            lblCategory.Text = txt;
            lblCode.Text = txt;
            lblCost.Text = txt;
            //lblMarkup.Text = txt;
            lblName.Text = txt;
            lblSalesPrice.Text = txt;
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtName.Text))
                {
                    var match = _suggestions.FirstOrDefault(m => m.Name.ToLower().Contains(txtName.Text.ToLower()));
                    if (match != null)
                    {
                        SetSelectedItem(match);
                        txtQuantity.Focus();
                    }
                    else
                    {
                        SetLabelText(string.Empty);
                        _selectedProductDTO = null;
                    }
                }
                else
                {
                    _selectedProductDTO = null;
                    SetLabelText(string.Empty);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void frmStoreAddProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            AcceptButton = null;
        }

        private void txtQuantity_Enter(object sender, EventArgs e)
        {
            AcceptButton = btnSave;
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if(_selectedProductDTO == null)
            {
                MessageBox.Show("Select a product");
                txtName.Focus();
            }
            else if(string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Enter quantity");
                txtQuantity.Focus();
            }
            else if(Convert.ToInt32(txtQuantity.Text) > _selectedProductDTO.AvailableStock)
            {
                MessageBox.Show("Quantity must be within available stock");
            }
            else
            {
                if(MessageBox.Show($@"Are you sure you want to add { txtQuantity.Text } { txtName.Text } to store?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    StoreProduct storeProduct = new StoreProduct()
                    {
                        DateCreated = DateTime.Now,
                        DateDeleted = null,
                        DateModified = null,
                        IsDeleted = false,
                        ProductID = _selectedProductDTO.ProductID,
                        Quantity = Convert.ToDecimal(txtQuantity.Text),
                        StoreID = Global.Store.StoreID,
                        UnitsLeft = Convert.ToDecimal(txtQuantity.Text),
                        UnitsSold = 0,
                        UserCreatedID = Global.User.UserID,
                        UserDeletedID = null,
                        UserModifiedID = null
                    };

                    var insertResult = await _storeManager.AddProduct(storeProduct);
                    if(insertResult.IsSuccess)
                    {
                        await _productManager.UpdateQuantity(_selectedProductDTO.ProductID, _selectedProductDTO.AvailableStock - Convert.ToDecimal(txtQuantity.Text), Global.User.UserID);
                        MessageBox.Show($@"{ txtQuantity.Text } { txtName.Text } added to the store", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("An error occured while adding product to store");
                    }
                }
            }
        }
    }
}
