using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Drawing;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using GestUO.Classi;
using static GestUO.Classi.Patrimonio;
using static GestUO.Classi.StradeNapoli;
using Table = iText.Layout.Element.Table;

namespace Uote
{

    public partial class test : System.Web.UI.Page
    {
        private const string CacheKeyImmobili = "ElencoImmobiliCSV";
        // Classe helper per i dati

        //public class DipendenteTurno
        //{
        //    public string Matricola { get; set; }
        //    public string Nominativo { get; set; }
        //    public string Ufficio { get; set; }
        //    public bool IsAutista { get; set; }
        //    public int QuartinaID { get; set; }
        //    public string StringaGiorniQ { get; set; } // Es: "5,12,21"
        //    public string[] TurniMensili { get; set; } // Array [32] (indice 1-31)
        //    public string StatisticaPerc { get; set; }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //txtAnno.Text = System.Convert.ToInt32(DateTime.Now.Year).ToString();
            }
        }
        /// <summary>
        /// stradario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void btnCerca_Click(object sender, EventArgs e)
        {
            string testoCercato = txtStrada.Text.Trim();
            if (string.IsNullOrEmpty(testoCercato)) return;

            try
            {
                lblErrore.Visible = false;

                // ID univoco della risorsa stradario sul portale di Napoli
                string resourceId = "420eb601-0f44-4a89-b5c0-d5987dc14aa6";

                // Costruiamo l'URL passando l'ID risorsa e la query di ricerca del testo
                string url = $"https://dati.comune.napoli.it/api/3/action/datastore_search?resource_id={resourceId}&q={Uri.EscapeDataString(testoCercato)}";

                using (HttpClient client = new HttpClient())
                {
                    // Importante: i server della PA spesso richiedono uno User-Agent compilato
                    client.DefaultRequestHeaders.Add("User-Agent", "ASP.NET-WebForms-App");

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var apiData = Newtonsoft.Json.JsonConvert.DeserializeObject<CkanApiResult<StradaNapoliOnline>>(jsonString);

                        if (apiData != null && apiData.Success)
                        {
                            // Colleghiamo direttamente i record filtrati alla GridView
                            gvRisultati.DataSource = apiData.Result.Records;
                            gvRisultati.DataBind();


                            lblInfo.Text = $"Trovate {apiData.Result.Total} corrispondenze.";
                        }
                    }
                    else
                    {
                        lblErrore.Text = $"Errore API Comune: {response.StatusCode}";
                        lblErrore.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrore.Text = "Errore durante la connessione: " + ex.Message;
                lblErrore.Visible = true;
            }
        }

        protected void gvRisultati_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        // Questo metodo viene invocato direttamente da JavaScript via AJAX
        [System.Web.Services.WebMethod]
        public static List<string> CercaStradeAjax(string termine)
        {
            List<string> suggerimenti = new List<string>();

            if (string.IsNullOrEmpty(termine) || termine.Length < 3)
                return suggerimenti;

            try
            {
                string resourceId = "420eb601-0f44-4a89-b5c0-d5987dc14aa6";
                string url = $"https://dati.comune.napoli.it/api/3/action/datastore_search?resource_id={resourceId}&q={Uri.EscapeDataString(termine)}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "WebForms-Autocomplete-App");

                    // Eseguiamo la chiamata attendendo il risultato in modo sincrono (.Result) perché richiesto dal WebMethod statico
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = response.Content.ReadAsStringAsync().Result;
                        var apiData = JsonConvert.DeserializeObject<CkanApiResult<StradaNapoliOnline>>(jsonString);

                        if (apiData != null && apiData.Success && apiData.Result != null)
                        {
                            // Estraiamo solo i nomi completi delle strade (stringhe) da passare a JavaScript
                            suggerimenti = apiData.Result.Records
                                                    .Select(s => s.NomeCompleto)
                                                    .Distinct()
                                                    .ToList();
                            // TEST DI DEBUG: Stampa nella finestra di Output di Visual Studio
                            System.Diagnostics.Debug.WriteLine($"Lettere cercate: {termine} - Righe trovate: {suggerimenti.Count}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In produzione logga l'errore su un file o sul registro di sistema
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return suggerimenti;
        }
        protected void gvRisultati_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandArgument = e.CommandArgument.ToString();
            // Controlliamo che il comando scatenato sia quello del nostro pulsante
            if (e.CommandName == "SelezionaStrada")
            {
                // Estraiamo il valore memorizzato nel CommandArgument (il "NomeCompleto" della via)
                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');
                string stradaSelezionata = values[0].ToString();

                // Scriviamo il valore nella TextBox in alto
                txtInput.Text = stradaSelezionata;
                TextBox1.Text = values[1].ToString(); 
            }
        }

        // immobili
        protected void gvImmobili_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Scegli")
            {
                // Recuperiamo la stringa passata tramite il CommandArgument
                string immobileScelto = e.CommandArgument.ToString();

                // La inseriamo nella TextBox in alto
                Label1.Text = immobileScelto;
            }
        }
        protected void btnCercaP_Click(object sender, EventArgs e)
        {
            string term = txtFiltro.Text.Trim().ToLower();
            string filePath = Server.MapPath("~/FileComuni/Inventario_Immobiliare_2025.xlsx");

            if (!File.Exists(filePath))
            {
                // lblErrore.Text = "File non trovato nel percorso specificato.";
                return;
            }

            List<UnitaImmobiliare> listaRisultati = new List<UnitaImmobiliare>();

            // Apertura del file Excel con ClosedXML
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // Prende il primo foglio
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Salta l'intestazione
                                                                     // Troviamo automaticamente in che colonna si trovano i dati
                var firstRow = worksheet.FirstRowUsed();
                int colIndirizzo = firstRow.Cells().First(c => c.Value.ToString().Contains("INDIRIZZO")).Address.ColumnNumber;
                int colCodice = firstRow.Cells().First(c => c.Value.ToString().Contains("CODICE UNITA")).Address.ColumnNumber;
                int colDenominazione = firstRow.Cells().First(c => c.Value.ToString().Contains("DENOMINAZIONE")).Address.ColumnNumber;
                int colSezione = firstRow.Cells().First(c => c.Value.ToString().Contains("SEZIONE")).Address.ColumnNumber;
                int colSub = firstRow.Cells().First(c => c.Value.ToString().Contains("SUB")).Address.ColumnNumber;
                int colFoglio = firstRow.Cells().First(c => c.Value.ToString().Contains("FOGLIO")).Address.ColumnNumber;
                int colPart = firstRow.Cells().First(c => c.Value.ToString().Contains("P.LLA/E")).Address.ColumnNumber;
                int colPiano = firstRow.Cells().First(c => c.Value.ToString().Contains("PIANO")).Address.ColumnNumber;
                int colInterno = firstRow.Cells().First(c => c.Value.ToString().Contains("INTERNO")).Address.ColumnNumber;
                int colCivico = firstRow.Cells().First(c => c.Value.ToString().Contains("CIVICO")).Address.ColumnNumber;
                int colQuartiere = firstRow.Cells().First(c => c.Value.ToString().Contains("QUARTIERE")).Address.ColumnNumber;
                

                //foreach (var row in rows)
                //{
                //    // Salta la riga se la prima cella (Edificio) è vuota
                //    if (row.Cell(1).IsEmpty()) continue;

                //    var unita = new UnitaImmobiliare
                //    {
                //        // Usiamo l'indice numerico 1, 2, 3 per sicurezza
                //        // .GetFormattedString() è più sicuro di .GetValue<string>() per i codici numerici
                //        Edificio = row.Cell(1).GetFormattedString().Trim(),
                //        CodiceUnita = row.Cell(2).GetFormattedString().Trim(),
                //        Denominazione = row.Cell(3).GetFormattedString().Trim(),
                //        TipologiaUnita = row.Cell(4).GetFormattedString().Trim(),
                //        Indirizzo = row.Cell(5).GetFormattedString().Trim(),
                //        Civico = row.Cell(6).GetFormattedString().Trim(),
                //        Quartiere = row.Cell(11).GetFormattedString().Trim(),
                //        Scala = row.Cell(13).GetFormattedString().Trim(),
                //        Piano = row.Cell(14).GetFormattedString().Trim(),
                //        Interno = row.Cell(15).GetFormattedString().Trim(),
                //        Sezione = row.Cell(16).GetFormattedString().Trim(),
                //        Foglio = row.Cell(17).GetFormattedString().Trim(),
                //        Particella = row.Cell(18).GetFormattedString().Trim(),
                //        Sub = row.Cell(19).GetFormattedString().Trim()
                //    };

                //    // Filtro di ricerca LINQ-style
                //    if (string.IsNullOrEmpty(term) ||
                //   (unita.Indirizzo ?? "").ToLower().Contains(term) ||
                //   (unita.CodiceUnita ?? "").ToUpper().Contains(term) ||
                //   (unita.Particella ?? "").Contains(term) ||
                //   (unita.Edificio ?? "").ToUpper().Contains(term) ||
                //   (unita.Denominazione ?? "").ToLower().Contains(term))
                //    {
                //        listaRisultati.Add(unita);
                //    }
                //}

                foreach (var row in rows.Skip(1))
                {
                    var unita = new UnitaImmobiliare
                    {
                        Indirizzo = row.Cell(colIndirizzo).GetFormattedString().Trim(),
                        CodiceUnita = row.Cell(colCodice).GetFormattedString().Trim(),
                        Denominazione = row.Cell(colDenominazione).GetFormattedString().Trim(),
                        Sezione = row.Cell(colSezione).GetFormattedString().Trim(),
                        Foglio = row.Cell(colFoglio).GetFormattedString().Trim(),
                        Particella = row.Cell(colPart).GetFormattedString().Trim(),
                        Sub = row.Cell(colSub).GetFormattedString().Trim(),
                        Piano = row.Cell(colPiano).GetFormattedString().Trim(),
                        Interno = row.Cell(colInterno).GetFormattedString().Trim(),
                        Civico = row.Cell(colCivico).GetFormattedString().Trim(),
                        Quartiere = row.Cell(colQuartiere).GetFormattedString().Trim()
                    };

                    if (string.IsNullOrEmpty(term) ||
                        unita.Indirizzo.ToLower().Contains(term) ||
                        unita.CodiceUnita.ToUpper().Contains(term) ||
                        unita.Denominazione.ToLower().Contains(term)
                        )
                    {
                        listaRisultati.Add(unita);
                    }
                }



            }

            // Bind dei dati alla GridView
            GridView1.DataSource = listaRisultati;
            GridView1.DataBind();
        }

       
        protected void gvImmobili_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}