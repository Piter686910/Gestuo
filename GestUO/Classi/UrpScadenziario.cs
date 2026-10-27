using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public class UrpScadenziario
    {

        public String nr_carico = string.Empty;
        public Int32 anno = 0;
        public String nr_pratica = string.Empty;
        public String richiedente = string.Empty;
        public String protGen = string.Empty;
        public DateTime dataArrivo;
        public DateTime dataScadenza;
        public Boolean controInteressati;
        public String esito = string.Empty;
        public String motivazione = string.Empty;
        public String protUscita = string.Empty;
        public DateTime dataUscita;
        public Boolean ric24190;
        public Boolean ric3313;

    }
}