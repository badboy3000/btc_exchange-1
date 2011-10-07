using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using btc_exchange.lib;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace btc_exchange.Account
{
    public partial class register : System.Web.UI.Page
    {

        private string connectionString = WebConfigurationManager.ConnectionStrings["exchange"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            
            if (Page.IsValid)
            {
                string sqlQuery;
                sqlQuery = "EXEC dbo.add_user @user_name, @user_email, @password";
                using (SqlConnection dataConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand dataCommand = new SqlCommand(sqlQuery, dataConnection))
                    {
                        dataCommand.Parameters.AddWithValue("user_name", txtUser.Text);
                        dataCommand.Parameters.AddWithValue("user_email", txtEmail.Text);
                        dataCommand.Parameters.AddWithValue("password", BCrypt.HashPassword(txtUser.Text + txtPass.Text, BCrypt.GenerateSalt(12)));

                        try
                        {
                            dataConnection.Open();
                            dataCommand.ExecuteNonQuery();
                            Response.Redirect("/Account/login.aspx");
                        }
                        catch (Exception err)
                        {

                            string errText = err.ToString();
                            if (errText.IndexOf("IX_username") != -1)
                            {
                                lblError.Text = "Username already exists";
                            }
                            else
                            {
                                lblError.Text = "An unknown error occured. Please try again.";
                                lblError.Text = err.ToString();
                            }
                        }
                        finally
                        {
                            dataConnection.Close();
                        }
                    }
                }
            }
            else
            {
                lblCaptchaErr.Text = recaptcha.ErrorMessage.ToString();
            }
        }
    }
}