using System;
using System.Linq;
using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;

namespace sistemaPlanMejoramientos.Aprendiz
{
    public partial class DashboardAprendiz : System.Web.UI.Page
    {
        ClEvidenciaL oEvidenciaL = new ClEvidenciaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["correo"] == null || Session["rol"] == null ||
                Session["rol"].ToString().ToUpper() != "APRENDIZ")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarDatosAprendiz();
                CargarMetricas();
            }
        }

        private void CargarDatosAprendiz()
        {
            int idUsuario = Convert.ToInt32(Session["idUsuario"]);

            ClAprendizM aprendiz =
                oEvidenciaL.MtObtenerAprendizPorUsuario(idUsuario);

            if (aprendiz == null) return;

            Session["idAprendiz"] = aprendiz.idAprendiz;

            lblAprendiz.Text = $"{aprendiz.nombres} {aprendiz.apellidos}";

            string estado = aprendiz.estadoAcademico;

            string cssEstado =
                estado == "Cancelado" ? "cancelado" :
                (estado == "Aplazado" || estado == "Condicionado") ? "aplazado" : "";

            lblEstadoBadge.Text =
                $"<span class='estado-badge {cssEstado}'>" +
                $"<i class='bi bi-circle-fill' style='font-size:7px'></i> {estado}</span>";

            if (aprendiz.ficha != null)
            {
                Session["codigoFicha"] = aprendiz.ficha.codigoFicha;
                Session["nombrePrograma"] = aprendiz.ficha.programa?.nombre;
                Session["jornada"] = aprendiz.ficha.jornada;
            }
        }

        private void CargarMetricas()
        {
            if (Session["idAprendiz"] == null) return;

            int idAprendiz = Convert.ToInt32(Session["idAprendiz"]);

            lblPlanesPendientes.Text =
                oEvidenciaL.MtContarPlanesPorEstado(idAprendiz, "Pendiente").ToString();

            lblPlanesAprobados.Text =
                oEvidenciaL.MtContarPlanesPorEstado(idAprendiz, "Aprobado").ToString();

            var planes = oEvidenciaL.MtListarPlanesPorAprendiz(idAprendiz);

            lblPlanesComite.Text =
                planes.Count(p => p.tipoPlan == "Comité" && p.estadoPlan == "Pendiente").ToString();
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Vista/FrmLogin.aspx");
        }
    }
}