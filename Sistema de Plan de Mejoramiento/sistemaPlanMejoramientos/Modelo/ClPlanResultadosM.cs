using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClPlanResultadosM
    {
        public int idPlanMejoramiento { get; set; }

        public int idResultadoAprendizaje { get; set; }

        public ClPlanMejoramientoM planMejoramiento { get; set; }

        public ClResultadoAprendizajeM resultadoAprendizaje { get; set; }
    }
}