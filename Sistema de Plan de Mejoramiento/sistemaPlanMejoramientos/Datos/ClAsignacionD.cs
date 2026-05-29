using System;
using System.Data;
using System.Data.SqlClient;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClAsignacionD
    {
        ClConexion oConex = new ClConexion();

        public DataTable MtListarInstructoresCombo()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = "SELECT idInstructor, (nombres + ' ' + apellidos) AS NombreCompleto FROM instructores";
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.Fill(dt);
                oConex.MtCerrarConexion();
            }
            catch (Exception) { }
            return dt;
        }

        public DataTable MtListarFichasCombo()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = @"SELECT f.idFicha, 
                                        f.codigoFicha + ' - ' + p.nombre + ' (' + f.jornada + ')' AS TextoFicha
                                 FROM fichas f
                                 INNER JOIN programas p ON f.idPrograma = p.idPrograma";
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.Fill(dt);
                oConex.MtCerrarConexion();
            }
            catch (Exception) { }
            return dt;
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
                    insertado = (filas > 0);
                }
                oConex.MtCerrarConexion();
            }
            catch (Exception) { }
            return insertado;
        }

        public DataTable MtListarAsignaciones()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = @"SELECT FI.idFichaInstructor, 
                                        (I.nombres + ' ' + I.apellidos) AS Instructor, 
                                        F.codigoFicha AS Ficha, 
                                        P.nombre AS Programa
                                 FROM fichaInstructor FI
                                 INNER JOIN instructores I ON FI.idInstructor = I.idInstructor
                                 INNER JOIN fichas F       ON FI.idFicha      = F.idFicha
                                 INNER JOIN programas P    ON F.idPrograma    = P.idPrograma";
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.Fill(dt);
                oConex.MtCerrarConexion();
            }
            catch (Exception) { }
            return dt;
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
                eliminado = (filas > 0);
                oConex.MtCerrarConexion();
            }
            catch (Exception) { }
            return eliminado;
        }
    }
}