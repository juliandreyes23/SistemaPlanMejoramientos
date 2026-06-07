using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class FrmHistoricoInternos : System.Web.UI.Page
    {
        ClPlanMejoramientoL oPlanL = new ClPlanMejoramientoL();
        ClEvidenciaL oEvidL = new ClEvidenciaL();
        ClEvaluacionL oEvalL = new ClEvaluacionL();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarPlanes("", "");
        }

        private void CargarPlanes(string filtroNombre, string filtroEstado)
        {
            int idInstructor = ObtenerIdInstructor();

            var lista = oPlanL.MtListarPlanesInternosPorInstructor(idInstructor, filtroNombre, filtroEstado);

            if (lista == null || lista.Count == 0)
            {
                rptPlanes.DataSource = null;
                rptPlanes.DataBind();
                rptPlanes.Visible = false;
                pnlVacio.Visible = true;
                return;
            }

            rptPlanes.DataSource = lista;
            rptPlanes.DataBind();
            rptPlanes.Visible = true;
            pnlVacio.Visible = false;
        }


        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarPlanes(txtBuscar.Text.Trim(), ddlEstado.SelectedValue);
        }

        protected void rptPlanes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Detalle") return;

            int idPlan = int.Parse(e.CommandArgument.ToString());
            hfIdPlanDetalle.Value = idPlan.ToString();
            CargarDetalle(idPlan);
            CargarPlanes(txtBuscar.Text.Trim(), ddlEstado.SelectedValue);
        }

        private void CargarDetalle(int idPlan)
        {
            int idInstructor = ObtenerIdInstructor();

            var listaPlanes = oPlanL.MtListarPlanesInternosPorInstructor(idInstructor, "", "");
            var plan = listaPlanes.FirstOrDefault(p => p.idPlanMejoramiento == idPlan);

            if (plan == null) return;

            lblIdPlan.Text = plan.idPlanMejoramiento.ToString();
            lblFechaAsig.Text = plan.fechaAsignacion.ToString("dd/MM/yyyy");
            lblFechaLimite.Text = plan.fechaLimite.ToString("dd/MM/yyyy");
            lblActividades.Text = plan.actividades ?? "—";
            lblInstructor.Text = plan.nombreInstructor ?? "—";

            lblAprendizHeader.Text = plan.nombreAprendiz;
            lblEstadoHeader.Text = plan.estadoPlan;
            lblFichaHeader.Text = plan.codigoFicha;

            var resultados = oEvidL.MtListarResultadosPorPlan(idPlan);

            if (resultados == null || resultados.Count == 0)
            {
                rptResultados.Visible = false;
                pnlSinResultados.Visible = true;
            }
            else
            {
                rptResultados.DataSource = resultados;
                rptResultados.DataBind();
                rptResultados.Visible = true;
                pnlSinResultados.Visible = false;
            }

            var evidencias = oEvidL.MtListarEvidenciaPorPlan(idPlan);

            if (evidencias == null || evidencias.Count == 0)
            {
                rptEvidencias.Visible = false;
                pnlSinEvidencias.Visible = true;
            }
            else
            {
                rptEvidencias.DataSource = evidencias;
                rptEvidencias.DataBind();
                rptEvidencias.Visible = true;
                pnlSinEvidencias.Visible = false;
            }

            var eval = oEvalL.MtConsultarEvaluacionPorPlan(idPlan);

            if (eval == null)
            {
                pnlSinEvaluacion.Visible = true;
                pnlConEvaluacion.Visible = false;
            }
            else
            {
                pnlSinEvaluacion.Visible = false;
                pnlConEvaluacion.Visible = true;

                string producto = eval.criterioProducto ?? "";
                string conocimiento = eval.criterioConocimiento ?? "";
                string desempeno = eval.criterioDesempeno ?? "";

                lblProducto.Text = producto;
                lblConocimiento.Text = conocimiento;
                lblDesempeno.Text = desempeno;

                divProducto.Attributes["class"] = "eval-crit " + GetClaseCriterio(producto);
                divConocimiento.Attributes["class"] = "eval-crit " + GetClaseCriterio(conocimiento);
                divDesempeno.Attributes["class"] = "eval-crit " + GetClaseCriterio(desempeno);

                lblObsEvaluacion.Text = string.IsNullOrWhiteSpace(eval.observaciones)
                    ? "Sin observaciones registradas."
                    : eval.observaciones;
            }

            panelDetalle.Style["display"] = "block";
        }


        protected string GetBadgeEstado(string estado)
        {
            switch (estado)
            {
                case "Aprobado": return "<span class='badge badge-aprobado'><i class='bi bi-check-circle-fill'></i> Aprobado</span>";
                case "No Aprobado": return "<span class='badge badge-noaprobado'><i class='bi bi-x-circle-fill'></i> No Aprobado</span>";
                default: return "<span class='badge badge-pendiente'><i class='bi bi-clock'></i> Pendiente</span>";
            }
        }

        private string GetClaseCriterio(string valor)
        {
            if (valor == "Aprobado") return "aprobado";
            if (valor == "No Aprobado") return "noaprobado";
            return "sineval";
        }

        protected string GetIconClass(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return "pdf";
            switch (tipo.ToUpper())
            {
                case "PDF": return "pdf";
                case "DOCX": return "docx";
                case "JPG": case "JPEG": case "PNG": return "img";
                case "ZIP": return "zip";
                default: return "pdf";
            }
        }

        protected string GetIconBi(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return "bi-file-earmark-fill";
            switch (tipo.ToUpper())
            {
                case "PDF": return "bi-file-earmark-pdf-fill";
                case "DOCX": return "bi-file-earmark-word-fill";
                case "JPG": case "JPEG": case "PNG": return "bi-file-earmark-image-fill";
                case "ZIP": return "bi-file-earmark-zip-fill";
                default: return "bi-file-earmark-fill";
            }
        }

        private int ObtenerIdInstructor()
        {
            if (Session["idInstructor"] != null)
                return Convert.ToInt32(Session["idInstructor"]);
            return 0;
        }
    }
}