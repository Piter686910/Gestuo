using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public class Rapportino
    {
        
        public string orario = string.Empty;
        public DateTime data_consegna_intervento;
        public DateTime dataInserimento;
        public string pattuglia = string.Empty;
        public string siglaVettura = string.Empty;
        public string matricola = string.Empty;
        public string grado = string.Empty;
        public string itinerario = string.Empty;
        public Boolean appiedata = false;
        public Boolean attivitaInterna = false;
        public string nota = string.Empty;
        public string uo = string.Empty;
        public Int32 ContrCds = 0;
        public Int32 PvContestatiCds = 0;
        public Int32 PvNoContestatiCds = 0;
        public Int32 OccupSuoloPub = 0;
        public Int32 PvContestatiOccupSuoloPub = 0;
        public Int32 PvNoContestatiOccupSuoloPub = 0;
        public Int32 Autotrasporti = 0;
        public Int32 PvContestatiAutotrasporti = 0;
        public Int32 PvNoContestatiAutotrasporti = 0;
        public Int32 TargheEstere = 0;
        public Int32 PvContestatiTargheEstere = 0;
        public Int32 PvNoContestatiTargheEstere = 0;
        public Int32 ParcheggiatoriAbusivi = 0;
        public Int32 PvContestatiParcheggiatoriAbusivi = 0;
        public Int32 PvNoContestatiParcheggiatoriAbusivi = 0;
        public Int32 ContrAltreForze = 0;
        public Int32 PvContestatiContrAltreForze = 0;
        public Int32 PvNoContestatiContrAltreForze = 0;
        public Int32 ContrMicroMobilita = 0;
        public Int32 PvContestatiContrMicroMobilita = 0;
        public Int32 PvNoContestatiContrMicroMobilita = 0;
        public Int32 ConferimentoRifiuti = 0;
        public Int32 SanzioniConferimentoRifiuti = 0;
        public Int32 DeiezioneCanina = 0;
        public Int32 SanzioniDeiezioneCanina = 0;
        public Int32 LeggeAntifumo = 0;
        public Int32 SanzioniLeggeAntifumo = 0;
        public Int32 AreaMercatale = 0;
        public Int32 SanzioniAreaMercatale = 0;
        public Int32 InquinamentoAcustico = 0;
        public Int32 SanzioniInquinamentoAcustico = 0;
        public Int32 ContrCantieriSeq = 0;
        public Int32 SanzioniContrCantieriSeq = 0;
        public Int32 LavoriSenzaProtez = 0;
        public Int32 SanzioniLavoriSenzaProtez = 0;
        public Int32 NotificheSicurezza = 0;
        public Int32 SanzioniNotificheSicurezza = 0;
        public Int32 Ambulanti = 0;
        public Int32 SanzioniAmbulanti = 0;
        public Int32 ProvenienzaAlimentare = 0;
        public Int32 SanzioniProvenienzaAlimentare = 0;
        public Int32 TrasportoAlimentare = 0;
        public Int32 SanzioniTrasportoAlimentare = 0;
        public Int32 Officine = 0;
        public Int32 SanzioniOfficine = 0;
        public Int32 ContrBnB = 0;
        public Int32 SanzioniContrBnB = 0;
        
        public Boolean print = false;
    }
}