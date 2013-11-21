using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Parse;
using System.Web.Security;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(User.Identity.IsAuthenticated)
            displayHint("Successfully logged into the system", System.Drawing.Color.Indigo);
    }
    protected async void Button1_Click(object sender, System.EventArgs e)
    {
        string username = Login1.UserName;
        string password = Login1.Password;
        var query = ParseObject.GetQuery("Authentication").WhereEqualTo("userName", username);
        int count = await query.CountAsync();
        IEnumerable<ParseObject> result = await query.FindAsync();

        // wrong username
        if (count == 0)
        {
            //displayHint("Username or password is invalid", System.Drawing.Color.Red);
            Response.Write("<script type='text/javascript'>alert('Fail in logging')</script>");
            Response.Flush();
        }
        foreach (ParseObject s in result)
        {
            try
            {
                if (s.Get<string>("password") == password)
                {
                    authenticate(username);   
                }
                else
                {
                    displayHint("Username or password is invalid", System.Drawing.Color.Red);
                }
            }
            catch (Exception)
            {

            }
        }
        
    }
    protected void authenticate(string username)
    {
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(10), true, String.Empty, FormsAuthentication.FormsCookiePath);
        string encryptedCookie = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedCookie);
        cookie.Expires = DateTime.Now.AddMinutes(10);
        Response.Cookies.Add(cookie);
        //go to patient information page
        Response.Redirect("~/Department.aspx");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void displayHint(string str, System.Drawing.Color color)
    {
        Label6.Font.Size = 15;
        Label6.Font.Bold = true;
        Label6.ForeColor = color;
        Label6.Text = str;
    }
}