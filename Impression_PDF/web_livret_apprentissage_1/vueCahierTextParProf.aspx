<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vueCahierTextParProf.aspx.cs" Inherits="web_livret_apprentissage_1.vueCahierTextParProf" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    
 <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Aperçu de votre (vos) cahier(s) de texte </a></li>
	</ul>
          
        <div id="tabs-1">
            <br />
            <asp:Table ID="tblcahierText" runat="server" CssClass="tableau">
            </asp:Table>
            <br />
            <!--<asp:PlaceHolder ID="PlaceHolder1" runat="server">


            </asp:PlaceHolder> -->

            <asp:Label ID="lbException" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>

             <br />
             <br />  
        </div>
            
</div>  

</asp:Content>
