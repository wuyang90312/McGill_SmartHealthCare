using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Parse;

public partial class department : System.Web.UI.Page
{
    private DropDownList departmentD;
    protected string patientID_List="";
    protected void Page_Load(object sender, EventArgs e)
    {
        getList(User.Identity.Name);
    }
    protected async void getList(string username)
    {
        var query = ParseObject.GetQuery("Department").WhereEqualTo("username", username);
        IEnumerable<ParseObject> result = await query.FindAsync();

        //generate a dynamic radio button table
        departmentD = new DropDownList();
        departmentD.Items.Add("Select...");
        departmentD.Font.Size = 14;
        departmentD.Font.Bold = true;
        departmentD.AutoPostBack = true;
        foreach (ParseObject s in result)
        {
            departmentD.Items.Add(s.Get<string>("department"));
            departmentD.SelectedIndexChanged += new System.EventHandler(department_CheckedChanged);   
        }

        string tmp = Request.QueryString["depart"];
        if (tmp != null)
        {
            departmentD.SelectedIndex = departmentD.Items.IndexOf(departmentD.Items.FindByText(tmp));
        }
        this.Label0.Controls.Add(departmentD);
        
    }

    protected void department_CheckedChanged(object sender, EventArgs e)
    {
         //refresh the page to generate buttons corresponding to the selected department
        string depart = ((DropDownList) sender).SelectedValue;
        
        string URL = "~/Department.aspx?depart=" + depart;
        // redirect to the current page
        Response.Redirect(URL,false);
    }
    protected override async void OnInit(EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            Button button;
            button = new Button();
            button.Text = "Log in";
            button.ID = "Log in";
            button.CausesValidation = false;
            button.Click += new EventHandler(Log_in);
            this.Label0.Controls.Add(button);
        }
        else
        {
            bool haveButton = false;
            // only if ID contains words, buttons will be generated
            string tmp = Request.QueryString["depart"];
            if (tmp != null)
            {
                //prove the existance of the authentication
                var query = ParseObject.GetQuery("Department").WhereEqualTo("username", User.Identity.Name);
                IEnumerable<ParseObject> result = await query.FindAsync();
                foreach (ParseObject s in result)
                {
                    if (tmp == s.Get<string>("department"))
                    {
                        haveButton = true;
                        break;
                    }
                }
            }

            if (haveButton)
            {
                var query = ParseObject.GetQuery("Patients").WhereEqualTo("department", tmp).OrderBy("iD");
                IEnumerable<ParseObject> result = await query.FindAsync();

                // Generate a table
                Table table = new Table();
                table.BackColor = System.Drawing.Color.Beige;

                var row = new TableRow();
                addnewCell(row, "Patient ID");
                addnewCell(row, "Name");
                addnewCell(row, "Birthday");
                addnewCell(row, "Gender");
                table.Rows.Add(row);
                //Generate buttons for patient
                Button button;
                int index = 0;
                foreach (ParseObject s in result)
                {
                    button = new Button();
                    string tmpe = s.Get<string>("iD");
                    button.Text = tmpe;
                    //store ID into patientID_list
                    patientID_List += "," + tmpe;
                    button.ID = ""+index;
                    button.CausesValidation = false;
                    button.Click += new EventHandler(button_click);
                    //this.Label1.Controls.Add(button);

                    var row1 = new TableRow();
                    var cell = new TableCell();
                    setCellBorder(cell);
                    cell.Controls.Add(button);
                    row1.Cells.Add(cell);
                    addnewCell(row1, s.Get<string>("name"));
                    addnewCell(row1, s.Get<string>("birthday"));
                    addnewCell(row1, s.Get<string>("gender"));
                    table.Rows.Add(row1);

                    index++;
                }
                Label1.Controls.Add(table);
            }
        }
        
        base.OnInit(e);
    }
    protected void addnewCell(TableRow row, string content)
    {
        var cell = new TableCell();
        setCellBorder(cell);
        cell.Text = content;
        row.Cells.Add(cell);
    }
    protected void setCellBorder(TableCell cell)
    {
        cell.BorderWidth = 1;
        cell.BorderColor = System.Drawing.Color.Gray;
        cell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Dotted;
    }
    protected void Log_in(object sender, EventArgs e)
    {
        Response.Redirect("~/",false);
    }
    protected void button_click(object sender, EventArgs e)
    {
        // decrypt the cookie and get all the data stored in it
        HttpCookie cookie = FormsAuthentication.GetAuthCookie(User.Identity.Name, true);
        var ticket = FormsAuthentication.Decrypt(cookie.Value);
        // Store the patient ID into the ticket now
        // in sync with the web.config
        string userData = ((Button) sender).ID+patientID_List;
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

        // Go to the patient data page
        Response.Redirect("~/About.aspx", false); 
    }
}