using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClFichaInstructorM
    {
        public int idFichaInstructor { get; set; }

        public int idFicha { get; set; }

        public int idInstructor { get; set; }

        public ClFichasM ficha { get; set; }

        public ClInstructoresM instructor { get; set; }
    }
}