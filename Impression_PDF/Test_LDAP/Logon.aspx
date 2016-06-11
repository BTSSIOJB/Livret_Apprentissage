﻿<%@ Page language="c#" AutoEventWireup="true" %>
<%@ Import Namespace="FormsAuth" %>
<script runat="server">

    protected void Login_Click(object sender, EventArgs e)
    {
        string adPath = "LDAP://192.168.0.2";

        LdapAuthentication adAuth = new LdapAuthentication(adPath);
        
        try
        {
            if (true == adAuth.IsAuthenticated(txtDomain.Text, txtUsername.Text, txtPassword.Text))
            {
                string groups = adAuth.GetGroups();


                //Create the ticket, and add the groups.
                bool isCookiePersistent = chkPersist.Checked;
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                          txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);
                
                
                // string groupTicket = authTicket.UserData; //test 
                
                
                //Encrypt the ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                
                //Create a cookie, and then add the encrypted ticket to the cookie as data.
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                // authCookie.Name = "tuteur"; //test : permet de nommer le cookie qui me permettra 
                
                if (true == isCookiePersistent)
                    authCookie.Expires = authTicket.Expiration;

                //Add the cookie to the outgoing cookies collection.
                Response.Cookies.Add(authCookie);

                //You can redirect now.
                Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
            }
            else
            {
                errorLabel.Text = "Authentication did not succeed. Check user name and password.";
            }
        }
        catch (Exception ex)
        {
            errorLabel.Text = "Error authenticating. " + ex.Message;
        }
    } 
</script>

<html>
  <body>
    <form id="Login" method="post" runat="server">
      <asp:Label ID="Label1" Runat=server >Domain:</asp:Label>
      <asp:TextBox ID="txtDomain" Runat=server ></asp:TextBox><br>    
      <asp:Label ID="Label2" Runat=server >Username:</asp:Label>
      <asp:TextBox ID=txtUsername Runat=server ></asp:TextBox><br>
      <asp:Label ID="Label3" Runat=server >Password:</asp:Label>
      <asp:TextBox ID="txtPassword" Runat=server TextMode=Password></asp:TextBox><br>
      <asp:Button ID="btnLogin" Runat=server Text="Login" OnClick="Login_Click"></asp:Button><br>
      <asp:Label ID="errorLabel" Runat=server ForeColor=#ff3300></asp:Label><br>
      <asp:CheckBox ID=chkPersist Runat=server Text="Persist Cookie" />
    </form>
  </body>
</html>