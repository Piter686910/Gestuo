using AjaxControlToolkit.Bundling;
using GestUO.Classi;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Runtime.Caching;
using System.Security.Policy;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Windows.Interop;
using System.Configuration; // Per leggere il Web.config
using MySql.Data.MySqlClient; // Namespace specifico di MySQL
using static GestUO.Classi.Enumerate;


namespace GestUO
{
    public partial class _Default : Page
    {
        public string Vuser { get; set; }
        public string uoAppartenenza { get; set; }
        string nomeImmagine = string.Empty;
        string urlRelativo = string.Empty;
        SiteMaster myMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            myMaster = this.Master as SiteMaster;
            String categoria = Request.QueryString["user"];
            Session["PaginaChiamante"] = "~/View/Default.aspx";

            if (categoria == "true")
            {



                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    return;

                }
            }
            if (!IsPostBack)
                if (Session["user"] != null)
                {
                    if (!String.IsNullOrEmpty(Session["user"].ToString()))
                    {
                        Manager mn = new Manager();
                        Vuser = Session["user"].ToString();
                        DataTable ricerca = mn.GetRuolo(Vuser);
                        Session["ruolo"] = ricerca.Rows[0].ItemArray[0];
                      
                        string paginaCorrente = Request.Url.AbsolutePath;
                        // Imposta il logo della MasterPage in base al valore di Session["uo"]
                        if (!string.IsNullOrEmpty(Session["uo"]?.ToString()))

                            ImpostaLogo(Session["uo"].ToString());

                        bool haPermesso = mn.VerificaPermesso(Session["ruolo"].ToString(), paginaCorrente);
                        if (!haPermesso)
                        {
                            // Se non ha i permessi, lo reindirizzi a una pagina di errore o alla pagina iniziale
                            //Response.Redirect("~/Contact.aspx?errore=Accesso negato, utente non abilitato a consultare la pagina");
                            if (myMaster != null)
                            {
                                // 2. Chiamo il metodo pubblico
                                myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.NoAutorizzato.GetDescription(), "danger");
                                //Session.Abandon();
                                return;

                            }
                        }
                        pnlLogin.Visible = false;

                    }
                }
            //al page load carica il popup delle scadenze
            //if (!IsPostBack)
            //{
            //    if (Session["profilo"].ToString().Contains("R"))
            //    {
            //        // Effettua il cast alla  classe MasterPage 
            //        var masterPage = (SiteMaster)this.Master;

            //        // chiamo il metodo pubblico
            //        masterPage.CaricaListDelegheScadenza();

            //    }

            //}
        }
        private void ImpostaLogo(string url)
        {

            switch (url)
            {
                case "CHIAIA":
                    nomeImmagine = "Logo1.jpeg";
                    break;
                case "SCAMPIA":
                    nomeImmagine = "Logo2.jpeg";

                    break;
                case "TUTTE":
                    nomeImmagine = "ComuneNapoli.jpeg";

                    break;
                default:
                    nomeImmagine = "ComuneNapoli.jpeg";
                    break;
            }
            urlRelativo = ResolveUrl($"~/FileComuni/{nomeImmagine}");
            if (myMaster != null)
            {
                myMaster.ImpostaLogoSfondo(urlRelativo);
            }



        }

        




        //private void CaricaListDelegheScadenza()
        //{
        //    //
        //    string msg = string.Empty;
        //    Manager mn = new Manager();
        //    int giorni= string.IsNullOrWhiteSpace(txtGiorniScad.Text) ? 60 : Convert.ToInt32(txtGiorniScad.Text);
        //    DataTable CaricaListDelegheScadenza = mn.getListDelegheInScadenza(Session["MacroArea"].ToString(), Session["user"].ToString(), giorni, out msg);

        //    rptDelegheScadenza.DataSource = CaricaListDelegheScadenza;
        //    rptDelegheScadenza.DataBind();
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#DelegheScadenza').modal('show');", true);

        //}



        protected void SalvaPassw_Click(object sender, EventArgs e)
        {

            //cripto la passowrd
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(txtNewPassw.Text, 13);

            Manager mn = new Manager();
            Boolean ins = mn.SavePassword(passwordHash, txtMatricola.Text.ToUpper());

            if (ins)
            {
                DivNewPassw.Visible = false;
                btsave.Visible = false;
                btnLogin.Visible = true;
                txtPassword.Enabled = true;
            }
            else
            {
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PwNoSave.GetDescription(), "danger");
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                    return;

                }

            }

        }

        protected void lkreset_Click(object sender, EventArgs e)
        {
            //cripto la passowrd
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(txtMatricola.Text.ToUpper() + "old", 13);
            Manager mn = new Manager();
            Boolean upd = mn.ResetPassw(passwordHash, txtMatricola.Text.ToUpper());
            if (upd)
            {
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico

                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PwResetOk.GetDescription(), "success");
                }
            }
            else
            {
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PwNoSave.GetDescription(), "danger");
                }
            }
        }

        protected void txtGiorniScad_TextChanged(object sender, EventArgs e)
        {
            // Effettua il cast alla  classe MasterPage 
            var masterPage = (SiteMaster)this.Master;

            // chiamo il metodo pubblico
            //masterPage.CaricaListDelegheScadenza();
        }
       
        protected void btnAccedi_Click(object sender, EventArgs e)
        {
            String Vpassw = "";
            Vuser = txtMatricola.Text.ToLower();
            Session["user"] = Vuser;
            //Hmatricola.Value = txtMatricola.Text.ToUpper();
            Vpassw = txtPassword.Text;

            DataTable Ricerca = new DataTable();

            Manager mn = new Manager();

            //prendo la password registrata in db per la verifica
 
            DataTable RicercaP = mn.getPass(txtMatricola.Text.ToLower());

            if (RicercaP.Rows.Count > 0)
            {

                //esiste matricola e passw
                string pwDB = RicercaP.Rows[0].ItemArray[0].ToString();
                //verifico correttezza passw inserita
                Ricerca = mn.getUserByUserPassw(txtMatricola.Text.ToLower(), pwDB);
                
                if (Ricerca.Rows.Count > 0)
                {
                    //I 26/03/2026 - aggiunta variabile per abilitazione operatore
                    //verifico se abilitato a loggare
                    Boolean abilitato = System.Convert.ToBoolean(Ricerca.Rows[0]["abilitato"]);
                   // Session["dbtarget"] = Ricerca.Rows[0]["dbtarget"];
                    if (abilitato == false)
                    {

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.UserDisable.GetDescription(), "danger");

                            return;
                        }
                    }
                    Boolean modifico = System.Convert.ToBoolean(Ricerca.Rows[0]["reset"]);
                    if (modifico == false)
                    {
                        DivNewPassw.Visible = true;
                        btsave.Visible = true;
                        btnLogin.Visible = false;
                        txtPassword.Enabled = false;
                        return;
                    }


                    //F 26/03/2026 - aggiunta variabile per abilitazione operatore
                    //verifico la correttezza della password criptata
                    string hashedPasswordSalvataNelDatabase = Ricerca.Rows[0]["passw"].ToString();
                    bool isMatch = BCrypt.Net.BCrypt.Verify(Vpassw, hashedPasswordSalvataNelDatabase);
                    if (isMatch)
                    {
                        //Boolean modifico = System.Convert.ToBoolean(Ricerca.Rows[0].ItemArray[7]);
                        //if (modifico == false)
                        //{
                        //    DivNewPassw.Visible = true;
                        //    btsave.Visible = true;
                        //    btLogin.Visible = false;
                        //    txtPassword.Enabled = false;
                        //}
                        // else
                        //  {
                        //salvo la matricola
                        Session["user"] = Vuser;
                        Session["uo"] = Ricerca.Rows[0]["uo"].ToString();
                        string ruolo = Ricerca.Rows[0]["ruolo"].ToString();
                        string url = string.Empty;
                        //switch (ruolo)
                        //{
                        //    case "comandante":
                        //        url = VirtualPathUtility.ToAbsolute("~/View/Menu.aspx");
                        //        break;
                        //    case "cdr chiaia":
                        //    case "cdr scampia":
                        //        url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                        //        break;


                        //    default:
                        //        url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                        //        break;
                        //}
                        url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                        Response.Redirect(url, false);
                    }
                    else
                    {

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.UserWrong.GetDescription(), "danger");
                            return;
                        }



                    }
                }
                else
                {

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.NoUser.GetDescription(), "danger");
                        return;
                    }
                }

            }
            else
            {
                if (myMaster != null)
                {
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.NoUser.GetDescription(), "danger");
                }
            }
        }


    }
}