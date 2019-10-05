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
    public partial class frmSalesPayment : Form
    {
        public decimal Total { get; set; }
        public bool GoodToGo { get; set; } = true;
        public bool TransactionDone { get; set; } = false;
        public List<SaleProduct> SaleProducts { get; set; }
        public bool UpdateTransaction { get; set; } = false;
        public int SaleID { get; set; } = 0;
        public decimal AmountPaid { get; set; }

        private SaleManager _saleManager;
        private StoreManager _storeManager;

        public frmSalesPayment()
        {
            InitializeComponent();
            _saleManager = new SaleManager();
            _storeManager = new StoreManager();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmSalesPayment_Load(object sender, EventArgs e)
        {
            lblTotal.Text = Total.ToString("C2").Replace("$", "");
            if (UpdateTransaction)
            {
                textBox1.Text = AmountPaid.ToString("C2").Replace("$", "");
            }
            else
            {
                textBox1.Text = Total.ToString();
            }

            Compute();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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


        private void frmSalesPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Compute();
        }

        private void Compute()
        {
            lblChange.Visible = false;
            GoodToGo = false;

            if (string.IsNullOrWhiteSpace(textBox1.Text))
                textBox1.Text = "0";

            decimal cash = Convert.ToDecimal(textBox1.Text);
            if (cash > Total)
            {
                lblChange.ForeColor = Color.Green;
                lblChange.Text = $@"Change { (cash - Total).ToString("C2").Replace("$", "") }";
                GoodToGo = true;
                lblChange.Visible = true;
            }
            else if (Total > cash)
            {
                lblChange.ForeColor = Color.Red;
                lblChange.Text = $@"Amount Due { Total.ToString("C2").Replace("$", "") }";
                GoodToGo = false;
                lblChange.Visible = true;
            }
            else if (cash == Total)
            {
                GoodToGo = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (GoodToGo)
            {
                if (!UpdateTransaction)
                {
                    AddSale();
                }
                else
                {
                    UpdateSale();
                }
            }
        }

        private async void UpdateSale()
        {
            var sale = _saleManager.GetByID(SaleID);
            var updateSaleResult = await _saleManager.UpdateSale(new Sale()
            {
                AmountPaid = Convert.ToDouble(textBox1.Text),
                Change = Convert.ToDouble(Convert.ToDecimal(textBox1.Text) - Total),
                DateCreated = sale.DateCreated,
                DateTime = sale.DateTime,
                IsDeleted = false,
                StoreID = sale.StoreID,
                Total = Convert.ToDecimal(Total),
                UserCreatedID = sale.UserCreatedID,
                UserID = sale.UserID,
                DateModified = DateTime.Now,
                SaleID = sale.SaleID,
                UserModifiedID = Global.User.UserID
            });

            if (updateSaleResult.IsSuccess)
            {
                SaleProducts = SaleProducts.Select(m =>
                {
                    m.SaleID = sale.SaleID;
                    return m;
                }).ToList();

                //update saleproducts
                //bawasan quantity ng dapat bawasan
                //dagdagan ang dapat dagdagan
                updateSaleResult = await _saleManager.UpdateSaleProduct(SaleProducts, SaleID);
            }

            if (updateSaleResult.IsSuccess)
            {
                //bawasan quantity sa store
                await _storeManager.DecreaseProductLeft(Global.Store.StoreID, SaleProducts);
                Close();
                TransactionDone = true;
            }
            else
            {
                //delete sale by id
                //delete saleproduct by saleid
                await _saleManager.DeleteSale(SaleID);
                await _saleManager.DeleteSaleProductBySaleID(SaleID);

                MessageBox.Show("An error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddSale()
        {
            int insertedSaleID = 0;
            var insertSaleResult = await _saleManager.AddSale(new Sale()
            {
                AmountPaid = Convert.ToDouble(textBox1.Text),
                Change = Convert.ToDouble(Convert.ToDecimal(textBox1.Text) - Total),
                DateCreated = DateTime.Now,
                DateTime = DateTime.Now,
                IsDeleted = false,
                StoreID = Global.Store.StoreID,
                Total = Convert.ToDecimal(Total),
                UserCreatedID = Global.User.UserID,
                UserID = Global.User.UserID
            });

            insertedSaleID = insertSaleResult.ID;

            if (insertSaleResult.IsSuccess)
            {
                SaleProducts = SaleProducts.Select(m =>
                {
                    m.SaleID = insertSaleResult.ID;
                    return m;
                }).ToList();

                foreach (var saleProduct in SaleProducts)
                {
                    insertSaleResult = await _saleManager.AddSaleProduct(saleProduct);
                }
            }

            if (insertSaleResult.IsSuccess)
            {
                //bawasan quantity sa store
                await _storeManager.DecreaseProductLeft(Global.Store.StoreID, SaleProducts);
                Close();
                TransactionDone = true;
            }
            else
            {
                //delete sale by id
                //delete saleproduct by saleid
                await _saleManager.DeleteSale(insertedSaleID);
                await _saleManager.DeleteSaleProductBySaleID(insertedSaleID);

                MessageBox.Show("An error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
