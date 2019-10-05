using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Windows.Forms;

namespace QualityPOS.Repository
{
    public class DatabaseConnection
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["localhost"].ConnectionString;
        public SqlCeConnection Connection;

        public DatabaseConnection()
        {
            ConnectionString = ConnectionString.Replace("|Application.StartupPath|", Application.StartupPath);
            Connection = new SqlCeConnection();
            Connection.ConnectionString = ConnectionString;
        }
    }
}
