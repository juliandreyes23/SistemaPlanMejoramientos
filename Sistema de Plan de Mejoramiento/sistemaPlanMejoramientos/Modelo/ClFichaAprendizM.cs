using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClFichaAprendizM
    {
        public int idFicha { get; set; }

        public int idAprendiz { get; set; }

        public ClFichasM ficha { get; set; }

        public ClAprendizM aprendiz { get; set; }
    }
}