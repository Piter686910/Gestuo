using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;
using GestUO.Classi;
using WebGrease.Activities;
using static GestUO.Classi.Enumerate;

namespace GestUO
{
    public partial class Visualizza : Page
    {
        // Variabile per gestire la pagina corrente (puoi salvarla in ViewState)

        public int PaginaCorrente
        {


            get
            {
                if (ViewState["PaginaCorrente"] == null)
                    return 1; // Forza la partenza a 1 se è la prima volta

                return (int)ViewState["PaginaCorrente"];
            }
            set { ViewState["PaginaCorrente"] = value; }
        }
        public string TitoloUO = string.Empty;
        public String annoCorr = DateTime.Now.Year.ToString();
        public string Vuser { get; set; }
        public string ruolo { get; set; }
        public string uoSelezionata { get; set; }
        public String Profilo = String.Empty;
        Carico p = new Carico(); public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string paginaChiamante = "~/View/Visualizza.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["PaginaChiamante"] = paginaChiamante;

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                ruolo = Session["ruolo"].ToString();
                uoSelezionata = Session["uo"].ToString();

               
                switch (uoSelezionata.ToUpper())
                {
                    case "CHIAIA":
                        TitoloUO = "RICERCA ATTI U.O. CHIAIA";
                        break;
                    case "SCAMPIA":
                        TitoloUO = "RICERCA ATTI U.O. SCAMPIA";
                        break;
                    case "TUTTE":
                        TitoloUO = "RICERCA ATTI";
                        break;
                    default:
                        break;
                }

                if (ruolo.ToUpper() == Enumerate.Ruolo.Archivio.GetDescription().ToUpper())
                    btModifica.Visible = false;
            }
            else
            {
                //btOKDup.Visible = false; // non visualizzo ok per non confondere l'utente perchè in questo caso non serve il button ok
                //TextMessage.InnerText = Enumerate.MsgOutput.SScaduta.GetDescription();
                ////TextMessage.InnerHtml = "style=""";
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#TextMessage').text('" + ".." + "'); $('#MsgModal').modal('show');", true);
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=false");
                    Response.Redirect(url, false);
                    return;
                }
            }


            // Legge il valore dal Web.config
            //string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            //string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            // ProtocolloLiteral.Text = decodedText;
            //int protocollo = 0;
            lblmessage.Text = string.Empty;
            if (!IsPostBack)
            {
                //     PaginaCorrente = 1;

                //TitoloUo = "RICERCA ATTI " + Session["dbtarget"];
                if (ruolo.ToUpper() == Enumerate.Ruolo.comandante.GetDescription().ToUpper())
                {
                    btValidaPratica.Visible = false;
                    btcarico.Visible = false;
                    ckRicercaGeneralizzata.Visible = true;
                    btModifica.Visible = false;
                    BtDuplica.Visible = false;
                    btDecreta.Visible = false;

                }
                DivDettagli.Visible = false;
                string idCarico = Request.QueryString["idscheda"];
                if (!String.IsNullOrEmpty(idCarico))
                {
                    txtNCarico.Text = Request.QueryString["Nr_Carico"];
                    txtAnnoRicerca.Text = Request.QueryString["Anno"];
                    //Ricerca_Click(sender, e);
                    Manager mn = new Manager();
                    DataTable pratica = mn.getPraticaId(Convert.ToInt32(idCarico));
                    FillScheda(pratica, mn);
                }

            }

        }

        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            if (ruolo.ToUpper() == Enumerate.Ruolo.comandante.GetDescription().ToUpper())
                ckRicercaGeneralizzata.Visible = true;
            Pulisci();
        }
        public Boolean VerificaAppartenenzaUO(string db, string uo)
        {
            Boolean resp = true;
            if (!db.Contains(uo))
            {
                SiteMaster myMaster = this.Master as SiteMaster;
                resp = false;
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.DbNonAppartenente.GetDescription(), "warning");
                }

            }
            return resp;
        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {
            String msg = string.Empty;
            String dbOrigine = string.Empty;
            Manager mn = new Manager();
            Boolean validazione = false;
            DataTable pratica = new DataTable();

            //se anno non valorizzato lo valorizzo con anno corrente
            if (String.IsNullOrEmpty(txtAnnoRicerca.Text))
            {
                txtAnnoRicerca.Text = DateTime.Now.Year.ToString();
            }
            if (txtRicPraticaVal.Text != string.Empty && txtRicAnnoVal.Text != string.Empty)
            {
                pratica = mn.getListPraticheVal(txtRicPraticaVal.Text, txtRicAnnoVal.Text, out msg);
                validazione = true;
            }
            //ricerca per carico e anno
            if (txtNCarico.Text != string.Empty && txtAnnoRicerca.Text != string.Empty)
            {
                //if (ckRicercaGeneralizzata.Checked)
                //{
                //    pratica = mn.CmdByAnnoCarico(txtAnnoRicerca.Text, txtNCarico.Text, out dbOrigine);
                //}
                //else
               // {
                    
                    pratica = mn.getListCaricoTabella(Vuser, txtNCarico.Text, txtAnnoRicerca.Text,uoSelezionata.ToUpper(), ckRicercaGeneralizzata.Checked ,out msg);
                    //verifico se la ricerca appartiene alla uo selezionata
                    if (pratica != null)
                    {
                        bool esisteColonna = pratica.Columns.Contains("uo");//verifico se esiste la colonna nel datatable

                        string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["uo"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                        Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, uoSelezionata.ToUpper(), dbPratica);

                        if (resp == false)

                            return;
                    }
                    else
                        return;
                //}
            }
            //ricerca per PROCEDIMENTO PENALE
            if (txtProcPenale.Text != string.Empty)
            {
                if (ckRicercaGeneralizzata.Checked)
                {
                    pratica = mn.CmdByProcPenale(txtProcPenale.Text, out dbOrigine);
                }
                else
                {
                  
                    pratica = mn.getListProcedimento(txtProcPenale.Text, "uo", out msg);
                    //verifico se la ricerca appartiene alla uo selezionata
                    if (pratica != null)
                    {
                        bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                        string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                        Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                        if (resp == false)

                            return;
                    }
                    else
                        return;

                }

            }
            if (txtDataDa.Text != string.Empty && txtDataA.Text != string.Empty)
            {
                pratica = mn.getListEvasaAg(txtDataDa.Text, txtDataA.Text, "uo", out msg);
            }
            //ricerca per PROTOCOLLO GENERALE
            if (txtProtGen.Text != string.Empty)
            {
                //    if (ckRicercaGeneralizzata.Checked)
                //  {
                //    pratica = mn.CmdByProtGenerale(txtProtGen.Text, out dbOrigine);
                // }
                //else
                //{
                
                pratica = mn.getListProtGen(txtProtGen.Text, ckRicercaGeneralizzata.Checked, "uo", out msg);
                if (pratica == null)
                {
                    //pratica = mn.getListProtGenInDecretazione(txtProtGen.Text, ckRicercaGeneralizzata.Checked,uoSelezionata, out msg);
                    List<string> listaDatabase = new List<string>();
                    listaDatabase.Add("Db_Chiaia");
                    listaDatabase.Add("Db_Scampia");
                    pratica = mn.CercaInTuttiIDatabase(listaDatabase, txtProtGen.Text.Trim());
                }
                //verifico se la ricerca appartiene alla uo selezionata
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;
            }
            //ricerca per pratica o cartellina patrimonio
            if (txtPratica.Text != string.Empty)
            {
                pratica = mn.getListPratica(txtPratica.Text.Trim(), ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata

                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;




            }
            //I mod 26/08/2026 ricerca per numero cartellina patrimonio
            //if (txtCartellinaRic.Text != string.Empty)
            //{
            //    if (txtQuart.Text != string.Empty)
            //        pratica = mn.getListCartellina(txtCartellinaRic.Text.Trim(), txtQuart.Text.Trim(), ckRicercaGeneralizzata.Checked, out msg, out dbOrigine);
            //    else
            //    {

            //        SiteMaster myMaster = this.Master as SiteMaster;

            //        if (myMaster != null)
            //        {
            //            // 2. Chiamo il metodo pubblico
            //            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.InserireQuartiere.GetDescription(), "warning");
            //        }
            //        return;
            //    }


            //    //verifico se la ricerca appartiene alla uo selezionata
            //    if (pratica != null)
            //    {
            //        bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

            //        string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
            //        Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

            //        if (resp == false)

            //            return;
            //    }
            //    else
            //        return;
            //}
            //if (txtCartellinaRic.Text != string.Empty)
            //{
            //    pratica = mn.getListCartellina(txtCartellinaRic.Text.Trim(), txtQuart.Text.Trim(), out msg);
            //}


            //ricerca per BU e codice edificio
            if (txtBuRic.Text != string.Empty)
            {
                pratica = mn.getListBu(txtBuRic.Text.Trim(), ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;
            }
            if (txtCodEdRic.Text != string.Empty)
            {
                pratica = mn.getListCodEdificio(txtCodEdRic.Text.Trim(), ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;
            }
            //F mod 26/08/2026 ricerca per numero cartellina patrimonio

            // ricerca per giudice
            if (txtRicGiudice.Text != string.Empty)
            {
                pratica = mn.getListGiudice(txtRicGiudice.Text, ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;
            }
            if (txtRicProvenienza.Text != string.Empty)
            {
                pratica = mn.getListProvenienza(txtRicProvenienza.Text, ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata
                bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                if (resp == false)

                    return;
            }
            if (txtRicNominativo.Text != string.Empty)
            {
                pratica = mn.getListNominativo(txtRicNominativo.Text, ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;

            }
            if (txtRicOperatori.Text != string.Empty)
            {
                pratica = mn.getListOperatori(txtRicOperatori.Text, ckRicercaGeneralizzata.Checked, "uo", out msg);
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;
            }
            if (txtRicIndirizzo.Text != string.Empty)
            {
                pratica = mn.getListIndirizzo(txtRicIndirizzo.Text, ckRicercaGeneralizzata.Checked, "uo", out msg);
                //verifico se la ricerca appartiene alla uo selezionata
                if (pratica != null)
                {
                    bool esisteColonna = pratica.Columns.Contains("DbOrigine");//verifico se esiste la colonna nel datatable

                    string dbPratica = (pratica.Rows.Count > 0 && esisteColonna) ? pratica.Rows[0]["DbOrigine"]?.ToString() ?? string.Empty : string.Empty; //se esite prelevo il valore altrimenti space
                    Boolean resp = CheckAppartenenzaDb(ckRicercaGeneralizzata.Checked, dbOrigine.ToUpper(), dbPratica);

                    if (resp == false)

                        return;
                }
                else
                    return;
            }
            if (txtDatArrivoDa.Text != string.Empty && txtDatArrivoA.Text != string.Empty)
            {
                pratica = mn.getListDataArrivo(txtDatArrivoDa.Text, txtDatArrivoA.Text, "uo", out msg);
            }
            //if (!String.IsNullOrEmpty(txtStampaDataCarico.Text))
            //{
            //    DataTable carico = new DataTable();
            //    carico = mn.getListCarico(DdlMacroArea.SelectedItem.Text, txtStampaDataCarico.Text, DdlSigla.SelectedItem.Text, out msg);
            //    if (carico.Rows.Count > 0)
            //    {
            //        Routine stampa = new Routine();
            //        stampa.CreaPdfCarichi(carico);
            //    }
            //}
            if (txtNote.Text != string.Empty)
            {
                pratica = mn.getListByNote(txtNote.Text, false ,"uo");
                if (pratica.Rows.Count > 0)
                {
                    Session["ListPratiche"] = pratica;
                    gvNote.DataSource = pratica;
                    gvNote.DataBind();
                    DivRicNote.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaNote').modal('show');", true);
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(msg))
                    {

                        Routine R = new Routine();
                        R.PagError(msg, paginaChiamante);
                    }

                }
                return;
            }
            //}
            if (pratica.Rows.Count > 0)
            {
                // Salva datatable pratica  nella Sessione
                Session["ListPratiche"] = pratica;
                lblNumRighe.Text = " - Num. righe trovate: " + pratica.Rows.Count.ToString();
                if (validazione)
                {
                    GVPratica.PageIndex = 0;
                    GVPratica.DataSource = pratica;
                    GVPratica.DataBind();
                    DivGridVal.Visible = true;
                }
                else
                {
                    gvPopup.DataSource = pratica;
                    gvPopup.DataBind();
                    DivGrid.Visible = true;
                    DivGridVal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
                }
                string a = pratica.Rows[0].ItemArray[1].ToString();


            }
            else
            {
                if (!String.IsNullOrWhiteSpace(msg))
                {

                    Routine R = new Routine();
                    R.PagError(msg, paginaChiamante);
                }
                else
                {

                    //richiama popup dalla site master
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaNotFound.GetDescription(), "warning");
                    }
                }
                // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non presente in database." + "'); $('#errorModal').modal('show');", true);
            }
            ckRicercaGeneralizzata.Visible = false;
        }
        public Boolean CheckAppartenenzaDb(Boolean CkGeneralizzato, string DbOut, string DbPratica)
        {
            Boolean resp = false;
            //if (String.IsNullOrWhiteSpace(Session["uo"].ToString()))
            //{
                if (ckRicercaGeneralizzata.Checked==false)
                    //faccio ricerca generalizzata
                    resp = VerificaAppartenenzaUO(DbOut, DbPratica);
                else
                    //verifico se il comandante o dirigente ha selezionato una uo di appartenenza
                    resp = VerificaAppartenenzaUO(DbOut, uoSelezionata.ToUpper());
            //}
            //else
            //{
            //    //verifico uo di appartenenza
            //    resp = VerificaAppartenenzaUO(DbOut, Session["dbtarget"].ToString().ToUpper());
            //}

            return resp;

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {

            //  ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);
            //adegua chiusura popup bootstrap 5
            string script = @"
    var modalElement = document.getElementById('ModalRicerca');
    if (modalElement) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(modalElement);
        }
        modalInstance.hide();
    }";
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", script, true);
            Pulisci();
            Session.Remove("ListPratiche");
            Session.Remove("ListRicerca");
            gvPopup.PageIndex = 0;
            if (ruolo.ToUpper() == Enumerate.Ruolo.comandante.GetDescription().ToUpper())
                ckRicercaGeneralizzata.Visible = true;
        }
        protected string EvalSafe(object dataItem, string fieldName)
        {
            //DataRowView drv = dataItem as DataRowView;
            //if (drv != null && drv.Row.Table.Columns.Contains(fieldName))
            //{
            //    return drv[fieldName] != DBNull.Value ? drv[fieldName].ToString() : string.Empty;
            //}
            //return string.Empty; // Se la colonna non esiste, mostra semplicemente una cella vuota



            // Verifica se per questo utente il campo NON deve essere inviato
            if (!VisualizzoSiNo())
            {
                return string.Empty;
            }

            DataRowView drv = dataItem as DataRowView;
            if (drv != null && drv.Row.Table.Columns.Contains(fieldName))
            {
                return drv[fieldName] != DBNull.Value ? drv[fieldName].ToString() : string.Empty;
            }

            return string.Empty;
        }
        private bool VisualizzoSiNo()
        {
            string ruolo = Session["ruolo"] != null ? Session["ruolo"].ToString() : "";

            // Esempio: restituisce true solo se l'utente appartiene a determinati ruoli/utenti
            if (ruolo == "comandante")
            {
                return true;
            }

            return false;
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);

        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 carico = 0;
            string matricola = String.Empty;
            string sigla = String.Empty;
            string dataInserimento = String.Empty;
            if (e.CommandName == "Select")
            {
                try
                {


                    // Ottieni il valore del CommandArgument
                    string commandArgument = e.CommandArgument.ToString();

                    // Separare i valori del CommandArgument usando il delimitatore "|"
                    string[] values = commandArgument.Split('|');

                    // Assicurati che ci siano almeno 3 valori
                    if (values.Length == 5)
                    {
                        carico = System.Convert.ToInt32(values[0]);    // Carico
                        matricola = values[1];     // Matricola
                        dataInserimento = values[2]; // DataInserimento
                        //sigla = values[3];  // sigla
                        HidPratica.Value = values[3]; // id
                        string dbOrigine = values[4]; // dbOrigine



                        //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                        //p.matricola = matricola;
                        if (!String.IsNullOrEmpty(dataInserimento))
                        {
                            p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLocalTime();


                        }
                        Manager mn = new Manager();
                        DataTable pratica = mn.getPraticaCaricoDataSiglaId(carico, System.Convert.ToDateTime(dataInserimento), System.Convert.ToInt32(HidPratica.Value));
                        FillScheda(pratica, mn);


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
                        sw.WriteLine("dataInserimento:" + dataInserimento + ",data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in update dati ");
                        sw.Close();
                    }
                }
            }
        }
        protected void FillScheda(DataTable pratica, Manager mn)
        {
            if (pratica.Rows.Count > 0)
            {
                Pulisci();
                txtProt.Text = pratica.Rows[0]["nr_carico"].ToString();
                // txtSigla.Text = pratica.Rows[0].ItemArray[2].ToString();
                //if (pratica.Rows[0].ItemArray[2].ToString() == Enumerate.Sigla.AG.ToString().ToUpper())
                //{
                //    divAg.Visible = true;
                //}
                //else
                //{
                //    divAg.Visible = false;

                //}
                if (!String.IsNullOrEmpty(pratica.Rows[0]["DataArrivo"].ToString()))

                    txtDataInsCarico.Text = System.Convert.ToDateTime(pratica.Rows[0]["DataArrivo"].ToString()).ToShortDateString();

                //if (pratica.Rows[0]["sigla"].ToString() == "TP")
                //{
                //    divbu.Style["display"] = "inline";
                //    divcd.Style["display"] = "inline";
                //    txtBU.Text = pratica.Rows[0]["bu"].ToString();
                //    txtCodEdificio.Text = pratica.Rows[0]["CodiceEdificio"].ToString();
                //}
                txtProvenienza.Text = pratica.Rows[0]["provenienza"].ToString().ToUpper();
                txtProvenienza.ToolTip = pratica.Rows[0]["provenienza"].ToString().ToUpper();
                txtTipoAtto.Text = pratica.Rows[0]["tipologia_atto"].ToString().ToUpper();
                txtTipoAtto.ToolTip = pratica.Rows[0]["tipologia_atto"].ToString().ToUpper();
                txtUltTipoAtto.Text = pratica.Rows[0]["UlterioreTipoAtto"].ToString().ToUpper();
                txtUltTipoAtto.ToolTip = pratica.Rows[0]["UlterioreTipoAtto"].ToString().ToUpper();
                txtGiudice.Text = pratica.Rows[0]["giudice"].ToString().ToUpper();
                TxtTipoProvvAg.Text = pratica.Rows[0]["TipoProvvedimentoAG"].ToString();
                TxtTipoProvvAg.ToolTip = pratica.Rows[0]["TipoProvvedimentoAG"].ToString().ToUpper();
                txtProdPenNr.Text = pratica.Rows[0]["ProcedimentoPen"].ToString();
                txtNominativo.Text = pratica.Rows[0]["nominativo"].ToString().ToUpper();
                txtNominativo.ToolTip = pratica.Rows[0]["nominativo"].ToString().ToUpper();
                txtIndirizzo.Text = pratica.Rows[0]["indirizzo"].ToString().ToUpper();
                txtIndirizzo.ToolTip = pratica.Rows[0]["indirizzo"].ToString().ToUpper();
                CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0]["Evasa"]);

                if (!String.IsNullOrEmpty(pratica.Rows[0]["EvasaData"].ToString()))
                {
                    //converte la data 01-01-1900 in SPACE
                    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0]["EvasaData"].ToString()); // Recupera la data dal DataTable
                    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                    {
                        txtDataDataEvasa.Text = ""; // Metti una stringa vuota
                    }
                    else
                    {
                        txtDataDataEvasa.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                    }
                }
                //  txtDataDataEvasa.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()).ToShortDateString();


                //     txtinviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();

                //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))

                //    txtDataInvio.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()).ToShortDateString();

                txtEsito.Text = pratica.Rows[0]["Esito"].ToString().ToUpper();
                txtEsito.ToolTip = pratica.Rows[0]["Esito"].ToString().ToUpper();
                //if (pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("-") || pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("/"))
                //{
                //    txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                //    txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                //}
                //else
                //{
                //    txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                //    txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                //}

                //I- mod 02/02/2026 operatori in lista
                if (!String.IsNullOrEmpty(pratica.Rows[0]["operatori"].ToString())) // 17
                {

                    ListOperatori.Items.Add(pratica.Rows[0]["operatori"].ToString());
                }
                if (!String.IsNullOrEmpty(pratica.Rows[0]["operatori2"].ToString()))
                {
                    ListOperatori.Items.Add(pratica.Rows[0]["operatori2"].ToString());
                }
                if (!String.IsNullOrEmpty(pratica.Rows[0]["operatori3"].ToString()))
                {
                    ListOperatori.Items.Add(pratica.Rows[0]["operatori3"].ToString());
                }
                //F- mod 02/02/2026 operatori in lista
                //I- mod 02/06/2026 numero esposti
                if (!String.IsNullOrEmpty(pratica.Rows[0]["NumProtRicStessoCarico"].ToString()))
                {
                    txtNumProtRicStessoCarico.Text = pratica.Rows[0]["NumProtRicStessoCarico"].ToString();
                }

                //F- mod 02/06/2026 numero esposti

                if (!String.IsNullOrEmpty(pratica.Rows[0]["DataCarico"].ToString()))
                {
                    //converte la data 01-01-1900 in SPACE
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
                //txtDataCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()).ToShortDateString();

                txtPraticaOut.Text = pratica.Rows[0]["nr_Pratica"].ToString();
               
                TxtQuartiere.Text = pratica.Rows[0]["quartiere"].ToString();
                //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                txtAnnoRicerca.Text = pratica.Rows[0]["anno"].ToString();
                //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                txtRifProtGen.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Gen"].ToString(), @"[^0-9/]", ";");  //pratica.Rows[0].ItemArray[24].ToString();
                if (!String.IsNullOrEmpty(pratica.Rows[0]["areacompetenza"].ToString()))
                {
                    txtAreaCompetenza.Text = pratica.Rows[0]["areacompetenza"].ToString().ToUpper();
                    txtAreaCompetenza.ToolTip = pratica.Rows[0]["areacompetenza"].ToString().ToUpper();
                }
                //I 23/04/2026 controllo deleghe
                if (!String.IsNullOrEmpty(pratica.Rows[0]["DataDelega"].ToString()))
                {
                    //converte la data 01-01-1900 in SPACE
                    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()); // Recupera la data dal DataTable
                    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                    {
                        txtDataDelega.Text = ""; // Metti una stringa vuota
                    }
                    else
                    {
                        txtDataDelega.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                    }
                    // txtDataDelega.Text = System.Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()).ToShortDateString();
                }
                txtGgDelega.Text = pratica.Rows[0]["GgDelega"].ToString();
                //F 23/04/2026 controllo deleghe
                //I 22/05/2026 protocollo uscita
                if (!String.IsNullOrEmpty(pratica.Rows[0]["Rif_Prot_Uscita"].ToString()))
                    txtProtUscita.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Uscita"].ToString(), @"[^0-9/]", ";"); ;
                //F 22/05/2026 protocollo uscita
                // Salva la lista nella Sessione
                Session["ListRicerca"] = pratica;
                // Puoi anche chiudere il popup se necessario
                ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                DivDettagli.Visible = true;
                DivRicerca.Visible = false;


                DataTable decretazione = new DataTable();
                //string dborigine = Session["dbtarget"].ToString();


                string dbOrigine = string.Empty;
                if (String.IsNullOrWhiteSpace(Session["uo"].ToString()))
                {
                    //se la richiesta parte dal comandante generale
                    decretazione = mn.CmdGetListDecretazione(pratica.Rows[0]["nr_carico"].ToString(), pratica.Rows[0]["id"].ToString(), pratica.Rows[0]["anno"].ToString(),"uo");
                }
                else
                {
                    dbOrigine = Session["uo"].ToString();
                    decretazione = mn.getListDecretazione(pratica.Rows[0]["nr_carico"].ToString(), pratica.Rows[0]["id"].ToString(), dbOrigine);
                    if (dbOrigine != Session["uo"].ToString())
                    {
                        SiteMaster myMaster = this.Master as SiteMaster;

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.DbNonAppartenente.GetDescription(), "warning");
                        }
                        return;
                    }
                }







                // decretazione = mn.getListDecretazione(pratica.Rows[0].ItemArray[1].ToString(), pratica.Rows[0].ItemArray[0].ToString());
                if (decretazione.Rows.Count > 0)
                {
                    GVDecretazione.DataSource = decretazione;
                    GVDecretazione.DataBind();
                    divDecretazione.Visible = true;
                }
                else
                    divDecretazione.Visible = false;

            }


        }
        private void Pulisci()
        {
            txtStampaDataCarico.Text = String.Empty;
            txtCapoArea.Text = String.Empty;
            // DdlMacroArea.Items.Clear();
            txtAnnoRicerca.Text = String.Empty;
            txtNCarico.Text = String.Empty;
            txtProcPenale.Text = String.Empty;
            //date per div evasa
            txtDataDa.Text = String.Empty;
            txtDataA.Text = String.Empty;
            txtRicAnnoVal.Text = string.Empty;
            txtRicPraticaVal.Text = string.Empty;
            txtProtGen.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicOperatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtDatArrivoA.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtDataCarico.Text = String.Empty;
            txtDataDataEvasa.Text = String.Empty;
            txtDataInsCarico.Text = String.Empty;
            txtTipoAtto.Text = String.Empty;
            txtProvenienza.Text = String.Empty;
            txtIndirizzo.Text = String.Empty;
            TxtQuartiere.Text = String.Empty;
            txtNominativo.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtAreaCompetenza.Text = String.Empty;
            txtEsito.Text = String.Empty;
            //I 23/04/2026 controllo deleghe
            txtDataDelega.Text = string.Empty;
            txtGgDelega.Text = string.Empty;
            //F 23/04/2026 controllo deleghe
            //  txtAccertatori.Text = String.Empty;
            List<string> operatoriList = new List<string>();
            CkEvasa.Checked = false;
            txtNote.Text = String.Empty;
            HfFiltroNota.Value = string.Empty;
            HfFiltroIndirizzo.Value = string.Empty;
            HfFiltroOperatori.Value = string.Empty;
            HfFiltroSigla.Value = string.Empty;
            HfFiltroNominativo.Value = string.Empty;
            //I 22/05/2026 protocollo uscita
            txtProtUscita.Text = string.Empty;
            //F 22/05/2026 protocollo uscita
            //I mod 26/08/2026 cartellina patrimonio
            txtCartellinaRic.Text = string.Empty;
            txtQuart.Text = string.Empty;
            txtBuRic.Text = string.Empty;
            txtCodEdRic.Text = string.Empty;
            //F mod 26/08/2026 cartellina patrimonio



        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Nr_Carico").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
            if (gvPopup.TopPagerRow != null)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)gvPopup.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = gvPopup.PageIndex + 1;
                    int totalPages = gvPopup.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }

        }

        protected void btNCarico_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivCaricoRicerca.Visible = true;
        }

        protected void btProcPenale_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProcPenale.Visible = true;
        }

        protected void btEvaseAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivEvasaAg.Visible = true;
        }

        protected void btProtGen_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProtGen.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivPratica.Visible = true;
        }
        protected void btValidaPratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivValidazione.Visible = true;
        }
        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivGiudice.Visible = true;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProvenienza.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivNominativo.Visible = true;
        }

        protected void btOperatori_Click(object sender, EventArgs e)
        {

            NascondiDiv();
            Pulisci();
            DivOperatori.Visible = true;

        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivIndirizzo.Visible = true;
        }
        protected void btDataCarico_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivDataArrivo.Visible = true;
        }
        protected void btNote_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivNote.Visible = true;
        }
        public void NascondiDiv()
        {
            DivRicerca.Visible = true;
            DivCarico.Visible = false;
            DivProcPenale.Visible = false;
            DivEvasaAg.Visible = false;
            DivProtGen.Visible = false;
            DivPratica.Visible = false;
            DivGiudice.Visible = false;
            DivProvenienza.Visible = false;
            DivNominativo.Visible = false;
            DivOperatori.Visible = false;
            DivIndirizzo.Visible = false;
            DivDataArrivo.Visible = false;
            DivDettagli.Visible = false;
            DivValidazione.Visible = false;
            DivGridVal.Visible = false;
            DivNote.Visible = false;
            DivBuCodEd.Visible = false;
            DivCaricoRicerca.Visible = false;
            Session.Remove("ListPratiche");
            Session.Remove("ListRicerca");
        }
        protected void gvPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPopup.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroOperatori.Value) && String.IsNullOrEmpty(HfFiltroSigla.Value) && String.IsNullOrEmpty(HfFiltroNominativo.Value))
            {
                Ricerca_Click(sender, e);
            }
            else
            {
                if (!String.IsNullOrEmpty(HfFiltroOperatori.Value))
                {
                    PopulateGridView("operatori", HfFiltroOperatori.Value);
                    // apripopup_Click(sender, e);
                }
                else
                {
                    if (!String.IsNullOrEmpty(HfFiltroSigla.Value))
                    {
                        PopulateGridView("Sigla", HfFiltroSigla.Value);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(HfFiltroNominativo.Value))
                        {
                            PopulateGridView("Nominativo", HfFiltroNominativo.Value);
                            //  apripopup_Click(sender, e);
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

            }
            //  Ricerca_Click(sender, e);
        }
        protected void txtFilterNominativo_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Nominativo", txtRicNominativo.Text };
            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNominativo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterNominativo")
            {
                columnName = "Nominativo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.
            PopulateGridView(columnName, columnName1, columnName2, HfFiltroNominativo.Value);
            //PopulateGridView(columnName, HfFiltroNominativo.Value); // Esempio di funzione di filtro
            //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }

        protected void txtFilterOperatori_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Operatori", txtRicOperatori.Text };

            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroOperatori.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; //campo db
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterOperatori")
            {
                columnName = "Operatori"; // Assumi che "arch_note" sia il campo del tuo DataSource
                columnName1 = "Operatori2";
                columnName2 = "Operatori3";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, columnName1, columnName2, HfFiltroOperatori.Value); // Esempio di funzione di filtro
                                                                                               //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }
        // esecuzione del filtro ulteriore sulla colonna indirizzo
        protected void txtFilterIndirizzo_TextChanged(object sender, EventArgs e)
        {

            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Indirizzo", txtRicIndirizzo.Text };
            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;

            string filterValue = txtFilter.Text.Trim();
            HfFiltroIndirizzo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterIndirizzo")
            {
                columnName = "indirizzo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.
            PopulateGridView(columnName, columnName1, columnName2, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
                                                                                             //PopulateGridView(columnName, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
                                                                                             //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

        }
        // Funzione  che carica i dati e applica il filtro
        private void PopulateGridView(string filterColumn = "", string filterColumn1 = "", string filterColumn2 = "", string filterValue = "")
        {

            DataTable dt = new DataTable();
            string filterExpression = string.Empty;
            dt = GetOriginalData(); // ricerco la lista nuovamente
            try
            {
                //applico il filtro
                if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                {


                    if (String.IsNullOrWhiteSpace(filterColumn1) && String.IsNullOrWhiteSpace(filterColumn2))
                    {
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    }
                    else if (String.IsNullOrWhiteSpace(filterColumn2))
                    {
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%') or {filterColumn1} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    }
                    else
                    {
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%') or {filterColumn1} LIKE ('%{filterValue.Replace("'", "''")}%') or {filterColumn2} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    }


                    DataRow[] filteredRows = dt.Select(filterExpression);

                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredDt = dt.Clone();
                        foreach (DataRow row in filteredRows)
                        {
                            filteredDt.ImportRow(row);
                        }
                        gvPopup.DataSource = filteredDt;
                    }
                    else
                    {
                        gvPopup.DataSource = null;

                    }

                }
                else
                {
                    gvPopup.DataSource = dt; // Nessun filtro
                }
                gvPopup.DataBind();
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore visualizza ");
                    sw.Close();
                }
                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = paginaChiamante;
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);
                // Response.Redirect("~/Contact.aspx?errore=" + ex.Message);


                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        //gridview per decretazione
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
                                                       //Decretazione_Click(sender, e);

        }
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
        private DataTable GetOriginalData()
        {
            DataTable pratica = new DataTable();
            DataView dv = new DataView();
            Manager mn = new Manager();
            string filtro = string.Empty;
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListRicerca"] != null)
            {


                List<string> ListRicerca = (List<string>)Session["ListRicerca"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();
                if (Session["ListPratiche"] != null)
                {
                    // Recupera la DataTable originale dalla Sessione
                    pratica = (DataTable)Session["ListPratiche"];
                }
                switch (ar[0])
                {
                    case "Nominativo":


                        filtro = $"Nominativo LIKE '%{HfFiltroNominativo.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;
                    case "Indirizzo":

                        filtro = $"Indirizzo LIKE '%{HfFiltroIndirizzo.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;
                        break;
                    case "Operatori":
                        string valoreCerca = HfFiltroOperatori.Value.Replace("'", "''");
                        filtro = $"(Operatori LIKE '%{valoreCerca}%' OR " +
                                 $"Operatori2 LIKE '%{valoreCerca}%' OR " +
                                $"Operatori3 LIKE '%{valoreCerca}%')";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;
                    case "Sigla":

                        filtro = $"Sigla LIKE '%{HfFiltroSigla.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;


                }
                if (pratica.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvPopup.DataSource = dv;
                    gvPopup.DataBind();

                    txtPratica.Enabled = false;
                    // Salva la lista nella Sessione
                    Session["ListRicerca"] = ListRicerca;
                }
            }
            else
            {
                //txtPratica.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
            return pratica;
            // return dt;
        }
        /// <summary>
        /// funzione che inserisce spaces al posto del min data value
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        //protected string FormatMyDate(object dateValue)
        //{
        //    if (dateValue == null || dateValue == DBNull.Value)
        //    {
        //        return "";
        //    }

        //    DateTime date;
        //    if (DateTime.TryParse(dateValue.ToString(), out date))
        //    {
        //        if (date == new DateTime(1900, 1, 1) || date == new DateTime(1, 1, 1))
        //        {
        //            return ""; // O " " se vuoi uno spazio fisico
        //        }
        //        return date.ToString("dd/MM/yyyy");
        //    }
        //    return ""; // Gestione di valori non validi
        //}

        protected void txtFilterSigla_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            //// Crea una lista
            //List<string> ListRicerca = new List<string> { "Sigla", txtRicNominativo.Text };
            //// Salva la lista nella Sessione
            //Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim().ToUpper();
            HfFiltroSigla.Value = filterValue;
            List<string> ListRicerca = new List<string> { "Sigla", filterValue };
            Session["ListRicerca"] = ListRicerca;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterSigla")
            {
                columnName = "Sigla"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.
            PopulateGridView(columnName, columnName1, columnName2, HfFiltroSigla.Value);
            //PopulateGridView(columnName,"","", HfFiltroSigla.Value); // Esempio di funzione di filtro
            //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }

        protected void btModifica_Click(object sender, EventArgs e)
        {
            string url = VirtualPathUtility.ToAbsolute("~/View/Modifica.aspx");
            Response.Redirect(url, false);
        }



        protected void btBack_Click(object sender, EventArgs e)
        {
            gvPopup.DataSource = Session["ListPratiche"];
            gvPopup.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }


        protected void GVPratica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Verifichiamo che la riga sia una riga di dati 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Recuperiamo il valore del campo "evasa" dal data item

                bool isEvasa = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "evasa"));

                if (!isEvasa)
                {
                    // Opzione A: Colore diretto tramite codice
                    e.Row.BackColor = System.Drawing.Color.LightCoral;


                }
            }
        }

        protected void GVPratica_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GVPratica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btValidazione_Click(object sender, EventArgs e)
        {

        }

        protected void btDecreta_Click(object sender, EventArgs e)
        {
            Session["decr"] = "true";//segnalo che provengo da visualizza
            string url = VirtualPathUtility.ToAbsolute("~/View/Modifica.aspx");
            Response.Redirect(url, false);
        }

        protected void gvPopup_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv.PageCount > 0)
            {
                lblInfoPagine.Text = $"Pagina {gv.PageIndex + 1} di {gv.PageCount}";
            }
            else
            {
                lblInfoPagine.Text = "Nessun record trovato";
            }
        }



        protected void BtDuplica_Click1(object sender, EventArgs e)
        {
            string url = VirtualPathUtility.ToAbsolute("~/View/Inserimento.aspx?dup=y");
            Response.Redirect(url, false);
        }

        protected void txtFilterNota_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Nota", txtNote.Text };

            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNota.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; //campo db
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterNota")
            {
                columnName = "Note"; // 
                columnName1 = "decr_nota";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, columnName1, columnName2, HfFiltroNota.Value); // Esempio di funzione di filtro
                                                                                        //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaNote').modal('show');", true);
        }

        protected void btChiudiPop_Click(object sender, EventArgs e)
        {
            gvNote.PageIndex = 0;
            txtNote.Text = string.Empty;
        }

        protected void gvNote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Nr_Carico").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
            if (gvNote.TopPagerRow != null)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)gvNote.TopPagerRow.FindControl("lblInfoPagineNote");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = gvNote.PageIndex + 1;
                    int totalPages = gvNote.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }



        protected void gvNote_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv.PageCount > 0)
            {
                lblInfoPagineNote.Text = $"Pagina {gv.PageIndex + 1} di {gv.PageCount}";
            }
            else
            {
                lblInfoPagineNote.Text = "Nessun record trovato";
            }
        }

        protected void gvNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNote.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);

        }

        protected void btcarico_Click(object sender, EventArgs e)
        {

            NascondiDiv();
            Pulisci();
            DivCarico.Visible = true;



        }
        protected void btBuCodEd_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivBuCodEd.Visible = true;
        }
        protected void DdlMacroArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            txtCapoArea.Text = mn.getCapoArea(DdlMacroArea.SelectedValue);
        }


    }
}