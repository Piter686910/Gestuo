using Microsoft.Ajax.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;
using Uote;
using GestUO.Classi;
using static GestUO.Classi.Enumerate;


namespace GestUO
{
    public partial class Inserimento : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        public string Vuser { get; set; }
        public string ruolo { get; set; }
        public string uoSelezionata { get; set; }
        public string TitoloUO { get; set; }
        string msg = string.Empty;
        string pagchiamante = "~/View/Inserimento.aspx";
        
        Routine r = new Routine();
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["PaginaChiamante"] = pagchiamante;
            string duplica = Request.QueryString["dup"];
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                ruolo = Session["ruolo"].ToString();
                uoSelezionata = Session["uo"].ToString();


                switch (uoSelezionata.ToUpper())
                {
                    case "CHIAIA":
                        TitoloUO = "INSERIMENTO CARICO U.O. CHIAIA";
                        break;
                    case "SCAMPIA":
                        TitoloUO = "INSERIMENTO CARICO U.O. SCAMPIA";
                        break;
                    case "TUTTE":
                        TitoloUO = "INSERIMENTO CARICO";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                Response.Redirect(url);

            }
            if (!IsPostBack)
            {
               
                Routine prot = new Routine();
                txtProt.Text = prot.GetProtocollo();
                txtDecretante.Text = prot.GetNomeOperatoreByMatr(Vuser);
                
                
                
                //Manager mn = new Manager();
                //DataTable operatore = mn.getNominativoOperatore(Vuser);
                //if (operatore.Rows.Count > 0)
                //{
                //    if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
                //        txtDecretante.Text = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
                //}



                if (!string.IsNullOrEmpty(txtProt.Text))
                {
                    txtPraticaDecr.Text = txtProt.Text;
                }
                txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");
                // Legge il valore dal Web.config
          //      string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            //    string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                // ProtocolloLiteral.Text = decodedText;
                CaricaDLL();
                //if (ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
                //{
                //    Manager mn = new Manager();
                //    DdlSigla.SelectedValue = Enumerate.Sigla.AG.ToString().ToUpper();
                //    DataTable RicercaProvvAg = mn.getListProvvAg(DdlSigla.SelectedValue.ToString());
                //    DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                //    DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                //    DdlTipoProvvAg.DataValueField = "id_tipo_nota_ag"; // Il valore associato a ogni opzione
                //    DdlTipoProvvAg.SelectedIndex = 1;
                //    DdlTipoProvvAg.DataBind();
                //    divAg.Visible = true;
                //}
                //else
                //    divAg.Visible = false;
               
                txtDataInsCarico.Text = DateTime.Now.Date.ToShortDateString();
                if (Session["ListRicerca"] != null && duplica=="y")
                {
                    DataTable pratica = (DataTable)Session["ListRicerca"];
                    txtSearchAtto.Value = pratica.Rows[0]["Tipologia_atto"].ToString();
                    //DdlSigla.SelectedValue = pratica.Rows[0]["Sigla"].ToString();
                    txtRifProtGen.Text = pratica.Rows[0]["Rif_Prot_Gen"].ToString();
                    txtTipoAtto.Text = pratica.Rows[0]["UlterioreTipoAtto"].ToString();
                    txtProvenienza.Text = pratica.Rows[0]["Provenienza"].ToString();
                    txtNumProtRicStessoCarico.Text = pratica.Rows[0]["NumProtRicStessoCarico"].ToString();
                    txtIndirizzo.Text = pratica.Rows[0]["Indirizzo"].ToString();
                    txtQuartiere.Text = pratica.Rows[0]["Quartiere"].ToString();
                    txtNominativo.Text = pratica.Rows[0]["Nominativo"].ToString();
                    txPratica.Text = pratica.Rows[0]["nr_Pratica"].ToString();
                    
                    if (!string.IsNullOrWhiteSpace(pratica.Rows[0]["DataCarico"].ToString()))
                    {


                        DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0]["DataCarico"].ToString()); // Recupera la data dal DataTable
                        if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                        {
                            txtDataCarico.Text = ""; // Metti una stringa vuota
                        }
                        else
                        {
                            txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                        }
                    }
                    DdlAreaCompetenza.SelectedItem.Text = pratica.Rows[0]["AreaCompetenza"].ToString();
                    txtGiudice.Text = pratica.Rows[0]["Giudice"].ToString();
                    txtProdPenNr.Text = pratica.Rows[0]["ProcedimentoPen"].ToString();

                    DdlTipoProvvAg.SelectedItem.Text = pratica.Rows[0]["TipoProvvedimentoAG"].ToString();
                    txtBU.Text = pratica.Rows[0]["BU"].ToString();

                    txtGiudice.Text = pratica.Rows[0]["Giudice"].ToString();
                    txtCodEdificio.Text = pratica.Rows[0]["CodiceEdificio"].ToString();
                    //if (pratica.Rows[0]["Sigla"].ToString() == "AG")
                    //{
                    //    divAg.Visible = true;
                    //}
                    //if (DdlSigla.SelectedItem.Text == "TP")
                    //{
                    //    divbu.Visible = true;
                    //    divcd.Visible = true;
                    //}
                    //I 23/04/2026 controllo deleghe
                    if (!String.IsNullOrEmpty(pratica.Rows[0]["DataDelega"].ToString()))
                    {
                    txtDataDelega.Text = Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()).ToShortDateString();

                    }
                    txtGgDelega.Text = pratica.Rows[0]["GgDelega"].ToString();
                    //F 23/04/2026 controllo deleghe
                    
                }
                Session.Remove("ListRicerca");
            }

        }
        public void Convalida()
        {

            //if (!String.IsNullOrEmpty(HfGiudice.Value))
            //    btSalvaGiudice.Visible = true;

            //if (!String.IsNullOrEmpty(HfTipoProv.Value))
            //    btSalvaTipoProvv.Visible = true;

            //if (!String.IsNullOrEmpty(HfProvenienza.Value))
            //    btSalvaProvenienza.Visible = true;

            //if (!String.IsNullOrEmpty(HfTipoAtto.Value))
            //    btSalvaTipoAtto.Visible = true;

            //if (!String.IsNullOrEmpty(HfInviata.Value))
            //    btSalvaInviata.Visible = true;

        }
        public Boolean ControlloCampiObbligatori()
        {
            Boolean ret = true;
            if (String.IsNullOrEmpty(txtProdPenNr.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txtTipoProv.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }

            if (String.IsNullOrEmpty(txtGiudice.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txtQuartiere.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txtProvenienza.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txtRifProtGen.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txtIndirizzo.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txtNominativo.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            if (String.IsNullOrEmpty(txPratica.Text) && ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
            {
                return false;
            }
            //if (String.IsNullOrEmpty(txtTipoAtto.Text) && Ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoAtti.ToString().ToUpper())
            //{
            //    return false;
            //}
            return ret;
        }
        protected Boolean Verifica()
        {
            Boolean resp = true;
            //Tipologie espostoSegn = Tipologie.EspostoSegnalazione;
            //Tipologie Delega = Tipologie.DelegaIndagine;
            //Tipologie altro = Tipologie.Altro;
            //string testoEsposto = espostoSegn.GetDescription();
            //string testoTipoProvv = Delega.GetDescription();
            //string testoAltro = altro.GetDescription();
            //if (DdlTipoProvvAg.SelectedIndex >= 0)
            //{
            //    if (DdlTipoAtto.SelectedItem.Text == testoEsposto)
            //    {
            //        if (DdlTipoProvvAg.SelectedItem.Text == testoTipoProvv)
            //            resp = false;
            //        else if (DdlTipoProvvAg.SelectedItem.Text == testoAltro)
            //        {
            //            resp = false;
            //        }
            //        //    //    if (divAg.Visible == true)
            //        //    //    {
            //        //    //        resp = false;
            //        //    //    }
            //        //    //}
            //        //    string displayValue = divAg.Style["display"];

            //        //    if (displayValue != null && displayValue.ToLower().Trim() == "block")
            //        //    {
            //        //        resp = false;
            //        //    }
            //    }
            //}

            return resp;
        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                SiteMaster myMaster = this.Master as SiteMaster;

                if (String.IsNullOrWhiteSpace(Vuser))
                {


                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    }
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=false");
                    Response.Redirect(url, false);
                    return;

                }
                Boolean verifica = Verifica();
                if (!verifica)
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SiglaErrata.GetDescription(), "danger");
                    }
                   // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorAvvertenze').text('" + "Se la sigla è AG tipologia atto non può essere ESPOSTO - SEGNALAZIONE." + "'); $('#ModalAvvertenze').modal('show');", true);

                }
                else
                {
                    //if (Session["user"] != null)
                    //{
                    //    if (String.IsNullOrEmpty(Session["user"].ToString()))
                    //    {
                    //        //richiama popup dalla site master

                    //        if (myMaster != null)
                    //        {
                    //            // 2. Chiamo il metodo pubblico
                    //            myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    //        }
                    //        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.SScaduta.GetDescription() + "'); $('#errorModal').modal('show');", true);

                    //        string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=false");
                    //        Response.Redirect(url, false);
                    //    }
                    //}
                    // int protocollo = 0;
                    //Boolean obbligo = ControlloCampiObbligatori();
                    //if (obbligo)
                    //{
                    Carico p = new Carico();
                    p.anno = annoCorr;

                    DateTime giorno = DateTime.Now;
                    p.giorno = giorno.ToString("dddd", new CultureInfo("it-IT"));

                    Manager mn = new Manager();

                    p.nrcarico = System.Convert.ToInt32(txtProt.Text);
                    p.uo = uoSelezionata;
               //     p.sigla = DdlSigla.SelectedItem.Text;
                    if (DateTime.TryParse(txtDataInsCarico.Text, out DateTime dataIns))
                    {
                        // Formato standard MySQL: YYYY-MM-DD
                        p.dataArrivo = dataIns.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        // Gestione valore vuoto o non valido per MySQL
                        p.dataArrivo = null;
                    }
                    p.dataInvio = null;
                    p.evasaData = null;
                    //p.dataCarico = DateTime.MinValue.ToShortDateString(); 
                    if (DateTime.TryParse(txtDataCarico.Text, out DateTime dataInsCarico))
                    {
                        // Formato standard MySQL: YYYY-MM-DD
                        p.dataCarico = dataInsCarico.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        // Gestione valore vuoto o non valido per MySQL
                        p.dataCarico = null;
                    }
                    p.nominativo = txtNominativo.Text;
                    if (String.IsNullOrEmpty(txPratica.Text))
                    {
                        p.nr_Pratica = String.Empty;
                    }
                    else
                    {
                        p.nr_Pratica = txPratica.Text;
                    }
                    if (String.IsNullOrEmpty(txtGiudice.Text))
                    {
                        p.giudice = String.Empty;
                    }
                    else
                    {

                        Boolean resp = mn.getGiudice(txtGiudice.Text);
                        if (!resp)
                        {
                            HfGiudice.Value = txtGiudice.Text;
                        }

                        p.giudice = txtGiudice.Text;
                    }

                    if (String.IsNullOrEmpty(txtProvenienza.Text))
                    {

                        p.provenienza = string.Empty;
                    }

                    else
                    {
                        Boolean resp = mn.getProvenienza(txtProvenienza.Text);
                        if (!resp)
                        {
                            HfProvenienza.Value = txtProvenienza.Text;
                        }
                        p.provenienza = txtProvenienza.Text;
                    }

                    //if (String.IsNullOrEmpty(DdlTipoAtto.SelectedItem.Text))
                    //{

                    //    p.tipologia_atto = String.Empty;
                    //}
                    //else
                    //{
                    //    Boolean resp = mn.getTipoAtto(DdlTipoAtto.SelectedItem.Text);
                    //    if (!resp)
                    //    {
                    //        HfTipoAtto.Value = DdlTipoAtto.SelectedItem.Text;
                    //    }
                    //    p.tipologia_atto = DdlTipoAtto.SelectedItem.Text;
                    //}
                    if (String.IsNullOrEmpty(txtSearchAtto.Value))
                    {

                        p.tipologia_atto = String.Empty;
                    }
                    else
                    {
                        Boolean resp1 = mn.getTipoAtto(txtSearchAtto.Value);
                        if (!resp1)
                        {
                            HfTipoAtto.Value = txtSearchAtto.Value;
                        }
                        p.tipologia_atto = txtSearchAtto.Value;
                    }
                    if (!String.IsNullOrEmpty(txtTipoAtto.Text))
                        p.ulterioreTipoAtto = txtTipoAtto.Text;
                    else
                        p.ulterioreTipoAtto = string.Empty;

                    //if (String.IsNullOrEmpty(txtTipoProv.Text))
                    if (DdlTipoProvvAg.Items.Count > 0)
                    {
                        if (String.IsNullOrEmpty(DdlTipoProvvAg.SelectedItem.Text))
                        {
                            p.tipoProvvedimentoAG = String.Empty;
                        }
                        else
                        {

                            Boolean resp = mn.getTipoProv(DdlTipoProvvAg.SelectedItem.Text);
                            if (!resp)
                            {
                                HfTipoProv.Value = DdlTipoProvvAg.SelectedItem.Text;
                            }

                            p.tipoProvvedimentoAG = DdlTipoProvvAg.SelectedItem.Text;// txtTipoProv.Text.ToUpper();
                        }
                    }
                    if (String.IsNullOrEmpty(txtIndirizzo.Text))
                    {
                        p.indirizzo = String.Empty;
                    }
                    else
                    {
                        p.indirizzo = txtIndirizzo.Text;
                    }
                    if (String.IsNullOrEmpty(txtQuartiere.Text))
                    {
                        p.quartiere = String.Empty;
                    }
                    else
                    {
                        p.quartiere = txtQuartiere.Text;
                        //p.quartiere = lblQuartiere.Text;
                    }




                    p.evasa = false;

                    if (!String.IsNullOrEmpty(DdlAreaCompetenza.SelectedItem.Text))
                    {
                        p.areaCompetenza = DdlAreaCompetenza.SelectedItem.Text.ToUpper();
                    }
                    else
                        p.areaCompetenza = string.Empty;

                    p.operatori = string.Empty;
                    

                    p.procedimentoPen = txtProdPenNr.Text;
                    p.matricola = Vuser;
                    p.data_ins_pratica = DateTime.Now.ToLocalTime();
                    p.NumProtRicStessoCarico = string.IsNullOrWhiteSpace(txtNumProtRicStessoCarico.Text) ? 0 : Convert.ToInt32(txtNumProtRicStessoCarico.Text);
                    //sostituisco tutto ciò che è diverso da "/" e numeri con ";"
                    // La regex [^0-9/] significa: 
                    // ^ = "tutto ciò che NON è"
                    // 0-9 = numeri
                    // / = il carattere slash
                    p.rif_Prot_Gen = Regex.Replace(txtRifProtGen.Text, @"[^0-9/]", ";");

                    
                    DateTime ora = DateTime.Now;
                    string mese = ora.ToString("MMMM");
                    string testoE = string.Empty;
                    string testo = string.Empty;
                    
                    if (String.IsNullOrEmpty(txtBU.Text))
                    {
                        p.bu = String.Empty;
                    }
                    else
                    {
                        p.bu = txtBU.Text;

                    }
                    if (String.IsNullOrEmpty(txtCodEdificio.Text))
                    {
                        p.codiceEdificio = String.Empty;
                    }
                    else
                    {
                        p.codiceEdificio = txtCodEdificio.Text;

                    }
                    p.dataDelega= string.IsNullOrWhiteSpace(txtDataDelega.Text) ? null : System.Convert.ToDateTime(txtDataDelega.Text).ToShortDateString();
                    p.ggDelega= string.IsNullOrWhiteSpace(txtGgDelega.Text) ? 0 : Convert.ToInt32(txtGgDelega.Text);
                    
                    
               //     p.cartellina= string.IsNullOrWhiteSpace(txtCartellina.Text) ? String.Empty : txtCartellina.Text.Trim();
                    long idN = 0;
                    Boolean ins = mn.InsCarico(p, System.Convert.ToInt32(txtProt.Text), out idN);

                    // 1. Controllo Sessione / Utente non valido
                    if (idN == -2)
                    {
                        if (myMaster != null)
                        {
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                        }
                        string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                        Response.Redirect(url, false);
                        return;
                    }

                    // Assegno l'ID generato all'HiddenField
                    Hid.Value = System.Convert.ToString(idN);
                    txtPraticaDecr.Text = txtProt.Text;

                    // 2. Controllo Duplicato o Errore Inserimento (idN == -3 oppure ins == false)
                    if (!ins || idN == -3)
                    {
                        // Ricalcolo il nuovo numero di protocollo libero
                        Routine prot = new Routine();
                        string nuovoCarico = prot.GetProtocollo();
                        txtProt.Text = nuovoCarico;

                        if (myMaster != null)
                        {
                            myMaster.MostraMessaggio(
                                "⚠️ ATTENZIONE",
                                $"Inserimento del carico non riuscito. Il numero carico {p.nrcarico} con anno {p.anno} è già esistente. Il nuovo protocollo assegnato è {nuovoCarico}.",
                                "danger"
                            );
                        }
                    }
                    else
                    {
                        // 3. Inserimento avvenuto con successo
                        string messaggioConferma = $"Carico {p.nrcarico} inserito correttamente, vuoi inserire una decretazione?";

                        ScriptManager.RegisterStartupScript(
                            this.Page,
                            this.GetType(),
                            "modalScript",
                            $"$('#Message').text('{messaggioConferma}'); $('#ModalRicDecretazione').modal('show');",
                            true
                        );

                        modalLabel1.CssClass = " bg-success"; // Evidenziatore verde

                        // Gestione stato pulsanti
                        btNewIns.Visible = true;
                        btSalva.Visible = false;
                    }
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Ci sono campi obbligatori non inseriti, controllare Tipologia Atto o Riferimento Protocollo Generale!" + "'); $('#errorModal').modal('show');", true);

                    //}
                }
            }
            catch (Exception ex)
            {

                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                // Response.Redirect("~/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = pagchiamante;

                //Response.Redirect("~/Contact.aspx");

            }
        }
        private void Pulisci()
        {
            Convalida();
            txtSearchAtto.Value = string.Empty;
            txtProt.Text = String.Empty;
            txtDataDecretazione.Text = String.Empty;
            //  txtDecretato.Text = String.Empty;
            txtSearchOperatore.Value = string.Empty;
            txtNotaDecretazione.Text = String.Empty;
            if (String.IsNullOrEmpty(HfGiudice.Value))
            {
                txtGiudice.Text = string.Empty;

            }
            if (String.IsNullOrEmpty(HfTipoProv.Value))
            {
                txtTipoProv.Text = string.Empty;
            }
            txtNumProtRicStessoCarico.Text = string.Empty;
            txtQuartiere.Text = string.Empty;
            //if (String.IsNullOrEmpty(HfInviata.Value))
            //{
            //    txtInviata.Text = string.Empty;
            //}
            if (String.IsNullOrEmpty(HfProvenienza.Value))
            {
                //txtTipoAtto.Text = string.Empty;
                DdlTipoAtto.ClearSelection();
            }

            txtIndirizzo.Text = string.Empty;
            HfIndirizzo.Value = string.Empty;
            if (String.IsNullOrEmpty(HfProvenienza.Value))
            {
                txtProvenienza.Text = string.Empty;
            }

            txtDataInsCarico.Text = String.Empty;
            txtRifProtGen.Text = String.Empty;
            //  txtVia.Text = String.Empty;
            txtProdPenNr.Text = String.Empty;
            txtNominativo.Text = String.Empty;
            txPratica.Text = String.Empty;
            txtTipoAtto.Text = String.Empty;
            txtProvenienza.Text = String.Empty;
            //txtAreaCompetenza.Text = string.Empty;
            DdlAreaCompetenza.ClearSelection();
            txtDataCarico.Text = string.Empty;
            txtBU.Text = String.Empty;
            txtCodEdificio.Text = String.Empty;
            // CkEvasa.Checked = false;
            CaricaDLL();
            //I 23/04/2026 controllo deleghe
            txtDataDelega.Text= String.Empty;
            txtGgDelega.Text = String.Empty;
            //F 23/04/2026 controllo deleghe
            
        }
        //popup giudice
        //protected void apripopupGiudice_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModalGiudice').modal('show');", true);
        //}
        ////tipo prov
        //protected void apripopupTipoProv_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModaTipoProv').modal('show');", true);
        //}

        //protected void chiudipopupTipoAtto_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalTipoAtto')); modal.hide();", true);

        //}
        //protected void chiudipopupTipoProv_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModaTipoProv')); modal.hide();", true);

        //}
        //protected void chiudipopupInviata_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalInviata')); modal.hide();", true);

        //}
        //protected void chiudipopupGiudice_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalGiudice')); modal.hide();", true);

        //}
        //popup provenienza
        //protected void apripopupProvenienza_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModalProvenienza').modal('show');", true);
        //}
        //protected void chiudipopupProvenienza_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalProvenienza')); modal.hide();", true);

        //}

        //popup quartiere
        protected void apripopup_Click(object sender, EventArgs e)
        {

            string script = "$('#ModalQuartiere').modal('show');";

            // 2. Aggiungi il focus con un ritardo di 500ms (tempo dell'animazione)
            // Sostituisci 'txtNome' con l'ID (Statico) o ClientID della tua textbox
            script += " setTimeout(function(){ document.getElementById('" + txtIndirizzoQuartiere.ClientID + "').focus(); }, 500);";

            // 3. Esegui
            ScriptManager.RegisterStartupScript(this, GetType(), "ApriEFocus", script, true);



            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }
        //protected void chiudipopup_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        //}

        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
            indirizzo = txtIndirizzoQuartiere.Text.Trim();


            //string specie = txtSpecie.Text.Trim();

            if (!string.IsNullOrEmpty(indirizzo))
            {
                // Simula il recupero del quartiere dal database o da una logica interna.
                Manager mn = new Manager();
                DataTable quartiere = mn.getQuartiere(indirizzo, out msg);

                if (quartiere.Rows.Count > 0)
                {
                    gvPopup.DataSource = quartiere;
                    gvPopup.DataBind();

                }
            }

            // Mantieni il popup aperto dopo l'interazione lato server.

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }
        private void CaricaDLL()
        {
            try
            {
                Routine r = new Routine();
                String msg = string.Empty;
                Manager mn = new Manager();
                //DataTable RicercaQuartiere = mn.getListQuartiere(out msg);
                //if (!String.IsNullOrWhiteSpace(msg))
                //    r.Reindirizzamento(msg, pagchiamante);
                //DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                //DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                //DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                //DdlQuartiere.DataBind();

                //DataTable RicercaIndirizzo = mn.getListIndirizzo(out msg);
                //if (!String.IsNullOrWhiteSpace(msg))
                //    r.Reindirizzamento(msg, pagchiamante);
                //DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                //DdlIndirizzo.DataTextField = "SpecieToponimo"; // Il campo visibile
                //DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                //DdlIndirizzo.DataBind();

                //DataTable RicercaTipoAtto = mn.getListTipologia(out msg);
                //if (!String.IsNullOrWhiteSpace(msg))
                //    r.Reindirizzamento(msg, pagchiamante);
                //DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                //DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                //DdlTipoAtto.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione

                //DdlTipoAtto.DataBind();
                //DdlTipoAtto.Items.Insert(0, new ListItem("", "0"));


                //DataTable RicercaProvenienza = mn.getListProvenienza(out msg);
                //if (!String.IsNullOrWhiteSpace(msg))
                //    r.Reindirizzamento(msg, pagchiamante);
                //DdlProvenienza.DataSource = RicercaProvenienza; // Imposta il DataSource della DropDownList
                //DdlProvenienza.DataTextField = "Provenienza"; // Il campo visibile
                //DdlProvenienza.DataValueField = "id_provenienza"; // Il valore associato a ogni opzione

                //DdlProvenienza.DataBind();
                ////   DdlProvenienza.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                //DataTable RicercaGiudice = mn.getListGiudice(out msg);
                //if (!String.IsNullOrWhiteSpace(msg))
                //    r.Reindirizzamento(msg, pagchiamante);
                //DdlGiudice.DataSource = RicercaGiudice; // Imposta il DataSource della DropDownList
                //DdlGiudice.DataTextField = "Giudice"; // Il campo visibile
                //DdlGiudice.DataValueField = "ID_giudice"; // Il valore associato a ogni opzione

                //DdlGiudice.DataBind();

                //DataTable RicercaProvvAg = mn.getListProvvAg(Enumerate.Sigla.AG.ToString(), out msg);
                //if (!String.IsNullOrWhiteSpace(msg))
                //    r.Reindirizzamento(msg, pagchiamante);
                //// DataTable RicercaProvvAg = mn.getListProvvAg(DdlSigla.SelectedItem.Text);
                //DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                //DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                //DdlTipoProvvAg.DataValueField = "id_tipo_nota_ag"; // Il valore associato a ogni opzione

                //DdlTipoProvvAg.DataBind();
                //DdlTipoProvvAg.Items.Insert(0, new ListItem("", "0"));

                System.Data.DataTable CaricaOperatoriDecretazione = mn.getListOperatore(out msg);
                ddlOperatore.DataSource = CaricaOperatoriDecretazione; // Imposta il DataSource della DropDownList
                ddlOperatore.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                ddlOperatore.Items.Insert(0, new ListItem("", "0"));
                ddlOperatore.DataBind();
                ddlOperatore.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
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
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                // Response.Redirect("/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = pagchiamante;
                //  Response.Redirect("~/Contact.aspx");

            }
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID_quartiere").ToString();

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
                txtQuartiere.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }





        protected void btSalvaGiudice_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciGiudice(HfGiudice.Value);
            if (ins)
            {
                HfGiudice.Value = string.Empty;
                txtGiudice.Text = string.Empty;
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
                }
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "" + "'); $('#errorModal').modal('show');", true);

            }
        }

        //protected void btSalvaTipoProvv_Click(object sender, EventArgs e)
        //{
        //    Manager mn = new Manager();
        //    Boolean ins = mn.InserisciTipologiaNotaAg(HfTipoProv.Value);
        //    if (ins)
        //    {
        //        HfTipoProv.Value = string.Empty;
        //        txtTipoProv.Text = string.Empty;
        //        SiteMaster myMaster = this.Master as SiteMaster;

        //        if (myMaster != null)
        //        {
        //            // 2. Chiamo il metodo pubblico
        //            myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
        //        }
        //        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

        //    }
        //}

        //protected void btSalvaProvenienza_Click(object sender, EventArgs e)
        //{

        //    Manager mn = new Manager();
        //    Boolean ins = mn.InserisciProvenienza(HfProvenienza.Value);
        //    if (ins)
        //    {
        //        HfProvenienza.Value = string.Empty;
        //        txtProvenienza.Text = string.Empty;
        //        SiteMaster myMaster = this.Master as SiteMaster;

        //        if (myMaster != null)
        //        {
        //            // 2. Chiamo il metodo pubblico
        //            myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
        //        }
        //        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

        //    }
        //}


        //protected void btSalvaTipoAtto_Click(object sender, EventArgs e)
        //{
        //    Manager mn = new Manager();
        //    Boolean ins = mn.InserisciTipologia(HfTipoAtto.Value);
        //    if (ins)
        //    {
        //        HfTipoAtto.Value = string.Empty;
        //        //txtTipoAtto.Text = string.Empty;
        //        DdlTipoAtto.ClearSelection();
        //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

        //    }
        //}



        //protected void btSalvaInviata_Click(object sender, EventArgs e)
        //{
        //    Manager mn = new Manager();
        //    Boolean ins = mn.InserisciInviata(HfInviata.Value);
        //    if (ins)
        //    {
        //        HfInviata.Value = string.Empty;
        //        txtInviata.Text = string.Empty;
        //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

        //    }
        //}

        protected void btNewIns_Click(object sender, EventArgs e)
        {
            Pulisci();
            Routine prot = new Routine();
            txtProt.Text = prot.GetProtocollo();
            txtDataInsCarico.Text = DateTime.Now.Date.ToShortDateString();
            btNewIns.Visible = false;
            btSalva.Visible = true;
        }

        protected void DdlSigla_TextChanged(object sender, EventArgs e)
        {
            CaricaDLL();
        }

        //protected void DdlSigla_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CaricaDLL();
        //    if (DdlSigla.SelectedItem.Text == Enumerate.Sigla.AG.ToString().ToUpper())
        //    {
        //        divAg.Visible = true;
        //    }
        //    else
        //    {
        //        divAg.Visible = false;
        //        txtGiudice.Text = string.Empty;
        //        txtProdPenNr.Text = string.Empty;
        //    }
        //}
        protected void Decretazione_Click(object sender, EventArgs e)
        {

            txtPraticaDecr.Text = txtProt.Text;
            txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");

            Manager mn = new Manager();
            //DataTable operatore = mn.getNominativoOperatore(Vuser);
            //if (operatore.Rows.Count > 0)
            //{
            //    if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
            //        txtDecretante.Text = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
            //}
            Routine r = new Routine();
            txtDecretante.Text = r.GetNomeOperatoreByMatr(Vuser);
            DataTable decretazione = new DataTable();
            if (!string.IsNullOrEmpty(txtPraticaDecr.Text))
            {
                string dborigine = Session["dbtarget"].ToString();
                decretazione = mn.getListDecretazione(txtPraticaDecr.Text, Hid.Value,dborigine);
            }
            if (decretazione.Rows.Count > 0)
            {

                GVDecretazione.DataSource = decretazione;
                GVDecretazione.DataBind();
                Boolean a = System.Convert.ToBoolean(decretazione.Rows[0].ItemArray[8]);
                if (a == true)
                {
                    btAggiungiDecretazione.Enabled = false;
                    btChiudiDecretazione.Enabled = false;
                }
                else

                {
                    //if (ruolo.ToUpper() == Enumerate.Ruolo.accertatori.ToString().ToUpper())
                    //{

                    //    btSalva.Visible = false;
                    //    //btCercaQuartiere.Visible = false;
                    //    btChiudiDecretazione.Visible = false;
                    //}
                    //else
                    //    btChiudiDecretazione.Visible = true;

                }

            }
            else
                btChiudiDecretazione.Visible = false;
            apripopupDecretazione_Click(sender, e);
        }
        protected void btAggiungiDecretazione_Click(object sender, EventArgs e)
        {
            try
            {
                Session["PaginaChiamante"] = pagchiamante;
                Manager mn = new Manager();
                Decretazione decr = new Decretazione();
                decr.idPratica = System.Convert.ToInt32(Hid.Value);
                decr.Npratica = txtPraticaDecr.Text;
                decr.decretante = txtDecretante.Text;
                decr.decretato = txtSearchOperatore.Value.ToUpper();
                decr.data = System.Convert.ToDateTime(txtDataDecretazione.Text);
                decr.nota = txtNotaDecretazione.Text.ToUpper();

                Boolean ins = mn.InsDecretazione(decr);
                if (!ins)
                {
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserimento non effettuato, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserimento effettuato correttamente." + "'); $('#errorModal').modal('show');", true);
                    Pulisci();
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in inserimento decretazione ");
                    sw.Close();
                }
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                //Response.Redirect("/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = pagchiamante;
                //                Response.Redirect("~/Contact.aspx");

            }
        }
        protected void apripopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

        }
        protected void chiudipopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDecretazione')); modal.hide();", true);
            // Pulisci();
        }
       
        //gridview per decretazione
        protected void GVDecretazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Ottieni il valore della colonna "ID"
            //    string id = DataBinder.Eval(e.Row.DataItem, "ID_quartiere").ToString();

            //    // Aggiungi l'attributo per il doppio clic
            //    e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            //    e.Row.Style["cursor"] = "pointer";
            //}
            if (GVDecretazione.TopPagerRow != null && GVDecretazione.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVDecretazione.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVDecretazione.PageIndex + 1;
                    int totalPages = GVDecretazione.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        protected void GVDecretazione_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    string selectedValue = e.CommandArgument.ToString();

            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    txtIndirizzo.Text = selectedValue;
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }
        protected void GVDecretazione_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            GVDecretazione.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Decretazione_Click(sender, e);

        }
        protected void btChiudiDecretazione_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ShowPopup", "$('#ModalDataEvasa').modal('show');", true);

        }
        protected void chiudipopupModalRicDecretazione_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicDecretazione')); modal.hide();", true);
            Pulisci();

        }
        protected void btChiudiAvvertenze_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalAvvertenze')); modal.hide();", true);

        }
        protected void Decreta_Click(object sender, EventArgs e)
        {
            txtPraticaDecr.Text = txtProt.Text;
            txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //Manager mn = new Manager();
            //DataTable operatore = mn.getNominativoOperatore(Vuser);
            //if (operatore.Rows.Count > 0)
            //{
            //    if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
            //        txtDecretante.Text = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
            //}
            Routine r = new Routine();
            txtDecretante.Text = r.GetNomeOperatoreByMatr(Vuser);
            //apripopupDecretazione_Click(sender, e);
        }

        /// <summary>
        /// CHIUSURA DECRETAZIONE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ModalChiudiDecretazione_Click(object sender, EventArgs e)
        {
            try
            {
                Decretazione decr = new Decretazione();
                decr.idPratica = System.Convert.ToInt32(Hid.Value);
                decr.Npratica = txtPraticaDecr.Text;
                decr.decretante = txtDecretante.Text.ToUpper();
                decr.nota = txtNotaDecretazione.Text.ToUpper();
                decr.decretato = txtSearchOperatore.Value.ToUpper();
                if (!String.IsNullOrEmpty(txtDataDecretazione.Text))
                    decr.data = System.Convert.ToDateTime(txtDataDecretazione.Text);
                decr.chiuso = true;
                if (!String.IsNullOrEmpty(txtdataEvasaPopup.Text))
                {
                    //string dataFormattata = DateTime.Now.ToString("dd/MM/yyyy");
                    decr.dataChiusura = System.Convert.ToDateTime(txtdataEvasaPopup.Text);
                }

                //string dataFormattata = DateTime.Now.ToString("dd/MM/yyyy");
                //decr.dataChiusura = System.Convert.ToDateTime(dataFormattata);

                Manager mn = new Manager();
                Boolean upd = mn.UpdDecretazioneChiusura(decr);
                if (!upd)
                {
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.CloseKO.GetDescription(), "danger");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "chiusura non effettuata, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.CloseOK.GetDescription(), "success");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "chiusura effettuata correttamente." + "'); $('#errorModal').modal('show');", true);

                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDataEvasa')); modal.hide();", true);
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in chiusura decretazione ");
                    sw.Close();
                }
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);
                // Response.Redirect("~/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = pagchiamante;
                // Response.Redirect("~/Contact.aspx");

            }

        }

        
    }
}