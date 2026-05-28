using System;
using System.Data;
using System.IO;
using System.Web.UI;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Aprendiz
{
    public partial class FrmSubirEvidencia : System.Web.UI.Page
    {
        ClEvidenciaL oEvidenciaL = new ClEvidenciaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["correo"] == null || Session["rol"] == null ||
                Session["rol"].ToString().ToUpper() != "APRENDIZ")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string qsPlan = Request.QueryString["idPlan"];

            if (string.IsNullOrEmpty(qsPlan))
            {
                Response.Redirect("FrmMisPlanes.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            hfIdPlan.Value = qsPlan;
            int idPlan = Convert.ToInt32(qsPlan);

            if (!IsPostBack)
            {
                hfAlerta.Value = "";
                CargarInfoPlan(idPlan);
            }

            CargarHistorialEvidencias(idPlan);
        }

        private void CargarInfoPlan(int idPlan)
        {
            DataTable dt = oEvidenciaL.MtObtenerPlanPorId(idPlan);

            if (dt.Rows.Count == 0)
            {
                Response.Redirect("FrmMisPlanes.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            DataRow r = dt.Rows[0];

            litTipoPlan.Text = $"<div class='plan-tipo'><i class='bi bi-bookmark-fill'></i> Plan {r["tipoPlan"]}</div>";
            lblActividades.Text = r["actividades"].ToString();
            lblInstructor.Text = r["nombreInstructor"].ToString();
            lblFechaLimite.Text = Convert.ToDateTime(r["fechaLimite"]).ToString("dd/MM/yyyy");
            lblEstadoPlan.Text = r["estadoPlan"].ToString();

            string estado = r["estadoPlan"].ToString();
            if (estado == "Aprobado" || estado == "No Aprobado")
            {
                btnSubir.Enabled = false;
                MostrarAlerta("warning", "Plan bloqueado", "Este plan ya fue evaluado. No puedes subir más evidencias.");
            }

            DataTable dtRaps = oEvidenciaL.MtListarResultadosPorPlan(idPlan);
            string html = "";
            foreach (DataRow rp in dtRaps.Rows)
                html += $"<span class='rap-chip'><i class='bi bi-exclamation-circle'></i> {rp["descripcion"]}</span>";
            litRaps.Text = html;
        }

        private void CargarHistorialEvidencias(int idPlan)
        {
            DataTable dt = oEvidenciaL.MtListarEvidenciaPorPlan(idPlan);
            rptEvidencias.DataSource = dt;
            rptEvidencias.DataBind();
        }

        protected void btnSubir_Click(object sender, EventArgs e)
        {
            int idPlan = Convert.ToInt32(hfIdPlan.Value);

            if (!fuEvidencia.HasFile)
            {
                MostrarAlerta("warning", "Archivo requerido", "Por favor selecciona un archivo.");
                return;
            }

            var file = fuEvidencia.PostedFile;
            string nombreOriginal = Path.GetFileName(file.FileName);

            if (!oEvidenciaL.MtValidarExtension(nombreOriginal))
            {
                MostrarAlerta("error", "Formato incorrecto", "Usa archivos PDF, DOCX, JPG, PNG o ZIP.");
                return;
            }

            if (file.ContentLength > 50_000_000)
            {
                MostrarAlerta("error", "Archivo demasiado grande", "El archivo supera el límite de 50 MB.");
                return;
            }

            string carpeta = Server.MapPath("~/Vista/Evidencias/");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string extension = Path.GetExtension(nombreOriginal);
            string nombreBase = Path.GetFileNameWithoutExtension(nombreOriginal);
            if (nombreBase.Length > 30) nombreBase = nombreBase.Substring(0, 30);
            string fileName = DateTime.Now.Ticks + "_" + nombreBase + extension;
            string rutaFisica = Path.Combine(carpeta, fileName);

            file.SaveAs(rutaFisica);

            string tipoArchivo = oEvidenciaL.MtObtenerTipoArchivo(nombreOriginal);
            DataTable dtExist = oEvidenciaL.MtListarEvidenciaPorPlan(idPlan);
            bool resultado;

            if (dtExist.Rows.Count > 0)
                resultado = oEvidenciaL.MtSobrescribirEvidencia(idPlan, fileName, rutaFisica, DateTime.Now, tipoArchivo);
            else
                resultado = oEvidenciaL.MtRegistrarEvidencia(idPlan, fileName, rutaFisica, DateTime.Now, tipoArchivo);

            if (resultado)
                MostrarAlerta("success", "¡Listo!", "La evidencia se subió correctamente.", true);
            else
                MostrarAlerta("error", "Error", "No se pudo registrar la evidencia en la base de datos.");
        }

        protected string ObtenerUrlDescarga(string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo)) return "#";
            string appPath = Request.ApplicationPath.TrimEnd('/');
            return $"{appPath}/Vista/Evidencias/{nombreArchivo}";
        }

        protected string ObtenerIconoTipo(string tipo)
        {
            switch (tipo)
            {
                case "PDF": return "bi bi-file-earmark-pdf-fill";
                case "DOCX": return "bi bi-file-earmark-word-fill";
                case "JPG":
                case "PNG": return "bi bi-file-earmark-image-fill";
                case "ZIP": return "bi bi-file-zip-fill";
                default: return "bi bi-file-earmark-fill";
            }
        }

        private void MostrarAlerta(string icono, string titulo, string texto, bool recargar = false)
        {
            string reload = recargar ? "true" : "false";
            hfAlerta.Value = $"{{\"icon\":\"{icono}\",\"title\":\"{titulo}\",\"text\":\"{texto}\",\"reload\":{reload}}}";
        }
    }
}