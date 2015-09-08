using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace SDNPortal
{
    public partial class _Default : Page
    {
        private MySqlConnection connection;

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["SdnMySqlServer"].ToString());

        }

        protected void Search_Click(object sender, EventArgs e)
        {
            connection.Open();
            string query = String.Format("SELECT * FROM customers where customernumber like '%{0}%' and customername like '%{1}%' and location like '%{2}%'", txtNumber.Value, txtName.Value,txtLocation.Value) ;

            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            CustomersGrid.DataSource = ds.Tables[0];
            CustomersGrid.DataBind();

            connection.Close();
            custitle.Visible = true;

        }

        protected void CustomersGrid_DataBinding(object sender, EventArgs e)
        {

        }
    }
}