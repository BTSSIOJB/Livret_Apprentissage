<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SaisieCahierTextProfesseur.aspx.cs" Inherits="web_livret_apprentissage_1.professeur" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <script type="text/javascript">
          $(function () {
              $("#MainContent_tbDateCours").datepicker({
                  defaultDate: "+1w",
                  dateFormat: "dd/mm/yy",
                  changeMonth: true,
                  changeYear: true,
                  yearRange: "2015:2025",
                  dayNamesMin: ["Di", "Lu", "Ma", "Me", "Je", "Ve", "Sa"],
                  monthNamesShort: ["Janv", "Févr", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Sept", "Oct", "Nov", "Déc"],
                  numberOfMonths: 1,
              });
          });

          /*$(function () {
              $("#MainContent_ddlChoixMatiere").selectmenu();
          }); 

          $(function () {
              $("#MainContent_ddlHeureDebut").selectmenu();
          });

          $(function () {
              $("#MainContent_ddlHeureFin").selectmenu();
          });*/

          $(function () {
              $("#tabs").tabs();
          });

          /*
          function compareHeure() {

              var selectDebut = document.getElementById('MainContent_ddlHeureDebut');
              var heureDebut = selectDebut.options[selectDebut.selectedIndex].text;
           
              var selectFin = document.getElementById('MainContent_ddlHeureFin');
              var heureFin = selectFin.options[selectFin.selectedIndex].text;
              
              var btnValidation = document.getElementById('MainContent_btnValidation');

              if (heureDebut > heureFin && btnValidation.value != 'Nouvelle Saisie ?') {
                  
                  document.getElementById('fenetre_alert').innerHTML = 'L\'heure de début de cours ne peut pas être supérieure à l\'heure de fin de cours. Merci de corriger.';
                  $(function () {
                      $("#fenetre_alert").dialog({
                          autoOpen: false,
                          width: 400,
                          buttons: [
                              {
                                  text: "Ok",
                                  click: function () {
                                      //window.location.reload();
                                      //location.assign(location.href);
                                      btnValidation.onclick = null;
                                      $(this).dialog("close");
                                  }
                              }

                          ]
                      });
                  });

                  $(function () {
                      $("#fenetre_alert").dialog("open");
                  });
                  return false;
              }
              else
              {
                  return true;
              }
          } */

          /*function remplirListe()
          {
              var hd = document.getElementById('heureDeb');
              hd.options[0].value = 'vide';
              hd.options[0].text = " ";
              for(var i = 1 ; i < 12 ; i++)
              {
                  hd.options[i].value = i + 7;
                  hd.options[i].text = i + 7;
              }
              var hf = document.getElementById('heureFin');
              hf.options[0].value = 'vide';
              hf.options[0].text = " ";
              for (var i = 1 ; i < 12 ; i++) {
                  hf.options[i].value = i + 7;
                  hf.options[i].text = i + 7;
              }

          }
         
          window.onload = function () { remplirListe() }; */
         

  </script>
    <div id="fenetre_alert" title="Le formulaire n'a pas été envoyé !">
    </div>
    <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Saisie du Cahier de texte</a></li>
		<li><a href="#tabs-2">Téléchargement documents</a></li>
	</ul>
        <div id="tabs-1">	
    <p style="text-align:center">&nbsp;</p>

    <p>
        <asp:Label ID="lbchoixMatiere" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Choisir matière :&nbsp;"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlChoixMatiere" runat="server" AutoPostBack="True" ViewStateMode="Enabled" OnTextChanged="trouverProfMatiere">
        </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvMatiere" runat="server" ControlToValidate="ddlChoixMatiere" ErrorMessage="Vous devez Choisir une matière !" ForeColor="Red"></asp:RequiredFieldValidator>
    </p>
    <p>
        <asp:Label ID="lbNomProf" runat="server" Font-Bold="True" ForeColor="#FF9933"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
    </p>
    <p>
        <asp:Label ID="lbchoixDate" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Choisir une date en cliquant sur la zone de texte ci-dessous :&nbsp;"></asp:Label>
</p>
        

    <p>
        <asp:TextBox ID="tbDateCours" runat="server" AutoPostBack="True" OnTextChanged="enableDdlHD"></asp:TextBox>
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbDateCours" ErrorMessage="Vous devez choisr une date !" ForeColor="Red"></asp:RequiredFieldValidator>
      </p>
            <p>
                <asp:Label ID="lbhoraire" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Horaire :"></asp:Label>
                &nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlHeureDebut" runat="server" AutoPostBack="True" OnSelectedIndexChanged="enableDdlHF">
        </asp:DropDownList>
    &nbsp;&nbsp; à&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlHeureFin" runat="server" AutoPostBack="True" CausesValidation="True" OnTextChanged="rechercheItemParDateHeure">
        </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvHeureDebut" runat="server" ControlToValidate="ddlHeureDebut" ErrorMessage="Choisir l'heure début !" ForeColor="Red"></asp:RequiredFieldValidator>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvHeureFin" runat="server" ControlToValidate="ddlHeureFin" ErrorMessage="Chosir l'heure de fin !" ForeColor="Red"></asp:RequiredFieldValidator>
      </p>
            <p>
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="L'heure de début doit être inférieure à l'heure de fin !" OnServerValidate="VerifHeure" ForeColor="Red"></asp:CustomValidator>
    </p>
            <p>
                <asp:Label ID="lbreumeCours" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Résumé du cours :"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </p>
        <asp:TextBox ID="tbResume" runat="server" TextMode="MultiLine" BorderStyle="Solid" BorderColor="#FF9933" Height="250px" ToolTip="Saisir votre texte. Si vous validez avec des erreurs, vous pouvez corriger en recommençant votre saisie pour la même période , la nouvelle saisie viendra remplacer la première. Si vous voulez annuler une saisie validée, vous ressaisissez pour la même période en laissant la zone de texte vierge, le système annulera alors votre saisie précédente."></asp:TextBox> <asp:Image ID="Image1" runat="server" Height="30px" ImageUrl="~/images/help-icon.png" ToolTip="Saisir votre texte. Si vous validez avec des erreurs, vous pouvez corriger en recommençant votre saisie pour la même période , la nouvelle saisie viendra remplacer la première. Si vous voulez annuler une saisie validée, vous ressaisissez pour la même période en laissant la zone de texte vierge, le système annulera alors votre saisie précédente." />  
    <div>
    <p>
        <asp:Label ID="lbItemTrouve" runat="server" EnableViewState="False" Font-Italic="True" Font-Names="Calibri Light" Font-Size="Small" ForeColor="#CC3300"></asp:Label>
        </p>
    </div>
    <p>
        &nbsp;</p>
    <p>
        <asp:Button ID="btnValidation" runat="server"  Text="Validation cours du jour" OnClick="btnValidation_Click" BackColor="#654B24" ForeColor="White" Height="38px" Font-Bold="True" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbValidationSaisie" runat="server" ForeColor="#009900"></asp:Label>
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
            <div id="tabs-2">
                </div>
            </div>
        </div>
</asp:Content>
