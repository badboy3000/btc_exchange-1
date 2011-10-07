using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.Security;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace btc_exchange
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {

        private string connectionString = WebConfigurationManager.ConnectionStrings["exchange"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            // Disallow the caching of pages
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            // Show anon or known user controls, as appropriate
            try
            {
                string test1 = Session["sKey01"].ToString();
                if (test1 != "")
                {
                    // Non-empty session key, user is logged in
                    pnlKnown.Visible = true;
                    pnlAnon.Visible = false;
                }
                else
                {
                    // Empty session key, user has logged out
                    pnlKnown.Visible = false;
                    pnlAnon.Visible = true;
                }
            }
            catch
            {
                // No session key, user not logged in yet
                pnlKnown.Visible = false;
                pnlAnon.Visible = true;
            }

            // Get the current page's file name
            string[] file = Request.CurrentExecutionFilePath.Split('/');
            string fileName = file[file.Length - 1];
            fileName = fileName.ToLower();

            // If we're not on a public page, check session key and redirect to login if invalid

            if (fileName != "default.aspx" && fileName != "login.aspx" && fileName != "register.aspx"
                && fileName != "logout.aspx" && fileName != "data.aspx" && fileName != "jsontest.aspx")
            {

                // Building a "footprint" of the current user from IP & HTTP headers
                string strHeaders = HttpContext.Current.Session.SessionID.ToString();
                strHeaders += HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                strHeaders += HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                strHeaders += HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_CHARSET"];
                strHeaders += HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"];
                strHeaders += HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];

                // Hash the "footprint" with SHA512 to avoid storing PII
                string encHeaders = Convert.ToBase64String(new System.Security.Cryptography.SHA512CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(strHeaders)));

                // Either retrieve or generate the random 512-bit session key
                string sessionKey;
                try
                {
                    sessionKey = Session["sKey01"].ToString();
                    sessionKey += encHeaders;
                    sessionKey = Convert.ToBase64String(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sessionKey)));

                    string selectSQL;
                    selectSQL = "SELECT * FROM dbo.users WHERE user_name LIKE @username";

                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand(selectSQL, con);
                    SqlParameter iParam = new SqlParameter();
                    iParam.ParameterName = "@username";
                    iParam.SqlDbType = System.Data.SqlDbType.VarChar;
                    iParam.Size = 50;
                    iParam.Direction = System.Data.ParameterDirection.Input;
                    iParam.Value = Session["username"].ToString();
                    cmd.Parameters.Add(iParam);
                    SqlDataReader reader;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    string sKey01 = reader["session_key"].ToString();

                    if (sessionKey != sKey01)
                    {
                        Response.Redirect("/Account/logout.aspx");
                    }
                }
                catch
                {
                    Response.Redirect("/Account/login.aspx");
                }
            }

        }
    }

    
}
