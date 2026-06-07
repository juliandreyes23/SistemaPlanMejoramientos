using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using sistemaPlanMejoramientos.Modelo;

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

        public List<ClCompetenciasM> MtListarCompetencias()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT c.idCompetencia,
                                    c.descripcion,
                                    p.idPrograma,
                                    p.nombre,
                                    p.codigoPrograma
                             FROM competencias c
                             INNER JOIN programas p
                             ON c.idPrograma = p.idPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClCompetenciasM> lista = new List<ClCompetenciasM>();

            while (rd.Read())
            {
                lista.Add(new ClCompetenciasM
                {
                    idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                    descripcion = rd["descripcion"].ToString(),
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

        public bool MtActualizarCompetencia(int idCompetencia, string descripcion, int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE competencias
                             SET descripcion = @descripcion,
                                 idPrograma = @idPrograma
                             WHERE idCompetencia = @idCompetencia";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idPrograma", idPrograma);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public bool MtEliminarCompetencia(int idCompetencia)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            SqlTransaction transaction = cn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("", cn, transaction);
                cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);

                cmd.CommandText = @"DELETE FROM planResultados
                                    WHERE idResultadoAprendizaje IN (
                                        SELECT idResultadoAprendizaje
                                        FROM resultadoAprendizaje
                                        WHERE idCompetencia = @idCompetencia
                                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM resultadoAprendizaje
                                    WHERE idCompetencia = @idCompetencia";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM competencias
                                    WHERE idCompetencia = @idCompetencia";

                int filas = cmd.ExecuteNonQuery();

                transaction.Commit();

                return filas > 0;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                oConex.MtCerrarConexion();
            }
        }

        public List<ClCompetenciasM> MtCargarCompetencias(int idPrograma)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCompetencia,
                                    descripcion,
                                    idPrograma
                             FROM competencias
                             WHERE idPrograma = @id
                             ORDER BY descripcion";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@id", idPrograma);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClCompetenciasM> lista = new List<ClCompetenciasM>();

            while (rd.Read())
            {
                lista.Add(new ClCompetenciasM
                {
                    idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                    descripcion = rd["descripcion"].ToString(),
                    idPrograma = Convert.ToInt32(rd["idPrograma"])
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public List<ClCompetenciasM> MtListaCompetencia()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idCompetencia,
                                    descripcion,
                                    idPrograma
                             FROM competencias
                             ORDER BY descripcion";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClCompetenciasM> lista = new List<ClCompetenciasM>();

            while (rd.Read())
            {
                lista.Add(new ClCompetenciasM
                {
                    idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                    descripcion = rd["descripcion"].ToString(),
                    idPrograma = rd["idPrograma"] != DBNull.Value
                        ? Convert.ToInt32(rd["idPrograma"])
                        : 0
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public List<ClCompetenciasM> MtBuscarCompetencias(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT c.idCompetencia,
                                    c.descripcion,
                                    p.idPrograma,
                                    p.nombre,
                                    p.codigoPrograma
                             FROM competencias c
                             INNER JOIN programas p
                             ON c.idPrograma = p.idPrograma
                             WHERE c.descripcion LIKE @filtro
                             OR p.nombre LIKE @filtro
                             OR p.codigoPrograma LIKE @filtro";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClCompetenciasM> lista = new List<ClCompetenciasM>();

            while (rd.Read())
            {
                lista.Add(new ClCompetenciasM
                {
                    idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                    descripcion = rd["descripcion"].ToString(),
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
    }
}