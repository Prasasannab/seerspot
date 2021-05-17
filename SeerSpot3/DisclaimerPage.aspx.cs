using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeerSpot3
{
    public partial class DisclaimerPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("njnjnjn");
            string userip = null;
            GetIpAddress(out userip);
        }

        protected void AcceptBtn_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = new HttpCookie("my cookie");
            cookie.Expires = DateTime.Now.AddDays(60);
            

            HttpBrowserCapabilities capabilities = Request.Browser;
            string br = capabilities.Browser;
            string ip = Request.UserHostAddress;

            cookie["ip"] = ip;
            //cookie["browser"] =br;


            Response.Cookies.Add(cookie);
            string piu = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARD_FOR"];
            if (string.IsNullOrEmpty(piu))
            {
                piu = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                piumal.Text = piu;
            }
          //  piumal.Text = ip;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "messae", "alert('Ip: " + HttpContext.Current.Request.UserHostAddress+ "')", true);

            System.Diagnostics.Debug.WriteLine(ip);


            Response.Redirect("http://seerspot3.azurewebsites.net/");
        }

        private void GetIpAddress(out string userip)
        {
            userip = Request.UserHostAddress;
            if (Request.UserHostAddress != null)
            {
                Int64 macinfo = new Int64();
                string macSrc = macinfo.ToString("X");
                if (macSrc == "0")
                {
                    if (userip == "127.0.0.1")
                    {
                        piumal.Text = "localhost ";
                        Response.Write("visited Localhost!");
                    }
                    else
                    {
                        piumal.Text = userip;
                    }
                }
            }
        }

    }
}
