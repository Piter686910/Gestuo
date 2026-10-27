using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace GestUO.Classi
{
    public class Patrimonio
    {

        public class ImmobiliHandler : IHttpHandler
        {
            private const string CacheKeyImmobili = "ElencoImmobiliNapoli";

            public void ProcessRequest(HttpContext context)
            {
                context.Response.ContentType = "application/json";
                string termine = context.Request.QueryString["term"];

                if (string.IsNullOrEmpty(termine) || termine.Length < 3)
                {
                    context.Response.Write("[]");
                    return;
                }

                try
                {
                    List<ImmobileNapoli> immobili;

                    // 1. Gestione della Cache per evitare di riscaricare il CSV ad ogni digitazione
                    if (context.Cache[CacheKeyImmobili] != null)
                    {
                        immobili = (List<ImmobileNapoli>)context.Cache[CacheKeyImmobili];
                    }
                    else
                    {
                        immobili = ScaricaCsvImmobili();
                        // Salva in memoria per 24 ore
                        context.Cache.Insert(CacheKeyImmobili, immobili, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
                    }

                    // 2. Filtriamo i dati in base al testo digitato dall'utente
                    // Cerchiamo corrispondenze nell'indirizzo, nella descrizione o nel quartiere
                    var risultati = immobili
                        .Where(i => (i.Localizzazione != null && i.Localizzazione.IndexOf(termine, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                    (i.Descrizione != null && i.Descrizione.IndexOf(termine, StringComparison.OrdinalIgnoreCase) >= 0))
                        .Select(i => $"{i.Localizzazione} - {i.Descrizione}".Trim(' ', '-'))
                        .Distinct()
                        .Take(20) // Limitiamo a 20 suggerimenti per non intasare il menu
                        .ToList();

                    context.Response.Write(JsonConvert.SerializeObject(risultati));
                }
                catch (Exception ex)
                {
                    // In caso di errore restituisce un array vuoto sicuro
                    context.Response.Write("[]");
                }
            }

            private List<ImmobileNapoli> ScaricaCsvImmobili()
            {
                // Il nuovo URL del dataset del patrimonio immobiliare che hai richiesto
                string url = "https://dati.comune.napoli.it/dataset/7bc313c9-335b-42fc-82fa-456359eb24b1/resource/c1e6e7cd-593a-4e6b-8e70-fb937a0efb80/download/elenco_immobili_rispetto__d.lgs._n.33_del_14_03_2013_.csv";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "WebForms-Immobili-App");
                    var response = client.GetStreamAsync(url).Result;

                    // Il file usa la codifica Windows-1252 per i caratteri accentati e i punti e virgola (;) come separatore
                    using (var reader = new StreamReader(response, System.Text.Encoding.GetEncoding("windows-1252")))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";",
                        PrepareHeaderForMatch = args => args.Header.ToUpper().Trim(),
                        MissingFieldFound = null,
                        HeaderValidated = null // Ignora discrepanze minori nell'header
                    }))
                    {
                        return csv.GetRecords<ImmobileNapoli>().ToList();
                    }
                }
            }

            public bool IsReusable => false;
        }

        // Classe Modello mappata sui campi reali del nuovo CSV
        public class ImmobileNapoli
        {
            [CsvHelper.Configuration.Attributes.Name("DESCRIZIONE IMMOBILE")]
            public string Descrizione { get; set; }

            [CsvHelper.Configuration.Attributes.Name("LOCALIZZAZIONE")]
            public string Localizzazione { get; set; }
        }


        public class UnitaImmobiliare
        {
            [Name("EDIFICIO")]
            public string Edificio { get; set; }

            [Name("CODICE UNITA")]
            public string CodiceUnita { get; set; }

            [Name("DENOMINAZIONE")]
            public string Denominazione { get; set; }

            [Name("TIPOLOGIA UNITA'")]
            public string TipologiaUnita { get; set; }

            [Name("INDIRIZZO")]
            public string Indirizzo { get; set; }

            [Name("CIVICO")]
            public string Civico { get; set; }

            [Name("QUARTIERE")]
            public string Quartiere { get; set; }

            [Name("SCALA")]
            public string Scala { get; set; }

            [Name("PIANO")]
            public string Piano { get; set; }

            [Name("INTERNO")]
            public string Interno { get; set; }

            // Dati Catastali
            [Name("SEZIONE")]
            public string Sezione { get; set; }

            [Name("FOGLIO")]
            public string Foglio { get; set; }

            [Name("P.LLA/E")]
            public string Particella { get; set; }

            [Name("SUB")]
            public string Sub { get; set; }
        }


    }
}