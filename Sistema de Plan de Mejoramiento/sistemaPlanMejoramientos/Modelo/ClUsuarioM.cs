using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClUsuarioM
    {
        public int idUsuario {  get; set; }
        public string correo {  get; set; }
        public string password { get; set; }
        public int idRol {  get; set; }

    }
}