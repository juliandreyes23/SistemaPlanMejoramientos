using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClFichasM
    {
        public int idFicha {  get; set; }
        public string codigoFicha { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFinalizacion { get; set; }
        public string jornada { get; set; }
        public string estado { get; set; }
        public int idPrograma { get; set; }

    }
}