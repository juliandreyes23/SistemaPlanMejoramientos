using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClCentroD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearCentro(string codigoCentro, string nombre, string regional, string municipio, string departamento, string estado)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO centros (codigoCentro, nombre, regional, municipio, departamento, estado)
                             VALUES (@codigoCentro, @nombre, @regional, @municipio, @departamento, @estado)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoCentro", codigoCentro);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@regional", regional);
            cmd.Parameters.AddWithValue("@municipio", municipio);
            cmd.Parameters.AddWithValue("@departamento", departamento);
            cmd.Parameters.AddWithValue("@estado", estado);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public DataTable MtListarCentros()
        {
            return MtListarCentros("");
        }

        public DataTable MtListarCentros(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro, codigoCentro, nombre, regional, municipio, departamento, estado
                             FROM centros
                             WHERE (@filtro = '' OR
                                    CAST(idCentro AS NVARCHAR)  LIKE '%' + @filtro + '%' OR
                                    codigoCentro                LIKE '%' + @filtro + '%' OR
                                    nombre                      LIKE '%' + @filtro + '%' OR
                                    regional                    LIKE '%' + @filtro + '%' OR
                                    municipio                   LIKE '%' + @filtro + '%' OR
                                    departamento                LIKE '%' + @filtro + '%' OR
                                    estado                      LIKE '%' + @filtro + '%')
                             ORDER BY idCentro ASC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarCentro(int idCentro, string codigoCentro, string nombre, string regional, string municipio, string departamento, string estado)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE centros
                             SET codigoCentro = @codigoCentro,
                                 nombre       = @nombre,
                                 regional     = @regional,
                                 municipio    = @municipio,
                                 departamento = @departamento,
                                 estado       = @estado
                             WHERE idCentro = @idCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);
            cmd.Parameters.AddWithValue("@codigoCentro", codigoCentro);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@regional", regional);
            cmd.Parameters.AddWithValue("@municipio", municipio);
            cmd.Parameters.AddWithValue("@departamento", departamento);
            cmd.Parameters.AddWithValue("@estado", estado);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public bool MtEliminarCentro(int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"DELETE FROM centros WHERE idCentro = @idCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public DataTable MtObtenerCentroPorId(int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro, codigoCentro, nombre, regional, municipio, departamento, estado
                             FROM centros
                             WHERE idCentro = @idCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtExisteCodigoCentro(string codigoCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(1) FROM centros WHERE codigoCentro = @codigoCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoCentro", codigoCentro);

            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();

            return Convert.ToInt32(resultado) > 0;
        }

        public int MtObtenerIdPorCodigo(string codigoCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro FROM centros WHERE codigoCentro = @codigoCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoCentro", codigoCentro);

            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();

            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        public DataTable MtListarCentrosActivos()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro, codigoCentro, nombre
                             FROM centros
                             WHERE estado = 'Activo'
                             ORDER BY nombre ASC";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }
    }
}