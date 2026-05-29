using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Aprendiz
{
    public partial class FrmMisPlanes : System.Web.UI.Page
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
                CargarPlanes();
        }

        private void CargarPlanes()
        {
            int idAprendiz = Convert.ToInt32(Session["idAprendiz"]);
            DataTable dt = oEvidenciaL.MtListarPlanesPorAprendiz(idAprendiz);

            if (dt.Rows.Count == 0)
            {
                rptPlanes.DataSource = dt;
                rptPlanes.DataBind();
                MostrarMensaje("No tienes planes de mejoramiento asignados.", "info");
                return;
            }

            rptPlanes.DataSource = dt;
            rptPlanes.DataBind();
        }

        protected void rptPlanes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem) return;

            DataRowView row = (DataRowView)e.Item.DataItem;
            int idPlan = Convert.ToInt32(row["idPlanMejoramiento"]);

            Literal litResultados = (Literal)e.Item.FindControl("litResultados");
            DataTable dtRaps = oEvidenciaL.MtListarResultadosPorPlan(idPlan);
            if (dtRaps.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class='rap-list' style='margin-top:6px;'>");
                foreach (DataRow r in dtRaps.Rows)
                    sb.Append("<li>" + r["descripcion"].ToString() + "</li>");
                sb.Append("</ul>");
                litResultados.Text = sb.ToString();
            }

            Literal litEvaluacion = (Literal)e.Item.FindControl("litEvaluacion");
            string producto = row["criterioProducto"] == DBNull.Value ? "" : row["criterioProducto"].ToString();
            string conocimiento = row["criterioConocimiento"] == DBNull.Value ? "" : row["criterioConocimiento"].ToString();
            string desempeno = row["criterioDesempeno"] == DBNull.Value ? "" : row["criterioDesempeno"].ToString();
            string obsEval = row["observacionesEvaluacion"] == DBNull.Value ? "" : row["observacionesEvaluacion"].ToString();

            if (!string.IsNullOrEmpty(producto))
            {
                StringBuilder sbEval = new StringBuilder();
                sbEval.Append("<div class='eval-panel'>");
                sbEval.Append("<div style='font-weight:600;color:#042940;margin-bottom:4px;'>Evaluación del instructor:</div>");
                sbEval.Append("<div class='eval-row'>");
                sbEval.Append(BadgeCriterio("Producto", producto));
                sbEval.Append(BadgeCriterio("Conocimiento", conocimiento));
                sbEval.Append(BadgeCriterio("Desempeño", desempeno));
                sbEval.Append("</div>");
                if (!string.IsNullOrEmpty(obsEval))
                    sbEval.Append($"<div style='margin-top:6px;color:#555;font-size:11px;'><b>Obs:</b> {obsEval}</div>");
                sbEval.Append("</div>");
                litEvaluacion.Text = sbEval.ToString();
            }

            HyperLink hlSubir = (HyperLink)e.Item.FindControl("hlSubir");
            string estadoPlan = row["estadoPlan"].ToString();
            if (estadoPlan == "Aprobado" || estadoPlan == "No Aprobado")
                hlSubir.CssClass = "btn-subir disabled";
        }

        private string BadgeCriterio(string nombre, string valor)
        {
            string css = valor == "Aprobado" ? "badge-aprobado" : "badge-noaprobado";
            return $"<span class='badge {css}'>{nombre}: {valor}</span>";
        }

        protected string ObtenerCssTipo(string tipo)
        {
            return tipo == "Comité" ? "badge badge-comite" : "badge badge-interno";
        }

        protected string ObtenerCssEstado(string estado)
        {
            switch (estado)
            {
                case "Aprobado": return "badge badge-aprobado";
                case "No Aprobado": return "badge badge-noaprobado";
                default: return "badge badge-pendiente";
            }
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = texto;
            switch (tipo)
            {
                case "success": lblMensaje.Style["background"] = "#D1E7DD"; lblMensaje.Style["color"] = "#0A5239"; break;
                case "info": lblMensaje.Style["background"] = "#D1ECF1"; lblMensaje.Style["color"] = "#0C5460"; break;
                default: lblMensaje.Style["background"] = "#FFF3CD"; lblMensaje.Style["color"] = "#856404"; break;
            }
        }
    }
}