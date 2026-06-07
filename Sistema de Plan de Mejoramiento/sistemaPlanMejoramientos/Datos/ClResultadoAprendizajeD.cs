using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClResultadoAprendizajeD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearResultado(string descripcion, int idCompetencia)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO resultadoAprendizaje
                            (descripcion, idCompetencia)
                             VALUES
                            (@descripcion, @idCompetencia)";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public List<ClResultadoAprendizajeM> MtListarResultadoAprendizaje()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
                SELECT r.idResultadoAprendizaje,
                       r.descripcion AS DescripcionResultado,
                       c.idCompetencia,
                       c.descripcion AS DescripcionCompetencia,
                       p.idPrograma,
                       p.nombre AS NombrePrograma,
                       p.codigoPrograma,
                       p.version,
                       p.nivel,
                       p.duracion,
                       p.estado
                FROM resultadoAprendizaje r
                INNER JOIN competencias c
                    ON r.idCompetencia = c.idCompetencia
                INNER JOIN programas p
                    ON c.idPrograma = p.idPrograma";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClResultadoAprendizajeM> lista = new List<ClResultadoAprendizajeM>();

            while (rd.Read())
            {
                lista.Add(new ClResultadoAprendizajeM
                {
                    idResultadoAprendizaje = Convert.ToInt32(rd["idResultadoAprendizaje"]),
                    descripcion = rd["DescripcionResultado"].ToString(),
                    nombreCompetencia = rd["DescripcionCompetencia"].ToString(),
                    idCompetencia = Convert.ToInt32(rd["idCompetencia"]),

                    competencia = new ClCompetenciasM
                    {
                        idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                        descripcion = rd["DescripcionCompetencia"].ToString(),
                        idPrograma = Convert.ToInt32(rd["idPrograma"]),

                        programa = new ClProgramasM
                        {
                            idPrograma = Convert.ToInt32(rd["idPrograma"]),
                            nombre = rd["NombrePrograma"].ToString(),
                            codigoPrograma = rd["codigoPrograma"].ToString(),
                            version = rd["version"].ToString(),
                            nivel = rd["nivel"].ToString(),
                            duracion = rd["duracion"].ToString(),
                            estado = rd["estado"].ToString()
                        }
                    }
                });
            }

            rd.Close();

            oConex.MtCerrarConexion();

            return lista;
        }

        public bool MtActualizarResultado(int idResultadoAprendizaje, string descripcion, int idCompetencia)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE resultadoAprendizaje
                             SET descripcion = @descripcion,
                                 idCompetencia = @idCompetencia
                             WHERE idResultadoAprendizaje = @idResultadoAprendizaje";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idResultadoAprendizaje", idResultadoAprendizaje);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@idCompetencia", idCompetencia);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public bool MtEliminarResultado(int idResultadoAprendizaje)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"DELETE FROM resultadoAprendizaje
                             WHERE idResultadoAprendizaje = @idResultadoAprendizaje";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idResultadoAprendizaje", idResultadoAprendizaje);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public List<ClProgramasM> MtCargarPrograma()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idPrograma,
                                    nombre,
                                    codigoPrograma,
                                    version,
                                    nivel,
                                    duracion,
                                    estado
                             FROM programas
                             ORDER BY nombre";

            SqlCommand cmd = new SqlCommand(query, cn);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClProgramasM> lista = new List<ClProgramasM>();

            while (rd.Read())
            {
                lista.Add(new ClProgramasM
                {
                    idPrograma = Convert.ToInt32(rd["idPrograma"]),
                    nombre = rd["nombre"].ToString(),
                    codigoPrograma = rd["codigoPrograma"].ToString(),
                    version = rd["version"].ToString(),
                    nivel = rd["nivel"].ToString(),
                    duracion = rd["duracion"].ToString(),
                    estado = rd["estado"].ToString()
                });
            }

            rd.Close();

            oConex.MtCerrarConexion();

            return lista;
        }
    }
}