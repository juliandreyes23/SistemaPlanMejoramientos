using System;
using System.Data;
using System.Web.UI;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        ClUsuarioL oUsuarioL = new ClUsuarioL();

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
                    Session["idCentro"] = oUsuarioL.MtObtenerIdCentroAdmin(idUsuario).ToString();
                else if (nombreRol == "INSTRUCTOR")
                    Session["idInstructor"] = oUsuarioL.MtObtenerIdInstructor(idUsuario).ToString();
                else if (nombreRol == "APRENDIZ")
                    Session["idAprendiz"] = oUsuarioL.MtObtenerIdAprendiz(idUsuario).ToString();

                if (nombreRol == "ADMINISTRADOR")
                    Response.Redirect("Dashboard.aspx");
                else if (nombreRol == "INSTRUCTOR")
                    Response.Redirect("DashboardInstructor.aspx");
                else if (nombreRol == "APRENDIZ")
                    Response.Redirect("DashboardAprendiz.aspx");
                else
                    MostrarError("El rol asignado no cuenta con un módulo de trabajo activo.");
            }
            else
            {
                MostrarError("Correo o contraseña incorrectos. Intente de nuevo.");
            }
        }

        private void MostrarError(string mensaje)
        {
            pnlAlerta.Visible = true;
            lblMensajeError.Text = mensaje;
        }
    }
}