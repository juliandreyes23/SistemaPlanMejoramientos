using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using sistemaPlanMejoramientos.Modelo;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClFichaD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearFicha(string codigoFicha, DateTime fechaInicio, DateTime fechaFinalizacion,
                          string jornada, string estado, int idPrograma, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO fichas
                            (codigoFicha, fechaInicio, fechaFinalizacion, jornada, estado, idPrograma, idCentro)
                             VALUES
                            (@codigoFicha, @fechaInicio, @fechaFinalizacion, @jornada, @estado, @idPrograma, @idCentro)";

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

        public List<ClFichasM> MtListarFichas()
        {
            return MtListarFichas("");
        }

        public List<ClFichasM> MtListarFichas(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT f.idFicha,
                                    f.codigoFicha,
                                    f.fechaInicio,
                                    f.fechaFinalizacion,
                                    f.jornada,
                                    f.estado,
                                    f.idPrograma,
                                    p.nombre,
                                    p.codigoPrograma
                             FROM fichas f
                             INNER JOIN programas p
                             ON f.idPrograma = p.idPrograma
                             WHERE (@filtro = '' OR
                                    CAST(f.idFicha AS NVARCHAR) LIKE '%' + @filtro + '%' OR
                                    CAST(f.codigoFicha AS NVARCHAR) LIKE '%' + @filtro + '%' OR
                                    f.jornada LIKE '%' + @filtro + '%' OR
                                    f.estado LIKE '%' + @filtro + '%' OR
                                    p.nombre LIKE '%' + @filtro + '%' OR
                                    CAST(p.codigoPrograma AS NVARCHAR) LIKE '%' + @filtro + '%')";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClFichasM> lista = new List<ClFichasM>();

            while (rd.Read())
            {
                lista.Add(new ClFichasM
                {
                    idFicha = Convert.ToInt32(rd["idFicha"]),
                    codigoFicha = rd["codigoFicha"].ToString(),
                    fechaInicio = Convert.ToDateTime(rd["fechaInicio"]),
                    fechaFinalizacion = Convert.ToDateTime(rd["fechaFinalizacion"]),
                    jornada = rd["jornada"].ToString(),
                    estado = rd["estado"].ToString(),
                    idPrograma = Convert.ToInt32(rd["idPrograma"]),
                    programa = new ClProgramasM
                    {
                        nombre = rd["nombre"].ToString(),
                        codigoPrograma = rd["codigoPrograma"].ToString()
                    }
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public bool MtActualizarFicha(int idFicha, string codigoFicha, DateTime fechaInicio, DateTime fechaFinalizacion, string jornada, string estado, int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE fichas
                             SET codigoFicha = @codigoFicha,
                                 fechaInicio = @fechaInicio,
                                 fechaFinalizacion = @fechaFinalizacion,
                                 jornada = @jornada,
                                 estado = @estado,
                                 idPrograma = @idPrograma
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
            SqlTransaction tr = cn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("", cn, tr);
                cmd.Parameters.AddWithValue("@idFicha", idFicha);

                cmd.CommandText = @"DELETE FROM planResultados
                            WHERE idPlanMejoramiento IN (
                                SELECT idPlanMejoramiento FROM planesMejoramiento
                                WHERE idAprendiz IN (
                                    SELECT idAprendiz FROM aprendices WHERE idFicha = @idFicha
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evaluaciones
                            WHERE idPlanMejoramiento IN (
                                SELECT idPlanMejoramiento FROM planesMejoramiento
                                WHERE idAprendiz IN (
                                    SELECT idAprendiz FROM aprendices WHERE idFicha = @idFicha
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evidencias
                            WHERE idPlanMejoramiento IN (
                                SELECT idPlanMejoramiento FROM planesMejoramiento
                                WHERE idAprendiz IN (
                                    SELECT idAprendiz FROM aprendices WHERE idFicha = @idFicha
                                )
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM planesMejoramiento
                            WHERE idAprendiz IN (
                                SELECT idAprendiz FROM aprendices WHERE idFicha = @idFicha
                            )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"UPDATE aprendices SET idUsuario = NULL WHERE idFicha = @idFicha";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM aprendices WHERE idFicha = @idFicha";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM fichaInstructor WHERE idFicha = @idFicha";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM fichas WHERE idFicha = @idFicha";
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

        public List<ClFichasM> MtListarFichasPorInstructor(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT f.idFicha,
                                    f.codigoFicha,
                                    f.fechaInicio,
                                    f.fechaFinalizacion,
                                    f.jornada,
                                    f.estado,
                                    f.idPrograma,
                                    p.nombre,
                                    p.codigoPrograma
                             FROM fichas f
                             INNER JOIN programas p
                             ON f.idPrograma = p.idPrograma
                             INNER JOIN fichaInstructor fi
                             ON f.idFicha = fi.idFicha
                             WHERE fi.idInstructor = @idInstructor";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClFichasM> lista = new List<ClFichasM>();

            while (rd.Read())
            {
                lista.Add(new ClFichasM
                {
                    idFicha = Convert.ToInt32(rd["idFicha"]),
                    codigoFicha = rd["codigoFicha"].ToString(),
                    fechaInicio = Convert.ToDateTime(rd["fechaInicio"]),
                    fechaFinalizacion = Convert.ToDateTime(rd["fechaFinalizacion"]),
                    jornada = rd["jornada"].ToString(),
                    estado = rd["estado"].ToString(),
                    idPrograma = Convert.ToInt32(rd["idPrograma"]),
                    programa = new ClProgramasM
                    {
                        nombre = rd["nombre"].ToString(),
                        codigoPrograma = rd["codigoPrograma"].ToString()
                    }
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public List<ClAprendizM> MtListarAprendicesPorFicha(int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT a.idAprendiz,
       a.nombres,
       a.apellidos,
       a.tipoDocumento,
       a.numeroDocumento,
       a.correo,
       a.telefono,
       a.estadoAcademico
FROM aprendices a
WHERE a.idFicha = @idFicha
ORDER BY a.apellidos";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idFicha", idFicha);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClAprendizM> lista = new List<ClAprendizM>();

            while (rd.Read())
            {
                lista.Add(new ClAprendizM
                {
                    idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                    nombres = rd["nombres"].ToString(),
                    apellidos = rd["apellidos"].ToString(),
                    tipoDocumento = rd["tipoDocumento"].ToString(),
                    numeroDocumento = rd["numeroDocumento"].ToString(),
                    correo = rd["correo"].ToString(),
                    telefono = rd["telefono"].ToString(),
                    estadoAcademico = rd["estadoAcademico"].ToString()
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
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
                             INNER JOIN programas p
                             ON f.idPrograma = p.idPrograma
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