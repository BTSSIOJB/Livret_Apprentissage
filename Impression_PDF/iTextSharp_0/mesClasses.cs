using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classeMétier
{
    public class Cmatiere
    {
        public int num;
        public string designation;
        public List<Citem> ocollItem;
        public static int nbtotal = 0;
        public Cmatiere()
        {
            ocollItem = new List<Citem>();

        }

        public static int augmentenbTotalMat()
        {

            return nbtotal++;

        }

    }

    public class Cprof
    {
        public int num;
        public string nom;
        public string prenom;
        public List<Cmatiere> lesMatieres;
        public static int nbtotal = 0;
        public Cprof()
        {
            lesMatieres = new List<Cmatiere>();

        }

        public static int augmentenbTotalProf()
        {

            return nbtotal++;
            
        }

    }

    public class Citem
    {
        public DateTime date;
        public string plage_horaire;
        public string resume;
    
    }

    public class Cprofs
    {
        private static Cprofs _instance = null;
        public List<Cprof> ocollProfs = null;

        private Cprofs()
        {
            ocollProfs = new List<Cprof>();
        }

        public static Cprofs getInstance()
        {
            if (_instance == null)
            {

                _instance = new Cprofs();
                return _instance;
            }
            else
            {
                return _instance;
            }
            

        }

    }

    public class Cmatieres
    {
        private static Cmatieres _instance = null;
        public List<Cmatiere> ocollMatieres = null;

        private Cmatieres()
        {
            ocollMatieres = new List<Cmatiere>();
        }

        public static  Cmatieres getInstance()
        {
            if (_instance == null)
            {

                _instance = new Cmatieres();
                return _instance;
            }
            else
            {
                return _instance;
            }
            

        }

    }
}
