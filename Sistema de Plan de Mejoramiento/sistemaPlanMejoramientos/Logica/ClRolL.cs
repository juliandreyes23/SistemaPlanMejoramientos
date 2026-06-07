using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClRolL
    {
        ClRolD oRolD = new ClRolD();

        public List<ClRolM> MtListarRoles()
        {
            return oRolD.MtListarRoles();
        }
    }
}