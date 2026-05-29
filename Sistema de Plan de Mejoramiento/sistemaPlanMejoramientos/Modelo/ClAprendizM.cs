using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Modelo
{
    public class ClAprendizM
    {
        public int idAprendiz {  get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nombres {  get; set; }
        public string apellidos {  get; set; }
        public string correo {  get; set; }
        public string telefono {  get; set; }
        public string estadoAcademico {  get; set; }
        public int idUsuario {  get; set; }
    }
}