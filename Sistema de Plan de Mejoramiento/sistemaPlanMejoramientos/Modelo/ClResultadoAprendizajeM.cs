using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClResultadoAprendizajeM
    {
        public int idResultadoAprendizaje { get; set; }

        public string descripcion { get; set; }

        public string nombreCompetencia { get; set; }

        public int idCompetencia { get; set; }

        public ClCompetenciasM competencia { get; set; }

        public List<ClPlanResultadosM> planesResultado { get; set; }
    }
}