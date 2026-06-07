using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using sistemaPlanMejoramientos.Modelo;

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

        public List<ClCentroM> MtListarCentros()
        {
            return MtListarCentros("");
        }

        public List<ClCentroM> MtListarCentros(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro, codigoCentro, nombre, regional, municipio, departamento, estado
                             FROM centros
                             WHERE (@filtro = '' OR
                                    CAST(idCentro AS NVARCHAR) LIKE '%' + @filtro + '%' OR
                                    codigoCentro LIKE '%' + @filtro + '%' OR
                                    nombre LIKE '%' + @filtro + '%' OR
                                    regional LIKE '%' + @filtro + '%' OR
                                    municipio LIKE '%' + @filtro + '%' OR
                                    departamento LIKE '%' + @filtro + '%' OR
                                    estado LIKE '%' + @filtro + '%')
                             ORDER BY idCentro ASC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClCentroM> lista = new List<ClCentroM>();

            while (rd.Read())
            {
                lista.Add(new ClCentroM
                {
                    idCentro = Convert.ToInt32(rd["idCentro"]),
                    codigoCentro = rd["codigoCentro"].ToString(),
                    nombre = rd["nombre"].ToString(),
                    regional = rd["regional"].ToString(),
                    municipio = rd["municipio"].ToString(),
                    departamento = rd["departamento"].ToString(),
                    estado = rd["estado"].ToString()
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public bool MtActualizarCentro(int idCentro, string codigoCentro, string nombre, string regional, string municipio, string departamento, string estado)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE centros
                             SET codigoCentro = @codigoCentro,
                                 nombre = @nombre,
                                 regional = @regional,
                                 municipio = @municipio,
                                 departamento = @departamento,
                                 estado = @estado
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
            SqlTransaction transaction = cn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("", cn, transaction);
                cmd.Parameters.AddWithValue("@idCentro", idCentro);

                cmd.CommandText = @"DELETE FROM planResultados
                    WHERE idPlanMejoramiento IN (
                        SELECT idPlanMejoramiento FROM planesMejoramiento
                        WHERE idAprendiz IN (
                            SELECT idAprendiz FROM aprendices WHERE idCentro = @idCentro
                        )
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evaluaciones
                    WHERE idPlanMejoramiento IN (
                        SELECT idPlanMejoramiento FROM planesMejoramiento
                        WHERE idAprendiz IN (
                            SELECT idAprendiz FROM aprendices WHERE idCentro = @idCentro
                        )
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evidencias
                    WHERE idPlanMejoramiento IN (
                        SELECT idPlanMejoramiento FROM planesMejoramiento
                        WHERE idAprendiz IN (
                            SELECT idAprendiz FROM aprendices WHERE idCentro = @idCentro
                        )
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM planesMejoramiento
                    WHERE idAprendiz IN (
                        SELECT idAprendiz FROM aprendices WHERE idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM planResultados
                    WHERE idResultadoAprendizaje IN (
                        SELECT ra.idResultadoAprendizaje
                        FROM resultadoAprendizaje ra
                        INNER JOIN competencias c ON ra.idCompetencia = c.idCompetencia
                        INNER JOIN programas p ON c.idPrograma = p.idPrograma
                        WHERE p.idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM resultadoAprendizaje
                    WHERE idCompetencia IN (
                        SELECT c.idCompetencia
                        FROM competencias c
                        INNER JOIN programas p ON c.idPrograma = p.idPrograma
                        WHERE p.idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM competencias
                    WHERE idPrograma IN (
                        SELECT idPrograma FROM programas WHERE idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichaAprendiz
                    WHERE idFicha IN (
                        SELECT idFicha FROM fichas WHERE idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichaAprendiz
                    WHERE idAprendiz IN (
                        SELECT idAprendiz FROM aprendices WHERE idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichaInstructor
                    WHERE idFicha IN (
                        SELECT idFicha FROM fichas WHERE idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichaInstructor
                    WHERE idInstructor IN (
                        SELECT idInstructor FROM instructores WHERE idCentro = @idCentro
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichas WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM programas WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM recuperacionPassword
                    WHERE idUsuario IN (
                        SELECT idUsuario FROM aprendices WHERE idCentro = @idCentro AND idUsuario IS NOT NULL
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM recuperacionPassword
                    WHERE idUsuario IN (
                        SELECT idUsuario FROM instructores WHERE idCentro = @idCentro AND idUsuario IS NOT NULL
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM usuarios
                    WHERE idUsuario IN (
                        SELECT idUsuario FROM aprendices WHERE idCentro = @idCentro AND idUsuario IS NOT NULL
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM usuarios
                    WHERE idUsuario IN (
                        SELECT idUsuario FROM instructores WHERE idCentro = @idCentro AND idUsuario IS NOT NULL
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"UPDATE aprendices SET idUsuario = NULL WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"UPDATE instructores SET idUsuario = NULL WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM aprendices WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM instructores WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM administradores WHERE idCentro = @idCentro";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM centros WHERE idCentro = @idCentro";
                int filas = cmd.ExecuteNonQuery();

                transaction.Commit();
                return filas > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                oConex.MtCerrarConexion();
            }
        }

        public ClCentroM MtObtenerCentroPorId(int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro, codigoCentro, nombre, regional, municipio, departamento, estado
                             FROM centros
                             WHERE idCentro = @idCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);

            SqlDataReader rd = cmd.ExecuteReader();

            ClCentroM centro = null;

            if (rd.Read())
            {
                centro = new ClCentroM
                {
                    idCentro = Convert.ToInt32(rd["idCentro"]),
                    codigoCentro = rd["codigoCentro"].ToString(),
                    nombre = rd["nombre"].ToString(),
                    regional = rd["regional"].ToString(),
                    municipio = rd["municipio"].ToString(),
                    departamento = rd["departamento"].ToString(),
                    estado = rd["estado"].ToString()
                };
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return centro;
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

        public List<ClCentroM> MtListarCentrosActivos()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCentro, codigoCentro, nombre
                             FROM centros
                             WHERE estado = 'Activo'
                             ORDER BY nombre ASC";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClCentroM> lista = new List<ClCentroM>();

            while (rd.Read())
            {
                lista.Add(new ClCentroM
                {
                    idCentro = Convert.ToInt32(rd["idCentro"]),
                    codigoCentro = rd["codigoCentro"].ToString(),
                    nombre = rd["nombre"].ToString()
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }
    }
}