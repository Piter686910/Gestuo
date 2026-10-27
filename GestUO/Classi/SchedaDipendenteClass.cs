using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public class SchedaDipendenteClass
    {


        public string Matricola_ced { get; set; }
        public string Matricola { get; set; }
        public string Nominativo { get; set; }
        public string Ufficio { get; set; }
        public bool IsAutista { get; set; }
        public int Quartina { get; set; }
        public string Grado { get; set; } 
        public DateTime dataAssunzione { get; set; }
        public DateTime dataSorveglianza { get; set; }
        public string MacroArea { get; set; }
        public string Area { get; set; }
        public string GruppoQuartina { get; set; }
        public string GruppoReperibilita { get; set; }
        public string CategoriaEconomica { get; set; }
        public string TurnoPref { get; set; }
        public bool Armato { get; set; }
        public bool limitazione { get; set; }
        public bool l53 { get; set; }
        public bool l104 { get; set; }
        public bool abilitato { get; set; }

    }
}