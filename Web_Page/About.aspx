<%@ Page  Async="true" Title="Patient Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

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
        <asp:Button ID="Button3" style="width: 40px;"  runat="server" Height="34px" OnClick="Button3_Click" Text="<" />
        <asp:Button ID="Button4" style="width: 40px;"  runat="server" Height="34px" OnClick="Button4_Click" Text=">" />
    </article>
        <asp:Button ID="Button1" style="position: relative; left: 140px; top:10px; width: 85px;"  runat="server" Height="32px" OnClick="Button1_Click" Text="Refresh" />
        <asp:Button ID="Button2" style="position: relative; left: -45px; top:10px; width: 85px;"  runat="server" Height="32px" OnClick="Button2_Click" Text="Reselect" />
   <asp:Label ID="Label0" runat="server"></asp:Label>
    <aside>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About.aspx">Patient Data</a></li>
            <li><a runat="server" href="~/Contact.aspx">Contact</a></li>
        </ul>
        <asp:Button ID="Button5" style="position: relative; left: 5px; top:10px; width: 85px;"  runat="server" Height="32px" OnClick="Button5_Click" Text="GRAPH" />
        <asp:image src="" ID="imgUpload" runat="server"  />
        </aside>
    
    </asp:Content>

