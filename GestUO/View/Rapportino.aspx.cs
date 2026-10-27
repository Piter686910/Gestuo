using GestUO.Classi;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static GestUO.Classi.Enumerate;

namespace GestUO
{
    public partial class Rapportino : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        public string Vuser { get; set; }
        public string ruolo { get; set; }
        public string uoSelezionata { get; set; }

        String Area = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        string msg = string.Empty;
        public string TitoloUO { get; set; } = string.Empty;
        [Serializable]
        public class OperatoreDTO
        {
            public string Grado { get; set; }
            public string Nominativo { get; set; }
            public string Matricola { get; set; }
        }
        private List<OperatoreDTO> ListaOperatoriSelezionati
        {
            get
            {
                if (ViewState["ListaOperatoriSelezionati"] == null)
                    ViewState["ListaOperatoriSelezionati"] = new List<OperatoreDTO>();
                return (List<OperatoreDTO>)ViewState["ListaOperatoriSelezionati"];
            }
            set => ViewState["ListaOperatoriSelezionati"] = value;
        }

        // Mantiene in cache la DataTable degli operatori per recuperare i campi singoli senza chiamare il DB
        private DataTable DtOperatoriCache
        {
            get => ViewState["DtOperatoriCache"] as DataTable;
            set => ViewState["DtOperatoriCache"] = value;
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();

                ruolo = Session["ruolo"].ToString();
                uoSelezionata = Session["uo"].ToString();
                switch (uoSelezionata.ToUpper())
                {
                    case "CHIAIA":
                        TitoloUO = " COMPILAZIONE RAPPORTINO DI SERVIZIO U.O. " + uoSelezionata.ToUpper();

                        break;
                    case "SCAMPIA":
                        TitoloUO = " COMPILAZIONE RAPPORTINO DI SERVIZIO U.O. " + uoSelezionata.ToUpper();

                        break;
                    default:
                        break;
                }
            }

            if (!IsPostBack)
            {

                AggiornaRepeaterPattuglia(); // faccio un primo carico per visualizzare il repeater vuoto
                divRepeater.Visible = true;
                // Legge il valore dal Web.config
                // string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                //string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                //ProtocolloLiteral.Text = decodedText;
                TxtDataIntervento.Attributes["placeholder"] = "gg/mm/aaaa";
                txtDataConsegna.Text = DateTime.Now.Date.ToShortDateString();

                CaricaDLL();
            }

        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            Boolean continua = Convalida();
            String MeseCorrente = DateTime.Now.ToString("MMMM");
            String AnnoCorrente = DateTime.Now.ToString("yyyy");
            Manager mn = new Manager();
            //continua = Convalida();
            if (continua)
            {
                Classi.Rapportino rap = new Classi.Rapportino();
                rap.orario = txtOrarioServizio.Text;
                rap.uo=uoSelezionata.ToUpper();

                rap.nota = txtNote.Text;
                rap.attivitaInterna = CkAttivita.Checked;
                rap.appiedata = CkAppiedata.Checked;
                if (!String.IsNullOrEmpty(txtDataConsegna.Text))
                {
                    rap.data_consegna_intervento = System.Convert.ToDateTime(txtDataConsegna.Text);
                }
                rap.dataInserimento = DateTime.Now;
                rap.siglaVettura = DdlSiglaVettura.SelectedItem.Text;
                rap.itinerario = txtItinerario.Text.Trim();


                List<string> componentiPattuglia = new List<string>();
                List<string> elencoMatricole = new List<string>();
                List<string> elencoGrado = new List<string>();

                foreach (RepeaterItem item in rptOperatori.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var spanGrado = (System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("lblGrado");
                        var spanPattuglia = (System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("lblNominativo");
                        var spanMatricola = (System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("lblMatricola");

                        if (spanMatricola != null && !string.IsNullOrWhiteSpace(spanMatricola.InnerText))
                        {
                            string grado = spanGrado?.InnerText.Trim() ?? "";
                            string nominativo = spanPattuglia?.InnerText.Trim() ?? "";
                            string matricola = spanMatricola.InnerText.Trim();


                            elencoGrado.Add(grado);
                            componentiPattuglia.Add(nominativo);
                            elencoMatricole.Add(matricola);
                        }
                    }
                }

                // Risultato concatenato con " / "
                if (elencoMatricole.Count > 0)
                {
                    rap.pattuglia = string.Join(" / ", componentiPattuglia);
                    rap.matricola = string.Join(" / ", elencoMatricole);
                    rap.grado = string.Join(" / ", elencoGrado);

                }
                rap.ContrCds = int.TryParse(txtContrCds.Text, out int val) ? val : 0;
                rap.PvContestatiCds = int.TryParse(txtPvNoContestatiContrCds.Text, out val) ? val : 0;
                rap.PvNoContestatiCds = int.TryParse(txtPvContestatiContrCds.Text, out val) ? val : 0;

                rap.OccupSuoloPub = int.TryParse(txtOccupSuoloPub.Text, out val) ? val : 0;
                rap.PvContestatiOccupSuoloPub = int.TryParse(txtPvContestatiOccupSuoloPub.Text, out val) ? val : 0;
                rap.PvNoContestatiOccupSuoloPub = int.TryParse(txtPvNoContestatiOccupSuoloPub.Text, out val) ? val : 0;

                rap.Autotrasporti = int.TryParse(txtAutotrasporti.Text, out val) ? val : 0;
                rap.PvContestatiAutotrasporti = int.TryParse(txtPvContestatiAutotrasporti.Text, out val) ? val : 0;
                rap.PvNoContestatiAutotrasporti = int.TryParse(txtPvNoContestatiAutotrasporti.Text, out val) ? val : 0;
                rap.TargheEstere = int.TryParse(txtTargheEstere.Text, out val) ? val : 0;
                rap.PvContestatiTargheEstere = int.TryParse(txtPvContestatiTargheEstere.Text, out val) ? val : 0;
                rap.PvNoContestatiTargheEstere = int.TryParse(txtPvNoContestatiTargheEstere.Text, out val) ? val : 0;
                rap.PvNoContestatiTargheEstere = int.TryParse(txtPvNoContestatiTargheEstere.Text, out val) ? val : 0;
                rap.ParcheggiatoriAbusivi = int.TryParse(txtParcheggiatoriAbusivi.Text, out val) ? val : 0;
                rap.PvContestatiParcheggiatoriAbusivi = int.TryParse(txtPvContestatiParcheggiatoriAbusivi.Text, out val) ? val : 0;
                rap.PvNoContestatiParcheggiatoriAbusivi = int.TryParse(txtPvNoContestatiParcheggiatoriAbusivi.Text, out val) ? val : 0;
                rap.ContrAltreForze = int.TryParse(txtContrAltreForze.Text, out val) ? val : 0;
                rap.PvContestatiContrAltreForze = int.TryParse(txtPvContestatiContrAltreForze.Text, out val) ? val : 0;
                rap.PvNoContestatiContrAltreForze = int.TryParse(txtPvNoContestatiContrAltreForze.Text, out val) ? val : 0;
                rap.ContrMicroMobilita = int.TryParse(txtContrMicroMobilita.Text, out val) ? val : 0;
                rap.PvContestatiContrMicroMobilita = int.TryParse(txtPvContestatiContrMicroMobilita.Text, out val) ? val : 0;
                rap.PvNoContestatiContrMicroMobilita = int.TryParse(txtPvNoContestatiContrMicroMobilita.Text, out val) ? val : 0;
                rap.ConferimentoRifiuti = int.TryParse(txtConferimentoRifiuti.Text, out val) ? val : 0;
                rap.SanzioniConferimentoRifiuti = int.TryParse(txtSanzioniConferimentoRifiuti.Text, out val) ? val : 0;
                rap.DeiezioneCanina = int.TryParse(txtDeiezioneCanina.Text, out val) ? val : 0;
                rap.SanzioniDeiezioneCanina = int.TryParse(txtSanzioniDeiezioneCanina.Text, out val) ? val : 0;
                rap.LeggeAntifumo = int.TryParse(txtLeggeAntifumo.Text, out val) ? val : 0;
                rap.SanzioniLeggeAntifumo = int.TryParse(txtSanzioniLeggeAntifumo.Text, out val) ? val : 0;
                rap.AreaMercatale = int.TryParse(txtAreaMercatale.Text, out val) ? val : 0;
                rap.SanzioniAreaMercatale = int.TryParse(txtSanzioniAreaMercatale.Text, out val) ? val : 0;
                rap.InquinamentoAcustico = int.TryParse(txtInquinamentoAcustico.Text, out val) ? val : 0;
                rap.SanzioniInquinamentoAcustico = int.TryParse(txtSanzioniInquinamentoAcustico.Text, out val) ? val : 0;
                rap.ContrCantieriSeq = int.TryParse(txtContrCantieriSeq.Text, out val) ? val : 0;
                rap.SanzioniContrCantieriSeq = int.TryParse(txtSanzioniContrCantieriSeq.Text, out val) ? val : 0;
                rap.LavoriSenzaProtez = int.TryParse(txtLavoriSenzaProtez.Text, out val) ? val : 0;
                rap.SanzioniLavoriSenzaProtez = int.TryParse(txtSanzioniLavoriSenzaProtez.Text, out val) ? val : 0;
                rap.NotificheSicurezza = int.TryParse(txtNotificheSicurezza.Text, out val) ? val : 0;
                rap.SanzioniNotificheSicurezza = int.TryParse(txtSanzioniNotificheSicurezza.Text, out val) ? val : 0;
                rap.Ambulanti = int.TryParse(txtAmbulanti.Text, out val) ? val : 0;
                rap.SanzioniAmbulanti = int.TryParse(txtSanzioniAmbulanti.Text, out val) ? val : 0;
                rap.ProvenienzaAlimentare = int.TryParse(txtProvenienzaAlimentare.Text, out val) ? val : 0;
                rap.SanzioniProvenienzaAlimentare = int.TryParse(txtSanzioniProvenienzaAlimentare.Text, out val) ? val : 0;
                rap.TrasportoAlimentare = int.TryParse(txtTrasportoAlimentare.Text, out val) ? val : 0;
                rap.SanzioniTrasportoAlimentare = int.TryParse(txtSanzioniTrasportoAlimentare.Text, out val) ? val : 0;
                rap.Officine = int.TryParse(txtOfficine.Text, out val) ? val : 0;
                rap.SanzioniOfficine = int.TryParse(txtSanzioniOfficine.Text, out val) ? val : 0;
                rap.ContrBnB = int.TryParse(txtContrBnB.Text, out val) ? val : 0;
                rap.SanzioniContrBnB = int.TryParse(txtSanzioniContrBnB.Text, out val) ? val : 0;
               
                Int32 idN = 0;
                 Boolean resp = mn.InsRapportino(rap, out idN);
                SiteMaster myMaster = this.Master as SiteMaster;
                if (!resp)
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica scheda non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    HfIdScheda.Value = idN.ToString();
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#MsgStampa').text('" + Enumerate.MsgOutput.VuoiStampa.GetDescription() + "'); $('#PopStampa').modal('show');", true);
                    
                    Pulisci();
                }
            }
        }

        protected void CkAttivita_CheckedChanged1(object sender, EventArgs e)
        {
            //ToggleDivControls(divTesta, CkAttivita.Checked);
            ToggleDivControls(divDettagli, CkAttivita.Checked);

        }
        private void Pulisci()
        {
            try
            {


                TxtDataIntervento.Text = string.Empty;
                txtOrarioServizio.Text = string.Empty;
                txtNote.Text = string.Empty;
                txtDataConsegna.Text = string.Empty;
                //  LPattugliaCompleta.Items.Clear();
                rptOperatori.DataSource = null;
                rptOperatori.DataBind(); 
                //ckNotificaTp.Checked = false;
                DdlSiglaVettura.ClearSelection();
            }
            catch (Exception ex)
            {

                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in pulisci() rapportino.cs ");
                    sw.Close();
                }
            }
        }
        private Boolean Convalida()
        {
            Boolean ret = true;
            //if (String.IsNullOrEmpty(LPattugliaCompleta.ToString()))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserire la pattuglia." + "'); $('#errorModal').modal('show');", true);

            //    ret = false;

            //}
            //if (ckEsposto.Checked && String.IsNullOrEmpty(txt_numEspostiSegn.Text))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Se hai selezionato esposti è necessario inserire il numero di esposti." + "'); $('#errorModal').modal('show');", true);

            //    ret = false;
            //}
            //if (!ckEsposto.Checked && !String.IsNullOrEmpty(txt_numEspostiSegn.Text))
            //{
            //    int num = System.Convert.ToInt32(txt_numEspostiSegn.Text);
            //    if (num>0)

            //        ckEsposto.Checked = true;


            //}
            //if (ckAccertAvvenutoRipr.Checked == true)
            //{
            //    if (rdTotale.Checked == false && rdParziale.Checked == false && rdNonAvvenuto.Checked == false)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare Totale o Parziale o Non Avvenuto ." + "'); $('#errorModal').modal('show');", true);

            //        ret = false;
            //    }
            //}
            //if (ckControlliLavoriEdiliSenzaProt.Checked && (rdCon.Checked == false && rdSenza.Checked == false))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare Con o Senza." + "'); $('#errorModal').modal('show');", true);

            //    ret = false;
            //}
            //if (ckSgomberi.Checked == true)
            //{
            //    if (CkSgombAbusiva.Checked == false && CkSgombImmobili.Checked == false)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare una tipologia di sgombero." + "'); $('#errorModal').modal('show');", true);

            //        ret = false;
            //    }
            //}
            //if (ckControlliOccupazioneAbus.Checked == true)
            //{
            //    if (ckAbitativo.Checked == false && ckNonAbitativo.Checked == false)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare una tipologia di occupazione." + "'); $('#errorModal').modal('show');", true);

            //        ret = false;
            //    }
            //}
            ////I- mod 31/01/2026 scheda int
            //if (ckNotificaTp.Checked == true)
            //{
            //    if (String.IsNullOrEmpty(txtNumNotificheNoAg.Text))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserire numero notifiche no AG." + "'); $('#errorModal').modal('show');", true);

            //        ret = false;
            //    }
            //}
            //if (ckAccRichiesti.Checked == true)
            //{
            //    if (String.IsNullOrEmpty(txtNumAccRichiesti.Text))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserire numero accertamenti." + "'); $('#errorModal').modal('show');", true);

            //        ret = false;
            //    }
            //}
            //if (ckContrNatoDaAccert.Checked == true)
            //{
            //    if (String.IsNullOrEmpty(txtNumContrNatoDaAccert.Text))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserire numero controlli noto/i da accertamenti." + "'); $('#errorModal').modal('show');", true);

            //        ret = false;
            //    }
            //}

            //F- mod 31/01/2026 scheda int
            return ret;
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }
        private void CaricaDLL()
        {
            try
            {

                //DataTable CaricaOperatori = mn.getListOperatore(uoSelezionata,out msg);
                //DdlPattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                //DdlPattuglia.DataTextField = "nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "matricola";
                //DdlPattuglia.DataValueField = "grado";

                //DdlPattuglia.Items.Insert(0, new ListItem("", "0"));
                //DdlPattuglia.DataBind();
                //DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                string uoSelezionata = Session["UO"]?.ToString() ?? "0"; // Adatta alla tua variabile UO
                DataTable dt = mn.getListOperatore(uoSelezionata, out string msg);

                DtOperatoriCache = dt; // Salva la tabella in ViewState

                DdlPattuglia.Items.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DdlPattuglia.DataSource = dt;
                    DdlPattuglia.DataTextField = "DisplayMember"; // Mostra: "Ispettore - Mario Rossi (57097)"
                    DdlPattuglia.DataValueField = "matricola";     // Valore univo: "57097"
                    DdlPattuglia.DataBind();
                }

                // Inserimento elemento predefinito DOPO il DataBind
                DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un operatore --", "0"));


                DataTable RicercaVettura = mn.getSiglaAuto(uoSelezionata);
                DdlSiglaVettura.DataSource = RicercaVettura; // Imposta il DataSource della DropDownList
                DdlSiglaVettura.DataTextField = "sigla"; // Il campo visibile

                DdlSiglaVettura.DataBind();
                DdlSiglaVettura.Items.Insert(0, new ListItem("", "0"));


            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl file inserimento.cs ");
                    sw.Close();
                }
            }
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                string selectedValue = e.CommandArgument.ToString();

                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                //DdlQuartiere.SelectedItem.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }

        protected void Aggiungi_Click(object sender, EventArgs e)
        {
            string matricolaSelezionata = DdlPattuglia.SelectedValue;

            if (matricolaSelezionata != "0" && DtOperatoriCache != null)
            {
                // Cerca i dati completi dell'operatore selezionato nella DataTable memorizzata
                DataRow[] rows = DtOperatoriCache.Select($"matricola = '{matricolaSelezionata}'");

                if (rows.Length > 0)
                {
                    DataRow r = rows[0];

                    List<OperatoreDTO> lista = ListaOperatoriSelezionati;

                    // Controllo per evitare di aggiungere due volte lo stesso operatore
                    if (!lista.Any(x => x.Matricola == matricolaSelezionata))
                    {
                        lista.Add(new OperatoreDTO
                        {
                            Grado = r["grado"]?.ToString() ?? "",
                            Nominativo = r["nominativo"]?.ToString() ?? "",
                            Matricola = r["matricola"]?.ToString() ?? ""
                        });

                        ListaOperatoriSelezionati = lista;
                        AggiornaRepeaterPattuglia();

                        // Resetta la selezione nella DropDownList
                        DdlPattuglia.SelectedIndex = 0;
                    }
                }
            }
            // Crea un nuovo ListViewDataItem con un valore di esempio
            //ListViewDataItem itemToAdd = new ListViewDataItem(0, 0);
            //itemToAdd.DataItem = DdlPattuglia.SelectedItem.Text;

            //// Verifica se l'elemento è già presente nella ListView
            //bool itemExists = false;

            //foreach (var item in LPattugliaCompleta.Items)
            //{
            //    if (item.ToString() == itemToAdd.DataItem.ToString())
            //    {
            //        itemExists = true;
            //        break;
            //    }
            //}

            //// Aggiungi l'elemento solo se non esiste già
            //if (!itemExists)
            //{
            //    LPattugliaCompleta.Items.Add(DdlPattuglia.SelectedItem.Text);
            //}

        }
        private void AggiornaRepeaterPattuglia()
        {
            rptOperatori.DataSource = ListaOperatoriSelezionati;
            rptOperatori.DataBind();
        }


        //protected void btStampa_Click(object sender, EventArgs e)
        //{
        //    // Percorso del file RDLC
        //    string reportPath = @"C:\Uotep\Report.rdlc";

        //    // Percorso del file PDF da creare
        //    string pdfPath = @"C:\UotepOutput\Report.pdf";
        //    try
        //    {
        //        // Configura il ReportViewer
        //        LocalReport report = new LocalReport();
        //        report.ReportPath = reportPath;

        //        // Aggiungi il DataSource (sostituisci con i tuoi dati)
        //        report.DataSources.Add(new ReportDataSource("DataSet1", GetSampleData(txtPratica.Text, null, null, false)));

        //        // Esporta il report in PDF
        //        ExportToPdf(report, pdfPath);

        //        Console.WriteLine($"PDF generato con successo: {pdfPath}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Errore: {ex.Message}");
        //    }
        //}
        // Metodo per creare dati di esempio (da sostituire con i dati reali)
        //static DataTable GetSampleData(string nrPratica, string pattuglia, string dat, Boolean at)
        //{
        //    Manager mn = new Manager();
        //    //DataTable schede =
        //    DataTable table = mn.GetSchedeBy(nrPratica, null, null, at, 0, null);
        //    if (table.Rows.Count > 0)
        //    {
        //        //GVRicecaScheda.DataSource = table;
        //        //GVRicecaScheda.DataBind();



        //        table.Columns.Add("Pratica", typeof(string));
        //        table.Columns.Add("Data", typeof(string));
        //        table.Columns.Add("Nominativo", typeof(int));

        //        table.Rows.Add(table.Rows[0].ItemArray[0], table.Rows[0].ItemArray[1], 30);
        //        //table.Rows.Add("Mario", "Rossi", 30);
        //        //table.Rows.Add("Luigi", "Verdi", 25);
        //        //table.Rows.Add("Anna", "Bianchi", 28);
        //    }
        //    return table;

        //}
        // Metodo per esportare il report in PDF
        //static void ExportToPdf(LocalReport report, string filePath)
        //{
        //    string mimeType, encoding, fileNameExtension;
        //    Warning[] warnings;
        //    string[] streamIds;

        //    // Renderizza il report in formato PDF
        //    byte[] bytes = report.Render(
        //        "PDF", null, out mimeType, out encoding, out fileNameExtension, out streamIds, out warnings);

        //    // Salva il file PDF
        //    using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //    {
        //        fs.Write(bytes, 0, bytes.Length);
        //    }
        //}

        //protected void btPopStampa_Click(object sender, EventArgs e)
        //{
        //    int id = System.Convert.ToInt32(HfIdScheda.Value);
        //    //I- mod 01/04/2026 scheda int aggiunto print e numeri abusi 
        //    mn.UpdRappUoteStampa(id);
        //    //F- mod 01/04/2026 scheda int aggiunto print e numeri abusi 
        //    DataTable schede = mn.GetSchedeBy(null, null, null, CkAttivita.Checked, id, null);

        //    Routine stampa = new Routine();
        //    stampa.CreaPdf(schede);
        //}

        private void ToggleDivControls(Control container, bool isEnabled)
        {
            foreach (Control control in container.Controls)
            {
                // Verifica se il controllo è un WebControl (TextBox, Button, DropDownList, ecc.)
                if (control is WebControl webControl)
                {
                    webControl.Enabled = !isEnabled;
                }

                // Se il controllo ha figli, chiama ricorsivamente la funzione
                if (control.HasControls())
                {
                    ToggleDivControls(control, !isEnabled);
                }
            }
        }

        protected void CkAppiedata_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDivControls(divDettagli, CkAttivita.Checked);
        }

        protected void rptOperatori_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptOperatori_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string matricolaDaRimuovere = e.CommandArgument.ToString();

                // Recupera la lista corrente dal ViewState
                List<OperatoreDTO> lista = ListaOperatoriSelezionati;

                // Rimuove l'operatore con la matricola corrispondente
                lista.RemoveAll(x => x.Matricola == matricolaDaRimuovere);

                // Aggiorna la lista memorizzata nel ViewState
                ListaOperatoriSelezionati = lista;

                // Riesegue il bind per aggiornare l'interfaccia utente
                AggiornaRepeaterPattuglia();
            }
        }
    }

}