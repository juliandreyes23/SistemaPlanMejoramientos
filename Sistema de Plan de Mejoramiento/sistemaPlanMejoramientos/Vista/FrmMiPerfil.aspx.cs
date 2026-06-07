using sistemaPlanMejoramientos.Logica;
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
            int idUsuario = Convert.ToInt32(Session["idUsuario"]);

            ClEvidenciaL oEvidenciaL = new ClEvidenciaL();
            var aprendiz = oEvidenciaL.MtObtenerAprendizPorUsuario(idUsuario);

            if (aprendiz == null) return;

            lblNombre.Text = aprendiz.nombres + " " + aprendiz.apellidos;
            lblTipoDoc.Text = aprendiz.tipoDocumento;
            lblDocumento.Text = aprendiz.numeroDocumento;
            lblCorreo.Text = aprendiz.correo;
            lblTelefono.Text = aprendiz.telefono;

            lblFicha.Text = aprendiz.ficha?.codigoFicha ?? "—";
            lblPrograma.Text = aprendiz.ficha?.programa?.nombre ?? "—";
            lblJornada.Text = aprendiz.ficha?.jornada ?? "—";

            string estado = aprendiz.estadoAcademico ?? "—";
            string css = estado == "Cancelado" ? "cancelado" :
                         (estado == "Aplazado" || estado == "Condicionado") ? "aplazado" : "";
            lblEstado.Text = $"<span class='estado-pill {css}'>" +
                             $"<i class='bi bi-circle-fill' style='font-size:7px'></i> {estado}</span>";
        }
    }
}