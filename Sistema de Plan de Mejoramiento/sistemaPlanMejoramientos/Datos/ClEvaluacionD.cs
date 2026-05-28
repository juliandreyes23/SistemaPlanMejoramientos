using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClEvaluacionD
    {
        ClConexion oConex = new ClConexion();

        public DataTable MtConsultarEvaluacionPorPlan(int idPlanMejoramiento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idEvaluacion, idPlanMejoramiento, criterioProducto,criterioConocimiento, criterioDesempeno, observaciones
            FROM evaluaciones
            WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }
    }
}