using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using sistemaPlanMejoramientos.Modelo;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClAsignacionD
    {
        ClConexion oConex = new ClConexion();

        public List<ClInstructoresM> MtListarInstructoresCombo()
        {
            List<ClInstructoresM> lista = new List<ClInstructoresM>();

            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();

                string query = @"SELECT idInstructor,
                                        nombres,
                                        apellidos
                                 FROM instructores";

                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lista.Add(new ClInstructoresM
                    {
                        idInstructor = Convert.ToInt32(rd["idInstructor"]),
                        nombres = rd["nombres"].ToString(),
                        apellidos = rd["apellidos"].ToString()
                    });
                }

                rd.Close();
                oConex.MtCerrarConexion();
            }
            catch
            {
            }

            return lista;
        }

        public List<ClFichasM> MtListarFichasCombo()
        {
            List<ClFichasM> lista = new List<ClFichasM>();

            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();

                string query = @"SELECT f.idFicha,
                                        f.codigoFicha,
                                        f.jornada,
                                        p.idPrograma,
                                        p.nombre
                                 FROM fichas f
                                 INNER JOIN programas p
                                 ON f.idPrograma = p.idPrograma";

                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lista.Add(new ClFichasM
                    {
                        idFicha = Convert.ToInt32(rd["idFicha"]),
                        codigoFicha = rd["codigoFicha"].ToString(),
                        jornada = rd["jornada"].ToString(),
                        idPrograma = Convert.ToInt32(rd["idPrograma"]),
                        programa = new ClProgramasM
                        {
                            nombre = rd["nombre"].ToString()
                        }
                    });
                }

                rd.Close();
                oConex.MtCerrarConexion();
            }
            catch
            {
            }

            return lista;
        }

        public bool MtRegistrarAsignacion(int idInstructor, int idFicha)
        {
            bool insertado = false;

            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();

                string vQuery = "SELECT COUNT(*) FROM fichaInstructor WHERE idInstructor = @idIns AND idFicha = @idFic";

                SqlCommand vCmd = new SqlCommand(vQuery, cn);
                vCmd.Parameters.AddWithValue("@idIns", idInstructor);
                vCmd.Parameters.AddWithValue("@idFic", idFicha);

                int existe = (int)vCmd.ExecuteScalar();

                if (existe == 0)
                {
                    string query = "INSERT INTO fichaInstructor (idInstructor, idFicha) VALUES (@idIns, @idFic)";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idIns", idInstructor);
                    cmd.Parameters.AddWithValue("@idFic", idFicha);

                    int filas = cmd.ExecuteNonQuery();

                    insertado = filas > 0;
                }

                oConex.MtCerrarConexion();
            }
            catch
            {
            }

            return insertado;
        }

        public List<ClFichaInstructorM> MtListarAsignaciones()
        {
            List<ClFichaInstructorM> lista = new List<ClFichaInstructorM>();

            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();

                string query = @"SELECT FI.idFichaInstructor,
                                        FI.idInstructor,
                                        FI.idFicha,
                                        I.nombres,
                                        I.apellidos,
                                        F.codigoFicha,
                                        P.nombre
                                 FROM fichaInstructor FI
                                 INNER JOIN instructores I
                                 ON FI.idInstructor = I.idInstructor
                                 INNER JOIN fichas F
                                 ON FI.idFicha = F.idFicha
                                 INNER JOIN programas P
                                 ON F.idPrograma = P.idPrograma";

                SqlCommand cmd = new SqlCommand(query, cn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lista.Add(new ClFichaInstructorM
                    {
                        idFichaInstructor = Convert.ToInt32(rd["idFichaInstructor"]),
                        idInstructor = Convert.ToInt32(rd["idInstructor"]),
                        idFicha = Convert.ToInt32(rd["idFicha"]),

                        instructor = new ClInstructoresM
                        {
                            nombres = rd["nombres"].ToString(),
                            apellidos = rd["apellidos"].ToString()
                        },

                        ficha = new ClFichasM
                        {
                            codigoFicha = rd["codigoFicha"].ToString(),

                            programa = new ClProgramasM
                            {
                                nombre = rd["nombre"].ToString()
                            }
                        }
                    });
                }

                rd.Close();
                oConex.MtCerrarConexion();
            }
            catch
            {
            }

            return lista;
        }

        public bool MtEliminarAsignacion(int idFichaInstructor)
        {
            bool eliminado = false;

            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();

                string query = "DELETE FROM fichaInstructor WHERE idFichaInstructor = @id";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@id", idFichaInstructor);

                int filas = cmd.ExecuteNonQuery();

                eliminado = filas > 0;

                oConex.MtCerrarConexion();
            }
            catch
            {
            }

            return eliminado;
        }
    }
}