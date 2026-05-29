using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClResultadoAprendizajeD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearResultado(string descripcion, int idCompetencia)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO resultadoAprendizaje (descripcion,idCompetencia)
            VALUES (@descripcion, @idCompetencia)";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public DataTable MtListarResultadoAprendizaje()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT r.idResultadoAprendizaje, r.descripcion AS DescripcionResultado, 
                                    c.idCompetencia, c.descripcion AS DescripcionCompetencia,
                                    p.idPrograma, p.nombre AS NombrePrograma
                             FROM resultadoAprendizaje r
                             INNER JOIN competencias c ON r.idCompetencia = c.idCompetencia
                             INNER JOIN programas p ON c.idPrograma = p.idPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();

            return dt;
        }
        public bool MtActualizarResultado(int idResultadoAprendizaje,string descripcion, int idCompetencia)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE resultadoAprendizaje SET descripcion = @descripcion, idCompetencia = @idCompetencia WHERE idResultadoAprendizaje = @idResultadoAprendizaje";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idResultadoAprendizaje", idResultadoAprendizaje);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idCompetencia" , idCompetencia);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public bool MtEliminarResultado(int idResultadoAprendizaje)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"DELETE FROM resultadoAprendizaje WHERE idResultadoAprendizaje = @idResultadoAprendizaje";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idResultadoAprendizaje", idResultadoAprendizaje);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }
    }
}