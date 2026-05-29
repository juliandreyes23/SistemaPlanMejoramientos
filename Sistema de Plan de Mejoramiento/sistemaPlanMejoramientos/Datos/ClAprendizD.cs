using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClAprendizD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearAprendiz(string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string estadoAcademico, int idUsuario, int idFicha, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO aprendices (tipoDocumento, numeroDocumento, nombres, apellidos, correo, telefono, estadoAcademico, idUsuario, idFicha, idCentro)
                    VALUES (@tipoDocumento, @numeroDocumento, @nombres, @apellidos, @correo, @telefono, @estadoAcademico, @idUsuario, @idFicha, @idCentro)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@tipoDocumento", tipoDocumento);
            cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
            cmd.Parameters.AddWithValue("@nombres", nombres);
            cmd.Parameters.AddWithValue("@apellidos", apellidos);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@estadoAcademico", estadoAcademico);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarAprendices()
        {
            return MtListarAprendices("");
        }

        public DataTable MtListarAprendices(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT a.idAprendiz, a.tipoDocumento, a.numeroDocumento, a.nombres, a.apellidos, 
                                    a.correo, a.telefono, a.estadoAcademico,
                                    ISNULL(CAST(f.codigoFicha AS NVARCHAR), 'Sin ficha') AS codigoFicha,
                                    ISNULL(u.correo, 'Sin usuario') AS CorreoUsuario,
                                    a.idFicha, a.idUsuario
                             FROM aprendices a
                             LEFT JOIN fichas f ON a.idFicha = f.idFicha
                             LEFT JOIN usuarios u ON a.idUsuario = u.idUsuario
                             WHERE (@filtro = '' OR
                                    CAST(a.idAprendiz AS NVARCHAR) LIKE '%' + @filtro + '%' OR
                                    a.tipoDocumento       LIKE '%' + @filtro + '%' OR
                                    a.numeroDocumento     LIKE '%' + @filtro + '%' OR
                                    a.nombres             LIKE '%' + @filtro + '%' OR
                                    a.apellidos           LIKE '%' + @filtro + '%' OR
                                    a.correo              LIKE '%' + @filtro + '%' OR
                                    a.telefono            LIKE '%' + @filtro + '%' OR
                                    a.estadoAcademico     LIKE '%' + @filtro + '%' OR
                                    CAST(f.codigoFicha AS NVARCHAR) LIKE '%' + @filtro + '%')";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarAprendiz(int idAprendiz, string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string estadoAcademico, int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE aprendices 
                     SET tipoDocumento = @tipoDocumento,
                         numeroDocumento = @numeroDocumento,
                         nombres = @nombres, 
                         apellidos = @apellidos, 
                         correo = @correo,
                         telefono = @telefono, 
                         estadoAcademico = @estadoAcademico, 
                         idFicha = @idFicha
                     WHERE idAprendiz = @idAprendiz";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            cmd.Parameters.AddWithValue("@tipoDocumento", tipoDocumento);
            cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
            cmd.Parameters.AddWithValue("@nombres", nombres);
            cmd.Parameters.AddWithValue("@apellidos", apellidos);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@estadoAcademico", estadoAcademico);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public bool MtEliminarAprendiz(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string queryIntermedia = "DELETE FROM fichaAprendiz WHERE idAprendiz = @idAprendiz";
            SqlCommand cmdIntermedia = new SqlCommand(queryIntermedia, cn);
            cmdIntermedia.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            cmdIntermedia.ExecuteNonQuery();

            string query = "DELETE FROM aprendices WHERE idAprendiz = @idAprendiz";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtRegistrarFichaIntermedia(int idFicha, int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO fichaAprendiz (idFicha, idAprendiz) VALUES (@idFicha, @idAprendiz)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public bool MtCargaMasivaAprendices(DataTable dtAprendicesExcel)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            bool resultado = false;

            SqlBulkCopy bulkCopy = new SqlBulkCopy(cn);
            bulkCopy.DestinationTableName = "dbo.aprendices";

            bulkCopy.ColumnMappings.Add("tipoDocumento", "tipoDocumento");
            bulkCopy.ColumnMappings.Add("numeroDocumento", "numeroDocumento");
            bulkCopy.ColumnMappings.Add("nombres", "nombres");
            bulkCopy.ColumnMappings.Add("apellidos", "apellidos");
            bulkCopy.ColumnMappings.Add("correo", "correo");
            bulkCopy.ColumnMappings.Add("telefono", "telefono");
            bulkCopy.ColumnMappings.Add("estadoAcademico", "estadoAcademico");
            bulkCopy.ColumnMappings.Add("idUsuario", "idUsuario");
            bulkCopy.ColumnMappings.Add("idFicha", "idFicha");
            bulkCopy.ColumnMappings.Add("idCentro", "idCentro");


            try
            {
                bulkCopy.WriteToServer(dtAprendicesExcel);
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }
            finally
            {
                bulkCopy.Close();
                oConex.MtCerrarConexion();
            }

            return resultado;
        }

        public int MtObtenerIdFichaPorCodigo(string codigoFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idFicha FROM fichas WHERE codigoFicha = @codigoFicha";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@codigoFicha", codigoFicha);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }


        public bool MtExisteUsuarioPorCorreo(string correo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT COUNT(1) FROM usuarios WHERE correo = @correo";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return Convert.ToInt32(resultado) > 0;
        }

        public int MtCrearUsuarioAprendiz(string correo, string contrasena)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO usuarios (correo, contrasena, idRol)
                             VALUES (@correo, @contrasena,
                                 (SELECT TOP 1 idRol FROM roles WHERE nombreRol = 'Aprendiz'))
                             SELECT SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@contrasena", contrasena);

            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }
        public int MtObtenerIdUsuarioPorAprendiz(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idUsuario FROM aprendices WHERE idAprendiz = @idAprendiz";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null && resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;
        }
        public int MtCrearAprendizConRetorno(string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string estadoAcademico, int idUsuario, int idFicha, int idCentro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO aprendices (tipoDocumento, numeroDocumento, nombres, apellidos, correo, telefono, estadoAcademico, idUsuario, idFicha, idCentro)
                    VALUES (@tipoDocumento, @numeroDocumento, @nombres, @apellidos, @correo, @telefono, @estadoAcademico, @idUsuario, @idFicha, @idCentro)
                    SELECT SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@tipoDocumento", tipoDocumento);
            cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
            cmd.Parameters.AddWithValue("@nombres", nombres);
            cmd.Parameters.AddWithValue("@apellidos", apellidos);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@telefono", telefono);
            cmd.Parameters.AddWithValue("@estadoAcademico", estadoAcademico);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            cmd.Parameters.AddWithValue("@idCentro", idCentro);

            object res = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return res != null ? Convert.ToInt32(res) : 0;
        }
        public bool MtExisteAprendiz(string numeroDocumento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT COUNT(1) FROM aprendices WHERE numeroDocumento = @doc";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@doc", numeroDocumento);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return Convert.ToInt32(resultado) > 0;
        }
    }
}