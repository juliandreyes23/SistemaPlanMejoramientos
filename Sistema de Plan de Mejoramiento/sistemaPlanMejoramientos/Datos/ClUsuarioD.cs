using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClUsuarioD
    {
        ClConexion oConex = new ClConexion();

        public bool MtCrearUsuario(string correo, string password, int idRol)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"INSERT INTO usuarios (correo, password, idRol)
                             VALUES (@correo, @password, @idRol)";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@password", MtEncriptarCadena(password));
            cmd.Parameters.AddWithValue("@idRol", idRol);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public int CrearUsuarioInstructor(string correo, string documento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"INSERT INTO usuarios (correo, password, idRol) 
                     OUTPUT INSERTED.idUsuario 
                     VALUES (@correo, @pass, 2)";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@pass", MtEncriptarCadena(documento));
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        public DataTable MtListarUsuarios()
        {
            return MtListarUsuarios("");
        }

        public DataTable MtListarUsuarios(string filtro)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT u.idUsuario, u.correo, r.nombreRol
                             FROM usuarios u 
                             INNER JOIN roles r ON u.idRol = r.idRol
                             WHERE (@filtro = '' OR
                                    CAST(u.idUsuario AS NVARCHAR) LIKE '%' + @filtro + '%' OR
                                    u.correo                      LIKE '%' + @filtro + '%' OR
                                    r.nombreRol                   LIKE '%' + @filtro + '%')";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@filtro", filtro.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarUsuario(int idUsuario, string correo, string password, int idRol)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query;
            SqlCommand cmd;

            if (string.IsNullOrEmpty(password))
            {
                query = @"UPDATE usuarios SET correo = @correo, idRol = @idRol
                  WHERE idUsuario = @idUsuario";
                cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@idRol", idRol);
            }
            else
            {
                query = @"UPDATE usuarios SET correo = @correo, password = @password, idRol = @idRol
                  WHERE idUsuario = @idUsuario";
                cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@password", MtEncriptarCadena(password));
                cmd.Parameters.AddWithValue("@idRol", idRol);
            }

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtEliminarUsuario(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"DELETE FROM usuarios WHERE idUsuario = @idUsuario";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtLogin(string correo, string password)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT u.idUsuario, u.correo, r.nombreRol 
                             FROM usuarios u 
                             INNER JOIN roles r ON u.idRol = r.idRol
                             WHERE u.correo = @correo AND u.password = @password";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@password", MtEncriptarCadena(password.Trim()));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public DataTable MtBuscarUsuarioPorId(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT u.idUsuario, u.correo, u.idRol, r.nombreRol 
                             FROM usuarios u 
                             INNER JOIN roles r ON u.idRol = r.idRol 
                             WHERE u.idUsuario = @idUsuario";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public string MtEncriptarCadena(string textoClave)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(textoClave));
                System.Text.StringBuilder constructor = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    constructor.Append(bytes[i].ToString("x2"));
                }
                return constructor.ToString();
            }
        }

        public bool MtExisteCorreo(string correo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            int conteo = Convert.ToInt32(cmd.ExecuteScalar());
            oConex.MtCerrarConexion();
            return conteo > 0;
        }

        public bool MtSolicitarRecuperacion(string correo)
        {
            if (!MtExisteCorreo(correo))
                return false;

            string token = Guid.NewGuid().ToString("N");
            DateTime expiracion = DateTime.Now.AddHours(1);

            bool tokenGuardado = MtGuardarTokenRecuperacion(correo, token, expiracion);
            if (!tokenGuardado)
                return false;

            string hostActual = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string link = $"{hostActual}/Vista/resetPassword.aspx?token={token}";

            return MtEnviarCorreoRecuperacion(correo, link);
        }

        public bool MtGuardarTokenRecuperacion(string correo, string token, DateTime expiracion)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"MERGE recuperacionPassword AS target
                 USING (SELECT u.idUsuario FROM usuarios u WHERE u.correo = @correo) AS source (idUsuario)
                 ON target.idUsuario = source.idUsuario
                 WHEN MATCHED THEN
                     UPDATE SET token = @token, fechaExpiracion = @expiracion, usado = 0
                 WHEN NOT MATCHED THEN
                     INSERT (idUsuario, token, fechaExpiracion, usado)
                     VALUES (source.idUsuario, @token, @expiracion, 0);";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@expiracion", expiracion);
            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtEnviarCorreoRecuperacion(string correoDestino, string link)
        {
            try
            {
                System.Net.Mail.MailMessage mensaje = new System.Net.Mail.MailMessage();
                mensaje.From = new System.Net.Mail.MailAddress("Juliandreyes23@gmail.com", "Sistema Plan Mejoramientos");
                mensaje.To.Add(correoDestino);
                mensaje.Subject = "Recuperación de contraseña - SENA";
                mensaje.IsBodyHtml = true;
                mensaje.Body = $@"
<!DOCTYPE html>
<html>
<head><meta charset='utf-8'></head>
<body style='margin:0;padding:0;background:#f4f7fb;font-family:Segoe UI,Arial,sans-serif;'>

  <table width='100%' cellpadding='0' cellspacing='0' style='background:#f4f7fb;padding:40px 0;'>
    <tr>
      <td align='center'>
        <table width='560' cellpadding='0' cellspacing='0' style='background:#ffffff;border-radius:16px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.08);'>

          <tr>
            <td style='background:#042940;padding:32px 40px;text-align:center;'>
              <div style='width:56px;height:56px;background:#9FC131;border-radius:50%;display:inline-block;margin-bottom:14px;text-align:center;line-height:56px;'>
                <span style='font-size:28px;'>🔐</span>
              </div>
              <h1 style='margin:0;font-size:22px;font-weight:800;color:#ffffff;letter-spacing:-0.5px;'>
                Sistema SENA
              </h1>
              <p style='margin:6px 0 0;font-size:12px;color:rgba(255,255,255,0.5);text-transform:uppercase;letter-spacing:1px;'>
                Plan de Mejoramientos
              </p>
            </td>
          </tr>

          <tr>
            <td style='padding:36px 40px 28px;'>
              <h2 style='margin:0 0 10px;font-size:20px;color:#042940;font-weight:700;'>
                Recuperación de contraseña
              </h2>
              <p style='margin:0 0 20px;font-size:14px;color:#6c757d;line-height:1.7;'>
                Recibimos una solicitud para restablecer la contraseña de tu cuenta.
                Si fuiste tú, haz clic en el botón de abajo para continuar.
              </p>

              <div style='height:1px;background:#e0e6ed;margin-bottom:28px;'></div>

              <table width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td align='center' style='padding-bottom:28px;'>
                    <a href='{link}'
                       style='display:inline-block;background:#042940;color:#ffffff;
                              font-size:14px;font-weight:700;text-decoration:none;
                              padding:14px 36px;border-radius:10px;letter-spacing:0.5px;'>
                      🔑 &nbsp; Restablecer mi contraseña
                    </a>
                  </td>
                </tr>
              </table>

              <div style='background:#f4f7fb;border:1px solid #e0e6ed;border-radius:10px;padding:14px 16px;margin-bottom:24px;'>
                <p style='margin:0 0 6px;font-size:11px;color:#6c757d;font-weight:700;text-transform:uppercase;letter-spacing:1px;'>
                  O copia este enlace en tu navegador:
                </p>
                <p style='margin:0;font-size:12px;color:#185FA5;word-break:break-all;'>{link}</p>
              </div>

              <table width='100%' cellpadding='0' cellspacing='0' style='margin-bottom:24px;'>
                <tr>
                  <td style='background:#fff3cd;border:1px solid #ffc107;border-radius:10px;padding:12px 16px;'>
                    <table cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='font-size:18px;padding-right:10px;vertical-align:middle;'>⏱️</td>
                        <td style='font-size:13px;color:#856404;vertical-align:middle;'>
                          Este enlace expira en <strong>1 hora</strong>. Después deberás solicitar uno nuevo.
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>

              <p style='margin:0;font-size:12px;color:#6c757d;line-height:1.6;'>
                🔒 Si <strong>no solicitaste</strong> restablecer tu contraseña, puedes ignorar este mensaje.
                Tu cuenta sigue segura y no se realizó ningún cambio.
              </p>
            </td>
          </tr>

          <tr>
            <td style='background:#f4f7fb;padding:20px 40px;border-top:1px solid #e0e6ed;text-align:center;'>
              <p style='margin:0;font-size:11px;color:#adb5bd;'>
                © {DateTime.Now.Year} Sistema de Gestión de Planes de Mejoramiento · SENA
              </p>
              <p style='margin:4px 0 0;font-size:11px;color:#adb5bd;'>
                Este es un correo automático, por favor no respondas a este mensaje.
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>

</body>
</html>";

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential("Juliandreyes23@gmail.com", "uksf ethn cqzy bsow");
                smtp.EnableSsl = true;
                smtp.Send(mensaje);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MtRestablecerContrasena(string token, string nuevaPassword)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string queryValidar = @"SELECT r.idUsuario FROM recuperacionPassword r
                                    WHERE r.token = @token 
                                      AND r.fechaExpiracion > GETDATE() 
                                      AND r.usado = 0";
            SqlCommand cmdValidar = new SqlCommand(queryValidar, cn);
            cmdValidar.Parameters.AddWithValue("@token", token);
            object resultado = cmdValidar.ExecuteScalar();
            oConex.MtCerrarConexion();

            if (resultado == null) return false;

            int idUsuario = Convert.ToInt32(resultado);

            cn = oConex.MtAbrirConexion();
            string queryActualizar = @"UPDATE usuarios SET password = @password WHERE idUsuario = @idUsuario";
            SqlCommand cmdActualizar = new SqlCommand(queryActualizar, cn);
            cmdActualizar.Parameters.AddWithValue("@password", MtEncriptarCadena(nuevaPassword));
            cmdActualizar.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmdActualizar.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            cn = oConex.MtAbrirConexion();
            string queryMarcar = @"UPDATE recuperacionPassword SET usado = 1 WHERE token = @token";
            SqlCommand cmdMarcar = new SqlCommand(queryMarcar, cn);
            cmdMarcar.Parameters.AddWithValue("@token", token);
            cmdMarcar.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return true;
        }
        public int MtCrearUsuarioConRetorno(string correo, string password, int idRol)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"INSERT INTO usuarios (correo, password, idRol)
                     OUTPUT INSERTED.idUsuario
                     VALUES (@correo, @password, @idRol)";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@password", MtEncriptarCadena(password));
            cmd.Parameters.AddWithValue("@idRol", idRol);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : -1;
        }
        public int MtObtenerIdAprendizPorUsuario(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idAprendiz FROM aprendices WHERE idUsuario = @idUsuario";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null && resultado != DBNull.Value ? Convert.ToInt32(resultado) : 0;
        }

        public int MtObtenerIdCentroAdmin(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idCentro FROM administradores WHERE idUsuario = @idUsuario";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        public int MtObtenerIdInstructor(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idInstructor FROM instructores WHERE idUsuario = @idUsuario";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        public int MtObtenerIdAprendiz(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idAprendiz FROM aprendices WHERE idUsuario = @idUsuario";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            object resultado = cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }
    }
}