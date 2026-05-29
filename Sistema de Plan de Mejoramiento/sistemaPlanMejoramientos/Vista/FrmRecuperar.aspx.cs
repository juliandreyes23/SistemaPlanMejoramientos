using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos
{
    public partial class FrmRecuperar : System.Web.UI.Page
    {
        ClUsuarioL oUsuarioL = new ClUsuarioL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlResultado.Visible = false;
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();

            if (string.IsNullOrEmpty(correo))
            {
                MostrarAlerta("Por favor, digite su correo electrónico.", false);
                return;
            }

            bool exito = oUsuarioL.MtSolicitarRecuperacion(correo);

            if (exito)
            {
                MostrarAlerta("Enlace de recuperación enviado con éxito. Revise su bandeja de entrada.", true);
                txtCorreo.Text = string.Empty;
            }
            else
            {
                MostrarAlerta("El correo ingresado no se encuentra registrado o el servicio de correo no se pudo conectar.", false);
            }
        }

        private void MostrarAlerta(string mensaje, bool esExito)
        {
            pnlResultado.Visible = true;
            lblMensaje.Text = mensaje;

            if (esExito)
            {
                pnlResultado.CssClass = "alert alert-success";
            }
            else
            {
                pnlResultado.CssClass = "alert alert-danger";
            }
        }
    }
}