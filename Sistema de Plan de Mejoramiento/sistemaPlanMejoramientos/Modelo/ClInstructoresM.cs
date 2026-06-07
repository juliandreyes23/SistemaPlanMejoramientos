using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClInstructoresM
    {
        public int idInstructor { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string especialidad { get; set; }
        public int idUsuario { get; set; }
        public string nombreCentro { get; set; }

        public ClUsuarioM usuario { get; set; }

        public List<ClFichaInstructorM> fichas { get; set; }

        public List<ClPlanMejoramientoM> planesMejoramiento { get; set; }

        public ClCentroM centro { get; set; }
    }
}