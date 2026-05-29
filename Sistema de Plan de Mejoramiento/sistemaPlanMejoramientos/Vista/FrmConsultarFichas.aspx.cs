using System;
using System.Data;
using System.Text;
using System.Web.UI;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Instructor
{
    public partial class FrmConsultarFichas : System.Web.UI.Page
    {
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
                RenderFichas("");
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            RenderFichas(txtBuscar.Text.Trim());
        }

        private void RenderFichas(string filtro)
        {
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);
            DataTable dtFichas = oFichaL.MtListarFichasPorInstructor(idInstructor);

            if (!string.IsNullOrEmpty(filtro))
            {
                string f = filtro.ToLower();
                DataRow[] filas = dtFichas.Select(
                    "codigoFicha LIKE '%" + f + "%' OR nombrePrograma LIKE '%" + f + "%'");
                DataTable dtFiltrado = dtFichas.Clone();
                foreach (DataRow r in filas) dtFiltrado.ImportRow(r);
                dtFichas = dtFiltrado;
            }

            if (dtFichas.Rows.Count == 0)
            {
                pnlFichas.Visible = false;
                pnlVacio.Visible = true;
                return;
            }

            pnlFichas.Visible = true;
            pnlVacio.Visible = false;
            pnlFichas.Controls.Clear();

            StringBuilder sb = new StringBuilder();

            foreach (DataRow ficha in dtFichas.Rows)
            {
                int idFicha = Convert.ToInt32(ficha["idFicha"]);
                string codigo = ficha["codigoFicha"].ToString();
                string programa = ficha["nombrePrograma"].ToString();
                string jornada = ficha["jornada"].ToString();
                string estado = ficha["estado"].ToString();
                string inicio = Convert.ToDateTime(ficha["fechaInicio"]).ToString("dd/MM/yyyy");
                string fin = Convert.ToDateTime(ficha["fechaFinalizacion"]).ToString("dd/MM/yyyy");

                DataTable dtAprendices = oFichaL.MtListarAprendicesPorFicha(idFicha);
                int totalAprendices = dtAprendices.Rows.Count;

                string pillEstado = estado == "En formacion" ? "pill-activa" : "pill-finalizada";

                sb.Append("<div class='ficha-card'>");
                sb.Append("<div class='ficha-header' onclick='toggleFicha(" + idFicha + ")'>");
                sb.Append("<div class='ficha-left'>");
                sb.Append("<div class='ficha-icon'><i class='bi bi-folder2-open'></i></div>");
                sb.Append("<div>");
                sb.Append("<div class='ficha-codigo'>Ficha " + codigo + "</div>");
                sb.Append("<div class='ficha-programa'>" + programa + " &nbsp;|&nbsp; " + inicio + " – " + fin + "</div>");
                sb.Append("</div></div>");
                sb.Append("<div class='ficha-right'>");
                sb.Append("<span class='pill pill-jornada'>" + jornada + "</span>");
                sb.Append("<span class='pill " + pillEstado + "'>" + estado + "</span>");
                sb.Append("<span class='pill pill-count'><i class='bi bi-people-fill'></i> " + totalAprendices + "</span>");
                sb.Append("<i class='bi bi-chevron-down chevron' id='chev_" + idFicha + "'></i>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class='aprendices-wrap' id='aprendices_" + idFicha + "'>");

                if (totalAprendices == 0)
                {
                    sb.Append("<p style='color:#adb5bd;font-size:13px;padding:16px 0;'>No hay aprendices asignados a esta ficha.</p>");
                }
                else
                {
                    foreach (DataRow ap in dtAprendices.Rows)
                    {
                        string nombres = ap["nombres"].ToString();
                        string apellidos = ap["apellidos"].ToString();
                        string doc = ap["tipoDocumento"] + " " + ap["numeroDocumento"];
                        string estadoAp = ap["estadoAcademico"].ToString();
                        string iniciales = (nombres.Length > 0 ? nombres[0].ToString() : "") +
                                           (apellidos.Length > 0 ? apellidos[0].ToString() : "");
                        string claseEstado = ObtenerClaseEstado(estadoAp);

                        sb.Append("<div class='aprendiz-row'>");
                        sb.Append("<div class='aprendiz-left'>");
                        sb.Append("<div class='aprendiz-avatar'>" + iniciales + "</div>");
                        sb.Append("<div>");
                        sb.Append("<div class='aprendiz-name'>" + apellidos + ", " + nombres + "</div>");
                        sb.Append("<div class='aprendiz-doc'>" + doc + "</div>");
                        sb.Append("</div></div>");
                        sb.Append("<span class='estado " + claseEstado + "'>" + estadoAp + "</span>");
                        sb.Append("</div>");
                    }
                }

                sb.Append("</div>");
                sb.Append("</div>");
            }

            pnlFichas.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private string ObtenerClaseEstado(string estado)
        {
            switch (estado)
            {
                case "En formación": return "estado-formacion";
                case "Aplazado": return "estado-aplazado";
                case "Desertado": return "estado-desertado";
                case "Retiro voluntario": return "estado-retiroVoluntario";
                case "Condicionado": return "estado-condicionado";
                case "Cancelado": return "estado-cancelado";
                case "Certificado": return "estado-certificado";
                default: return "estado-default";
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Vista/FrmLogin.aspx");
        }
    }
}