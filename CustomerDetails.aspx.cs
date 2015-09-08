using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SDNPortal
{
    public partial class _CustomerDetails : Page
    {
        private MySqlConnection connection;
        private  int pageloadcount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["SdnMySqlServer"].ToString());
            if (!IsPostBack)
            {             
                GetCustomerData(Request.QueryString["CustomerNumber"]);                
                lstRouters_SelectedIndexChanged(sender, e);
                
                Session.Add("pagehistory", pageloadcount.ToString());
            }

            pageloadcount = Convert.ToInt32(Session["pagehistory"].ToString()) + 1;
            Session.Add("pagehistory", pageloadcount.ToString());

        }

        protected void lstRouters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lstPolicy.Items.Clear();
                string PolicyList = API_Get(GetPolicyList(lstRouters.SelectedItem.Text));

                JObject joResponse = JObject.Parse(PolicyList);
                JObject ojObject = (JObject)joResponse["policy"];
                JArray array = (JArray)ojObject["vyatta-policy-qos:qos"];
               
                for (int i = 0; i < array.Count; i++)
                {
                    lstPolicy.Items.Add(array[i]["tagnode"].ToString());

                    connection.Open();
                    string squery = "Select * from policy where policyname = '" + array[i]["tagnode"].ToString() + "'";
                    MySqlCommand cmd1 = new MySqlCommand(squery, connection);
                    MySqlDataReader dr = cmd1.ExecuteReader();
                    if (dr.HasRows == false)
                    {
                        dr.Close();
                        string squery1 = String.Format("insert into policy select max(idpolicy)+1, '{0}','{1}' from policy", array[i]["tagnode"].ToString(), Get_Policy(lstRouters.SelectedItem.Text, array[i]["tagnode"].ToString()));
                        MySqlCommand cmd2 = new MySqlCommand(squery1, connection);
                        int updatestatus = cmd2.ExecuteNonQuery();
                    }
                    else
                        dr.Close();
                    connection.Close();
                }
                GetAllPolicies();
                if (lstPolicy.Items.Count > 0)
                    lstPolicy.SelectedIndex = 0;
                lstPolicy_SelectedIndexChanged(sender, e);

            }
            catch (Exception ex)
            {

            }
        }

        protected void lstPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                description.Value = Get_Policy(lstRouters.SelectedItem.Text,lstPolicy.SelectedItem.Text);
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkEditRout_Click(object sender, EventArgs e)
        {
            try
            { 
            mpePopUp.Show();
            txtRouterName.Value = lstRouters.SelectedItem.Text;            
            Get_Interfaces();
            lstPmap.Items.Clear();
            lstPmap1.Items.Clear();
            foreach (ListItem li in lstPolicy.Items)
            {
                lstPmap.Items.Add(li);
                lstPmap1.Items.Add(li);
            }
            }
            catch (Exception ex)
            {

            }
        }

        protected void routSave_Click(object sender, EventArgs e)
        {
            try
            { 
            if (hidRType.Value == "New")
            {
                connection.Open();

                string squery = String.Format("insert into routers select max(idrouters)+1, '{0}','{1}' from routers", txtRouterName.Value, "");
                MySqlCommand cmd1 = new MySqlCommand(squery, connection);
                int updatestatus = cmd1.ExecuteNonQuery();

                squery = "SELECT max(idrouters) FROM routers";
                MySqlCommand cmd2 = new MySqlCommand(squery, connection);
                MySqlDataReader dr = cmd2.ExecuteReader();
                dr.Read();
                string routid = dr.GetString(0);
                dr.Close();

                foreach (ListItem l1 in lstPmap.Items)
                {
                    squery = String.Format("insert into rout_policy select max(idrout_policy)+1, {0},{1} from rout_policy", routid, l1.Value);
                    MySqlCommand cmd3 = new MySqlCommand(squery, connection);
                    updatestatus = cmd3.ExecuteNonQuery();
                }

                squery = String.Format("insert into cust_routers select max(idcust_routers)+1, {0},{1} from cust_routers", Session["custid"].ToString(), routid);
                MySqlCommand cmd4 = new MySqlCommand(squery, connection);
                updatestatus = cmd4.ExecuteNonQuery();

                connection.Close();
                //GetCustomerData(Request.QueryString["CustomerNumber"]);
                mpePopUp.Hide();
            }
            else
            {
                connection.Open();
                string routid = lstRouters.SelectedItem.Value;
                  
                    string squery = String.Format("delete from rout_policy where router_id = {0}", routid);
                MySqlCommand cmd2 = new MySqlCommand(squery, connection);
                cmd2.ExecuteNonQuery();

                foreach (ListItem lp in lstPolicy.Items)
                {
                    try
                    {
                        Push_Policy("DeletePolicyMap", lstRouters.SelectedItem.Text, lp.Text, "");
                    }
                    catch(Exception ex)
                        {
                           
                        }
                }
                lstPolicy.Items.Clear();
                foreach (ListItem l1 in lstPmap.Items)
                {
                    squery = String.Format("insert into rout_policy select case when max(idrout_policy) is null then 1 else max(idrout_policy)+1 end, {0}, (select idpolicy from policy where policyname = '{1}' limit 1) from rout_policy", routid, l1.Text);
                    MySqlCommand cmd3 = new MySqlCommand(squery, connection);
                    int updatestatus = cmd3.ExecuteNonQuery();

                    squery = String.Format("SELECT policyname,policydescription FROM policy where policyname = '{0}'", l1.Text);
                    MySqlCommand cmdpol = new MySqlCommand(squery, connection);
                    MySqlDataReader dr = cmdpol.ExecuteReader();
                    dr.Read();
                    Push_Policy("AddPolicy", lstRouters.SelectedItem.Text, dr.GetString(0), dr.GetString(1));
                    lstPolicy.Items.Add(l1.Text);
                    dr.Close();
                }
                connection.Close();

               // GetCustomerData(Request.QueryString["CustomerNumber"]);
                mpePopUp.Hide();
            }
            }
            catch (Exception ex)
            {

            }
        }

        protected void routCancel_Click(object sender, EventArgs e)
        {
            mpePopUp.Hide();
        }

        protected void PMap_Click(object sender, EventArgs e)
        {
            try
            { 
            foreach (int li in lstPavbl.GetSelectedIndices())
            {
                ListItem avl = new ListItem(lstPavbl.Items[li].Text, lstPavbl.Items[li].Value);
                lstPmap.Items.Add(avl);
                lstPmap1.Items.Add(avl);
            }
            mpePopUp.Show();
            }
            catch (Exception ex)
            {

            }
        }

        protected void PuMap_Click(object sender, EventArgs e)
        {
            try
            { 
            foreach (int li in lstPmap.GetSelectedIndices())
            {
                lstPmap.Items.Remove(lstPmap.Items[li]);
                lstPmap1.Items.Remove(lstPmap1.Items[li]);
            }
            mpePopUp.Show();
            }
            catch (Exception ex)
            {

            }
        }

        protected void routMapInterface_Click(object sender, EventArgs e)
        {
            try
            { 
            AssignPolicytoInterfaces();
            lblpopRmessage.Text = "Selected interface mapped with selected policy";
            mpePopUp.Show();
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkPolicyAdd_Click(object sender, EventArgs e)
        {
            try
            { 
            hidimpact.Value = "New";
            mpePopUp2.Show();
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkModify_Click(object sender, EventArgs e)
        {
            hidimpact.Value = "Edit";
            txtNewPloicyName.Value = lstPolicy.SelectedItem.Text;
            txtNewPolicyDescr.Value = description.Value;
            mpePopUp2.Show();
        }

        protected void polySave_Click(object sender, EventArgs e)
        {
            try
            { 

            if (hidimpact.Value == "New")
            {
                connection.Open();
                string squery = String.Format("insert into policy select max(idpolicy)+1, '{0}','{1}' from policy", txtNewPloicyName.Value, txtNewPolicyDescr.Value);
                MySqlCommand cmd1 = new MySqlCommand(squery, connection);
                int updatestatus = cmd1.ExecuteNonQuery();

                squery = "SELECT max(idpolicy) FROM policy";
                MySqlCommand cmd2 = new MySqlCommand(squery, connection);
                MySqlDataReader dr = cmd2.ExecuteReader();
                dr.Read();
                string polyid = dr.GetString(0);
                dr.Close();

                squery = String.Format("insert into rout_policy select max(idrout_policy)+1, {0},{1} from rout_policy", lstRouters.SelectedItem.Value, polyid);
                MySqlCommand cmd3 = new MySqlCommand(squery, connection);
                updatestatus = cmd3.ExecuteNonQuery();
                Push_Policy("AddPolicy", lstRouters.SelectedItem.Text, txtNewPloicyName.Value, txtNewPolicyDescr.Value);
                connection.Close();
                // GetCustomerData(Request.QueryString["CustomerNumber"]);
                lstPavbl.Items.Add(txtNewPloicyName.Value);
                lstPolicy.Items.Add(txtNewPloicyName.Value);
               // GetAllPolicies();
                mpePopUp2.Hide();
            }
            else
            {
                connection.Open();
                mpePopUp3.Show();
                description.Value = txtNewPolicyDescr.Value;
                string query = String.Format("SELECT distinct rp.router_id, routersName FROM policy p join rout_policy rp on p.idpolicy = rp.policy_id join routers r on rp.router_id = r.idrouters join cust_routers cr on r.idrouters = cr.router_id join customers c on cr.cust_id = c.idcustomers where p.policyname = '{0}'", lstPolicy.SelectedItem.Text);

                MySqlCommand cmd = new MySqlCommand(query, connection);               
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                CustomersGrid.DataSource = ds.Tables[0];
                CustomersGrid.DataBind();
                connection.Close();
            }
            }
            catch (Exception ex)
            {

            }
        }

        protected void polyCancel_Click(object sender, EventArgs e)
        {
            mpePopUp2.Hide();
        }

        protected void btnUpdatePolicy_Click(object sender, EventArgs e)
        {
            try
            { 
            int chkcount = 0;
            for (int i = 0; i < CustomersGrid.Rows.Count; i++)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)CustomersGrid.Rows[i].FindControl("chksel");

                if (cb != null && cb.Checked)
                {
                        connection.Open();
                        string squery = String.Format("update policy set policydescription = '{1}' where policyname = '{0}'", txtNewPloicyName.Value, txtNewPolicyDescr.Value);
                        MySqlCommand cmd1 = new MySqlCommand(squery, connection);
                        int updatestatus = cmd1.ExecuteNonQuery();
                        connection.Close();

                        Push_Policy("AddPolicy", CustomersGrid.Rows[i].Cells[2].Text, lstPolicy.SelectedItem.Text, txtNewPolicyDescr.Value);
                    chkcount += 1;
                }
            }
                

            mpePopUp3.Hide();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancelP_Click(object sender, EventArgs e)
        {
            mpePopUp3.Hide();
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myFunction", "javascript:history.go(-" + Session["pagehistory"].ToString() + ");", true);
        }

        protected void btnPopup_Click(object sender, EventArgs e)
        {
            mpePopUp.Show();
        }

        protected void lnkRouterAdd_Click(object sender, EventArgs e)
        {
            mpePopUp.Show();
        }

        protected void chkselh_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbh = (CheckBox)CustomersGrid.HeaderRow.FindControl("chkselh");
            // Iterate through the Products.Rows property
            foreach (GridViewRow row in CustomersGrid.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("chksel");

                if (cb != null)
                    cb.Checked = cbh.Checked;
            }
            mpePopUp3.Show();
        }

        protected void GetCustomerData(string custNumber)
        {
            connection.Open();
            string query1 = String.Format("SELECT * FROM customers where customernumber = {0}", custNumber.ToString());
            int custid = 0;
            MySqlCommand cmd1 = new MySqlCommand(query1, connection);

            MySqlDataReader dr = cmd1.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                custid = dr.GetInt32(0);
                txtNumber.Value = dr.GetString(1);
                txtName.Value = dr.GetString(2);
                txtLocation.Value = dr.GetString(3);
            }
            dr.Close();
            Session.Add("custid", custid);
            string query2 = String.Format("SELECT * FROM routers where idrouters in (select router_id from cust_routers where cust_id = {0})", custid);

            MySqlCommand cmd2 = new MySqlCommand(query2, connection);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd2);
            DataSet ds1 = new DataSet();
            adp.Fill(ds1);

            lstRouters.DataSource = ds1.Tables[0];
            lstRouters.DataTextField = "routersname";
            lstRouters.DataValueField = "idrouters";
            lstRouters.DataBind();
            lstRouters.SelectedIndex = 0;
            connection.Close();         
        }

        protected void GetAllPolicies()
        {
            connection.Open();
            lstPavbl.Items.Clear();
            string query3 = "SELECT distinct policyname FROM policy";
            MySqlCommand cmd3 = new MySqlCommand(query3, connection);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd3);
            DataSet ds2 = new DataSet();
            adp.Fill(ds2);            
            lstPavbl.DataSource = ds2.Tables[0];
            lstPavbl.DataTextField = "policyname";
            lstPavbl.DataValueField = "policyname";
            lstPavbl.DataBind();
            connection.Close();

        }

        protected void Get_Interfaces()
        {
            string strRequestURL = "";
            WebRequest req;
            HttpWebResponse resp;
            var encoding = ASCIIEncoding.ASCII;

            strRequestURL = GetInterfaces(lstRouters.SelectedItem.Text);
            req = WebRequest.Create(strRequestURL);
            req.Method = "GET";
            //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("admin:admin"));      -- Incease credential needed uncomment it.      
            resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new System.IO.StreamReader(resp.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();

                JObject joResponse = JObject.Parse(responseText);
                JObject ojObject = (JObject)joResponse["interfaces"];
                JArray array = (JArray)ojObject["vyatta-interfaces-dataplane:dataplane"];
                lstInterfaces.Items.Clear();
                for (int i = 0; i < array.Count; i++)
                {
                    lstInterfaces.Items.Add(array[i]["tagnode"].ToString());
                }                
            }
        }     

        protected string Get_Policy(string routername, string policyname)
        {          
            string strRequestURL = "";
            WebRequest req;
            HttpWebResponse resp;
            var encoding = ASCIIEncoding.ASCII;

            strRequestURL = CreateRequest(routername, policyname);
            req = WebRequest.Create(strRequestURL);
            req.Method = "GET";
            //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("admin:admin"));      -- Incease credential needed uncomment it.      
            resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new System.IO.StreamReader(resp.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                return responseText;
            }
        }

        protected void AssignPolicytoInterfaces()
        {
            string strRequestURL = "";
            WebRequest req;
            HttpWebResponse resp;
            var encoding = ASCIIEncoding.ASCII;
            string pushinterface = "{\"vyatta-interfaces-dataplane:dataplane\":[{\"tagnode\": ";

            strRequestURL = GetInterfaceDetail(txtRouterName.Value, lstInterfaces.SelectedItem.Value);
            req = WebRequest.Create(strRequestURL);
            req.Method = "GET";            
            resp = req.GetResponse() as HttpWebResponse;
            using (var reader = new System.IO.StreamReader(resp.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                JObject joResponse = JObject.Parse(responseText);                
                JArray array = (JArray)joResponse["vyatta-interfaces-dataplane:dataplane"];                
                pushinterface = pushinterface + "\"" + array[0]["tagnode"].ToString() + "\", \"vyatta-policy-qos:qos-policy\": ";
                pushinterface = pushinterface + "\"" + lstPmap1.SelectedItem.Text + "\", \"address\": ";
                pushinterface = pushinterface + array[0]["address"].ToString() + "}]}";

            }
                       
            req = WebRequest.Create(strRequestURL);
            req.Method = "PUT";
            req.ContentType = "application/json";

            encoding = new UTF8Encoding();
            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(pushinterface);
            req.ContentLength = bytes.Length;

            using (var writeStream = req.GetRequestStream())
            {
                writeStream.Write(bytes, 0, bytes.Length);
            }
        }

        protected void Push_Policy(string action,string routername, string policyname, string policydescr)
        {           
            string strRequestURL = "";
            WebRequest req;                

            if (action == "AddPolicy")
            {
                strRequestURL = CreateRequest(routername, policyname); 
                req = WebRequest.Create(strRequestURL);
                req.Method = "PUT";
                req.ContentType = "application/json";

                var encoding = new UTF8Encoding();
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(policydescr);
                req.ContentLength = bytes.Length;

                using (var writeStream = req.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }   
            else if(action == "DeletePolicyMap")
            {
                strRequestURL = CreateRequest(routername, policyname);
                req = WebRequest.Create(strRequestURL);
                req.Method = "DELETE";               
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;                
            }                    
        }

        protected string API_Get(string APIURL)
        {            
            WebRequest req = WebRequest.Create(APIURL);
            req.Method = "GET";
            //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("admin:admin"));            
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(resp.GetResponseStream(), encoding))
            {
                return reader.ReadToEnd();                
            }       
        }

        protected string CreateRequest(string routername, string policyname)
        {
            string UrlRequest = ConfigurationManager.AppSettings["APILink"].ToString() + "node/" + routername + "/yang-ext:mount/vyatta-policy:policy/vyatta-policy-qos:qos/" + policyname + "/";
                                 
            return (UrlRequest);
        }

        protected string GetNodesLink()
        {
            string UrlRequest = ConfigurationManager.AppSettings["APILink"].ToString();
            return (UrlRequest);
        }

        protected string GetPolicyList(string routername)
        {
            string UrlRequest = ConfigurationManager.AppSettings["APILink"].ToString() + "node/" + routername + "/yang-ext:mount/vyatta-policy:policy/";
            return (UrlRequest);
        }

        protected string GetInterfaces(string routername)
        {
            string UrlRequest = ConfigurationManager.AppSettings["APILink"].ToString() + "node/" + routername + "/yang-ext:mount/vyatta-interfaces:interfaces";
            return (UrlRequest);
        }

        protected string GetInterfaceDetail( string routername, string interfacename)
        {
            string UrlRequest = ConfigurationManager.AppSettings["APILink"].ToString() + "node/" + routername + "/yang-ext:mount/vyatta-interfaces:interfaces/vyatta-interfaces-dataplane:dataplane/" + interfacename;
            return (UrlRequest);
        }                      
    }
}