using AjaxControlToolkit.HtmlEditor.Popups;
using DocumentFormat.OpenXml;
using System;
using System.Data;
using System.Runtime.Caching;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestUO.Classi;
using static GestUO.Classi.Enumerate;

namespace GestUO
{
    public partial class _Attivita : Page
    {
        String Vuser = String.Empty;
        String area = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
      //          area = Session["MacroArea"].ToString();

            }
            //if (!String.IsNullOrEmpty(ruolo))
            //{
            //    if (ruolo != "admin")
            //    {
            //        string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
            //        Response.Redirect(url, false);
            //       // Response.Redirect("~/View/default.aspx");
            //    }
            //}
            //if (!IsPostBack)
            //{
            //    Manager mn = new Manager();
            //    DataTable CaricaOperatori = mn.getListOperatore();
            //    DdlPersonale.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
            //    DdlPersonale.DataTextField = "Nominativo"; // Il campo visibile
            //    DdlPersonale.Items.Insert(0, new ListItem("", "0"));
            //    DdlPersonale.DataBind();
            //}
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

      
        protected void GVAttivita_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVAttivita.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroMA1.Value) && String.IsNullOrEmpty(HfFiltroMA2.Value) && String.IsNullOrEmpty(HfFiltroMA3.Value))
            {
                switch (HfProvenienzaBt.Value)
                {
                    case "incarico":
                        btAttivitaInCarico_Click(sender, e);
                        break;
                    case "chiuso":
                        btAttivitaConcluse_Click(sender, e);
                        break;
                }

            }
           

        }
        

        protected void btAttivitaInCarico_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            if (area == "A") //admin visualizza tutte le attività, altrimenti visualizza solo quelle dell'area di competenza
            {
                PopulateGridView(mn, false);

            }
            else
            {
                DataTable dt = mn.getAttivita(area, false);
                GVAttivita.DataSource = dt;
                GVAttivita.DataBind();
            }
            HfProvenienzaBt.Value = "incarico";
        }


        protected void btAttivitaConcluse_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            if (area == "A") //admin visualizza tutte le attività, altrimenti visualizza solo quelle dell'area di competenza
            {
                PopulateGridView(mn, true);

            }
            else
            {
                DataTable dt = mn.getAttivita(area, true);
                GVAttivita.DataSource = dt;
                GVAttivita.DataBind();
            }
            HfProvenienzaBt.Value = "chiuso";
        }
        protected void GVMC1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVMC1.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroMA1.Value) && String.IsNullOrEmpty(HfFiltroMA2.Value) && String.IsNullOrEmpty(HfFiltroMA3.Value))
            {
                switch (HfProvenienzaBt.Value)
                {
                    case "incarico":
                        btAttivitaInCarico_Click(sender, e);
                        break;
                    case "chiuso":
                        btAttivitaConcluse_Click(sender, e);
                        break;
                }

            }
            //else
            //  {
            //if (!String.IsNullOrEmpty(HfFiltroMA1.Value))
            //{
            //    PopulateGridView("arch_note", HfFiltroMA1.Value);
            //    apripopupPratica_Click(sender, e);
            //}
            //else
            //{
            //    if (!String.IsNullOrEmpty(HfFiltroIndirizzo.Value))
            //    {
            //        PopulateGridView("arch_indirizzo", HfFiltroIndirizzo.Value);
            //        apripopupPratica_Click(sender, e);
            //    }
            //    else
            //    {
            //        if (!String.IsNullOrEmpty(HfFiltroResponsabile.Value))
            //        {
            //            PopulateGridView("arch_responsabile", HfFiltroResponsabile.Value);
            //            apripopupPratica_Click(sender, e);
            //        }
            //    }
            //}
            //   }


        }
        protected void GVMC2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVMC2.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroMA1.Value) && String.IsNullOrEmpty(HfFiltroMA2.Value) && String.IsNullOrEmpty(HfFiltroMA3.Value))
            {
                switch (HfProvenienzaBt.Value)
                {
                    case "incarico":
                        btAttivitaInCarico_Click(sender, e);
                        break;
                    case "chiuso":
                        btAttivitaConcluse_Click(sender, e);
                        break;
                }

            }
            //else
            //  {
            //if (!String.IsNullOrEmpty(HfFiltroMA1.Value))
            //{
            //    PopulateGridView("arch_note", HfFiltroMA1.Value);
            //    apripopupPratica_Click(sender, e);
            //}
            //else
            //{
            //    if (!String.IsNullOrEmpty(HfFiltroIndirizzo.Value))
            //    {
            //        PopulateGridView("arch_indirizzo", HfFiltroIndirizzo.Value);
            //        apripopupPratica_Click(sender, e);
            //    }
            //    else
            //    {
            //        if (!String.IsNullOrEmpty(HfFiltroResponsabile.Value))
            //        {
            //            PopulateGridView("arch_responsabile", HfFiltroResponsabile.Value);
            //            apripopupPratica_Click(sender, e);
            //        }
            //    }
            //}
            //   }


        }
        protected void GVMC3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVMC3.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroMA1.Value) && String.IsNullOrEmpty(HfFiltroMA2.Value) && String.IsNullOrEmpty(HfFiltroMA3.Value))
            {
                switch (HfProvenienzaBt.Value)
                {
                    case "incarico":
                        btAttivitaInCarico_Click(sender, e);
                        break;
                    case "chiuso":
                        btAttivitaConcluse_Click(sender, e);
                        break;
                }

            }
            //else
            //  {
            //if (!String.IsNullOrEmpty(HfFiltroMA1.Value))
            //{
            //    PopulateGridView("arch_note", HfFiltroMA1.Value);
            //    apripopupPratica_Click(sender, e);
            //}
            //else
            //{
            //    if (!String.IsNullOrEmpty(HfFiltroIndirizzo.Value))
            //    {
            //        PopulateGridView("arch_indirizzo", HfFiltroIndirizzo.Value);
            //        apripopupPratica_Click(sender, e);
            //    }
            //    else
            //    {
            //        if (!String.IsNullOrEmpty(HfFiltroResponsabile.Value))
            //        {
            //            PopulateGridView("arch_responsabile", HfFiltroResponsabile.Value);
            //            apripopupPratica_Click(sender, e);
            //        }
            //    }
            //}
            //   }


        }
        private void PopulateGridView(Manager mn, Boolean val)
        {
            GVMC1.Visible = true;
            GVMC2.Visible = true;
            GVMC3.Visible = true;
            DataTable dt1 = mn.getAttivitaAdmin("MA1", val);
            DataTable dt2 = mn.getAttivitaAdmin("MA2", val);
            DataTable dt3 = mn.getAttivitaAdmin("MA3", val);
            GVMC1.DataSource = dt1;
            GVMC1.DataBind();
            GVMC2.DataSource = dt2;
            GVMC2.DataBind();
            GVMC3.DataSource = dt3;
            GVMC3.DataBind();
            if (dt1.Rows.Count == 0)
            {
                lbl1.Visible = false;
            }
            else
            {
                lbl1.Visible = true;
            }
            if (dt2.Rows.Count == 0)
            {
                lbl2.Visible = false;
            }
            else
            {
                lbl2.Visible = true;
            }
            if (dt3.Rows.Count == 0)
            {
                lbl3.Visible = false;
            }
            else
            {
                lbl3.Visible = true;
            }
        }
        //gestione gridview attivita
        protected void GVAttivita_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";



            }
            if (GVAttivita.TopPagerRow != null && GVAttivita.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVAttivita.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVAttivita.PageIndex + 1;
                    int totalPages = GVAttivita.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        protected void GVAttivita_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    //string selectedValue = e.CommandArgument.ToString();


            //    string[] args = e.CommandArgument.ToString().Split(';');
            //    int idP = System.Convert.ToInt32(args[0]);
            //    string Npratica = args[1];


            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    // txtPratica.Text = Npratica;

            //    Manager mn = new Manager();
            //    //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

            //    DataTable pratica = mn.getPraticaArchivioUoteById(idP);
            //    //if (pratica.Rows.Count > 0)
            //    //{
            //    //    FillScheda(pratica);

            //    //}
            //    //Session.Remove("ListRicerca");
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }
        //gestione gridview attività mc1
        protected void GVMC1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";



            }
            if (GVMC1.TopPagerRow != null && GVMC1.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVMC1.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVMC1.PageIndex + 1;
                    int totalPages = GVMC1.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        protected void GVMC1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    //string selectedValue = e.CommandArgument.ToString();


            //    string[] args = e.CommandArgument.ToString().Split(';');
            //    int idP = System.Convert.ToInt32(args[0]);
            //    string Npratica = args[1];


            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    // txtPratica.Text = Npratica;

            //    Manager mn = new Manager();
            //    //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

            //    DataTable pratica = mn.getPraticaArchivioUoteById(idP);
            //    //if (pratica.Rows.Count > 0)
            //    //{
            //    //    FillScheda(pratica);

            //    //}
            //    //Session.Remove("ListRicerca");
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }
        protected void GVMC2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    //string selectedValue = e.CommandArgument.ToString();


            //    string[] args = e.CommandArgument.ToString().Split(';');
            //    int idP = System.Convert.ToInt32(args[0]);
            //    string Npratica = args[1];


            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    // txtPratica.Text = Npratica;

            //    Manager mn = new Manager();
            //    //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

            //    DataTable pratica = mn.getPraticaArchivioUoteById(idP);
            //    //if (pratica.Rows.Count > 0)
            //    //{
            //    //    FillScheda(pratica);

            //    //}
            //    //Session.Remove("ListRicerca");
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }
        //gestione gridview attività mc2
        protected void GVMC2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";



            }
            if (GVMC2.TopPagerRow != null && GVMC2.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVMC2.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVMC2.PageIndex + 1;
                    int totalPages = GVMC2.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        //gestione gridview attività mc3
        protected void GVMC3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";



            }
            if (GVMC3.TopPagerRow != null && GVMC3.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVMC3.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVMC3.PageIndex + 1;
                    int totalPages = GVMC3.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }

        protected void GVMC3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    //string selectedValue = e.CommandArgument.ToString();


            //    string[] args = e.CommandArgument.ToString().Split(';');
            //    int idP = System.Convert.ToInt32(args[0]);
            //    string Npratica = args[1];


            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    // txtPratica.Text = Npratica;

            //    Manager mn = new Manager();
            //    //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

            //    DataTable pratica = mn.getPraticaArchivioUoteById(idP);
            //    //if (pratica.Rows.Count > 0)
            //    //{
            //    //    FillScheda(pratica);

            //    //}
            //    //Session.Remove("ListRicerca");
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }

        protected void BtScadenziario_Click(object sender, EventArgs e)
        {

            // Effettua il cast alla  classe MasterPage 
            var masterPage = (SiteMaster)this.Master;

            // chiamo il metodo pubblico
            masterPage.CaricaListDelegheScadenza();
        }
    }
}