using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClPlanMejoramientoM
    {
        public int idPlanMejoramiento { get; set; }

        public string tipoPlan { get; set; }

        public DateTime fechaAsignacion { get; set; }

        public DateTime fechaLimite { get; set; }

        public string actividades { get; set; }

        public string observaciones { get; set; }

        public string estadoPlan { get; set; }

        public int idAprendiz { get; set; }

        public int idInstructor { get; set; }

        public string nombreAprendiz { get; set; }

        public string docAprendiz { get; set; }

        public string nombreInstructor { get; set; }

        public string codigoFicha { get; set; }

        public int totalEvidencias { get; set; }

        public bool yaEvaluado { get; set; }

        public string criterioProducto { get; set; }

        public string criterioConocimiento { get; set; }

        public string criterioDesempeno { get; set; }
        public string observacionesEvaluacion { get; set; }

        public ClAprendizM aprendiz { get; set; }

        public ClInstructoresM instructor { get; set; }

        public List<ClPlanResultadosM> resultados { get; set; }

        public List<ClEvidenciaM> evidencias { get; set; }

        public List<ClEvaluacionM> evaluaciones { get; set; }
    }
}