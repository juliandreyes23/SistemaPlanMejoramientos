using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionCompetencias : Page
    {
        ClCompetenciaL oCompetenciaL = new ClCompetenciaL();
        ClProgramaL oProgramaL = new ClProgramaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProgramas();
                ViewState["PaginaActual"] = 0;
                CargarCompetencias("");
            }
            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";

        }

        private void CargarProgramas()
        {
            var lista = oProgramaL.MtListarProgramas();

            ddlPrograma.Items.Clear();
            ddlPrograma.Items.Add(new ListItem("-- Seleccione un programa --", "0"));

            foreach (var item in lista)
            {
                ddlPrograma.Items.Add(new ListItem(
                    item.codigoPrograma + " - " + item.nombre,
                    item.idPrograma.ToString()
                ));
            }
        }


        private void CargarCompetencias(string filtro)
        {
            List<ClCompetenciasM> lista;

            if (!string.IsNullOrWhiteSpace(filtro))
                lista = oCompetenciaL.MtBuscarCompetencias(filtro);
            else
                lista = oCompetenciaL.MtListarCompetencias();

            int pageSize = 10;
            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);

            int totalRegistros = lista.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

            if (paginaActual >= totalPaginas && totalPaginas > 0)
            {
                paginaActual = totalPaginas - 1;
                ViewState["PaginaActual"] = paginaActual;
            }

            ViewState["TotalPaginas"] = totalPaginas;

            gvCompetencias.PageIndex = paginaActual;
            gvCompetencias.DataSource = lista;
            gvCompetencias.DataBind();

            litPaginaActual.Text = (paginaActual + 1).ToString();
            litTotalPaginas.Text = totalPaginas == 0 ? "1" : totalPaginas.ToString();
            litTotalRegistros.Text = totalRegistros.ToString();

            rptPaginacion.DataSource = totalPaginas > 1
                ? Enumerable.Range(0, totalPaginas).ToList()
                : null;

            rptPaginacion.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text.Trim();
            int idPrograma = Convert.ToInt32(ddlPrograma.SelectedValue);

            if (string.IsNullOrWhiteSpace(descripcion) || idPrograma <= 0)
            {
                hfMensajeTipo.Value = "warning";
                hfMensajeTxt.Value = "Por favor completa todos los campos obligatorios.";
                CargarCompetencias(txtBuscar.Text.Trim());
                return;
            }

            int idCompetencia = 0;
            int.TryParse(hfIdCompetencia.Value, out idCompetencia);

            bool ok;
            string msg;

            if (idCompetencia > 0)
            {
                ok = oCompetenciaL.MtActualizarCompetencia(idCompetencia, descripcion, idPrograma);
                msg = ok ? "Competencia actualizada correctamente." : "Error al actualizar la competencia.";
            }
            else
            {
                ok = oCompetenciaL.MtCrearCompetencia(descripcion, idPrograma);
                msg = ok ? "Competencia registrada correctamente." : "Error al registrar la competencia.";
            }

            hfMensajeTipo.Value = ok ? "success" : "error";
            hfMensajeTxt.Value = msg;

            LimpiarFormulario();
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            CargarCompetencias("");
        }

        protected void gvCompetencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                List<ClCompetenciasM> lista = oCompetenciaL.MtListarCompetencias();

                var item = lista.FirstOrDefault(x => x.idCompetencia == id);

                if (item != null)
                {
                    hfIdCompetencia.Value = item.idCompetencia.ToString();

                    txtDescripcion.Text = item.descripcion;

                    if (item.programa != null)
                        ddlPrograma.SelectedValue = item.programa.idPrograma.ToString();

                    lblTituloForm.Text = "Actualizar Competencia";
                    btnGuardar.Text = "Actualizar Competencia";
                    btnCancelar.Visible = true;
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                bool ok = oCompetenciaL.MtEliminarCompetencia(id);

                hfMensajeTipo.Value = ok ? "success" : "error";
                hfMensajeTxt.Value = ok ? "Competencia eliminada correctamente." : "Error al eliminar.";

                LimpiarFormulario();
                ViewState["PaginaActual"] = 0;
                CargarCompetencias(txtBuscar.Text.Trim());
            }

            else if (e.CommandName == "Eliminar")
            {
                bool ok = oCompetenciaL.MtEliminarCompetencia(id);
                hfMensajeTipo.Value = ok ? "success" : "error";
                hfMensajeTxt.Value = ok ? "Competencia eliminada correctamente." : "Error al intentar eliminar.";
                LimpiarFormulario();
                ViewState["PaginaActual"] = 0;
                CargarCompetencias(txtBuscar.Text.Trim());
            }
        }

        protected void gvCompetencias_RowDataBound(object sender, GridViewRowEventArgs e) { }

        protected void gvCompetencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Pagina") return;
            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int totalPaginas = Convert.ToInt32(ViewState["TotalPaginas"]);

            if (e.CommandArgument.ToString() == "anterior")
                paginaActual = Math.Max(0, paginaActual - 1);
            else if (e.CommandArgument.ToString() == "siguiente")
                paginaActual = Math.Min(totalPaginas - 1, paginaActual + 1);
            else
                paginaActual = Convert.ToInt32(e.CommandArgument);

            ViewState["PaginaActual"] = paginaActual;
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vista/Dashboard.aspx");
        }

        private void LimpiarFormulario()
        {
            hfIdCompetencia.Value = "";
            txtDescripcion.Text = "";
            ddlPrograma.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Competencia";
            btnGuardar.Text = "Guardar Competencia";
            btnCancelar.Visible = false;
        }
    }
}