using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bitnet.Client;
using System.Net;

namespace btc_exchange
{
    public partial class Deposit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BitnetClient nmc = new BitnetClient("http://127.0.0.1:9332");
            nmc.Credentials = new NetworkCredential("enmaku", "nightowl");
            //bc.SendFrom("enmaku", "ms43w2uQ4nUerVRmTGRBvwxG1XY3B4uDST", 1, 1, "sent from Bitnet code", "");
            var p = nmc.GetBalance();

            BitnetClient btc = new BitnetClient("http://127.0.0.1:8332");
            btc.Credentials = new NetworkCredential("enmaku", "nightowl");
            var q = btc.GetBalance("mtred (multi)", 0);
        }
    }
}