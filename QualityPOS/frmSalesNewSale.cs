using QualityPOS.Manager;
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

namespace QualityPOS
{
    public partial class frmSalesNewSale : Form
    {
        private SaleManager _saleManager;
        private ProductManager _productManager;
        private StoreManager _storeManager;

        private List<ProductDTO> _suggestions;
        private ProductDTO _selected = new ProductDTO()
        {
            Name = "Mt. Dew GREEN",
            AvailableStock = 3,
            SalesPrice = 13
        };

        public frmSalesNewSale()
        {
            InitializeComponent();
            _productManager = new ProductManager();
            _saleManager = new SaleManager();
            _storeManager = new StoreManager();

        }

        private async void frmSalesNewSale_Load(object sender, EventArgs e)
        {
            _suggestions = await _storeManager.GetStoreProducts(Global.Store.StoreID);
            if (_suggestions.Count() > 0)
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                collection.AddRange(_suggestions.Select(m => m.Name).ToArray());
                txtProduct.AutoCompleteCustomSource = collection;
            }
            ComputeTotal();
        }

        private async void frmSalesNewSale_VisibleChanged(object sender, EventArgs e)
        {
            //_suggestions = await _productManager.GetListDTOAsync();
            //if (_suggestions.Count() > 0)
            //{
            //    AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            //    collection.AddRange(_suggestions.Select(m => m.Name).ToArray());
            //    txtProduct.AutoCompleteCustomSource = collection;
            //}
            //ComputeTotal();
        }

        private void ComputeTotal()
        {
            decimal subtotal = 0;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                subtotal += Convert.ToDecimal(row.Cells["colTotal"].Value);
            }
            string subTotalStr = String.Format("{0:C2}", subtotal);
            lblTotal.Text = $@"Total = { subTotalStr.Replace("$", "") }";
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtProduct.Text))
                {
                    var match = _suggestions.FirstOrDefault(m => m.Name.ToLower().Contains(txtProduct.Text.ToLower()));
                    if (match != null)
                    {
                        _selected = match;
                        txtQuantity.Focus();
                    }
                    else
                    {
                        _selected = null;
                    }
                }
                else
                {
                    _selected = null;
                }
            }
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_selected != null)
                {
                    decimal quantity = Convert.ToDecimal(txtQuantity.Text);

                    if(quantity <= 0)
                    {
                        txtQuantity.Focus();
                        return;
                    }
                    if (quantity > _selected.UnitsLeft)
                    {
                        MessageBox.Show($@"There are only { _selected.UnitsLeft } { _selected.Name } in store", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtQuantity.Focus();
                        return;
                    }

                    var total = quantity * _selected.SalesPrice;
                    var i = dataGridView1.Rows.Add(_selected.ProductID, _selected.Name, txtQuantity.Text, _selected.SalesPrice, total, "Remove");
                    dataGridView1.Rows[i].Height = 50;

                    txtProduct.Focus();
                    txtProduct.Text = string.Empty;
                    txtQuantity.Text = "1";

                    dataGridView1.Rows[dataGridView1.RowCount - 1].Selected = true;
                    ComputeTotal();
                }
            }
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "colQuantity")
            {
                var qty = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["colQuantity"].Value);
                var price = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["colPrice"].Value);
                dataGridView1.Rows[e.RowIndex].Cells["colTotal"].Value = qty * price;
                ComputeTotal();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewLinkColumn &&
                e.RowIndex >= 0)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[e.RowIndex]);
                ComputeTotal();
            }
        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProduct.Text))
            {
                _selected = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount > 0)
            {
                using(frmSalesPayment frm = new frmSalesPayment())
                {
                    decimal subtotal = 0;
                    List<SaleProduct> saleProducts = new List<SaleProduct>();
                    int productId = 0;
                    ProductDTO product = new ProductDTO();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        productId = Convert.ToInt32(row.Cells[0].Value);
                        subtotal += Convert.ToDecimal(row.Cells["colTotal"].Value);
                        product = _suggestions.FirstOrDefault(m => m.ProductID == productId);

                        saleProducts.Add(new SaleProduct()
                        {
                            DateCreated = DateTime.Now,
                            IsDeleted = false,
                            ProductID = productId,
                            Quantity = Convert.ToInt32(row.Cells["colQuantity"].Value),
                            UserCreatedID = Global.User.UserID,
                            ProductPrice = product?.SalesPrice ?? 0,
                            ProductCost = product?.Cost ?? 0
                        });
                    }
                    frm.Total = subtotal;
                    frm.SaleProducts = saleProducts;
                    frm.ShowDialog();
                    if (frm.TransactionDone)
                        Close();
                }
            }
        }
    }
}
