using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClRolD
    {
        ClConexion oConex = new ClConexion();

        public DataTable MtListarRoles()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = "SELECT idRol, nombreRol FROM roles";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            oConex.MtCerrarConexion();

            return dt;
        }
    }
}