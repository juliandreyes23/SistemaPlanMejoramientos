using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClAsignacionM
    {
        public int idFichaInstructor { get; set; }
        public string Instructor { get; set; }
        public string Ficha { get; set; }
        public string Programa { get; set; }
    }
}