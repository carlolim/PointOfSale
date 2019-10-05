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

namespace QualityPOS
{
    public partial class Form1 : Form
    {
        UserManager _userManager = new UserManager();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await _userManager.Login(new Objects.User()
            {
                Username = "carlo",
                Password = "1234"
            });
        }
    }
}
