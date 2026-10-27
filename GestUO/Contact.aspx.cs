using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestUO
{
    public partial class Contact : Page
    {
        string pagina = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            pagina = Session["PaginaChiamante"] as string;
            if (!IsPostBack)
            {
                string messaggioErrore = Session["MessaggioErrore"] as string;
               

                if (!string.IsNullOrEmpty(messaggioErrore))
                {
                    // URL decode (anche se in questo caso non strettamente necessario dato UrlEncode in invio, ma buona pratica)
                    messaggioErrore = Server.UrlDecode(messaggioErrore);

                    // Visualizza il messaggio di errore (es. in una Label)
                    LabelMessaggioErrore.Text = messaggioErrore;
                    Session.Remove("MessaggioErrore");
                }

            }
        }

        protected void ButtonTornaIndietro_Click(object sender, EventArgs e)
        {
            // Recupera l'URL della pagina precedente (chiamante)
            if (Request.UrlReferrer != null)
            {
                // Reindirizza alla pagina precedente
                Response.Redirect(pagina);
            }
            else
            {
                // Se non c'è una pagina precedente (es. accesso diretto a pagina2),
                // puoi reindirizzare ad una pagina di default (es. la homepage)
                //Response.Redirect("~/View/Default.aspx"); // o un'altra pagina di fallback appropriata
                string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                Response.Redirect(url, false);
            }
        }
    }
}