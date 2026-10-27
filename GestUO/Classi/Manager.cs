using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using static GestUO.Classi.Enumerate;
using DataTable = System.Data.DataTable;

namespace GestUO.Classi
{
    public class Manager
    {

        //public String ConnString = ConfigurationManager.AppSettings["ConnString"];
        public String ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();

        public String path = ConfigurationManager.AppSettings["PathSqlAudit"];

        public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        public string msg = string.Empty;
        //delete
        public bool DeleteTran(string numero_pratica)
        {
            string delAna = "DELETE FROM anagrafica1 WHERE numero_pratica_accertamenti = @numero_pratica";
            string delAcc = "DELETE FROM Accertamenti WHERE num_pratica_accertamenti = @numero_pratica";

            bool resp = false;

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();

                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = conn.CreateCommand())
                        {
                            command.Transaction = tran;

                            // Aggiunta del parametro al comando per evitare SQL Injection
                            command.Parameters.AddWithValue("@numero_pratica", numero_pratica);

                            // 1. Cancellazione da anagrafica1
                            command.CommandText = delAna;
                            int res = command.ExecuteNonQuery();

                            if (res > 0)
                            {
                                // 2. Cancellazione da Accertamenti
                                command.CommandText = delAcc;
                                command.ExecuteNonQuery();

                                tran.Commit();
                                resp = true;
                            }
                            else
                            {
                                // Nessuna riga trovata in anagrafica1
                                tran.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // In caso di errore annulla la transazione
                        try
                        {
                            tran.Rollback();
                        }
                        catch (Exception)
                        {
                            // Gestione eventuale eccezione durante il rollback
                        }

                        resp = false;
                    }
                }
            }

            return resp;
        }
        //delete
        public Boolean DeleteRappUote(String numero_pratica)
        {

            String Del_RappUote = "delete from RappUote where rapp_numero_pratica = '" + numero_pratica + "'";

            String testoSql = String.Empty;

            Boolean resp = false;

            using (SqlConnection conn1 = new SqlConnection(ConnString))
            {
                conn1.Open();
                SqlCommand command = conn1.CreateCommand();

                try
                {

                    command.CommandText = Del_RappUote;
                    testoSql = "RappUote";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                    {

                        resp = true;
                    }


                }

                catch (Exception)
                {
                    resp = false;
                }
                conn1.Close();
                return resp;
            }
        }
        public bool DeleteMatricola(string matricola)
        {
            string delOperatore = "DELETE FROM utenti WHERE matricola = @matricola";
            bool resp = false;

            using (MySqlConnection conn1 = new MySqlConnection(ConnString))
            {
                try
                {
                    conn1.Open();

                    using (MySqlCommand command = new MySqlCommand(delOperatore, conn1))
                    {
                        // Parametrizzazione della query
                        command.Parameters.AddWithValue("@matricola", matricola);

                        int res = command.ExecuteNonQuery();
                        if (res > 0)
                        {
                            resp = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gestione dell'eccezione (es. log dell'errore)
                    resp = false;
                }
            } // Il 'using' chiude automaticamente la connessione conn1 alla fine del blocco

            return resp;
        }
        /// <summary>
        /// cancella i file per nome
        /// </summary>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public Boolean DeleteFileCaricati(String nomefile)
        {

            String Del_FileCaricati = "delete from File_Caricati where nomefile = '" + nomefile + "'";

            String testoSql = String.Empty;

            Boolean resp = false;

            using (SqlConnection conn1 = new SqlConnection(ConnString))
            {
                conn1.Open();
                SqlCommand command = conn1.CreateCommand();

                try
                {

                    command.CommandText = Del_FileCaricati;
                    testoSql = "FileCaricati";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn1.Close();
                return resp;
            }
        }
        public Boolean DelInterrogatorioById(int id)
        {
            string sql = string.Empty;
            string Del_Interrogatori = "delete  FROM Interrogatori where id = " + id;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_Interrogatori;
                    //testoSql = "Registro";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        public Boolean DelRegistroById(int id)
        {
            string sql = string.Empty;
            string Del_Registro = "delete  FROM RegistroUrp where id_registro = " + id;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_Registro;
                    //testoSql = "Registro";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        /// <summary>
        /// cancella i file con flag cancella a true
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteFileScaricati()
        {
            String Del_FileCaricati = "delete from File_Caricati where cancella = 'True'";

            String testoSql = String.Empty;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_FileCaricati;
                    testoSql = "FileCaricati";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }

        /// <summary>
        /// delete la tabella rsnl
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteRSNL()
        {
            String Del_FileCaricati = "delete from rsnl ";

            String testoSql = String.Empty;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_FileCaricati;
                    testoSql = "rsnl";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        //get
        public DataTable getPass(string user)
        {
            try
            {
                string matricolaPulita = (user ?? string.Empty).Trim();
                DataTable tb = new DataTable();
                string sql = "SELECT passw FROM utenti WHERE Matricola = TRIM(@user) LIMIT 1;";

                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("@user", MySqlDbType.VarChar, 11)
                        {
                            Value = matricolaPulita
                        });

                        // MySqlDataAdapter apre e chiude la connessione automaticamente ed effettua la Fill
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }
                }

                return tb;
            
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine("matricola:" + user + ", " + ex.Message + @" getpass" + "; ");
                }

                return null;
            }
        }
        public DataTable GetRuolo(string user)
        {
            string sql = "SELECT ruolo FROM utenti WHERE Matricola = @user";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione per prevenire SQL Injection
                    cmd.Parameters.AddWithValue("@user", user);

                    // Invocazione di FillTable con l'oggetto MySqlCommand
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getUserByUserPassw(string user, string pasw)
        {
            try
            {
                string sql = "SELECT * FROM utenti WHERE Matricola = @user AND passw = @pasw";

                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        // Parametri per prevenire SQL Injection
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@pasw", pasw);

                        return FillTable(cmd, out msg);
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
                    sw.WriteLine("matricola:" + user + ", " + ex.Message + @" getUserByUserPassw" + "; ");
                }

                return null;
            }
        }
        public DataTable getUserRules(string user)
        {
            string sql = "SELECT * FROM utenti WHERE Matricola = @user";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione per prevenire SQL Injection
                    cmd.Parameters.AddWithValue("@user", user);

                    // Invocazione del metodo FillTable convertito per MySqlCommand
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListGiudice(out string msg)
        {
            string sql = "SELECT * FROM Giudice ORDER BY Giudice";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListProvenienza(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Provenienza order by Provenienza";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListTipologiaAbuso(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM TipologiaAbuso order by tipologia";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListTipologia(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Tipologia order by tipo_nota";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListScaturito(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Scaturito order by scaturito";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListRicercaEsitoUrp(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM EsitoUrp order by descrizione";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListQuartiere(out string msg)
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT distinct quartiere FROM Quart order by quartiere";
            string sql = "SELECT MIN(id_quartiere) AS id_quartiere, quartiere FROM Quart GROUP BY quartiere ORDER BY quartiere";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListQuartiereTP(out string msg)
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT distinct quartiere FROM Quart order by quartiere";
            string sql = "SELECT id, quartiere FROM Quartiere ORDER BY quartiere";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getQuartiere(string indirizzo, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Quart where toponimo like '%" + indirizzo.Replace("'", "''") + "%'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }

        /// <summary>
        /// preleva il max pregressivo per l'anno e mese corrente per la gestione auto
        /// </summary>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable MaxProgressivoGestioneAuto(int anno, string mese)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT COUNT(*) AS MaxNumero FROM gestioneauto WHERE ANNO = @anno AND mese = @mese";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Aggiunta dei parametri in sicurezza
                    cmd.Parameters.AddWithValue("@anno", anno);
                    cmd.Parameters.AddWithValue("@mese", mese);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(tb);
                    }
                }
            } // Il blocco 'using' si occupa di chiudere e disporre automaticamente sia la connessione che il comando

            return tb;
        }
        public DataTable MaxNPr(string anno)
        {
            DataTable tb = new DataTable();

            if (string.IsNullOrWhiteSpace(anno)) return tb;

            // Query MySQL parametrizzata e priva di sintassi T-SQL incompatibile
            string sql = @"
        SELECT 
            ANNO, 
            MAX(CAST(nr_carico AS UNSIGNED)) AS MaxNumero 
        FROM carico 
        WHERE nr_carico REGEXP '^[0-9]+$' 
          AND ANNO = @anno 
        GROUP BY ANNO;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@anno", anno.Trim());

                    try
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
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
                            sw.WriteLine($"Anno:{anno} - {ex.Message} - Errore in MaxNPr");
                        }
                    }
                }
            }

            return tb;
        }

        public DataTable getListCarico(string ma, string capoarea, string dataCarico, string sigla, out string msg)
        {
            //string connectionString = DbHelper.GetConnectionString();

            string sql = @"
        WITH Classificata AS (
            SELECT 
                p.Nr_carico AS carico,
                d.decr_pratica,
                d.decr_decretato,
                d.decr_data,
                d.decr_nota AS Note,
                d.decr_chiuso,
                p.Macro_area,
                p.DataCarico,
                p.Tipologia_atto,
                p.nr_pratica AS pratica,
                p.QUARTIERE AS località,
                p.DataInserimento,
                d.decr_unire AS UNIRE,
                ROW_NUMBER() OVER (PARTITION BY d.decr_pratica ORDER BY d.decr_data DESC) AS rn
            FROM Decretazione AS d
            JOIN carico AS p ON d.decr_pratica = p.Nr_Protocollo
            WHERE p.Macro_area = @ma 
              AND p.Sigla = @sigla 
              AND d.decr_chiuso = 0 
              AND d.decr_decretato = @capoarea
              AND (
                  CAST(p.DataCarico AS DATE) = STR_TO_DATE(@dataCarico, '%Y-%m-%d')
                  OR (
                      YEAR(d.decr_data) = YEAR(STR_TO_DATE(@dataCarico, '%Y-%m-%d'))
                      AND WEEK(d.decr_data, 3) = WEEK(STR_TO_DATE(@dataCarico, '%Y-%m-%d'), 3)
                  )
              )
        )
        SELECT carico, pratica, DataCarico, località, Note, UNIRE, decr_decretato, Macro_area
        FROM Classificata
        WHERE rn = 1
        ORDER BY carico;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione completa
                    cmd.Parameters.AddWithValue("@ma", ma);
                    cmd.Parameters.AddWithValue("@sigla", sigla);
                    cmd.Parameters.AddWithValue("@capoarea", capoarea);
                    cmd.Parameters.AddWithValue("@dataCarico", dataCarico);

                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListQuartina(int anno)
        {
            string sql = @"SELECT 
                    anno, quartina, gennaio, febbraio, marzo, aprile, 
                    maggio, giugno, luglio, agosto, settembre, ottobre, 
                    novembre, dicembre 
                   FROM quartina 
                   WHERE anno = @anno";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione della query
                    cmd.Parameters.AddWithValue("@anno", anno);

                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// ottiene nominativo operatore dalla matricola
        /// </summary>
        /// <param name="matricola"></param>
        /// <returns></returns>
        public DataTable getNominativoOperatore(string matricola)
        {
            string sql = "SELECT nominativo FROM utenti WHERE matricola = @matricola";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione sicura
                    cmd.Parameters.AddWithValue("@matricola", matricola);

                    return FillTable(cmd, out msg);
                }
            }
        }
        public string getCapoArea(string area)
        {
            string sql = "SELECT nominativo FROM utenti WHERE macroarea = @area AND profilo LIKE '%R%'";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@area", area);

                    DataTable tb = FillTable(cmd, out string msg);

                    if (tb != null && tb.Rows.Count > 0)
                    {
                        return tb.Rows[0]["nominativo"].ToString();
                    }
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// ottiene la matricola dal nominativo
        /// </summary>
        /// <param name="nominativo"></param>
        /// <returns></returns>
        public DataTable getMatricolaOperatore(string nominativo)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT matricola FROM utenti where nominativo = @nominativo";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione sicura
                    cmd.Parameters.AddWithValue("@nominativo", nominativo);

                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListOperatore(out string msg)
        {
            string sql = "SELECT nominativo FROM utenti WHERE nominativo <> '' AND abilitato = 1 ORDER BY nominativo";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }

        /// <summary>
        /// lista operatori per unità operativa
        /// </summary>
        /// <param name="uo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable getListOperatore(string uo, out string msg)
        {
            //  string sql = "SELECT nominativo, matricola,grado FROM utenti WHERE abilitato = 1 and uo = @uo ORDER BY nominativo";
            string sql = @"
        SELECT 
            matricola, 
            nominativo, 
            grado,
            CONCAT_WS(' - ', NULLIF(grado, ''), nominativo, CONCAT('(', matricola, ')')) AS DisplayMember
        FROM utenti 
        WHERE abilitato = 1 
          AND uo = @uo 
        ORDER BY nominativo;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uo", uo); 
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable GetSchedeByPattuglia(string uoSelezionata, string pattuglia,out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM rapportino where pattuglia like @pattuglia and uo = @uo order by dataConsegna ";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@pattuglia", $"%{pattuglia}%");
                    cmd.Parameters.AddWithValue("@uo", uoSelezionata);
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListOperatoreCompleta(string uoSelezionata,out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM utenti where nominativo <> '' and uo = @uo order by nominativo ";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uo", uoSelezionata);
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// estrae le deleghe in scadenza per area e operatore, con filtro giorni per data carico (es. gg = 30 => deleghe caricate da più di 30 giorni e non ancora evase)
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Matricola"></param>
        /// <param name="gg"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable getListDelegheInScadenza(string Area, string Matricola, out string msg)
        {
            string sql = string.Empty;

            if (Area == "A")
            {
                sql = @"SELECT * FROM principale 
                WHERE sigla = 'AG' 
                  AND evasa = 0 
                  AND macro_area IN ('MA1', 'MA2', 'MA3') 
                  AND ggdelega > 0 
                ORDER BY datacarico ASC";
            }
            else
            {
                sql = @"SELECT * FROM principale 
                WHERE sigla = 'AG' 
                  AND macro_area = @area 
                  AND ggdelega > 0 
                  AND evasa = 0 
                ORDER BY datacarico ASC";
            }

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (Area != "A")
                    {
                        cmd.Parameters.AddWithValue("@area", Area);
                    }

                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// ricerca solo gli operatori abilitati
        /// </summary>
        /// <returns></returns>
        public DataTable getListOperatori(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT nominativo FROM utenti where (ruolo = '" + Enumerate.Ruolo.operatori.GetDescription() + "' or ruolo= '" + Enumerate.Ruolo.PG.GetDescription() + "') and abilitato=1 order by nominativo ";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable GetListRegistro(out string msg)
        {
            DataTable dt = new DataTable();

            string sql = "SELECT oggetto,dataPresentRichiesta, nrPgTrasmissioneRichiesto, uffDetentore,controInteressati,esito,motivazione, nrPgTrasmissioneRiscontro, dataConclProcedimento FROM RegistroUrp " +
    " ORDER BY dataConclProcedimento";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }

        }

        public DataTable getCheck(string pratica, string anno)
        {
            DataTable tb = new DataTable();

            //estrae informazioni su chi ha visualizzato la pratica, statement NOT LIKE '%sys.fn_get_audit_file%=esclude i doppioni

            //            string sql = @" SELECT
            //    MAX(event_time) AS[Data Accesso], 
            //    CASE
            //        WHEN statement LIKE '%User:%' THEN
            //            REPLACE(SUBSTRING(statement, CHARINDEX('User:', statement) + 5,
            //            CHARINDEX('''', statement, CHARINDEX('User:', statement) + 5) - (CHARINDEX('User:', statement) + 5)), '''', '')
            //        ELSE 'Utente Generico'
            //    END AS[Operatore],
            //    server_principal_name AS[Account SQL],
            //    client_ip AS[Indirizzo IP]
            //FROM sys.fn_get_audit_file('C:\SQLAudit\*', NULL, NULL)
            //WHERE statement LIKE @prot 
            //  AND statement LIKE @anno
            //  AND statement NOT LIKE '%sys.fn_get_audit_file%' 
            //GROUP BY
            //    statement,
            //    server_principal_name,
            //    client_ip
            //ORDER BY[Data Accesso] DESC";
            //FROM sys.fn_get_audit_file('C:\SQLAudit\*', NULL, NULL)
            string sql = @"SELECT 
    LOGS.[Data Accesso],
    ISNULL(OP.NOMINATIVO, LOGS.CodiceOperatore) AS [Operatore],
    LOGS.[Account SQL],
    LOGS.[Indirizzo IP]
FROM (
    SELECT 
        MAX(event_time) AS [Data Accesso], 
        CASE 
            WHEN statement LIKE '%User:%' THEN 
                REPLACE(SUBSTRING(statement, CHARINDEX('User:', statement) + 5, 
                CHARINDEX('''', statement, CHARINDEX('User:', statement) + 5) - (CHARINDEX('User:', statement) + 5)), '''', '')
            ELSE 'N/A' 
        END AS CodiceOperatore,
        server_principal_name AS [Account SQL],
        client_ip AS [Indirizzo IP],
        statement
    
FROM sys.fn_get_audit_file(@path, NULL, NULL)
WHERE statement LIKE @prot 
      AND statement LIKE @anno
      AND statement NOT LIKE '%sys.fn_get_audit_file%'
    GROUP BY statement, server_principal_name, client_ip
) AS LOGS
LEFT JOIN OPERATORE OP ON LOGS.CodiceOperatore = CAST(OP.MATRICOLA AS VARCHAR(50))
ORDER BY LOGS.[Data Accesso] DESC";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Passiamo SOLO il valore con i caratteri jolly % per la clausola LIKE
                    cmd.Parameters.AddWithValue("@prot", "%" + pratica + "%");
                    cmd.Parameters.AddWithValue("@anno", "%" + anno + "%");

                    // Tipo specifico MySQL (MySqlDbType.VarChar o MySqlDbType.Text)
                    cmd.Parameters.Add("@path", MySqlDbType.VarChar, 4000).Value = "%" + path.Trim() + "%";

                    // Timeout in secondi
                    cmd.CommandTimeout = 120;

                    // Uso del DataAdapter nativo per MySQL
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        try
                        {
                            conn.Open();
                            da.Fill(tb); // Riempie la DataTable usando il MySqlCommand configurato
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message; // Gestione eccezione e logging
                        }
                    }
                }
            }
            return tb;
        }
        public DataTable getListDipendenti()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT ufficio, s.nominativo,matricola_ced, grado, data_assunzione, id_dip, autista, armato, quartina,gruppo_quartina,gruppo_reperibilita,permessi_studio," +
                         "perm_53,perm_104,limitazioni,turni_pref,Macro_area,s.area,data_sorv_sanitaria, o.abilitato FROM SchedaDipendente as s left join operatore as o on s.matricola= o.matricola" +
                         " where o.abilitato='True'  ORDER BY ufficio,nominativo";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        //public DataTable getListDipendentiById(int id)
        //{
        //    DataTable tb = new DataTable();
        //    string sql = "SELECT ufficio, nominativo,matricola, grado, data_assunzione, id_dip, autista, armato, quartina,gruppo_quartina,gruppo_reperibilita,permessi_studio," +
        //                 "perm_53,perm_104,limitazioni,turni_pref,turni_blocc,Macro_area,area,data_sorv_sanitaria FROM SchedaDipendente where abilitato='True' ORDER BY ufficio,nominativo";
        //    using (SqlConnection conn = new SqlConnection(ConnString))
        //    {

        //        return tb = FillTable(sql, conn, out msg);
        //    }
        //}
        public Boolean getTipoProv(string tipo)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM TipoNotaAG where tipologia = '" + tipo + "'";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {


                    MySqlDataAdapter da;
                    System.Data.DataSet ds;

                    da = new MySqlDataAdapter(sql, conn);
                    ds = new System.Data.DataSet();
                    da.Fill(ds);

                    tb = ds.Tables[0];
                    if (tb.Rows.Count > 0)
                        return true;
                    else

                        return false; ;
                }
            }
        }
        public DataTable getListProvvAg(String sigla, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM TipoNotaAG where sigla = '" + sigla + "' order by tipologia";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListInviati(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM inviati order by inviata";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// cerca indirizzo nella tabella quartiere
        /// </summary>
        /// <returns></returns>
        public DataTable getListIndirizzo(out string msg)
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT specie,toponimo  FROM Quart order by toponimo";
            string sql = "SELECT ISNULL(Specie, '') + ' ' + ISNULL(toponimo, '') AS SpecieToponimo FROM  Quart";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListPraticheVal(string pratica, string anno, out string msg)
        {
            DataTable tb = new DataTable();


            string sql = "SELECT id,nr_Pratica ,nr_protocollo, evasa,anno FROM [principale] WHERE Anno ='" + anno + "' and nr_Pratica = '" + pratica +
                                     "' GROUP BY nr_Pratica ,nr_protocollo,evasa,anno,id HAVING COUNT(Nr_Protocollo) >= 1 ORDER BY Nr_Protocollo DESC";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// ricerca per carico relativo al db di origine(in funzione di appartenenza alla uo), con join su utenti per nominativo operatore
        /// </summary>
        /// <param name="sessionUser"></param>
        /// <param name="carico"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable getListCaricoTabella(string sessionUser, string carico, string anno, string uo, bool ckGeneralizzato, out string msg)
        {
            DataTable tb = new DataTable();


            // Costruzione dinamica della clausola WHERE per la UO
             string filtroUo = ckGeneralizzato ? " AND P.uo = @uo" : string.Empty; 

            string sql = $@"
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.Nr_carico = @Carico 
          AND P.anno = @Anno
          {filtroUo}
        ORDER BY P.dataarrivo DESC;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carico", carico.Trim());
                        cmd.Parameters.AddWithValue("@Anno", anno.Trim());
                        //cmd.Parameters.AddWithValue("@uo", uo ?? string.Empty);
                        // Aggiunge il parametro uo solo se il filtro non è generalizzato
                        if (ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo ?? string.Empty);
                        }

                        // Esecuzione tramite helper FillTable
                        return FillTable(cmd, out msg);
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine($"Carico:{carico}, Anno:{anno}, UO:{uo} - {ex.Message} - Errore in getListCaricoTabella"); 
                    }
                    return tb;
                }
            }
        }
        /// <summary>
        /// ricerca le diverse decretazioni per la pratica selezionata
        /// </summary>
        /// <param name="pratica"></param>
        /// <param name="idPratica"></param>
        /// <returns></returns>
        public DataTable getListDecretazione(string pratica, string idPratica, string uo)
        {
            DataTable tb = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {

                string sql = $@"
        SELECT * from Decretazione 
        WHERE decr_pratica = @Pratica
          AND decr_Idpratica = @IdPratica order by decr_data desc";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Pratica", pratica);
                    cmd.Parameters.AddWithValue("@IdPratica", idPratica);
                    cmd.Parameters.AddWithValue("@uo", uo);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    return dt;
                }
            }
        }

        /// <summary>
        /// ricerca le diverse decretazioni per la pratica selezionata se chiamata dal comandante generale
        /// </summary>
        /// <param name="carico"></param>
        /// <param name="idPratica"></param>
        /// <param name="anno"></param>
        /// <param name="dbOrigineOut"></param>
        /// <returns></returns>
        public DataTable CmdGetListDecretazione(string carico, string idPratica, string anno, string uo)
        {
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();

                string sql = $@"
        SELECT * from Decretazione 
        WHERE  decr_Idpratica = @decr_Idpratica order by decr_data desc";
                //AND decr_pratica = @decr_pratica order by decr_data desc";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@decr_Idpratica", idPratica);
                    cmd.Parameters.AddWithValue("@decr_uo", uo);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);

                    }
                    return dt;
                }
            }

        }
        /// <summary>
        /// ricerca per procedimento penale
        /// </summary>
        /// <param name="procedimento"></param>

        /// <returns></returns>
        public DataTable getListProcedimento(string ProcPenale, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            // il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
            string sqlCompleta = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.ProcedimentoPen like @ProcPenale 
        ORDER BY P.dataarrivo DESC;";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                {
                    // cmd.Parameters.AddWithValue("@SessionUser", sessionUser ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ProcPenale", $"%{ProcPenale}%");
                    cmd.Parameters.AddWithValue("@uo", $"%{uo}%");
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(tb);
                    }
                    msg = "OK";
                    return tb;
                }

            }
        }
        /// <summary>
        /// ricerca per evasa
        /// </summary>
        /// <param name="datada"></param>
        ///  /// <param name="dataa"></param>
        /// <returns></returns>
        public DataTable getListEvasaAg(string datada, string dataa, string uo, out string msg)
        {
            DataTable tb = new DataTable();

            DateTime dtda = System.Convert.ToDateTime(datada);
            DateTime dta = System.Convert.ToDateTime(dataa);
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM carico P LEFT JOIN utenti S ON P.matricola = S.matricola " +
                " where EvasaData BETWEEN @dtda and @dta and P.uo = @uo order by dataarrivo desc";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@dtda", dtda);
                    cmd.Parameters.AddWithValue("@dta", dta);
                    cmd.Parameters.AddWithValue("@uo", uo);
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// ricerca per protocollo generale
        /// </summary>
        /// <param name="protgen"></param>
        /// <returns></returns>
        public DataTable getListProtGen(string ProtGenerale, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                // Costruzione dinamica della query in base a ckGeneralizzato
                string sql = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.Rif_Prot_Gen LIKE @ProtGenerale
        {(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY P.dataarrivo DESC;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProtGenerale", $"%{ProtGenerale}%");

                    if (!ckGeneralizzato)
                    {
                        cmd.Parameters.AddWithValue("@uo", uo);
                    }

                    return FillTable(cmd, out msg);
                }
            }

        }
        public DataTable getListProtGenInDecretazione(string ProtGenerale, Boolean ckGeneralizzato, String uo, out string msg)
        {

            DataTable tb = new DataTable();
            try
            {


                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    string sqlCompleta = $@"        
            SELECT p.Id,p.Nr_Carico,p.Sigla,p.DataArrivo,p.Provenienza,p.Tipologia_atto,p.Giudice,p.TipoProvvedimentoAG,p.ProcedimentoPen,p.Nominativo,p.Indirizzo,p.Evasa, s.nominativo as nomeoperatore
            ,p.EvasaData,p.Inviata,p.DataInvio,p.Scaturito,p.Operatori,p.DataCarico,p.nr_Pratica,p.Quartiere,p.Note,p.Anno,p.Giorno,p.Rif_Prot_Gen,p.Matricola,p.DataInserimento,p.Macro_area
            ,p.Operatori2,p.Operatori3,p.UlterioreTipoAtto,p.BU,p.CodiceEdificio
        FROM Carico p LEFT JOIN decretazione d ON d.decr_idPratica = p.id
        LEFT JOIN utenti S 
            ON p.matricola = S.matricola 
        WHERE d.decr_nota like @ProtGenerale 
 {(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY p.dataarrivo DESC";

                    using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProtGenerale", $"%{ProtGenerale}%");
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }


                    msg = "OK";
                    return tb;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }

        }
        /// <summary>
        /// ricerca per giudice
        /// </summary>
        /// <param name="giudice"></param>
        /// <returns></returns>
        public DataTable getListGiudice(string giudice, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();

            string dbOrigine = string.Empty;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {

                    conn.Open();
                    // il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
                    string sqlCompleta = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.giudice = @giudice 
{(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY P.dataarrivo DESC OPTION (RECOMPILE);";

                    using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                    {
                        cmd.Parameters.AddWithValue("@giudice", giudice);
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }
                    msg = "OK";
                    return tb;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }

        }
        /// <summary>
        /// preleva le statistiche del mese e anno selezionati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable getStatisticaByMeseAnno(string mese, int anno)
        {
            DataTable tb = new DataTable();

            if (string.IsNullOrWhiteSpace(mese)) return tb;

            // Query parametrizzata per MySQL
            string sql = @"
        SELECT * 
        FROM statistiche 
        WHERE LOWER(mese) = LOWER(@mese) 
          AND anno = @anno;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Aggiunta parametri sicuri
                    cmd.Parameters.AddWithValue("@mese", mese.Trim());
                    cmd.Parameters.AddWithValue("@anno", anno);

                    try
                    {
                        conn.Open();
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
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
                            sw.WriteLine($"Mese:{mese}, Anno:{anno} - {ex.Message} - Errore in getStatisticaByMeseAnno");
                        }

                    }
                }
            }

            return tb;
        }

        /// <summary>
        /// ricerca singolo inviata
        /// </summary>
        /// <param name="inviata"></param>
        /// <returns></returns>
        public Boolean getGiudice(string giudice)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM giudice where giudice = '" + giudice.Replace("'", "''") + "'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                MySqlDataAdapter da;
                System.Data.DataSet ds;

                da = new MySqlDataAdapter(sql, conn);
                ds = new System.Data.DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }

        }
        /// <summary>
        /// ricerca singolo inviati
        /// </summary>
        /// <param name="inviata"></param>
        /// <returns></returns>
        public Boolean getInviata(string inviata)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM inviati where inviata = '" + inviata.Replace("'", "''") + "'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                MySqlDataAdapter da;
                System.Data.DataSet ds;

                da = new MySqlDataAdapter(sql, conn);
                ds = new System.Data.DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        /// <summary>
        /// ricerca la singola provenienza
        /// </summary
        /// <param name="provenienza"></param>
        /// <returns></returns>
        public Boolean getProvenienza(string provenienza)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM provenienza where provenienza = '" + provenienza.Replace("'", "''").Replace("*", "%") + "'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                MySqlDataAdapter da;
                System.Data.DataSet ds;

                da = new MySqlDataAdapter(sql, conn);
                ds = new System.Data.DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        /// <summary>
        /// ricerca singolo tipo atto
        /// </summary>
        /// <param name="tipoatto"></param>
        /// <returns></returns>
        public Boolean getTipoAtto(string tipoatto)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM tipologia where tipo_nota = '" + tipoatto.Replace("'", "''") + "'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                MySqlDataAdapter da;
                System.Data.DataSet ds;

                da = new MySqlDataAdapter(sql, conn);
                ds = new System.Data.DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        public Boolean getTipoScaturito(string scaturito)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM scaturito where scaturito = '" + scaturito.Replace("'", "''") + "'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                MySqlDataAdapter da;
                System.Data.DataSet ds;

                da = new MySqlDataAdapter(sql, conn);
                ds = new System.Data.DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        /// <summary>
        /// ricerca lista per provenienza
        /// </summary>
        /// <param name="provenienza"></param>
        /// <returns></returns>
        public DataTable getListProvenienza(string provenienza, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    //il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
                    string sql = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.provenienza like @provenienza 
{(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY P.dataarrivo DESC OPTION (RECOMPILE);";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@provenienza", $"%{provenienza}%");
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                        msg = "OK";
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }

        }
        /// <summary>
        /// ricerca per nominativo
        /// </summary>
        /// <param name="nominativo"></param>
        /// <returns></returns>
        public DataTable getListNominativo(string nominativo, bool ckGeneralizzato, string uo, out string msg)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    // Costruzione dinamica della query senza 'OPTION (RECOMPILE)'
                    string sql = $@"        
                SELECT 
                    P.*, 
                    S.Nominativo AS NomeOperatore 
                FROM Carico P 
                LEFT JOIN utenti S 
                    ON P.matricola = S.matricola 
                WHERE P.nominativo LIKE @nominativo 
                {(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
                ORDER BY P.dataarrivo DESC;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        // Parametrizzazione sicura per il LIKE
                        cmd.Parameters.AddWithValue("@nominativo", $"%{nominativo}%");

                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }

                        // Utilizziamo l'helper FillTable per coerenza ed esecuzione sicura
                        DataTable tb = FillTable(cmd, out msg);

                        if (string.IsNullOrEmpty(msg))
                        {
                            msg = "OK";
                        }

                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// ricerca per indirizzo  in tabella pricipale
        /// </summary>
        /// <param name="indirizzo"></param>
        /// <returns></returns>
        public DataTable getListIndirizzo(string indirizzo, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {

                    //il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
                    string sqlCompleta = $@"        
SELECT 
    P.*, 
    S.Nominativo AS NomeOperatore FROM Carico P 
LEFT JOIN utenti S 
    ON P.matricola = S.matricola 
WHERE P.indirizzo LIKE @indirizzo 
	{(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
ORDER BY P.dataarrivo DESC;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                    {
                        cmd.Parameters.AddWithValue("@indirizzo", $"%{indirizzo}%");
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }
                    msg = "OK";
                    return tb;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// ricerca per data inserimento
        /// </summary>
        /// <param name="dataarrivo"></param>
        /// <returns></returns>
        public DataTable getListDataArrivo(string dataArrivoDa, string dataArrivoA, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            DateTime dtda = System.Convert.ToDateTime(dataArrivoDa);
            DateTime dta = System.Convert.ToDateTime(dataArrivoA);

            //string sql = "SELECT * FROM Principale where DataArrivo BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola " +
                " where EvasaData BETWEEN @dtda and @dta and P.uo = @uo order by dataarrivo desc";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@dtda", dtda);
                    cmd.Parameters.AddWithValue("@dta", dta);
                    cmd.Parameters.AddWithValue("@uo", uo);
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// ricerca per operatori   
        /// </summary>
        /// <param name="operatori"></param>
        /// <returns></returns>
        public DataTable getListOperatori(string operatori, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            string valoreCerca = operatori.Replace("'", "''").Replace("*", "%");


            string dbOrigine = string.Empty;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    //il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
                    string sqlCompleta = $@"        
SELECT 
    P.*, 
    S.Nominativo AS NomeOperatore 
FROM Carico P 
LEFT JOIN utenti S 
    ON P.matricola = S.matricola 
WHERE Operatori LIKE @valoreCerca 
   {(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}                    
ORDER BY P.dataarrivo DESC;";


                    //OR Operatori2 LIKE @valoreCerca
                    //   OR Operatori3 LIKE @valoreCerca) 

                    using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                    {
                        cmd.Parameters.AddWithValue("@valoreCerca", $"%{valoreCerca}%");
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }

                    msg = "OK";
                    return tb;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// ricerca per note sia in decretazione che principale, in questo modo si riesce a recuperare la pratica anche se la nota è stata inserita in fase di decretazione e non in fase di inserimento pratica
        /// </summary>
        /// <param name="note"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable getListByNote(string note, Boolean ckGeneralizzato, string uo)
        {
            DataTable tb = new DataTable();
            string valoreCerca = note.Replace("'", "''").Replace("*", "%");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {

                    string sql = "SELECT P.*, O.Nominativo AS NomeOperatore,P.NOTE, D.decr_nota  FROM carico P LEFT JOIN decretazione d ON P.id = d.decr_idPratica " +
                " LEFT JOIN utenti O ON P.matricola = O.matricola" +
                " WHERE (d.decr_nota LIKE '%" + valoreCerca + "%'  OR " +
                        "p.note LIKE '%" + valoreCerca + "%') " +
                        (!string.IsNullOrEmpty(uo) ? " AND P.uo = '" + uo + "'" : string.Empty) +
                        "ORDER BY dataarrivo DESC";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }
                }
            }
            // Sostituisci questo blocco nel metodo getListByNote
            catch (Exception ex)
            {
                // Gestione specifica per timeout SqlException
                if (ex is System.Data.SqlClient.SqlException sqlEx && sqlEx.Number == -2) // Codice errore specifico per Timeout
                {
                    msg = "Time out";
                }
                else
                {
                    msg = "Errore database: " + ex.Message;
                }
            }
            return tb;
        }

        public DataTable getListPratica(string nominativo, string indirizzo, string operatori, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(nominativo))
                sql = "SELECT * FROM Principale where nominativo like '%" + nominativo.Replace("'", "''") + "%'";
            if (!String.IsNullOrEmpty(operatori))
                sql = "SELECT * FROM Principale where operatori like '%" + operatori.Replace("'", "''") + "%'";
            if (!String.IsNullOrEmpty(indirizzo))
                sql = "SELECT * FROM Principale where indirizzo like '%" + indirizzo.Replace("'", "''") + "%'";
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// ricerca per pratica
        /// </summary>
        /// <param name="pratica"></param>
        /// <returns></returns>
        public DataTable getListPratica(string pratica, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                //il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}

                string sqlCompleta = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.nr_pratica = @pratica 
{(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY P.dataarrivo DESC;";

                using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                {
                    cmd.Parameters.AddWithValue("@pratica", pratica);
                    //      cmd.Parameters.AddWithValue("@nr_pratica", pratica);
                    if (!ckGeneralizzato)
                    {
                        cmd.Parameters.AddWithValue("@uo", uo);
                    }
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(tb);
                    }
                }
                msg = "OK";
                return tb;
            }
        }


        /// <summary>
        /// ricerca per BU
        /// </summary>
        /// <param name="bu"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable getListBu(string bu, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                // il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
                string sqlCompleta = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.bu = @bu 
{(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY P.dataarrivo DESC;";

                using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                {
                    cmd.Parameters.AddWithValue("@bu", bu);
                    if (!ckGeneralizzato)
                    {
                        cmd.Parameters.AddWithValue("@uo", uo);
                    }
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(tb);
                    }
                }

                msg = "OK";
                return tb;
            }
        }
        /// <summary>
        /// ricerca per codice edificio
        /// </summary>
        /// <param name="codEd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable getListCodEdificio(string codEd, Boolean ckGeneralizzato, string uo, out string msg)
        {
            DataTable tb = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    //il simbolo '$' all'inizio per interpolare la variabile {dbOrigine}
                    string sqlCompleta = $@"        
        SELECT 
            P.*, 
            S.Nominativo AS NomeOperatore 
        FROM Carico P 
        LEFT JOIN utenti S 
            ON P.matricola = S.matricola 
        WHERE P.CodiceEdificio = @codEd 
{(!ckGeneralizzato ? " AND P.uo = @uo" : string.Empty)}
        ORDER BY P.dataarrivo DESC;";
                    using (MySqlCommand cmd = new MySqlCommand(sqlCompleta, conn))
                    {
                        cmd.Parameters.AddWithValue("@codEd", codEd);
                        if (!ckGeneralizzato)
                        {
                            cmd.Parameters.AddWithValue("@uo", uo);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(tb);
                        }
                    }

                    msg = "OK";
                    return tb;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }

        }
        public DataTable getAutoBySigla(string sigla, string uo)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM parcoauto where sigla = @sigla";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sigla", sigla);

                    return tb = FillTable(cmd, out msg);
                }
            }
        }

        /// <summary>
        /// get auto sigla
        /// </summary>
        /// <returns></returns>
        public DataTable getSiglaAuto(string uo)
        {
            string sql = "SELECT * FROM parcoauto WHERE uo = @uo ORDER BY sigla";
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uo", uo); 
                
                    return tb = FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getListAuto(string Mese, int Anno, string Uo)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM GestioneAuto where mese =@mese and anno = @anno and uo = @uo order by sigla, data";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mese", Mese);
                    cmd.Parameters.AddWithValue("@anno", Anno);
                    cmd.Parameters.AddWithValue("@uo", Uo);
                    
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(tb);
                    }
                }
                return tb;
            }
        }
        /// <summary>
        /// elenca i file cancellabili
        /// </summary>
        /// <returns>datatable</returns>
        public DataTable GetFileScaricati()
        {

            string sql = "select ISNULL(folder, '') +  ISNULL(nomefile, '') AS percorso  from File_Caricati where cancella = 'True'";


            String testoSql = String.Empty;
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return tb = FillTable(cmd, out msg);
                }
            }
        }



        public DataTable getAttivitaAdmin(string area, bool val)
        {
            // Costruzione della query con parametri e formattazione leggibile
            string sql = $@"
        SELECT 
            a.Nr_Protocollo, 
            a.quartiere, 
            a.Macro_area, 
            MIN(b.decr_decretato) AS decr_decretato, 
            b.decr_chiuso, 
            a.id, 
            b.decr_dataChiusura, 
            a.anno, 
            a.nr_pratica 
        FROM principale AS a 
        INNER JOIN Decretazione AS b 
            ON a.Nr_Protocollo = b.decr_pratica 
        WHERE a.Macro_area = @area 
          AND b.decr_chiuso = @val 
          {(val ? "AND b.decr_dataChiusura <> '1900-01-01 00:00:00' AND b.decr_dataChiusura IS NOT NULL" : string.Empty)}
        GROUP BY 
            a.Nr_Protocollo, 
            a.quartiere, 
            a.Macro_area, 
            b.decr_chiuso, 
            a.id, 
            b.decr_dataChiusura, 
            a.anno, 
            a.nr_pratica 
        ORDER BY a.Nr_Protocollo ASC;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione sicura
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@val", val); // MySqlConnector gestisce bool convertendolo in 1/0 (TINYINT)

                    return FillTable(cmd, out msg);
                }
            }
        }

        public DataTable GetScadenziarioById(int id)
        {
            string sql = "SELECT * FROM ScadenziarioUrp WHERE id_scadenziario = @id";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione dell'ID
                    cmd.Parameters.AddWithValue("@id", id);

                    return FillTable(cmd, out msg);
                }
            }
        }

        public DataTable getAttivita(string area, bool val)
        {
            string sql = @"
        SELECT 
            a.Nr_Protocollo, 
            a.quartiere, 
            a.Macro_area, 
            MIN(b.decr_decretato) AS decr_decretato, 
            b.decr_chiuso, 
            a.id, 
            b.decr_dataChiusura 
        FROM principale AS a 
        INNER JOIN Decretazione AS b 
            ON a.Nr_Protocollo = b.decr_pratica 
        WHERE a.Macro_area = @area 
          AND b.decr_chiuso = @val 
        GROUP BY 
            a.Nr_Protocollo, 
            a.quartiere, 
            a.Macro_area, 
            b.decr_chiuso, 
            a.id, 
            b.decr_dataChiusura 
        ORDER BY a.Nr_Protocollo ASC;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione sicura
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@val", val); // Convertito automaticamente in 1/0 da Connector/NET

                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getPraticaCaricoDataSiglaId(Int32 carico, DateTime data, Int32 IdRecordOrigine)
        {
            DataTable tb = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();

                string sql = $@"
        SELECT * from  carico
        WHERE datainserimento = @dataInserimento 
          AND nr_Carico = @Carico and Id=@Id ";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@DataInserimento", data);
                    cmd.Parameters.AddWithValue("@Carico", carico);
                    //cmd.Parameters.AddWithValue("@Sigla", sigla);
                    cmd.Parameters.AddWithValue("@Id", IdRecordOrigine);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    return dt;
                }
            }

        }
        public DataTable getPraticaId(Int32 id)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where id = " + id;
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

                    return FillTable(cmd, out msg);
                }
            }
        }


        private DataTable FillTable(MySqlCommand cmd, out string msg)
        {
            DataTable table = new DataTable();
            msg = string.Empty;

            try
            {
                // Imposta il timeout se necessario (in secondi)
                cmd.CommandTimeout = 240;

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                // Garantisce la chiusura e il rilascio della connessione
                if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return table;
        }
        //INSERIMENTI



        /// <summary>
        /// isnserimento tabella giudice
        /// </summary>
        /// <param name="giudice"></param>
        /// <returns></returns>
        /// 
        public Boolean InserisciGiudice(string giudice)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into giudice (giudice)" +
                   " Values('" + @giudice.Replace("'", "''") + "')";


                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "giudice";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Giudice:" + giudice + ", " + ex.Message + @" - Errore in inserimento tabella giudice ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }

        private void LogMessage(string message)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch { /* Gestione silenziosa errori di log */ }
        }

        /// <summary>
        /// Inserisce una decretazione e aggiorna la tabella principale con il nuovo accertatore
        /// </summary>
        /// <param name="decr"></param>
        /// <returns></returns>
        public bool InsDecretazione(Decretazione decr)
        {
            bool resp = true;

            string sql = @"INSERT INTO decretazione (
                        decr_idPratica, 
                        decr_pratica, 
                        decr_decretante, 
                        decr_decretato, 
                        decr_data, 
                        decr_nota, 
                        decr_dataChiusura, 
                        decr_chiuso, 
                        decr_unire,
                        decr_uo
                   ) 
                   VALUES (
                        @idPratica, 
                        @pratica, 
                        @decretante, 
                        @decretato, 
                        @data, 
                        @nota, 
                        @dataChiusura, 
                        @chiuso, 
                        @unire,
                        @uo
                   );";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand(sql, conn))
                    {
                        // Parametrizzazione sicura
                        command.Parameters.AddWithValue("@idPratica", decr.idPratica);
                        command.Parameters.AddWithValue("@pratica", decr.Npratica ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@decretante", decr.decretante ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@decretato", decr.decretato ?? (object)DBNull.Value);

                        // Gestione Data (se null o minima imposta DBNull)
                        command.Parameters.AddWithValue("@data", decr.data != DateTime.MinValue ? decr.data : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nota", decr.nota ?? (object)DBNull.Value);

                        // dataChiusura impostata espressamente a NULL
                        command.Parameters.AddWithValue("@dataChiusura", DBNull.Value);

                        command.Parameters.AddWithValue("@chiuso", decr.chiuso);
                        command.Parameters.AddWithValue("@unire", decr.unire);

                        command.Parameters.AddWithValue("@uo", decr.uo ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestione e append del log di errore
                try
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine($"Decretazione:{decr.Npratica}, {ex.Message} - Errore in inserimento tabella decretazione");
                    }
                }
                catch
                {
                    // Evita che eventuali eccezioni di I/O nel logging blocchino la restituzione del flag
                }

                resp = false;
            }

            return resp;
        }

        public Boolean InsOperatore(Operatore op)
        {
            bool resp = true;
            string sql = @"INSERT INTO utenti (
                        matricola, 
                        passw, 
                        uo, 
                        ruolo, 
                        reset, 
                        pwstandard, 
                        nominativo,
                        grado,
                        abilitato
                   ) 
                   VALUES (
                        @matricola, 
                        @passw, 
                        @uo, 
                        @ruolo, 
                        @reset, 
                        @pwstandard, 
                        @nominativo, 
                        @grado,
                        @abilitato
                   );";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand(sql, conn))
                    {
                        // Parametrizzazione sicura
                        command.Parameters.AddWithValue("@matricola", op.matricola ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@passw", op.passw ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@uo", op.uo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ruolo", op.ruolo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@reset", op.reset);
                        command.Parameters.AddWithValue("@pwstandard", op.pwstandard);
                        command.Parameters.AddWithValue("@nominativo", op.nominativo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@abilitato", op.abilitato);
                        command.Parameters.AddWithValue("@grado", op.grado);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestione e append del log di errore
                try
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine($"Matricola:{op.matricola}, {ex.Message} - Errore in inserimento dati utente");
                    }
                }
                catch
                {
                    // Evita che errori di scrittura log blocchino l'eccezione principale
                }

                resp = false;
            }

            return resp;

        }


        /// <summary>
        /// Inserisce scheda nuova e alimenta la tabella statistiche
        /// </summary>
        /// <param name="rapp"></param>
        /// <param name="stat"></param>
        /// <param name="txt"></param>
        /// <param name="idN"></param>
        /// <returns></returns>
        //public Boolean InsRappUoteStatistiche(RappUote rapp, Statistiche stat, string txt, out Int32 idN)
        //{
        //    bool resp = true;
        //    string sql_insRap = String.Empty;
        //    string sql_Statistiche = String.Empty;

        //    using (SqlConnection conn = new SqlConnection(ConnString))
        //    {
        //        conn.Open();

        //        SqlTransaction transaction = null;
        //        SqlCommand command = conn.CreateCommand();

        //        transaction = conn.BeginTransaction("trans");
        //        command.Transaction = transaction;
        //        idN = -1;

        //        try
        //        {
        //            if (txt == "ins")
        //            {


        //                sql_Statistiche = "insert into statistiche (mese,anno,relazioni,ponteggi,dpi,esposti_ricevuti,esposti_evasi,ripristino_tot_par,controlli_scia,contr_cant_daily,cnr,annotazioni,notifiche" +
        //                    ",sequestri,riapp_sigilli,deleghe_ricevute,deleghe_esitate,cnr_annotazioni,interrogazioni,denunce_uff,convalide,demolizioni" +
        //                    ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti,viol_amm_reg_com,censimentoAllPubb" +
        //                    ",Abitativo,nonAbitativo,Sgomberi_abus,Sgomberi_immobili,NotificaTp) " +
        //                " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
        //                  stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
        //                  stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
        //                  stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
        //                  stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
        //                  stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + "," + stat.viol_amm_reg_com + "," +
        //                  stat.censimentoAllPubb + "," + stat.Abitativo + "," + stat.NonAbitativo + "," + stat.Sgomberi_abus + "," + stat.Sgomberi_immobili + "," + stat.NotificaTp + ")";

        //            }
        //            else
        //            {
        //                sql_Statistiche = "update statistiche set relazioni =  +" + @stat.relazioni + ",ponteggi = +" + @stat.ponteggi + ",dpi =+" + @stat.dpi + ",esposti_ricevuti=  +" + @stat.esposti_ricevuti +
        //                ",esposti_evasi =  +" + @stat.esposti_evasi + ",ripristino_tot_par =  +" + @stat.ripristino_tot_par + ",controlli_scia =  +" + @stat.controlli_scia +
        //                ",contr_cant_daily =  +" + @stat.contr_cant_daily + ",cnr = +" + stat.cnr + ", notifiche = +" + @stat.notifiche +
        //                ",annotazioni = +" + @stat.annotazioni + ",deleghe_esitate = +" + @stat.deleghe_esitate +
        //                ",sequestri =  +" + @stat.sequestri + ",riapp_sigilli = +" + @stat.riapp_sigilli + ",deleghe_ricevute =  +" + @stat.deleghe_ricevute +
        //                ",cnr_annotazioni =  +" + @stat.cnr_annotazioni + ",interrogazioni =  +" + @stat.interrogazioni + ",denunce_uff =  +" + @stat.denunce_uff + ",convalide = +" + @stat.convalide +
        //                ",demolizioni =  +" + @stat.demolizioni + ",violazione_sigilli =  +" + @stat.violazione_sigilli + ",dissequestri =  +" + @stat.dissequestri +
        //                ",dissequestri_temp =+" + @stat.dissequestri_temp + ",rimozione_sigilli =+" + @stat.rimozione_sigilli + ",controlli_42_04 =+" + @stat.controlli_42_04 +
        //                ",contr_cant_suolo_pubb =  +" + @stat.contr_cant_suolo_pubb + ",contr_lavori_edili =  +" + @stat.contr_lavori_edili + ",contr_cant =  +" + @stat.contr_cant +
        //                ",contr_nato_da_esposti =  +" + @stat.contr_nato_da_esposti +
        //                ", viol_amm_reg_com =+ " + stat.viol_amm_reg_com + ",censimentoAllPubb = +" + stat.censimentoAllPubb + ", Sgomberi_immobili = +" + stat.Sgomberi_immobili +
        //                ",Abitativo = +" + stat.Abitativo + ",nonAbitativo = +" + stat.NonAbitativo + ",Sgomberi_abus = +" + stat.Sgomberi_abus + ", NotificaTp =+" + stat.NotificaTp +
        //                " where mese = '" + @stat.mese + "' and anno = " + stat.anno;



        //            }

        //            sql_insRap = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
        //             "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
        //             "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
        //             "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
        //             "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
        //             "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento, " +
        //             "rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto," +
        //             "rapp_censimento_all_pubb,rapp_contr_occupazione_abus,rapp_contr_occ_abitativo,rapp_contr_occ_no_abitativo,rapp_sgomberi,rapp_sgomberi_abus,rapp_sgomberi_immobili,rapp_notifica_no_ag, " +
        //             "rapp_quartiere,rapp_num_censimento_all_pubb,rapp_numero_controlli_cant_seq,rapp_giro_cantieri,rapp_accRichiesti,rapp_numAccRichiesti,rapp_verbOccCensimento,rapp_contrNatoDaAcc,rapp_NumNotificheNoAg," +
        //             "rapp_NumcontrNatoDaAcc,rapp_NumAbusiAbitatSi,rapp_NumAbusiAbitatNo,rapp_NumAbusiNoAbitatSi,rapp_NumAbusiNoAbitatNo,rapp_print)" +
        //       " Values('" + rapp.pratica + "','" +
        //         //@rapp.ora + "','" +
        //         @rapp.data + "','" +
        //         @rapp.nominativo.Replace("'", "''") + "','" +
        //         @rapp.indirizzo.Replace("'", "''") + "','" +
        //         @rapp.pattuglia.Replace("'", "''") + "','" +
        //         @rapp.delegaAG + "','" +
        //         @rapp.resa + "','" +
        //         @rapp.segnalazione + "','" +
        //         @rapp.esposti + "','" +
        //         @rapp.num_esposti + "','" +
        //         @rapp.notifica + "','" +
        //         @rapp.iniziativa + "','" +
        //         @rapp.cdr + "','" +
        //         @rapp.coordinatore + "','" +
        //         @rapp.relazione + "','" +
        //         @rapp.cnr + "','" +
        //         @rapp.annotazionePG + "','" +
        //         @rapp.verbaleSeq + "','" +
        //         @rapp.esitoDelega + "','" +
        //         @rapp.contestazioneAmm + "','" +
        //         @rapp.convalida + "','" +
        //         @rapp.dissequestroDef + "','" +
        //         @rapp.dissequestroTemp + "','" +
        //         @rapp.rimozione + "','" +
        //         @rapp.riapposizione + "','" +
        //         @rapp.violazioneSigilli + "','" +
        //         @rapp.controlliScia + "','" +
        //         @rapp.accertAvvenutoRip + "','" +
        //         @rapp.totale + "','" +
        //         @rapp.parziale + "','" +
        //         @rapp.violazioneBeniCult + "','" +
        //         @rapp.contrCantSuoloPubb + "','" +
        //         @rapp.contrEdiliDPI + "','" +
        //         @rapp.contr_cantiereSeq + "','" +
        //         @rapp.contrDaEsposti + "','" +
        //         @rapp.contrDaSegn + "','" +
        //         @rapp.attività_interna + "','" +
        //         @rapp.nota.Replace("'", "''") + "','" +
        //         @rapp.data_consegna_intervento + "','" + @rapp.capopattuglia.Replace("'", "''") + "','" +
        //         @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" +
        //         @rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "','" +
        //         @rapp.censimento_all_pubb + "','" + @rapp.contr_occupazione_abus + "','" + @rapp.contr_occ_abitativo + "','" + @rapp.contr_occ_no_abitativo + "','" + @rapp.sgomberi + "','" +
        //         @rapp.sgomberi_abus + "','" + @rapp.sgomberi_immobili + "','" + @rapp.notifica_no_ag + "','" + @rapp.quartiere.Replace("'", "''") + "'," +
        //         @rapp.num_censimento_all_pubb + "," + @rapp.numero_controlli_cant_seq + ",'" + @rapp.giro_controlli +
        //         //I- mod 31/01/2026 scheda int
        //         "','" + @rapp.accRichiesti + "','" + @rapp.numAccRichiesti + "','" + @rapp.verbOccCensimento + "','" +
        //         @rapp.contrNatoDaAcc + "'," + @rapp.NumNotificheNoAg + ",'" + @rapp.NumcontrNatoDaAcc + "'," + @rapp.NumAbusiAbitatSi + "," + @rapp.NumAbusiAbitatNo + "," +
        //         @rapp.NumAbusiNoAbitatSi + "," + @rapp.NumAbusiNoAbitatNo + ",'" + @rapp.print + "' " +
        //         //F- mod 31/01/2026 scheda int
        //         "); SELECT SCOPE_IDENTITY();";
        //            command.CommandText = sql_insRap;
        //            object a = command.ExecuteScalar();


        //            command.CommandText = sql_Statistiche;
        //            command.ExecuteNonQuery();
        //            transaction.Commit();
        //            idN = Convert.ToInt32(a);
        //            resp = true;
        //        }

        //        catch (Exception ex)
        //        {
        //            if (transaction != null)
        //            {
        //                transaction.Rollback();

        //                if (!File.Exists(LogFile))
        //                {
        //                    using (StreamWriter sw = File.CreateText(LogFile)) { }
        //                }

        //                using (StreamWriter sw = File.AppendText(LogFile))
        //                {
        //                    sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
        //                    sw.Close();
        //                }
        //            }
        //            resp = false;


        //        }
        //        conn.Close();
        //        return resp;
        //    }

        //}

        public Boolean UpdRapportino(Rapportino rapp,string id_scheda, string uo)
        {
            //prima cancello e poi inserisco
            bool resp = true;
            if (rapp == null)
            {
                resp = false;

                return resp;
            }
            string sql_Ins = @"
    INSERT INTO rapportino (
        orario, dataconsegna, datainserimento, pattuglia, siglaVettura, 
        matricola, grado, itinerario, appiedata, attivitaInterna, nota, 
        ContrCds, PvContestatiCds, PvNoContestatiCds, OccupSuoloPub, PvContestatiOccupSuoloPub, 
        PvNoContestatiOccupSuoloPub, Autotrasporti, PvContestatiAutotrasporti, PvNoContestatiAutotrasporti, 
        TargheEstere, PvContestatiTargheEstere, PvNoContestatiTargheEstere, ParcheggiatoriAbusivi, 
        PvContestatiParcheggiatoriAbusivi, PvNoContestatiParcheggiatoriAbusivi, ContrAltreForze, 
        PvContestatiContrAltreForze, PvNoContestatiContrAltreForze, ContrMicroMobilita, 
        PvContestatiContrMicroMobilita, PvNoContestatiContrMicroMobilita, ConferimentoRifiuti, 
        SanzioniConferimentoRifiuti, DeiezioneCanina, SanzioniDeiezioneCanina, LeggeAntifumo, 
        SanzioniLeggeAntifumo, AreaMercatale, SanzioniAreaMercatale, InquinamentoAcustico, 
        SanzioniInquinamentoAcustico, ContrCantieriSeq, SanzioniContrCantieriSeq, LavoriSenzaProtez, 
        SanzioniLavoriSenzaProtez, NotificheSicurezza, SanzioniNotificheSicurezza, Ambulanti, 
        SanzioniAmbulanti, ProvenienzaAlimentare, SanzioniProvenienzaAlimentare, TrasportoAlimentare, 
        SanzioniTrasportoAlimentare, Officine, SanzioniOfficine, ContrBnB, SanzioniContrBnB, print, uo
    ) VALUES (
        @orario, @data_consegna_intervento, @datainserimento, @pattuglia, @siglaVettura, 
        @matricola, @grado, @itinerario, @appiedata, @attivitaInterna, @nota, 
        @ContrCds, @PvContestatiCds, @PvNoContestatiCds, @OccupSuoloPub, @PvContestatiOccupSuoloPub, 
        @PvNoContestatiOccupSuoloPub, @Autotrasporti, @PvContestatiAutotrasporti, @PvNoContestatiAutotrasporti, 
        @TargheEstere, @PvContestatiTargheEstere, @PvNoContestatiTargheEstere, @ParcheggiatoriAbusivi, 
        @PvContestatiParcheggiatoriAbusivi, @PvNoContestatiParcheggiatoriAbusivi, @ContrAltreForze, 
        @PvContestatiContrAltreForze, @PvNoContestatiContrAltreForze, @ContrMicroMobilita, 
        @PvContestatiContrMicroMobilita, @PvNoContestatiContrMicroMobilita, @ConferimentoRifiuti, 
        @SanzioniConferimentoRifiuti, @DeiezioneCanina, @SanzioniDeiezioneCanina, @LeggeAntifumo, 
        @SanzioniLeggeAntifumo, @AreaMercatale, @SanzioniAreaMercatale, @InquinamentoAcustico, 
        @SanzioniInquinamentoAcustico, @ContrCantieriSeq, @SanzioniContrCantieriSeq, @LavoriSenzaProtez, 
        @SanzioniLavoriSenzaProtez, @NotificheSicurezza, @SanzioniNotificheSicurezza, @Ambulanti, 
        @SanzioniAmbulanti, @ProvenienzaAlimentare, @SanzioniProvenienzaAlimentare, @TrasportoAlimentare, 
        @SanzioniTrasportoAlimentare, @Officine, @SanzioniOfficine, @ContrBnB, @SanzioniContrBnB, @print,@uo
    );";

            string sql_Del = @"delete from rapportino where id=@id_scheda and uo=@uo";


            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql_Del, conn, tran))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.AddWithValue("@id_scheda", id_scheda);
                        cmd.Parameters.AddWithValue("@uo", uo);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"Errore in eliminazione scheda rapportino id:{id_scheda}, uo:{uo} - {ex.Message}");
                            tran.Rollback();
                            return false;
                        }
                    }
                        using (MySqlCommand cmd = new MySqlCommand(sql_Ins, conn, tran))
                    {
                        
                        try
                        {
                            // Stringhe (con gestione null/string.Empty)
                            cmd.Parameters.AddWithValue("@orario", rapp.orario ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@data_consegna_intervento", rapp.data_consegna_intervento == DateTime.MinValue ? (object)DBNull.Value : rapp.data_consegna_intervento);
                            cmd.Parameters.AddWithValue("@datainserimento", rapp.dataInserimento == DateTime.MinValue ? DateTime.Now : rapp.dataInserimento);
                            cmd.Parameters.AddWithValue("@pattuglia", rapp.pattuglia ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@siglaVettura", rapp.siglaVettura ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@matricola", rapp.matricola ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@grado", rapp.grado ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@itinerario", rapp.itinerario ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@nota", rapp.nota ?? (object)DBNull.Value);

                            // Boolean
                            cmd.Parameters.AddWithValue("@appiedata", rapp.appiedata);
                            cmd.Parameters.AddWithValue("@attivitaInterna", rapp.attivitaInterna);
                            cmd.Parameters.AddWithValue("@print", rapp.print);

                            // Interi
                            cmd.Parameters.AddWithValue("@ContrCds", rapp.ContrCds);
                            cmd.Parameters.AddWithValue("@PvContestatiCds", rapp.PvContestatiCds);
                            cmd.Parameters.AddWithValue("@PvNoContestatiCds", rapp.PvNoContestatiCds);
                            cmd.Parameters.AddWithValue("@OccupSuoloPub", rapp.OccupSuoloPub);
                            cmd.Parameters.AddWithValue("@PvContestatiOccupSuoloPub", rapp.PvContestatiOccupSuoloPub);
                            cmd.Parameters.AddWithValue("@PvNoContestatiOccupSuoloPub", rapp.PvNoContestatiOccupSuoloPub);
                            cmd.Parameters.AddWithValue("@Autotrasporti", rapp.Autotrasporti);
                            cmd.Parameters.AddWithValue("@PvContestatiAutotrasporti", rapp.PvContestatiAutotrasporti);
                            cmd.Parameters.AddWithValue("@PvNoContestatiAutotrasporti", rapp.PvNoContestatiAutotrasporti);
                            cmd.Parameters.AddWithValue("@TargheEstere", rapp.TargheEstere);
                            cmd.Parameters.AddWithValue("@PvContestatiTargheEstere", rapp.PvContestatiTargheEstere);
                            cmd.Parameters.AddWithValue("@PvNoContestatiTargheEstere", rapp.PvNoContestatiTargheEstere);
                            cmd.Parameters.AddWithValue("@ParcheggiatoriAbusivi", rapp.ParcheggiatoriAbusivi);
                            cmd.Parameters.AddWithValue("@PvContestatiParcheggiatoriAbusivi", rapp.PvContestatiParcheggiatoriAbusivi);
                            cmd.Parameters.AddWithValue("@PvNoContestatiParcheggiatoriAbusivi", rapp.PvNoContestatiParcheggiatoriAbusivi);
                            cmd.Parameters.AddWithValue("@ContrAltreForze", rapp.ContrAltreForze);
                            cmd.Parameters.AddWithValue("@PvContestatiContrAltreForze", rapp.PvContestatiContrAltreForze);
                            cmd.Parameters.AddWithValue("@PvNoContestatiContrAltreForze", rapp.PvNoContestatiContrAltreForze);
                            cmd.Parameters.AddWithValue("@ContrMicroMobilita", rapp.ContrMicroMobilita);
                            cmd.Parameters.AddWithValue("@PvContestatiContrMicroMobilita", rapp.PvContestatiContrMicroMobilita);
                            cmd.Parameters.AddWithValue("@PvNoContestatiContrMicroMobilita", rapp.PvNoContestatiContrMicroMobilita);
                            cmd.Parameters.AddWithValue("@ConferimentoRifiuti", rapp.ConferimentoRifiuti);
                            cmd.Parameters.AddWithValue("@SanzioniConferimentoRifiuti", rapp.SanzioniConferimentoRifiuti);
                            cmd.Parameters.AddWithValue("@DeiezioneCanina", rapp.DeiezioneCanina);
                            cmd.Parameters.AddWithValue("@SanzioniDeiezioneCanina", rapp.SanzioniDeiezioneCanina);
                            cmd.Parameters.AddWithValue("@LeggeAntifumo", rapp.LeggeAntifumo);
                            cmd.Parameters.AddWithValue("@SanzioniLeggeAntifumo", rapp.SanzioniLeggeAntifumo);
                            cmd.Parameters.AddWithValue("@AreaMercatale", rapp.AreaMercatale);
                            cmd.Parameters.AddWithValue("@SanzioniAreaMercatale", rapp.SanzioniAreaMercatale);
                            cmd.Parameters.AddWithValue("@InquinamentoAcustico", rapp.InquinamentoAcustico);
                            cmd.Parameters.AddWithValue("@SanzioniInquinamentoAcustico", rapp.SanzioniInquinamentoAcustico);
                            cmd.Parameters.AddWithValue("@ContrCantieriSeq", rapp.ContrCantieriSeq);
                            cmd.Parameters.AddWithValue("@SanzioniContrCantieriSeq", rapp.SanzioniContrCantieriSeq);
                            cmd.Parameters.AddWithValue("@LavoriSenzaProtez", rapp.LavoriSenzaProtez);
                            cmd.Parameters.AddWithValue("@SanzioniLavoriSenzaProtez", rapp.SanzioniLavoriSenzaProtez);
                            cmd.Parameters.AddWithValue("@NotificheSicurezza", rapp.NotificheSicurezza);
                            cmd.Parameters.AddWithValue("@SanzioniNotificheSicurezza", rapp.SanzioniNotificheSicurezza);
                            cmd.Parameters.AddWithValue("@Ambulanti", rapp.Ambulanti);
                            cmd.Parameters.AddWithValue("@SanzioniAmbulanti", rapp.SanzioniAmbulanti);
                            cmd.Parameters.AddWithValue("@ProvenienzaAlimentare", rapp.ProvenienzaAlimentare);
                            cmd.Parameters.AddWithValue("@SanzioniProvenienzaAlimentare", rapp.SanzioniProvenienzaAlimentare);
                            cmd.Parameters.AddWithValue("@TrasportoAlimentare", rapp.TrasportoAlimentare);
                            cmd.Parameters.AddWithValue("@SanzioniTrasportoAlimentare", rapp.SanzioniTrasportoAlimentare);
                            cmd.Parameters.AddWithValue("@Officine", rapp.Officine);
                            cmd.Parameters.AddWithValue("@SanzioniOfficine", rapp.SanzioniOfficine);
                            cmd.Parameters.AddWithValue("@ContrBnB", rapp.ContrBnB);
                            cmd.Parameters.AddWithValue("@SanzioniContrBnB", rapp.SanzioniContrBnB);
                            cmd.Parameters.AddWithValue("@uo", rapp.uo); 
                            cmd.ExecuteNonQuery();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            LogMessage($"Errore in update rapportino id:{id_scheda}, uo:{uo} - {ex.Message}");
                            return false;
                        }
                    }
                }
            }
           
            return resp;
            
        }


        public Boolean InsRapportino(Rapportino rapp, out Int32 idN)
        {
            idN = -1;
            bool resp = true;
            if (rapp == null)
            {
                resp = false;
                idN = -2;
                return resp;
            }
            string sql_Del = @"delete from rapportino where id=@id_scheda and uo=@uo";

            string sql_Ins = @"
    INSERT INTO rapportino (
        orario, dataconsegna, datainserimento, pattuglia, siglaVettura, 
        matricola, grado, itinerario, appiedata, attivitaInterna, nota, 
        ContrCds, PvContestatiCds, PvNoContestatiCds, OccupSuoloPub, PvContestatiOccupSuoloPub, 
        PvNoContestatiOccupSuoloPub, Autotrasporti, PvContestatiAutotrasporti, PvNoContestatiAutotrasporti, 
        TargheEstere, PvContestatiTargheEstere, PvNoContestatiTargheEstere, ParcheggiatoriAbusivi, 
        PvContestatiParcheggiatoriAbusivi, PvNoContestatiParcheggiatoriAbusivi, ContrAltreForze, 
        PvContestatiContrAltreForze, PvNoContestatiContrAltreForze, ContrMicroMobilita, 
        PvContestatiContrMicroMobilita, PvNoContestatiContrMicroMobilita, ConferimentoRifiuti, 
        SanzioniConferimentoRifiuti, DeiezioneCanina, SanzioniDeiezioneCanina, LeggeAntifumo, 
        SanzioniLeggeAntifumo, AreaMercatale, SanzioniAreaMercatale, InquinamentoAcustico, 
        SanzioniInquinamentoAcustico, ContrCantieriSeq, SanzioniContrCantieriSeq, LavoriSenzaProtez, 
        SanzioniLavoriSenzaProtez, NotificheSicurezza, SanzioniNotificheSicurezza, Ambulanti, 
        SanzioniAmbulanti, ProvenienzaAlimentare, SanzioniProvenienzaAlimentare, TrasportoAlimentare, 
        SanzioniTrasportoAlimentare, Officine, SanzioniOfficine, ContrBnB, SanzioniContrBnB, print, uo
    ) VALUES (
        @orario, @data_consegna_intervento, @datainserimento, @pattuglia, @siglaVettura, 
        @matricola, @grado, @itinerario, @appiedata, @attivitaInterna, @nota, 
        @ContrCds, @PvContestatiCds, @PvNoContestatiCds, @OccupSuoloPub, @PvContestatiOccupSuoloPub, 
        @PvNoContestatiOccupSuoloPub, @Autotrasporti, @PvContestatiAutotrasporti, @PvNoContestatiAutotrasporti, 
        @TargheEstere, @PvContestatiTargheEstere, @PvNoContestatiTargheEstere, @ParcheggiatoriAbusivi, 
        @PvContestatiParcheggiatoriAbusivi, @PvNoContestatiParcheggiatoriAbusivi, @ContrAltreForze, 
        @PvContestatiContrAltreForze, @PvNoContestatiContrAltreForze, @ContrMicroMobilita, 
        @PvContestatiContrMicroMobilita, @PvNoContestatiContrMicroMobilita, @ConferimentoRifiuti, 
        @SanzioniConferimentoRifiuti, @DeiezioneCanina, @SanzioniDeiezioneCanina, @LeggeAntifumo, 
        @SanzioniLeggeAntifumo, @AreaMercatale, @SanzioniAreaMercatale, @InquinamentoAcustico, 
        @SanzioniInquinamentoAcustico, @ContrCantieriSeq, @SanzioniContrCantieriSeq, @LavoriSenzaProtez, 
        @SanzioniLavoriSenzaProtez, @NotificheSicurezza, @SanzioniNotificheSicurezza, @Ambulanti, 
        @SanzioniAmbulanti, @ProvenienzaAlimentare, @SanzioniProvenienzaAlimentare, @TrasportoAlimentare, 
        @SanzioniTrasportoAlimentare, @Officine, @SanzioniOfficine, @ContrBnB, @SanzioniContrBnB, @print,@uo
    );";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql_Ins, conn))
                {
                    // Stringhe (con gestione null/string.Empty)
                    cmd.Parameters.AddWithValue("@orario", rapp.orario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@data_consegna_intervento", rapp.data_consegna_intervento == DateTime.MinValue ? (object)DBNull.Value : rapp.data_consegna_intervento);
                    cmd.Parameters.AddWithValue("@datainserimento", rapp.dataInserimento == DateTime.MinValue ? DateTime.Now : rapp.dataInserimento);
                    cmd.Parameters.AddWithValue("@pattuglia", rapp.pattuglia ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@siglaVettura", rapp.siglaVettura ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@matricola", rapp.matricola ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@grado", rapp.grado ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@itinerario", rapp.itinerario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nota", rapp.nota ?? (object)DBNull.Value);

                    // Boolean
                    cmd.Parameters.AddWithValue("@appiedata", rapp.appiedata);
                    cmd.Parameters.AddWithValue("@attivitaInterna", rapp.attivitaInterna);
                    cmd.Parameters.AddWithValue("@print", rapp.print);

                    // Interi
                    cmd.Parameters.AddWithValue("@ContrCds", rapp.ContrCds);
                    cmd.Parameters.AddWithValue("@PvContestatiCds", rapp.PvContestatiCds);
                    cmd.Parameters.AddWithValue("@PvNoContestatiCds", rapp.PvNoContestatiCds);
                    cmd.Parameters.AddWithValue("@OccupSuoloPub", rapp.OccupSuoloPub);
                    cmd.Parameters.AddWithValue("@PvContestatiOccupSuoloPub", rapp.PvContestatiOccupSuoloPub);
                    cmd.Parameters.AddWithValue("@PvNoContestatiOccupSuoloPub", rapp.PvNoContestatiOccupSuoloPub);
                    cmd.Parameters.AddWithValue("@Autotrasporti", rapp.Autotrasporti);
                    cmd.Parameters.AddWithValue("@PvContestatiAutotrasporti", rapp.PvContestatiAutotrasporti);
                    cmd.Parameters.AddWithValue("@PvNoContestatiAutotrasporti", rapp.PvNoContestatiAutotrasporti);
                    cmd.Parameters.AddWithValue("@TargheEstere", rapp.TargheEstere);
                    cmd.Parameters.AddWithValue("@PvContestatiTargheEstere", rapp.PvContestatiTargheEstere);
                    cmd.Parameters.AddWithValue("@PvNoContestatiTargheEstere", rapp.PvNoContestatiTargheEstere);
                    cmd.Parameters.AddWithValue("@ParcheggiatoriAbusivi", rapp.ParcheggiatoriAbusivi);
                    cmd.Parameters.AddWithValue("@PvContestatiParcheggiatoriAbusivi", rapp.PvContestatiParcheggiatoriAbusivi);
                    cmd.Parameters.AddWithValue("@PvNoContestatiParcheggiatoriAbusivi", rapp.PvNoContestatiParcheggiatoriAbusivi);
                    cmd.Parameters.AddWithValue("@ContrAltreForze", rapp.ContrAltreForze);
                    cmd.Parameters.AddWithValue("@PvContestatiContrAltreForze", rapp.PvContestatiContrAltreForze);
                    cmd.Parameters.AddWithValue("@PvNoContestatiContrAltreForze", rapp.PvNoContestatiContrAltreForze);
                    cmd.Parameters.AddWithValue("@ContrMicroMobilita", rapp.ContrMicroMobilita);
                    cmd.Parameters.AddWithValue("@PvContestatiContrMicroMobilita", rapp.PvContestatiContrMicroMobilita);
                    cmd.Parameters.AddWithValue("@PvNoContestatiContrMicroMobilita", rapp.PvNoContestatiContrMicroMobilita);
                    cmd.Parameters.AddWithValue("@ConferimentoRifiuti", rapp.ConferimentoRifiuti);
                    cmd.Parameters.AddWithValue("@SanzioniConferimentoRifiuti", rapp.SanzioniConferimentoRifiuti);
                    cmd.Parameters.AddWithValue("@DeiezioneCanina", rapp.DeiezioneCanina);
                    cmd.Parameters.AddWithValue("@SanzioniDeiezioneCanina", rapp.SanzioniDeiezioneCanina);
                    cmd.Parameters.AddWithValue("@LeggeAntifumo", rapp.LeggeAntifumo);
                    cmd.Parameters.AddWithValue("@SanzioniLeggeAntifumo", rapp.SanzioniLeggeAntifumo);
                    cmd.Parameters.AddWithValue("@AreaMercatale", rapp.AreaMercatale);
                    cmd.Parameters.AddWithValue("@SanzioniAreaMercatale", rapp.SanzioniAreaMercatale);
                    cmd.Parameters.AddWithValue("@InquinamentoAcustico", rapp.InquinamentoAcustico);
                    cmd.Parameters.AddWithValue("@SanzioniInquinamentoAcustico", rapp.SanzioniInquinamentoAcustico);
                    cmd.Parameters.AddWithValue("@ContrCantieriSeq", rapp.ContrCantieriSeq);
                    cmd.Parameters.AddWithValue("@SanzioniContrCantieriSeq", rapp.SanzioniContrCantieriSeq);
                    cmd.Parameters.AddWithValue("@LavoriSenzaProtez", rapp.LavoriSenzaProtez);
                    cmd.Parameters.AddWithValue("@SanzioniLavoriSenzaProtez", rapp.SanzioniLavoriSenzaProtez);
                    cmd.Parameters.AddWithValue("@NotificheSicurezza", rapp.NotificheSicurezza);
                    cmd.Parameters.AddWithValue("@SanzioniNotificheSicurezza", rapp.SanzioniNotificheSicurezza);
                    cmd.Parameters.AddWithValue("@Ambulanti", rapp.Ambulanti);
                    cmd.Parameters.AddWithValue("@SanzioniAmbulanti", rapp.SanzioniAmbulanti);
                    cmd.Parameters.AddWithValue("@ProvenienzaAlimentare", rapp.ProvenienzaAlimentare);
                    cmd.Parameters.AddWithValue("@SanzioniProvenienzaAlimentare", rapp.SanzioniProvenienzaAlimentare);
                    cmd.Parameters.AddWithValue("@TrasportoAlimentare", rapp.TrasportoAlimentare);
                    cmd.Parameters.AddWithValue("@SanzioniTrasportoAlimentare", rapp.SanzioniTrasportoAlimentare);
                    cmd.Parameters.AddWithValue("@Officine", rapp.Officine);
                    cmd.Parameters.AddWithValue("@SanzioniOfficine", rapp.SanzioniOfficine);
                    cmd.Parameters.AddWithValue("@ContrBnB", rapp.ContrBnB);
                    cmd.Parameters.AddWithValue("@SanzioniContrBnB", rapp.SanzioniContrBnB);
                    cmd.Parameters.AddWithValue("@uo", rapp.uo);
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"Matricola:{rapp.matricola}, data:{rapp.dataInserimento}: {ex.Message} - Errore in Insert Rapportino");
                        }

                        return resp;
                    }
                }
            }

        }

        //FINE INSERIMENTO
        public DataTable GetSchedeInfo(string boxChiamante, string mese, string anno)
        {
            // Trasforma la stringa del mese nel formato numerico (es. "1", "01", ecc.)
            string meseS = GetNumeroMeseByText(mese);

            // Mappatura della condizione specifica per ciascun filtro
            string condizioneFiltro = string.Empty;

            switch (boxChiamante)
            {
                case "EspostiEvasi":
                    condizioneFiltro = "rapp_esposto = 1";
                    break;
                case "Relazioni":
                    condizioneFiltro = "rapp_relazione = 1";
                    break;
                case "Ponteggi":
                    condizioneFiltro = "rapp_contr_cantiere_suolo_pubb = 1";
                    break;
                case "DPI":
                    condizioneFiltro = "rapp_contr_lavori_edili = 1";
                    break;
                case "SCIA":
                    condizioneFiltro = "rapp_controlliScia = 1";
                    break;
                case "Annotazioni":
                    condizioneFiltro = "rapp_annotazionePG = 1";
                    break;
                case "Notifiche":
                    condizioneFiltro = "rapp_notifica = 1";
                    break;
                case "NotificheNoAG":
                    condizioneFiltro = "rapp_notifica_no_ag = 1";
                    break;
                case "Sequestri":
                    condizioneFiltro = "rapp_verbale_seq = 1";
                    break;
                case "RiappSigilli":
                    condizioneFiltro = "rapp_disseq_temp_Riapp = 1";
                    break;
                case "DelegheEsitate":
                    condizioneFiltro = "rapp_esito_delega = 1";
                    break;
                case "Convalide":
                    condizioneFiltro = "rapp_convalida = 1";
                    break;
                case "ViolazioneSigilli":
                    condizioneFiltro = "rapp_violazione_sigilli = 1";
                    break;
                case "DissequestriTemp":
                    condizioneFiltro = "rapp_disseq_temp = 1";
                    break;
                case "Dissequestri":
                    condizioneFiltro = "rapp_disseq_def = 1";
                    break;
                case "RimozioneSigilli":
                    condizioneFiltro = "rapp_disseq_temp_Rim = 1";
                    break;
                case "ControlliDLGS":
                    condizioneFiltro = "rapp_violazioneBeniCult = 1";
                    break;
                case "ControlliCant":
                    condizioneFiltro = "(rapp_contr_cantieri_seq = 1 OR rapp_giro_cantieri = 1)";
                    break;
                case "ViolAmmRegCom":
                    condizioneFiltro = "rapp_contestaz_amm = 1";
                    break;
                case "CensimentoAllPubb":
                    condizioneFiltro = "rapp_censimento_all_pubb = 1";
                    break;
                case "OccupAbusivaAbit":
                    condizioneFiltro = "rapp_contr_occ_abitativo = 1";
                    break;
                case "OccupAbusivaNoAbit":
                    condizioneFiltro = "rapp_contr_occ_no_abitativo = 1";
                    break;
                case "SgomberiAbus":
                    condizioneFiltro = "txtSgomberiAbus = 1";
                    break;
                case "SgomberiImmobili":
                    condizioneFiltro = "rapp_sgomberi_immobili = 1";
                    break;
                case "AccertAltriEnti":
                    condizioneFiltro = "rapp_contrNatoDaAcc = 1";
                    break;
                case "CNR":
                    condizioneFiltro = "rapp_cnr = 1";
                    break;
                default:
                    // Se il valore di boxChiamante non corrisponde a nessun caso noto
                    return new DataTable();
            }

            // Struttura base della query SQL per MySQL
            string sql = $@"
        SELECT * 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND {condizioneFiltro};";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Conversione sicura dei parametri numerici/stringa
                    cmd.Parameters.AddWithValue("@anno", Convert.ToInt32(anno));
                    cmd.Parameters.AddWithValue("@mese", Convert.ToInt32(meseS));

                    string msg = string.Empty;
                    return FillTable(cmd, out msg);
                }
            }
        }
        public DataTable getObiettivi(int anno)
        {
            string sql = @"SELECT 
                        impalcature, 
                        dpi, 
                        contcantseq, 
                        contr_esposti, 
                        cens_allogg_pubb, 
                        occ_prop_com_abit, 
                        occ_prop_com_no_abit, 
                        contr_nati_da_accer_richiesti 
                   FROM obiettivi 
                   WHERE anno = @anno";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione sicura per l'anno
                    cmd.Parameters.AddWithValue("@anno", anno);

                    string msg = string.Empty;
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// preleva le statistiche per mese e anno
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable GetStatistiche(string mese, int anno)
        {
            string sql = "SELECT * FROM statistiche WHERE mese = @mese AND anno = @anno";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione per prevenire SQL Injection ed errori di tipo
                    cmd.Parameters.AddWithValue("@mese", mese ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@anno", anno);

                    string msg = string.Empty;
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// calcola statistiche annuali
        /// </summary>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable GetStatisticheAnnuali(int anno)
        {
            // Sintassi nativa MySQL per funzioni temporali, aggregazioni e cast
            string sql = @"
        SELECT 
            MONTHNAME(rapp_data_consegna_intervento) AS Mese,
            SUM(CASE WHEN rapp_contr_cantiere_suolo_pubb = 1 THEN 1 ELSE 0 END) AS rapp_contr_cantiere_suolo_pubb,
            SUM(CASE WHEN rapp_contr_lavori_edili = 1 THEN 1 ELSE 0 END) AS rapp_contr_lavori_edili,
            SUM(
                (CASE WHEN rapp_contr_cantieri_seq = 1 THEN 1 ELSE 0 END) + 
                (CASE WHEN rapp_giro_cantieri = 1 THEN CAST(rapp_numero_controlli_cant_seq AS SIGNED) ELSE 0 END)
            ) AS rapp_contr_cantieri_seq,
            SUM(CASE WHEN rapp_numEsposti REGEXP '^[0-9]+$' THEN CAST(rapp_numEsposti AS DECIMAL(18, 0)) ELSE 0 END) AS rapp_numEsposti,
            SUM(CASE WHEN rapp_censimento_all_pubb = 1 THEN CAST(rapp_num_censimento_all_pubb AS SIGNED) ELSE 0 END) AS rapp_censimento_all_pubb,
            SUM(CASE WHEN rapp_contr_occ_abitativo = 1 THEN 1 ELSE 0 END) AS rapp_contr_occ_abitativo,
            SUM(CASE WHEN rapp_contr_occ_no_abitativo = 1 THEN 1 ELSE 0 END) AS rapp_contr_occ_no_abitativo,
            SUM(CASE WHEN rapp_NumcontrNatoDaAcc REGEXP '^[0-9]+$' THEN CAST(rapp_NumcontrNatoDaAcc AS DECIMAL(18, 0)) ELSE 0 END) AS rapp_NumcontrNatoDaAcc
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno
        GROUP BY 
            MONTHNAME(rapp_data_consegna_intervento), 
            MONTH(rapp_data_consegna_intervento)
        ORDER BY 
            MONTH(rapp_data_consegna_intervento) ASC;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parametrizzazione dell'anno
                    cmd.Parameters.AddWithValue("@anno", anno);

                    string msg = string.Empty;
                    return FillTable(cmd, out msg);
                }
            }
        }
        /// <summary>
        /// preleva il numero di relazioni redatte
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumRelazione(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            // Query MySQL ottimizzata con parametri temporali e filtro booleano
            string sql = @"
        SELECT COUNT(rapp_relazione) 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND rapp_relazione = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati");
                        }

                        return "0";
                    }
                }
            }
        }
        /// <summary>
        /// prteleva il numero di annotazioni fatte
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumAnnotazioni(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            // Query MySQL parametrizzata e ottimizzata
            string sql = @"
        SELECT COUNT(rapp_annotazionePG) 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND rapp_annotazionePG = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati");
                        }

                        return "0";
                    }
                }
            }
        }
        /// <summary>
        /// preleva il numero di notifiche fatte
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumNotifiche(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            string sql = @"
        SELECT COUNT(rapp_notifica) 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND rapp_notifica = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati (GetNumNotifiche)");
                        }

                        return "0";
                    }
                }
            }
        }

        public string GetNumInterrogatoriByMeseAnno(string mese, int anno)
        {
            // MySQL supporta IFNULL/COALESCE e SUM con espressioni booleane
            string sql = @"
        SELECT COALESCE(SUM(
            (CASE WHEN Nominativo1 IS NOT NULL AND TRIM(Nominativo1) <> '' THEN 1 ELSE 0 END) +
            (CASE WHEN Nominativo2 IS NOT NULL AND TRIM(Nominativo2) <> '' THEN 1 ELSE 0 END) +
            (CASE WHEN Nominativo3 IS NOT NULL AND TRIM(Nominativo3) <> '' THEN 1 ELSE 0 END) +
            (CASE WHEN Nominativo4 IS NOT NULL AND TRIM(Nominativo4) <> '' THEN 1 ELSE 0 END)
        ), 0) AS TotaleComplessivoNominativi 
        FROM Interrogatori 
        WHERE mese = @mese AND anno = @anno;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@mese", mese ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@anno", anno);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati (GetNumInterrogatoriByMeseAnno)");
                        }
                        return "0";
                    }
                }
            }
        }

        /// <summary>
        /// Preleva il numero di deleghe esitate
        /// </summary>
        public string GetNumDelegheEsitate(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            string sql = @"
        SELECT COUNT(rapp_esito_delega) 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND rapp_esito_delega = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati (GetNumDelegheEsitate)");
                        }
                        return "0";
                    }
                }
            }
        }
        /// <summary>
        /// prelevo numero esposti evasi
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetEspostiEvasi(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            // Gestione sicura del cast senza TRY_CAST e filtri temporali nativi MySQL
            string sql = @"
        SELECT COALESCE(
            SUM(
                CASE 
                    WHEN rapp_numEsposti REGEXP '^[0-9]+$' THEN CAST(rapp_numEsposti AS DECIMAL(18, 0)) 
                    ELSE 0 
                END
            ), 0) AS SommaTotale 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati (GetEspostiEvasi)");
                        }
                        return "0";
                    }
                }
            }
        }
        /// <summary>
        /// preleva il numero di ponteggi controllati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumPonteggi(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            // Query MySQL con parametri e filtri temporali nativi
            string sql = @"
        SELECT COUNT(rapp_contr_cantiere_suolo_pubb) 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND rapp_contr_cantiere_suolo_pubb = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                            sw.Close();
                        }

                    }

                    return "0";
                }
            }
        }
        /// <summary>
        /// prelevo num vilazioni amministrative
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumViolAmm(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            // Query MySQL parametrizzata con funzioni temporali native e flag booleano numerico
            string sql = @"
    SELECT COUNT(rapp_contestaz_amm) 
    FROM rappuote 
    WHERE YEAR(rapp_data_consegna_intervento) = @anno 
      AND MONTH(rapp_data_consegna_intervento) = @mese 
      AND rapp_contestaz_amm = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati");
                            sw.Close();
                        }
                        return "0";
                    }
                }
            }

        }
        /// <summary>
        /// preleva il numero di ripristini effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumRipristino(string mese, int anno)
        {
            string meseS = GetNumeroMeseByText(mese);
            int meseNum = Convert.ToInt32(meseS);

            // Query MySQL parametrizzata con funzioni temporali native
            string sql = @"
        SELECT COUNT(rapp_accert_avvenuto) 
        FROM rappuote 
        WHERE YEAR(rapp_data_consegna_intervento) = @anno 
          AND MONTH(rapp_data_consegna_intervento) = @mese 
          AND rapp_accert_avvenuto = 1;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@anno", anno);
                    command.Parameters.AddWithValue("@mese", meseNum);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"mese {mese}, anno: {anno} - {ex.Message} - Errore in lettura dati (GetNumRipristino)");
                            sw.Close();
                        }
                        return "0";
                    }
                }
            }
        }

        /// <summary>
        /// trasforma il mese in numero
        /// </summary>
        /// <param name="mese"></param>
        /// <returns></returns>
        public string GetNumeroMeseByText(string mese)
        {
            if (string.IsNullOrWhiteSpace(mese))
                return "01";

            try
            {
                // Converte il nome del mese (in italiano) nel corrispondente numero a due cifre (es. "Gennaio" -> "01")
                int meseNum = DateTime.ParseExact(mese.Trim(), "MMMM", new CultureInfo("it-IT")).Month;
                return meseNum.ToString("D2");
            }
            catch
            {
                // Gestione di fallback nel caso in cui il mese sia già numerico o in formato abbreviato
                if (DateTime.TryParseExact(mese.Trim(), "MMM", new CultureInfo("it-IT"), DateTimeStyles.None, out DateTime dtAbbr))
                {
                    return dtAbbr.Month.ToString("D2");
                }

                if (int.TryParse(mese, out int parsedNum) && parsedNum >= 1 && parsedNum <= 12)
                {
                    return parsedNum.ToString("D2");
                }

                return "01"; // Valore di default in caso di input non valido
            }
        }
        
        /// <summary>
        /// cerca la scheda per id e uo 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uo"></param> 
        /// <returns></returns>
        public DataTable GetSchedaById(string Idscheda, string uo)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM rapportino WHERE id = @idScheda AND uo = @uo;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    // Aggiunta sicura del parametro
                    command.Parameters.AddWithValue("@idScheda", Idscheda ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@uo", uo ?? (object)DBNull.Value);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                    {
                        try
                        {
                            da.Fill(tb);
                        }
                        catch (Exception ex)
                        {
                            // LogErrore($"Errore in GetSchedaById per ID {Idschedaa}: {ex.Message}");
                        }
                    }
                }
            }

            return tb;
        }
        /// <summary>
        /// Inserisce una gestione auto su backend MySQL e gestisce la conversione dei valori decimali.
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="idN">Restituisce l'ID autoincrementale generato, -1 se già esistente, -2 per errori di parsing.</param>
        /// <returns></returns>
        public bool InsGestioneAuto(GestAuto auto, out long idN)
        {
            bool resp = true;
            idN = 0;
            string msg = string.Empty;

            try
            {
                string inputlitri = auto.litri.ToString();
                string inputEuro = auto.euro.ToString();

                decimal importoFloat;
                decimal litriFloat;

                CultureInfo culturaItaliana = new CultureInfo("it-IT");

                // Conversione numerica
                if (decimal.TryParse(inputEuro.Replace("€", "").Trim(), NumberStyles.Any, culturaItaliana, out importoFloat) &&
                    decimal.TryParse(inputlitri.Replace("€", "").Trim(), NumberStyles.Any, culturaItaliana, out litriFloat))
                {
                    decimal valoreArrotondatoE = Math.Round(importoFloat, 2);
                    decimal valoreArrotondatoL = Math.Round(litriFloat, 2);

                    // Query MySQL ottimizzata: Inserimento e recupero ID in un'unica esecuzione atomica
                    string sql = @"
INSERT INTO gestioneauto (
    sigla, targa, stan, data, ora, litri, tipoCarburante, euro, 
    indirizzo, autista, mese, anno, nota, progressivo, uo
) 
VALUES (
    @sigla, @targa, @stan, @data, @ora, @litri, @tipoCarburante, @euro, 
    @indirizzo, @autista, @mese, @anno, @nota, @progressivo, @uo
);
SELECT LAST_INSERT_ID();";

                    using (MySqlConnection conn = new MySqlConnection(ConnString))
                    {
                        conn.Open();

                        using (MySqlCommand command = new MySqlCommand(sql, conn))
                        {
                            // Aggiunta parametri con gestione tipi nullabili e DBNull
                            command.Parameters.AddWithValue("@sigla", auto.sigla ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@targa", auto.targa ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@stan", auto.stan ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@data", auto.data > DateTime.MinValue ? (object)auto.data : DBNull.Value);
                            command.Parameters.AddWithValue("@ora", auto.ora > DateTime.MinValue ? (object)auto.ora : DBNull.Value);
                            command.Parameters.AddWithValue("@litri", valoreArrotondatoL);
                            command.Parameters.AddWithValue("@tipoCarburante", auto.tipoCarburante ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@euro", valoreArrotondatoE);
                            command.Parameters.AddWithValue("@indirizzo", auto.indirizzo ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@autista", auto.autista ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@mese", auto.mese ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@anno", auto.anno > 0 ? (object)auto.anno : DBNull.Value);
                            command.Parameters.AddWithValue("@nota", auto.nota ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@progressivo", auto.progressivo ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@uo", auto.uo ?? (object)DBNull.Value);

                            try
                            {
                                object result = command.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    idN = Convert.ToInt64(result);
                                }
                            }
                            catch (MySqlException myEx)
                            {
                                // Codice Errore MySQL 1062: Chiave Duplicata (progressivo già presente)
                                if (myEx.Number == 1062)
                                {
                                    idN = -1; // Record già esistente
                                    resp = false;
                                    msg = "Il record con questo progressivo esiste già.";
                                }
                                else
                                {
                                    resp = false;
                                    msg = $"Errore MySQL ({myEx.Number}): {myEx.Message}";
                                }

                                LogicaScritturaLog($"Sigla: {auto.progressivo}, msg: {msg} - Errore in inserimento gestioneauto");
                            }
                            catch (Exception ex)
                            {
                                resp = false;
                                LogicaScritturaLog($"Sigla: {auto.progressivo}, {ex.Message} - Errore generico inserimento gestioneauto");
                            }
                        }
                    }
                }
                else
                {
                    idN = -2; // Fallita la conversione decimal dei litri o dell'importo
                    resp = false;
                    LogicaScritturaLog($"Sigla: {auto.progressivo} - Errore conversione numerica importo o litri");
                }
            }
            catch (Exception ex)
            {
                resp = false;
                LogicaScritturaLog($"Sigla: {auto.progressivo}, msg: {msg} - {ex.Message} - Errore critico nel metodo InsGestioneAuto");
            }

            return resp;
        }

        // Routine helper di logging unificata per evitare ridondanze di codice File.AppendText
        
        /// <summary>
        /// Salva il nuovo carico nel database e restituisce l'ID generato. Se il carico esiste già, restituisce false.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool InsCarico(Carico p, int id, out long idN)
        {
            idN = -1;

            // 1. Controllo validità parametri e sessione/utente
            if (p == null || string.IsNullOrEmpty(p.matricola))
            {
                idN = -2; // Codice per Utente/Sessione non valida
                return false;
            }

            // 2. Query di inserimento diretta con restituzione immediata dell'ID generato
            string insertSql = @"
INSERT INTO carico (
    Nr_Carico, DataArrivo, Provenienza, Tipologia_atto, Giudice, TipoProvvedimentoAG, ProcedimentoPen,
    Nominativo, Indirizzo, Evasa, EvasaData, Inviata, DataInvio, esito, Operatori, DataCarico, nr_Pratica,
    Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, Matricola, DataInserimento, UlterioreTipoAtto, BU, CodiceEdificio,
    NumProtRicStessoCarico, DataDelega, GgDelega, uo,areacompetenza
) 
VALUES (
    @nr_carico, @dataArrivo, @provenienza, @tipologia_atto, @giudice, @tipoProvvedimentoAG, @procedimentoPen,
    @nominativo, @indirizzo, @evasa, @evasaData, @inviata, @dataInvio, @esito, @operatori, @dataCarico, @nr_Pratica,
    @quartiere, @note, @anno, @giorno, @rif_Prot_Gen, @matricola, @data_ins_pratica, @ulterioreTipoAtto, @bu, @codiceEdificio,
    @numProtRicStessoCarico, @dataDelega, @ggDelega, @uo,@areacompetenza
);
SELECT LAST_INSERT_ID();";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand insertCmd = new MySqlCommand(insertSql, conn))
                    {
                        // Mapping parametri con controllo NULL accurato
                        insertCmd.Parameters.AddWithValue("@nr_carico", p.nrcarico);
                        //insertCmd.Parameters.AddWithValue("@sigla", p.sigla ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@dataArrivo", p.dataArrivo ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@provenienza", p.provenienza ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@tipologia_atto", p.tipologia_atto ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@giudice", p.giudice ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@tipoProvvedimentoAG", p.tipoProvvedimentoAG ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@procedimentoPen", p.procedimentoPen ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@nominativo", p.nominativo ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@indirizzo", p.indirizzo ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@evasa", p.evasa);
                        insertCmd.Parameters.AddWithValue("@evasaData", p.evasaData ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@inviata", p.inviata ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@dataInvio", p.dataInvio ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@esito", p.esito ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@operatori", p.operatori ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@dataCarico", p.dataCarico ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@nr_Pratica", p.nr_Pratica ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@quartiere", p.quartiere ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@note", p.note ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@anno", p.anno);
                        insertCmd.Parameters.AddWithValue("@giorno", p.giorno ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@rif_Prot_Gen", p.rif_Prot_Gen ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@matricola", p.matricola);
                        insertCmd.Parameters.AddWithValue("@data_ins_pratica", p.data_ins_pratica ?? (object)DBNull.Value);
                        
                        insertCmd.Parameters.AddWithValue("@ulterioreTipoAtto", p.ulterioreTipoAtto ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@bu", p.bu ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@codiceEdificio", p.codiceEdificio ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@numProtRicStessoCarico", p.NumProtRicStessoCarico > 0 ? (object)p.NumProtRicStessoCarico : DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@dataDelega", p.dataDelega ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@ggDelega", p.ggDelega > 0 ? (object)p.ggDelega : DBNull.Value);
                        //insertCmd.Parameters.AddWithValue("@cartellina", p.cartellina ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@uo", p.uo ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@areacompetenza", p.areaCompetenza ?? (object)DBNull.Value);
                        object newIdResult = insertCmd.ExecuteScalar();
                        if (newIdResult != null && newIdResult != DBNull.Value)
                        {
                            idN = Convert.ToInt64(newIdResult);
                            return true;
                        }
                    }
                }
                catch (MySqlException myEx)
                {
                    // Errore MySQL 1062: Chiave Duplicata (Nr_Carico + Anno già presente)
                    if (myEx.Number == 1062)
                    {
                        idN = -3; // Segnalazione specifica per duplicato al chiamante
                    }

                    LogicaScritturaLog($"ERRORE MYSQL ({myEx.Number}): {myEx.Message} - Carico {p.nrcarico}/{p.anno}");
                    return false;
                }
                catch (Exception ex)
                {
                    LogicaScritturaLog($"ERRORE GENERICO: {ex.Message} - Carico {p.nrcarico}/{p.anno}");
                    return false;
                }
            }

            return false;
        }

        // Scrittura dei log pulita e protetta
        private void LogicaScritturaLog(string messaggio)
        {
            try
            {
                File.AppendAllText(LogFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {messaggio}{Environment.NewLine}");
            }
            catch { /* Evita blocco dell'applicazione se il file è occupato */ }
        }

        public bool UpdDecretazioneChiusura(Decretazione p)
        {
            if (p == null) return false;

            // 1. Query di inserimento storizzazione decretazione
            string sql_decretazione = @"
        INSERT INTO decretazione (
            decr_idPratica, decr_pratica, decr_decretante, decr_decretato,
            decr_data, decr_nota, decr_dataChiusura, decr_chiuso, decr_unire
        ) 
        VALUES (
            @idPratica, @pratica, @decretante, @decretato,
            @data, @nota, @dataChiusura, @chiuso, @unire
        );";

            // 2. Query di aggiornamento dello stato chiuso su decretazione
            string sql_updDecretazione = @"
        UPDATE decretazione 
        SET decr_chiuso = @chiuso 
        WHERE decr_pratica = @pratica 
          AND decr_idPratica = @idPratica;";

            // 3. Query di aggiornamento dello stato evasa sulla tabella principale
            string sql_updPrincipale = @"
        UPDATE principale 
        SET Evasa = 1 
        WHERE id = @idPratica 
          AND Nr_Protocollo = @pratica;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                try
                {
                    conn.Open();

                    // Avvio della transazione MySQL per garantire la consistenza di tutte e 3 le operazioni
                    using (MySqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // --- STEP 1: INSERT DECRETAZIONE ---
                            using (MySqlCommand cmdInsert = new MySqlCommand(sql_decretazione, conn, tran))
                            {
                                cmdInsert.Parameters.AddWithValue("@idPratica", p.idPratica);
                                cmdInsert.Parameters.AddWithValue("@pratica", p.Npratica ?? (object)DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("@decretante", p.decretante ?? (object)DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("@decretato", p.decretato ?? (object)DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("@data", p.data);
                                cmdInsert.Parameters.AddWithValue("@nota", p.nota ?? (object)DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("@dataChiusura", p.dataChiusura);
                                cmdInsert.Parameters.AddWithValue("@chiuso", p.chiuso);
                                cmdInsert.Parameters.AddWithValue("@unire", p.unire);
                                cmdInsert.Parameters.AddWithValue("@uo", p.uo ?? (object)DBNull.Value);

                                int rowsInserted = cmdInsert.ExecuteNonQuery();
                                if (rowsInserted <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }

                            // --- STEP 2: UPDATE DECRETAZIONE ---
                            using (MySqlCommand cmdUpdDecr = new MySqlCommand(sql_updDecretazione, conn, tran))
                            {
                                cmdUpdDecr.Parameters.AddWithValue("@chiuso", p.chiuso);
                                cmdUpdDecr.Parameters.AddWithValue("@pratica", p.Npratica ?? (object)DBNull.Value);
                                cmdUpdDecr.Parameters.AddWithValue("@idPratica", p.idPratica);

                                int rowsUpdatedDecr = cmdUpdDecr.ExecuteNonQuery();
                                if (rowsUpdatedDecr <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }

                            // --- STEP 3: UPDATE PRINCIPALE ---
                            using (MySqlCommand cmdUpdPrinc = new MySqlCommand(sql_updPrincipale, conn, tran))
                            {
                                cmdUpdPrinc.Parameters.AddWithValue("@idPratica", p.idPratica);
                                cmdUpdPrinc.Parameters.AddWithValue("@pratica", p.Npratica ?? (object)DBNull.Value);

                                cmdUpdPrinc.ExecuteNonQuery();
                            }

                            // Se tutte le operazioni sono andate a buon fine, effettua il Commit
                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // In caso di eccezione durante le query, esegui il Rollback
                            tran.Rollback();
                            if (!File.Exists(LogFile))
                            {
                                using (StreamWriter sw = File.CreateText(LogFile)) { }
                            }

                            using (StreamWriter sw = File.AppendText(LogFile))
                            {
                                sw.WriteLine($"pratica:{p.Npratica} - {ex.Message} - Errore in update dati decretazione");
                                sw.Close();
                            }

                            return false;
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
                        sw.WriteLine($"Connessione DB fallita per pratica:{p?.Npratica} - {ex.Message}");
                        sw.Close();
                    }

                    return false;
                }
            }
        }

        public bool UpdDecretazione(Decretazione p)
        {
            if (p == null) return false;

            // Query di aggiornamento MySQL parametrizzata con aggiunta del campo decr_uo
            string sql_updDecretazione = @"
        UPDATE decretazione 
        SET decr_data = @data, 
            decr_nota = @nota, 
            decr_decretato = @decretato, 
            decr_decretante = @decretante, 
            decr_unire = @unire,
            decr_uo = @uo
        WHERE decr_id = @id;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql_updDecretazione, conn))
                {
                    // Mappatura sicura dei parametri
                    command.Parameters.AddWithValue("@data", p.data);
                    command.Parameters.AddWithValue("@nota", p.nota ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@decretato", p.decretato ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@decretante", p.decretante ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@unire", p.unire);
                    command.Parameters.AddWithValue("@uo", p.uo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id", p.id);

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"pratica:{p.Npratica} - {ex.Message} - Errore in update dati decretazione (UpdDecretazione)");
                            sw.Close();
                        }

                        return false;
                    }
                }
            }
        }

        //I 26/03/2026 - aggiunta variabile per abilitazione operatore
        public bool UpdOperatore(string matricola, string ruolo, string grado)
        {
            if (string.IsNullOrEmpty(matricola)) return false;

            string sql_Upd = @"
        UPDATE utenti 
        SET ruolo = @ruolo, 
            grado = @grado
        WHERE matricola = @matricola;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql_Upd, conn))
                {
                    command.Parameters.AddWithValue("@ruolo", ruolo?.Trim() ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@grado", grado?.Trim() ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@matricola", matricola.Trim());

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"Matricola:{matricola}, grado:{grado}: {ex.Message} - Errore in update utenti");
                            sw.Close();
                        }

                        return false;
                    }
                }
            }
        }

        // <summary>
        /// modifica il flag verificato a true nella tabella gestione auto
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool UpdGestioneAutoById(GestAuto p)
        {
            if (p == null || p.id <= 0)
                return false;

            // Query parametrizzata per prevenire SQL Injection e gestire correttamente i tipi MySQL
            string sql = @"
UPDATE gestioneauto 
SET 
    verificato = 1, 
    data_verifica = @dataVerifica, 
    matricola = @matricola 
WHERE id = @id;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand(sql, conn))
                    {
                        // Parametrizzazione sicura dei valori
                        command.Parameters.AddWithValue("@dataVerifica", p.dataVerifica > DateTime.MinValue ? (object)p.dataVerifica : DBNull.Value);
                        command.Parameters.AddWithValue("@matricola", !string.IsNullOrEmpty(p.matricola) ? p.matricola.Trim() : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id", p.id);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Ritorna true se almeno una riga è stata effettivamente aggiornata
                        return rowsAffected > 0;
                    }
                }
                catch (MySqlException myEx)
                {
                    LogicaScritturaLog($"Targa: {p.targa}, DataVerifica: {p.dataVerifica}, ID: {p.id} - Errore MySQL ({myEx.Number}): {myEx.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    LogicaScritturaLog($"Targa: {p.targa}, DataVerifica: {p.dataVerifica}, ID: {p.id} - Errore generico UPDATE gestione auto: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// modifica i campi della gestione auto esistente
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        public bool UpdGestioneAuto(GestAuto auto)
        {
            if (auto == null || auto.id <= 0)
                return false;

            // Query UPDATE con parametri fortemente tipizzati per eliminare SQL Injection e manipolazione di stringhe
            string sql = @"
UPDATE gestioneauto 
SET 
    data = @data, 
    ora = @ora, 
    sigla = @sigla, 
    stan = @stan, 
    progressivo = @progressivo, 
    litri = @litri, 
    euro = @euro, 
    matricola = @matricola, 
    indirizzo = @indirizzo
WHERE id = @id;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand(sql, conn))
                    {
                        // Mappatura nativa dei tipi di dato senza formattazioni manuali (Replace)
                        command.Parameters.AddWithValue("@data", auto.data);
                        command.Parameters.AddWithValue("@ora", auto.ora);
                        command.Parameters.AddWithValue("@sigla", auto.sigla ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@stan", auto.stan ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@progressivo", auto.progressivo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@litri", Math.Round((decimal)auto.litri, 2));
                        command.Parameters.AddWithValue("@euro", Math.Round((decimal)auto.euro, 2));
                        command.Parameters.AddWithValue("@matricola", auto.matricola ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@indirizzo", auto.indirizzo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id", auto.id);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (MySqlException myEx)
                {
                    // Errore 1062: Tentativo di aggiornare il progressivo con uno già esistente in tabella
                    if (myEx.Number == 1062)
                    {
                        LogicaScritturaLog($"Progressivo duplicato ({auto.progressivo}) - Impossibile aggiornare ID {auto.id}");
                    }
                    else
                    {
                        LogicaScritturaLog($"Stan: {auto.stan}, ID: {auto.id} - Errore MySQL ({myEx.Number}): {myEx.Message}");
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    LogicaScritturaLog($"Stan: {auto.stan}, ID: {auto.id} - Errore generico UPDATE gestioneauto: {ex.Message}");
                    return false;
                }
            }
        }

        public bool UpdAbilitaOperatore(string matricola, bool abilitato)
        {
            if (string.IsNullOrEmpty(matricola)) return false;

            // Inverte il valore booleano come nel codice originale (!abilitato)
            bool nuovoStato = !abilitato;

            string sql_Upd = @"
        UPDATE utenti 
        SET abilitato = @abilitato 
        WHERE matricola = @matricola;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql_Upd, conn))
                {
                    // MySQL mappa automaticamente il booleano su TINYINT(1) (1 per true, 0 per false)
                    command.Parameters.AddWithValue("@abilitato", nuovoStato);
                    command.Parameters.AddWithValue("@matricola", matricola.Trim());

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"Matricola:{matricola}: {ex.Message} - Errore in update operatore abilitazione");
                            sw.Close();
                        }

                        return false;
                    }
                }
            }
        }
        //F 26/03/2026 - aggiunta variabile per abilitazione operatore


        public bool UpdPratica(Carico p, string oldMat, int ID, DateTime olddate, string user)
        {
            if (string.IsNullOrWhiteSpace(user) || p == null)
            {
                return false;
            }

            // 1. Query UPDATE su 'carico' con l'aggiunta del campo 'uo'
            string sql_pratica = @"
        UPDATE carico 
        SET Sigla = @sigla,
            Nominativo = @nominativo,
            Indirizzo = @indirizzo,
            via = @via,
            Inviata = @inviata,
            DataInvio = @dataInvio,
            Scaturito = @scaturito,
            Operatori = @operatori,
            DataCarico = @dataCarico,
            Quartiere = @quartiere,
            nr_Pratica = @nr_Pratica,
            giudice = @giudice,
            ProcedimentoPen = @procedimentoPen,
            matricola = @matricola,
            DataInserimento = @data_ins_pratica,
            macro_area = @macro_area,
            Rif_Prot_Gen = @rif_Prot_Gen,
            dataarrivo = @dataArrivo,
            Tipologia_atto = @tipologia_atto,
            provenienza = @provenienza,
            TipoProvvedimentoAG = @tipoProvvedimentoAG,
            UlterioreTipoAtto = @ulterioreTipoAtto,
            evasadata = @evasaData,
            bu = @bu,
            codiceEdificio = @codiceEdificio,
            operatori2 = @operatori2,
            operatori3 = @operatori3,
            NumProtRicStessoCarico = @numProtRicStessoCarico,
            DataDelega = @dataDelega,
            GgDelega = @ggDelega,
            cartellina = @cartellina,
            Rif_Prot_Uscita = @rif_Prot_Uscita,
            uo = @uo
        WHERE ID = @ID;";

            // 2. Query INSERT su 'principalestorico' (sostituito getdate() con NOW())
            string sql_storico = @"
        INSERT INTO principalestorico (
            nr_protocollo, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen, 
            Nominativo, Indirizzo, via, Evasa, EvasaData, Inviata, DataInvio, Scaturito, Operatori, DataCarico, nr_Pratica, Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, matricola, DataInserimento, 
            DataStoricizzazione, MatricolaStoricizzazione, UlterioreTipoAtto, bu, CodiceEdificio, operatori2, operatori3, NumProtRicStessoCarico, DataDelega, GgDelega, Rif_Prot_Uscita
        ) 
        SELECT 
            nr_protocollo, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen, 
            Nominativo, Indirizzo, via, Evasa, EvasaData, Inviata, DataInvio, Scaturito, Operatori, DataCarico, nr_Pratica, Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, matricola, DataInserimento, 
            NOW(), @MatricolaOperatore, UlterioreTipoAtto, bu, CodiceEdificio, operatori2, operatori3, NumProtRicStessoCarico, DataDelega, GgDelega, Rif_Prot_Uscita
        FROM principale 
        WHERE id = @ID;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                try
                {
                    conn.Open();

                    using (MySqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // --- STEP 1: ESECUZIONE UPDATE CARICO ---
                            using (MySqlCommand cmdPratica = new MySqlCommand(sql_pratica, conn, tran))
                            {
                                //cmdPratica.Parameters.AddWithValue("@sigla", p.sigla ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@nominativo", p.nominativo ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@indirizzo", p.indirizzo ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@via", p.via ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@inviata", p.inviata ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@dataInvio", p.dataInvio ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@scaturito", p.esito ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@operatori", p.operatori ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@dataCarico", p.dataCarico ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@quartiere", p.quartiere ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@nr_Pratica", p.nr_Pratica ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@giudice", p.giudice ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@procedimentoPen", p.procedimentoPen ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@matricola", p.matricola ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@data_ins_pratica", p.data_ins_pratica);
                                cmdPratica.Parameters.AddWithValue("@macro_area", p.areaCompetenza ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@rif_Prot_Gen", p.rif_Prot_Gen ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@dataArrivo", p.dataArrivo ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@tipologia_atto", p.tipologia_atto ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@provenienza", p.provenienza ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@tipoProvvedimentoAG", p.tipoProvvedimentoAG ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@ulterioreTipoAtto", p.ulterioreTipoAtto ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@evasaData", p.evasaData ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@bu", p.bu ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@codiceEdificio", p.codiceEdificio ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@operatori2", p.operatori2 ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@operatori3", p.operatori3 ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@numProtRicStessoCarico", p.NumProtRicStessoCarico);
                                cmdPratica.Parameters.AddWithValue("@dataDelega", p.dataDelega ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@ggDelega", p.ggDelega);
                                //cmdPratica.Parameters.AddWithValue("@cartellina", p.cartellina ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@rif_Prot_Uscita", p.rif_Prot_Uscita ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@uo", p.uo ?? (object)DBNull.Value);
                                cmdPratica.Parameters.AddWithValue("@ID", ID);

                                int rowsUpdated = cmdPratica.ExecuteNonQuery();
                                if (rowsUpdated <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }

                            // --- STEP 2: ESECUZIONE STORICIZZAZIONE ---
                            using (MySqlCommand cmdStorico = new MySqlCommand(sql_storico, conn, tran))
                            {
                                cmdStorico.Parameters.AddWithValue("@MatricolaOperatore", user);
                                cmdStorico.Parameters.AddWithValue("@ID", ID);

                                int rowsInserted = cmdStorico.ExecuteNonQuery();
                                if (rowsInserted <= 0)
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }

                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            if (!File.Exists(LogFile))
                            {
                                using (StreamWriter sw = File.CreateText(LogFile)) { }
                            }

                            using (StreamWriter sw = File.AppendText(LogFile))
                            {
                                sw.WriteLine($"matricola:{p.matricola}, data ins:{p.data_ins_pratica} - {ex.Message} - Errore in salva pratica transaction");
                                sw.Close();
                            }

                            return false;
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
                        sw.WriteLine($"Errore di connessione DB per matricola:{p?.matricola} - {ex.Message}");
                        sw.Close();
                    }
                    return false;
                }
            }
        }

        public bool SavePassword(string password, string matricola)
        {
            if (string.IsNullOrWhiteSpace(matricola)) return false;

            // Query parametrizzata per MySQL
            string sql_save = @"
        UPDATE utenti 
        SET passw = @password, 
            reset = @reset 
        WHERE matricola = @matricola;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql_save, conn))
                {
                    // Mappatura sicura dei parametri
                    command.Parameters.AddWithValue("@password", password ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@reset", true); // Passa true come booleano nativo
                    command.Parameters.AddWithValue("@matricola", matricola.Trim());

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"matricola:{matricola} {ex.Message} - Errore in salva password");
                            sw.Close();
                        }

                        return false;
                    }
                }
            }
        }
        public bool ResetPassw(string password, string matricola)
        {
            if (string.IsNullOrWhiteSpace(matricola)) return false;

            // Query UPDATE parametrizzata
            string sql_reset = @"
        UPDATE utenti 
        SET passw = @password, 
            reset = @reset 
        WHERE matricola = @matricola;";

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand command = new MySqlCommand(sql_reset, conn))
                {
                    // Aggiunta parametri sicuri
                    command.Parameters.AddWithValue("@password", password ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@reset", false); // Mappato come 0/TINYINT(1) in MySQL
                    command.Parameters.AddWithValue("@matricola", matricola.Trim());

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine($"matricola:{matricola} {ex.Message} - Errore in reset password");
                            sw.Close();
                        }


                        return false;
                    }
                }
            }
        }

        public bool VerificaPermesso(string ruolo, string pagina)
        {
            if (string.IsNullOrWhiteSpace(ruolo) || string.IsNullOrWhiteSpace(pagina))
            {
                return false;
            }
            // Normalizza la stringa ricevuta: rimuove tilde e slash iniziali (es. "/View/Default.aspx" -> "view/default.aspx")
            string urlSanitizzato = pagina.Trim().TrimStart('~', '/').ToLower();
            string nomeFile = System.IO.Path.GetFileName(pagina).Trim().ToLower();
            //--Confronta rimuovendo '~' e '/' dal campo del DB
            string sql = @"
SELECT COUNT(1) 
FROM PermessiRuoli pr
INNER JOIN Pagine p ON pr.ID_Pagina = p.ID_Pagina
WHERE LOWER(TRIM(pr.Ruolo)) = LOWER(@Ruolo) 
  AND (
        
        LOWER(REPLACE(REPLACE(TRIM(p.URL), '~', ''), '/', '')) = REPLACE(@UrlSanitizzato, '/', '')
     OR LOWER(TRIM(p.URL)) LIKE CONCAT('%', @NomeFile)
      );";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.Add(new MySqlParameter("@Ruolo", MySqlDbType.VarChar, 50) { Value = ruolo.Trim() });
                            cmd.Parameters.Add(new MySqlParameter("@UrlSanitizzato", MySqlDbType.VarChar, 255) { Value = urlSanitizzato });
                            cmd.Parameters.Add(new MySqlParameter("@NomeFile", MySqlDbType.VarChar, 255) { Value = nomeFile });

                            conn.Open();

                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            return count > 0;
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
                    sw.WriteLine($"Ruolo:{ruolo}, Pagina:{pagina} - {ex.Message} - Errore in VerificaPermesso");
                    sw.Close();
                }
                
                return false;
            }
        }
        ///////// METODI PER RICERCA DATI DA CARICO CENTRALIZZATO /////////////////////


        
        /// <summary>
        /// ricerca effettuata dal comandante / dirigente per procedimento penale
        /// </summary>
        /// <param name="procPenale"></param>
        /// <param name="dbOrigineOut"></param>
        /// <returns></returns>
        public DataTable CmdByProcPenale(string ProcPenale, out string dbOrigineOut)
        {

            string sql = string.Empty;
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();

                // STEP 1: Recupera il nome del Database di origine da RegistroCarico
                string dbOrigine = string.Empty;
                string sqlGetDb = @"SELECT DatabaseOrigine 
                        FROM [DB_ProgCaricoCentralizzato].[dbo].[RegistroCarico] 
                        WHERE ProcPenale LIKE @ProcPenale";

                using (MySqlCommand cmdDb = new MySqlCommand(sqlGetDb, conn))
                {
                    cmdDb.Parameters.AddWithValue("@ProcPenale", $"%{ProcPenale}%");

                    object result = cmdDb.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        dbOrigine = result.ToString();
                        dbOrigineOut = result.ToString();
                    }
                }

                // Se non trova il record, interrompi o gestisci l'errore
                if (string.IsNullOrEmpty(dbOrigine))
                {
                    // Gestisci il caso: Carico non trovato
                    dbOrigineOut = string.Empty;
                    return null;
                }

                // STEP 2: Costruisci la query dinamica usando il DB recuperato
                // Usiamo le parentesi quadre [] intorno a dbOrigine per evitare sintassi errate

                string sqlDinamica = $@"
        SELECT 
            x.*, 
            u.nominativo AS nomeoperatore , @DbOrigine AS DatabaseOrigine
        FROM [DB_ProgCaricoCentralizzato].[dbo].[RegistroCarico] AS c 
        INNER JOIN [{dbOrigine}].[dbo].[Carico] AS x 
            ON c.IdRecordOrigine = x.Id 
           AND c.Carico = x.Nr_Carico 
        LEFT JOIN [DB_ProgCaricoCentralizzato].[dbo].[Utenti] AS u 
            ON x.Matricola = u.Matricola 
        WHERE x.ProcedimentoPen like @ProcPenale 
          order by dataarrivo desc";

                using (MySqlCommand cmd = new MySqlCommand(sqlDinamica, conn))
                {
                    cmd.Parameters.AddWithValue("@ProcPenale", $"%{ProcPenale}%");
                    cmd.Parameters.AddWithValue("@DbOrigine", dbOrigine);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        // 'dt' ora contiene i dati presi dal database dinamico!
                    }

                    // 2. Aggiunge la colonna MANUALE al DataTable prima del DataBind
                    if (!dt.Columns.Contains("DbOrigine"))
                    {
                        dt.Columns.Add("DbOrigine", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {
                            row["DbOrigine"] = dbOrigine; // Assegna il valore
                        }
                    }
                    dbOrigineOut = dbOrigine.ToUpper();
                    return dt;
                }
            }

        }

        public DataTable CmdByProtGenerale(string ProtGenerale, out string dbOrigineOut)
        {

            string sql = string.Empty;
            DataTable tb = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();

                // STEP 1: Recupera il nome del Database di origine da RegistroCarico
                string dbOrigine = string.Empty;
                string sqlGetDb = @"SELECT DatabaseOrigine 
                        FROM [DB_ProgCaricoCentralizzato].[dbo].[RegistroCarico] 
                        WHERE ProtGenerale LIKE @ProtGenerale";

                using (MySqlCommand cmdDb = new MySqlCommand(sqlGetDb, conn))
                {
                    cmdDb.Parameters.AddWithValue("@ProtGenerale", $"%{ProtGenerale}%");

                    object result = cmdDb.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        dbOrigine = result.ToString();
                        dbOrigineOut = result.ToString();
                    }
                }

                // Se non trova il record, interrompi o gestisci l'errore
                if (string.IsNullOrEmpty(dbOrigine))
                {
                    // Gestisci il caso: Carico non trovato
                    dbOrigineOut = string.Empty;
                    return null;
                }

                // STEP 2: Costruisci la query dinamica usando il DB recuperato
                // Usiamo le parentesi quadre [] intorno a dbOrigine per evitare sintassi errate
                string sqlDinamica = $@"
        SELECT 
            x.*, 
            u.nominativo AS nomeoperatore , @DbOrigine AS DatabaseOrigine
        FROM [DB_ProgCaricoCentralizzato].[dbo].[RegistroCarico] AS c 
        INNER JOIN [{dbOrigine}].[dbo].[Carico] AS x 
            ON c.IdRecordOrigine = x.Id 
           AND c.Carico = x.Nr_Carico 
        LEFT JOIN [DB_ProgCaricoCentralizzato].[dbo].[Utenti] AS u 
            ON x.Matricola = u.Matricola 
        WHERE x.Rif_Prot_Gen like @ProtGenerale 
          order by dataarrivo desc";

                using (MySqlCommand cmd = new MySqlCommand(sqlDinamica, conn))
                {
                    cmd.Parameters.AddWithValue("@ProtGenerale", $"%{ProtGenerale}%");
                    cmd.Parameters.AddWithValue("@DbOrigine", dbOrigine);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        // 'dt' ora contiene i dati presi dal database dinamico!
                    }

                    // 2. Aggiunge la colonna MANUALE al DataTable prima del DataBind
                    if (!dt.Columns.Contains("DbOrigine"))
                    {
                        dt.Columns.Add("DbOrigine", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {
                            row["DbOrigine"] = dbOrigine.ToUpper(); // Assegna il valore
                        }
                    }
                    dbOrigineOut = dbOrigine.ToUpper();
                    return dt;
                }
            }

        }



        public DataTable CercaInTuttiIDatabase(List<string> listaDatabase, string testoDaCercare)
        {
            DataTable dtRisultati = new DataTable();
            if (listaDatabase == null || listaDatabase.Count == 0 || string.IsNullOrWhiteSpace(testoDaCercare))
            {

                return dtRisultati;
            }

            List<string> subQueries = new List<string>();

            // 1. Costruiamo una SELECT per ogni database valido
            // 1. Costruisci TUTTE le sotto-query prima nel ciclo
            foreach (string db in listaDatabase.Where(d => !string.IsNullOrWhiteSpace(d)).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                subQueries.Add($@"
SELECT 
    p.Id, p.Nr_Carico, p.Sigla, p.DataArrivo, p.Provenienza, p.Tipologia_atto,
    p.Giudice, p.TipoProvvedimentoAG, p.ProcedimentoPen, p.Nominativo, p.Indirizzo,
    p.Evasa, s.nominativo AS nomeoperatore, p.EvasaData, p.Inviata, p.DataInvio,
    p.Scaturito, p.Operatori, p.DataCarico, p.nr_Pratica, p.Quartiere, p.Note,
    p.Anno, p.Giorno, p.Rif_Prot_Gen, p.Matricola, p.DataInserimento, p.Macro_area,
    p.Operatori2, p.Operatori3, p.UlterioreTipoAtto, p.BU, p.CodiceEdificio, p.cartellina,uo
    
FROM Carico p
LEFT JOIN utenti S ON p.matricola = S.matricola 
WHERE p.ProcedimentoPen LIKE @SearchText
   OR EXISTS (
        SELECT 1 
        FROM decretazione d 
        WHERE d.decr_idPratica = p.id AND d.decr_nota LIKE @SearchText
   )");
            }

            // 2. Se non ci sono sotto-query valide, esci
            if (subQueries.Count == 0)
            {
                return dtRisultati;
            }

            // 3. Unisci le query in una sola stringa SQL con ORDER BY finale
            string sqlFinale = string.Join(" UNION ALL ", subQueries) + " ORDER BY DataArrivo DESC";

            //string connectionString = DbHelper.GetConnectionString();

            // 4. Esegui la query UNA SOLA VOLTA fuori dal foreach
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlFinale, conn))
                {
                    // Se vuoi la ricerca parziale passa "%" + testoDaCercare + "%", altrimenti passa solo testoDaCercare
                    cmd.Parameters.AddWithValue("@SearchText", $"%{testoDaCercare}%");

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dtRisultati);

                        
                    }
                }
            }

            return dtRisultati;


        }
    }

}