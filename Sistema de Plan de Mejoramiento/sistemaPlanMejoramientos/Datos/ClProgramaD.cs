using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
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

            string query = @"INSERT INTO programas
                            (codigoPrograma, nombre, version, nivel, duracion, estado, idCentro)
                             VALUES
                            (@codigoPrograma, @nombre, @version, @nivel, @duracion, @estado, @idCentro)";

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

        public List<ClProgramasM> MtListarProgramas()
        {
            return MtListarProgramas("");
        }

        public List<ClProgramasM> MtListarProgramas(string filtro)
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
                        c.nombre AS nombreCentro,
                        c.codigoCentro,
                        c.regional,
                        c.municipio,
                        c.departamento,
                        c.estado AS estadoCentro
                FROM programas p
                INNER JOIN centros c
                    ON c.idCentro = p.idCentro
                WHERE (@filtro = ''
                    OR CAST(p.idPrograma AS NVARCHAR) LIKE '%' + @filtro + '%'
                    OR CAST(p.codigoPrograma AS NVARCHAR) LIKE '%' + @filtro + '%'
                    OR p.nombre LIKE '%' + @filtro + '%'
                    OR CAST(p.version AS NVARCHAR) LIKE '%' + @filtro + '%'
                    OR p.nivel LIKE '%' + @filtro + '%'
                    OR p.estado LIKE '%' + @filtro + '%'
                    OR c.nombre LIKE '%' + @filtro + '%')
                ORDER BY p.idPrograma ASC";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClProgramasM> lista = new List<ClProgramasM>();

            while (rd.Read())
            {
                lista.Add(new ClProgramasM
                {
                    idPrograma = Convert.ToInt32(rd["idPrograma"]),
                    codigoPrograma = rd["codigoPrograma"].ToString(),
                    nombre = rd["nombre"].ToString(),
                    version = rd["version"].ToString(),
                    nivel = rd["nivel"].ToString(),
                    duracion = rd["duracion"].ToString(),
                    estado = rd["estado"].ToString(),
                    idCentro = Convert.ToInt32(rd["idCentro"]),

                    centro = new ClCentroM
                    {
                        idCentro = Convert.ToInt32(rd["idCentro"]),
                        codigoCentro = rd["codigoCentro"].ToString(),
                        nombre = rd["nombreCentro"].ToString(),
                        regional = rd["regional"].ToString(),
                        municipio = rd["municipio"].ToString(),
                        departamento = rd["departamento"].ToString(),
                        estado = rd["estadoCentro"].ToString()
                    }
                });
            }

            rd.Close();

            oConex.MtCerrarConexion();

            return lista;
        }

        public bool MtObtenerProgramaPorCodigo(string codigo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(*)
                             FROM programas
                             WHERE codigoPrograma = @codigoPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@codigoPrograma", codigo);

            int conteo = Convert.ToInt32(cmd.ExecuteScalar());

            oConex.MtCerrarConexion();

            return conteo > 0;
        }

        public ClProgramasM MtObtenerProgramaPorId(int idPrograma)
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
                        c.nombre AS nombreCentro,
                        c.codigoCentro,
                        c.regional,
                        c.municipio,
                        c.departamento,
                        c.estado AS estadoCentro
                FROM programas p
                INNER JOIN centros c
                    ON c.idCentro = p.idCentro
                WHERE p.idPrograma = @idPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);

            SqlDataReader rd = cmd.ExecuteReader();

            ClProgramasM programa = null;

            if (rd.Read())
            {
                programa = new ClProgramasM
                {
                    idPrograma = Convert.ToInt32(rd["idPrograma"]),
                    codigoPrograma = rd["codigoPrograma"].ToString(),
                    nombre = rd["nombre"].ToString(),
                    version = rd["version"].ToString(),
                    nivel = rd["nivel"].ToString(),
                    duracion = rd["duracion"].ToString(),
                    estado = rd["estado"].ToString(),
                    idCentro = Convert.ToInt32(rd["idCentro"]),

                    centro = new ClCentroM
                    {
                        idCentro = Convert.ToInt32(rd["idCentro"]),
                        codigoCentro = rd["codigoCentro"].ToString(),
                        nombre = rd["nombreCentro"].ToString(),
                        regional = rd["regional"].ToString(),
                        municipio = rd["municipio"].ToString(),
                        departamento = rd["departamento"].ToString(),
                        estado = rd["estadoCentro"].ToString()
                    }
                };
            }

            rd.Close();

            oConex.MtCerrarConexion();

            return programa;
        }

        public bool MtActualizarPrograma(int idPrograma, string codigoPrograma, string nombre,
                                         string version, string nivel, string duracion,
                                         string estado, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE programas
                             SET codigoPrograma = @codigoPrograma,
                                 nombre = @nombre,
                                 version = @version,
                                 nivel = @nivel,
                                 duracion = @duracion,
                                 estado = @estado,
                                 idCentro = @idCentro
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
            SqlTransaction tr = cn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("", cn, tr);
                cmd.Parameters.AddWithValue("@idPrograma", idPrograma);

                cmd.CommandText = @"DELETE pr FROM planResultados pr
                            INNER JOIN resultadoAprendizaje ra ON pr.idResultadoAprendizaje = ra.idResultadoAprendizaje
                            INNER JOIN competencias c ON ra.idCompetencia = c.idCompetencia
                            WHERE c.idPrograma = @idPrograma";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM planResultados
                            WHERE idPlanMejoramiento IN (
                                SELECT idPlanMejoramiento FROM planesMejoramiento
                                WHERE idAprendiz IN (
                                    SELECT idAprendiz FROM aprendices
                                    WHERE idFicha IN (
                                        SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                                    )
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evaluaciones
                            WHERE idPlanMejoramiento IN (
                                SELECT idPlanMejoramiento FROM planesMejoramiento
                                WHERE idAprendiz IN (
                                    SELECT idAprendiz FROM aprendices
                                    WHERE idFicha IN (
                                        SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                                    )
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evidencias
                            WHERE idPlanMejoramiento IN (
                                SELECT idPlanMejoramiento FROM planesMejoramiento
                                WHERE idAprendiz IN (
                                    SELECT idAprendiz FROM aprendices
                                    WHERE idFicha IN (
                                        SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                                    )
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM planesMejoramiento
                            WHERE idAprendiz IN (
                                SELECT idAprendiz FROM aprendices
                                WHERE idFicha IN (
                                    SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"UPDATE aprendices SET idUsuario = NULL
                            WHERE idFicha IN (
                                SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM aprendices
                            WHERE idFicha IN (
                                SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichaInstructor
                            WHERE idFicha IN (
                                SELECT idFicha FROM fichas WHERE idPrograma = @idPrograma
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE ra FROM resultadoAprendizaje ra
                            INNER JOIN competencias c ON ra.idCompetencia = c.idCompetencia
                            WHERE c.idPrograma = @idPrograma";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM competencias WHERE idPrograma = @idPrograma";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM fichas WHERE idPrograma = @idPrograma";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM programas WHERE idPrograma = @idPrograma";
                int filas = cmd.ExecuteNonQuery();

                tr.Commit();
                oConex.MtCerrarConexion();
                return filas > 0;
            }
            catch
            {
                tr.Rollback();
                oConex.MtCerrarConexion();
                return false;
            }
        }

        public bool MtObtenerProgramaPorCodigoExcluyendo(string codigo, int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(*)
                             FROM programas
                             WHERE codigoPrograma = @codigo
                             AND idPrograma <> @idPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@codigo", codigo);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);

            int conteo = Convert.ToInt32(cmd.ExecuteScalar());

            oConex.MtCerrarConexion();

            return conteo > 0;
        }
    }
}