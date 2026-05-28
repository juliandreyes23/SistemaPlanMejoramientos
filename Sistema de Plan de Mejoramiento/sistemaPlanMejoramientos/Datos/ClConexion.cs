using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClConexion
    {
        SqlConnection oConex = new SqlConnection("Data Source=.;Initial Catalog=planMejoramiento;Integrated Security=True;Encrypt=False;");

        public SqlConnection MtAbrirConexion()
        {
            oConex.Open();
            return oConex;
        }
        public void MtCerrarConexion()
        {
            oConex.Close();
        }
    }
}