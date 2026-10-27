using System;
using System.ComponentModel;
using System.Reflection;

namespace GestUO.Classi
{
    public static class Enumerate
    {
        public enum Area
        {
            UOTE = 0,
            UOTP = 1
        }
        public enum Ruolo
        {
            Admin = 0,
            operatori = 1,
            MasterAG = 2,
            Archivio = 3,
            CoordinamentoAtti = 4,
            PG = 5,
            SuperAdmin = 6,
            CoordinamentoPg = 7,
            urp = 8,
            comandante=9
        }
        public enum Profilo
        {
            operatori = 1, //operatori
            due = 2,
            tre = 3, //admin, responsabili, sa
            V = 4 // solo visualizzazine

        }

        public enum Tipologie
        {
            [Description("Altro")]
            Altro = 1,

            [Description("DELEGA INDAGINE")]
            DelegaIndagine = 2,

            [Description("ESPOSTO - SEGNALAZIONE - RICHIESTA ACCERTAMENTI")]
            EspostoSegnalazione = 3


        }
        public enum MsgOutput
        {
            [Description("Sessione Scaduta, effettuare Login.")]
            SScaduta = 1,
            [Description("Nome utente o password errati.")]
            UserWrong = 2,
            [Description("La matricola ineserita è inesistente.")]
            NoUser = 3,
            [Description("password non salvata.")]
            PwNoSave = 4,
            [Description("Password resettata. La nuova password temporanea è la tua matricola")]
            PwResetOk = 5,
            [Description("inserimento/modifica non effettuata, controllare il log.")]
            ErrorLog = 6,
            [Description("modifica effettuata correttamente.")]
            ModificaCorretta = 7,
            [Description("E' possibile inserire max 3 operatori.")]
            Maxoperatori = 8,
            [Description("Pratica non trovata.")]
            PraticaNotFound = 9,
            [Description("Inserimento effettuato correttamente.")]
            InsOk = 10,
            [Description("chiusura non effettuata, controllare il log.")]
            CloseKO = 11,
            [Description("chiusura effettuata correttamente.")]
            CloseOK = 12,
            [Description("Nessuna scheda associata questa statistica.")]
            NoStatistiche = 13,
            [Description("Registro Modificato.")]
            UpdRegistroOk = 14,
            [Description("Errore update Registro.")]
            UpdRegistroKo = 15,
            [Description("Pratica già esistente il nuovo numero è")]
            DupPratica = 16,
            [Description("la Modifica non può essere effettuata pratica chiusa.")]
            PraticaChiusa = 17,
            [Description("Selezionare un valore valido.")]
            SelKo = 18,
            [Description("Errore update Interrogatorio.")]
            UpdInterrogatorioKo = 19,
            [Description("Vuoi salvare anche la pratica?")]
            SavePratica = 20,
            //I 26/03/2024: Aggiunta verifica connessione sicura (HTTPS) prima di procedere con l'autenticazione
            [Description("Errore di sicurezza: Connessione non criptata. Impossibile procedere con il login.")]
            ConnectNotSure = 21,
            //F 26/03/2024: Aggiunta verifica connessione sicura (HTTPS) prima di procedere con l'autenticazione
            //I 26/03/2024:  utente disabilitato
            [Description("Utente non abilitato")]
            UserDisable = 22,
            [Description("Cancellazione avvenuta con successo.")]
            Delok = 23,
            [Description("Inserire il Fascicolo.")]
            InserireFascicolo = 24,
            [Description("Nessuna Corrispondenza trovata.")]
            Notfound = 25,
            [Description("Inserire Matricola o Nominativo.")]
            FiledRegìquired = 26,

            [Description("Inserimento non effettuato.")]
            InsKo = 27,
            [Description("Inserire Quartiere.")]
            InserireQuartiere = 28,
            [Description("Utente non autorizzato a consultare la pagina.")]
            NoAutorizzato = 29,
            [Description("Pratica non presente nel Database di appartenenza.")]
            DbNonAppartenente = 30,
            [Description("Se la sigla è AG tipologia atto non può essere ESPOSTO - SEGNALAZIONE.")]
            SiglaErrata = 31,
            [Description("Inserimento scheda effettuato. Vuoi stampare?")]
            VuoiStampa = 32

            
            
        }
        public enum Sigla
        {
            AG = 1,
            ED = 2,
            TP = 3

        }
        public enum CampiXStatistiche
        {
            relazione = 0,
            espoEvasi = 1,
            ripTotPar = 2,
            contrScia = 3,
            notifiche = 4,
            cnr = 5,
            sequestri = 6,
            riappSigilli = 7,
            annotazioni = 8,
            delegheEsitate = 9,
            violAmm = 10,
            convalide = 11,
            dissequestri = 12,
            disseqTemp = 13,
            rimozSigilli = 14,
            violSigilli = 15,
            contr4204 = 16,
            ponteggi = 17,
            dpi = 18,
            contrCant = 19,
            contr_cant_suolo_pubb = 20,
            censimentoAllPubb = 21,
            controlliOccupazioneAbus = 22,
            abitativo = 23,
            nonAbitativo = 24,
            SgomberiAbus = 25,
            SgomberiImmobili = 26,
            NotificaTp = 27


        }

        /// <summary>
        /// preleva la stringa da Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;

            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));

            // Ritorna la descrizione se presente, altrimenti il nome del membro stesso.
            return attribute != null ? attribute.Description : value.ToString();
        }

        // Classi di supporto
        public class RisultatoRicerca
        {
            public bool Trovato { get; set; }
            public int? Giorno { get; set; }
        }

        public class RecordRsnl
        {
            public string Gruppo { get; set; }
            public int Quartina { get; set; }
            public DateTime? DataRS { get; set; }
            public DateTime? DataNL { get; set; }
            public string MeseStringa { get; set; } // Campo richiesto: stringa originale
        }

    }
}