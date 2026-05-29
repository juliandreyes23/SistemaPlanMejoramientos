using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Instructor
{
    public partial class FrmCrearPlan : System.Web.UI.Page
    {
        ClPlanMejoramientoL oPlanL = new ClPlanMejoramientoL();
        ClFichaL oFichaL = new ClFichaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["correo"] == null || Session["rol"] == null ||
                Session["rol"].ToString().ToUpper() != "INSTRUCTOR")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarAprendices();
                txtFechaLimite.Text = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd");
            }
        }

        private void CargarAprendices()
        {
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);
            DataTable dt = oPlanL.MtListarAprendicesPorInstructor(idInstructor);
            ddlAprendiz.Items.Clear();
            ddlAprendiz.Items.Add(new ListItem("-- Seleccione un aprendiz --", ""));
            foreach (DataRow r in dt.Rows)
            {
                string texto = r["nombreCompleto"] + " | Ficha: " + r["codigoFicha"] + " | Doc: " + r["numeroDocumento"];
                string valor = r["idAprendiz"] + "|" + r["idFicha"];
                ddlAprendiz.Items.Add(new ListItem(texto, valor));
            }
        }

        protected void ddlAprendiz_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlResultados.Controls.Clear();

            if (string.IsNullOrEmpty(ddlAprendiz.SelectedValue))
            {
                txtFicha.Text = "";
                hfIdFicha.Value = "0";
                StringBuilder sbVacio = new StringBuilder();
                sbVacio.Append("<div class='empty-resultados'>");
                sbVacio.Append("<i class='bi bi-arrow-up-circle' style='font-size:24px;display:block;margin-bottom:8px;'></i>");
                sbVacio.Append("Selecciona un aprendiz para ver los resultados disponibles.");
                sbVacio.Append("</div>");
                pnlResultados.Controls.Add(new LiteralControl(sbVacio.ToString()));
                return;
            }

            string[] partes = ddlAprendiz.SelectedValue.Split('|');
            int idFicha = Convert.ToInt32(partes[1]);
            hfIdFicha.Value = idFicha.ToString();
            txtFicha.Text = "Ficha: " + ddlAprendiz.SelectedItem.Text.Split('|')[1].Replace("Ficha:", "").Trim();

            CargarResultados(idFicha);
        }

        private void CargarResultados(int idFicha)
        {
            DataTable dt = oPlanL.MtListarResultadosPorFicha(idFicha);
            StringBuilder sb = new StringBuilder();

            if (dt.Rows.Count == 0)
            {
                sb.Append("<div class='empty-resultados'>No hay resultados de aprendizaje registrados para esta ficha.</div>");
                pnlResultados.Controls.Add(new LiteralControl(sb.ToString()));
                return;
            }

            sb.Append("<div class='resultados-wrap'>");
            string competenciaActual = "";
            foreach (DataRow r in dt.Rows)
            {
                string comp = r["nombreCompetencia"].ToString();
                if (comp != competenciaActual)
                {
                    competenciaActual = comp;
                    sb.Append("<div class='resultado-comp' style='padding:6px 10px 2px;'>" + comp + "</div>");
                }
                sb.Append("<div class='resultado-item'>");
                sb.Append("<input type='checkbox' name='chkResultados' value='" + r["idResultadoAprendizaje"] + "' id='chk_" + r["idResultadoAprendizaje"] + "' />");
                sb.Append("<label class='resultado-label' for='chk_" + r["idResultadoAprendizaje"] + "'>" + r["descripcion"] + "</label>");
                sb.Append("</div>");
            }
            sb.Append("</div>");
            pnlResultados.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlAprendiz.SelectedValue))
            {
                SetMensaje("warning", "Selecciona un aprendiz.");
                return;
            }

            string[] partes = ddlAprendiz.SelectedValue.Split('|');
            int idAprendiz = Convert.ToInt32(partes[0]);
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);

            string actividades = txtActividades.Text.Trim();
            string observaciones = txtObservaciones.Text.Trim();
            string fechaLimiteStr = txtFechaLimite.Text.Trim();

            if (string.IsNullOrEmpty(actividades))
            {
                SetMensaje("warning", "Describe las actividades del plan.");
                return;
            }

            if (string.IsNullOrEmpty(fechaLimiteStr))
            {
                SetMensaje("warning", "Selecciona una fecha límite.");
                return;
            }

            DateTime fechaLimite = Convert.ToDateTime(fechaLimiteStr);
            if (fechaLimite < DateTime.Today)
            {
                SetMensaje("warning", "La fecha límite no puede ser anterior a hoy.");
                return;
            }

            string[] resultadosSeleccionados = Request.Form.GetValues("chkResultados");
            if (resultadosSeleccionados == null || resultadosSeleccionados.Length == 0)
            {
                SetMensaje("warning", "Selecciona al menos un resultado de aprendizaje incumplido.");
                return;
            }

            int idPlan = oPlanL.MtCrearPlanMejoramiento(
                "Interno", DateTime.Today, fechaLimite,
                actividades, observaciones, "Pendiente",
                idAprendiz, idInstructor);

            if (idPlan > 0)
            {
                foreach (string idResultado in resultadosSeleccionados)
                    oPlanL.MtAsociarResultadoAPlan(idPlan, Convert.ToInt32(idResultado));

                SetMensaje("success", "¡Plan de mejoramiento creado exitosamente!");
                LimpiarFormulario();
            }
            else
            {
                SetMensaje("error", "No se pudo crear el plan. Verifica los datos.");
            }
        }

        private void LimpiarFormulario()
        {
            CargarAprendices();
            txtFicha.Text = "";
            hfIdFicha.Value = "0";
            txtActividades.Text = "";
            txtObservaciones.Text = "";
            txtFechaLimite.Text = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd");
            pnlResultados.Controls.Clear();
            pnlResultados.Controls.Add(new LiteralControl(
                "<div class='empty-resultados'><i class='bi bi-arrow-up-circle' style='font-size:24px;display:block;margin-bottom:8px;'></i>Selecciona un aprendiz para ver los resultados disponibles.</div>"));
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}