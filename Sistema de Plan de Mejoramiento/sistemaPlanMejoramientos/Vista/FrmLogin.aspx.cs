using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        ClUsuarioL oUsuarioL = new ClUsuarioL();
        ClConexion oConex = new ClConexion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                Session.Abandon();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                MostrarError("Por favor, digite todos los campos.");
                return;
            }

            DataTable dtUsuario = oUsuarioL.MtLogin(correo, password);

            if (dtUsuario != null && dtUsuario.Rows.Count > 0)
            {
                DataRow fila = dtUsuario.Rows[0];
                int idUsuario = Convert.ToInt32(fila["idUsuario"]);
                string nombreRol = fila["nombreRol"].ToString().ToUpper();

                Session["idUsuario"] = idUsuario.ToString();
                Session["correo"] = fila["correo"].ToString();
                Session["rol"] = nombreRol;

                if (nombreRol == "ADMINISTRADOR")
                {
                    Session["idCentro"] = ObtenerIdCentroAdmin(idUsuario).ToString();
                }
                else if (nombreRol == "INSTRUCTOR")
                {
                    Session["idInstructor"] = ObtenerIdInstructor(idUsuario).ToString();
                }
                else if (nombreRol == "APRENDIZ")
                {
                    Session["idAprendiz"] = ObtenerIdAprendiz(idUsuario).ToString();
                }

                if (nombreRol == "ADMINISTRADOR")
                {
                    Response.Redirect("Dashboard.aspx");
                }
                else if (nombreRol == "INSTRUCTOR")
                {
                    Response.Redirect("DashboardInstructor.aspx");
                }
                else if (nombreRol == "APRENDIZ")
                {
                    Response.Redirect("DashboardAprendiz.aspx");
                }
                else
                {
                    MostrarError("El rol asignado no cuenta con un módulo de trabajo activo.");
                }
            }
            else
            {
                MostrarError("Correo o contraseña incorrectos. Intente de nuevo.");
            }
        }

        private int ObtenerIdCentroAdmin(int idUsuario)
        {
            int id = 0;
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = "SELECT idCentro FROM administradores WHERE idUsuario = @idUsuario";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                object resultado = cmd.ExecuteScalar();
                if (resultado != null)
                    id = Convert.ToInt32(resultado);
                oConex.MtCerrarConexion();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ObtenerIdCentroAdmin: " + ex.Message);
            }
            return id;
        }

        private int ObtenerIdInstructor(int idUsuario)
        {
            int id = 0;
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = "SELECT idInstructor FROM instructores WHERE idUsuario = @idUsuario";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                object resultado = cmd.ExecuteScalar();
                if (resultado != null)
                    id = Convert.ToInt32(resultado);
                oConex.MtCerrarConexion();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ObtenerIdInstructor: " + ex.Message);
            }
            return id;
        }

        private int ObtenerIdAprendiz(int idUsuario)
        {
            int id = 0;
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = "SELECT idAprendiz FROM aprendices WHERE idUsuario = @idUsuario";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                object resultado = cmd.ExecuteScalar();
                if (resultado != null)
                    id = Convert.ToInt32(resultado);
                oConex.MtCerrarConexion();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ObtenerIdAprendiz: " + ex.Message);
            }
            return id;
        }

        private void MostrarError(string mensaje)
        {
            pnlAlerta.Visible = true;
            lblMensajeError.Text = mensaje;
        }
    }
}