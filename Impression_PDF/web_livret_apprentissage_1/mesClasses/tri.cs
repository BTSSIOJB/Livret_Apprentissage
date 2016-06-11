using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace web_livret_apprentissage_1.mesClasses
{
    public class myReverserClass : IComparer
    {

        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(Object x, Object y)
        {
            int indexUnderscore_x = Convert.ToString(x).IndexOf('_');
            int indexUnderscore_y = Convert.ToString(y).IndexOf('_');
            string souschaine_x = Convert.ToString(x).Substring(indexUnderscore_x + 1, Convert.ToString(x).Length - (indexUnderscore_x + 1));
            string souschaine_y = Convert.ToString(y).Substring(indexUnderscore_y + 1, Convert.ToString(y).Length - (indexUnderscore_y + 1));
            x = (object)souschaine_x;
            y = (object)souschaine_x;
            return ((new CaseInsensitiveComparer()).Compare(y, x));
        }

    }
}