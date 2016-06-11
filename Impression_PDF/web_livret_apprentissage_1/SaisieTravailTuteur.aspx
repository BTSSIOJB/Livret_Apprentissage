<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SaisieTravailTuteur.aspx.cs" Inherits="web_livret_apprentissage_1.Tuteur" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
          $(function () {
              $("#MainContent_tbDebutPeriode").datepicker({
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

          $(function () {
              $("#MainContent_tbFinPeriode").datepicker({
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

          /* $(function () {
              $("#MainContent_ddlChoixEtudiant").selectmenu();
          });*/


          $(function () {
              $("#tabs").tabs();
          });

         
          function compareDate()
          {
              var sdate1 = document.getElementById('MainContent_tbDebutPeriode').value
              var date1 = new Date();
              date1.setFullYear(sdate1.substr(6,4));
              date1.setMonth(sdate1.substr(3,2));
              date1.setDate(sdate1.substr(0,2));
              date1.setHours(0);
              date1.setMinutes(0);
              date1.setSeconds(0);
              date1.setMilliseconds(0);
              var d1=date1.getTime()
 
              var sdate2 = document.getElementById('MainContent_tbFinPeriode').value
              var date2 = new Date();
              date2.setFullYear(sdate2.substr(6,4));
              date2.setMonth(sdate2.substr(3,2));
              date2.setDate(sdate2.substr(0,2));
              date2.setHours(0);
              date2.setMinutes(0);
              date2.setSeconds(0);
              date2.setMilliseconds(0);
              var d2=date2.getTime()
 
              //si la date d'arrviée et superieur a la date de depart en afficher un message d'erreur
              if(d1>d2)
              {  
                  /*alert('La date de début de période ne peut pas être supérieure à la date de fin de période !')*/
                  document.getElementById('fenetre_alert').innerHTML = 'La date de début de période ne peut pas être supérieure à la date de fin de période. Merci de corriger.';
                  $(function () {
                      $("#fenetre_alert").dialog({
                          autoOpen: false,
                          width: 400,
                          buttons: [
                              {
                                  text: "Ok",
                                  click: function () {
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

             
              
 
          }
  </script>
    <div id="fenetre_alert" title="Le formulaire n'a pas été envoyé !">
    </div>
    <div id="tabs">
	<ul>
		<li><a href="#tabs-1">Saisie du travail réalisé en entreprise</a></li>
		<li><a href="#tabs-2">Téléchargement de documents</a></li>
	</ul>
        <div id="tabs-1">
    <p style="text-align:center">&nbsp;</p>
    <p>
        <asp:Label ID="lbchoisirEtudiant" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Choisir un apprenti :"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlChoixEtudiant" runat="server" AutoPostBack="true" OnTextChanged="trouverTuteurApprenti" ViewStateMode="Enabled">
        </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvMatiere" runat="server" ControlToValidate="ddlChoixEtudiant" ErrorMessage="Vous devez Choisir un étudiant !" ForeColor="Red"></asp:RequiredFieldValidator>
    </p>
    <p>
        <asp:Label ID="lbNomTuteur" runat="server" Font-Bold="True" ForeColor="#FF9933"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
    </p>
            <div>
            <br />
    </div>
    
        <div>
     <p>  
         <asp:Label ID="lbdebutPeriode" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Date début de période :"></asp:Label>
&nbsp;<asp:TextBox ID="tbDebutPeriode" runat="server" AutoPostBack="True" OnTextChanged="enabledTbFinPeriode"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
         <asp:RequiredFieldValidator ID="rfvDateDebut" runat="server" ControlToValidate="tbDebutPeriode" ErrorMessage="Vous devez saisir une date de début !" ForeColor="Red"></asp:RequiredFieldValidator>
            </p>

        </div>
    <p>
        <asp:Label ID="lbfinPeriode" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Date de fin de période :"></asp:Label>
&nbsp;<asp:TextBox ID="tbFinPeriode" runat="server" AutoPostBack="True" OnTextChanged="rappelSaisieAnte"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
        <asp:RequiredFieldValidator ID="rfvDateFin" runat="server" ControlToValidate="tbFinPeriode" ErrorMessage="Vous devez saisir une date de fin !" ForeColor="Red"></asp:RequiredFieldValidator>
            </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        <asp:Label ID="lbtravail" runat="server" Font-Bold="True" ForeColor="#654B24" Text="Travail réalisé :"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </p>
    <div class="divtbresume">
    <p>
        <asp:TextBox ID="tbResume" runat="server" Height="250px" TextMode="MultiLine" BorderStyle="Solid" BorderColor="#FF9933" ToolTip="Saisir votre texte. Si vous validez avec des erreurs, vous pouvez corriger en recommençant votre saisie pour la même période , la nouvelle saisie viendra remplacer la première. Si vous voulez annuler une saisie validée, vous ressaisissez pour la même période en laissant la zone de texte vierge, le système annulera alors votre saisie précédente."></asp:TextBox> <asp:Image ID="ImgAide" runat="server" Height="30px" ImageUrl="~/images/help-icon.png" ToolTip="Saisir votre texte. Si vous validez avec des erreurs, vous pouvez corriger en recommençant votre saisie pour la même période , la nouvelle saisie viendra remplacer la première. Si vous voulez annuler une saisie validée, vous ressaisissez pour la même période en laissant la zone de texte vierge, le système annulera alors votre saisie précédente." /> 
    </p>
    </div>
    <p>
        &nbsp;</p>
    <p>
        <asp:Button ID="btnValidation" runat="server"  Text="Validation travail de la période" OnClick="btnValidation_Click" BackColor="#654B24" ForeColor="White" OnClientClick="return(compareDate())" BorderStyle="Solid" Height="39px" ToolTip="Enregistrement de la saisie." Font-Bold="True" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbValidationSaisie" runat="server" ForeColor="#009900"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;
        
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
     </div>
     <div id="tabs-2">
         <!--Dans cette DIV les documents à télécharger -->
     </div>
</div>
</asp:Content>
