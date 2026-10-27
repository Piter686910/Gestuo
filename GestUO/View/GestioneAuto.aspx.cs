using AjaxControlToolkit.HtmlEditor.Popups;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestUO;
using GestUO.Classi;
using static GestUO.Classi.Enumerate;                



namespace GestUO
{
    public partial class GestioneAuto : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();

        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string Isdecr = string.Empty;
        public string uoSelezionata { get; set; }
        public string Vuser { get; set; }
        public string ruolo { get; set; }
        public string TitoloUO = string.Empty;
        SiteMaster myMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            myMaster = this.Master as SiteMaster;
            Session["PaginaChiamante"] = "/View/GestioneAuto.aspx";

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Hfuser.Value = Session["ruolo"].ToString();
                ruolo = Session["ruolo"].ToString();
                uoSelezionata = Session["uo"].ToString();
                Manager mn = new Manager();
                bool haPermesso = mn.VerificaPermesso(Session["ruolo"].ToString(), Session["PaginaChiamante"].ToString());
                if (!haPermesso)
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.NoAutorizzato.GetDescription(), "danger");
                        Session.Abandon();
                        return;

                    }
                }
                switch (uoSelezionata.ToUpper())
                {
                    case "CHIAIA":
                        TitoloUO = " GESTIONE CARTE CARBURANTE U.O. CHIAIA";
                        break;
                    case "SCAMPIA":
                        TitoloUO = " GESTIONE CARTE CARBURANTE U.O. SCAMPIA";
                        break;
                    case "TUTTE":
                        TitoloUO = " GESTIONE CARTE CARBURANTE";
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
                //if (Session["ListRicercaGestioneAuto"] != null)
                //{
                //    DataTable auto = (DataTable)Session["ListRicercaGestioneAuto"];

                //    Hfid.Value = auto.Rows[0]["id"].ToString();

                //}

                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                //ProtocolloLiteral.Text = decodedText;
                CaricaDLL();
                txtMese.Text = DateTime.Now.ToString("MMMM").ToUpper();
                txtAnno.Text = DateTime.Now.ToString("yyyy");
                TxtData.Text = DateTime.Now.ToShortDateString();// ToString("yyyy");
                if (ruolo == Enumerate.Ruolo.operatori.ToString())
                {
                    btCerca.Visible = false;
                    btStampa.Visible = false;
                }
                else
                {
                    txtAnno.Enabled = true;
                    txtMese.Enabled = true;
                    divInserimento.Visible = false;
                    btSalva.Visible = false;
                }
            }

        }

        protected void btChiudiAvvertenze_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalAvvertenze')); modal.hide();", true);

        }
        public float ConvertiStringaInFloat(string inputStringa)
        {
            float valoreConvertito = 0f;
            CultureInfo culturaItaliana = new CultureInfo("it-IT");
            NumberStyles stile = NumberStyles.Currency;

            if (float.TryParse(
                    inputStringa,
                    stile,
                    culturaItaliana,
                    out valoreConvertito))
            {
                return valoreConvertito;
            }
            else
            {
                throw new FormatException($"La stringa '{inputStringa}' non è un formato numerico valido per float.");
            }
        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean ins = false;
                int num = 0;
                Manager mn = new Manager();
                GestAuto auto = new GestAuto();
                auto.sigla = DdlSigla.SelectedItem.Text;
                auto.targa = txtTarga.Text;
                auto.data = System.Convert.ToDateTime(TxtData.Text);
                auto.ora = System.Convert.ToDateTime(txtOra.Text);
                auto.stan = txtStan.Text;
                auto.litri = ConvertiStringaInFloat(txtLitri.Text);
                auto.tipoCarburante = DdlCarburante.SelectedItem.Text;
                auto.euro = ConvertiStringaInFloat(txtEuro.Text);
                auto.indirizzo = txtIndirizzo.Text.ToUpper();
                auto.autista = txtAutista.Text.ToUpper();
                auto.mese = txtMese.Text;
                auto.anno = System.Convert.ToInt16(txtAnno.Text);
                auto.verificato = false;
                auto.uo = uoSelezionata.ToUpper();
                DataTable prog = mn.MaxProgressivoGestioneAuto(auto.anno, auto.mese);
                num = Convert.ToInt32(prog.Rows[0].ItemArray[0].ToString());
                if (num == 0)

                    num = 1;
                else
                    num++;

                auto.progressivo = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString("D2") + "/" + num;
                if (!string.IsNullOrEmpty(Vuser))
                {
                    auto.matricola = Vuser;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.SScaduta.GetDescription() + "'); $('#errorModal').modal('show');", true);

                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                    Response.Redirect(url, false);
                }
                SiteMaster myMaster = this.Master as SiteMaster;
                long idN = 0;
                ins = mn.InsGestioneAuto(auto, out idN);
                if (!ins)
                {
                    if (myMaster != null)

                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.InsKo.GetDescription(), "danger");

                    // }

                }

                else
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", "Il numero progressivo è " + auto.progressivo + ", inserire sulla ricevuta la Sigla, il cognome e progressivo. Grazie", "success");
                        Pulisci();
                    }
                }
            }
            catch (Exception ex)
            {

                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/GestioneAuto.aspx";
            }
        }
        private void Pulisci()
        {
            txtTarga.Text = string.Empty;
            TxtData.Text = string.Empty;
            txtOra.Text = string.Empty;
            txtStan.Text = string.Empty;
            txtLitri.Text = string.Empty;
            DdlCarburante.ClearSelection();

            txtEuro.Text = string.Empty;
            txtIndirizzo.Text = string.Empty;
            txtAutista.Text = string.Empty;
            DdlSigla.ClearSelection();


        }



        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalAvvertenze').modal('show');", true);
        }

        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable Sigla = mn.getSiglaAuto(uoSelezionata);
                DdlSigla.DataSource = Sigla; // Imposta il DataSource della DropDownList
                DdlSigla.DataTextField = "sigla"; // Il campo visibile
                DdlSigla.DataValueField = "ID_sigla"; // Il valore associato a ogni opzione
                DdlSigla.DataBind();
                DdlSigla.Items.Insert(0, new ListItem("", "0"));

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
                Session["PaginaChiamante"] = "~/View/GestioneAuto.aspx";
                //  Response.Redirect("~/Contact.aspx");

            }
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Ottieni il valore della colonna "ID"
            //    string id = DataBinder.Eval(e.Row.DataItem, "ID_quartiere").ToString();

            //    // Aggiungi l'attributo per il doppio clic
            //    e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            //    e.Row.Style["cursor"] = "pointer";
            //}
        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    string selectedValue = e.CommandArgument.ToString();

            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    txtQuartiere.Text = selectedValue;
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }


        protected void DdlSigla_SelectedIndexChanged(object sender, EventArgs e)
        {
            //    CaricaDLL();

        }


        protected void btSalvaGiudice_Click(object sender, EventArgs e)
        {
            Salva_Click(sender, e); ;
        }


        protected void DdlSigla_TextChanged(object sender, EventArgs e)
        {


        }

        protected void btStampa_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable scheda = mn.getListAuto(txtMese.Text, Convert.ToInt32(txtAnno.Text), uoSelezionata);
            // Ordina i dati per Sigla (numerica) e poi per Data (più recente prima)
            var datiOrdinati = scheda.AsEnumerable()
            .OrderBy(row =>
            {
                string sigla = row.Field<string>("Sigla");
                string soloNumeri = System.Text.RegularExpressions.Regex.Replace(sigla ?? "", @"\D", "");
                return int.TryParse(soloNumeri, out int num) ? num : 0;
            })
            .ThenByDescending(row => row.Field<DateTime>("Data")) // Ordina per Data (più recente prima)
            .CopyToDataTable();

            Routine stampa = new Routine();
            switch (uoSelezionata.ToUpper())
            { 
                case "SCAMPIA":
                    stampa.CreaPdfSchedaCarburanteScampia(datiOrdinati);
                    break;
                default:
                    break;
            }
            
        }

        protected void DdlSigla_SelectedIndexChanged1(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable Sigla = mn.getAutoBySigla(DdlSigla.SelectedItem.Text, uoSelezionata);
            if (Sigla.Rows.Count > 0)
            {
                txtTarga.Text = Sigla.Rows[0]["targa"].ToString();
            }
        }

        protected void btCerca_Click(object sender, EventArgs e)
        {
            Session.Remove("ListRicercaGestioneAuto");
            Manager mn = new Manager();
            DataTable listaAuto = mn.getListAuto(txtMese.Text, Convert.ToInt32(txtAnno.Text), uoSelezionata);


            if (listaAuto.Rows.Count > 0)
            {
                // Ordina i dati per Sigla (numerica) e poi per Data (più recente prima)
                var datiOrdinati = listaAuto.AsEnumerable()
                .OrderBy(row =>
                {
                    string sigla = row.Field<string>("Sigla");
                    string soloNumeri = System.Text.RegularExpressions.Regex.Replace(sigla ?? "", @"\D", "");
                    return int.TryParse(soloNumeri, out int num) ? num : 0;
                })
                .ThenByDescending(row => row.Field<DateTime>("Data")) // Ordina per Data (più recente prima)
                .CopyToDataTable();

                Session["ListRicercaGestioneAuto"] = datiOrdinati;
                lblNumRighe.Text = " - Num. righe trovate: " + listaAuto.Rows.Count.ToString();
                gvDett.DataSource = datiOrdinati;
                gvDett.DataBind();
                btBack.Visible = true;
                DivGrid.Visible = true;
            }
        }

        protected void gvDett_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvDett.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            btCerca_Click(sender, e);

        }
        protected void gvDett_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Ottieni il valore della colonna "ID"
            //    string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

            //    // Aggiungi l'attributo per il doppio clic
            //    e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            //    e.Row.Style["cursor"] = "pointer";
            //}



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
                if (gvDett.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvDett.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvDett.PageIndex + 1;
                        int totalPages = gvDett.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }

            }

        }
        protected void gvDett_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 4)
                {
                    GestAuto obj = new GestAuto();
                    obj.id = System.Convert.ToInt32(values[0]);    // id riga
                    obj.targa = values[1];     // targa
                    obj.data = Convert.ToDateTime(values[2]); // DataInserimento
                    obj.autista = values[3]; // autista
                    //Hid.Value = values[4]; // id

                    obj.matricola = Vuser;
                    obj.dataVerifica = DateTime.Now.Date;
                    Manager mn = new Manager();
                    Boolean upd = mn.UpdGestioneAutoById(obj);
                    if (upd)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Rifornimento per : " + obj.targa + " verificato." + "'); $('#errorModal').modal('show');", true);
                        btCerca_Click(sender, e);

                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Errore in modifica tabella gestione auto." + "'); $('#errorModal').modal('show');", true);

                    }
                }
            }
            if (e.CommandName == "Edit")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 1)
                {
                    Hfid.Value = values[0]; // id

                    //Manager mn = new Manager();
                    //DataTable auto = mn.getAutoById(System.Convert.ToInt32(Hfid.Value));
                    //if (auto.Rows.Count > 0)
                    //{
                    //    txtProgressivoMod.text = auto.Rows[0]["progressivo"].ToString();    


                    //}
                }
            }
        }
        protected void txtFilterProgressivo_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "progressivo", DdlSigla.SelectedItem.Text };
            // Salva la lista nella Sessione
            Session["ListAuto"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfProgressivo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterProgressivo")
            {
                columnName = "progressivo";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfProgressivo.Value);
        }
        protected void txtFilterSigla_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "sigla", DdlSigla.SelectedItem.Text };
            // Salva la lista nella Sessione
            Session["ListAuto"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroSigla.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterSigla")
            {
                columnName = "sigla";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroSigla.Value);
        }
        protected void txtFilterData_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "data", TxtData.Text };
            // Salva la lista nella Sessione
            Session["ListAuto"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroData.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterData")
            {
                columnName = "data";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroData.Value); // Esempio di funzione di filtro
                                                              //            apripopup_Click(sender, e);
                                                              //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }
        private void PopulateGridView(string filterColumn = "", string filterValue = "")
        {

            DataTable dt = new DataTable();

            dt = GetOriginalData(); // ricerco la lista nuovamente
            try
            {
                //applico il filtro
                string filterExpression = string.Empty;
                if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                {

                    if (filterColumn == "data")
                        filterExpression = $"{filterColumn} IN ('{filterValue.Replace("'", "''")}')";
                    else
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    DataRow[] filteredRows = dt.Select(filterExpression);

                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredDt = dt.Clone();
                        foreach (DataRow row in filteredRows)
                        {
                            filteredDt.ImportRow(row);
                        }
                        gvDett.DataSource = filteredDt;
                    }
                    else
                    {
                        gvDett.DataSource = null;
                    }

                }
                else
                {
                    gvDett.DataSource = dt; // Nessun filtro
                }
                gvDett.DataBind();
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        private DataTable GetOriginalData()
        {
            DataTable auto = new DataTable();
            DataView dv = new DataView();
            Manager mn = new Manager();
            string filtro = string.Empty;
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListRicercaGestioneAuto"] != null)
            {


                List<string> ListRicerca = (List<string>)Session["ListAuto"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();
                if (Session["ListRicercaGestioneAuto"] != null)
                {
                    // Recupera la DataTable originale dalla Sessione
                    auto = (DataTable)Session["ListRicercaGestioneAuto"];
                }
                switch (ar[0])
                {
                    case "data":


                        filtro = $"data in ('{HfFiltroData.Value}')";
                        dv = new DataView(auto);

                        dv.RowFilter = filtro;

                        break;
                    case "sigla":

                        filtro = $"sigla LIKE '%{HfFiltroSigla.Value}%'";
                        dv = new DataView(auto);

                        dv.RowFilter = filtro;
                        break;
                    //case "Accertatori":

                    //    filtro = $"Accertatori LIKE '%{HfFiltroAccertatori.Value}%'";
                    //    dv = new DataView(pratica);

                    //    dv.RowFilter = filtro;

                    //    break;
                    case "progressivo":

                        filtro = $"progressivo LIKE '%{HfProgressivo.Value}%'";
                        dv = new DataView(auto);

                        dv.RowFilter = filtro;
                        break;

                }
                if (auto.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvDett.DataSource = dv;
                    gvDett.DataBind();

                    //txtPratica.Enabled = false;
                }
            }
            else
            {
                //txtPratica.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
            return auto;
            // return dt;
        }

        protected void gvDett_DataBound(object sender, EventArgs e)
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

        protected void btBack_Click(object sender, EventArgs e)
        {
            gvDett.DataSource = Session["ListRicercaGestioneAuto"];
            gvDett.DataBind();
            DivGrid.Visible = true;
        }

        protected void gvDett_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //edito il rigo selezionato
            gvDett.EditIndex = e.NewEditIndex;
            Manager mn = new Manager();
            DataTable auto = new DataTable();
            auto = mn.getListAuto(txtMese.Text, Convert.ToInt32(txtAnno.Text), uoSelezionata);
            if (auto.Rows.Count > 0)
            {
                // Ordina i dati per Sigla (numerica) e poi per Data (più recente prima)
                var datiOrdinati = auto.AsEnumerable()
                .OrderBy(row =>
                {
                    string sigla = row.Field<string>("Sigla");
                    string soloNumeri = System.Text.RegularExpressions.Regex.Replace(sigla ?? "", @"\D", "");
                    return int.TryParse(soloNumeri, out int num) ? num : 0;
                })
                .ThenByDescending(row => row.Field<DateTime>("Data")) // Ordina per Data (più recente prima)
                .CopyToDataTable();

                gvDett.DataSource = datiOrdinati;
                gvDett.DataBind();
            }
        }

        protected void gvDett_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Esce dalla modalità modifica
            gvDett.EditIndex = -1;
            // Ricarica i dati per mostrare le TextBox
            // Decretazione_Click(sender, e);
            btCerca_Click(sender, e);
        }

        protected void gvDett_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GestAuto auto = new GestAuto();
            try
            {

                // A. Recupera l'ID del record (da DataKeyNames)
                int idRifAuto = Convert.ToInt32(gvDett.DataKeys[e.RowIndex].Value);

                // B. Recupera i nuovi valori inseriti nelle TextBox
                // Devi cercare i controlli usando l'ID che hai dato nell'EditItemTemplate
                GridViewRow row = gvDett.Rows[e.RowIndex];
                Manager mn = new Manager();

                TextBox txtProgressivoMod = (TextBox)row.FindControl("txtProgressivoMod");
                TextBox txtTargaMod = (TextBox)row.FindControl("txtTargaMod");
                TextBox txtOraMod = (TextBox)row.FindControl("txtOraMod");
                TextBox txtDataMod = (TextBox)row.FindControl("txtDataMod");
                TextBox txtSiglaMod = (TextBox)row.FindControl("txtSiglaMod");
                TextBox txtStanMod = (TextBox)row.FindControl("txtStanMod");
                TextBox txtLitriMod = (TextBox)row.FindControl("txtLitriMod");
                TextBox txtEuroMod = (TextBox)row.FindControl("txtEuroMod");
                TextBox txtIndirizzoMod = (TextBox)row.FindControl("txtIndirizzoMod");

                auto.matricola = string.IsNullOrWhiteSpace(Session["user"].ToString()) ? string.Empty : Session["user"].ToString();
                auto.data = string.IsNullOrWhiteSpace(txtDataMod.Text.Trim()) ? DateTime.MinValue : System.Convert.ToDateTime(txtDataMod.Text);
                // decr.data = System.Convert.ToDateTime(txtDataMod.Text);
                auto.id = idRifAuto;
                auto.targa = string.IsNullOrWhiteSpace(txtTargaMod.Text.Trim()) ? string.Empty : txtTargaMod.Text.Trim();
                auto.progressivo = string.IsNullOrWhiteSpace(txtProgressivoMod.Text.Trim()) ? string.Empty : txtProgressivoMod.Text.Trim();
                auto.sigla = string.IsNullOrWhiteSpace(txtSiglaMod.Text.Trim()) ? string.Empty : txtSiglaMod.Text.Trim();
                auto.ora = Convert.ToDateTime(string.IsNullOrWhiteSpace(txtOraMod.Text.Trim()) ? string.Empty : txtOraMod.Text.Trim());
                auto.stan = string.IsNullOrWhiteSpace(txtStanMod.Text.Trim()) ? string.Empty : txtStanMod.Text.Trim();
                auto.litri = ConvertiStringaInFloat(string.IsNullOrWhiteSpace(txtLitriMod.Text.Trim()) ? string.Empty : txtLitriMod.Text.Trim());
                auto.euro = ConvertiStringaInFloat(string.IsNullOrWhiteSpace(txtEuroMod.Text.Trim()) ? string.Empty : txtEuroMod.Text.Trim());
                auto.indirizzo = string.IsNullOrWhiteSpace(txtIndirizzoMod.Text.Trim()) ? string.Empty : txtIndirizzoMod.Text.Trim();



                SiteMaster myMaster = this.Master as SiteMaster;

                Boolean resp = mn.UpdGestioneAuto(auto);
                if (resp)
                {
                    //richiama popup dalla site master


                    //if (myMaster != null)
                    //{
                    // 2. Chiamo il metodo pubblico
                    // myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdRegistroOk.GetDescription(), "success");
                    gvDett.EditIndex = -1;
                    btCerca_Click(sender, e);
                    // }
                }


            }
            catch (Exception ex)
            {
                // Gestione Errore
                //richiama popup dalla site master
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine(ex.Message + @" - Errore in GVDecretazione_RowUpdating modifica.cs ");
                        sw.Close();
                    }

                }
            }
        }


        protected void txtSiglaMod_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSiglaMod = (TextBox)sender;

            // 2. riga della GridView che contiene questa TextBox
            GridViewRow row = (GridViewRow)txtSiglaMod.NamingContainer;

            // 3. Preleva il valore modificato
            string siglaInserita = txtSiglaMod.Text;
            Manager mn = new Manager();
            DataTable Sigla = mn.getAutoBySigla(siglaInserita, uoSelezionata);

            if (Sigla.Rows.Count > 0)
            {
                // 5. Cerca la TextBox "txtTarga" all'interno della stessa riga
                TextBox txtTargaMod = (TextBox)row.FindControl("txtTargaMod");
                txtTargaMod.Text = Sigla.Rows[0]["targa"].ToString();

            }
        }
    }
}