using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClRecuperacionPasswordM
    {
        public int idRecuperacion {  get; set; }
        public int idUsuario { get; set; }
        public string token { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public bool usado { get; set; }

    }
}