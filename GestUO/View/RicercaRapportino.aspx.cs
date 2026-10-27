using AjaxControlToolkit;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;
using GestUO.Classi;
using static System.Windows.Forms.AxHost;
using static GestUO.Classi.Enumerate;
using ListItem = System.Web.UI.WebControls.ListItem;
using DocumentFormat.OpenXml.Wordprocessing;
using Control = System.Web.UI.Control;



namespace GestUO
{
    public partial class RicercaRapportino : Page
    {

        public string Vuser { get; set; }
        public string ruolo { get; set; }
        public string uoSelezionata { get; set; }

        string msg = String.Empty;
        public int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                    return 0;
                return (int)ViewState["CurrentPage"];
            }
            set { ViewState["CurrentPage"] = value; }
        }


        [Serializable]
        public class OperatoreDTO
        {
            public string Grado { get; set; }
            public string Nominativo { get; set; }
            public string Matricola { get; set; }
        }
        String annoCorr = DateTime.Now.Year.ToString();
        //String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        //String profilo = string.Empty;
        //String ruolo = string.Empty;
        public string TitoloUo { get; set; } = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                uoSelezionata = Session["uo"].ToString();
                ruolo = Session["ruolo"].ToString();
                switch (uoSelezionata.ToUpper())
                {
                    case "CHIAIA":
                        TitoloUo = " COMPILAZIONE RAPPORTINO DI SERVIZIO U.O. " + uoSelezionata.ToUpper();

                        break;
                    case "SCAMPIA":
                        TitoloUo = " COMPILAZIONE RAPPORTINO DI SERVIZIO U.O. " + uoSelezionata.ToUpper();
                        break;
                    default:
                        break;
                }
            }


            if (!IsPostBack)
            {
                string idRicevuto = Request.QueryString["idscheda"];

                if (!string.IsNullOrEmpty(idRicevuto))
                {

                    DataTable scheda = mn.GetSchedaById(idRicevuto, uoSelezionata);
                    if (scheda.Rows.Count > 0)
                    {
                        CaricaDLL();
                        FillScheda(scheda);

                        SetControlsEnabled(divDettagli, false);
                        SetControlsEnabled(divTesta, false);

                    }
                    return;

                }


                //effettua autofocus sul popup modale
                string script = "$('#ModalRicerca').modal('show');";
                // 2. Aggiungi il focus con un ritardo di 500ms (tempo dell'animazione)
                // Sostituisci 'txtNome' con l'ID (Statico) o ClientID della tua textbox
                script += " setTimeout(function(){ document.getElementById('" + txtModPratica.ClientID + "').focus(); }, 500);";

                // 3. Esegui
                ScriptManager.RegisterStartupScript(this, GetType(), "ApriEFocus", script, true);
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica scheda non riuscito, controllare il log." + "'); $('#ModalRicerca').modal('show');", true);

                Session["popApertoRicercaScheda"] = "si";
                SetControlsEnabled(divDettagli, false);
                SetControlsEnabled(divTesta, false);
                //SetControlsVisible(divDDLPattuglia, false);
                //SetControlsVisible(divPattuglia, true);
                btStampa.Enabled = true;
                btModificaScheda.Enabled = true;
                CaricaDLL();
            }


        }
        /// <summary>
        /// abilita/disabilita gli oggetti del div
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isEnabled"></param>
        protected void SetControlsEnabled(Control container, bool isEnabled)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is WebControl webCtrl)
                {

                    webCtrl.Enabled = isEnabled;
                }

                // Se il controllo ha figli, applica la ricorsione
                if (ctrl.HasControls())
                {
                    SetControlsEnabled(ctrl, isEnabled);
                }
            }
        }
        /// <summary>
        /// visualizzo/non visualizzo gli oggetti del div
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isEnabled"></param>
        protected void SetControlsVisible(Control container, bool isEnabled)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is WebControl webCtrl)
                {
                    webCtrl.Visible = isEnabled;
                }

                // Se il controllo ha figli, applica la ricorsione
                if (ctrl.HasControls())
                {
                    SetControlsVisible(ctrl, isEnabled);
                }
            }
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
            Session["popApertoRicercaScheda"] = "si";
        }


        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

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
                //string selectedValue = e.CommandArgument.ToString();


                string[] args = e.CommandArgument.ToString().Split(';');
                string itinerario = args[3];
                string orario = args[1];
                string pattuglia = args[0];
                string id_scheda = args[2];
                HfIdScheda.Value = System.Convert.ToInt32(id_scheda).ToString();
                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                //txtPratica.Text = numeroP;
                //txtNominativo.Text = nominativo;
                //txtPattuglia.Text = pattuglia;
                //string[] columns = pattuglia.Split('/');

                //for (int i = 0; i < columns.Length; i++)
                //{
                //    LPattugliaCompleta.Items.Add(columns[i]); // Colonne successive
                //}
                CaricaDLL();
                Manager mn = new Manager();
                //                DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

                DataTable scheda = mn.GetSchedaById(id_scheda, uoSelezionata);
                if (scheda.Rows.Count > 0)
                {
                    FillScheda(scheda);

                }
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#ModalRicerca').modal('hide');", true);
            }
        }

        protected void FillScheda(DataTable rap)
        {
            string[] ArOperatori = rap.Rows[0]["pattuglia"].ToString().Split('/');
            string[] ArGrado = rap.Rows[0]["grado"].ToString().Split('/');
            string[] ArMatricola = rap.Rows[0]["matricola"].ToString().Split('/');
            List<OperatoreDTO> listaOperatori = new List<OperatoreDTO>();

            for (int i = 0; i < ArOperatori.Length; i++)
            {
                listaOperatori.Add(new OperatoreDTO
                {
                    Grado = ArGrado[i],
                    Nominativo = ArOperatori[i],
                    Matricola = ArMatricola[i]
                });
            }

            rptOperatori.DataSource = listaOperatori;
            rptOperatori.DataBind();

            if (!string.IsNullOrEmpty(rap.Rows[0]["dataInserimento"].ToString()))
            {
                DateTime dataIntervento = System.Convert.ToDateTime(rap.Rows[0]["dataInserimento"].ToString()); // Recupera la data dal DataTable
                TxtDataIntervento.Text = dataIntervento.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            }

            CkAppiedata.Checked = System.Convert.ToBoolean(rap.Rows[0]["appiedata"]);
            CkAttivita.Checked = System.Convert.ToBoolean(rap.Rows[0]["attivitaInterna"]);
            string siglaDb = rap.Rows[0]["siglaVettura"]?.ToString() ?? string.Empty;

            ListItem voceSigla = DdlSiglaVettura.Items.FindByText(siglaDb); // Oppure FindByValue(siglaDb)

            if (voceSigla != null)
            {
                DdlSiglaVettura.ClearSelection(); // Pulisce eventuali selezioni precedenti
                voceSigla.Selected = true;        // Seleziona la voce trovata
            }
            else
            {
                DdlSiglaVettura.SelectedIndex = 0; // Se non la trova, imposta la prima di default
            }
            txtNote.Text = rap.Rows[0]["nota"].ToString();

            if (!string.IsNullOrEmpty(rap.Rows[0]["dataConsegna"].ToString()))
            {
                DateTime dataConsegna = System.Convert.ToDateTime(rap.Rows[0]["dataConsegna"].ToString()); // Recupera la data dal DataTable
                txtDataConsegna.Text = dataConsegna.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            }
            txtItinerario.Text = rap.Rows[0]["itinerario"].ToString();
            txtOrarioServizio.Text = (rap.Rows[0]["orario"] != DBNull.Value && rap.Rows[0]["orario"] != null && !string.IsNullOrEmpty(rap.Rows[0]["orario"].ToString())) ? rap.Rows[0]["orario"].ToString() : string.Empty;
            txtContrCds.Text = (rap.Rows[0]["ContrCds"] != DBNull.Value && rap.Rows[0]["ContrCds"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ContrCds"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ContrCds"]).ToString() : string.Empty;
            txtPvContestatiContrCds.Text = (rap.Rows[0]["PvContestatiCds"] != DBNull.Value && rap.Rows[0]["PvContestatiCds"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiCds"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiCds"]).ToString() : string.Empty;
            txtPvNoContestatiContrCds.Text = (rap.Rows[0]["PvNoContestatiCds"] != DBNull.Value && rap.Rows[0]["PvNoContestatiCds"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiCds"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiCds"]).ToString() : string.Empty;
            txtOccupSuoloPub.Text = (rap.Rows[0]["OccupSuoloPub"] != DBNull.Value && rap.Rows[0]["OccupSuoloPub"] != null && !string.IsNullOrEmpty(rap.Rows[0]["OccupSuoloPub"].ToString())) ? Convert.ToInt32(rap.Rows[0]["OccupSuoloPub"]).ToString() : string.Empty;
            txtPvContestatiOccupSuoloPub.Text = (rap.Rows[0]["PvContestatiOccupSuoloPub"] != DBNull.Value && rap.Rows[0]["PvContestatiOccupSuoloPub"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiOccupSuoloPub"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiOccupSuoloPub"]).ToString() : string.Empty;
            txtPvNoContestatiOccupSuoloPub.Text = (rap.Rows[0]["PvNoContestatiOccupSuoloPub"] != DBNull.Value && rap.Rows[0]["PvContestatiOccupSuoloPub"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiOccupSuoloPub"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiOccupSuoloPub"]).ToString() : string.Empty;
            txtAutotrasporti.Text = (rap.Rows[0]["Autotrasporti"] != DBNull.Value && rap.Rows[0]["Autotrasporti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["Autotrasporti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["Autotrasporti"]).ToString() : string.Empty;
            txtPvContestatiAutotrasporti.Text = (rap.Rows[0]["PvContestatiAutotrasporti"] != DBNull.Value && rap.Rows[0]["PvContestatiAutotrasporti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiAutotrasporti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiAutotrasporti"]).ToString() : string.Empty;
            txtPvNoContestatiAutotrasporti.Text = (rap.Rows[0]["PvNoContestatiAutotrasporti"] != DBNull.Value && rap.Rows[0]["PvNoContestatiAutotrasporti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiAutotrasporti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiAutotrasporti"]).ToString() : string.Empty;
            txtTargheEstere.Text = (rap.Rows[0]["TargheEstere"] != DBNull.Value && rap.Rows[0]["TargheEstere"] != null && !string.IsNullOrEmpty(rap.Rows[0]["TargheEstere"].ToString())) ? Convert.ToInt32(rap.Rows[0]["TargheEstere"]).ToString() : string.Empty;
            txtPvContestatiTargheEstere.Text = (rap.Rows[0]["PvContestatiTargheEstere"] != DBNull.Value && rap.Rows[0]["PvContestatiTargheEstere"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiTargheEstere"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiTargheEstere"]).ToString() : string.Empty;
            txtPvNoContestatiTargheEstere.Text = (rap.Rows[0]["PvNoContestatiTargheEstere"] != DBNull.Value && rap.Rows[0]["PvNoContestatiTargheEstere"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiTargheEstere"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiTargheEstere"]).ToString() : string.Empty;
            txtParcheggiatoriAbusivi.Text = (rap.Rows[0]["ParcheggiatoriAbusivi"] != DBNull.Value && rap.Rows[0]["ParcheggiatoriAbusivi"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ParcheggiatoriAbusivi"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ParcheggiatoriAbusivi"]).ToString() : string.Empty;
            txtPvContestatiParcheggiatoriAbusivi.Text = (rap.Rows[0]["PvContestatiParcheggiatoriAbusivi"] != DBNull.Value && rap.Rows[0]["PvContestatiParcheggiatoriAbusivi"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiParcheggiatoriAbusivi"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiParcheggiatoriAbusivi"]).ToString() : string.Empty;
            txtPvNoContestatiParcheggiatoriAbusivi.Text = (rap.Rows[0]["PvNoContestatiParcheggiatoriAbusivi"] != DBNull.Value && rap.Rows[0]["PvNoContestatiParcheggiatoriAbusivi"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiParcheggiatoriAbusivi"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiParcheggiatoriAbusivi"]).ToString() : string.Empty;
            txtContrAltreForze.Text = (rap.Rows[0]["ContrAltreForze"] != DBNull.Value && rap.Rows[0]["ContrAltreForze"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ContrAltreForze"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ContrAltreForze"]).ToString() : string.Empty;
            txtPvContestatiContrAltreForze.Text = (rap.Rows[0]["PvContestatiContrAltreForze"] != DBNull.Value && rap.Rows[0]["PvContestatiContrAltreForze"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiContrAltreForze"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiContrAltreForze"]).ToString() : string.Empty;
            txtPvNoContestatiContrAltreForze.Text = (rap.Rows[0]["PvNoContestatiContrAltreForze"] != DBNull.Value && rap.Rows[0]["PvNoContestatiContrAltreForze"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiContrAltreForze"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiContrAltreForze"]).ToString() : string.Empty;
            txtContrMicroMobilita.Text = (rap.Rows[0]["ContrMicroMobilita"] != DBNull.Value && rap.Rows[0]["ContrMicroMobilita"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ContrMicroMobilita"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ContrMicroMobilita"]).ToString() : string.Empty;
            txtPvContestatiContrMicroMobilita.Text = (rap.Rows[0]["PvContestatiContrMicroMobilita"] != DBNull.Value && rap.Rows[0]["PvContestatiContrMicroMobilita"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvContestatiContrMicroMobilita"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvContestatiContrMicroMobilita"]).ToString() : string.Empty;
            txtPvNoContestatiContrMicroMobilita.Text = (rap.Rows[0]["PvNoContestatiContrMicroMobilita"] != DBNull.Value && rap.Rows[0]["PvNoContestatiContrMicroMobilita"] != null && !string.IsNullOrEmpty(rap.Rows[0]["PvNoContestatiContrMicroMobilita"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiContrMicroMobilita"]).ToString() : string.Empty;
            txtConferimentoRifiuti.Text = (rap.Rows[0]["ConferimentoRifiuti"] != DBNull.Value && rap.Rows[0]["ConferimentoRifiuti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ConferimentoRifiuti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ConferimentoRifiuti"]).ToString() : string.Empty;
            txtSanzioniConferimentoRifiuti.Text = (rap.Rows[0]["SanzioniConferimentoRifiuti"] != DBNull.Value && rap.Rows[0]["SanzioniConferimentoRifiuti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniConferimentoRifiuti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["PvNoContestatiCds"]).ToString() : string.Empty;
            txtDeiezioneCanina.Text = (rap.Rows[0]["DeiezioneCanina"] != DBNull.Value && rap.Rows[0]["DeiezioneCanina"] != null && !string.IsNullOrEmpty(rap.Rows[0]["DeiezioneCanina"].ToString())) ? Convert.ToInt32(rap.Rows[0]["DeiezioneCanina"]).ToString() : string.Empty;
            txtSanzioniDeiezioneCanina.Text = (rap.Rows[0]["SanzioniDeiezioneCanina"] != DBNull.Value && rap.Rows[0]["SanzioniDeiezioneCanina"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniDeiezioneCanina"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniDeiezioneCanina"]).ToString() : string.Empty;
            txtLeggeAntifumo.Text = (rap.Rows[0]["LeggeAntifumo"] != DBNull.Value && rap.Rows[0]["LeggeAntifumo"] != null && !string.IsNullOrEmpty(rap.Rows[0]["LeggeAntifumo"].ToString())) ? Convert.ToInt32(rap.Rows[0]["LeggeAntifumo"]).ToString() : string.Empty;
            txtSanzioniLeggeAntifumo.Text = (rap.Rows[0]["SanzioniLeggeAntifumo"] != DBNull.Value && rap.Rows[0]["SanzioniLeggeAntifumo"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniLeggeAntifumo"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniLeggeAntifumo"]).ToString() : string.Empty;
            txtAreaMercatale.Text = (rap.Rows[0]["AreaMercatale"] != DBNull.Value && rap.Rows[0]["AreaMercatale"] != null && !string.IsNullOrEmpty(rap.Rows[0]["AreaMercatale"].ToString())) ? Convert.ToInt32(rap.Rows[0]["AreaMercatale"]).ToString() : string.Empty;
            txtSanzioniAreaMercatale.Text = (rap.Rows[0]["SanzioniAreaMercatale"] != DBNull.Value && rap.Rows[0]["SanzioniAreaMercatale"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniAreaMercatale"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniAreaMercatale"]).ToString() : string.Empty;
            txtInquinamentoAcustico.Text = (rap.Rows[0]["InquinamentoAcustico"] != DBNull.Value && rap.Rows[0]["InquinamentoAcustico"] != null && !string.IsNullOrEmpty(rap.Rows[0]["InquinamentoAcustico"].ToString())) ? Convert.ToInt32(rap.Rows[0]["InquinamentoAcustico"]).ToString() : string.Empty;
            txtSanzioniInquinamentoAcustico.Text = (rap.Rows[0]["SanzioniInquinamentoAcustico"] != DBNull.Value && rap.Rows[0]["SanzioniInquinamentoAcustico"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniInquinamentoAcustico"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniInquinamentoAcustico"]).ToString() : string.Empty;
            txtContrCantieriSeq.Text = (rap.Rows[0]["ContrCantieriSeq"] != DBNull.Value && rap.Rows[0]["ContrCantieriSeq"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ContrCantieriSeq"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ContrCantieriSeq"]).ToString() : string.Empty;
            txtSanzioniContrCantieriSeq.Text = (rap.Rows[0]["SanzioniContrCantieriSeq"] != DBNull.Value && rap.Rows[0]["SanzioniContrCantieriSeq"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniContrCantieriSeq"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniContrCantieriSeq"]).ToString() : string.Empty;
            txtLavoriSenzaProtez.Text = (rap.Rows[0]["LavoriSenzaProtez"] != DBNull.Value && rap.Rows[0]["LavoriSenzaProtez"] != null && !string.IsNullOrEmpty(rap.Rows[0]["LavoriSenzaProtez"].ToString())) ? Convert.ToInt32(rap.Rows[0]["LavoriSenzaProtez"]).ToString() : string.Empty;
            txtSanzioniLavoriSenzaProtez.Text = (rap.Rows[0]["SanzioniLavoriSenzaProtez"] != DBNull.Value && rap.Rows[0]["SanzioniLavoriSenzaProtez"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniLavoriSenzaProtez"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniLavoriSenzaProtez"]).ToString() : string.Empty;
            txtNotificheSicurezza.Text = (rap.Rows[0]["NotificheSicurezza"] != DBNull.Value && rap.Rows[0]["NotificheSicurezza"] != null && !string.IsNullOrEmpty(rap.Rows[0]["NotificheSicurezza"].ToString())) ? Convert.ToInt32(rap.Rows[0]["NotificheSicurezza"]).ToString() : string.Empty;
            txtSanzioniNotificheSicurezza.Text = (rap.Rows[0]["SanzioniNotificheSicurezza"] != DBNull.Value && rap.Rows[0]["SanzioniNotificheSicurezza"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniNotificheSicurezza"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniNotificheSicurezza"]).ToString() : string.Empty;
            txtAmbulanti.Text = (rap.Rows[0]["Ambulanti"] != DBNull.Value && rap.Rows[0]["Ambulanti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["Ambulanti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["Ambulanti"]).ToString() : string.Empty;
            txtSanzioniAmbulanti.Text = (rap.Rows[0]["SanzioniAmbulanti"] != DBNull.Value && rap.Rows[0]["SanzioniAmbulanti"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniAmbulanti"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniAmbulanti"]).ToString() : string.Empty;
            txtProvenienzaAlimentare.Text = (rap.Rows[0]["ProvenienzaAlimentare"] != DBNull.Value && rap.Rows[0]["ProvenienzaAlimentare"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ProvenienzaAlimentare"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ProvenienzaAlimentare"]).ToString() : string.Empty;
            txtSanzioniProvenienzaAlimentare.Text = (rap.Rows[0]["SanzioniProvenienzaAlimentare"] != DBNull.Value && rap.Rows[0]["SanzioniProvenienzaAlimentare"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniProvenienzaAlimentare"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniProvenienzaAlimentare"]).ToString() : string.Empty;
            txtTrasportoAlimentare.Text = (rap.Rows[0]["TrasportoAlimentare"] != DBNull.Value && rap.Rows[0]["TrasportoAlimentare"] != null && !string.IsNullOrEmpty(rap.Rows[0]["TrasportoAlimentare"].ToString())) ? Convert.ToInt32(rap.Rows[0]["TrasportoAlimentare"]).ToString() : string.Empty;
            txtSanzioniTrasportoAlimentare.Text = (rap.Rows[0]["SanzioniTrasportoAlimentare"] != DBNull.Value && rap.Rows[0]["SanzioniTrasportoAlimentare"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniTrasportoAlimentare"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniTrasportoAlimentare"]).ToString() : string.Empty;
            txtOfficine.Text = (rap.Rows[0]["Officine"] != DBNull.Value && rap.Rows[0]["Officine"] != null && !string.IsNullOrEmpty(rap.Rows[0]["Officine"].ToString())) ? Convert.ToInt32(rap.Rows[0]["Officine"]).ToString() : string.Empty;
            txtSanzioniOfficine.Text = (rap.Rows[0]["SanzioniOfficine"] != DBNull.Value && rap.Rows[0]["SanzioniOfficine"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniOfficine"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniOfficine"]).ToString() : string.Empty;
            txtContrBnB.Text = (rap.Rows[0]["ContrBnB"] != DBNull.Value && rap.Rows[0]["ContrBnB"] != null && !string.IsNullOrEmpty(rap.Rows[0]["ContrBnB"].ToString())) ? Convert.ToInt32(rap.Rows[0]["ContrBnB"]).ToString() : string.Empty;
            txtSanzioniContrBnB.Text = (rap.Rows[0]["SanzioniContrBnB"] != DBNull.Value && rap.Rows[0]["SanzioniContrBnB"] != null && !string.IsNullOrEmpty(rap.Rows[0]["SanzioniContrBnB"].ToString())) ? Convert.ToInt32(rap.Rows[0]["SanzioniContrBnB"]).ToString() : string.Empty;
        }

        protected void btA_Click(object sender, EventArgs e)
        {

            // Crea un nuovo ListViewDataItem con un valore di esempio
            ListViewDataItem itemToAdd = new ListViewDataItem(0, 0);
            itemToAdd.DataItem = DdlPattuglia.SelectedItem.Text;

            // Verifica se l'elemento è già presente nella ListView
            //bool itemExists = false;

            //foreach (var item in LPattugliaCompleta.Items)
            //{
            //    if (item.ToString() == itemToAdd.DataItem.ToString())
            //    {
            //        itemExists = true;
            //        break;
            //    }
            //}

            // Aggiungi l'elemento solo se non esiste già
            //if (!itemExists)
            //{
            //    LPattugliaCompleta.Items.Add(DdlPattuglia.SelectedItem.Text);
            //}

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#ModalRicerca').modal('hide');", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

        }
        protected void btRicScheda_Click(object sender, EventArgs e)
        {

            string pattuglia = txtModPattuglia.Text;


            if (!string.IsNullOrEmpty(pattuglia))
            {

                Manager mn = new Manager();
                DataTable rapportino = mn.GetSchedeByPattuglia(uoSelezionata, pattuglia, out msg);

                if (rapportino.Rows.Count > 0)
                {
                    GVRicercaRapportino.DataSource = rapportino;
                    GVRicercaRapportino.DataBind();

                }
                else
                {
                    //richiama popup dalla site master
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaNotFound.GetDescription(), "warning");
                        return;
                    }

                    //}

                }
                //if (!string.IsNullOrEmpty(txtModPattuglia.Text))
                //{

                //    Manager mn = new Manager();
                //    DataTable schede = mn.GetSchedeBy(null, txtModPattuglia.Text, null, ckModAttivitaInterna.Checked, 0, null);

                //    if (schede.Rows.Count > 0)
                //    {
                //        GVRicecaScheda.DataSource = schede;
                //        GVRicecaScheda.DataBind();

                //    }
                //    else
                //    {
                //        //richiama popup dalla site master
                //        SiteMaster myMaster = this.Master as SiteMaster;

                //        if (myMaster != null)
                //        {
                //            // 2. Chiamo il metodo pubblico
                //            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaNotFound.GetDescription(), "warning");
                //        }
                //        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);

                //    }
                //}
                //if (!string.IsNullOrEmpty(txtModDataIntervento.Text))
                //{

                //    Manager mn = new Manager();
                //    DataTable schede = mn.GetSchedeBy(null, null, txtModDataIntervento.Text, ckModAttivitaInterna.Checked, 0, null);

                //    if (schede.Rows.Count > 0)
                //    {
                //        GVRicecaScheda.DataSource = schede;
                //        GVRicecaScheda.DataBind();

                //    }

                //}
                //if (ckModAttivitaInterna.Checked)
                //{
                //    //if (String.IsNullOrEmpty(txtModPattuglia.Text))
                //    //{
                //    //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "pattuglia " + "'); $('#errorModal').modal('show');", true);

                //    //}
                //    Manager mn = new Manager();
                //    DataTable schede = mn.GetSchedeBy(null, txtModPattuglia.Text, null, ckModAttivitaInterna.Checked, 0, null);

                //    if (schede.Rows.Count > 0)
                //    {
                //        GVRicecaScheda.DataSource = schede;
                //        GVRicecaScheda.DataBind();

                //    }

                //}
                // Mantieni il popup aperto dopo l'interazione lato server.
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica scheda non riuscito, controllare il log." + "'); $('#ModalRicerca').modal('show');", true);
            }
        }


        protected void btModificaScheda_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(divTesta, true);
            SetControlsEnabled(divDettagli, true);
            //SetControlsVisible(divDDLPattuglia, true);
            //SetControlsVisible(divPattuglia, false);
            //btStampa.Enabled = true;
            CkAttivita.Enabled = true;

            //CaricaDLL();
        }
        private Boolean Convalida()
        {
            Boolean ret = true;
            SiteMaster myMaster = this.Master as SiteMaster;
            //if (String.IsNullOrEmpty(LPattugliaCompleta.ToString()))
            //{
            //    // richiama il popup di errore nella site page master

            //    if (myMaster != null)
            //    {
            //        // 2. Chiamo il metodo pubblico
            //        myMaster.MostraMessaggio("⚠️ ATTENZIONE", "Inserire la pattuglia.", "warning");


            //    }

            //    ret = false;
            //    return ret;
            //}


            //F- mod 31/01/2026 scheda int
            return ret;
        }


        protected void btElimina_Click(object sender, EventArgs e)
        {
            //if (LPattugliaCompleta.SelectedItem != null)
            //    LPattugliaCompleta.Items.Remove(LPattugliaCompleta.SelectedItem);
        }
        protected void Aggiungi_Click(object sender, EventArgs e)
        {

            // Crea un nuovo ListViewDataItem con un valore di esempio
            ListViewDataItem itemToAdd = new ListViewDataItem(0, 0);
            itemToAdd.DataItem = DdlPattuglia.SelectedItem.Text;

            // Verifica se l'elemento è già presente nella ListView
            bool itemExists = false;

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
        protected void btPopStampa_Click(object sender, EventArgs e)
        {
            int id = System.Convert.ToInt32(HfIdScheda.Value);
            //I- mod 01/04/2026 scheda int aggiunto print e numeri abusi 
            //mn.UpdRappUoteStampa(id);
            //F- mod 01/04/2026 scheda int aggiunto print e numeri abusi 
            //DataTable schede = mn.GetSchedeBy(null, null, null, CkAttivita.Checked, id, null);

            //Routine stampa = new Routine();
            //stampa.CreaPdf(schede);

        }
        private void CaricaDLL()
        {
            try
            {
                string msg = string.Empty;
                DataTable CaricaOperatori = mn.getListOperatore(uoSelezionata, out msg);
                DdlPattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                DdlPattuglia.DataTextField = "Nominativo"; // Il campo visibile
                                                           //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                DdlPattuglia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                DdlPattuglia.DataBind();
                //DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable CaricaAuto = mn.getSiglaAuto(uoSelezionata);
                DdlSiglaVettura.DataSource = CaricaAuto;
                DdlSiglaVettura.DataTextField = "targa";
                DdlSiglaVettura.DataBind();



                //
                //DdlQuartiereIns.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                //DdlQuartiereIns.DataTextField = "Quartiere"; // Il campo visibile
                //DdlQuartiereIns.DataValueField = "id";
                //DdlQuartiereIns.DataBind();
                //DdlQuartiereIns.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleziona un'opzione --", "0"));

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
        private void Pulisci()
        {
            //txtPratica.Text = string.Empty;
            //txtIndirizzo.Text = string.Empty;
            //TxtDataIntervento.Text = string.Empty;
            //txtNominativo.Text = string.Empty;
            txtNote.Text = string.Empty;
            txtDataConsegna.Text = string.Empty;

        }

        protected void btStampa_Click(object sender, EventArgs e)
        {
            //int id = System.Convert.ToInt32(HfIdScheda.Value);
            //DataTable schede = mn.GetSchedeBy(null, null, null, ckModAttivitaInterna.Checked, id, null);
            ////I- mod 01/04/2026 scheda int aggiunto print e numeri abusi 
            //mn.UpdRappUoteStampa(id);
            ////F- mod 01/04/2026 scheda int aggiunto print e numeri abusi 
            //Routine stampa = new Routine();
            //stampa.CreaPdf(schede);


        }
        protected void btChiudi_Click(object sender, EventArgs e)
        {
            Session.Remove("popApertoRicercaScheda");
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#ModalRicerca').modal('hide');", true);
        }
        protected void GVRicercaRapportino_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVRicercaRapportino.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            btRicScheda_Click(sender, e);

        }

        protected void CkAttivita_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void CkAppiedata_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rptOperatori_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptOperatori_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void btSalva_Click(object sender, EventArgs e)
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
                rap.uo = uoSelezionata.ToUpper();

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

                Boolean resp = mn.UpdRapportino(rap, HfIdScheda.Value, uoSelezionata);
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
    }
}