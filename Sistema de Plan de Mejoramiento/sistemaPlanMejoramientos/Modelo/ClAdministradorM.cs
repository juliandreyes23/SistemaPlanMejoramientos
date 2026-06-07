using System;
using System.Collections.Generic;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClAdministradorM
    {
        public int idAdministrador { get; set; }

        public int idUsuario { get; set; }

        public int idCentro { get; set; }

        public ClUsuarioM usuario { get; set; }

        public ClCentroM centro { get; set; }
    }
}