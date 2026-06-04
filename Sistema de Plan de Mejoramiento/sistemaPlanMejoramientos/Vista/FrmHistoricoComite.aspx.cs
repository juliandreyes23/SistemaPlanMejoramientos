using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Instructor
{
    public partial class FrmHistoricoComite : System.Web.UI.Page
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
            DataTable dt = oPlanL.MtListarPlanesComitePorInstructor(idInstructor, filtroNombre, filtroEstado);

            if (dt == null || dt.Rows.Count == 0)
            {
                rptPlanes.Visible = false;
                pnlVacio.Visible = true;
            }
            else
            {
                rptPlanes.DataSource = dt;
                rptPlanes.DataBind();
                rptPlanes.Visible = true;
                pnlVacio.Visible = false;
            }
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
            DataTable dtLista = oPlanL.MtListarPlanesComitePorInstructor(ObtenerIdInstructor(), "", "");
            DataRow[] filas = dtLista.Select("idPlanMejoramiento = " + idPlan);

            if (filas.Length == 0) return;
            DataRow plan = filas[0];

            lblIdPlan.Text = idPlan.ToString();
            lblFechaAsig.Text = Convert.ToDateTime(plan["fechaAsignacion"]).ToString("dd/MM/yyyy");
            lblFechaLimite.Text = Convert.ToDateTime(plan["fechaLimite"]).ToString("dd/MM/yyyy");
            lblActividades.Text = plan["actividades"]?.ToString() ?? "—";
            lblInstructor.Text = plan["nombreInstructor"]?.ToString() ?? "—";

            lblAprendizHeader.Text = plan["nombreAprendiz"].ToString();
            lblEstadoHeader.Text = plan["estadoPlan"].ToString();
            lblFichaHeader.Text = plan["codigoFicha"].ToString();

            DataTable dtRaps = oEvidL.MtListarResultadosPorPlan(idPlan);
            if (dtRaps.Rows.Count == 0)
            {
                rptResultados.Visible = false;
                pnlSinResultados.Visible = true;
            }
            else
            {
                rptResultados.DataSource = dtRaps;
                rptResultados.DataBind();
                rptResultados.Visible = true;
                pnlSinResultados.Visible = false;
            }

            DataTable dtEv = oEvidL.MtListarEvidenciaPorPlan(idPlan);
            if (dtEv.Rows.Count == 0)
            {
                rptEvidencias.Visible = false;
                pnlSinEvidencias.Visible = true;
            }
            else
            {
                rptEvidencias.DataSource = dtEv;
                rptEvidencias.DataBind();
                rptEvidencias.Visible = true;
                pnlSinEvidencias.Visible = false;
            }

            DataTable dtCrit = oEvalL.MtConsultarEvaluacionPorPlan(idPlan);

            if (dtCrit.Rows.Count == 0)
            {
                pnlSinEvaluacion.Visible = true;
                pnlConEvaluacion.Visible = false;
            }
            else
            {
                pnlSinEvaluacion.Visible = false;
                pnlConEvaluacion.Visible = true;

                DataRow ev = dtCrit.Rows[0];
                string producto = ev["criterioProducto"]?.ToString() ?? "";
                string conocimiento = ev["criterioConocimiento"]?.ToString() ?? "";
                string desempeno = ev["criterioDesempeno"]?.ToString() ?? "";

                lblProducto.Text = producto;
                lblConocimiento.Text = conocimiento;
                lblDesempeno.Text = desempeno;

                divProducto.Attributes["class"] = "eval-crit " + GetClaseCriterio(producto);
                divConocimiento.Attributes["class"] = "eval-crit " + GetClaseCriterio(conocimiento);
                divDesempeno.Attributes["class"] = "eval-crit " + GetClaseCriterio(desempeno);

                string obsEval = ev["observaciones"]?.ToString() ?? "";
                lblObsEvaluacion.Text = string.IsNullOrWhiteSpace(obsEval) ? "Sin observaciones registradas." : obsEval;
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