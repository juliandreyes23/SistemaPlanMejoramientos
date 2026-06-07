using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClRolD
    {
        ClConexion oConex = new ClConexion();

        public List<ClRolM> MtListarRoles()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idRol, nombreRol
                             FROM roles";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClRolM> lista = new List<ClRolM>();

            while (rd.Read())
            {
                lista.Add(new ClRolM
                {
                    idRol = Convert.ToInt32(rd["idRol"]),
                    nombreRol = rd["nombreRol"].ToString()
                });
            }

            rd.Close();

            oConex.MtCerrarConexion();

            return lista;
        }
    }
}