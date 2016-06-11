<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inscription.aspx.cs" Inherits="Web_Livret_Apprentissage_1.Inscription" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    
    <div>
        <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Inscription des Professeurs-Tuteurs-Apprentis et attribution des matières aux professeurs et des apprentis aux tuteurs</a></li>		
	</ul>
        <div id="tabs-1">
        <!--<blockquote>-->
            &nbsp;<asp:Label ID="lbChoixType" runat="server" Text="Choisir type d'inscription :" ForeColor="#654B24" Font-Bold="True"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlChoixType" runat="server" AutoPostBack="True" 
        Height="20px" Width="152px" 
    onselectedindexchanged="ddlChoixType_TextChanged">
        <asp:ListItem></asp:ListItem>
        <asp:ListItem>Professeur</asp:ListItem>
        <asp:ListItem>Tuteur</asp:ListItem>
        <asp:ListItem>Etudiant</asp:ListItem>
        <asp:ListItem>Matiere</asp:ListItem>
    </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="rfvChoixType" runat="server" ControlToValidate="ddlChoixType" ErrorMessage="Choisir un type" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
        <!--</blockquote>-->
        <!--<blockquote>-->
            &nbsp;<asp:Label ID="lbChoixMatiere" runat="server" Text="Choisir matière : " 
        Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlChoixMatiere" runat="server" Height="20px" 
        Width="152px" Visible="False" AutoPostBack="True">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="rfvChoixMatiere" runat="server" 
                ErrorMessage="Choisir une matière" 
            ControlToValidate="ddlChoixMatiere" ForeColor="Red"></asp:RequiredFieldValidator>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lbChoixProf" runat="server" Text="Choisir professeur :" 
        Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;
            <asp:DropDownList ID="ddlChoixProf" runat="server" Height="20px" Width="152px" 
        AutoPostBack="True" Visible="False">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="rfvChoixProfesseur" runat="server" 
                ErrorMessage="Choisir un professeur" ControlToValidate="ddlChoixProf" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            &nbsp;<asp:Label ID="lbNomInscription" runat="server" Text="Nom de l'inscrit :" Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tbNomInscription" runat="server" Width="152px" Height="20px" 
        Visible="False"></asp:TextBox>          
        &nbsp;&nbsp;          
        <asp:RequiredFieldValidator ID="rfvNomInscription" runat="server" 
                ErrorMessage="Saisir un nom" 
            ControlToValidate="tbNomInscription" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lbPrenomInscription" runat="server" Text="Prénom de l'inscrit :" 
        Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;
            <asp:TextBox ID="tbPrenomInscription" runat="server" Width="150px" 
        Height="20px" Visible="False"></asp:TextBox>         
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;         
        <asp:RequiredFieldValidator ID="rfvPrenomInscription" runat="server" 
                ErrorMessage="Saisir un prénom" 
            ControlToValidate="tbNomInscription" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            &nbsp;<asp:Label ID="lbClasseEtudiant" runat="server" Text="Classe de l'étudiant : " 
        Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlChoixClasse" runat="server" AutoPostBack="True" 
                Height="20px" Visible="False" Width="152px">
                <asp:ListItem>sio1-sisr</asp:ListItem>
                <asp:ListItem>sio1-slam</asp:ListItem>
                <asp:ListItem>sio2-sisr</asp:ListItem>
                <asp:ListItem>sio2-slam</asp:ListItem>
            </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvClasseEtudiant" runat="server" 
                ErrorMessage="Saisir une classe" ControlToValidate="ddlChoixClasse" 
            ForeColor="Red"></asp:RequiredFieldValidator>          
            <br />
            <br />
            &nbsp;<asp:Label ID="lbNomTuteur" runat="server" Text="Nom du tuteur : " 
        Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlNomTuteur" runat="server" Height="20px" Width="212px" 
        AutoPostBack="True" Visible="False">
    </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvChoixTuteur" runat="server" 
                ErrorMessage="Choisir un tuteur" ControlToValidate="ddlNomTuteur" 
            ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            &nbsp;<asp:Label ID="lbNomEntreprise" runat="server" Text="Nom de l'entreprise :" 
        Visible="False" ForeColor="#654B24" Font-Bold="True"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tbNomEntreprise" runat="server" Width="150px" 
        Visible="False" Height="20px"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvNomEntreprise" runat="server" 
                ErrorMessage="Saisir le nom de l'entreprise" 
            ControlToValidate="tbNomEntreprise" ForeColor="Red"></asp:RequiredFieldValidator>         
            <br />
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="buttonValiderInscription" runat="server" Text="Valider l'inscription" 
        onclick="buttonValiderInscription_Click" BackColor="#654B24" ForeColor="White" Height="50px" Width="204px" Font-Bold="True" Font-Size="Large" />
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbException" runat="server" ForeColor="Red"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;
            <!--</blockquote>-->
            </div>
            </div>
    </div>
</asp:Content>
