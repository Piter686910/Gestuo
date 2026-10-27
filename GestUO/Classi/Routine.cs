using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;


namespace GestUO.Classi
{
    public class Routine
    {
        public void Reindirizzamento(string msg, string pagchiamante)
        {
            HttpContext.Current.Session["MessaggioErrore"] = msg;
            HttpContext.Current.Session["PaginaChiamante"] = pagchiamante;
            string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
            HttpContext.Current.Response.Redirect(url + msg.Replace("\r\n", " ").ToString());
        }
        public string GetNomeOperatoreByMatr(String user)
        {
            string txt = string.Empty;
            Manager mn = new Manager();
            DataTable operatore = mn.getNominativoOperatore(user);
            if (operatore.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
                    txt = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
            }
            return txt;
        }
        public string GetProtocollo()
        {
            string txt = string.Empty;
            String annoCorr = DateTime.Now.Year.ToString();
            int protocollo = 0;
            Manager mn = new Manager();
            //DataTable tb = mn.MaxNPr(annoCorr);
            DataTable tb = mn.MaxNPr(annoCorr);
            if (tb.Rows.Count > 0)
            {
                //txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
                int annoMAx = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]);

                if (System.Convert.ToInt16(annoCorr) <= annoMAx)
                {
                    protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                    txt = protocollo.ToString();//tb.Rows[0].ItemArray[1].ToString();
                }
                else
                {
                    protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                    txt = protocollo.ToString();

                }
            }
            else
            {
                txt = "1";

            }
            return txt;
        }
        
        /// <summary>
        /// converte true e false in si e no
        /// </summary>
        /// <param name="ws"></param>
        public void ConvertiBooleaniInItaliano(IXLWorksheet ws)
        {
            IXLWorksheet worksheet = ws;
            //sostituisce booleani con stringa si o no
            int lastRow = worksheet.LastRowUsed().RowNumber();
            int lastColumn = worksheet.LastColumnUsed().ColumnNumber();
            for (int row = 2; row <= lastRow; row++)
            {
                // Itera su tutte le colonne nella riga corrente
                for (int column = 1; column <= lastColumn; column++)
                {
                    IXLCell cell = worksheet.Cell(row, column); // Ottieni la cella corrente
                    XLDataType tipoDatoD1 = cell.DataType;
                    string valore = string.Empty;
                    if (tipoDatoD1 is XLDataType.Boolean)
                        valore = System.Convert.ToString(((bool)cell.Value));

                    if (valore == "True")
                    {

                        cell.Value = "SI"; // Sostituisci "true" (stringa) con "si"
                    }
                    else if (valore == "False")
                    {
                        cell.Value = "NO"; // Sostituisci "false" (stringa) con "no"
                    }
                }
            }
        }

        /// <summary>
        /// Sostituisce la data minima 01/01/1900 con stringa vuota
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="e"></param>
        /// <param name="column"></param>
        public void NonVisualizzaDataMinima(GridView griglia, GridViewRowEventArgs e, string column)
        {
            int dataRegistrazioneColumnIndex = -1;
            for (int i = 0; i < griglia.Columns.Count; i++)
            {
                // Cerchiamo il BoundField con il DataField corretto
                if (griglia.Columns[i] is BoundField bf && bf.DataField == column)
                {
                    dataRegistrazioneColumnIndex = i;
                    break;
                }
            }

            // Se la colonna è stata trovata
            if (dataRegistrazioneColumnIndex != -1)
            {
                // Ottieni il valore originale del campo "decr_dataChiusura" dalla riga
                object rawDateValue = DataBinder.Eval(e.Row.DataItem, column);

                // Controlla se il valore è una data e se è 01/01/1900
                if (rawDateValue is DateTime actualDate && actualDate == new DateTime(1900, 1, 1))
                {
                    // Se la data è 01/01/1900, impostiamo il testo della cella su una stringa vuota
                    e.Row.Cells[dataRegistrazioneColumnIndex].Text = "";
                }
            }
        }


        private void stampaX(float x, float y, Document document, Boolean X)
        {
            float boxSize = 10;
            float boxVerticalOffset = 5f;
            // Ottieni il PdfDocument e PdfCanvas
            PdfDocument pdfDocument = document.GetPdfDocument();
            PdfCanvas canvas = new PdfCanvas(pdfDocument.GetFirstPage());

            // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
            float xPosBox = x;
            float yPosBox = y - (boxSize / 2) + boxVerticalOffset;

            // --- Disegna il riquadro ---
            canvas.SetStrokeColor(ColorConstants.BLACK);
            canvas.SetLineWidth(0.8f);
            canvas.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

            // --- Posizione della "X" (centro del riquadro) ---
            float xPosText = xPosBox + (boxSize / 2);
            float yPosText = y;
            if (X == true)
            {


                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                canvas.BeginText()
                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                         .SetColor(ColorConstants.BLACK, true)
                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                         .ShowText("X")
                         .EndText();
            }
        }

        /// <summary>
        /// prepara la stampa del pdf scheda intervento
        /// </summary>
        /// <param name="schede"></param>
        public void CreaPdf(DataTable schede)
        {
            float boxSize = 10;
            //float boxVerticalOffset = 4f;
            float startX_270 = 270;
            float startX_290 = 290;
            float startX_70 = 70;
            float startX_55 = 55;
            float startX_50 = 50;
            float startX_400 = 400;
            float startX_350 = 350;
            float startX_370 = 370;
            float startX_450 = 450;
            float startX_470 = 470;
            float startY_430 = 430;

            float lineHeight = 15f;
            float lineHeight1 = 30f;
            float startY = 630;

            using (MemoryStream stream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(stream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdf))
                        {
                            // --- Creazione del Contenuto del Documento ---

                            // Titolo
                            DateTime dataIntervento = System.Convert.ToDateTime(schede.Rows[0].ItemArray[2].ToString());
                            string dataFormattata = dataIntervento.ToString("dd/MM/yyyy");

                            document.Add(new Paragraph($"Scheda Intervento del: {dataFormattata} , Quartiere:" + schede.Rows[0].ItemArray[56].ToString())
                                .SetFixedPosition(70, 800, 400)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetFontSize(8));

                            // Prima riga: Numero Pratica, Nominativo
                            document.Add(new Paragraph($"Numero Pratica: {schede.Rows[0].ItemArray[1]}").SetFixedPosition(70, 780, 200));
                            document.Add(new Paragraph($"Nominativo: {schede.Rows[0].ItemArray[3]}").SetFixedPosition(250, 780, 500));

                            // Seconda riga: Indirizzo, Data Consegna
                            document.Add(new Paragraph($"Indirizzo: {schede.Rows[0].ItemArray[4]}").SetFixedPosition(70, 760, 800));

                            DateTime dataConsegna = System.Convert.ToDateTime(schede.Rows[0].ItemArray[39].ToString());
                            string dataFormattataConsegna = dataConsegna.ToString("dd/MM/yyyy");

                            document.Add(new Paragraph($"Data Consegna: {dataFormattataConsegna}").SetFixedPosition(70, 740, 800));

                            // Terza riga: Capo pattuglia, pattuglia
                            document.Add(new Paragraph($"Capo Pattuglia: {schede.Rows[0].ItemArray[40]}").SetFixedPosition(70, 720, 200));
                            document.Add(new Paragraph($"Pattuglia: {schede.Rows[0].ItemArray[5]}").SetFixedPosition(70, 700, 600));
                            // Note
                            document.Add(new Paragraph($"Note: {schede.Rows[0].ItemArray[38]}").SetFixedPosition(70, 680, 450));
                            // FONTE INTERVENTO
                            document.Add(new Paragraph("FONTE INTERVENTO").SetFixedPosition(70, 650, 500).SetTextAlignment(TextAlignment.CENTER));
                            //riga interruzione sezione
                            float x = 65;
                            float y = 645;
                            float width = 490;

                            PdfCanvas canvas = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x, y) // Inizia la linea nel punto (x, y)
                                  .LineTo(x + width, y) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // Delega AG
                            bool delegaAG = Convert.ToBoolean(schede.Rows[0].ItemArray[6]);
                            string delegaAGString = delegaAG ? "X" : "";

                            // --- Posizione di riferimento INIZIALE per la X e il Riquadro (lato SINISTRO) ---
                            // float startX_70_DelegaAG = 70; // Posizione X iniziale SPECIFICA per "Delega AG"
                            float startY_DelegaAG = startY; // Use the dynamic startY

                            if (delegaAGString == "X")
                            {
                                stampaX(startX_50, startY_DelegaAG, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Delega AG:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_DelegaAG - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_DelegaAG, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Delega AG:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_DelegaAG, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Resa
                            bool? resaNullable = schede.Rows[0].ItemArray[7] as bool?;
                            string resaString = resaNullable.HasValue && resaNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Resa" ---
                            // float startX_70_Resa = 70; // Use startX_70 for single column
                            float startY_Resa = startY; // Use the dynamic startY


                            if (resaString == "X")
                            {
                                stampaX(startX_50, startY_Resa, document, true);

                                // --- Paragrafo per la descrizione "Resa:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Resa:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Resa - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Resa, document, false);
                                // --- Solo la descrizione "Resa:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Resa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Resa, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Segnalazione
                            bool? segnalazioneNullable = schede.Rows[0].ItemArray[8] as bool?;
                            string segnalazioneString = segnalazioneNullable.HasValue && segnalazioneNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Segnalazione" ---
                            //float startX_70_Segnalazione = 70; // Use startX_70 for single column
                            float startY_Segnalazione = startY; // Use the dynamic startY


                            if (segnalazioneString == "X")
                            {
                                stampaX(startX_50, startY_Segnalazione, document, true);

                                // --- Paragrafo per la descrizione "Segnalazione:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Segnalazione:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Segnalazione - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Segnalazione, document, false);
                                // --- Solo la descrizione "Segnalazione:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Segnalazione:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Segnalazione, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Esposto
                            bool? espostoNullable = schede.Rows[0].ItemArray[9] as bool?;
                            string espostoString = espostoNullable.HasValue && espostoNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Esposto" ---

                            float startY_Esposto = startY; // Use the dynamic startY

                            if (espostoString == "X")
                            {
                                stampaX(startX_50, startY_Esposto, document, true);

                                // --- Paragrafo per la descrizione "Esposto:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Esposto:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Esposto - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Esposto, document, false);
                                // --- Solo la descrizione "Esposto:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Esposto:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Esposto, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Num. Esposto
                            // --- Posizione di riferimento per "Num. Esposto" ---

                            float startY_NumEsposto = startY_430; //


                            document.Add(new Paragraph($"Num. Esposto: {schede.Rows[0].ItemArray[10]}").SetFixedPosition(startX_270, startY_Esposto, 200));

                            startY -= lineHeight; // Move to the next line

                            // Notifica
                            bool? notificaNullable = schede.Rows[0].ItemArray[11] as bool?;
                            string notificaString = notificaNullable.HasValue && notificaNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Notifica" ---
                            float startY_Notifica = startY; // Use the dynamic startY

                            if (notificaString == "X")
                            {
                                stampaX(startX_50, startY_Notifica, document, true);

                                // --- Paragrafo per la descrizione "Notifica:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Notifica - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Notifica, document, false);
                                // --- Solo la descrizione "Notifica:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Notifica, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line


                            // Iniziativa
                            bool? iniziativaNullable = schede.Rows[0].ItemArray[12] as bool?;
                            string iniziativaString = iniziativaNullable.HasValue && iniziativaNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Iniziativa" ---

                            float startY_Iniziativa = startY; // Use the dynamic startY
                            if (iniziativaString == "X")
                            {
                                stampaX(startX_50, startY_Iniziativa, document, true);

                                // --- Paragrafo per la descrizione "Iniziativa:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Iniziativa:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Iniziativa - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Iniziativa, document, false);
                                // --- Solo la descrizione "Iniziativa:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Iniziativa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Iniziativa, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // CDR
                            bool? cdrNullable = schede.Rows[0].ItemArray[13] as bool?;
                            string cdrString = cdrNullable.HasValue && cdrNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "CDR" ---

                            float startY_CDR = startY; // Use the dynamic startY
                            if (cdrString == "X")
                            {
                                stampaX(startX_50, startY_CDR, document, true);

                                // --- Paragrafo per la descrizione "CDR:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("CDR / Capoturno:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_CDR - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_CDR, document, false);
                                // --- Solo la descrizione "CDR:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("CDR / Capoturno:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_CDR, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // notifica non ag
                            bool? notifnoAgNullable = schede.Rows[0].ItemArray[55] as bool?;
                            string notifnoAgString = notifnoAgNullable.HasValue && notifnoAgNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "notifica no Ag" ---

                            float startY_notifnoAg = startY; // Use the dynamic startY
                            if (notifnoAgString == "X")
                            {
                                stampaX(startX_50, startY_notifnoAg, document, true);

                                // --- Paragrafo per la descrizione "startY_notif no Ag:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica No AG:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_notifnoAg - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_notifnoAg, document, false);
                                // --- Solo la descrizione "startY_notif no Ag:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica No AG:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_notifnoAg, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line 470
                            // Accertamenti Richiesti
                            bool? accertamentiRichNullable = schede.Rows[0]["rapp_accRichiesti"] as bool?;
                            string accertamentiRichString = accertamentiRichNullable.HasValue && accertamentiRichNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Coordinatore di turno" ---
                            // float startX_70_Coord = 70; // Use startX_70 for single column
                            float startY_accertamentiRich = startY; // Use the dynamic startY 450
                            if (accertamentiRichString == "X")
                            {
                                stampaX(startX_50, startY_accertamentiRich, document, true);

                                // --- Paragrafo per la descrizione "Coordinatore:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Accertamenti richiesti :");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_accertamentiRich - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_accertamentiRich, document, false);
                                // --- Solo la descrizione "Coordinatore:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Accertamenti richiesti:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_accertamentiRich, 200);
                                document.Add(descriptionParagraph);
                            }

                            // Num. accertamenti richiesti
                            // --- Posizione di riferimento per "accertamenti richiesti" ---

                            float startY_NumaccertamentiRichiesti = startY_430; //


                            document.Add(new Paragraph($"Num. accertam: {schede.Rows[0]["rapp_numAccRichiesti"]}").SetFixedPosition(startX_270, 475, 200));

                            //  startY -= lineHeight; // Move to the next line




                            //// Coordinatore di turno
                            //bool? coordinatorediturnoNullable = schede.Rows[0].ItemArray[14] as bool?;
                            //string coordinatorediturnoString = coordinatorediturnoNullable.HasValue && coordinatorediturnoNullable.Value ? "X" : "";
                            //// --- Posizione di riferimento per "Coordinatore di turno" ---
                            //// float startX_70_Coord = 70; // Use startX_70 for single column
                            //float startY_Coord = startY; // Use the dynamic startY 450
                            //if (coordinatorediturnoString == "X")
                            //{
                            //    stampaX(startX_50, startY_Coord, document, true);

                            //    // --- Paragrafo per la descrizione "Coordinatore:", posizionato *A DESTRA* del riquadro ---
                            //    Paragraph descriptionParagraph = new Paragraph("Coordinatore di turno:");
                            //    descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Coord - 5, 200);
                            //    document.Add(descriptionParagraph);
                            //}
                            //else
                            //{
                            //    stampaX(startX_50, startY_Coord, document, false);
                            //    // --- Solo la descrizione "Coordinatore:", nella posizione originale ---
                            //    Paragraph descriptionParagraph = new Paragraph("Coordinatore di turno:");
                            //    descriptionParagraph.SetFixedPosition(startX_70, startY_Coord, 200);
                            //    document.Add(descriptionParagraph);
                            //}
                            // startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x1 = 65;
                            float y1 = startY;
                            float width1 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas1 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x1, y1) // Inizia la linea nel punto (x, y)
                                  .LineTo(x1 + width1, y1) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // ATTI REDATTI
                            document.Add(new Paragraph("ATTI REDATTI").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));

                            startY -= lineHeight; // Move to the next line
                            // Relazione
                            bool? relazioneNullable = schede.Rows[0].ItemArray[15] as bool?;
                            string relazioneString = relazioneNullable.HasValue && relazioneNullable.Value ? "X" : "";
                            //float startX_70_Relazione = 70; // Use startX_70 for single column
                            float startY_Relazione = startY; // Use the dynamic startY 


                            if (relazioneString == "X")
                            {
                                stampaX(startX_50, startY_Relazione, document, true);


                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Relazione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Relazione - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Relazione, document, false);
                                // --- Solo la descrizione "relazione:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Relazione:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Relazione, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // CNR
                            bool? cnrNullable = schede.Rows[0].ItemArray[16] as bool?;
                            string cnrString = cnrNullable.HasValue && cnrNullable.Value ? "X" : "";
                            float startY_CNR = startY; // Use the dynamic startY
                            if (cnrString == "X")
                            {
                                stampaX(startX_50, startY_CNR, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("CNR:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_CNR - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_CNR, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("CNR:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_CNR, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Annotazione PG
                            bool? annotazionepgNullable = schede.Rows[0].ItemArray[17] as bool?;
                            string annotazionepgString = annotazionepgNullable.HasValue && annotazionepgNullable.Value ? "X" : "";
                            float startY_AnnotazionePG = startY; // Use the dynamic startY
                            if (annotazionepgString == "X")
                            {
                                stampaX(startX_50, startY_AnnotazionePG, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Annotazione PG:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_AnnotazionePG - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_AnnotazionePG, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Annotazione PG:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_AnnotazionePG, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Verbale Sequestro
                            bool? verbalesequestroNullable = schede.Rows[0].ItemArray[18] as bool?;
                            string verbalesequestroString = verbalesequestroNullable.HasValue && verbalesequestroNullable.Value ? "X" : "";
                            float startY_VerbaleSequestro = startY; // Use the dynamic startY
                            if (verbalesequestroString == "X")
                            {
                                stampaX(startX_50, startY_VerbaleSequestro, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale Sequestro:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_VerbaleSequestro - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_VerbaleSequestro, document, false);
                                // --- Solo la descrizione "esito delega:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale Sequestro:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_VerbaleSequestro, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Esito Delega
                            bool? esitodelegaNullable = schede.Rows[0].ItemArray[19] as bool?;
                            string esitodelegaString = esitodelegaNullable.HasValue && esitodelegaNullable.Value ? "X" : "";
                            float startY_EsitoDelega = startY; // Use the dynamic startY
                            if (esitodelegaString == "X")
                            {
                                stampaX(startX_50, startY_EsitoDelega, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Esito Delega:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_EsitoDelega - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_EsitoDelega, document, false);
                                // --- Solo la descrizione "esito Delega :", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Esito Delega:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_EsitoDelega, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Contestazione Amministrativa
                            bool? contestazioneamministrativaNullable = schede.Rows[0].ItemArray[20] as bool?;
                            string contestazioneamministrativaString = contestazioneamministrativaNullable.HasValue && contestazioneamministrativaNullable.Value ? "X" : "";
                            float startY_ContestazioneAmministrativa = startY; // Use the dynamic startY
                            if (contestazioneamministrativaString == "X")
                            {
                                stampaX(startX_50, startY_ContestazioneAmministrativa, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Contestazione Amministrativa:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_ContestazioneAmministrativa - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_ContestazioneAmministrativa, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Contestazione Amministrativa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_ContestazioneAmministrativa, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x2 = 65;
                            float y2 = startY;
                            float width2 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            //PdfCanvas canvas2 = new PdfCanvas(pdf.GetFirstPage());
                            //canvas.MoveTo(x2, y2) // Inizia la linea nel punto (x, y)
                            //      .LineTo(x2 + width2, y2) // Traccia la linea orizzontale fino a (x + width, y)
                            //      .Stroke(); // Applica il tratto per rendere la linea visibile
                            //startY -= lineHeight; // Move to the next line
                            //// PROVVEDIMENTI ADOTTATI E ATTIVITA' SVOLTE
                            //document.Add(new Paragraph("PROVVEDIMENTI ADOTTATI E ATTIVITA' SVOLTE").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                            //startY -= lineHeight; // Move to the next line
                            // Convalida
                            //bool? convalidaNullable = schede.Rows[0].ItemArray[21] as bool?;
                            //string convalidaString = convalidaNullable.HasValue && convalidaNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Convalida: {convalidaString}").SetFixedPosition(70, 470, 200));
                            bool? convalidaNullable = schede.Rows[0].ItemArray[21] as bool?;
                            string convalidaString = convalidaNullable.HasValue && convalidaNullable.Value ? "X" : "";
                            float startY_Convalida = startY; // Use the dynamic startY
                            if (convalidaString == "X")
                            {
                                stampaX(startX_50, startY_Convalida, document, true);


                                // --- Paragrafo per la descrizione "Convalida:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Convalida:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_Convalida + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Convalida - 5, 200); // Usa startX_Convalida e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Convalida, document, false);
                                // --- Solo la descrizione "Convalida:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Convalida:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Convalida, 200); // Usa startX_Convalida
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Dissequestro Definitivo
                            bool? dissequestrodefinitivoNullable = schede.Rows[0].ItemArray[22] as bool?;
                            string dissequestrodefinitivoString = dissequestrodefinitivoNullable.HasValue && dissequestrodefinitivoNullable.Value ? "X" : "";
                            float startY_DissequestroDefinitivo = startY; // Use the dynamic startY
                            if (dissequestrodefinitivoString == "X")
                            {
                                stampaX(startX_50, startY_DissequestroDefinitivo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Definitivo:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_DissequestroDefinitivo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_DissequestroDefinitivo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Definitivo:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_DissequestroDefinitivo, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Violazione sigilli
                            bool? violazionesigilliNullable = schede.Rows[0].ItemArray[26] as bool?;
                            string violazionesigilliString = violazionesigilliNullable.HasValue && violazionesigilliNullable.Value ? "X" : "";
                            float startY_violazionesigilli = startY; // Use the dynamic startY
                            if (violazionesigilliString == "X")
                            {
                                stampaX(startX_50, startY_violazionesigilli, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Violazione sigilli:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_violazionesigilli - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_violazionesigilli, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Violazione sigilli:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_violazionesigilli, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Dissequestro Temporaneo
                            bool? dissequestrotemporaneoNullable = schede.Rows[0].ItemArray[23] as bool?;
                            string dissequestrotemporaneoString = dissequestrotemporaneoNullable.HasValue && dissequestrotemporaneoNullable.Value ? "X" : "";
                            float startY_dissequestrotemporaneo = startY; // Use the dynamic startY
                            if (dissequestrotemporaneoString == "X")
                            {
                                stampaX(startX_50, startY_dissequestrotemporaneo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Temporaneo:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_dissequestrotemporaneo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_dissequestrotemporaneo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Temporaneo:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_dissequestrotemporaneo, 200);
                                document.Add(descriptionParagraph);
                            }

                            // Rimozione
                            bool? rimozioneNullable = schede.Rows[0].ItemArray[24] as bool?;
                            string rimozioneString = rimozioneNullable.HasValue && rimozioneNullable.Value ? "X" : "";
                            // float startY_Rimozione = startX_270; // Use the dynamic startY
                            if (rimozioneString == "X")
                            {
                                stampaX(startX_270, startY_dissequestrotemporaneo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Rimozione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_290 + boxSize + 5, startY_dissequestrotemporaneo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_270, startY_dissequestrotemporaneo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Rimozione:");
                                descriptionParagraph.SetFixedPosition(startX_290, startY_dissequestrotemporaneo, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Riapposizione
                            bool? riapposizioneNullable = schede.Rows[0].ItemArray[25] as bool?;
                            string riapposizioneString = riapposizioneNullable.HasValue && riapposizioneNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Riapposizione: {riapposizioneString}").SetFixedPosition(400, 430, 80));
                            if (riapposizioneString == "X")
                            {
                                stampaX(startX_400, startY_dissequestrotemporaneo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Riapposizione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_450 + boxSize + 5, startY_dissequestrotemporaneo - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_400, startY_dissequestrotemporaneo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Riapposizione:");
                                descriptionParagraph.SetFixedPosition(startX_450, startY_dissequestrotemporaneo, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Violaz. Cod. Beni Culturali
                            bool? violazioniCodiciNullable = schede.Rows[0].ItemArray[31] as bool?;
                            string violazioniCodiciString = violazioniCodiciNullable.HasValue && violazioniCodiciNullable.Value ? "X" : "";
                            float startY_violazioniCodici = startY; // Use the dynamic startY
                            if (violazioniCodiciString == "X")
                            {
                                stampaX(startX_50, startY_violazioniCodici, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Violaz. Cod. Beni Culturali:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_violazioniCodici - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_violazioniCodici, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Violaz. Cod. Beni Culturali:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_violazioniCodici, 200);
                                document.Add(descriptionParagraph);
                            }
                            //***
                            startY -= lineHeight; // Move to the next line
                            // Accertamento avvenuto ripristino
                            bool? accertamentoRipNullable = schede.Rows[0].ItemArray[28] as bool?;
                            string accertamentoRipString = accertamentoRipNullable.HasValue && accertamentoRipNullable.Value ? "X" : "";
                            float startY_accertamentoRip = startY; // Use the dynamic startY
                            if (accertamentoRipString == "X")
                            {
                                stampaX(startX_50, startY_accertamentoRip, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Accertamento avvenuto ripristino:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_accertamentoRip - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Accertamento avvenuto ripristino:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_accertamentoRip, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Totale 
                            bool? totaleRipNullable = schede.Rows[0].ItemArray[29] as bool?;
                            string totaleRipString = totaleRipNullable.HasValue && totaleRipNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Totale: {totaleRipString}").SetFixedPosition(270, 390, 100));
                            if (totaleRipString == "X")
                            {
                                stampaX(startX_270, startY_accertamentoRip, document, true);
                                //// Ottieni il PdfDocument e PdfCanvas

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Totale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(280 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_270, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Totale:");
                                descriptionParagraph.SetFixedPosition(280, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Parziale 
                            bool? parzialeRipNullable = schede.Rows[0].ItemArray[30] as bool?;
                            string parzialeRipString = parzialeRipNullable.HasValue && parzialeRipNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Parziale:  {parzialeRipString}").SetFixedPosition(350, 390, 100));
                            if (parzialeRipString == "X")
                            {
                                stampaX(startX_350, startY_accertamentoRip, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Parziale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_370 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_350, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Parziale:");
                                descriptionParagraph.SetFixedPosition(startX_370, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Non Avvenuto 
                            bool? NonAvvenutoRipNullable = schede.Rows[0].ItemArray[47] as bool?;
                            string NonAvvenutoRipString = NonAvvenutoRipNullable.HasValue && NonAvvenutoRipNullable.Value ? "X" : "";

                            if (NonAvvenutoRipString == "X")
                            {
                                stampaX(startX_450, startY_accertamentoRip, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Non Avvenuto:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_470 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_450, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Non Avvenuto:");
                                descriptionParagraph.SetFixedPosition(startX_470, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            //***
                            startY -= lineHeight; // Move to the next line
                            // Sgomberi
                            bool? SgomberiNullable = schede.Rows[0].ItemArray[52] as bool?;
                            string SgomberiString = SgomberiNullable.HasValue && SgomberiNullable.Value ? "X" : "";
                            float startY_Sgomberi = startY; // Use the dynamic startY
                            if (SgomberiString == "X")
                            {
                                stampaX(startX_50, startY_Sgomberi, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Sgomberi:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Sgomberi - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_Sgomberi, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Sgomberi:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Sgomberi, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Sgomberi abusi 
                            bool? SgomberiAbusiNullable = schede.Rows[0].ItemArray[53] as bool?;
                            string SgomberiAbusiString = SgomberiAbusiNullable.HasValue && SgomberiAbusiNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Totale: {totaleRipString}").SetFixedPosition(270, 390, 100));
                            if (SgomberiAbusiString == "X")
                            {
                                stampaX(startX_270, startY_Sgomberi, document, true);
                                //// Ottieni il PdfDocument e PdfCanvas

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Occ. Abusiva:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(280 + boxSize + 5, startY_Sgomberi - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_270, startY_Sgomberi, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Occ. Abusiva:");
                                descriptionParagraph.SetFixedPosition(280, startY_Sgomberi, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Sgmoberi immobili  
                            bool? SgmoberiImmobiliNullable = schede.Rows[0].ItemArray[54] as bool?;
                            string SgmoberiImmobiliString = SgmoberiImmobiliNullable.HasValue && SgmoberiImmobiliNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Parziale:  {parzialeRipString}").SetFixedPosition(350, 390, 100));
                            if (SgmoberiImmobiliString == "X")
                            {
                                stampaX(300, startY_Sgomberi, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Immobili e/o Area pubb:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(300 + boxSize + 5, startY_Sgomberi - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(400, startY_Sgomberi, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Imm./Area pubb:");
                                descriptionParagraph.SetFixedPosition(420, startY_Sgomberi, 100);
                                document.Add(descriptionParagraph);
                            }

                            startY -= lineHeight; // Move to the next line
                            // Controlli Scia
                            bool? sciaNullable = schede.Rows[0].ItemArray[27] as bool?;
                            string sciaString = sciaNullable.HasValue && sciaNullable.Value ? "X" : "";
                            float startY_scia = startY; // Use the dynamic startY
                            if (sciaString == "X")
                            {
                                stampaX(startX_50, startY_scia, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli Scia:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_scia - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_scia, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli Scia:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_scia, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //  Verbale di verifica occupazionale / censimento
                            bool? VerbOccCensNullable = schede.Rows[0]["rapp_verbOccCensimento"] as bool?;
                            string VerbOccCensString = VerbOccCensNullable.HasValue && VerbOccCensNullable.Value ? "X" : "";
                            float startY_VerbOccCens = startY; // Use the dynamic startY
                            if (VerbOccCensString == "X")
                            {
                                stampaX(startX_50, startY_VerbOccCens, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale di verifica occupazionale / censimento:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 1, startY_VerbOccCens - 5, 400); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_VerbOccCens, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Verbale di verifica occupazionale / censimento:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_VerbOccCens, 400);
                                document.Add(descriptionParagraph);
                            }
                            //riga interruzione sezione
                            float x3 = 65;
                            float y3 = startY;
                            float width3 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas3 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x3, y3) // Inizia la linea nel punto (x, y)
                                  .LineTo(x3 + width3, y3) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // QUALIFICAZIONE INTERVENTO
                            document.Add(new Paragraph("QUALIFICAZIONE INTERVENTO").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                            startY -= lineHeight; // Move to the next line
                            // Controllo aree cantiere su suolo pubblico
                            bool? contrAreeNullable = schede.Rows[0].ItemArray[32] as bool?;
                            string contrAreeString = contrAreeNullable.HasValue && contrAreeNullable.Value ? "X" : "";
                            float startY_contrAree = startY; // Use the dynamic startY
                            if (contrAreeString == "X")
                            {
                                stampaX(startX_50, startY_contrAree, document, true);
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo aree cantiere su suolo pubblico (impalcature):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrAree - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrAree, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo aree cantiere su suolo pubblico (impalcature):");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrAree, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo Cantiere
                            bool? contrSeqNullable = schede.Rows[0].ItemArray[34] as bool?;
                            string contrSeqString = contrSeqNullable.HasValue && contrSeqNullable.Value ? "X" : "";
                            float startY_contrSeq = startY; // Use the dynamic startY
                            if (contrSeqString == "X")
                            {
                                stampaX(startX_50, startY_contrSeq, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo Cantiere rientrano i controlli anche dei cantieri a sequestro:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrSeq - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrSeq, document, false);
                                // --- Solo la descrizione, nella posizione originale ---rel
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo Cantiere rientrano i controlli anche dei cantieri a sequestro:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrSeq, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo nato da esposti
                            bool? contrEspNullable = schede.Rows[0].ItemArray[35] as bool?;
                            string contrEspString = contrEspNullable.HasValue && contrEspNullable.Value ? "X" : "";
                            float startY_contrEsp = startY; // Use the dynamic startY
                            if (contrEspString == "X")
                            {
                                stampaX(startX_50, startY_contrEsp, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da esposti:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrEsp - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrEsp, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da esposti:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrEsp, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo nato da segnalazioni
                            bool? contrSegnNullable = schede.Rows[0].ItemArray[36] as bool?;
                            string contrSegnString = contrSegnNullable.HasValue && contrSegnNullable.Value ? "X" : "";
                            float startY_contrSegn = startY; // Use the dynamic startY
                            if (contrSegnString == "X")
                            {
                                stampaX(startX_50, startY_contrSegn, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da segnalazioni:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrSegn - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrSegn, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da segnalazioni:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrSegn, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controlli lavori edili con/senza protezione (d.p.i.)
                            bool? contrEdilNullable = schede.Rows[0].ItemArray[33] as bool?;
                            string contrEdilString = contrEdilNullable.HasValue && contrEdilNullable.Value ? "X" : "";
                            float startY_contrEdil = startY; // Use the dynamic startY
                            if (contrEdilString == "X")
                            {
                                stampaX(startX_50, startY_contrEdil, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli lavori edili con/senza protezione (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrEdil - 5, 600); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrEdil, document, false);

                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli lavori edili con/senza protezione (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrEdil, 800);
                                document.Add(descriptionParagraph);
                            }

                            // Con (d.p.i.)
                            bool? contrConDpiNullable = schede.Rows[0].ItemArray[44] as bool?;
                            string contrConDpiString = contrConDpiNullable.HasValue && contrConDpiNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Con  {contrConDpiString}").SetFixedPosition(350, 250, 70));
                            if (contrConDpiString == "X")
                            {
                                stampaX(startX_350, startY_contrEdil, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Con (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_350 + boxSize + 5, startY_contrEdil - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_350, startY_contrEdil, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Con (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_370, startY_contrEdil, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Senza (d.p.i.)
                            bool? contrSenzaDpiNullable = schede.Rows[0].ItemArray[45] as bool?;
                            string contrSenzaDpiString = contrSenzaDpiNullable.HasValue && contrSenzaDpiNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Senza  {contrSenzaDpiString}").SetFixedPosition(450, 250, 70));
                            if (contrSenzaDpiString == "X")
                            {
                                stampaX(startX_450, startY_contrEdil, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Senza (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_470 + boxSize + 5, startY_contrEdil - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_450, startY_contrEdil, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Senza (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_470, startY_contrEdil, 100);
                                document.Add(descriptionParagraph);
                            }

                            //****
                            // Controlli occupazione abusiva imm. propr. comunale (abitativo - non abitativo)
                            startY -= lineHeight; // Move to the next line
                            bool? contrOccupazioneNullable = schede.Rows[0].ItemArray[49] as bool?;
                            string contrOccupazioneString = contrOccupazioneNullable.HasValue && contrOccupazioneNullable.Value ? "X" : "";
                            float startY_contrOccupazione = startY; // Use the dynamic startY
                            if (contrOccupazioneString == "X")
                            {
                                stampaX(startX_50, startY_contrOccupazione, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli occupazione abusiva imm. propr. comunale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrOccupazione - 5, 600); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrOccupazione, document, false);

                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli occupazione abusiva imm. propr. comunale:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrOccupazione, 800);
                                document.Add(descriptionParagraph);
                            }

                            // abitativo
                            bool? abitativoNullable = schede.Rows[0].ItemArray[50] as bool?;
                            string abitativoString = abitativoNullable.HasValue && abitativoNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Con  {contrConDpiString}").SetFixedPosition(350, 250, 70));
                            if (abitativoString == "X")
                            {
                                stampaX(startX_350, startY_contrOccupazione + 1, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Abitat.:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_350 + boxSize + 5, startY_contrOccupazione - 1, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);
                                startY -= lineHeight;
                                //abusi si
                                object sumObject = schede.Compute("SUM(rapp_NumAbusiAbitatSi)", "");
                                string totaleSomma = (sumObject != DBNull.Value) ? sumObject.ToString() : "0";
                                document.Add(new Paragraph("S" + totaleSomma).SetFixedPosition(410, startY_contrOccupazione, 20));
                                //no abusi
                                object sumObject1 = schede.Compute("SUM(rapp_NumAbusiAbitatNo)", "");
                                string totaleSomma1 = (sumObject1 != DBNull.Value) ? sumObject1.ToString() : "0";
                                document.Add(new Paragraph("N" + totaleSomma1).SetFixedPosition(430, startY_contrOccupazione, 20));
                            }
                            else
                            {
                                stampaX(startX_350, startY_contrOccupazione, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Abitat.:");
                                descriptionParagraph.SetFixedPosition(startX_370, startY_contrOccupazione, 100);
                                document.Add(descriptionParagraph);
                            }


                            // no abitativo
                            bool? NoabitativoNullable = schede.Rows[0].ItemArray[51] as bool?;
                            string NoabitativoString = NoabitativoNullable.HasValue && NoabitativoNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Senza  {contrSenzaDpiString}").SetFixedPosition(450, 250, 70));
                            if (NoabitativoString == "X")
                            {
                                stampaX(startX_450, startY_contrOccupazione, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Non Abit:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_450 + boxSize + 5, startY_contrOccupazione - 1, 80); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);
                                //abusi si
                                object sumObject = schede.Compute("SUM(rapp_NumAbusiNoAbitatSi)", "");
                                string totaleSomma = (sumObject != DBNull.Value) ? sumObject.ToString() : "0";
                                document.Add(new Paragraph("S" + totaleSomma).SetFixedPosition(520, startY_contrOccupazione, 20));
                                //no abusi
                                object sumObject1 = schede.Compute("SUM(rapp_NumAbusiNoAbitatNo)", "");
                                string totaleSomma1 = (sumObject1 != DBNull.Value) ? sumObject1.ToString() : "0";
                                document.Add(new Paragraph("N" + totaleSomma1).SetFixedPosition(540, startY_contrOccupazione, 20));
                            }
                            else
                            {
                                stampaX(startX_450, startY_contrOccupazione, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Non Abit:");
                                descriptionParagraph.SetFixedPosition(startX_450, startY_contrOccupazione, 5);
                                document.Add(descriptionParagraph);
                            }

                            //***
                            startY -= lineHeight; // Move to the next line

                            // censimento nuclei familiari
                            bool? cenrimentoNucFamNullable = schede.Rows[0].ItemArray[48] as bool?;
                            string cenrimentoNucFamString = cenrimentoNucFamNullable.HasValue && cenrimentoNucFamNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "notifica no Ag" ---

                            float startY_cenrimentoNucFam = startY; // Use the dynamic startY
                            if (cenrimentoNucFamString == "X")
                            {
                                stampaX(startX_50, startY_cenrimentoNucFam, document, true);

                                // --- Paragrafo per la descrizione "startY_notif no Ag:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Censimento nuclei c/o alloggi pubb.:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_cenrimentoNucFam - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_cenrimentoNucFam, document, false);
                                // --- Solo la descrizione "startY_notif no Ag:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Censimento nuclei c/o alloggi pubb.:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_cenrimentoNucFam, 200);
                                document.Add(descriptionParagraph);
                            }
                            object sumObjectpubb = schede.Compute("SUM(rapp_num_censimento_all_pubb)", "");
                            string totaleSommapubb = (sumObjectpubb != DBNull.Value) ? sumObjectpubb.ToString() : "0";
                            document.Add(new Paragraph(totaleSommapubb).SetFixedPosition(280, startY_cenrimentoNucFam - 5, 200));


                            startY -= lineHeight; // Move to the next line

                            // controllo nato da accertamenti
                            bool? contrNatoDaAccNullable = schede.Rows[0]["rapp_contrNatoDaAcc"] as bool?;
                            string contrNatoDaAccString = contrNatoDaAccNullable.HasValue && contrNatoDaAccNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "notifica no Ag" ---

                            float startY_contrNatoDaAcc = startY; // Use the dynamic startY
                            if (contrNatoDaAccString == "X")
                            {
                                stampaX(startX_50, startY_contrNatoDaAcc, document, true);

                                // --- Paragrafo per la descrizione "startY_notif no Ag:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo/i nato da acc. Rich:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrNatoDaAcc - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_contrNatoDaAcc, document, false);
                                // --- Solo la descrizione "startY_notif no Ag:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo/i nato da acc. Rich:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrNatoDaAcc, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                                                  // float startY_NumCensimenti = startY_430; //


                            document.Add(new Paragraph($"Num. Acc. Rich: {schede.Rows[0]["rapp_NumcontrNatoDaAcc"]}").SetFixedPosition(300, startY_contrNatoDaAcc - 5, 200));

                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x4 = 65;
                            float y4 = startY;
                            float width4 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas4 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x4, y4) // Inizia la linea nel punto (x, y)
                                  .LineTo(x4 + width4, y4) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight1; // Move to the next line
                            // La PG Operante - Sezione firma
                            document.Add(new Paragraph($"La PG Operante").SetFixedPosition(280, startY, 500));
                            startY -= lineHeight1; // Move to the next line
                            document.Add(new Paragraph($"_______________________/_______________________/_______________________").SetFixedPosition(55, startY, 500));
                            //startY -= lineHeight1; // Move to the next line
                            //document.Add(new Paragraph($"_______________________").SetFixedPosition(260, startY, 500));

                            document.Close(); // Chiude il documento.


                        }

                    }
                }

                // Invia l'output PDF direttamente al browser.
                byte[] pdfBytes = stream.ToArray();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/pdf";
                response.AddHeader("Content-Disposition", "inline; filename=SchedaIntervento.pdf");
                response.BinaryWrite(pdfBytes);
                response.Flush();
                response.End();
            }

        }
        /// <summary>
        /// funzione che inserisce spaces al posto del min data value
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static string FormatMyDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
            {
                return "";
            }

            DateTime date;
            if (DateTime.TryParse(dateValue.ToString(), out date))
            {
                if (date == new DateTime(1900, 1, 1) || date == new DateTime(1, 1, 1))
                {
                    return ""; // O " " se vuoi uno spazio fisico
                }
                return date.ToString("dd/MM/yyyy");
            }
            return ""; // Gestione di valori non validi
        }
        /// <summary>
        /// estrae la tabella Registro e la esporta in un file Excel
        /// </summary>
        /// <param name="listaDati"></param>
        /// <param name="Filename"></param>
        /// <param name="Context"></param>
        public void CreaExcelRegistro(DataTable listaDati, string Filename, HttpContext Context)
        {
            // 2. CREA IL WORKBOOK
            string tempFilePath = System.IO.Path.GetTempFileName(); // Ottieni un nome di file temporaneo univoco
            tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".xlsx"); // Cambia l'estensione in .xlsx
            string semestre = (DateTime.Now.Month <= 6) ? "I SEM" : "II SEM";
            // 2. Esporta la DataTable in Excel


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Registro " + semestre + " " + DateTime.Now.Year);

                // 1. AGGIUNTA COLONNA CONTATORE (PRIMA DELL'ESPORTAZIONE)
                // Aggiungo la colonna "N." di tipo intero
                DataColumn colContatore = new DataColumn("N.", typeof(int));
                if (!listaDati.Columns.Contains("N."))
                {
                    listaDati.Columns.Add(colContatore);
                }

                // Sposto la colonna "N." in prima posizione (indice 0)
                listaDati.Columns["N."].SetOrdinal(0);

                // Popolo il contatore progressivo
                for (int i = 0; i < listaDati.Rows.Count; i++)
                {
                    listaDati.Rows[i]["N."] = i + 1;
                }

                // 2. RINOMINA COLONNE (Tuo codice esistente)
                var columnRenameMap = new Dictionary<string, string>
    {
        { "oggetto", "OGGETTO" },
        { "dataPresentRichiesta", "DATA PRESENTAZIONE RICHIESTA" },
        { "nrPgTrasmissioneRichiesto", "NUMERO PG TRASMISSIONE\r\nRICHIESTA DA PARTE DELL'URP" },
        { "uffDetentore", "UFFICIO DETENTORE" },
        { "CONTROINTERESSATI", "CONTRO INTERESSATI" },
        { "esito", "ESITO" },
        { "motivazione", "MOTIVAZIONE\r\n(diniego/differimento)" },
        { "nrPgTrasmissioneRiscontro", "NUMERO PG TRASMISSIONE\r\n RISCONTRO ALL'URP" },
        { "dataConclProcedimento", "DATA CONCLUSIONE\r\nPROCEDIMENTO" }
    };

                foreach (var entry in columnRenameMap)
                {
                    if (listaDati.Columns.Contains(entry.Key))
                    {
                        listaDati.Columns[entry.Key].ColumnName = entry.Value;
                    }
                }

                // 3. CREAZIONE RIGA DI INTESTAZIONE (TITOLO) ALLA RIGA 1
                string titoloPrincipale = "REGISTRO DELLE RICHIESTE DI ACCESSO DOCUMENTALE\r\n(ART. 22 LEGGE 241/1990)\r\n " + semestre + " " + DateTime.Now.Year;
                var cellaTitolo = worksheet.Cell(1, 1);
                cellaTitolo.Value = titoloPrincipale;

                // Unisco le celle dalla colonna 1 fino all'ultima colonna occupata dai dati
                worksheet.Range(1, 1, 1, listaDati.Columns.Count).Merge();

                // Stile del titolo principale
                var stileTitolo = worksheet.Range(1, 1, 1, listaDati.Columns.Count).Style;
                stileTitolo.Font.Bold = true;
                stileTitolo.Font.FontSize = 14;
                stileTitolo.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                stileTitolo.Fill.BackgroundColor = XLColor.Orange;


                // Stile riga intestazione
                var stileRigaIntest = worksheet.Range(2, 1, 2, listaDati.Columns.Count).Style;
                stileRigaIntest.Font.Bold = true;
                stileRigaIntest.Font.FontSize = 12;
                stileRigaIntest.Font.FontColor = XLColor.Black;
                stileRigaIntest.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                stileRigaIntest.Fill.BackgroundColor = XLColor.Silver; // Opzionale: sfondo grigio chiaro per il titolo

                // 4. INSERIMENTO TABELLA DATI (PARTENDO DALLA RIGA 2)
                // Usiamo Cell(2, 1) invece di Cell(1, 1) per lasciare spazio al titolo
                var table = worksheet.Cell(2, 1).InsertTable(listaDati);
                table.Theme = XLTableTheme.None;
                // Opzionale: se non vuoi le freccette dei filtri sulla riga 2
                table.ShowAutoFilter = false;
                // Opzionale: togliere i filtri automatici se non li vuoi
                // table.ShowAutoFilter = false;

                // 5. FORMATTAZIONE
                worksheet.Columns().AdjustToContents(); // Auto-fit

                // Forza il "Testo a capo" per le intestazioni della tabella (riga 2) perché contengono \r\n
                worksheet.Row(2).Style.Alignment.WrapText = true;
                // Rieseguo l'autofit sulla riga 2 per adattare l'altezza al testo a capo
                worksheet.Row(2).AdjustToContents();

                worksheet.Row(1).Height = 60;
                worksheet.Row(1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Routine per convertire booleani (True/False -> Si/No)
                Routine al = new Routine();
                // NOTA: Controlla che la tua Routine gestisca il fatto che ora i dati partono dalla riga 3 (Riga 1 Titolo, Riga 2 Intestazioni)
                al.ConvertiBooleaniInItaliano(worksheet);


                string fileNameForDownload = Filename + @"\\REG ACC " + semestre + " " + DateTime.Now.Year + ".xlsx";
                workbook.SaveAs(fileNameForDownload);
            }
        }
        public void CreaExcelTurnazioneMensile(List<DipendenteTurno> listaDati, int anno, int mese, int giorniMese, HttpContext Context)
        {
            // 2. CREA IL WORKBOOK
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add($"Turni {mese}-{anno}");
                var itCulture = CultureInfo.GetCultureInfo("it-IT"); // Per i nomi giorni in Italiano

                // --- STILI ---
                var stileCellaBase = wb.Style;
                stileCellaBase.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                stileCellaBase.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                stileCellaBase.Border.OutsideBorder = XLBorderStyleValues.Thin;
                stileCellaBase.Font.FontSize = 10;

                // --- RIGA 1: INTESTAZIONI ---
                int riga = 1;

                // Colonne Fisse (Senza Matricola)
                ws.Cell(riga, 1).Value = "Nominativo";
                ws.Cell(riga, 2).Value = "Q";

                // Stile Intestazione Fissa
                var headFixed = ws.Range(riga, 1, riga, 2);
                headFixed.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
                headFixed.Style.Font.FontColor = XLColor.White;
                headFixed.Style.Font.Bold = true;
                headFixed.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Colonne Giorni
                for (int i = 1; i <= giorniMese; i++)
                {
                    var dt = new DateTime(anno, mese, i);

                    // Calcolo lettera giorno (es. "lun" -> "L")
                    string nomeGiorno = dt.ToString("ddd", itCulture).ToUpper(); // LUN, MAR...
                    string lettera = nomeGiorno.Substring(0, 1);

                    // Scrittura Header: "1 L"
                    var cella = ws.Cell(riga, 2 + i); // Offset di 2 colonne (Nom, Q)
                    cella.Value = $"{i} {lettera}";

                    // Colora Header Festivi/Weekend
                    if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || IsGiornoFestivo(dt))
                    {
                        cella.Style.Fill.BackgroundColor = XLColor.DarkRed;
                    }
                    else
                    {
                        cella.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
                    }
                    cella.Style.Font.FontColor = XLColor.White;
                    cella.Style.Font.Bold = true;
                    cella.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cella.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // --- CORPO DEL REPORT ---
                var gruppi = listaDati.GroupBy(x => x.Ufficio).OrderBy(k => k.Key);

                riga++;

                foreach (var gruppo in gruppi)
                {
                    // RIGA UFFICIO (Colspan adattato: giorniMese + 2 colonne fisse)
                    var cellaUfficio = ws.Range(riga, 1, riga, 2 + giorniMese);
                    cellaUfficio.Merge();
                    cellaUfficio.Value = "📂 " + gruppo.Key.ToUpper();
                    cellaUfficio.Style.Fill.BackgroundColor = XLColor.LightGray;
                    cellaUfficio.Style.Font.Bold = true;
                    cellaUfficio.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    riga++;

                    foreach (var dip in gruppo)
                    {
                        // 1. NOMINATIVO
                        ws.Cell(riga, 1).Value = dip.Nominativo;
                        ws.Cell(riga, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left; // Nome a sx

                        // 2. QUARTINA
                        ws.Cell(riga, 2).Value = "Q" + dip.QuartinaID;

                        // Bordo per anagrafica
                        ws.Range(riga, 1, riga, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        // 3. GIORNI
                        for (int i = 1; i <= giorniMese; i++)
                        {
                            var cella = ws.Cell(riga, 2 + i); // Offset 2
                            string turno = dip.TurniMensili[i] ?? "";

                            cella.Value = turno;
                            cella.Style = stileCellaBase; // Applica stile centrato con bordi

                            // --- COLORI (Uguali al web) ---
                            if (turno == "Q")
                            {
                                cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#ffcdd2");
                                cella.Style.Font.FontColor = XLColor.DarkRed;
                                cella.Style.Font.Bold = true;
                            }
                            else if (turno == "1")
                            {
                                cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#e3f2fd");
                                cella.Style.Font.FontColor = XLColor.DarkBlue;
                                cella.Style.Font.Bold = true;
                            }
                            else if (turno == "2")
                            {
                                cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#fff8e1");
                                cella.Style.Font.FontColor = XLColor.FromHtml("#e65100");
                                cella.Style.Font.Bold = true;
                            }
                            else if (turno == "RF")
                            {
                                cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#c8e6c9");
                                cella.Style.Font.FontColor = XLColor.DarkGreen;
                                cella.Style.Font.Bold = true;
                            }
                            else
                            {
                                // Celle vuote weekend grigie
                                DateTime dt = new DateTime(anno, mese, i);
                                if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#f2f2f2");
                                }
                            }
                        }
                        riga++;
                    }
                }

                // --- FORMATTAZIONE LARGHEZZE ---
                ws.Column(1).Width = 35;  // Nominativo più largo
                ws.Column(2).Width = 5;   // Q stretta

                // Colonne Giorni
                for (int i = 1; i <= giorniMese; i++)
                {
                    ws.Column(2 + i).Width = 4; // Larghezza fissa per i giorni
                }
                HttpResponse Response = HttpContext.Current.Response;
                // --- DOWNLOAD ---
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment;filename=Turni_{mese}_{anno}.xlsx");

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.SuppressContent = true;
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }
        /// <summary>
        /// ctrea pdf turnazine mensile
        /// </summary>
        /// <param name="listaDati"></param>
        /// <param name="nomeMeseTesto"></param>
        /// <param name="anno"></param>
        /// <param name="mese"></param>
        /// <param name="giorniMese"></param>
        public void CreaPdfTurnazioneMensile(List<DipendenteTurno> listaDati, string nomeMeseTesto, int anno, int mese, int giorniMese)
        {
            byte[] bytes;

            // USARE GLI USING CORRETTAMENTE PER GARANTIRE IL FLUSH DEI DATI
            using (MemoryStream stream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(stream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        // Orientamento Orizzontale (Landscape) per far stare 31 giorni
                        using (Document document = new Document(pdf, PageSize.A4.Rotate()))
                        {
                            document.SetMargins(10, 10, 10, 10);
                            PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                            PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                            // Titolo
                            document.Add(new Paragraph($"TURNAZIONE: {nomeMeseTesto.ToUpper()} {anno}")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(14)
                                .SetFont(fontBold));

                            // Definizione larghezze colonne (2 + giorniMese)
                            float[] colWidths = new float[2 + giorniMese];
                            colWidths[0] = 10f; // Nominativo
                            colWidths[1] = 2f;  // Q
                            for (int i = 0; i < giorniMese; i++) colWidths[2 + i] = 1.3f;

                            iText.Layout.Element.Table table = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(colWidths));
                            table.SetWidth(UnitValue.CreatePercentValue(100));

                            // HEADER
                            AddCellHeader(table, "Nominativo", new DeviceRgb(64, 64, 64), fontBold);
                            AddCellHeader(table, "Q", new DeviceRgb(64, 64, 64), fontBold);

                            var itCulture = CultureInfo.GetCultureInfo("it-IT");
                            for (int i = 1; i <= giorniMese; i++)
                            {
                                DateTime dt = new DateTime(anno, mese, i);
                                string lettera = dt.ToString("ddd", itCulture).Substring(0, 1).ToUpper();

                                Color headColor = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || IsGiornoFestivo(dt))
                                                  ? new DeviceRgb(150, 0, 0) : new DeviceRgb(64, 64, 64);

                                AddCellHeader(table, $"{i}\n{lettera}", headColor, fontBold);
                            }

                            // CORPO
                            var gruppi = listaDati.GroupBy(x => x.Ufficio).OrderBy(k => k.Key);
                            foreach (var gruppo in gruppi)
                            {
                                // Riga Ufficio
                                Cell cellUfficio = new Cell(1, 2 + giorniMese)
                                    .Add(new Paragraph("📂 " + gruppo.Key.ToUpper()))
                                    .SetBackgroundColor(new DeviceRgb(220, 220, 220))
                                    .SetFont(fontBold).SetFontSize(8).SetPadding(2);
                                table.AddCell(cellUfficio);

                                foreach (var dip in gruppo)
                                {
                                    string nomeDaStampare = string.IsNullOrEmpty(dip.Nominativo) ? "NOME MANCANTE" : dip.Nominativo.ToUpper();
                                    AddCellBody(table, nomeDaStampare, ColorConstants.WHITE, fontNormal, 6, TextAlignment.LEFT);


                                    //            AddCellBody(table, dip.Nominativo, ColorConstants.WHITE, fontNormal, 7, TextAlignment.LEFT);
                                    AddCellBody(table, "Q" + dip.QuartinaID, ColorConstants.WHITE, fontNormal, 7, TextAlignment.CENTER);

                                    for (int i = 1; i <= giorniMese; i++)
                                    {
                                        //     string turno = (dip.TurniMensili != null && i < dip.TurniMensili.Length) ? dip.TurniMensili[i] : "";
                                        // Se TurniMensili è null o l'indice è vuoto, metti un segnaposto per il debug
                                        string turno = "";
                                        if (dip.TurniMensili != null && i < dip.TurniMensili.Length)
                                        {
                                            turno = dip.TurniMensili[i];
                                        }

                                        // --- DEBUG: Se vedi "-" nel PDF, significa che la lista ha i dipendenti ma l'array Turni è vuoto ---
                                        if (string.IsNullOrEmpty(turno)) turno = "-";
                                        // Semplificazione colori per brevità
                                        Color bg = ColorConstants.WHITE;
                                        if (turno == "Q") bg = new DeviceRgb(255, 205, 210);
                                        else if (turno == "1") bg = new DeviceRgb(227, 242, 253);
                                        else if (turno == "2") bg = new DeviceRgb(255, 248, 225);
                                        else if (turno == "RF") bg = new DeviceRgb(200, 230, 201);

                                        Cell c = new Cell().Add(new Paragraph(turno ?? ""))
                                            .SetBackgroundColor(bg).SetFontSize(7).SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(c);
                                    }
                                }
                            }

                            document.Add(table);
                            // FONDAMENTALE: document.Close() deve essere chiamato DENTRO l'using dello stream 
                            // ma PRIMA di fare ToArray()
                            document.Close();
                        }
                    }
                }
                bytes = stream.ToArray();
            }
            HttpResponse Response = HttpContext.Current.Response;
            // INVIO FILE AL BROWSER
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", bytes.Length.ToString());
            Response.AddHeader("Content-Disposition", $"attachment; filename=Turni_{mese}_{anno}.pdf");

            Response.BinaryWrite(bytes);
            Response.Flush();
            // Non usare Response.End() perché lancia eccezioni che rompono gli using. 
            // Meglio questa sequenza:
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        private bool IsGiornoFestivo(DateTime dt)
        {
            // 1. Controlla le festività fisse (giorno, mese)
            if (dt.Day == 1 && dt.Month == 1) return true;   // Capodanno
            if (dt.Day == 6 && dt.Month == 1) return true;   // Epifania
            if (dt.Day == 25 && dt.Month == 4) return true;  // Liberazione
            if (dt.Day == 1 && dt.Month == 5) return true;   // Festa Lavoro
            if (dt.Day == 2 && dt.Month == 6) return true;   // Repubblica
            if (dt.Day == 15 && dt.Month == 8) return true;  // Ferragosto
            if (dt.Day == 1 && dt.Month == 11) return true;  // Ognissanti
            if (dt.Day == 8 && dt.Month == 12) return true;  // Immacolata
            if (dt.Day == 25 && dt.Month == 12) return true; // Natale
            if (dt.Day == 26 && dt.Month == 12) return true; // Santo Stefano

            // 2. Calcolo della Pasqua (Algoritmo standard)
            int year = dt.Year;
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            DateTime pasqua = new DateTime(year, month, day);
            DateTime pasquetta = pasqua.AddDays(1);

            // Controlla Pasqua e Pasquetta
            if (dt.Date == pasqua.Date) return true;
            if (dt.Date == pasquetta.Date) return true;

            // 3. Controlla la Domenica
            if (dt.DayOfWeek == DayOfWeek.Sunday) return true;

            // Nota: Se devi gestire il Santo Patrono locale, aggiungi qui la data specifica
            // es: if (dt.Day == 24 && dt.Month == 6) return true; // San Giovanni

            return false;
        }
        private void AddCellHeader(iText.Layout.Element.Table table, string text, Color bgColor, PdfFont font)
        {
            Cell c = new Cell().Add(new Paragraph(text));
            c.SetBackgroundColor(bgColor);
            c.SetFontColor(ColorConstants.WHITE);
            c.SetFont(font); // Qui applico Helvetica Bold
            c.SetFontSize(7);
            c.SetTextAlignment(TextAlignment.CENTER);
            c.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            table.AddCell(c);
        }
        private void AddCellBody(iText.Layout.Element.Table table, string text, Color bgColor, PdfFont font, float fontSize, TextAlignment align)
        {
            Cell c = new Cell().Add(new Paragraph(text));
            c.SetBackgroundColor(bgColor);
            c.SetFont(font);
            c.SetFontSize(fontSize);
            c.SetTextAlignment(align);
            c.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            c.SetPaddingLeft(2);
            table.AddCell(c);
        }
        public void CreaPdfCarichi(DataTable decretazione)
        {
            // Usa il namespace corretto per la risorsa
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Uotep.FileComuni.LetteraAccompagnamento.pdf";
            float startY = 630;
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception($"La risorsa incorporata '{resourceName}' non è stata trovata.");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    using (PdfReader reader = new PdfReader(resourceStream))
                    {
                        using (PdfWriter writer = new PdfWriter(stream))
                        {
                            using (PdfDocument pdf = new PdfDocument(reader, writer))
                            {
                                // =========================================================================
                                // CREA UN UNICO OGGETTO DOCUMENT PER GESTIRE TUTTO
                                // =========================================================================
                                using (Document document = new Document(pdf, PageSize.A4.Rotate()))
                                {


                                    // =========================================================================
                                    // PAGINA 1: LETTERA DI ACCOMPAGNAMENTO
                                    // Il testo viene aggiunto alla prima pagina, che è la carta intestata.
                                    // =========================================================================
                                    document.Add(new Paragraph($"CARICO A.G. EDILIZIA ANNO " + decretazione.Rows[0].ItemArray[2].ToString().Substring(6, 4) + "\n" + "\n")
                                                                    .SetFixedPosition(10, 680, 800)
                                                                    .SetTextAlignment(TextAlignment.LEFT)
                                                                    .SetFontSize(12));

                                 
                                    document.Add(new Paragraph(" - " + decretazione.Rows[0].ItemArray[7].ToString() + " Referente: " + decretazione.Rows[0].ItemArray[6].ToString().ToUpper())
                                        .SetFixedPosition(15, 670, 700)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(12));

                                   

                                    // =========================================================================
                                    // PAGINA 2: REPORT CON TABELLA
                                    // =========================================================================
                                    // document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                    float pageWidth = PageSize.A4.Rotate().GetWidth();
                                    float leftMargin = 36;
                                    float usableWidth = pageWidth - (leftMargin * 2);
                                    float currentY = 550; // Inizia dall'alto della nuova pagina
                                    float lineHeight = 20f;
                                    // --- TITOLI DEL REPORT ---
                                    //document.Add(new Paragraph($"Prot.: PG/" + decretazione.Rows[0].ItemArray[0].ToString() + "/__________________ di " + schede.Rows[0].ItemArray[11].ToString() + " " + schede.Rows[0].ItemArray[12].ToString())
                                    //    .SetFixedPosition(leftMargin, currentY, 500)
                                    //    .SetTextAlignment(TextAlignment.LEFT)
                                    //    .SetFontSize(12));

                                    //document.Add(new Paragraph($"U.O.TUTELA EDILIZIA E PATRIMONIO")
                                    //    .SetFixedPosition(leftMargin, currentY, usableWidth) // Area a tutta larghezza per allineare a destra
                                    //    .SetTextAlignment(TextAlignment.RIGHT)
                                    //    .SetFontSize(12));

                                    currentY -= (lineHeight * 2);

                                    //document.Add(new Paragraph("Riepilogo rifornimento carburante mese di: " + schede.Rows[0].ItemArray[11].ToString() + " " + schede.Rows[0].ItemArray[12].ToString())
                                    //    .SetFixedPosition(leftMargin, currentY, usableWidth)
                                    //    .SetTextAlignment(TextAlignment.CENTER)
                                    //    .SetFontSize(12));

                                    //currentY -= lineHeight;

                                    //document.Add(new Paragraph("AUTO ASSEGNATE AL PERSONALE UOTEP")
                                    //    .SetFixedPosition(leftMargin, currentY, usableWidth)
                                    //    .SetTextAlignment(TextAlignment.CENTER)
                                    //    .SetFontSize(12));

                                    currentY -= 20; // Spazio prima della tabella
                                    
                                    // --- TABELLA ---
                                    const int colonneDati = 7;
                                    const int colonneTotali = colonneDati ;

                                    UnitValue[] columnWidths = new UnitValue[colonneTotali];
                                    columnWidths[0] = UnitValue.CreatePercentValue(5);
                                    for (int j = 1; j < colonneTotali; j++) { columnWidths[j] = UnitValue.CreatePercentValue(95f / colonneDati); }

                                    Table table = new Table(columnWidths);

                                    table.SetMarginTop(150);
                                    // Intestazione
                                    table.AddHeaderCell(new Cell().Add(new Paragraph("#")).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetFontSize(10));
                                    for (int i = 0; i <= colonneDati; i++) // Ciclo corretto: da 1 a 7
                                    {
                                        if (decretazione.Columns[i].ColumnName.ToUpper() != "MACRO_AREA" && decretazione.Columns[i].ColumnName.ToUpper() != "DECR_DECRETATO" && decretazione.Columns[i].ColumnName.ToUpper() != "DECR_unire")
                                        {


                                            table.AddHeaderCell(new Cell().Add(new Paragraph(decretazione.Columns[i].ColumnName.ToUpper())).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER)
                                                .SetPaddingTop(2)    // <-- PADDING SUPERIORE
                                                .SetPaddingBottom(2) // <-- PADDING INFERIORE
                                                );
                                        }
                                    }

                                    // Dati
                                    int contatoreRiga = 1;
                                    foreach (DataRow riga in decretazione.Rows)
                                    {

                                        table.AddCell(new Cell().Add(new Paragraph(contatoreRiga.ToString())).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER));
                                        for (int i = 0; i < riga.ItemArray.Length - 1; i++)
                                        {
                                            object item = riga.ItemArray[i];
                                            string cellText;
                                            if (item == null || item is DBNull) { cellText = ""; }
                                            else if (i == 2) { cellText = item is DateTime ? ((DateTime)item).ToString("dd/MM/yyyy") : item.ToString(); }
                                            //else if (i == 5) { cellText = item is TimeSpan ? ((TimeSpan)item).ToString("hh\\:mm") : item.ToString(); }
                                         //  else if (i == 5) { cellText = item is double ? ((double)item).ToString("N2") : item.ToString(); }
                                            else { cellText = item.ToString(); }
                                            if (i != 6 )
                                            {

                                                if (cellText == "True")
                                                {

                                                    cellText = "SI"; // Sostituisci "true" (stringa) con "si"
                                                }
                                                else if (cellText == "False")
                                                {
                                                    cellText = ""; // Sostituisci "false" (stringa) con "no"
                                                }
                                                table.AddCell(new Cell().Add(new Paragraph(cellText)).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER)
                                                    .SetPaddingTop(2)    // <-- PADDING SUPERIORE RIDOTTO
                                                    .SetPaddingBottom(2) // <-- PADDING INFERIORE RIDOTTO
                                                    .SetHeight(15)
                                                    );
                                            }
                                        }
                                        contatoreRiga++;
                                    }
                                    document.Add(table);

                                    // Firma
                                    //float signatureTextY = 80;
                                   // float signatureLineY = 60;
                                    //document.Add(new Paragraph("Il Responsabile Macro Area").SetFixedPosition(leftMargin, signatureTextY, usableWidth).SetTextAlignment(TextAlignment.RIGHT));
                                    //document.Add(new Paragraph("_______________________").SetFixedPosition(leftMargin, signatureLineY, usableWidth).SetTextAlignment(TextAlignment.RIGHT));


                                    // Rimuovendo SetFixedPosition, il paragrafo si posiziona subito dopo l'ultimo elemento aggiunto
                                    document.Add(new Paragraph("Il Responsabile Macro Area\n\n__________________________________")
                                        .SetMarginTop(30) // Crea uno spazio di sicurezza dai dettagli variabili sopra
                                        .SetTextAlignment(TextAlignment.RIGHT)); 
                                } 
                            }
                        }
                    }

                    // Invia l'output PDF direttamente al browser.
                    byte[] pdfBytes = stream.ToArray();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ContentType = "application/pdf";
                    response.AddHeader("Content-Disposition", "inline; filename=SchedaCarburante.pdf");
                    response.BinaryWrite(pdfBytes);
                    response.Flush();
                    response.End();
                }
            }
        }
        //public void CreaPdfCarichi(DataTable decretazione)
        //{
        //    float startY = 400;
        //    float lineHeight = 20f;
        //    var assembly = Assembly.GetExecutingAssembly();
        //    var resourceName = "Uotep.FileComuni.LetteraAccompagnamento.pdf";



        //    float boxSize = 10;
        //    //float boxVerticalOffset = 4f;
        //    float startX_270 = 270;
        //    float startX_290 = 290;
        //    float startX_70 = 70;
        //    float startX_55 = 55;
        //    float startX_50 = 50;
        //    float startX_400 = 400;
        //    float startX_350 = 350;
        //    float startX_370 = 370;
        //    float startX_450 = 450;
        //    float startX_470 = 470;
        //    float startY_430 = 430;


        //    float lineHeight1 = 30f;


        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        using (PdfWriter writer = new PdfWriter(stream))
        //        {
        //            using (PdfDocument pdf = new PdfDocument(writer))
        //            {
        //                using (Document document = new Document(pdf))
        //                {
        //                    // --- Creazione del Contenuto del Documento ---

        //                    // Titolo
        //                    //DateTime dataIntervento = System.Convert.ToDateTime(schede.Rows[0].ItemArray[2].ToString());
        //                    //string dataFormattata = dataIntervento.ToString("dd/MM/yyyy");

        //                    document.Add(new Paragraph($"CARICO A.G. EDILIZIA ANNO " + decretazione.Rows[0].ItemArray[2].ToString().Substring(6, 4) + " - " + decretazione.Rows[0].ItemArray[6].ToString() + " Referente: " + decretazione.Rows[0].ItemArray[1].ToString().ToUpper())
        //                        .SetFixedPosition(10, 800, 800)
        //                        .SetTextAlignment(TextAlignment.LEFT)
        //                        .SetFontSize(12));

        //                    // Prima riga: Numero Pratica, Nominativo
        //                    document.Add(new Paragraph($"Numero Carico: {decretazione.Rows[0].ItemArray[0]}").SetFixedPosition(10, 780, 500));

        //                    startY -= lineHeight; // Move to the next line



        //                    // --- Posizione di riferimento per "Resa" ---
        //                    // float startX_70_Resa = 70; // Use startX_70 for single column
        //                    float startY_Resa = startY; // Use the dynamic startY




        //                    startY -= lineHeight1; // Move to the next line
        //                    // La PG Operante - Sezione firma
        //                    document.Add(new Paragraph($"La PG Operante").SetFixedPosition(280, startY, 500));
        //                    startY -= lineHeight1; // Move to the next line
        //                    document.Add(new Paragraph($"_______________________/_______________________/_______________________").SetFixedPosition(55, startY, 500));
        //                    //startY -= lineHeight1; // Move to the next line
        //                    //document.Add(new Paragraph($"_______________________").SetFixedPosition(260, startY, 500));

        //                    document.Close(); // Chiude il documento.


        //                }

        //            }
        //        }

        //        // Invia l'output PDF direttamente al browser.
        //        byte[] pdfBytes = stream.ToArray();
        //        HttpResponse response = HttpContext.Current.Response;
        //        response.Clear();
        //        response.ContentType = "application/pdf";
        //        response.AddHeader("Content-Disposition", "inline; filename=SchedaIntervento.pdf");
        //        response.BinaryWrite(pdfBytes);
        //        response.Flush();
        //        response.End();
        //    }


        //}
        public void CreaPdfSchedaCarburanteScampia(DataTable schede)
        {
            // Usa il namespace corretto per la risorsa
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GestUO.FileComuni.LetteraAccompagnamentoScampia.pdf";

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception($"La risorsa incorporata '{resourceName}' non è stata trovata.");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    using (PdfReader reader = new PdfReader(resourceStream))
                    {
                        using (PdfWriter writer = new PdfWriter(stream))
                        {
                            using (PdfDocument pdf = new PdfDocument(reader, writer))
                            {
                                // =========================================================================
                                // CREA UN UNICO OGGETTO DOCUMENT PER GESTIRE TUTTO
                                // =========================================================================
                                using (Document document = new Document(pdf, PageSize.A4.Rotate()))
                                {
                                    // =========================================================================
                                    // PAGINA 1: LETTERA DI ACCOMPAGNAMENTO
                                    // Il testo viene aggiunto alla prima pagina, che è la carta intestata.
                                    // =========================================================================
                                    document.Add(new Paragraph("Alla U.O. Gestione Parco Veicolare")
                                        .SetFixedPosition(360, 500, 300) // Coordinate aggiustate per pagina orizzontale
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(12));

                                    document.Add(new Paragraph("Oggetto: Invio scontrini carburante Fuel Card Mese di " + schede.Rows[0].ItemArray[11].ToString() + " " + schede.Rows[0].ItemArray[12].ToString())
                                        .SetFixedPosition(72, 450, 700)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(12));

                                    document.Add(new Paragraph("In allegato si trasmette l'elenco riepilogativo degli scontrini relativi al mese di " + schede.Rows[0].ItemArray[11].ToString() + " " + schede.Rows[0].ItemArray[12].ToString() + ",\n riguardante n. " + schede.Rows.Count + " rifornimenti di carburante effettuati per le auto assegnate alla U.O. SCAMPIA")
                                        .SetFixedPosition(72, 400, 700)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(10));

                                    document.Add(new Paragraph("Allegati:\n 1) Nr. " + schede.Rows.Count + " scontrini originali + fotocopie degli scontrini\n 2) Elenco riepilogativo rifornimento carburante con allegate fotocopie degli scontrini.")
                                        .SetFixedPosition(72, 300, 700)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(10));

                                    document.Add(new Paragraph("Estensore :  Gruosso Ilaria Ferraro Daniela")
                                        .SetFixedPosition(72, 150, 300)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(8));

                                    document.Add(new Paragraph("p. Il comandante di Reparto\n S.Ten. Pagano Vincenzo\n Gruosso Ilaria\n Ferraro Daniela")
                                        .SetFixedPosition(360, 150, 300)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(10));

                                    // =========================================================================
                                    // PAGINA 2: REPORT CON TABELLA
                                    // =========================================================================
                                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                    float pageWidth = PageSize.A4.Rotate().GetWidth();
                                    float leftMargin = 36;
                                    float usableWidth = pageWidth - (leftMargin * 2);
                                    float currentY = 550; // Inizia dall'alto della nuova pagina
                                    float lineHeight = 20f;

                                    // --- TITOLI DEL REPORT ---
                                    document.Add(new Paragraph($"Prot.: PG/" + schede.Rows[0].ItemArray[12].ToString() + "/__________________ di " + schede.Rows[0].ItemArray[11].ToString() + " " + schede.Rows[0].ItemArray[12].ToString())
                                        .SetFixedPosition(leftMargin, currentY, 500)
                                        .SetTextAlignment(TextAlignment.LEFT)
                                        .SetFontSize(12));

                                    document.Add(new Paragraph($"U.O.SCAMPIA")
                                        .SetFixedPosition(leftMargin, currentY, usableWidth) // Area a tutta larghezza per allineare a destra
                                        .SetTextAlignment(TextAlignment.RIGHT)
                                        .SetFontSize(12));

                                    currentY -= (lineHeight * 2);

                                    document.Add(new Paragraph("Riepilogo rifornimento carburante mese di: " + schede.Rows[0].ItemArray[11].ToString() + " " + schede.Rows[0].ItemArray[12].ToString())
                                        .SetFixedPosition(leftMargin, currentY, usableWidth)
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetFontSize(12));

                                    currentY -= lineHeight;

                                    document.Add(new Paragraph("AUTO ASSEGNATE AL PERSONALE UO SCAMPIA")
                                        .SetFixedPosition(leftMargin, currentY, usableWidth)
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetFontSize(12));

                                    currentY -= 20; // Spazio prima della tabella

                                    // --- TABELLA ---
                                    const int colonneDati = 10; //colonne da stampare
                                    const int colonneTotali = colonneDati + 1; //stampa progressivo del record stampato

                                    UnitValue[] columnWidths = new UnitValue[colonneTotali];
                                    columnWidths[0] = UnitValue.CreatePercentValue(5);
                                    for (int j = 1; j < colonneTotali; j++) { columnWidths[j] = UnitValue.CreatePercentValue(95f / colonneDati); }

                                    Table table = new Table(columnWidths);
                                    //table.SetWidth(UnitValue.CreatePercentValue(30));
                                    //table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                                    //table.SetFixedPosition(leftMargin, currentY, usableWidth); // Posiziona la tabella con coordinate corrette
                                    table.SetMarginTop(80);
                                    // Intestazione
                                    table.AddHeaderCell(new Cell().Add(new Paragraph("#")).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetFontSize(10));
                                    for (int i = 1; i <= colonneDati; i++) // Ciclo corretto: da 1 a 10
                                    {
                                        table.AddHeaderCell(new Cell().Add(new Paragraph(schede.Columns[i].ColumnName.ToUpper())).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetFontSize(10)
                                            .SetPaddingTop(2)    // <-- PADDING SUPERIORE RIDOTTO
                                            .SetPaddingBottom(2) // <-- PADDING INFERIORE RIDOTTO
                                            );
                                    }

                                    // Dati
                                    int contatoreRiga = 1;
                                    foreach (DataRow riga in schede.Rows)
                                    {

                                        table.AddCell(new Cell().Add(new Paragraph(contatoreRiga.ToString())).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER));
                                        for (int i = 1; i < riga.ItemArray.Length - 8; i++) // totale colonne 19 e tolgo le ultime 8
                                        {
                                            object item = riga.ItemArray[i];
                                            string cellText;
                                            if (item == null || item is DBNull) { cellText = ""; }
                                            else if (i == 4) { cellText = item is DateTime ? ((DateTime)item).ToString("dd/MM/yyyy") : item.ToString(); }
                                            else if (i == 5) { cellText = item is TimeSpan ? ((TimeSpan)item).ToString("hh\\:mm") : item.ToString(); }
                                            else if (i == 6) { cellText = item is double ? ((double)item).ToString("N2") : item.ToString(); }
                                            else { cellText = item.ToString(); }
                                            table.AddCell(new Cell().Add(new Paragraph(cellText)).SetFontSize(8)
                                                .SetPaddingTop(2)    // <-- PADDING SUPERIORE RIDOTTO
                                                .SetPaddingBottom(2) // <-- PADDING INFERIORE RIDOTTO
                                                .SetHeight(30)
                                                );

                                        }
                                        contatoreRiga++;
                                    }
                                    document.Add(table);

                                    // Firma
                                    float signatureTextY = 80;
                                    float signatureLineY = 60;
                                    document.Add(new Paragraph("Il Comandante di Reparto").SetFixedPosition(leftMargin, signatureTextY, usableWidth).SetTextAlignment(TextAlignment.RIGHT));
                                    document.Add(new Paragraph("_______________________").SetFixedPosition(leftMargin, signatureLineY, usableWidth).SetTextAlignment(TextAlignment.RIGHT));
                                } // Il blocco 'using document' finisce qui, chiudendo il documento UNA SOLA VOLTA.
                            }
                        }
                    }

                    // Invia l'output PDF direttamente al browser.
                    byte[] pdfBytes = stream.ToArray();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ContentType = "application/pdf";
                    response.AddHeader("Content-Disposition", "inline; filename=SchedaCarburante.pdf");
                    response.BinaryWrite(pdfBytes);
                    response.Flush();
                    response.End();
                }
            }
        }
        public void PagError(String msg, string Session)
        {
            HttpContext.Current.Session["MessaggioErrore"] = msg;
            HttpContext.Current.Session["PaginaChiamante"] = Session;
            string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
            HttpContext.Current.Response.Redirect(url + msg);
        }
    }

}
