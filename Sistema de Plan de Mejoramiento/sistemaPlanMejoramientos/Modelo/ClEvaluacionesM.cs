using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClEvaluacionesM
    {
        public int idEvaluacion {  get; set; }
        public int idPlanMejoramiento { get; set; }
        public string criterioProducto { get; set; }
        public string criterioConocimiento { get; set; }
        public string criterioDesempeno { get; set; }
        public string observaciones {  get; set; }

    }
}