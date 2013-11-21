using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Http;
using Parse;

public partial class About : Page
{
    protected string[] candidate;
    protected string IDseries;
    //Prepare the JSON string the pass the neccessary information into cookie
    protected string JSON;
    //used for clean data
    protected string information;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        //initialize JSON, IDseries
        IDseries = "";
        JSON = "";
        if (User.Identity.IsAuthenticated == true)
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            
            //clean the cookie
            if (ticket.UserData.Contains("|"))
            {
                String[] tmp = ticket.UserData.Split('|');
                information = tmp[0];
                IDseries = information;
                cleanInfo();

            }
            else
            {
                IDseries = ticket.UserData;
            }
            
            
            
        }
        
        if (IDseries.Length>0)
        {
            string localID = ParseID(IDseries);
            fetchdata(localID);
        }
        else
        {
            Response.Redirect("~/Department.aspx");
        }
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Department.aspx", false);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        change(-1);
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        change(1);
    }
    /*Redirect the page to graph page, and to keep the vital sign info into cookie*/
    protected void Button5_Click(object sender, EventArgs e)
    {
        if (JSON.Length == 0)
        {
            Response.Write("<script type='text/javascript'>alert('Error Occurs')</script>");
            Response.Flush();
        }
        else
        {

            // decrypt the cookie and get all the data stored in it
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(User.Identity.Name, true);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            // Store the patient ID into the ticket now
            // in sync with the web.config
            string userData = IDseries+"|"+JSON;
            var newticket = new FormsAuthenticationTicket(ticket.Version,
                                                          ticket.Name,
                                                          ticket.IssueDate,
                                                          ticket.Expiration,
                                                          true, // always persistent
                                                          userData,
                                                          ticket.CookiePath);
            // Encrypt the ticket and store it in the cookie, let the cookie have the same expiration as the ticket
            cookie.Value = FormsAuthentication.Encrypt(newticket);
            cookie.Expires = newticket.Expiration.AddMinutes(10);
            this.Context.Response.Cookies.Set(cookie); 

            Response.Redirect("~/graph.aspx", false);
        }
        
    }
    protected void change(int offset)
    {
        int length = candidate.Length;
        int index = (Convert.ToInt32(candidate[0]) + offset) % (length - 1);
        if (index < 0)
        {
            index += (length - 1);
        }
        string tmp = "" + index;
        for (int i = 1; i < length; i++)
        {
            tmp += "," + candidate[i];
        }
        // decrypt hte cookie and get all the data stored in it
        HttpCookie cookie = FormsAuthentication.GetAuthCookie(User.Identity.Name, true);
        var ticket = FormsAuthentication.Decrypt(cookie.Value);
        // Store the patient ID into the ticket now
        var newticket = new FormsAuthenticationTicket(ticket.Version,
                                                      ticket.Name,
                                                      ticket.IssueDate,
                                                      ticket.Expiration,
                                                      true, // always persistent
                                                      tmp,
                                                      ticket.CookiePath);
        // Encrypt the ticket and store it in the cookie, let the cookie have the same expiration as the ticket
        cookie.Value = FormsAuthentication.Encrypt(newticket);
        cookie.Expires = newticket.Expiration.AddMinutes(10);
        this.Context.Response.Cookies.Set(cookie);

        //refresh 
        Response.Redirect("~/About.aspx", false);
    }
    protected String ParseID(string ptID)
    {
        candidate = ptID.Split(',');
        int index =Convert.ToInt32(candidate[0])+1;
        return candidate[index];
    }

    /*Load the patient photo from database*/
    protected async void LoadPhoto(string ptID)
    {
        var query = ParseObject.GetQuery("Patient_photo").WhereEqualTo("iD", ptID);
        IEnumerable<ParseObject> result = await query.FindAsync();

        string fileURL;
        foreach (ParseObject s in result)
        {
           var profile = s.Get<ParseFile>("photo");
           fileURL = await new HttpClient().GetStringAsync(profile.Url);
           //imgUpload.Load(""+fileURL);
           Label0.Text = "hello bro"+fileURL;
        }
       // string filename = Path.GetFileName(imgUpload.FileName);
       // if(filename != null)
            

    }
    protected async void fetchdata(string ptID)
    {
        // try to Load the photo to the page
        LoadPhoto(ptID);
        // add the timestamp;
        int counthr = 0;
        int[] timestamp = new int[5];
        // Store the patient ID into the ticket now
        var query = ParseObject.GetQuery("Test").WhereEqualTo("iD", ptID).OrderByDescending("createdAt").Limit(20);
        try
        {
            int count = await query.CountAsync();
        }
        catch (Exception e)
        {

        };
        IEnumerable<ParseObject> result = await query.FindAsync();

        if (!User.Identity.IsAuthenticated)
        {
            switchFont(true);

            var row = new TableRow();
            addnewCell(row, "You have no authentication, please log in.");
            Table1.Rows.Add(row);
        }
        else
        {
            // set the font back to normal
            switchFont(false);

            //Initialize the table with item names
            initializeTable(Table1);

            foreach (ParseObject s in result)
            {
                var row = new TableRow();
                try
                {
                    addnewCell(row, s.Get<String>("name"));
                    addnewCell(row, s.Get<String>("temp"));
                    addnewCell(row, s.Get<String>("sys"));
                    addnewCell(row, s.Get<String>("dia"));
                    addnewCell(row, s.Get<String>("pulse"));
                    addnewCell(row, s.Get<String>("spo2"));
                    addnewCell(row, s.Get<String>("rr"));
                    addnewCell(row, s.Get<String>("pain")); 

                    //prepare the timezonedestination for converting UTC to EDT time
                    TimeZoneInfo edt = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime convertTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)s.CreatedAt, edt);

                    string str = "" + convertTime;
                    addnewCell(row, str);
                   // str = "" + TimeZoneInfo.ConvertTimeFromUtc((DateTime)s.UpdatedAt, edt);
                   // addnewCell(row, str);
                    Table1.Rows.Add(row);

                    counthr++;
                    // define the current time
                    DateTime currentTime = DateTime.Now;
                    // compare the time difference to distinguish 12 hrs, 24 hrs, 36 hrs, 48 hrs, or one month
                    TimeSpan offset = currentTime - convertTime;
                    if (offset.Days <= 30)
                    {
                        timestamp[0] = counthr;
                        timestamp[1] = counthr;
                        timestamp[2] = counthr;
                        timestamp[3] = counthr;
                        timestamp[4] = counthr;
                    }
                    else if ( offset.Days <= 60)
                    {
                        timestamp[1] = counthr;
                        timestamp[2] = counthr;
                        timestamp[3] = counthr;
                        timestamp[4] = counthr;
                    }
                    else if ( offset.Days <= 90)
                    {
                        timestamp[2] = counthr;
                        timestamp[3] = counthr;
                        timestamp[4] = counthr;
                    }
                    else if (offset.Days <= 120)
                    {
                        timestamp[3] = counthr;
                        timestamp[4] = counthr;
                    }
                    else if (offset.Days == 150)
                    {
                        timestamp[4] = counthr;
                    }

                    JSON = JSON + s.Get<String>("temp") + "," + s.Get<String>("sys") + "," + s.Get<String>("dia") 
                        + "," + s.Get<String>("pulse") + "," + s.Get<String>("spo2") + "," + str+";";
                }
                catch (Exception)
                {

                }

            }
            JSON = ptID + "!" +timestamp[0]+","+timestamp[1]+","+timestamp[2]+","+timestamp[3]+","+timestamp[4]+";"+ JSON;
        }
    }
    protected void addnewCell(TableRow row, string content)
    {
        var cell = new TableCell();
        setCellBorder(cell);
        cell.Text = content;
        row.Cells.Add(cell);
    }
    protected void initializeTable(Table table)
    {
        var row = new TableRow();
        //addnewCell(row, "ObjectID");
        addnewCell(row, "Name");
        //addnewCell(row, "ID");
        addnewCell(row, "Temp");
        addnewCell(row, "SYS");
        addnewCell(row, "DIA"); 
        addnewCell(row, "Pulse");
        addnewCell(row, "SPO2");
        addnewCell(row, "RR");
        addnewCell(row, "Pain");
        addnewCell(row, "CreatedAt(MM/DD/YYYY)");
        //addnewCell(row, "UpdatedAt(MM/DD/YYYY)");
        table.Rows.Add(row);

    }
    private void switchFont(bool sign)
    {
        if (sign)
        {
            // set table with original background color
            Table1.BackColor = System.Drawing.Color.Gray;
            Table1.Font.Size = 20;
        }
        else
        {
            // set table with another background color
            Table1.BackColor = System.Drawing.Color.Beige;
            Table1.Font.Size = 10;
        }
        Table1.Font.Bold = sign;
    }
    protected void setCellBorder(TableCell cell)
    {
        cell.BorderWidth = 1;
        cell.BorderColor = System.Drawing.Color.Gray;
        cell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Dotted;
    }

    protected void cleanInfo()
    {
        //Restore the storing data back to the cookie
        // decrypt hte cookie and get all the data stored in it
        HttpCookie cookie = FormsAuthentication.GetAuthCookie(User.Identity.Name, true);
        var ticket = FormsAuthentication.Decrypt(cookie.Value);
        // Store the patient ID into the ticket now
        var newticket = new FormsAuthenticationTicket(ticket.Version,
                                                        ticket.Name,
                                                        ticket.IssueDate,
                                                        ticket.Expiration,
                                                        true, // always persistent
                                                        information,
                                                        ticket.CookiePath);
        // Encrypt the ticket and store it in the cookie, let the cookie have the same expiration as the ticket
        cookie.Value = FormsAuthentication.Encrypt(newticket);
        cookie.Expires = newticket.Expiration.AddMinutes(10);

        try
        {
            this.Context.Response.Cookies.Set(cookie);
        }
        catch (HttpException e)
        {

        }
    }

}