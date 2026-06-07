using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Data.SqlClient;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClEvaluacionD
    {
        ClConexion oConex = new ClConexion();

        public ClEvaluacionM MtConsultarEvaluacionPorPlan(int idPlanMejoramiento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idEvaluacion,
                                    idPlanMejoramiento,
                                    criterioProducto,
                                    criterioConocimiento,
                                    criterioDesempeno,
                                    observaciones
                             FROM evaluaciones
                             WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

            SqlDataReader rd = cmd.ExecuteReader();

            ClEvaluacionM evaluacion = null;

            if (rd.Read())
            {
                evaluacion = new ClEvaluacionM
                {
                    idEvaluacion = Convert.ToInt32(rd["idEvaluacion"]),
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    criterioProducto = rd["criterioProducto"].ToString(),
                    criterioConocimiento = rd["criterioConocimiento"].ToString(),
                    criterioDesempeno = rd["criterioDesempeno"].ToString(),
                    observaciones = rd["observaciones"].ToString()
                };
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return evaluacion;
        }
    }
}