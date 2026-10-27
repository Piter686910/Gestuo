using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Interop;
using GestUO.Classi;
using static GestUO.Classi.Enumerate;

namespace GestUO
{
    public partial class Modifica : Page
    {
        String profilo = string.Empty;
        string ruolo = string.Empty;
        String Vuser = String.Empty;
        Carico p = new Carico();
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string msg = string.Empty;
        string pagchiamante = "~/View/Modifica.aspx";
        Routine r = new Routine();
        string Isdecr = string.Empty;//variabile per identificare se la chiamata alla pagina è avvenuta da visualizza, se è true è stata chiamata da visualizza e deve aprire il popup di decretazione con i campi valorizzati,
                                     //se è false è stata chiamata da visualizza e deve aprire la scheda con i campi valorizzati
        protected void Page_Load(object sender, EventArgs e)
        {
            //verifica se ho ricevuto evento dal popup presente in sitemaster
            HiddenField hf = (HiddenField)Master.FindControl("hfMasterParam");
            if (hf != null && !string.IsNullOrEmpty(hf.Value))
            {
                string valoreRicevuto = hf.Value;




                Pulisci();
                CaricaDLL();
                Session.Remove("ListRicerca");
                Session.Remove("ListPratiche");
                Session.Remove("decr");
                Isdecr = String.Empty;
                string url = VirtualPathUtility.ToAbsolute("~/View/Modifica.aspx");
                Response.Redirect(url, false);
            }
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
        //        profilo = Session["profilo"].ToString();
                ruolo = Session["ruolo"].ToString();
                if (Session["decr"] != null)
                    Isdecr = Session["decr"].ToString();
                else Isdecr = "false";
            }

            else
            {
                // Se l'utente non è autenticato, reindirizza alla pagina di login
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true"); //segnalo alla pagina di default che la user è vuota
                    Response.Redirect(url, false);
                    return;
                }

            }
            Session["PaginaChiamante"] = pagchiamante;

            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
               // ProtocolloLiteral.Text = decodedText;
                DivRicerca.Visible = false;
                NascondiDiv();
                CaricaDLL();
                if (ruolo.ToUpper() == Enumerate.Ruolo.operatori.ToString().ToUpper())
                {
                    if (Session["profilo"].ToString().Contains(Enumerate.Profilo.tre.GetHashCode().ToStringInvariant()))
                        btChiudiDecretazione.Visible = true;
                    else
                    { 
                        //btChiudiDecretazione.Visible = false;
                    btSalva.Visible = false;
                    btCercaQuartiere.Visible = false;
                    btChiudiDecretazione.Visible = false;
                    }
                }
                if (Session["ListRicerca"] != null)
                {
                    DataTable pratica = (DataTable)Session["ListRicerca"];
                    //I- mod 31/01/2026 decretazione da pag visualizza
                    if (Isdecr == "true")
                    {
                        txtPraticaDecr.Text = pratica.Rows[0]["Nr_Carico"].ToString();
                        Hid.Value = pratica.Rows[0]["id"].ToString();
                        txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        Manager mn = new Manager();
                        DataTable decretazione = new DataTable();
                        string dborigine = string.Empty;
                        if (!string.IsNullOrEmpty(txtPraticaDecr.Text))
                        {
                            decretazione = mn.getListDecretazione(txtPraticaDecr.Text, Hid.Value,dborigine);
                            if (decretazione.Rows.Count > 0)
                            {
                                GVDecretazione.DataSource = decretazione;
                                GVDecretazione.DataBind();
                                Hfdecretazione.Value = decretazione.Rows[0]["decr_chiuso"].ToString();
                                ckUnire.Checked = Convert.ToBoolean(decretazione.Rows[0]["decr_unire"]);
                                if (Hfdecretazione.Value == "True")
                                {
                                    //btAggiungiDecretazione.Enabled = false;
                                    //btChiudiDecretazione.Enabled = false;
                                    SiteMaster myMaster = this.Master as SiteMaster;
                                    if (myMaster != null)
                                    {
                                        // 2. Chiamo il metodo pubblico
                                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaChiusa.GetDescription(), "danger");
                                        //string url = VirtualPathUtility.ToAbsolute("~/View/Visualizza.aspx");
                                        //Response.Redirect(url, false);
                                        return;
                                    }
                                }

                            }
                        }
                        DataTable operatore = mn.getNominativoOperatore(Vuser);
                        if (operatore.Rows.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(operatore.Rows[0]["nominativo"].ToString()))
                                txtDecretante.Text = operatore.Rows[0]["nominativo"].ToString().ToUpper();
                            
                        }

                        apripopupDecretazione_Click(sender, e);
                        //F- mod 31/01/2026 decretazione da pag visualizza
                    }
                    else
                    {
                        if (pratica.Rows.Count > 0)
                        {
                            Hid.Value = pratica.Rows[0].ItemArray[0].ToString();
                            DivDettagli.Visible = true;
                            CaricaDLL();
                            FillScheda(pratica);
                            Session.Remove("ListRicerca");
                        }
                    }
                }



            }
            

        }
        protected void FillScheda(DataTable pratica)
        {
            txtProt.Text = pratica.Rows[0].ItemArray[1].ToString();
            DdlSigla.SelectedValue = pratica.Rows[0].ItemArray[2].ToString();
            //if (pratica.Rows[0].ItemArray[2].ToString() == Enumerate.Sigla.AG.ToString().ToUpper())
            //{
            //    divAg.Visible = true;
            //}
            //else
            //{
            //    divAg.Visible = false;

            //}
            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[3].ToString()))

                txtDataInsCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()).ToShortDateString();
            txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
            txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
            txtTipoAtto.Text = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
            txtTipoAtto.ToolTip = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
            // DdlTipoAtto.SelectedItem.Text = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
            txtSearchAtto.Value = pratica.Rows[0]["Tipologia_atto"].ToString().ToUpper();
            DdlTipoAtto.ToolTip = pratica.Rows[0]["Tipologia_atto"].ToString().ToUpper();
            txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString().ToUpper();
            TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[7].ToString();
            TxtTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
            DdlTipoProvvAg.SelectedItem.Text = pratica.Rows[0].ItemArray[7].ToString();
            DdlTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString();

            txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
            txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
            txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
            txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper() + " " + pratica.Rows[0].ItemArray[11].ToString().ToUpper();
            txtIndirizzo.ToolTip = pratica.Rows[0].ItemArray[10].ToString().ToUpper();
            //I- mod 02/06/2026 numero esposti
            if (!String.IsNullOrEmpty(pratica.Rows[0]["NumProtRicStessoCarico"].ToString()))
            {
                txtNumProtRicStessoCarico.Text = pratica.Rows[0]["NumProtRicStessoCarico"].ToString().ToUpper();

            }

            //F- mod 02/06/2026 numero esposti
            //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
            //{
            //    //converte la data 01-01-1900 in SPACE
            //    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
            //    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
            //    {
            //        txtd.Text = ""; // Metti una stringa vuota
            //    }
            //    else
            //    {
            //        txtDataDataEvasa.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
            //    }
            //}
            txtEsito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
            txtEsito.ToolTip = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
            //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
            if (!String.IsNullOrEmpty(pratica.Rows[0]["EvasaData"].ToString()))

            {
                //converte la data 01-01-1900 in SPACE
                DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0]["EvasaData"].ToString()); // Recupera la data dal DataTable
                if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                {
                    TxtDataEsito.Text = string.Empty; // Metti una stringa vuota
                }
                else
                {
                    TxtDataEsito.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                }
            }

            //if (pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("-") || pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("/"))
            //{
            //    //txtOperatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
            //    //txtOperatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
            //}
            //else
            //{
            //    //txtOperatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
            //    //txtOperatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
            //}

            if (!string.IsNullOrWhiteSpace(pratica.Rows[0]["operatori"].ToString()))
            {
                ListOperatori.Items.Add(pratica.Rows[0]["operatori"].ToString());
            }
            if (!string.IsNullOrWhiteSpace(pratica.Rows[0]["operatori2"].ToString()))
            {
                ListOperatori.Items.Add(pratica.Rows[0]["operatori2"].ToString());
            }
            if (!string.IsNullOrWhiteSpace(pratica.Rows[0]["operatori3"].ToString()))
            {
                ListOperatori.Items.Add(pratica.Rows[0]["operatori3"].ToString());
            }


            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
            {
                //converte la data 01-01-1900 in SPACE
                DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                {
                    txtDataCarico.Text = ""; // Metti una stringa vuota
                }
                else
                {
                    txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                }
            }

            txPratica.Text = pratica.Rows[0].ItemArray[19].ToString();
            txtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
            //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
            //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
            txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
            //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
            txtRifProtGen.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Gen"].ToString(), @"[^0-9/]", ";");
            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[27].ToString()))
            {
                //DdlMacroArea.SelectedItem.Text = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                DdlMacroArea.SelectedValue = pratica.Rows[0]["Macro_area"].ToString();//  ItemArray[27].ToString().ToUpper();
                //txtAreaCompetenza.ToolTip = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
            }

            txtBU.Text = pratica.Rows[0].ItemArray[29].ToString().ToUpper();
            txtCodEdificio.Text = pratica.Rows[0].ItemArray[30].ToString().ToUpper();
            //I 23/04/2026 controllo deleghe
            if (!String.IsNullOrEmpty(pratica.Rows[0]["DataDelega"].ToString()))
                txtDataDelega.Text = System.Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()).ToShortDateString();

            txtGgDelega.Text = pratica.Rows[0]["GgDelega"].ToString();
            //F 23/04/2026 controllo deleghe
            //I 22/05/2026 protocollo uscita
            txtProtUscita.Text = pratica.Rows[0]["Rif_Prot_Uscita"].ToString();
            //F 22/05/2026 protocollo uscita

            //I mod 26/08/2026 cartellina patrimonio
            txtCartellina.Text = pratica.Rows[0]["cartellina"].ToString();
            //F mod 26/08/2026 cartellina patrimonio

        }
        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable RicercaQuartiere = mn.getListQuartiere(out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                //DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlQuartiere.DataBind();
                //DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                
                System.Data.DataTable CaricaOperatoriDecretazione = mn.getListOperatore(out msg);
                ddlOperatore.DataSource = CaricaOperatoriDecretazione; // Imposta il DataSource della DropDownList
                ddlOperatore.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                ddlOperatore.Items.Insert(0, new ListItem("", "0"));
                ddlOperatore.DataBind();
                ddlOperatore.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo(out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "SpecieToponimo"; // Il campo visibile
                DdlIndirizzo.DataBind();
                //DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                DataTable RicercaProvvAg = mn.getListProvvAg(DdlSigla.SelectedValue.ToString(), out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                DdlTipoProvvAg.DataValueField = "id_tipo_nota_ag"; // Il valore associato a ogni opzione

                DdlTipoProvvAg.DataBind();
                DdlTipoProvvAg.Items.Insert(0, new ListItem("", "0"));
                DataTable Esito = mn.getListScaturito(out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlEsito.DataSource = Esito; // Imposta il DataSource della DropDownList
                DdlEsito.DataTextField = "Scaturito"; // Il campo visibile
                DdlEsito.DataValueField = "Id_scaturito"; // Il valore associato a ogni opzione
                DdlEsito.DataBind();

                DataTable CaricaOperatori = mn.getListOperatori(out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlOperatori.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                DdlOperatori.DataTextField = "Nominativo"; // Il campo visibile                
                DdlOperatori.Items.Insert(0, new ListItem("", "0"));
                DdlOperatori.DataBind();
                DdlOperatori.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                DataTable RicercaTipoAtto = mn.getListTipologia(out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAtto.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione
                DdlTipoAtto.DataBind();
                DdlTipoAtto.Items.Insert(0, new ListItem("", "0"));

                DataTable RicercaProvenienza = mn.getListProvenienza(out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);

                DdlProvenienza.DataSource = RicercaProvenienza; // Imposta il DataSource della DropDownList
                DdlProvenienza.DataTextField = "Provenienza"; // Il campo visibile
                DdlProvenienza.DataValueField = "id_provenienza"; // Il valore associato a ogni opzione

                DdlProvenienza.DataBind();
                DdlSigla.Items.Clear();
                foreach (Sigla stato in Enum.GetValues(typeof(Sigla)))
                {
                    // Crea un nuovo ListItem
                    ListItem item = new ListItem();

                    // Il testo visibile all'utente viene preso dalla Description
                    item.Text = GetEnumDescription(stato);

                    // Il valore interno è il nome del membro dell'enum 
                    item.Value = stato.ToString();

                    DdlSigla.Items.Add(item);
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
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl modifica.cs ");
                    sw.Close();
                }
            }

        }
        /// <summary>
        /// Funzione di supporto per ottenere la stringa dall'attributo [Description] di un enum.
        /// </summary>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        protected Boolean Convalida()
        {
            Boolean resp = true;
            Tipologie espostoSegn = Tipologie.EspostoSegnalazione;
            string testoE = espostoSegn.GetDescription();

            if (txtTipoAtto.Text == testoE)
            {
                //if (divAg.Visible == true)
                //{
                //    resp = false;
                //}
            }
            if (String.IsNullOrEmpty(txtDataInsCarico.Text))
            {
                resp = false;
            }

            return resp;
        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(Vuser))
                {
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    }
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true"); //segnalo alla pagina di default che la user è vuota
                    Response.Redirect(url, false);
                    return;
                }
                Boolean resp = Convalida();
                Manager mn = new Manager();
                if (!resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorAvvertenze').text('" + "Per modificare devi prima cercare il numero carico." + "'); $('#ModalAvvertenze').modal('show');", true);

                }
                else
                {
                    //if (Session["user"] != null)
                    //{
                    //    if (String.IsNullOrEmpty(Session["user"].ToString()))
                    //    {
                    //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.SScaduta.GetDescription() + "'); $('#errorModal').modal('show');", true);

                    //        string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                    //        Response.Redirect(url, false);
                    //    }
                    //}
                    Carico p = new Carico();

                    //p.sigla = DdlSigla.SelectedItem.Text;
                    p.dataArrivo = System.Convert.ToDateTime(txtDataInsCarico.Text).ToShortDateString();
                    if (!string.IsNullOrEmpty(txtDataCarico.Text))
                    {
                        p.dataCarico = System.Convert.ToDateTime(txtDataCarico.Text).ToShortDateString();
                    }

                    p.nominativo = txtNominativo.Text;

                    if (!String.IsNullOrEmpty(txPratica.Text))
                    {
                        p.nr_Pratica = txPratica.Text.Trim();
                    }
                    if (String.IsNullOrEmpty(txtIndirizzo.Text))
                    {
                        p.indirizzo = String.Empty;
                    }
                    else
                    {
                        p.indirizzo = txtIndirizzo.Text;
                        p.via = string.Empty;

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
                    p.provenienza = txtProvenienza.Text;
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
                    //if (DdlTipoAtto.SelectedItem.Text == "")
                    //{

                    //    p.tipologia_atto = String.Empty;
                    //}
                    //else
                    //{
                    //    Boolean resp1 = mn.getTipoAtto(DdlTipoAtto.SelectedItem.Text);
                    //    if (!resp1)
                    //    {
                    //        HfTipoAtto.Value = DdlTipoAtto.SelectedItem.Text;
                    //    }
                    //    p.tipologia_atto = DdlTipoAtto.SelectedItem.Text;
                    // }
                    if (!String.IsNullOrEmpty(txtTipoAtto.Text))
                        p.ulterioreTipoAtto = txtTipoAtto.Text;
                    p.rif_Prot_Gen = txtRifProtGen.Text;
                    p.giudice = txtGiudice.Text;
                    if (!string.IsNullOrEmpty(TxtTipoProvvAg.Text))

                        p.tipoProvvedimentoAG = DdlTipoProvvAg.SelectedItem.Text;

                    //if (DdlQuartiere.SelectedValue == "0")
                    //{
                    //    p.quartiere = String.Empty;
                    //}
                    //else
                    //{
                    //    p.quartiere = DdlQuartiere.SelectedItem.Text;

                    //}

                    // p.note = txtNote.Text;
                    // p.evasa = ck.Checked;
                    //I- mod 02/06/2026 numero esposti
                    p.NumProtRicStessoCarico = string.IsNullOrWhiteSpace(txtNumProtRicStessoCarico.Text) ? 0 : Convert.ToInt32(txtNumProtRicStessoCarico.Text);

                    //F- mod 02/06/2026 numero esposti
                    if (!string.IsNullOrEmpty(TxtDataEsito.Text))
                    {
                        p.evasaData = System.Convert.ToDateTime(TxtDataEsito.Text).ToShortDateString();
                    }

                    if (!String.IsNullOrEmpty(DdlMacroArea.SelectedItem.Text))
                    {
                        p.areaCompetenza = DdlMacroArea.SelectedItem.Text.ToUpper();
                    }
                    else
                        p.areaCompetenza = string.Empty;

                    // p.accertatori = txtAccertatori.Text.ToUpper();
                    string[] accer = ListOperatori.Items.Cast<ListItem>()
                                 .Select(i => i.Text)
                                 .ToArray();

                    string contaA = Convert.ToString((accer.Length));
                    // Ora distribuiamo i valori agli oggetti disponibili
                    switch (contaA)
                    {
                        case "1":
                            p.operatori = accer[0];
                            break;
                        case "2":
                            p.operatori = accer[0];
                            p.operatori2 = accer[1];
                            break;
                        case "3":
                            p.operatori = accer[0];
                            p.operatori2 = accer[1];
                            p.operatori3 = accer[2];
                            break;
                        default:
                            break;
                    }




                    //if (accer.Length > 0)
                    //{
                    //    p.accertatori = accer[0];
                    //    p.accertatori2 = accer[1];
                    //    p.accertatori3 = accer[2];
                    //}

                    if (!string.IsNullOrEmpty(txtEsito.Text))
                        p.esito = txtEsito.Text;
                    //if (!string.IsNullOrEmpty(txtInviata.Text))
                    //    p.inviata = txtInviata.Text;
                    //if (!string.IsNullOrEmpty(txtDataInvio.Text))
                    //{
                    //    p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
                    //}

                    p.procedimentoPen = txtProdPenNr.Text;
                    //matricola del popup
                    p.matricola = Vuser;
                    //string newMat = ;

                    p.data_ins_pratica = DateTime.Now.ToLocalTime();
                    p.nrcarico = System.Convert.ToInt32(txtProt.Text.Trim());
                    DateTime dat = System.Convert.ToDateTime(txtDataInsCarico.Text);

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
                    switch (DdlSigla.SelectedValue)
                    {
                        case "TP":
                        case "ED":
                            p.tipoProvvedimentoAG = string.Empty;
                            p.giudice = string.Empty;
                            p.procedimentoPen = string.Empty;
                            txtProdPenNr.Text = string.Empty;
                            txtGiudice.Text = string.Empty;
                            DdlTipoProvvAg.ClearSelection();
                            TxtTipoProvvAg.Text = string.Empty;
                            break;

                    }
                    //I 23/04/2026 controllo deleghe
                    p.dataDelega = string.IsNullOrWhiteSpace(txtDataDelega.Text) ? DateTime.MinValue.ToShortDateString() : System.Convert.ToDateTime(txtDataDelega.Text).ToShortDateString();
                    p.ggDelega = string.IsNullOrWhiteSpace(txtGgDelega.Text) ? 0 : Convert.ToInt32(txtGgDelega.Text);

                    //F 23/04/2026 controllo deleghe
                    //I 22/05/2026 protocollo uscita
                    p.rif_Prot_Uscita = txtProtUscita.Text;
                    //F 22/05/2026 protocollo uscita

                   

                    // id proveniente dalla selezione della pratica
                    int ID = System.Convert.ToInt32(Hid.Value);
                    SiteMaster myMaster = this.Master as SiteMaster;
                    Boolean ins = mn.UpdPratica(p, Holdmat.Value, ID, dat, Vuser);
                    if (!ins)
                    {
                        // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.ErrorLog.GetDescription() + "'); $('#errorModal').modal('show');", true);
                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("INFORMAZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");
                        }
                    }
                    else
                    {
                        //se provengo dal button decretazione non deve apparire il popup di salvataggio
                        if (HfButtonProv.Value != "Decretazione")
                        {

                            HfButtonProv.Value = string.Empty;


                            if (myMaster != null)
                            {
                                // 2. Chiamo il metodo pubblico
                                myMaster.MostraMessaggio("✅  INFORMAZIONE", Enumerate.MsgOutput.ModificaCorretta.GetDescription(), "success");
                                //  ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.ModificaCorretta.GetDescription() + "'); $('#errorModal').modal('show');", true);
                            }
                        }
                        //DivDettagli.Visible = false;
                        //Pulisci();
                    }
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
                    sw.WriteLine(ex.Message + @" - Errore in modifica pratica");
                    sw.Close();
                }
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
            txtCartellina.Text = string.Empty;
            txtEsito.Text = string.Empty;
            HfEsito.Value = string.Empty;
            txtQuartiere.Text = string.Empty;
            HfQuartiere.Value = string.Empty;
            // txtInviata.Text = string.Empty;
            // HfInviata.Value = string.Empty;
            txtIndirizzo.Text = string.Empty;
            HfIndirizzo.Value = string.Empty;
            txtAnnoRicerca.Text = String.Empty;
            //txPratica.Text = String.Empty;
            txtNProtocollo.Text = String.Empty;
            txtProcPenale.Text = String.Empty;
            txtDataDa.Text = String.Empty;
            txtDataA.Text = String.Empty;
            txtProtGen.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicOperatori.Text = String.Empty;
            txtRicOperatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtNotaDecretazione.Text = String.Empty;
            txtDecretante.Text = String.Empty;
            //txtDecretato.Text = String.Empty;
            txtSearchOperatore.Value = String.Empty; 
            txtDataDecretazione.Text = String.Empty;
            TxtDataEsito.Text = String.Empty;
            txPratica.Text = String.Empty;
            txtTipoAtto.Text = String.Empty;
            DdlTipoAtto.Items.Clear();// ClearSelection();
            txtProvenienza.Text = String.Empty;
            txtRifProtGen.Text = String.Empty;
            txtNominativo.Text = String.Empty;
            DdlMacroArea.SelectedIndex = 0;
            // DdlMacroArea.ClearSelection();
            txtDataCarico.Text = String.Empty;
            txtDataInsCarico.Text = String.Empty;
            //txtAccertatori.Text = string.Empty;
            txtdataEvasaPopup.Text = string.Empty;
            txtProt.Text = string.Empty;
            txtGiudice.Text = string.Empty;
            txtProdPenNr.Text = string.Empty;
            txtBU.Text = string.Empty;
            txtCodEdificio.Text = string.Empty;
            List<string> operatoriList = new List<string>();
            DdlSigla.Items.Clear();
            ListOperatori.Items.Clear();
            //I 22/05/2026 protocollo uscita
             txtProtUscita.Text=string.Empty;
            //F 22/05/2026 protocollo uscita

        }

        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            DivDettagli.Visible = false;
            DivRicerca.Visible = true;
            DivGrid.Visible = false;
            txtAnnoRicerca.Text = String.Empty;
            txPratica.Text = String.Empty;
            Hfdecretazione.Value = string.Empty;

        }
        protected void RicercaPerModifica(object sender, EventArgs e)
        {

            string msg = string.Empty;
            Manager mn = new Manager();
            String dbOrigine = string.Empty;
            DataTable pratica = new DataTable();
            if (String.IsNullOrEmpty(txtAnnoRicerca.Text))
            {
                txtAnnoRicerca.Text = DateTime.Now.Year.ToString();
            }
            if (!string.IsNullOrEmpty(txtNProtocollo.Text))
            {
                pratica = mn.getListCaricoTabella(Vuser, txtNProtocollo.Text, txtAnnoRicerca.Text,"uo",false, out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtProcPenale.Text))
            {
                pratica = mn.getListProcedimento(txtProcPenale.Text, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }

            if (!string.IsNullOrEmpty(txtDataDa.Text))
            {
                pratica = mn.getListEvasaAg(txtDataDa.Text, txtDataA.Text, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtProtGen.Text))
            {
                //modificare false con ckgeneralizzato.Checked
                pratica = mn.getListProtGen(txtProtGen.Text, false,  "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtPratica.Text))
            {
                pratica = mn.getListPratica(txtPratica.Text, false,  "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicGiudice.Text))
            {
                pratica = mn.getListGiudice(txtRicGiudice.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicProvenienza.Text))
            {
                pratica = mn.getListProvenienza(txtRicProvenienza.Text,false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicNominativo.Text))
            {
                pratica = mn.getListNominativo(txtRicNominativo.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicOperatori.Text))
            {
                pratica = mn.getListOperatori(txtRicOperatori.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicIndirizzo.Text))
            {
                pratica = mn.getListIndirizzo(txtRicIndirizzo.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtDataDa.Text))
            {
                pratica = mn.getListDataArrivo(txtDatArrivoDa.Text, txtDatArrivoA.Text,"uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))

                    r.Reindirizzamento(msg, pagchiamante);
            }

            if (pratica.Rows.Count > 0)
            {
                // Salva datatable pratica  nella Sessione
                // Session["ListPratiche"] = pratica;

                GVDecretazione.DataSource = pratica;
                GVDecretazione.DataBind();
                //DivDettagli.Visible = true;
                //DivRicerca.Visible = false;
                // DivGrid.Visible = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);
            }
        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {
            String dbOrigine = string.Empty;
            string msg = string.Empty;
            Manager mn = new Manager();

            DataTable pratica = new DataTable();
            if (String.IsNullOrEmpty(txtAnnoRicerca.Text))
            {
                txtAnnoRicerca.Text = DateTime.Now.Year.ToString();
            }
            if (!string.IsNullOrEmpty(txtNProtocollo.Text))
            {
                pratica = mn.getListCaricoTabella(Vuser, txtNProtocollo.Text, txtAnnoRicerca.Text,"uo",false, out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtProcPenale.Text))
            {
                pratica = mn.getListProcedimento(txtProcPenale.Text, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }

            if (!string.IsNullOrEmpty(txtDataDa.Text))
            {
                pratica = mn.getListEvasaAg(txtDataDa.Text, txtDataA.Text, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtProtGen.Text))
            {
                //modificare false con ckgeneralizzato.Checked
                pratica = mn.getListProtGen(txtProtGen.Text,false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtPratica.Text))
            {
                pratica = mn.getListPratica(txtPratica.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicGiudice.Text))
            {
                pratica = mn.getListGiudice(txtRicGiudice.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicProvenienza.Text))
            {
                pratica = mn.getListProvenienza(txtRicProvenienza.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicNominativo.Text))
            {
                pratica = mn.getListNominativo(txtRicNominativo.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicOperatori.Text))
            {
                pratica = mn.getListOperatori(txtRicOperatori.Text, false, "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtRicIndirizzo.Text))
            {
                pratica = mn.getListIndirizzo(txtRicIndirizzo.Text, false,  "uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))
                    r.Reindirizzamento(msg, pagchiamante);
            }
            if (!string.IsNullOrEmpty(txtDataDa.Text))
            {
                pratica = mn.getListDataArrivo(txtDatArrivoDa.Text, txtDatArrivoA.Text,"uo", out msg);
                if (!String.IsNullOrWhiteSpace(msg))

                    r.Reindirizzamento(msg, pagchiamante);
            }

            if (pratica.Rows.Count > 0)
            {
                // Salva datatable pratica  nella Sessione
                Session["ListPratiche"] = pratica;

                gvPopupD.DataSource = pratica;
                gvPopupD.DataBind();
                //DivDettagli.Visible = true;
                //DivRicerca.Visible = false;
                DivGrid.Visible = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
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

                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);
            }
        }

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
            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);





        }

        protected void gvPopupD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 5)
                {
                    Int32 protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                    string matricola = values[1];     // Matricola
                    string dataInserimento = values[2]; // DataInserimento
                    //string sigla = values[3]; // sigla
                    Hid.Value = values[3]; // id


                    //// Ora puoi usare questi valori per aggiornare i tuoi controlli
                    //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                    //conservo la matricola precedente
                    Holdmat.Value = matricola;
                    HolDate.Value = dataInserimento;
                    //p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLongDateString();
                    Manager mn = new Manager();
                      
                    DataTable pratica = mn.getPraticaCaricoDataSiglaId(protocollo, System.Convert.ToDateTime(dataInserimento), System.Convert.ToInt32(Hid.Value));

                    if (pratica.Rows.Count > 0)
                    {
                        CaricaDLL();
                        DivDettagli.Visible = true;
                        txtProt.Text = pratica.Rows[0].ItemArray[1].ToString();
                        //txtSigla.Text = pratica.Rows[0].ItemArray[2].ToString();
                        DdlSigla.SelectedValue = pratica.Rows[0].ItemArray[2].ToString();
                        //switch (DdlSigla.SelectedValue)
                        //{
                        //    case "AG":
                        //        divAg.Visible = true;

                        //        break;
                        //    default:
                        //        divAg.Visible = false;
                        //        //CaricaDLL();
                        //        break;
                        //}
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[3].ToString()))
                        {
                            DateTime dataappo1 = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()); // Recupera la data dal DataTable
                            txtDataInsCarico.Text = dataappo1.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                        }
                        else
                            txtDataInsCarico.Text = string.Empty;

                        txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        txtSearchAtto.Value = pratica.Rows[0]["Tipologia_atto"].ToString().ToUpper();
                        DdlTipoAtto.SelectedItem.Text = pratica.Rows[0]["Tipologia_atto"].ToString().ToUpper();
                        DdlTipoAtto.ToolTip = pratica.Rows[0]["Tipologia_atto"].ToString().ToUpper();
                        txtTipoAtto.Text = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
                        txtTipoAtto.ToolTip = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
                        txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString();
                        DdlTipoProvvAg.Items.Insert(0, new ListItem(pratica.Rows[0].ItemArray[7].ToString().ToUpper()));
                        // DdlTipoProvvAg.SelectedValue = "1";
                        DdlTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        TxtTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
                        txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[10].ToString()))
                        {
                            txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper();
                            txtIndirizzo.ToolTip = pratica.Rows[0].ItemArray[10].ToString().ToUpper();
                        }
                        //txtVia.Text = pratica.Rows[0].ItemArray[10].ToString();
                        //CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[12]);

                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
                            //converte la data 01-01-1900 in SPACE

                            if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                            {
                                TxtDataEsito.Text = ""; // Metti una stringa vuota
                            }
                            else
                            {
                                TxtDataEsito.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                            }
                        }
                        else
                            TxtDataEsito.Text = string.Empty;


                        //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[14].ToString()))

                        //    txtInviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();

                        //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))
                        //{
                        //    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()); // Recupera la data dal DataTable
                        //    //converte la data 01-01-1900 in SPACE

                        //    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                        //    {
                        //        txtDataInvio.Text = ""; // Metti una stringa vuota
                        //    }
                        //    else
                        //    {
                        //        txtDataInvio.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                        //    }
                        //}
                        //else
                        //    txtDataInvio.Text = string.Empty;

                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[16].ToString()))

                            txtEsito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();

                        DdlMacroArea.SelectedItem.Text = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                            //if (dataappo == DateTime.MinValue)
                            //{
                            //    txtDataCarico.Text = string.Empty;
                            //}
                            //else
                            //    txtDataCarico.Text = dataappo.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox

                            //converte la data 01-01-1900 in SPACE

                            if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                            {
                                txtDataCarico.Text = ""; // Metti una stringa vuota
                            }
                            else
                            {
                                txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                            }
                        }
                        else
                            txtDataCarico.Text = string.Empty;
                        //I- mod 31/01/2026 scheda int
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
                        //I- mod 02/06/2026 numero esposti
                        if (!String.IsNullOrEmpty(pratica.Rows[0]["NumProtRicStessoCarico"].ToString()))
                        {
                            txtNumProtRicStessoCarico.Text = pratica.Rows[0]["NumProtRicStessoCarico"].ToString().ToUpper();

                        }
                        //F- mod 02/06/2026 numero esposti
                        //F- mod 31/01/2026 scheda int
                        //if (ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoAtti.ToString().ToUpper() || ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
                        //{
                        //    txtOperatori.Enabled = false;
                        //}
                        //I 22/05/2026 protocollo uscita
                        if (!String.IsNullOrEmpty(pratica.Rows[0]["Rif_Prot_Uscita"].ToString()))
                            txtProtUscita.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Uscita"].ToString(), @"[^0-9/]", ";"); ;
                        //F 22/05/2026 protocollo uscita
                        txPratica.Text = pratica.Rows[0].ItemArray[19].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[20].ToString()))
                            txtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
                        //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
                        //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                        //txtRifProtGen.Text = pratica.Rows[0].ItemArray[24].ToString();
                        txtRifProtGen.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Gen"].ToString(), @"[^0-9/]", ";");
                        txtBU.Text = pratica.Rows[0].ItemArray[29].ToString();
                        txtCodEdificio.Text = pratica.Rows[0].ItemArray[30].ToString();

                        // Puoi anche chiudere il popup se necessario
                        ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                        DivDettagli.Visible = true;
                        DivRicerca.Visible = false;
                        // Pulisci();
                    }
                    else
                    {
                        SiteMaster myMaster = this.Master as SiteMaster;

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaNotFound.GetDescription(), "warning");
                        }
                        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica: " + protocollo + " non trovata." + "'); $('#errorModal').modal('show');", true);

                    }
                }
            }
        }

        protected void gvPopupD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Nr_Protocollo").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
                if (gvPopupD.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvPopupD.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvPopupD.PageIndex + 1;
                        int totalPages = gvPopupD.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }

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
        //gridview per decretazione
        protected void GVDecretazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "decr_id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";

                if (GVDecretazione.TopPagerRow != null)
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


            //// Ottieni il valore della colonna "ID"
            //string id = DataBinder.Eval(e.Row.DataItem, "decr_id").ToString();

            //// Aggiungi l'attributo per il doppio clic
            //e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            //e.Row.Style["cursor"] = "pointer";
            ////if (e.Row.RowType == DataControlRowType.DataRow)
            ////{
            ////    // Ottieni il valore della colonna "ID"
            ////    string id = DataBinder.Eval(e.Row.DataItem, "decr_id").ToString();

            ////    // Aggiungi l'attributo per il doppio clic
            ////    e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            ////    e.Row.Style["cursor"] = "pointer";
            ////}
            //if (GVDecretazione.TopPagerRow != null && GVDecretazione.TopPagerRow.Visible)
            //{
            //    // Trova il controllo Label all'interno del PagerTemplate
            //    Label lblPageInfo = (Label)GVDecretazione.TopPagerRow.FindControl("lblPageInfo");
            //    if (lblPageInfo != null)
            //    {
            //        // Calcola e imposta il testo
            //        int currentPage = GVDecretazione.PageIndex + 1;
            //        int totalPages = GVDecretazione.PageCount;
            //        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
            //    }
            //}
        }
        protected void GVDecretazione_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                string selectedValue = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = selectedValue.Split('|');

                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 4)
                {
                    if (Hfdecretazione.Value == "True")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "la Modifica non può essere effettuata pratica chiusa." + "'); $('#errorModal').modal('show');", true);
                        SiteMaster myMaster = this.Master as SiteMaster;
                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaChiusa.GetDescription(), "warning");
                        }

                    }
                    else
                    {


                        Int32 idDecr = System.Convert.ToInt32(values[0]);    // Protocollo
                        txtSearchOperatore.Value = values[1];     // Matricola
                        txtDataDecretazione.Text = values[2]; // DataInserimento
                        txtNotaDecretazione.Text = values[3]; // sigla

                        // Imposta il valore nel TextBox
                        //txtSelectedValue.Text = selectedValue;
                        // txtDecretato.Text = decretato;

                        // Chiudi il popup
                        // ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);
                    }
                }
            }
            if (e.CommandName == "Save")
            {

                // Ottieni il valore dell'ID dalla CommandArgument
                string selectedValue = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = selectedValue.Split('|');

                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 4)
                {
                    Int32 idDecr = System.Convert.ToInt32(values[0]);    // 


                    Manager mn = new Manager();
                    Decretazione decr = new Decretazione();
                    decr.data = System.Convert.ToDateTime(txtDataDecretazione.Text);
                    decr.id = idDecr;
                    decr.decretante = txtDecretante.Text;
                    decr.decretato = txtSearchOperatore.Value;
                    decr.nota = txtNotaDecretazione.Text;
                    Boolean resp = mn.UpdDecretazione(decr);
                    Decretazione_Click(sender, e);

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

                }
            }
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
        protected void GVDecretazione_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Esce dalla modalità modifica
            GVDecretazione.EditIndex = -1;
            // Ricarica i dati per mostrare le TextBox
            Decretazione_Click(sender, e);
        }

        protected void GVDecretazione_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVDecretazione.EditIndex = e.NewEditIndex;
            Session.Remove("decr");
            Isdecr = "edit";
            // Ricarica i dati per mostrare le TextBox
            Decretazione_Click(sender, e);
            //Ricerca_Click(sender, e);
        }

        protected void GVDecretazione_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
             Decretazione decr = new Decretazione();
            try
            {
                Isdecr = "edit";
                // A. Recupera l'ID del record (da DataKeyNames)
                int idDecretazione = Convert.ToInt32(GVDecretazione.DataKeys[e.RowIndex].Value);

                // B. Recupera i nuovi valori inseriti nelle TextBox
                // Devi cercare i controlli usando l'ID che hai dato nell'EditItemTemplate
                GridViewRow row = GVDecretazione.Rows[e.RowIndex];
                Manager mn = new Manager();
               
                TextBox txtDecretanteMod = (TextBox)row.FindControl("txtDecretanteMod");
                TextBox txtDataMod = (TextBox)row.FindControl("txtDataMod");
                TextBox txtDecretatoMod = (TextBox)row.FindControl("txtDecretatoMod");
                TextBox txtNotaMod = (TextBox)row.FindControl("txtNotaMod");

                decr.data = string.IsNullOrWhiteSpace(txtDataMod.Text.Trim()) ? DateTime.MinValue : System.Convert.ToDateTime(txtDataMod.Text);
                // decr.data = System.Convert.ToDateTime(txtDataMod.Text);
                decr.id = idDecretazione;

                decr.decretato = string.IsNullOrWhiteSpace(txtDecretatoMod.Text.Trim()) ? string.Empty : txtDecretatoMod.Text.Trim();
                decr.decretante = string.IsNullOrWhiteSpace(txtDecretanteMod.Text.Trim()) ? string.Empty : txtDecretanteMod.Text.Trim();
                decr.nota = string.IsNullOrWhiteSpace(txtNotaMod.Text.Trim()) ? string.Empty : txtNotaMod.Text.Trim();
                SiteMaster myMaster = this.Master as SiteMaster;
               
                Boolean resp = mn.UpdDecretazione(decr);
                if (resp)
                {
                    //richiama popup dalla site master
                  

                    //if (myMaster != null)
                    //{
                        // 2. Chiamo il metodo pubblico
                        // myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdRegistroOk.GetDescription(), "success");
                        GVDecretazione.EditIndex = -1;
                        Decretazione_Click(sender, e);
                   // }
                }
                //Decretazione_Click(sender, e);

                //if (resp)
                //{
                //    //richiama popup dalla site master
                //    SiteMaster myMaster = this.Master as SiteMaster;

                //    if (myMaster != null)
                //    {
                //        // 2. Chiamo il metodo pubblico
                //        // myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdRegistroOk.GetDescription(), "success");
                //        GVDecretazione.EditIndex = -1;
                //        Ricerca_Click(sender, e);
                //    }
                //}

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
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }
        protected void apripopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {

            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);
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
            //Pulisci();
        }
        protected void chiudipopupDecretazione_Click(object sender, EventArgs e)
        {
            GVDecretazione.DataSource = null;
            GVDecretazione.DataBind();
            txtNotaDecretazione.Text = String.Empty;
            txtDecretante.Text = String.Empty;
            txtSearchOperatore.Value = String.Empty;
            txtDataDecretazione.Text = String.Empty;
            Hfdecretazione.Value = string.Empty;
            Session.Remove("decr");
            Isdecr = "false";
            if (!String.IsNullOrWhiteSpace(Isdecr))
            {

                string url = VirtualPathUtility.ToAbsolute("~/View/Visualizza.aspx");
                Response.Redirect(url, false);
            }



           // ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDecretazione')); modal.hide();", true);
            //adegua chiusura popup bootstrap 5
            string script = @"
    var modalElement = document.getElementById('ModalDecretazione');
    if (modalElement) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(modalElement);
        }
        modalInstance.hide();
    }";
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", script, true);
            // Pulisci();
        }
        protected void NascondiDiv()
        {
            DivProtocollo.Visible = false;
            DivEvasaAg.Visible = false;
            DivProtGen.Visible = false;
            DivIndirizzo.Visible = false;
            DivOperatori.Visible = false;
            DivDataArrivo.Visible = false;
            DivNominativo.Visible = false;
            DivProvenienza.Visible = false;
            DivGiudice.Visible = false;
            DivPratica.Visible = false;
            DivProcPenale.Visible = false;
            DivDettagli.Visible = false;
            Session.Remove("ListPratiche");
            // Session.Remove("ListRicerca");
            Hfdecretazione.Value = string.Empty;
        }

        protected void btNProtocollo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProtocollo.Visible = true;
            DivRicerca.Visible = true;

        }

        protected void btProcPenale_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProcPenale.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btProtGen_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProtGen.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btEvaseAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivEvasaAg.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivPratica.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivGiudice.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivRicerca.Visible = true;
            DivProvenienza.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivNominativo.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btDataArrivo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivDataArrivo.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btOperatori_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivOperatori.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivIndirizzo.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void gvPopupD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvPopupD.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);

        }
        protected void GVRicecaScheda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }
            gvPopupD.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);
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
                decr.unire = ckUnire.Checked;
                SiteMaster myMaster = this.Master as SiteMaster;
                Boolean ins = mn.InsDecretazione(decr);
                if (!ins)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserimento non effettuato, controllare il log." + "'); $('#errorModal').modal('show');", true);

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");
                    }
                }
                else
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserimento effettuato correttamente." + "'); $('#errorModal').modal('show');", true);


                    //HiddenField hf = (HiddenField)Master.FindControl("hfMasterParam");
                    //if (hf != null && !string.IsNullOrEmpty(hf.Value))
                    //{
                    //    string valoreRicevuto = hf.Value;




                    //    Pulisci();
                    //    CaricaDLL();
                    //    Session.Remove("ListRicerca");
                    //    Session.Remove("ListPratiche");
                    //    Session.Remove("decr");
                    //    Isdecr = String.Empty;
                    //    string url = VirtualPathUtility.ToAbsolute("~/View/Modifica.aspx");
                    //    Response.Redirect(url, false);
                    //}
                }
            }
            catch (Exception ex)
            {
                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = pagchiamante;
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
                string msg = ex.Message.Replace("\r", "").Replace("\n", " ");

                Response.Redirect(url + msg);
                //Response.Redirect("/Contact.aspx?errore=" + ex.Message);


            }
        }

        protected void Decretazione_Click(object sender, EventArgs e)
        {
            HfButtonProv.Value = "Decretazione";

            //I- 04/03/2026 decretazione 
            if (Isdecr == "false")
            {
                MostraMsg("⚠️ ATTENZIONE", Enumerate.MsgOutput.SavePratica.GetDescription(), "warning"); // aspetta la risposta del popup prima di procedere con la decretazione

            }
            else
                eseguiDecrtazione(sender, e);
            //F- 04/03/2026 decretazione 
            // Salva_Click(sender, e);
            //txtPraticaDecr.Text = txtProt.Text;
            //txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");

            //Manager mn = new Manager();
            //DataTable operatore = mn.getNominativoOperatore(Vuser);
            //if (operatore.Rows.Count > 0)
            //{
            //    if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
            //        txtDecretante.Text = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
            //}

            //DataTable decretazione = new DataTable();
            //if (!string.IsNullOrEmpty(txtPraticaDecr.Text))
            //{
            //    decretazione = mn.getListDecretazione(txtPraticaDecr.Text, Hid.Value);
            //}
            //if (decretazione.Rows.Count > 0)
            //{

            //    GVDecretazione.DataSource = decretazione;
            //    GVDecretazione.DataBind();
            //    Hfdecretazione.Value = decretazione.Rows[0].ItemArray[8].ToString();
            //    if (Hfdecretazione.Value == "True")
            //    {
            //        btAggiungiDecretazione.Enabled = false;
            //        btChiudiDecretazione.Enabled = false;
            //    }
            //    else

            //    {
            //        if (ruolo.ToUpper() == Enumerate.Ruolo.accertatori.ToString().ToUpper())
            //        {

            //            btSalva.Visible = false;
            //            btCercaQuartiere.Visible = false;
            //            if (Session["profilo"].ToString().Contains(Enumerate.Profilo.tre.GetHashCode().ToStringInvariant()))
            //                btChiudiDecretazione.Visible = true;
            //            else
            //                btChiudiDecretazione.Visible = false;
            //        }
            //        else
            //            btChiudiDecretazione.Visible = true;
            //        //    btAggiungiDecretazione.Enabled = true;
            //        //    btChiudiDecretazione.Enabled = true;
            //    }

            //}
            ////  else
            //// btChiudiDecretazione.Visible = false;
            // apripopupDecretazione_Click(sender, e);
        }

        protected void btChiudiDecretazione_Click(object sender, EventArgs e)
        {
            GVDecretazione.DataSource = null;
            GVDecretazione.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDataEvasa').modal('show');", true);

        }


        // Funzione  che carica i dati e applica il filtro
        private void PopulateGridView(string filterColumn = "", string filterValue = "")
        {

            DataTable dt = new DataTable();

            dt = GetOriginalData(); // ricerco la lista nuovamente
            try
            {
                //applico il filtro
                if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                {



                    string filterExpression = $"{filterColumn} LIKE '%{filterValue.Replace("'", "''")}%'";
                    DataRow[] filteredRows = dt.Select(filterExpression);

                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredDt = dt.Clone();
                        foreach (DataRow row in filteredRows)
                        {
                            filteredDt.ImportRow(row);
                        }
                        gvPopupD.DataSource = filteredDt;
                    }
                    else
                    {
                        gvPopupD.DataSource = null;
                    }

                }
                else
                {
                    gvPopupD.DataSource = dt; // Nessun filtro
                }
                gvPopupD.DataBind();
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
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

                        filtro = $"Nominativo LIKE '%{HfFiltroNominativo.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;
                        break;
                    case "Operatori":

                        filtro = $"Operatori LIKE '%{HfFiltroOperatori.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;


                }
                if (pratica.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvPopupD.DataSource = dv;
                    gvPopupD.DataBind();

                    txtPratica.Enabled = false;
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
                decr.decretato = txtSearchOperatore.Value.ToUpper();
                decr.nota = txtNotaDecretazione.Text.ToUpper();
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
                SiteMaster myMaster = this.Master as SiteMaster;
                if (!upd)
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.CloseKO.GetDescription(), "danger");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "chiusura non effettuata, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    Pulisci();
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.CloseOK.GetDescription(), "success");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "chiusura effettuata correttamente." + "'); $('#errorModal').modal('show');", true);

                }
         //       ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDataEvasa')); modal.hide();", true);
                //adegua chiusura popup bootstrap 5
                string script = @"
    var modalElement = document.getElementById('ModalDataEvasa');
    if (modalElement) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(modalElement);
        }
        modalInstance.hide();
    }";
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", script, true);
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
        //protected void DdlSigla_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //  CaricaDLL();
        //    if (DdlSigla.SelectedItem.Text == Enumerate.Sigla.AG.ToString().ToUpper())
        //    {
        //       // divAg.Visible = true;
        //        TxtTipoProvvAg.Text = DdlTipoProvvAg.SelectedItem.Text;
        //        CaricaDLL();
        //    }
        //    else
        //    {
        //   //     divAg.Visible = false;
        //        txtGiudice.Text = string.Empty;
        //        TxtTipoProvvAg.Text = string.Empty;
        //        txtProdPenNr.Text = string.Empty;

        //    }
        //}
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
            if (txtFilter.ID == "txtFilterIndirizzo")
            {
                columnName = "indirizzo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
                                                                   //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

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
            if (txtFilter.ID == "txtFilterNominativo")
            {
                columnName = "Nominativo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroNominativo.Value); // Esempio di funzione di filtro
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
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterOperatori")
            {
                columnName = "Operatori"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroOperatori.Value); // Esempio di funzione di filtro
                                                                     //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }

        protected void DdlTipoProvvAg_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtTipoProvvAg.Text = DdlTipoProvvAg.SelectedItem.Text;
        }

        protected void btChiudiAvvertenze_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalAvvertenze')); modal.hide();", true);
            //adegua chiusura popup bootstrap 5
            string script = @"
    var modalElement = document.getElementById('ModalAvvertenze');
    if (modalElement) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(modalElement);
        }
        modalInstance.hide();
    }";
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", script, true);

        }

        protected void NuovaModifica_Click(object sender, EventArgs e)
        {
            Pulisci();
            DivRicerca.Visible = true;
            DivDettagli.Visible = false;
            Session.Remove("ListRicerca");
            Session.Remove("ListPratiche");
        }
        protected void btAggiungi_Click(object sender, EventArgs e)
        {

            // Crea un nuovo ListViewDataItem con un valore di esempio
            ListViewDataItem itemToAdd = new ListViewDataItem(0, 0);
            itemToAdd.DataItem = DdlOperatori.SelectedItem.Text;

            // Verifica se l'elemento è già presente nella ListView
            bool itemExists = false;
            if (ListOperatori.Items.Count > 3)
            {
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.Maxoperatori.GetDescription(), "warning");
                }

                return;
            }
            foreach (var item in ListOperatori.Items)
            {
                if (item.ToString() == itemToAdd.DataItem.ToString())
                {
                    itemExists = true;
                    break;
                }
            }

            // Aggiungi l'elemento solo se non esiste già
            if (!itemExists)
            {
                ListOperatori.Items.Add(DdlOperatori.SelectedItem.Text);
            }

        }

        protected void btElimina_Click(object sender, EventArgs e)
        {
            if (ListOperatori.SelectedItem != null)
                ListOperatori.Items.Remove(ListOperatori.SelectedItem);
        }

        protected void BtConferma_Click(object sender, EventArgs e)
        {
            Salva_Click(sender, e);
            eseguiDecrtazione(sender, e);
        }

        protected void BtNo_Click(object sender, EventArgs e)
        {
            eseguiDecrtazione(sender, e);
        }
        protected void eseguiDecrtazione(object sender, EventArgs e)
        {
            txtPraticaDecr.Text = (string.IsNullOrEmpty(txtPraticaDecr.Text)) ? txtProt.Text : txtPraticaDecr.Text;
            //txtPraticaDecr.Text = txtProt.Text;
            txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");

            Manager mn = new Manager();
            DataTable operatore = mn.getNominativoOperatore(Vuser);
            if (operatore.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
                    txtDecretante.Text = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
            }

            DataTable decretazione = new DataTable();
            if (!string.IsNullOrEmpty(txtPraticaDecr.Text))
            {
                string dborigine = Session["dbtarget"].ToString();
                decretazione = mn.getListDecretazione(txtPraticaDecr.Text, Hid.Value, dborigine);
            }
            if (decretazione.Rows.Count > 0)
            {

                GVDecretazione.DataSource = decretazione;
                GVDecretazione.DataBind();
                Hfdecretazione.Value = decretazione.Rows[0].ItemArray[8].ToString();
                ckUnire.Checked = Convert.ToBoolean(decretazione.Rows[0]["decr_unire"]);
                if (Hfdecretazione.Value == "True")
                {
                    btAggiungiDecretazione.Enabled = false;
                    btChiudiDecretazione.Enabled = false;
                }
                else

                {
                    if (ruolo.ToUpper() == Enumerate.Ruolo.operatori.ToString().ToUpper())
                    {

                        btSalva.Visible = false;
                        btCercaQuartiere.Visible = false;
                        if (Session["profilo"].ToString().Contains(Enumerate.Profilo.tre.GetHashCode().ToStringInvariant()))
                            btChiudiDecretazione.Visible = true;
                        else
                            btChiudiDecretazione.Visible = false;
                    }
                    else
                        btChiudiDecretazione.Visible = true;

                }

            }

            apripopupDecretazione_Click(sender, e);
        }
        public void MostraMsg(string titolo, string messaggio, string tipoMessaggio = "info")
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
            string script = "$('#ModalRichiestaSalva').modal('show');";

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ModalRichiestaSalva", script, true);
        }

       
    }
}