<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vueCahierTexteParApprenti.aspx.cs" Inherits="web_livret_apprentissage_1.vueCahierTexteParApprenti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    
 <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Aperçu du cahier de texte professeurs par l'apprenti</a></li>
	</ul>
     <div id="tabs-1">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
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
