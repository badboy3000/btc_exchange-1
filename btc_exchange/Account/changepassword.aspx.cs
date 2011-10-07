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
    public partial class changepassword : System.Web.UI.Page
    {

        private string connectionString = WebConfigurationManager.ConnectionStrings["exchange"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblCaptchaErr.Text = "";
            if (Page.IsValid)
            {
                // Code to change password goes here
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
                        string newPassword = BCrypt.HashPassword(txtUser.Text + txtNewPass.Text, BCrypt.GenerateSalt(12));
                        string sqlQuery;
                        sqlQuery = "UPDATE dbo.users SET password = @password, lastseen = GETDATE() WHERE user_name = @username";
                        using (SqlConnection dataConnection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand dataCommand = new SqlCommand(sqlQuery, dataConnection))
                            {
                                dataCommand.Parameters.AddWithValue("password", newPassword);
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
                        Response.Redirect("/Account/login.aspx");
                    }
                    else
                    {
                        lblError.Text = "Incorrect username or password";
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

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {

        }
    }
}