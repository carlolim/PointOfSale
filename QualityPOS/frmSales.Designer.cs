namespace QualityPOS
{
    partial class frmSales
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSales));
            this.panelMain = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSalesReport = new System.Windows.Forms.Button();
            this.lblCurrentStoreUser = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnNewSale = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.gridControl1);
            this.panelMain.Controls.Add(this.btnSalesReport);
            this.panelMain.Controls.Add(this.lblCurrentStoreUser);
            this.panelMain.Controls.Add(this.lblTotal);
            this.panelMain.Controls.Add(this.button9);
            this.panelMain.Controls.Add(this.btnExchange);
            this.panelMain.Controls.Add(this.btnNewSale);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1097, 594);
            this.panelMain.TabIndex = 6;
            this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(20, 73);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1064, 448);
            this.gridControl1.TabIndex = 11;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // btnSalesReport
            // 
            this.btnSalesReport.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnSalesReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSalesReport.FlatAppearance.BorderSize = 0;
            this.btnSalesReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesReport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesReport.ForeColor = System.Drawing.Color.White;
            this.btnSalesReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesReport.Location = new System.Drawing.Point(158, 13);
            this.btnSalesReport.Name = "btnSalesReport";
            this.btnSalesReport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSalesReport.Size = new System.Drawing.Size(168, 47);
            this.btnSalesReport.TabIndex = 10;
            this.btnSalesReport.Text = "Sales Report";
            this.btnSalesReport.UseVisualStyleBackColor = false;
            this.btnSalesReport.Click += new System.EventHandler(this.button10_Click);
            // 
            // lblCurrentStoreUser
            // 
            this.lblCurrentStoreUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentStoreUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCurrentStoreUser.Font = new System.Drawing.Font("Tahoma", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentStoreUser.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrentStoreUser.Location = new System.Drawing.Point(15, 524);
            this.lblCurrentStoreUser.Name = "lblCurrentStoreUser";
            this.lblCurrentStoreUser.Size = new System.Drawing.Size(513, 60);
            this.lblCurrentStoreUser.TabIndex = 9;
            this.lblCurrentStoreUser.Text = "Current store user: Carlo Lim";
            this.lblCurrentStoreUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrentStoreUser.Click += new System.EventHandler(this.lblCurrentStoreUser_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.Green;
            this.lblTotal.Location = new System.Drawing.Point(475, 524);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(609, 60);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "Total Sales: 1,342.75";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.BackColor = System.Drawing.SystemColors.Highlight;
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Tahoma", 11F);
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.Image = global::QualityPOS.Properties.Resources.Print_48;
            this.button9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.Location = new System.Drawing.Point(980, 13);
            this.button9.Name = "button9";
            this.button9.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.button9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button9.Size = new System.Drawing.Size(104, 47);
            this.button9.TabIndex = 4;
            this.button9.Text = "Print";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // btnExchange
            // 
            this.btnExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExchange.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExchange.FlatAppearance.BorderSize = 0;
            this.btnExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExchange.Font = new System.Drawing.Font("Tahoma", 11F);
            this.btnExchange.ForeColor = System.Drawing.Color.White;
            this.btnExchange.Image = ((System.Drawing.Image)(resources.GetObject("btnExchange.Image")));
            this.btnExchange.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExchange.Location = new System.Drawing.Point(840, 13);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnExchange.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnExchange.Size = new System.Drawing.Size(134, 47);
            this.btnExchange.TabIndex = 2;
            this.btnExchange.Text = "Exchange";
            this.btnExchange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExchange.UseVisualStyleBackColor = false;
            this.btnExchange.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnNewSale
            // 
            this.btnNewSale.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnNewSale.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNewSale.FlatAppearance.BorderSize = 0;
            this.btnNewSale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewSale.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewSale.ForeColor = System.Drawing.Color.White;
            this.btnNewSale.Image = ((System.Drawing.Image)(resources.GetObject("btnNewSale.Image")));
            this.btnNewSale.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewSale.Location = new System.Drawing.Point(19, 13);
            this.btnNewSale.Name = "btnNewSale";
            this.btnNewSale.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNewSale.Size = new System.Drawing.Size(133, 47);
            this.btnNewSale.TabIndex = 0;
            this.btnNewSale.Text = "New Sale";
            this.btnNewSale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewSale.UseVisualStyleBackColor = false;
            this.btnNewSale.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 594);
            this.Controls.Add(this.panelMain);
            this.Name = "frmSales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales";
            this.Load += new System.EventHandler(this.frmSales_Load);
            this.VisibleChanged += new System.EventHandler(this.frmSales_VisibleChanged);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnNewSale;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblCurrentStoreUser;
        private System.Windows.Forms.Button btnSalesReport;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}