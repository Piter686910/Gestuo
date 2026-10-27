using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public class DipendenteTurno
    {


        public string Matricola { get; set; }
        public string Nominativo { get; set; }
        public string Ufficio { get; set; }
        public bool IsAutista { get; set; }
        public int QuartinaID { get; set; }
        public string StringaGiorniQ { get; set; } // Es: "5,12,21"
        public string[] TurniMensili { get; set; } // Array [32] (indice 1-31)
        public string StatisticaPerc { get; set; }
        public string Gruppo { get; set; }
        public string Area { get; set; }
    }
}