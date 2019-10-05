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
    public partial class frmFilterDateRange : Form
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public frmFilterDateRange()
        {
            InitializeComponent();
        }

        private void frmFilterDateRange_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = From != null ? From.Value : DateTime.Now;
            dtpTo.Value = To != null ? To.Value : DateTime.Now;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            From = dtpFrom.Value;
            To = dtpTo.Value;
            Close();
        }
    }
}
