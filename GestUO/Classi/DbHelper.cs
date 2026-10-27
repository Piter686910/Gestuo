using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public static class DbHelper
    {
        /// <summary>
        /// Restituisce la stringa di connessione sostituendo il Database con quello presente in Sessione.
        /// </summary>
        public static string GetConnectionString()
        {
            // 1. Recupera la stringa base dal web.config
            string baseConn = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            // 2. Inizializza lo builder di SQL Server
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(baseConn);

            // 3. Recupera il nome del DB salvato in Sessione (dopo il click nella pagina Menu.aspx)
    //        string dbSelezionato = HttpContext.Current?.Session["dbtarget"] as string;

            // 4. Se la sessione contiene un DB valido, sovrascrive quello del web.config
            //if (!string.IsNullOrEmpty(dbSelezionato))
            //{
            //    builder.InitialCatalog = dbSelezionato; // Modifica il parametro "Database="
            //}

            return builder.ConnectionString;
        }

    }
}