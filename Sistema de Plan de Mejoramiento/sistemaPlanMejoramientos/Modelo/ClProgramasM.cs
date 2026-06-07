using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClProgramasM
    {
        public int idPrograma { get; set; }
        public string codigoPrograma { get; set; }
        public string nombre { get; set; }
        public string version { get; set; }
        public string nivel { get; set; }
        public string duracion { get; set; }
        public string estado { get; set; }

        public int idCentro { get; set; }

        public ClCentroM centro { get; set; }

        public List<ClFichasM> fichas { get; set; }

        public List<ClCompetenciasM> competencias { get; set; }
    }
}