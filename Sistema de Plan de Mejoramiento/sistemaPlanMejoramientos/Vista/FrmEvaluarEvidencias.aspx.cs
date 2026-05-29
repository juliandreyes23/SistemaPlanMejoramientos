using System;
using System.Data;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Instructor
{
    public partial class FrmEvaluarEvidencias : System.Web.UI.Page
    {
        ClPlanMejoramientoL oPlanL = new ClPlanMejoramientoL();
        ClEvidenciaL oEvidL = new ClEvidenciaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarPlanesPendientes();
        }

        private void CargarPlanesPendientes()
        {
            int idInstructor = ObtenerIdInstructor();
            DataTable dt = oPlanL.MtListarPlanesPendientesEvaluacion(idInstructor);

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

        protected void rptPlanes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Evaluar") return;

            string[] partes = e.CommandArgument.ToString().Split('|');
            if (partes.Length < 4) return;

            int idPlan = int.Parse(partes[0]);
            int idAprendiz = int.Parse(partes[1]);
            string tipoPlan = partes[2];
            DateTime fechaLimite = DateTime.Parse(partes[3]);

            hfIdPlan.Value = idPlan.ToString();
            hfIdAprendiz.Value = idAprendiz.ToString();
            hfTipoPlan.Value = tipoPlan;
            hfFechaLimite.Value = fechaLimite.ToString("yyyy-MM-dd");

            bool vencido = DateTime.Now.Date > fechaLimite.Date;
            hfVencido.Value = vencido ? "1" : "0";

            CargarDetallePlan(idPlan, tipoPlan, fechaLimite, vencido);
            CargarPlanesPendientes();
        }

        private void CargarDetallePlan(int idPlan, string tipoPlan, DateTime fechaLimite, bool vencido)
        {
            DataTable dtPlan = oEvidL.MtObtenerPlanPorId(idPlan);
            if (dtPlan.Rows.Count == 0) return;

            DataRow row = dtPlan.Rows[0];

            lblIdPlanHeader.Text = idPlan.ToString();
            lblTipoPlanHeader.Text = tipoPlan;
            lblFechaLimiteHeader.Text = fechaLimite.ToString("dd/MM/yyyy");
            lblActividadesInfo.Text = row["actividades"]?.ToString() ?? "";
            lblAprendizInfo.Text = "ID Aprendiz: " + hfIdAprendiz.Value;
            lblFichaInfo.Text = "—";

            pnlAlertaVencido.Visible = vencido;

            rbProductoAprueba.Enabled = !vencido;
            rbProductoNoAprueba.Enabled = !vencido;
            rbConocimientoAprueba.Enabled = !vencido;
            rbConocimientoNoAprueba.Enabled = !vencido;
            rbDesempenoAprueba.Enabled = !vencido;
            rbDesempenoNoAprueba.Enabled = !vencido;

            if (vencido)
            {
                rbProductoNoAprueba.Checked = true;
                rbConocimientoNoAprueba.Checked = true;
                rbDesempenoNoAprueba.Checked = true;
            }
            else
            {
                rbProductoAprueba.Checked = false;
                rbProductoNoAprueba.Checked = false;
                rbConocimientoAprueba.Checked = false;
                rbConocimientoNoAprueba.Checked = false;
                rbDesempenoAprueba.Checked = false;
                rbDesempenoNoAprueba.Checked = false;
            }

            txtObservaciones.Text = "";

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

            panelEvaluar.Style["display"] = "block";
        }

        protected void btnGuardarEvaluacion_Click(object sender, EventArgs e)
        {
            int idPlan = int.Parse(hfIdPlan.Value);
            int idAprendiz = int.Parse(hfIdAprendiz.Value);
            int idInstructor = ObtenerIdInstructor();
            string tipoPlan = hfTipoPlan.Value;
            DateTime fechaLimite = DateTime.Parse(hfFechaLimite.Value);
            bool vencido = hfVencido.Value == "1";

            string producto = vencido ? "No Aprobado" : (rbProductoAprueba.Checked ? "Aprobado" : "No Aprobado");
            string conocimiento = vencido ? "No Aprobado" : (rbConocimientoAprueba.Checked ? "Aprobado" : "No Aprobado");
            string desempeno = vencido ? "No Aprobado" : (rbDesempenoAprueba.Checked ? "Aprobado" : "No Aprobado");
            string observaciones = txtObservaciones.Text.Trim();

            string resultado = oPlanL.MtEvaluarPlan(
                idPlan, idAprendiz, idInstructor,
                producto, conocimiento, desempeno,
                observaciones, tipoPlan, fechaLimite
            );

            hfResultado.Value = resultado;
            hfIdPlan.Value = "0";
            panelEvaluar.Style["display"] = "none";
            CargarPlanesPendientes();
        }


        protected string GetIconClass(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return "pdf";
            switch (tipo.ToUpper())
            {
                case "PDF": return "pdf";
                case "DOCX": return "docx";
                case "JPG":
                case "JPEG":
                case "PNG": return "img";
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
                case "JPG":
                case "JPEG":
                case "PNG": return "bi-file-earmark-image-fill";
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