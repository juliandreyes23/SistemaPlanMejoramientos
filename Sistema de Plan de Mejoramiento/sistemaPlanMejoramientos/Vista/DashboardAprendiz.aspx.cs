using System;
using System.Data;
using sistemaPlanMejoramientos.Logica;

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
            DataTable dt = oEvidenciaL.MtObtenerAprendizPorUsuario(idUsuario);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                Session["idAprendiz"] = r["idAprendiz"];
                Session["nombreAprendiz"] = r["nombres"].ToString() + " " + r["apellidos"].ToString();
                Session["tipoDocumento"] = r["tipoDocumento"].ToString();
                Session["numeroDocumento"] = r["numeroDocumento"].ToString();
                Session["correoAprendiz"] = r["correo"].ToString();
                Session["telefono"] = r["telefono"].ToString();
                Session["estadoAcademico"] = r["estadoAcademico"].ToString();
                Session["codigoFicha"] = r["codigoFicha"].ToString();
                Session["nombrePrograma"] = r["nombrePrograma"].ToString();
                Session["jornada"] = r["jornada"].ToString();

                lblAprendiz.Text = Session["nombreAprendiz"].ToString();

                string estado = r["estadoAcademico"].ToString();
                string cssEstado = estado == "Cancelado" ? "cancelado" :
                                   (estado == "Aplazado" || estado == "Condicionado") ? "aplazado" : "";
                lblEstadoBadge.Text = $"<span class='estado-badge {cssEstado}'>" +
                                      $"<i class='bi bi-circle-fill' style='font-size:7px'></i> {estado}</span>";
            }
        }

        private void CargarMetricas()
        {
            if (Session["idAprendiz"] == null) return;
            int idAprendiz = Convert.ToInt32(Session["idAprendiz"]);

            lblPlanesPendientes.Text = oEvidenciaL.MtContarPlanesPorEstado(idAprendiz, "Pendiente").ToString();
            lblPlanesAprobados.Text = oEvidenciaL.MtContarPlanesPorEstado(idAprendiz, "Aprobado").ToString();

            DataTable dtPlanes = oEvidenciaL.MtListarPlanesPorAprendiz(idAprendiz);
            int comite = 0;
            foreach (DataRow r in dtPlanes.Rows)
                if (r["tipoPlan"].ToString() == "Comité" && r["estadoPlan"].ToString() == "Pendiente")
                    comite++;
            lblPlanesComite.Text = comite.ToString();
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Vista/FrmLogin.aspx");
        }
    }
}