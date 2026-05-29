using System;
using System.Data;
using System.Data.SqlClient;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClProgramaD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearPrograma(string codigoPrograma, string nombre, string version,
                                    string nivel, string duracion, string estado, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"INSERT INTO programas (codigoPrograma, nombre, version, nivel, duracion, estado, idCentro)
                             VALUES (@codigoPrograma, @nombre, @version, @nivel, @duracion, @estado, @idCentro)";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoPrograma", codigoPrograma);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@version", version);
            cmd.Parameters.AddWithValue("@nivel", nivel);
            cmd.Parameters.AddWithValue("@duracion", duracion);
            cmd.Parameters.AddWithValue("@estado", estado);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarProgramas()
        {
            return MtListarProgramas("");
        }

        public DataTable MtListarProgramas(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"
                SELECT  p.idPrograma,
                        p.codigoPrograma,
                        p.nombre,
                        p.version,
                        p.nivel,
                        p.duracion,
                        p.estado,
                        p.idCentro,
                        c.nombre AS nombreCentro
                FROM    programas p
                INNER JOIN centros c ON c.idCentro = p.idCentro
                WHERE   (@filtro = ''
                         OR CAST(p.idPrograma       AS NVARCHAR) LIKE '%' + @filtro + '%'
                         OR CAST(p.codigoPrograma   AS NVARCHAR) LIKE '%' + @filtro + '%'
                         OR p.nombre                             LIKE '%' + @filtro + '%'
                         OR CAST(p.version          AS NVARCHAR) LIKE '%' + @filtro + '%'
                         OR p.nivel                              LIKE '%' + @filtro + '%'
                         OR p.estado                             LIKE '%' + @filtro + '%'
                         OR c.nombre                             LIKE '%' + @filtro + '%')
                ORDER BY p.idPrograma ASC";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtObtenerProgramaPorCodigo(string codigo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "select codigoPrograma FROM programas WHERE codigoPrograma = @codigoPrograma";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoPrograma", codigo);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }
        public DataTable MtObtenerProgramaPorId(int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"
                SELECT  p.idPrograma,
                        p.codigoPrograma,
                        p.nombre,
                        p.version,
                        p.nivel,
                        p.duracion,
                        p.estado,
                        p.idCentro,
                        c.nombre AS nombreCentro
                FROM    programas p
                INNER JOIN centros c ON c.idCentro = p.idCentro
                WHERE   p.idPrograma = @idPrograma";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarPrograma(int idPrograma, string codigoPrograma, string nombre,
                                         string version, string nivel, string duracion,
                                         string estado, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"UPDATE programas
                             SET codigoPrograma = @codigoPrograma,
                                 nombre         = @nombre,
                                 version        = @version,
                                 nivel          = @nivel,
                                 duracion       = @duracion,
                                 estado         = @estado,
                                 idCentro       = @idCentro
                             WHERE idPrograma = @idPrograma";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);
            cmd.Parameters.AddWithValue("@codigoPrograma", codigoPrograma);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@version", version);
            cmd.Parameters.AddWithValue("@nivel", nivel);
            cmd.Parameters.AddWithValue("@duracion", duracion);
            cmd.Parameters.AddWithValue("@estado", estado);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtEliminarPrograma(int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "DELETE FROM programas WHERE idPrograma = @idPrograma";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }
    }
}
