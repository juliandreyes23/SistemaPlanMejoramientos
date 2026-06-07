using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System.Collections.Generic;

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
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
                CargarPlanes();
        }

        private void CargarPlanes()
        {
            int idAprendiz = Convert.ToInt32(Session["idAprendiz"]);

            List<ClPlanMejoramientoM> planes =
                oEvidenciaL.MtListarPlanesPorAprendiz(idAprendiz);

            if (planes == null || planes.Count == 0)
            {
                rptPlanes.DataSource = null;
                rptPlanes.DataBind();
                MostrarMensaje("No tienes planes de mejoramiento asignados.", "info");
                return;
            }

            rptPlanes.DataSource = planes;
            rptPlanes.DataBind();
        }

        protected void rptPlanes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            ClPlanMejoramientoM plan = (ClPlanMejoramientoM)e.Item.DataItem;

            Literal litResultados = (Literal)e.Item.FindControl("litResultados");
            Literal litEvaluacion = (Literal)e.Item.FindControl("litEvaluacion");
            HyperLink hlSubir = (HyperLink)e.Item.FindControl("hlSubir");

            if (litResultados != null && plan.resultados != null && plan.resultados.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class='rap-list' style='margin-top:6px;'>");
                foreach (var r in plan.resultados)
                    sb.Append("<li>" + ObtenerTextoResultado(r) + "</li>");
                sb.Append("</ul>");
                litResultados.Text = sb.ToString();
            }

            if (litEvaluacion != null && !string.IsNullOrEmpty(plan.criterioProducto))
            {
                StringBuilder sbEval = new StringBuilder();
                sbEval.Append("<div class='eval-panel'>");
                sbEval.Append("<div style='font-weight:600;color:#042940;margin-bottom:4px;'>Evaluación del instructor:</div>");
                sbEval.Append("<div class='eval-row'>");
                sbEval.Append(BadgeCriterio("Producto", plan.criterioProducto));
                sbEval.Append(BadgeCriterio("Conocimiento", plan.criterioConocimiento));
                sbEval.Append(BadgeCriterio("Desempeño", plan.criterioDesempeno));
                sbEval.Append("</div>");

                if (!string.IsNullOrWhiteSpace(plan.observacionesEvaluacion))
                {
                    sbEval.Append("<div style='margin-top:8px;padding-top:8px;border-top:1px solid #e0e6ed;'>");
                    sbEval.Append("<span style='font-weight:600;color:#042940;'>Observaciones: </span>");
                    sbEval.Append("<span style='color:#555;'>" + plan.observacionesEvaluacion + "</span>");
                    sbEval.Append("</div>");
                }

                sbEval.Append("</div>");
                litEvaluacion.Text = sbEval.ToString();
            }

            if (hlSubir != null)
            {
                if (plan.estadoPlan == "Aprobado" || plan.estadoPlan == "No Aprobado")
                    hlSubir.CssClass = "btn-subir disabled";
            }
        }

        private string ObtenerTextoResultado(ClPlanResultadosM r)
        {
            if (r == null) return "";

            var prop = r.GetType().GetProperties();

            foreach (var p in prop)
            {
                var val = p.GetValue(r);
                if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    return val.ToString();
            }

            return "Resultado sin descripción";
        }

        private string BadgeCriterio(string nombre, string valor)
        {
            string estado = string.IsNullOrEmpty(valor) ? "Pendiente" : valor;

            string css =
                estado == "Aprobado" ? "badge-aprobado" :
                estado == "No Aprobado" ? "badge-noaprobado" :
                "badge-pendiente";

            return $"<span class='badge {css}'>{nombre}: {estado}</span>";
        }

        protected string ObtenerCssTipo(string tipo)
        {
            return tipo == "Comité"
                ? "badge badge-comite"
                : "badge badge-interno";
        }

        protected string ObtenerCssEstado(string estado)
        {
            switch (estado)
            {
                case "Aprobado":
                    return "badge badge-aprobado";

                case "No Aprobado":
                    return "badge badge-noaprobado";

                default:
                    return "badge badge-pendiente";
            }
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = texto;

            switch (tipo)
            {
                case "success":
                    lblMensaje.Style["background"] = "#D1E7DD";
                    lblMensaje.Style["color"] = "#0A5239";
                    break;

                case "info":
                    lblMensaje.Style["background"] = "#D1ECF1";
                    lblMensaje.Style["color"] = "#0C5460";
                    break;

                default:
                    lblMensaje.Style["background"] = "#FFF3CD";
                    lblMensaje.Style["color"] = "#856404";
                    break;
            }
        }
    }
}