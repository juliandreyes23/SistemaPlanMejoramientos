using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClUsuarioM
    {
        public int idUsuario { get; set; }
        public string correo { get; set; }
        public string password { get; set; }
        public int idRol { get; set; }

        public ClRolM rol { get; set; }

        public ClAprendizM aprendiz { get; set; }

        public ClInstructoresM instructor { get; set; }

        public List<ClRecuperacionPasswordM> recuperaciones { get; set; }
    }
}