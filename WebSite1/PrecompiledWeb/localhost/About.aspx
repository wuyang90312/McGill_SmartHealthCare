<%@ page async="true" title="Patient Data" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" inherits="About, App_Web_plpkowyz" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>:</h1>
        <h2>&nbsp;Vital Signs</h2>
        </hgroup>

    <article>
        <div style="height:300px; width: 700px; overflow:auto">
        <p>        
            <asp:Table ID="Table1" runat="server"  style="border:thin; border-color:gray">
            </asp:Table>
        </p>
        </div>
        <asp:Button ID="Button3" style="width: 40px;"  runat="server" Height="30px" OnClick="Button3_Click" Text="<" />
        <asp:Button ID="Button4" style="width: 40px;"  runat="server" Height="30px" OnClick="Button4_Click" Text=">" />
    </article>
        <asp:Button ID="Button1" style="position: relative; left: 140px; top:10px; width: 85px;"  runat="server" Height="29px" OnClick="Button1_Click" Text="Refresh" />
        <asp:Button ID="Button2" style="position: relative; left: -45px; top:10px; width: 85px;"  runat="server" Height="29px" OnClick="Button2_Click" Text="Reselect" />
   
    <aside>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About.aspx">Patient Data</a></li>
            <li><a runat="server" href="~/Contact.aspx">Contact</a></li>
        </ul>
    </aside>
    
    </asp:Content>