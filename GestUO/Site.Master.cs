using AjaxControlToolkit.HtmlEditor.Popups;
using DocumentFormat.OpenXml.Math;
using Microsoft.Ajax.Utilities;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using GestUO.Classi;
using static GestUO.Classi.Enumerate;

namespace GestUO
{
    public partial class SiteMaster : MasterPage
    {
        DataTable profilo = new DataTable();
        MemoryCache _cache = MemoryCache.Default;
        String user = string.Empty;
        String Profilo = string.Empty;
        String ruolo = string.Empty;
        public int PaginaIndice
        {
            get { return ViewState["PaginaIndice"] != null ? (int)ViewState["PaginaIndice"] : 0; }
            set { ViewState["PaginaIndice"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["user"] != null)
                {
                    string Vuser = Session["user"].ToString();
                    string ruolo = Session["ruolo"].ToString();
                    // string area = Session["area"].ToString();
                    Manager mn = new Manager();
                    DataTable Ricerca = mn.getUserRules(Vuser);
                    String uo = Session["uo"].ToString().ToUpper();
                    //if (!string.IsNullOrWhiteSpace(Ricerca.Rows[0]["uo"].ToString()))
                    //{
                    //    switch (Ricerca.Rows[0]["uo"].ToString().ToUpper())
                    //    {
                    //        case "DB_CHIAIA":
                    //            uo = "CHIAIA";
                    //            break;
                    //        case "DB_SCAMPIA":
                    //            uo = "SCAMPIA";
                    //            break;

                    //    }
                    //}
                    //else
                    //    uo = "COMANDANTE";
                    if (Ricerca.Rows.Count > 0)
                    {
                        lblUser.Text = "Benvenuto " + Ricerca.Rows[0]["nominativo"].ToString().ToUpper() + " - Matricola: " + Ricerca.Rows[0]["matricola"].ToString() + " - UO: " + uo;
                        userLog.Visible = true;
                        // LiHelp.Visible = true;
                        switch (Ricerca.Rows[0]["ruolo"].ToString())
                        {
                            case "sicurezza":
                                menuSicurezza.Visible = true;
                                menuEsci.Visible = true;
                                break;
                            case "dirigente":
                                menuUo.Visible = true;
                                HlinkChiaia.Visible = true;
                                HlinkScampia.Visible = true;
                                //menuCoordinamentoAtti.Visible = true;
                                //RicercaAtti.Visible = true;
                                menuEsci.Visible = true;
                                //RicercaAtti.Visible = true;
                                break;
                            case "cdr chiaia":
                            case "cdr scampia":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuEsci.Visible = true;
                                RicercaAtti.Visible = true;
                                break;
                            case "coordinamentoatti":
                                // Mostra voci specifiche per coordinamento pg
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivio.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                RicercaArchivioUotp.Visible = true;
                                menuOperatori.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuSegreteria.Visible = false;
                                menuSicurezza.Visible = false;
                                menuEsci.Visible = true;
                                GestionePraticaUote.Visible = true;
                                //menuHome.Visible = true;
                                //if (Session["profilo"].ToString() != Enumerate.Profilo.accertatore.GetHashCode().ToString())
                                //{
                                //    menuNuovaScheda.Visible = false;
                                //    menuRicercaScheda.Visible = true;
                                //}
                                InserimentoAtti.Visible = true;
                                //ModificaAtti.Visible = true;
                                // ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                if (ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoAtti.ToString().ToUpper())
                                    StatisticheAtti.Visible = true;
                                if (ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
                                    EstraiStatistiche.Visible = true;
                                menuFureria.Visible = true;
                                TurnoMensile.Visible = true;

                                break;
                            case "operatori":
                                // Mostra voci per utenti standard
                                menuSicurezza.Visible = false;
                                menuCoordinamentoAtti.Visible = false;
                                menuOperatori.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                PG.Visible = true;
                                GestionePraticaUote.Visible = true;
                                //if (Session["profilo"].ToString().Contains("R"))
                                //{
                                //    menuScadenziario.Visible = true;
                                //}
                                //        if (Session["profilo"].ToString().Contains(Enumerate.Profilo.accertatore.GetHashCode().ToStringInvariant()))
                                //      {
                                NuovaRapportino.Visible = true;
                                RicercaRapportino.Visible = true;
                                //EstraiStatistiche.Visible = false;
                                //    }
                                //             menuArchivio.Visible = true;
                                //subMenuUOTE.Visible = true;
                                //subMenuUOTP.Visible = true;
                                // RicercaArchivioUotp.Visible = true;
                                mnGestioneAuto.Visible = true;

                                //  RicercaArchivioUote.Visible = true;
                                //if (Session["profilo"].ToString().Contains(Enumerate.Profilo.tre.GetHashCode().ToStringInvariant()))
                                //{
                                //    Decretazione.Visible = true;
                                //    Attivita.Visible = true;
                                //}
                                //menuFureria.Visible = true;
                                //Turnazione.Visible = true;
                                //AccTrunoMensile.Visible = true;
                                break;
                            //case "urp":
                            //    // Mostra voci per utenti standard
                            //    menuSicurezza.Visible = false;
                            //    menuCoordinamentoAtti.Visible = false;
                            //    menuOperatori.Visible = false;
                            //    menuSegreteria.Visible = false;
                            //    menuAmministratore.Visible = false;
                            //    menuEsci.Visible = true;
                            //    // menuHome.Visible = true;
                            //    PG.Visible = true;
                            //    GestionePraticaUote.Visible = true;

                            //    menuNuovaScheda.Visible = false;
                            //    menuRicercaScheda.Visible = false;
                            //    EstraiStatistiche.Visible = false;

                            //    menuArchivio.Visible = true;
                            //    subMenuUOTE.Visible = true;
                            //    subMenuUOTP.Visible = true;
                            //    RicercaArchivioUotp.Visible = true;
                            //    mnGestioneAuto.Visible = false;

                            //    RicercaArchivioUote.Visible = true;

                            //    Decretazione.Visible = false;
                            //    Attivita.Visible = false;

                            //    menuFureria.Visible = true;
                            //    Turnazione.Visible = true;
                            //    AccTrunoMensile.Visible = true;
                            //    //Urp.Visible = true;
                            //    break;
                            case "PG":
                                // Mostra voci per utenti standard
                                //menuCoordinamentoAtti.Visible = false;
                                menuSicurezza.Visible = false;
                                menuOperatori.Visible = false;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;
                                menuArchivio.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                RicercaArchivioUotp.Visible = true;
                                GestionePraticaUote.Visible = true;
                                if (ruolo.ToUpper() == Enumerate.Ruolo.PG.ToString().ToUpper())
                                {
                                    menuCoordinamentoAtti.Visible = true;
                                    InserimentoAtti.Visible = false;
                                    //ModificaAtti.Visible = false;
                                    RicercaAtti.Visible = true;
                                }
                                break;
                            case "archivio":
                                // Mostra voci per utenti standard
                                // menuHome.Visible = true;
                                menuSicurezza.Visible = false;
                                menuArchivio.Visible = true;
                                menuCoordinamentoAtti.Visible = true;
                                RicercaAtti.Visible = true;
                                menuOperatori.Visible = false;
                                menuSegreteria.Visible = false;
                                menuAmministratore.Visible = false;
                                StatisticheAtti.Visible = true;
                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                //if (area.ToUpper() == Enumerate.Area.UOTE.ToString().ToUpper())
                                if (Session["profilo"].ToString().Contains(Enumerate.Profilo.V.ToString()))

                                {

                                    InserimentoArchivioUote.Visible = false;
                                    InserimentoArchivioUotp.Visible = false;
                                }
                                else
                                {
                                    InserimentoArchivioUote.Visible = true;
                                    InserimentoArchivioUotp.Visible = true;
                                }
                                GestionePraticaUote.Visible = true;
                                menuManTabelle.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;

                                RicercaArchivioUotp.Visible = true;

                                break;
                            case "admin":
                                //
                                // CaricaScheda.Visible = true;
                                // Mostra voci per utenti standard
                                menuSicurezza.Visible = false;
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivio.Visible = true;
                                menuOperatori.Visible = true;
                                NuovaRapportino.Visible = true;
                                RicercaRapportino.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = true;
                                menuManTabelle.Visible = true;
                                menuEsci.Visible = true;
                                //  menuHome.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                InserimentoArchivioUote.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;
                                StatisticheAtti.Visible = true;

                                //menuScadenziario.Visible = true;

                                menuFureria.Visible = true;
                                TurnoMensile.Visible = true;
                                FSchedaDipendente.Visible = true;


                                //A1.Visible = true;

                                //*
                                InserimentoAtti.Visible = true;
                                //ModificaAtti.Visible = true;
                                //  ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                EstraiStatistiche.Visible = true;
                                GestionePraticaUote.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                InserimentoArchivioUotp.Visible = true;
                                RicercaArchivioUotp.Visible = true;
                                menuAttivita.Visible = true;
                                Amministratore.Visible = true;
                                SchedaCarburante.Visible = true;
                                //Urp.Visible = true;
                                //*
                                break;
                           
                            case "fureria":
                                menuSicurezza.Visible = false;
                                menuFureria.Visible = true;
                                TurnoMensile.Visible = true;
                                menuEsci.Visible = true;
                                FSchedaDipendente.Visible = true;
                                break;
                            default:

                                break;
                        }
                    }
                    //else
                    //    lblMsg.Text = "Matricola assente";
                }
                //else
                //    lblMsg.Text = "Utente non loggato";

            }
        }
        public void ImpostaLogoSfondo(string relativoUrl)
        {
            string urlCompleto = ResolveUrl(relativoUrl);

            divSfondo.Style["--url-sfondo"] = $"url('{urlCompleto}')";
        }
        protected void Esci_Click(object sender, EventArgs e)
        {
            Session.Remove("user");
            Session.Remove("POP");
            Session.Remove("filetemp");
            Session.Remove("profilo");
            Session.Remove("ruolo");
            Session.Remove("ListRicerca");
            Session.Remove("popAperto");
            Session.Remove("popApertoRicercaScheda");
            Session.Remove("ListPratiche");
            Session.Remove("ListRicercaTp");
            Session.Remove("arc");
            Session.Remove("area");
            Session.Remove("MacroArea");
            Session.Remove("ListRicercaGestioneAuto");
            Session.Remove("ListAuto");
            Session.Remove("TurniMensili");
            Session.Remove("ListScadenziario");
            Session.Remove("ListRicercaFiltro");
            Session.Remove("uo");
            Session.Abandon();
            string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
            Response.Redirect(url, false);
            //Response.Redirect(/Default.aspx"), false);
        }
        public void CaricaListDelegheScadenza()
        {
            //
            string msg = string.Empty;
            Manager mn = new Manager();
            //int giorni = string.IsNullOrWhiteSpace(txtGiorniScad.Text) ? 60 : Convert.ToInt32(txtGiorniScad.Text);
            DataTable CaricaListDelegheScadenza = mn.getListDelegheInScadenza(Session["MacroArea"].ToString(), Session["user"].ToString(), out msg);


            string filtroTesto = "";

            // Specifichiamo System.Web.UI.Control e System.Web.UI.WebControls.TextBox
            var headerTextBox = rptDelegheScadenza.Controls
                .Cast<System.Web.UI.Control>()
                .Select(c => c.FindControl("txtFiltroMacroArea"))
                .FirstOrDefault(c => c != null) as System.Web.UI.WebControls.TextBox;

            filtroTesto = headerTextBox?.Text ?? "";


            // 3. Applica il filtro se è stato scritto qualcosa
            if (!string.IsNullOrEmpty(filtroTesto))
            {
                DataView dv = CaricaListDelegheScadenza.DefaultView;
                dv.RowFilter = string.Format("Macro_area LIKE '%{0}%'", filtroTesto.Replace("'", "''"));
                CaricaListDelegheScadenza = dv.ToTable();
            }

            // paginazione
            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = CaricaListDelegheScadenza.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = 15; // Quanti record vuoi per pagina?
            pds.CurrentPageIndex = PaginaIndice;

            // Gestione visibilità pulsanti
            btnPrecedente.Enabled = !pds.IsFirstPage;
            btnSuccessivo.Enabled = !pds.IsLastPage;

            // Aggiornamento Label contatore
            lblPaginaCorrente.Text = (PaginaIndice + 1).ToString();
            lblTotalePagine.Text = pds.PageCount.ToString();
            ///////////
            rptDelegheScadenza.DataSource = pds;
            rptDelegheScadenza.DataBind();


            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#DelegheScadenza').modal('show');", true);

        }
        protected void btnPrecedente_Click(object sender, EventArgs e)
        {
            PaginaIndice -= 1;
            CaricaListDelegheScadenza();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#DelegheScadenza').modal('show');", true);
        }

        protected void btnSuccessivo_Click(object sender, EventArgs e)
        {
            PaginaIndice += 1;
            CaricaListDelegheScadenza();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#DelegheScadenza').modal('show');", true);
        }
        //protected void txtGiorniScad_TextChanged(object sender, EventArgs e)
        //{
        //    CaricaListDelegheScadenza();
        //}
        protected void btChiudiPop_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#DelegheScadenza').modal('hide');", true);
        }
        // Metodo per mostrare il popup. 
        // tipoMessaggio può essere: "info", "success", "warning", "danger" (colori Bootstrap)
        public void MostraMessaggio(string titolo, string messaggio, string tipoMessaggio = "info")
        {
            // 1. Imposta i testi
            lblModalTitolo.Text = titolo;
            TxtMessage.InnerText = messaggio;

            // 2. Imposta il colore dell'header in base al tipo
            string classeColore = "modal-header"; // Classe base
            switch (tipoMessaggio.ToLower())
            {
                case "success":
                    classeColore += " bg-success"; // Verde
                    break;
                case "danger":
                case "error":
                    classeColore += " bg-danger"; // Rosso
                    break;
                case "warning":
                    classeColore += " bg-warning"; // Giallo/Arancio
                    break;
                default:
                    classeColore += " bg-info"; // Azzurro (Info)
                    break;
            }

            // Assegna la classe al div header
            modalHeaderColor.Attributes["class"] = classeColore;

            // 3. Lancia lo script per aprire il modale
            string script = "$('#SiteModal').modal('show');";
            hfMasterParam.Value = "fatto";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ApriModalMaster", script, true);
        }

        public void MostraConferma(string titolo, string messaggio, string urlDiDestinazione)
        {
            // 1. Imposta i testi nel modale
            lblTitoloConferma.Text = titolo;
            lblTestoConferma.Text = messaggio;

            // 2. Prepara lo script JS chiamando la funzione che abbiamo scritto sopra
            // Passiamo Titolo, Messaggio e URL al JavaScript
            string script = $"ApriModalConferma('{titolo}', '{messaggio}', '{urlDiDestinazione}');";

            // 3. Esegue lo script
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "OpenConfModal", script, true);
        }
        protected void btChiudiPopUp_Click(object sender, EventArgs e)
        {
            hfMasterParam.Value = "fatto";
            //string script = "$('#SiteModal').modal('hide');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ApriModalMaster", "", false);
        }

        protected void menuScadenziario_ServerClick(object sender, EventArgs e)
        {
            // txtGiorniScad.Text = string.Empty;
            CaricaListDelegheScadenza();
        }

        protected void txtFiltroMacroArea_TextChanged(object sender, EventArgs e)
        {
            PaginaIndice = 0;
            CaricaListDelegheScadenza();
        }

        protected void lnkProtocollo_Click(object sender, EventArgs e)
        {

        }

        protected void rptDelegheScadenza_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // 1. Recuperiamo i dati della riga corrente
                var row = (DataRowView)e.Item.DataItem;
                System.Web.UI.WebControls.Literal litGiorni = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litGGScadenza");

                System.Web.UI.WebControls.Literal litGiorniRimanenti = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litGiorniRimanenti");
                //giorni di scadenza della delega , si trova in una colonna del repeater non visibile
                int giorniDisposizione = Convert.ToInt32(litGiorniRimanenti.Text);

                if (litGiorni != null && row["DataDelega"] != DBNull.Value)
                {
                    // 2. Parametri del calcolo
                    DateTime dataCarico = Convert.ToDateTime(row["DataDelega"]);

                    // 3. Calcolo la Data di Scadenza (Data delega + giorni a disposizione)
                    DateTime dataScadenza = dataCarico.AddDays(giorniDisposizione);

                    // 4. Calcolo la differenza con la data di oggi (solo parte data, senza ore)
                    TimeSpan differenza = dataScadenza.Date - DateTime.Now.Date;
                    int giorniMancanti = differenza.Days;

                    // 5. Formattazione dell'output con un tocco di colore (opzionale)
                    if (giorniMancanti < 0)
                    {
                        // Scaduta
                        litGiorni.Text = $"<span class='text-danger fw-bold'>SCADUTA ({Math.Abs(giorniMancanti)} gg fa)</span>";
                    }
                    else if (giorniMancanti == 0)
                    {
                        // Scade oggi
                        litGiorni.Text = "<span class='text-warning fw-bold'>SCADE OGGI</span>";
                    }
                    else
                    {
                        // Ancora in tempo
                        litGiorni.Text = $"<span class='text-success fw-bold'>MANCANO {giorniMancanti} gg</span>";
                    }
                }

            }
        }

        protected void btBack_Click(object sender, EventArgs e)
        {
            CaricaListDelegheScadenza();
        }
    }


}
