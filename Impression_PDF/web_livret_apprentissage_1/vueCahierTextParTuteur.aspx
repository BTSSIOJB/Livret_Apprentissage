<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vueCahierTextParTuteur.aspx.cs" Inherits="web_livret_apprentissage_1.vueCahierTextParTuteur" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    
 <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Aperçu du cahier de texte professeurs par le tuteur</a></li>
	</ul>
          
        <div id="tabs-1">
            <br />
            &nbsp;
            <asp:Label ID="Label1" runat="server" Text="Choisir un apprenti dans la liste :" Font-Bold="True" ForeColor="#654B24"></asp:Label>
        &nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlChoixEtudiant" runat="server" OnTextChanged="peuplerCDT" AutoPostBack="True" Height="29px"></asp:DropDownList>
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
