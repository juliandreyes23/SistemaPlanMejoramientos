using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClEvidenciasM
    {
        public int idEvidencia {  get; set; }
        public int idPlanMejoramiento { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime fechaSubida {  get; set; }
        public string tipoArchivo { get; set; }
    }
}