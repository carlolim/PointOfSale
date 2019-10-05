namespace QualityPOS
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement6 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
            this.sidePanel1 = new DevExpress.XtraEditors.SidePanel();
            this.tileControl1 = new DevExpress.XtraEditors.TileControl();
            this.tileGroup2 = new DevExpress.XtraEditors.TileGroup();
            this.tileSales = new DevExpress.XtraEditors.TileItem();
            this.tileStore = new DevExpress.XtraEditors.TileItem();
            this.tileStockRoom = new DevExpress.XtraEditors.TileItem();
            this.tileUsers = new DevExpress.XtraEditors.TileItem();
            this.tileLogout = new DevExpress.XtraEditors.TileItem();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.tileMyAccount = new DevExpress.XtraEditors.TileItem();
            this.sidePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // sidePanel1
            // 
            this.sidePanel1.Controls.Add(this.tileControl1);
            this.sidePanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidePanel1.Location = new System.Drawing.Point(0, 0);
            this.sidePanel1.Name = "sidePanel1";
            this.sidePanel1.Size = new System.Drawing.Size(225, 670);
            this.sidePanel1.TabIndex = 0;
            this.sidePanel1.Text = "sidePanel1";
            // 
            // tileControl1
            // 
            this.tileControl1.AllowSelectedItem = true;
            this.tileControl1.AppearanceItem.Normal.BackColor = System.Drawing.SystemColors.Highlight;
            this.tileControl1.AppearanceItem.Normal.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileControl1.AppearanceItem.Normal.ForeColor = System.Drawing.Color.White;
            this.tileControl1.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileControl1.AppearanceItem.Normal.Options.UseFont = true;
            this.tileControl1.AppearanceItem.Normal.Options.UseForeColor = true;
            this.tileControl1.AppearanceItem.Normal.Options.UseTextOptions = true;
            this.tileControl1.AppearanceItem.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tileControl1.AppearanceItem.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.tileControl1.AppearanceText.BackColor = System.Drawing.SystemColors.Highlight;
            this.tileControl1.AppearanceText.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileControl1.AppearanceText.Options.UseBackColor = true;
            this.tileControl1.AppearanceText.Options.UseFont = true;
            this.tileControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileControl1.Groups.Add(this.tileGroup2);
            this.tileControl1.IndentBetweenGroups = 0;
            this.tileControl1.ItemSize = 100;
            this.tileControl1.Location = new System.Drawing.Point(0, 0);
            this.tileControl1.MaxId = 7;
            this.tileControl1.Name = "tileControl1";
            this.tileControl1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tileControl1.Padding = new System.Windows.Forms.Padding(0, 18, 0, 18);
            this.tileControl1.Position = 18;
            this.tileControl1.Size = new System.Drawing.Size(224, 670);
            this.tileControl1.TabIndex = 0;
            this.tileControl1.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
            // 
            // tileGroup2
            // 
            this.tileGroup2.Items.Add(this.tileSales);
            this.tileGroup2.Items.Add(this.tileStore);
            this.tileGroup2.Items.Add(this.tileStockRoom);
            this.tileGroup2.Items.Add(this.tileUsers);
            this.tileGroup2.Items.Add(this.tileMyAccount);
            this.tileGroup2.Items.Add(this.tileLogout);
            this.tileGroup2.Name = "tileGroup2";
            // 
            // tileSales
            // 
            tileItemElement1.Text = "Sales";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileSales.Elements.Add(tileItemElement1);
            this.tileSales.Id = 0;
            this.tileSales.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileSales.Name = "tileSales";
            this.tileSales.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileSales_ItemClick);
            // 
            // tileStore
            // 
            tileItemElement2.Text = "Store";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileStore.Elements.Add(tileItemElement2);
            this.tileStore.Id = 1;
            this.tileStore.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileStore.Name = "tileStore";
            this.tileStore.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileStore_ItemClick);
            // 
            // tileStockRoom
            // 
            tileItemElement3.Text = "Stock Room";
            tileItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileStockRoom.Elements.Add(tileItemElement3);
            this.tileStockRoom.Id = 2;
            this.tileStockRoom.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileStockRoom.Name = "tileStockRoom";
            this.tileStockRoom.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileStockRoom_ItemClick);
            // 
            // tileUsers
            // 
            tileItemElement4.Text = "Users";
            tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileUsers.Elements.Add(tileItemElement4);
            this.tileUsers.Id = 3;
            this.tileUsers.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileUsers.Name = "tileUsers";
            this.tileUsers.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileUsers_ItemClick);
            // 
            // tileLogout
            // 
            tileItemElement6.Text = "Logout";
            tileItemElement6.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileLogout.Elements.Add(tileItemElement6);
            this.tileLogout.Id = 4;
            this.tileLogout.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileLogout.Name = "tileLogout";
            this.tileLogout.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileLogout_ItemClick);
            // 
            // documentManager1
            // 
            this.documentManager1.MdiParent = this;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2016 Colorful";
            // 
            // tileMyAccount
            // 
            tileItemElement5.Text = "My Account";
            tileItemElement5.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileMyAccount.Elements.Add(tileItemElement5);
            this.tileMyAccount.Id = 6;
            this.tileMyAccount.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileMyAccount.Name = "tileMyAccount";
            this.tileMyAccount.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileMyAccount_ItemClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 670);
            this.Controls.Add(this.sidePanel1);
            this.IsMdiContainer = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.sidePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SidePanel sidePanel1;
        private DevExpress.XtraEditors.TileControl tileControl1;
        private DevExpress.XtraEditors.TileGroup tileGroup2;
        private DevExpress.XtraEditors.TileItem tileSales;
        private DevExpress.XtraEditors.TileItem tileStore;
        private DevExpress.XtraEditors.TileItem tileStockRoom;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraEditors.TileItem tileUsers;
        private DevExpress.XtraEditors.TileItem tileLogout;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.TileItem tileMyAccount;
    }
}