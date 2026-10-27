using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestUO.Classi
{
    public class GestAuto
    {
        public Int32 id = 0;
        public string sigla = string.Empty;
        public string targa = string.Empty;
        public DateTime data;
        public DateTime ora;
        public string stan = string.Empty;
        public float litri = 0;
        public string tipoCarburante = string.Empty;
        public float euro = 0;
        public string indirizzo = string.Empty;
        public string autista = string.Empty;
        public string mese = string.Empty;
        public Int16 anno = 0;
        public string nota = string.Empty;
        public string matricola = string.Empty;
        public DateTime dataVerifica;
        public Boolean verificato;
        public string progressivo = string.Empty;
        public string uo = string.Empty;
    }
}