using System;
using System.Web.UI;
using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;

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

            ClUsuarioM usuario = oUsuarioL.MtLogin(correo, password);

            if (usuario != null)
            {
                string nombreRol = usuario.rol.nombreRol.ToUpper();

                Session["idUsuario"] = usuario.idUsuario.ToString();
                Session["correo"] = usuario.correo;
                Session["rol"] = nombreRol;

                if (nombreRol == "ADMINISTRADOR")
                    Session["idCentro"] = oUsuarioL.MtObtenerIdCentroAdmin(usuario.idUsuario).ToString();
                else if (nombreRol == "INSTRUCTOR")
                    Session["idInstructor"] = oUsuarioL.MtObtenerIdInstructor(usuario.idUsuario).ToString();
                else if (nombreRol == "APRENDIZ")
                    Session["idAprendiz"] = oUsuarioL.MtObtenerIdAprendiz(usuario.idUsuario).ToString();

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