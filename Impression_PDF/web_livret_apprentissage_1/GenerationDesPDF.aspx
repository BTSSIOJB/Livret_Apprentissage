
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerationDesPDF.aspx.cs" Inherits="web_livret_apprentissage_1.Administration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    
    <div>
        <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Génération des PDF - Cahier de texte - Travail Entreprise - Livret Etudiant</a></li>
		<li><a href="#tabs-2">Utilisation ASP.NET</a></li>
	</ul>
        <div id="tabs-1">
    <p style="text-align:center">&nbsp;</p>
    &nbsp;<br />
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbChoix" runat="server" Text="Génération de livrets PDF par :" Height="26px" Width="241px" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlChoixGeneration" runat="server" OnTextChanged="InitDDLChoixType" AutoPostBack="True" Height="27px" Width="137px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvCritere" runat="server" ControlToValidate="ddlChoixGeneration" ErrorMessage="Choisir un critère !" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:Label ID="lbanneeScolaire" runat="server" Height="26px" Text="Choisir l'année scolaire :" Width="200px" ForeColor="#654B24" Font-Bold="True"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlAnneeScolaire" runat="server" Height="27px" Width="137px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvAnneeScolaire" runat="server" ControlToValidate="ddlAnneeScolaire" ErrorMessage="Choisir une année scolaire !" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <br />
&nbsp;&nbsp;&nbsp; &nbsp;
        <asp:Label ID="lbChoixType" runat="server" Height="26px" Width="200px" ForeColor="#654B24" Font-Bold="True">Choisir le professeur :</asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlChoixType" runat="server" Height="27px" Width="137px" AutoPostBack="True" OnTextChanged="InitDDLChoixMat">
        </asp:DropDownList>
    
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlChoixType" ErrorMessage="Choisir un type !" ForeColor="Red"></asp:RequiredFieldValidator>
    
        <br />
        &nbsp;&nbsp;&nbsp;
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbChoixMatière" runat="server" Text="Choisir sa  matière :" Height="26px" Width="200px" ForeColor="#654B24" Font-Bold="True"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlChoixMatiere" runat="server" Height="27px" Width="137px">
        </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvmatiere" runat="server" ControlToValidate="ddlChoixMatiere" ErrorMessage="Choisir sa matière !" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="btnGenerationPDF" runat="server" OnClick="btnGenerationPDF_Click" Text="Génération du Livret en PDF" BackColor="#654B24" ForeColor="White" />
    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbGenerationPDF" runat="server" Width="530px"></asp:Label>
    </div>
            <div id="tabs-2">
                <a href="Project_Readme.html">Utilisation ASP.NET</a>
            </div>
            
    </div>
    

</div>
</asp:Content>