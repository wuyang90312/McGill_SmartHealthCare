<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <section class="contact">
        <header>
            <h3>Phone:</h3>
        </header>
        <p>
            <span class="label">Main:</span>
            <span>514-398-7151</span>
        </p>
    </section>

    <section class="contact">
        <header>
            <h3>Email:</h3>
        </header>
        <p>
            <span class="label">Support:</span>
            <span><a href="mailto:yang.wu2@mail.mcgill.com">yang.wu2@mail.mcgill.com</a></span>
        </p>
        <p>
            <span class="label">General:</span>
            <span><a href="mailto:quang.ho@mcgill.ca">quang.ho@mcgill.ca</a></span>
        </p>
    </section>

    <section class="contact">
        <header>
            <h3>Address:</h3>
        </header>
        <p>
            3480 rue University<br />
            Montreal Quebec Canada<br />
            H3A 0E9
        </p>
    </section>
</asp:Content>