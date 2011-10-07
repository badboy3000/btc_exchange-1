using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using btc_exchange.lib;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.Security;

namespace btc_exchange.Account
{
    public partial class login : System.Web.UI.Page
    {

        private string connectionString = WebConfigurationManager.ConnectionStrings["exchange"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblCaptchaErr.Text = "";
            if (Page.IsValid)
            {
                string selectSQL;
                selectSQL = "SELECT * FROM dbo.users WHERE user_name LIKE @user_name";

                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                SqlParameter iParam = new SqlParameter();
                iParam.ParameterName = "@user_name";
                iParam.SqlDbType = System.Data.SqlDbType.VarChar;
                iParam.Size = 50;
                iParam.Direction = System.Data.ParameterDirection.Input;
                iParam.Value = txtUser.Text;
                cmd.Parameters.Add(iParam);
                SqlDataReader reader;

                try
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    string hashedPassword = reader["password"].ToString();
                    if (BCrypt.CheckPassword(txtUser.Text + txtPass.Text, hashedPassword))
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
                        }
                        catch
                        {
                            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                            byte[] key = new byte[64];
                            rng.GetBytes(key);
                            sessionKey = Convert.ToBase64String(key);
                            Session["sKey01"] = sessionKey;
                        }

                        // Concatenate the session key and the fingerprint, then compute an SHA512 hash
                        // This hash requires both the session cookie, which can be stolen (a la firesheep)
                        // and matching HTTP headers, which can also be faked but with somewhat more difficulty
                        // This will be permuted at the database level before storing (to prevent faking if SQL is
                        // compromised) and a job deletes stored session keys more than 10 minutes old, thus
                        // enforcing auto-logout of idle clients at the database level.

                        sessionKey += encHeaders;
                        sessionKey = Convert.ToBase64String(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sessionKey)));

                        // absolutely nothing can be done at the database level without this un-permuted session
                        // key since everything works through stored procedures which all take this session key
                        // as input and validate it against the permuted stored version for each transaction.
                        // Each row of every table has a validation field which is invalidated by changes done
                        // outside of the stored procedures, thus a SQL compromise is useless without a separate
                        // account compromise. Also, since the SQL account ASP.NET is using for the site does not have
                        // access to the stored procedures that finalize a deposit, SQL access nets an attacker
                        // no new abilities compared to what could be done through the web interface. Balances
                        // cannot be modified except for a single stored procedure that can be run only by an
                        // executable local to the SQL server running under a different SQL account.
                        // Any discrepancy between computed and stored validation fields automatically results
                        // in the locking of the affected account.

                        // Update the session key in the database

                        string sqlQuery;
                        sqlQuery = "UPDATE dbo.users SET session_key = @skey, lastseen = GETDATE() WHERE user_name = @username";
                        using (SqlConnection dataConnection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand dataCommand = new SqlCommand(sqlQuery, dataConnection))
                            {
                                dataCommand.Parameters.AddWithValue("skey", sessionKey);
                                dataCommand.Parameters.AddWithValue("username", txtUser.Text);

                                try
                                {
                                    dataConnection.Open();
                                    dataCommand.ExecuteNonQuery();
                                }
                                finally
                                {
                                    dataConnection.Close();
                                }
                            }
                        }

                        Session["username"] = txtUser.Text;
                        Response.Redirect("/Default.aspx");
                    }
                    else
                    {
                        lblError.Text = "Username or password are incorrect";
                    }
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                lblCaptchaErr.Text = recaptcha.ErrorMessage.ToString();
            }
        }
    }
}