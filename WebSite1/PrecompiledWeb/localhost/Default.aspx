<%@ page async="true" title="Home Page" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" inherits="_Default, App_Web_plpkowyz" enableeventvalidation="false" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
               <!-- <h1><%: Title %>.</h1>-->
                <h2>To have an access to patients' vital signs at anytime from anywhere!</h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <!-- <h3>Project Description:</h3>
    <ol class="round">
        <li class="one">
            <h5>Terminal</h5>
            Iphone is chosen as a personal portable terminal which can query data from medical device and update onto online data server.
            All other smart phones can also be developed as platforms.
            <a href="http://go.microsoft.com/fwlink/?LinkId=245146">Learn more…</a>
        </li>
        <li class="two">
            <h5>Online Database Server</h5>
            Here, a free online database server PARSE is the database hosting server.
            <a href="http://parse.com/docs/">Learn more…</a>
        </li>
        <li class="three">
            <h5>Web Hosting</h5>
            The patient information can also be accessed via website. Therefore, a website is generated on the server
            <a href="https://somee.com/default.aspx">SOMEE</a>.
            <a href="https://somee.com/Support.aspx">Learn more…</a>
        </li>
    </ol> -->
    <section>
        <h2>Log into uVS</h2>
        <asp:Login ID="Login1" runat="server" ViewStateMode="Disabled" RenderOuterTable="false">
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <ol>
                        <li>
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="UserName">User name</asp:Label>
                            <asp:TextBox runat="server" ID="UserName" ></asp:TextBox> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="The user name field is required." />
                        </li>
                        <li>
                            <asp:Label ID="Label4" runat="server" AssociatedControlID="Password">Password&nbsp&nbsp</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" ></asp:TextBox> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="The password field is required." /> 
                        </li>
                        <li>
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label ID="Label5" runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
                        </li>
                    </ol>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Log in" />
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
    </section>
       <section id="sign">
        <asp:Label ID="Label6"  style="position:relative; bottom:150px; left: 400px" runat="server" Text=""></asp:Label>
    </section>
</asp:Content>