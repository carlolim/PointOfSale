using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QualityPOS
{
    public partial class frmReportViewer : Form
    {
        public frmReportViewer()
        {
            InitializeComponent();
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            SqlCeConnection cn = new SqlCeConnection($@"DataSource={ Application.StartupPath }\POS.sdf; Password=ywMCsVSHnX");
            cn.Open();
            DataSet ds = new DataSet();

            SqlCeDataAdapter da = new SqlCeDataAdapter("select * FROM Product", cn);
            da.Fill(ds, "empds");


            ReportDataSource RDS = new ReportDataSource("empds", ds.Tables[0]);


            ReportViewer RV = new ReportViewer();
            RV.Visible = true;


            RV.ProcessingMode = ProcessingMode.Local;
            LocalReport lc = RV.LocalReport;
            lc.DataSources.Add(RDS);

            RV.LocalReport.Refresh();
        }
    }
}
