<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vueTravailEntrepriseParProf.aspx.cs" Inherits="web_livret_apprentissage_1.vueTravailEntrepriseParProf" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    
    <div>
        <div id="tabs">
	        <ul>
		        <li><a href="#tabs-1">Aperçu du travail réalisé en entreprise par l'apprenti</a></li>
	        </ul>
          <div id="tabs-1">
                <br />
                &nbsp;
                <asp:Label ID="Label1" runat="server" Text="Choisir un étudiant dans la liste :" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddlChoixEtudiant" runat="server" OnTextChanged="peuplerCDT" AutoPostBack="True" Height="29px"></asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <!--<asp:PlaceHolder ID="PlaceHolder1" runat="server">


                </asp:PlaceHolder> -->
                <asp:Table runat="server" ID="tblcahierText" GridLines="Both" CssClass="tableau" CellPadding="1" CellSpacing="1">
 
                </asp:Table>

                <asp:Label ID="lbException" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>

                 <br />
                 <br />   
         </div>
        </div>
     </div> 

</asp:Content>
