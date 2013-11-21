using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class graph : System.Web.UI.Page
{
    protected string information;
    protected string name;
    protected string[,] vitalSign;
    protected void Page_Load(object sender, EventArgs e)
    {

        //initialize all the global variables
        information = "";
        if (User.Identity.IsAuthenticated == true)
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string data = ticket.UserData;
            processData(data);
        }
    }

    protected void processData(string input)
    {
        string[] category1;
        string[] category2;
        //check if the data does not contain useful information
        if (!input.Contains("|"))
        {
            Response.Write("<script type='text/javascript'>alert('Without Selection')</script>");
            Response.Flush();
            return;
        }
        //first split the storing info and needed info;
        category1 = input.Split('|');
        information = category1[0];

        if (!category1[1].Contains(";"))
        {
            Response.Write("<script type='text/javascript'>alert('Null Data')</script>");
            Response.Flush();
            return;
        }

        //Second split the name and vital signs;
        category2 = category1[1].Split('!');
        name = category2[0];

        HiddenField1.Value = category2[1];
        
        /***set the default value**/
        selection_vital.SelectedIndex = 4;
        time_scale.SelectedIndex = 5;
        //ShowDiagram();
    }

}

//BACKUP OF ORIGINAL

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.Security;

//public partial class graph : System.Web.UI.Page
//{
//    protected string information;
//    protected string name;
//    protected string[,] vitalSign;
//    protected void Page_Load(object sender, EventArgs e)
//    {

//        //initialize all the global variables
//        information = "";
//        if (User.Identity.IsAuthenticated == true)
//        {
//            FormsIdentity id = (FormsIdentity)User.Identity;
//            FormsAuthenticationTicket ticket = id.Ticket;
//            string data = ticket.UserData;
//            processData(data);
//        }
//    }

//    protected void processData(string input)
//    {
//        string[] category1;
//        string[] category2;
//        //check if the data does not contain useful information
//        if (!input.Contains("|"))
//        {
//            Response.Write("<script type='text/javascript'>alert('Without Selection')</script>");
//            Response.Flush();
//            restoreInfo();
//            return;
//        }
//        //first split the storing info and needed info;
//        category1 = input.Split('|');
//        information = category1[0];

//        if (!category1[1].Contains(";"))
//        {
//            Response.Write("<script type='text/javascript'>alert('Null Data')</script>");
//            Response.Flush();
//            restoreInfo();
//            return;
//        }
//        //Second split the name and vital signs;
//        category2 = category1[1].Split('!');
//        name = category2[0];
//        //Third split different sets of vital signs
//        /*category1 = null;
//        category1 = category2[1].Split(';');

//       vitalSign = new string[(category1.Length - 1), 5];
//       for (int i = 0; i < (category1.Length - 1); i++)
//       {   //fourth split each items in a set of vital signs
//           category2 = null;
//           category2 = category1[i].Split(',');

//           for (int j = 0; j < category2.Length; j++)
//           {   // fill all the data into a #sets * #items 2D array
//               vitalSign[i, j] = category2[j];
//           }
//       }*/
//        restoreInfo();
//        HiddenField1.Value = category2[1];
        
//        /***set the default value**/
//        selection_vital.SelectedIndex = 4;
//        time_scale.SelectedIndex = 5;
//        //ShowDiagram();
//    }

//    protected void restoreInfo()
//    {
//        //Restore the storing data back to the cookie
//        // decrypt hte cookie and get all the data stored in it
//        HttpCookie cookie = FormsAuthentication.GetAuthCookie(User.Identity.Name, true);
//        var ticket = FormsAuthentication.Decrypt(cookie.Value);
//        // Store the patient ID into the ticket now
//        var newticket = new FormsAuthenticationTicket(ticket.Version,
//                                                        ticket.Name,
//                                                        ticket.IssueDate,
//                                                        ticket.Expiration,
//                                                        true, // always persistent
//                                                        information,
//                                                        ticket.CookiePath);
//        // Encrypt the ticket and store it in the cookie, let the cookie have the same expiration as the ticket
//        cookie.Value = FormsAuthentication.Encrypt(newticket);
//        cookie.Expires = newticket.Expiration.AddMinutes(10);

//        try
//        {
//            this.Context.Response.Cookies.Set(cookie);
//        }
//        catch (HttpException e)
//        {

//        }
//    }
///*
//    protected void ShowDiagram()
//    {*/
//        // To cleanup the values
//        // Label0.Text = null;
//        /*Get to know the index of choice*/
//        /* int index = 3;//Convert.ToInt32(selection_vital.Value);

//        for (int i = 0; i < vitalSign.GetLength(0); i++)
//        {
//            Label0.Text = Label0.Text + vitalSign[i, index];
//        }
//    }*/
//}