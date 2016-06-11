using System;
using System.Text;
using System.Collections;
using System.Web.Security;

using System.Security.Principal;
using System.DirectoryServices;


namespace FormsAuth
{
    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;

        public string FilterAttribute
        {
            get { return _filterAttribute; }
            //set { _filterAttribute = value; }
        }

        private DirectoryEntry _entry = null; //pour les deux méthodes
        public LdapAuthentication(string path)
        {
            _path = path; // path url ldap reçue
        }

        public bool IsAuthenticated(string username, string pwd)
        {
                
                string domainAndUsername = "btsljb.fr\\" + username; // ou @"btsljb.fr\" ou @"btsljb.fr@PROFS\" car va chercher dans toutes les OU 
                _entry = new DirectoryEntry(_path, domainAndUsername, pwd, AuthenticationTypes.Secure);
                DirectorySearcher search; // = new DirectorySearcher(_entry);
           
                        
            try
            {
                //Bind to the native AdsObject to force authentication.
                //object obj = _entry.NativeObject;

                search = new DirectorySearcher(_entry); //lève une exception si login incorrect
                
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();


                //_path = "LDAP://192.168.0.2/CN=Philippe Pilla,OU=Profs-IG,OU=PROFS,DC=BTSLJB,DC=FR"
             
                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0]; // pour pillap retourne "Philippe Pilla"
            }
            catch
            {
                return false; //En cas d'exception le catch renvoie False car la fonction est true/false
                //throw new Exception("Erreur d'authentification : " + "mot de passe et/ou nom d'utilisateur incorrect !");

            }


            
            return true;
        }

        

        public string GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_entry);  // entry à la place de "_path" pour avoir les permissions 
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();
            

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur dans l'obtention du Groupe Utilisateur. " + ex.Message + " !");
            }
            return groupNames.ToString();
        }
    }
}