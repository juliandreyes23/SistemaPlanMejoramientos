using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClPlanMejoramientoM
    {
        public int idPlanMejoramiento { get; set; }
        public string tipoPlan {  get; set; }
        public DateTime fechaAsignacion { get; set; }
        public DateTime fechaLimite { get; set; }
        public string actividades { get; set; }
        public string observaciones { get; set; }
        public string estadoPlan { get; set; }
        public int idAprendiz {  get; set; }
        public int idInstructor { get; set; }

    }
}