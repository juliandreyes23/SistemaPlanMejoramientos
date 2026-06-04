using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClConexion
    {
        SqlConnection oConex = new SqlConnection("Data Source=SistemaPlanMejoramiento.mssql.somee.com;Initial Catalog=SistemaPlanMejoramiento;User ID=JulianReyes_SQLLogin_1;Password=52y3bvedhs;Integrated Security=False;Encrypt=False;TrustServerCertificate=True;");
        //SqlConnection oConex = new SqlConnection("Data Source=.;Initial Catalog=planMejoramiento;Integrated Security=True;Encrypt=False;");

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