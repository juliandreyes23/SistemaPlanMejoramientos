using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClFichaD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearFicha(string codigoFicha, DateTime fechaInicio, DateTime fechaFinalizacion,
                          string jornada, string estado, int idPrograma, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"INSERT INTO fichas (codigoFicha, fechaInicio, fechaFinalizacion, jornada, estado, idPrograma, idCentro)
                     VALUES (@codigoFicha, @fechaInicio, @fechaFinalizacion, @jornada, @estado, @idPrograma, @idCentro)";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoFicha", codigoFicha);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFinalizacion", fechaFinalizacion);
            cmd.Parameters.AddWithValue("@jornada", jornada);
            cmd.Parameters.AddWithValue("@estado", estado);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarFichas()
        {
            return MtListarFichas("");
        }

        public DataTable MtListarFichas(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT f.idFicha, f.codigoFicha, f.fechaInicio, f.fechaFinalizacion, f.jornada, f.estado,
                                    p.nombre AS nombrePrograma, p.codigoPrograma
                             FROM fichas f
                             INNER JOIN programas p ON f.idPrograma = p.idPrograma
                             WHERE (@filtro = '' OR
                                    CAST(f.idFicha AS NVARCHAR)        LIKE '%' + @filtro + '%' OR
                                    CAST(f.codigoFicha AS NVARCHAR)    LIKE '%' + @filtro + '%' OR
                                    f.jornada                          LIKE '%' + @filtro + '%' OR
                                    f.estado                           LIKE '%' + @filtro + '%' OR
                                    p.nombre                           LIKE '%' + @filtro + '%' OR
                                    CAST(p.codigoPrograma AS NVARCHAR) LIKE '%' + @filtro + '%')";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarFicha(int idFicha, string codigoFicha, DateTime fechaInicio, DateTime fechaFinalizacion, string jornada, string estado, int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"UPDATE fichas SET codigoFicha = @codigoFicha, fechaInicio = @fechaInicio,
                             fechaFinalizacion = @fechaFinalizacion, jornada = @jornada, estado = @estado, idPrograma = @idPrograma
                             WHERE idFicha = @idFicha";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            cmd.Parameters.AddWithValue("@codigoFicha", codigoFicha);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFinalizacion", fechaFinalizacion);
            cmd.Parameters.AddWithValue("@jornada", jornada);
            cmd.Parameters.AddWithValue("@estado", estado);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtEliminarFicha(int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "DELETE FROM fichas WHERE idFicha = @idFicha";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarFichasPorInstructor(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"
                SELECT f.idFicha, f.codigoFicha, f.fechaInicio, f.fechaFinalizacion,
                       f.jornada, f.estado,
                       p.nombre AS nombrePrograma, p.codigoPrograma
                FROM fichas f
                INNER JOIN programas p        ON f.idPrograma = p.idPrograma
                INNER JOIN fichaInstructor fi ON f.idFicha    = fi.idFicha
                WHERE fi.idInstructor = @idInstructor";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public DataTable MtListarAprendicesPorFicha(int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"
                SELECT a.idAprendiz, a.nombres, a.apellidos,
                       a.tipoDocumento, a.numeroDocumento,
                       a.correo, a.telefono, a.estadoAcademico
                FROM aprendices a
                INNER JOIN fichaAprendiz fa ON a.idAprendiz = fa.idAprendiz
                WHERE fa.idFicha = @idFicha
                ORDER BY a.apellidos";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public int MtContarFichasPorInstructor(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT COUNT(*) FROM fichaInstructor WHERE idInstructor = @idInstructor";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            int total = (int)cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return total;
        }
        public int MtObtenerIdCentroPorFicha(int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT p.idCentro 
                     FROM fichas f
                     INNER JOIN programas p ON f.idPrograma = p.idPrograma
                     WHERE f.idFicha = @idFicha";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }
        public int MtObtenerIdCentroPorPrograma(int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idCentro FROM programas WHERE idPrograma = @idPrograma";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        public bool MtExisteFicha(string codigoFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = "SELECT COUNT(*) FROM fichas WHERE codigoFicha = @codigoFicha";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoFicha", codigoFicha);

            int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

            oConex.MtCerrarConexion();

            return cantidad > 0;
        }

        public bool MtExisteFichaEditar(int idFicha, string codigoFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(*) 
                     FROM fichas 
                     WHERE codigoFicha = @codigoFicha
                     AND idFicha <> @idFicha";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoFicha", codigoFicha);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);

            int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

            oConex.MtCerrarConexion();

            return cantidad > 0;
        }
    }
}