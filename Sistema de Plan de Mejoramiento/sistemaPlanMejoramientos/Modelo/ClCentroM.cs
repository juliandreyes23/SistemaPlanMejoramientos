using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClCentroM
    {
        public int idCentro { get; set; }
        public string codigoCentro { get; set; }
        public string nombre { get; set; }
        public string regional { get; set; }
        public string municipio { get; set; }
        public string departamento { get; set; }
        public string estado { get; set; }

        public List<ClProgramasM> programas { get; set; }
    }
}