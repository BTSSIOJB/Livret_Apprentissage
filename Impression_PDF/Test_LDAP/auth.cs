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
        private DirectoryEntry _entry = null; //pour les deux méthodes
        public LdapAuthentication(string path)
        {
            _path = path; // path url ldap reçue
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            Boolean reponse;
            
           
                string domainAndUsername = @"btsljb.fr@PROFS\" + username; //@PROFS correspond à l'OU dans laquelle se trouve les compte professeur
                _entry = new DirectoryEntry(_path, domainAndUsername, pwd, AuthenticationTypes.Secure);
                DirectorySearcher search = new DirectorySearcher(_entry);
           
            
            
            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = _entry.NativeObject;

                search = new DirectorySearcher(_entry);
                
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();



                if (null == result)  //_path = "LDAP://192.168.0.2/CN=Philippe Pilla,OU=Profs-IG,OU=PROFS,DC=BTSLJB,DC=FR"
                {
                    reponse = false;
                    return reponse;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0]; // pour pillap retourne "Philippe Pilla"
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }


            reponse = true;
            return reponse;
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
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }
    }
}