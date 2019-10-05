using QualityPOS.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace QualityPOS
{
    public partial class frmProducts : Form
    {
        ProductManager _productManager = new ProductManager();
        public int StoreID { get; set; }
        int selectedProductID { get; set; }

        public frmProducts()
        {
            //TODO: create refill / decrease quantity ng product (new feature)
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (frmProductsNew frm = new frmProductsNew())
            {
                frm.ShowDialog(this);
                PopulateListView();
            }
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            StoreID = Global.Store.StoreID;
            PopulateListView();
        }

        private async void PopulateListView(string search = "")
        {
            var items = await _productManager.GetProductsByStoreID(StoreID);

            gridControl1.DataSource = null;
            foreach (GridColumn column in gridView1.Columns)
            {
                GridSummaryItem item = column.SummaryItem;
                if (item != null)
                    column.Summary.Remove(item);
            }
            if (items.Any())
            {
                var source = items.Select(m => new FrmProductGridItem
                {
                    Category = m.Category,
                    Id = m.ProductID,
                    Name = m.Name,
                    Cost = m.Cost,
                    AvailableStock = m.AvailableStock,
                    Price = m.SalesPrice,
                    Brand = m.Brand,
                    InStore = m.InStore,
                    TotalQuantity = m.TotalQuantity
                });

                gridControl1.DataSource = source;
                //GridColumnSummaryItem item3 = new GridColumnSummaryItem(SummaryItemType.Sum, "Price", "{0:N}");
                //GridColumnSummaryItem item5 = new GridColumnSummaryItem(SummaryItemType.Sum, "Cost", "{0:N}");
                //gridView1.Columns["Price"].Summary.Add(item3);
                //gridView1.Columns["Cost"].Summary.Add(item5);
            }
            //if (!string.IsNullOrWhiteSpace(search))
            //    items.RemoveAll(m => !m.Name.ToLower().Contains(search.ToLower()));

            //listView1.Items.Clear();
            //foreach (var item in items)
            //{
            //    ListViewItem lvi = new ListViewItem(item.ProductID.ToString());
            //    lvi.SubItems.Add(item.Name);
            //    lvi.SubItems.Add(item.Code);
            //    lvi.SubItems.Add(item.AvailableStock.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(item.InStore.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(item.TotalQuantity.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(item.SalesPrice.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(item.Cost.ToString("C2").Replace("$", ""));
            //    lvi.SubItems.Add(item.Category);
            //    lvi.SubItems.Add(item.Brand);

            //    listView1.Items.Add(lvi);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(selectedProductID == 0)
            {
                MessageBox.Show("Please select a product to edit");
                return;
            }

            using(frmProductsEdit frm = new frmProductsEdit())
            {
                frm.ProductID = selectedProductID;
                frm.ShowDialog();
                PopulateListView();
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (selectedProductID == 0)
            {
                MessageBox.Show("Please select a product to delete");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                await _productManager.Delete(selectedProductID);
                PopulateListView();
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

            GridView gridView = gridControl1.FocusedView as GridView;
            FrmProductGridItem row = (FrmProductGridItem)gridView.GetRow(gridView.FocusedRowHandle);
            selectedProductID = row.Id;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!gridControl1.IsPrintingAvailable)
            {
                MessageBox.Show("Printing not available", "Error");
                return;
            }

            gridView1.ShowPrintPreview();
        }
    }

    public class FrmProductGridItem
    {
        [Display(Order = -1)]
        public int Id { get; set; }
        [Editable(false)]
        public string Name { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal AvailableStock { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal InStore { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal TotalQuantity { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Price { get; set; }
        [Editable(false), DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Cost { get; set; }
        [Editable(false)]
        public string Category { get; set; }
        [Editable(false)]
        public string Brand { get; set; }
    }
}
