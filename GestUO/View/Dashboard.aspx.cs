using System;
using System.Data;
using System.IO;
using System.Runtime.Caching;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestUO.Classi;
using static GestUO.Classi.Enumerate;

namespace GestUO
{
    public partial class _Dashboard : Page
    {
        public string Vuser { get; set; }
        public string ruolo { get; set; }
        public string uoSelezionata { get; set; }
        public string TitoloUO { get; set; } = string.Empty;
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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                ruolo = Session["ruolo"].ToString();
                uoSelezionata = Session["uo"].ToString();

            }
            switch (uoSelezionata.ToUpper())
            {
                case "CHIAIA":
                    TitoloUO = " DASHBOARD AMMINISTRATORE U.O. CHIAIA";
                    txtUO.Text = "CHIAIA";
                    break;
                case "SCAMPIA":
                    TitoloUO = " DASHBOARD AMMINISTRATORE U.O. SCAMPIA";
                    txtUO.Text = "SCAMPIA";
                    break;
                case "TUTTE":
                    TitoloUO = " DASHBOARD AMMINISTRATORE";
                    break;
                default:
                    break;
            }
            if (!String.IsNullOrEmpty(ruolo))
            {
                if (ruolo != "admin")
                {
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                    Response.Redirect(url, false);
                    // Response.Redirect("~/View/default.aspx");
                }
            }
            if (!IsPostBack)
            {
                
                CaricaPersonale();
                CaricaListOperatori();
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            divNewUtente.Visible = false;
            divDestra.Visible = false;
            divReset.Visible = true;
            divCheck.Visible = false;
            GVcheck.Visible = false;
        }
        //protected void ModificaP_Click(object sender, EventArgs e)
        //{
        //    SiteMaster myMaster = this.Master as SiteMaster;
        //    //cripto la passowrd
        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(txtResetMatricola.Text + "old", 13);
        //    Manager mn = new Manager();
        //    Boolean upd = mn.ResetPassw(passwordHash, txtResetMatricola.Text);
        //    if (upd)
        //    {

        //        //I 26/03/2026 - aggiunta variabile per abilitazione operatore
        //        if (myMaster != null)
        //        {
        //            // 2. Chiamo il metodo pubblico
        //            myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");


        //        }

        //    }
        //    else
        //    {

        //        if (myMaster != null)
        //        {
        //            // 2. Chiamo il metodo pubblico
        //            myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.ModificaCorretta.GetDescription(), "success");

        //        }
        //        //F 26/03/2026 - aggiunta variabile per abilitazione operatore
        //    }
        //}
        protected void InsOpetratore_Click(object sender, EventArgs e)
        {
            Operatore op = new Operatore();
            op.matricola = TxtMatricola.Text.ToUpper();
            //cripto la passowrd
            op.passw = BCrypt.Net.BCrypt.HashPassword(TxtMatricola.Text.ToUpper(), 13);
            op.pwstandard = TxtMatricola.Text;
            op.grado = ddlGrado.SelectedItem.Text;
            op.nota = TxtNota.Text;
            op.ruolo = DdlRuolo.SelectedItem.Text;
            op.reset = System.Convert.ToBoolean("false");
            //op.area = txtArea.Text.ToLower();
            // op.macroarea = txtMacroArea.Text;
            op.nominativo = txtNominativo.Text;
            op.abilitato = true;

            Manager mn = new Manager();
            Boolean ins = mn.InsOperatore(op);
            SiteMaster myMaster = this.Master as SiteMaster;
            if (ins)
            {
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("✅ ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
                    CaricaListOperatori();
                }

            }
            else
            {
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("✅ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "success");

                }
            }
        }
        public void pulisci()
        {
            txtAnno.Text = string.Empty;
            txtPratica.Text = string.Empty;
            ddlGrado.ClearSelection();
            GVcheck.DataSource = null;
            GVcheck.DataBind();
            //  GVcheck.DataBind();
        }
        protected void NuovoUt_Click(object sender, EventArgs e)
        {
            divNewUtente.Visible = true;
            divDestra.Visible = true;
            divReset.Visible = false;
            divCheck.Visible = false;
            GVcheck.Visible = false;
            divRepeater.Visible = false;
            pulisci();

        }
        protected void Lista_Click(object sender, EventArgs e)
        {
            divNewUtente.Visible = false;
            divDestra.Visible = false;
            divReset.Visible = false;
            divCheck.Visible = false;
            GVcheck.Visible = false;
            divRepeater.Visible = true;
            pulisci();
            CaricaListOperatori();

        }
       
        protected void Elimina_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean del = mn.DeleteMatricola(txtResetMatricola.Text);
            SiteMaster myMaster = this.Master as SiteMaster;
            if (del)
            {
                // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Matricola cancellata." + "'); $('#errorModal').modal('show');", true);

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.Delok.GetDescription(), "success");
                }

            }
            else
            // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Matricola non trovata." + "'); $('#errorModal').modal('show');", true);
            {

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️  ATTENZIONE", Enumerate.MsgOutput.Notfound.GetDescription(), "warning");
                }
            }
        }

        protected void Check_Click(object sender, EventArgs e)
        {
            divCheck.Visible = true;
            divNewUtente.Visible = false;
            divDestra.Visible = false;
            divReset.Visible = false;
            divRepeater.Visible = false;
            GVcheck.Visible = true;
        }

        protected void Cerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getCheck(txtPratica.Text.Trim(), txtAnno.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                GVcheck.DataSource = dt;
                GVcheck.DataBind();
            }
        }

        protected void GVcheck_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Colonna 0: Data Accesso (es: 10/03/2026 14:30)
            //    e.Row.Cells[0].Width = Unit.Pixel(100);
            //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            //    // Colonna 1: Operatore (Nominativo lungo)
            //    e.Row.Cells[1].Width = Unit.Pixel(100);

            //    // Colonna 2: Account SQL
            //    e.Row.Cells[2].Width = Unit.Pixel(120);

            //    // Colonna 3: Indirizzo IP
            //    e.Row.Cells[3].Width = Unit.Pixel(110);
            //    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //}
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Forza la tabella intera a essere fissa (non autofit)
                e.Row.TableSection = TableRowSection.TableBody;

                // 2. Colonna Data Accesso
                e.Row.Cells[0].Width = Unit.Pixel(100);
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[0].Attributes.Add("style", "border-left: 1px solid #dee2e6; white-space:nowrap;");

                // 3. Colonna Operatore
                e.Row.Cells[1].Width = Unit.Pixel(120); // Più spazio qui

                // 4. Colonna Account SQL
                e.Row.Cells[2].Width = Unit.Pixel(120);

                // 5. Colonna Indirizzo IP
                e.Row.Cells[3].Width = Unit.Pixel(120);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].Attributes.Add("style", "border-right: 1px solid #dee2e6; white-space:nowrap;");
            }
        }

        protected void GVcheck_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GVcheck_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        //I 26/03/2026 - aggiunta variabile per abilitazione operatore
        protected void btnDisabilita_Click(object sender, EventArgs e)
        {
            // Trasformo il "sender" (chi ha scatenato l'evento) nel controllo LinkButton
            LinkButton btn = (LinkButton)sender;

            // Recupero il valore del CommandArgument
            string argument = btn.CommandArgument;
            if (!string.IsNullOrEmpty(argument))
            {
                // Divido la stringa usando il separatore scelto
                string[] ar = argument.Split('|');

                if (ar.Length == 2)
                {
                    string matricola = ar[0]; // "M123"
                    string abil = ar[1]; // "Mario Rossi"
                    Boolean abilitato = System.Convert.ToBoolean(abil);
                    SiteMaster myMaster = this.Master as SiteMaster;
                    Manager mn = new Manager();
                    Boolean resp = mn.UpdAbilitaOperatore(matricola, abilitato);
                    if (resp)
                    {
                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.ModificaCorretta.GetDescription(), "success");
                            CaricaListOperatori();
                        }

                    }
                }
            }
        }
        protected void rptOperatori_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            PlaceHolder phView = (PlaceHolder)e.Item.FindControl("phView");
            PlaceHolder phEdit = (PlaceHolder)e.Item.FindControl("phEdit");

            switch (e.CommandName)
            {
                case "Edit":
                    phView.Visible = false;
                    phEdit.Visible = true;
                    break;

                case "Cancel":
                    phView.Visible = true;
                    phEdit.Visible = false;
                    break;

                case "Update":
                    string matricola = e.CommandArgument.ToString();
                    string ruolo = ((TextBox)e.Item.FindControl("txtRuolo")).Text;
                    string grado = ((TextBox)e.Item.FindControl("txtGrado")).Text;
                    //string macroArea = ((TextBox)e.Item.FindControl("txtMacroArea")).Text;

                    // Esegui il salvataggio sul tuo DB
                    Manager mn = new Manager();
                    SiteMaster myMaster = this.Master as SiteMaster;
                    Boolean resp = mn.UpdOperatore(matricola, ruolo, grado);
                    if (resp)
                    {
                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("✅ ATTENZIONE", Enumerate.MsgOutput.ModificaCorretta.GetDescription(), "success");
                        }

                    }
                    else
                    {
                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");
                        }
                    }
                    // Torna in visualizzazione e ricarica i dati
                    phView.Visible = true;
                    phEdit.Visible = false;
                    CaricaListOperatori();
                    //CaricaDati(); // Funzione che rifà il DataBind
                    break;
            }
        }


        protected void ModificaPass_Click(object sender, EventArgs e)
        {
            SiteMaster myMaster = this.Master as SiteMaster;


            Manager mn = new Manager();

            // Trasformo il "sender" (chi ha scatenato l'evento) nel controllo LinkButton
            LinkButton btn = (LinkButton)sender;

            // Recupero il valore del CommandArgument
            string matricola = btn.CommandArgument;
            if (!string.IsNullOrEmpty(matricola))
            {

                Boolean resp = mn.ResetPassw(BCrypt.Net.BCrypt.HashPassword(matricola + "old", 13), matricola);
                if (resp)
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("✅ ATTENZIONE", Enumerate.MsgOutput.PwResetOk.GetDescription(), "success");
                        //CaricaListOperatori();
                    }

                }
            }
        }
        private void CaricaListOperatori()
        {
            //

            Manager mn = new Manager();
            DataTable CaricaListOperatori = mn.getListOperatoreCompleta(uoSelezionata, out msg);
            // 2. Applica i filtri se inseriti
            DataView dv = CaricaListOperatori.DefaultView;

            string filtro = "1=1";

            if (!string.IsNullOrEmpty(txtFiltroNominativo.Text.Trim()))
            {
                filtro += string.Format(" AND Nominativo LIKE '%{0}%'", txtFiltroNominativo.Text.Trim().Replace("'", "''"));
            }

            if (!string.IsNullOrEmpty(txtFiltroMatricola.Text.Trim()))
            {
                filtro += string.Format(" AND Matricola LIKE '%{0}%'", txtFiltroMatricola.Text.Trim().Replace("'", "''"));
            }

            dv.RowFilter = filtro;
            // 3. Gestione Paginazione con PagedDataSource
            PagedDataSource pds = new PagedDataSource
            {
                DataSource = CaricaListOperatori.DefaultView,
                AllowPaging = true,
                PageSize = 15 // <--- IMPOSTATO A 15 RECORD
            }
            ;
            if (CurrentPage >= pds.PageCount)
            {
                CurrentPage = Math.Max(0, pds.PageCount - 1);
            }
            pds.CurrentPageIndex = CurrentPage;

            // Gestione abilitazione pulsanti paginazione
            btnPrev.Enabled = !pds.IsFirstPage;
            btnNext.Enabled = !pds.IsLastPage;
            // Info Pagina
            lblPageInfo.Text = string.Format("Pagina <b>{0}</b> di <b>{1}</b> (Totale record: {2})",
                pds.PageCount == 0 ? 0 : CurrentPage + 1,
                pds.PageCount,
                dv.Count);

            //if (pds.PageCount > 0)
            //{
            //    lblPageInfo.Text = string.Format("Pagina <b>{0}</b> di <b>{1}</b> (Totale record: {2})", CurrentPage + 1, pds.PageCount, dv.Count);
            //}
            //else
            //{
            //    lblPageInfo.Text = "Nessun risultato trovato.";
            //}
            rptOperatori.DataSource = pds;
            rptOperatori.DataBind();
        }
        private void CaricaPersonale()
        {
            Manager mn = new Manager();
            DataTable CaricaOperatori = mn.getListOperatore(out msg);
            DdlPersonale.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
            DdlPersonale.DataTextField = "Nominativo"; // Il campo visibile
            DdlPersonale.Items.Insert(0, new ListItem("", "0"));
            DdlPersonale.DataBind();
        }
        protected void EliminaMatricola_Click(object sender, EventArgs e)
        {

            SiteMaster myMaster = this.Master as SiteMaster;
            Manager mn = new Manager();

            // Trasformo il "sender" (chi ha scatenato l'evento) nel controllo LinkButton
            LinkButton btn = (LinkButton)sender;

            // Recupero il valore del CommandArgument
            string matricola = btn.CommandArgument;
            if (!string.IsNullOrEmpty(matricola))
            {

                Boolean del = mn.DeleteMatricola(matricola);
                if (del)
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("✅ ATTENZIONE", Enumerate.MsgOutput.Delok.GetDescription(), "success");
                        //CaricaListOperatori();
                    }

                }
            }






            //if (del)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Matricola cancellata." + "'); $('#errorModal').modal('show');", true);

            //}
            //else
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Matricola non trovata." + "'); $('#errorModal').modal('show');", true);
        }

        protected void btnCercaFiltri_Click(object sender, EventArgs e)
        {
            CurrentPage = 0; // Torna alla prima pagina quando si applica un filtro
            CaricaListOperatori();
        }
        protected void btnResetFiltri_Click(object sender, EventArgs e)
        {
            txtFiltroNominativo.Text = string.Empty;
            txtFiltroMatricola.Text = string.Empty;
            CurrentPage = 0;
            CaricaListOperatori();
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            CaricaListOperatori();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            CaricaListOperatori();
        }
    }
}