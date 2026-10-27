using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public class StradeNapoli
    {

        public class CkanApiResult<T>
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("result")]
            public DatastoreResult<T> Result { get; set; }
        }

        public class DatastoreResult<T>
        {
            [JsonProperty("records")]
            public List<T> Records { get; set; } // Contiene le righe filtrate
            [JsonProperty("total")]
            public int Total { get; set; }       // Totale dei risultati trovati
        }

        // Le colonne reali del CSV del Comune di Napoli mappate dai metadati CKAN
        public class StradaNapoliOnline
        {
            [JsonProperty("TIPOLOGIA")]
            public string Tipologia { get; set; } // Es: VIA, PIAZZA, VICO

            [JsonProperty("TOPONIMO")]
            public string Toponimo { get; set; }  // Es: TOLEDO, GARIBALDI

            [JsonProperty("QUARTIERE")]
            public string Quartiere { get; set; }

            [JsonProperty("MUN")]
            public string Municipalita { get; set; }

            // Proprietà calcolata comoda per la GridView
            public string NomeCompleto => $"{Tipologia} {Toponimo}".Trim();
        }


    }
}