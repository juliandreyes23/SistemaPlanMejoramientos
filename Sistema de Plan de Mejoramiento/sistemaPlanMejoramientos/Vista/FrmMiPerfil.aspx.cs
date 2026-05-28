using System;
using System.Web.UI;

namespace sistemaPlanMejoramientos.Aprendiz
{
    public partial class FrmMiPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["correo"] == null || Session["rol"] == null ||
                Session["rol"].ToString().ToUpper() != "APRENDIZ")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
                CargarPerfil();
        }

        private void CargarPerfil()
        {
            lblNombre.Text = Session["nombreAprendiz"]?.ToString() ?? "—";
            lblTipoDoc.Text = Session["tipoDocumento"]?.ToString() ?? "—";
            lblDocumento.Text = Session["numeroDocumento"]?.ToString() ?? "—";
            lblCorreo.Text = Session["correoAprendiz"]?.ToString() ?? "—";
            lblTelefono.Text = Session["telefono"]?.ToString() ?? "—";
            lblFicha.Text = Session["codigoFicha"]?.ToString() ?? "—";
            lblPrograma.Text = Session["nombrePrograma"]?.ToString() ?? "—";
            lblJornada.Text = Session["jornada"]?.ToString() ?? "—";

            string estado = Session["estadoAcademico"]?.ToString() ?? "—";
            string css = estado == "Cancelado" ? "cancelado" :
                         (estado == "Aplazado" || estado == "Condicionado") ? "aplazado" : "";
            lblEstado.Text = $"<span class='estado-pill {css}'>" +
                             $"<i class='bi bi-circle-fill' style='font-size:7px'></i> {estado}</span>";
        }
    }
}