using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        ClUsuarioL oUsuarioL = new ClUsuarioL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlResultado.Visible = false;

                if (Request.QueryString["token"] == null || string.IsNullOrWhiteSpace(Request.QueryString["token"]))
                {
                    phFormulario.Visible = false;
                    MostrarAlerta("El enlace de recuperación es inválido, está incompleto o ha expirado.", false);
                }
            }
        }

        protected void btnRestablecer_Click(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"].ToString().Trim();
            string nuevaClave = txtNuevaPassword.Text;
            string confirmarClave = txtConfirmarPassword.Text;

            if (string.IsNullOrWhiteSpace(nuevaClave) || string.IsNullOrWhiteSpace(confirmarClave))
            {
                MostrarAlerta("Por favor, rellene todos los campos de contraseña.", false);
                return;
            }

            if (nuevaClave != confirmarClave)
            {
                MostrarAlerta("Las contraseñas ingresadas no coinciden. Verifique e intente de nuevo.", false);
                return;
            }

            if (nuevaClave.Length < 4)
            {
                MostrarAlerta("La nueva contraseña debe tener como mínimo 4 caracteres.", false);
                return;
            }

            bool actualizacionExitosa = oUsuarioL.MtRestablecerContrasena(token, nuevaClave);

            if (actualizacionExitosa)
            {
                phFormulario.Visible = false;
                MostrarAlerta("¡Tu contraseña ha sido actualizada correctamente! Ya puedes regresar al Inicio de Sesión.", true);
            }
            else
            {
                MostrarAlerta("No se pudo restablecer la contraseña. El enlace ya fue utilizado anteriormente o el tiempo de 1 hora expiró.", false);
            }
        }

        private void MostrarAlerta(string mensaje, bool esExito)
        {
            pnlResultado.Visible = true;
            lblMensaje.Text = mensaje;

            if (esExito)
            {
                pnlResultado.CssClass = "alert alert-success d-flex align-items-center";
            }
            else
            {
                pnlResultado.CssClass = "alert alert-danger d-flex align-items-center";
            }
        }
    }
}