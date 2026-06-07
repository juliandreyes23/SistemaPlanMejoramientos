using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class ResultadosAprendizaje : Page
    {
        ClResultadoAprendizajeL oLogica = new ClResultadoAprendizajeL();
        ClCompetenciaL oCompetencia = new ClCompetenciaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                ViewState["FiltroActual"] = "";

                CargarProgramas();
                CargarGrilla("");
            }
            else
            {
                hfMensajeTipo.Value = "";
                hfMensajeTxt.Value = "";
            }
        }

        private void CargarProgramas()
        {
            ddlPrograma.Items.Clear();
            ddlPrograma.Items.Add(new ListItem("-- Seleccione un programa --", "0"));

            var programas = oLogica.MtCargarPrograma(); 

            foreach (var p in programas)
            {
                ddlPrograma.Items.Add(new ListItem(p.nombre, p.idPrograma.ToString()));
            }
        }

        protected void ddlPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPrograma = int.Parse(ddlPrograma.SelectedValue);

            ddlCompetencia.Items.Clear();
            ddlCompetencia.Items.Add(new ListItem("-- Seleccione una competencia --", "0"));

            if (idPrograma > 0)
            {
                var competencias = oCompetencia.MtCargarCompetencias(idPrograma);

                foreach (var c in competencias)
                {
                    ddlCompetencia.Items.Add(new ListItem(
                        c.descripcion,
                        c.idCompetencia.ToString()
                    ));
                }
            }
        }

        private void CargarGrilla(string filtro)
        {
            var lista = oLogica.MtListarResultadoAprendizaje(); 

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                string f = filtro.ToLower();

                lista = lista.Where(x =>
                    (x.descripcion ?? "").ToLower().Contains(f) ||
                    (x.nombreCompetencia ?? "").ToLower().Contains(f) ||
                    (x.competencia?.programa?.nombre ?? "").ToLower().Contains(f)
                ).ToList();
            }

            var data = lista.Select(x => new
            {
                idResultadoAprendizaje = x.idResultadoAprendizaje,
                DescripcionResultado = x.descripcion,
                DescripcionCompetencia = x.nombreCompetencia,
                NombrePrograma = x.competencia.programa.nombre 
            }).ToList();

            int pageSize = gvResultados.PageSize;
            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);

            int totalRegistros = data.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);
            if (totalPaginas == 0) totalPaginas = 1;

            if (paginaActual >= totalPaginas)
                paginaActual = totalPaginas - 1;

            ViewState["PaginaActual"] = paginaActual;
            ViewState["TotalPaginas"] = totalPaginas;

            var paged = data
                .Skip(paginaActual * pageSize)
                .Take(pageSize)
                .ToList();

            gvResultados.DataSource = paged;
            gvResultados.DataBind();

            litPaginaActual.Text = (paginaActual + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = totalRegistros.ToString();

            rptPaginacion.DataSource = Enumerable.Range(0, totalPaginas);
            rptPaginacion.DataBind();
        }

        protected void gvResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarGrilla(ViewState["FiltroActual"] as string);
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Pagina") return;

            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int totalPaginas = Convert.ToInt32(ViewState["TotalPaginas"]);

            string arg = e.CommandArgument.ToString();

            if (arg == "anterior")
            {
                if (paginaActual > 0) paginaActual--;
            }
            else if (arg == "siguiente")
            {
                if (paginaActual < totalPaginas - 1) paginaActual++;
            }
            else
            {
                paginaActual = int.Parse(arg);
            }

            ViewState["PaginaActual"] = paginaActual;
            CargarGrilla(ViewState["FiltroActual"] as string);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idPrograma = int.Parse(ddlPrograma.SelectedValue);
            int idCompetencia = int.Parse(ddlCompetencia.SelectedValue);
            string descripcion = txtDescripcion.Text.Trim();
            int idResultado = int.Parse(hfIdResultado.Value);

            if (idPrograma <= 0 || idCompetencia <= 0 || string.IsNullOrWhiteSpace(descripcion))
            {
                SetMensaje("warning", "Complete todos los campos obligatorios.");
                CargarGrilla(ViewState["FiltroActual"] as string);
                return;
            }

            bool ok;

            if (idResultado == 0)
                ok = oLogica.MtCrearResultado(descripcion, idCompetencia);
            else
                ok = oLogica.MtActualizarResultado(idResultado, descripcion, idCompetencia);

            SetMensaje(ok ? "success" : "error",
                ok ? "Operación realizada correctamente." : "Error al guardar.");

            if (ok) LimpiarFormulario();

            ViewState["PaginaActual"] = 0;
            CargarGrilla(null);
        }

        protected void gvResultados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            var lista = oLogica.MtListarResultadoAprendizaje();
            var item = lista.FirstOrDefault(x => x.idResultadoAprendizaje == id);

            if (item == null) return;

            if (e.CommandName == "Eliminar")
            {
                bool ok = oLogica.MtEliminarResultado(id);

                SetMensaje(ok ? "success" : "error",
                    ok ? "Eliminado correctamente." : "Error al eliminar.");

                CargarGrilla(ViewState["FiltroActual"] as string);
            }

            if (e.CommandName == "Editar")
            {
                hfIdResultado.Value = item.idResultadoAprendizaje.ToString();
                txtDescripcion.Text = item.descripcion;

                CargarProgramas();
                ddlPrograma.SelectedValue = "0";

                ddlCompetencia.Items.Clear();
                ddlCompetencia.Items.Add(new ListItem("-- Seleccione una competencia --", "0"));

                ddlCompetencia.SelectedValue = item.idCompetencia.ToString();

                lblTituloForm.Text = "Actualizar Resultado";
                btnGuardar.Text = "Actualizar Resultado";
                btnCancelar.Visible = true;

                CargarGrilla(ViewState["FiltroActual"] as string);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ViewState["FiltroActual"] = txtBuscar.Text.Trim();
            CargarGrilla(ViewState["FiltroActual"] as string);
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ViewState["FiltroActual"] = "";
            ViewState["PaginaActual"] = 0;

            txtBuscar.Text = "";

            CargarGrilla("");
        }
        protected void gvResultados_RowDataBound(object sender, GridViewRowEventArgs e)
{

}
        private void LimpiarFormulario()
        {
            hfIdResultado.Value = "0";
            txtDescripcion.Text = "";
            ddlPrograma.SelectedIndex = 0;

            ddlCompetencia.Items.Clear();
            ddlCompetencia.Items.Add(new ListItem("-- Seleccione una competencia --", "0"));

            lblTituloForm.Text = "Registrar Resultado";
            btnGuardar.Text = "Guardar Resultado";
            btnCancelar.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}