<%@ Page language="c#" AutoEventWireup="true" %>
<%@ Import Namespace="System.Security.Principal" %>
<html>
  <body>
    <form id="Form1" method="post" runat="server">
      <asp:Label ID="lblName" Runat=server /><br>
      <asp:Label ID="lblAuthType" Runat=server />
    </form>
  </body>
</html>
<script runat=server>
void Page_Load(object sender, EventArgs e)
{

    if (Context.User.IsInRole("ggtuteurs")) //la variable context est definie dans le fichier Global.asax.
    {
       // Response.Redirect("~/Logon.aspx?ReturnUrl=%2fWebForm1.aspx"); //redemande Logon mais avec une redirection vers webform1 si authentifié
    }
    
  lblName.Text = "Hello " + Context.User.Identity.Name + "."; //affiche pillap
  lblAuthType.Text = "You were authenticated using " + Context.User.Identity.AuthenticationType + "."; //affiche le nom de lalsse d'authentification
}
</script>