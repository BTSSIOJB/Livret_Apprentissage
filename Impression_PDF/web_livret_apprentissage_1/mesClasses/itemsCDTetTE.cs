using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_livret_apprentissage_1.mesClasses
{
    public class CitemCDT
    {
        private string _numMatiere;

        public string NumMatiere
        {
            get { return _numMatiere; }
            set { _numMatiere = value; }
        }
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        private string _heure_debut;

        public string Heure_debut
        {
            get { return _heure_debut; }
            set { _heure_debut = value; }
        }
        private string _heure_fin;

        public string Heure_fin
        {
            get { return _heure_fin; }
            set { _heure_fin = value; }
        }
        private string resume_cours;

        public string Resume_cours
        {
            get { return resume_cours; }
            set { resume_cours = value; }
        }
    }

    public class CitemTE
    {
        private string _numEtudiant;

        public string NumEtudiant
        {
            get { return _numEtudiant; }
            set { _numEtudiant = value; }
        }
        private DateTime _dateDebut;

        public DateTime DateDebut
        {
            get { return _dateDebut; }
            set { _dateDebut = value; }
        }
        private DateTime _dateFin;

        public DateTime DateFin
        {
            get { return _dateFin; }
            set { _dateFin = value; }
        }
        private string travail_realise;

        public string Travail_realise
        {
            get { return travail_realise; }
            set { travail_realise = value; }
        }



    }
}