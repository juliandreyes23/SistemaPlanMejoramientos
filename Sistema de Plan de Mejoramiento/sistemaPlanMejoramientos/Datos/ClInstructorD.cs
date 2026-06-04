using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClInstructorD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearInstructor(string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string especialidad, int idUsuario, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO instructores (tipoDocumento, numeroDocumento, nombres, apellidos, correo, telefono, especialidad, idUsuario, idCentro)
            VALUES (@tipoDocumento, @numeroDocumento, @nombres, @apellidos, @correo, @telefono, @especialidad, @idUsuario, @idCentro)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@tipoDocumento", tipoDocumento);
            cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
            cmd.Parameters.AddWithValue("@nombres", nombres);
            cmd.Parameters.AddWithValue("@apellidos", apellidos);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@especialidad", especialidad);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarInstructores()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT i.idInstructor, i.tipoDocumento, i.numeroDocumento, i.nombres,
                             i.apellidos, i.correo, i.telefono, i.especialidad, u.idUsuario,
                             c.nombre AS centro, i.idCentro
                             FROM instructores i
                             INNER JOIN usuarios u ON i.idUsuario = u.idUsuario
                             INNER JOIN centros c ON i.idCentro = c.idCentro";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarInstructor(int idInstructor, string nombres, string apellidos, string correo, string telefono, string especialidad, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE instructores SET nombres = @nombres, apellidos = @apellidos,
                             correo = @correo, telefono = @telefono, especialidad = @especialidad,
                             idCentro = @idCentro
                             WHERE idInstructor = @idInstructor";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@nombres", nombres);
            cmd.Parameters.AddWithValue("@apellidos", apellidos);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@especialidad", especialidad);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtEliminarInstructor(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"DELETE FROM instructores WHERE idInstructor = @idInstructor";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtAsignarInstructorAFicha(int idInstructor, int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"INSERT INTO fichaInstructor (idInstructor, idFicha) VALUES (@idInstructor, @idFicha)";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }
        public DataTable MtListarCentros()
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idCentro, nombre FROM centros WHERE estado = 'Activo'";
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }
    }
}