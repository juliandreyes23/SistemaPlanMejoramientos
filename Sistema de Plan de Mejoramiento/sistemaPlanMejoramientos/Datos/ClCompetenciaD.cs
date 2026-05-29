using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClCompetenciaD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearCompetencia(string descripcion, int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO competencias (descripcion, idPrograma)
            VALUES (@descripcion, @idPrograma)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarCompetencias()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT c.idCompetencia,c.descripcion AS DescripcionCompetencia,
            p.idPrograma, p.nombre AS NombrePrograma, p.codigoPrograma
            FROM competencias c
            INNER JOIN programas p ON c.idPrograma = p.idPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            oConex.MtCerrarConexion();

            return dt;
        }

        public bool MtActualizarCompetencia (int idCompetencia, string descripcion, int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE competencias SET descripcion = @descripcion , idPrograma = @idPrograma
            WHERE idCompetencia = @idCompetencia";

            SqlCommand cmd = new SqlCommand (query, cn);
            cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }
        public bool MtEliminarCompetencia (int idCompetencia)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"DELETE FROM competencias WHERE idCompetencia = @idCompetencia";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public DataTable MtCargarCompetencias(int idPrograma)
        {

            ClConexion oConex = new ClConexion();
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idCompetencia, descripcion FROM competencias WHERE idPrograma = @id ORDER BY descripcion";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@id", idPrograma);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }
        public DataTable MtListaCompetencia()
        {

            ClConexion oConex = new ClConexion();
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idCompetencia, descripcion FROM competencias ORDER BY descripcion";
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }
    }
}