using sistemaPlanMejoramientos.Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Linq;

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

            List<ClFichasM> fichas = oFichaL.MtListarFichasPorInstructor(idInstructor);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                string f = filtro.ToLower();

                fichas = fichas
                    .Where(x =>
                        (x.codigoFicha != null && x.codigoFicha.ToLower().Contains(f)) ||
                        (x.programa?.nombre != null && x.programa.nombre.ToLower().Contains(f))
                    )
                    .ToList();
            }

            if (fichas.Count == 0)
            {
                pnlFichas.Visible = false;
                pnlVacio.Visible = true;
                return;
            }

            pnlFichas.Visible = true;
            pnlVacio.Visible = false;
            pnlFichas.Controls.Clear();

            StringBuilder sb = new StringBuilder();

            foreach (var ficha in fichas)
            {
                int idFicha = ficha.idFicha;

                string codigo = ficha.codigoFicha;
                string programa = ficha.programa?.nombre ?? "";
                string jornada = ficha.jornada;
                string estado = ficha.estado;
                string inicio = ficha.fechaInicio.ToString("dd/MM/yyyy");
                string fin = ficha.fechaFinalizacion.ToString("dd/MM/yyyy");

                var aprendices = oFichaL.MtListarAprendicesPorFicha(idFicha);
                int total = aprendices.Count;

                string pillEstado = estado == "En formacion" ? "pill-activa" : "pill-finalizada";

                sb.Append("<div class='ficha-card'>");

                sb.Append("<div class='ficha-header' onclick='toggleFicha(" + idFicha + ")'>");

                sb.Append("<div class='ficha-left'>");
                sb.Append("<div class='ficha-icon'><i class='bi bi-folder2-open'></i></div>");
                sb.Append("<div>");
                sb.Append("<div class='ficha-codigo'>Ficha " + codigo + "</div>");
                sb.Append("<div class='ficha-programa'>" + programa +
                          " &nbsp;|&nbsp; " + inicio + " – " + fin + "</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class='ficha-right'>");
                sb.Append("<span class='pill pill-jornada'>" + jornada + "</span>");
                sb.Append("<span class='pill " + pillEstado + "'>" + estado + "</span>");
                sb.Append("<span class='pill pill-count'><i class='bi bi-people-fill'></i> " + total + "</span>");
                sb.Append("<i class='bi bi-chevron-down chevron' id='chev_" + idFicha + "'></i>");
                sb.Append("</div>");

                sb.Append("</div>");

                sb.Append("<div class='aprendices-wrap' id='aprendices_" + idFicha + "'>");

                if (total == 0)
                {
                    sb.Append("<p style='color:#adb5bd;font-size:13px;padding:16px 0;'>No hay aprendices asignados.</p>");
                }
                else
                {
                    foreach (var ap in aprendices)
                    {
                        string nombres = ap.nombres;
                        string apellidos = ap.apellidos;
                        string doc = ap.tipoDocumento + " " + ap.numeroDocumento;

                        string estadoAp = ap.estadoAcademico;
                        string iniciales =
                            (nombres?.Length > 0 ? nombres[0].ToString() : "") +
                            (apellidos?.Length > 0 ? apellidos[0].ToString() : "");

                        string clase = ObtenerClaseEstado(estadoAp);

                        sb.Append("<div class='aprendiz-row'>");

                        sb.Append("<div class='aprendiz-left'>");
                        sb.Append("<div class='aprendiz-avatar'>" + iniciales + "</div>");
                        sb.Append("<div>");
                        sb.Append("<div class='aprendiz-name'>" + apellidos + ", " + nombres + "</div>");
                        sb.Append("<div class='aprendiz-doc'>" + doc + "</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        sb.Append("<span class='estado " + clase + "'>" + estadoAp + "</span>");

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