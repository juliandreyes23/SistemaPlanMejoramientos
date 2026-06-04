using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionFichas : System.Web.UI.Page
    {
        ClFichaL oFichaL = new ClFichaL();
        ClProgramaL oProgramaL = new ClProgramaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                LlenarProgramas();
                ListarFichas();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void LlenarProgramas()
        {
            try
            {
                DataTable dtProg = oProgramaL.MtListarProgramas();
                ddlPrograma.DataSource = dtProg;
                ddlPrograma.DataTextField = "nombre";
                ddlPrograma.DataValueField = "idPrograma";
                ddlPrograma.DataBind();
                ddlPrograma.Items.Insert(0, new ListItem("-- Seleccione un Programa --", ""));
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error al cargar programas: " + ex.Message);
            }
        }

        private void ListarFichas()
        {
            try
            {
                DataTable dtFichas = oFichaL.MtListarFichas(txtBuscar.Text.Trim());

                gvFichas.PageIndex = (int)ViewState["PaginaActual"];
                gvFichas.DataSource = dtFichas;
                gvFichas.DataBind();

                int totalPaginas = gvFichas.PageCount;
                ViewState["TotalPaginas"] = totalPaginas;

                litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
                litTotalPaginas.Text = totalPaginas.ToString();
                litTotalRegistros.Text = dtFichas.Rows.Count.ToString();

                var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
                rptPaginacion.DataSource = paginas;
                rptPaginacion.DataBind();
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error al listar fichas: " + ex.Message);
            }
        }

        protected void gvFichas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            ListarFichas();
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Pagina") return;

            int paginaActual = (int)ViewState["PaginaActual"];
            int totalPaginas = (int)ViewState["TotalPaginas"];

            if (e.CommandArgument.ToString() == "anterior")
            {
                if (paginaActual > 0) ViewState["PaginaActual"] = paginaActual - 1;
            }
            else if (e.CommandArgument.ToString() == "siguiente")
            {
                if (paginaActual < totalPaginas - 1) ViewState["PaginaActual"] = paginaActual + 1;
            }
            else
            {
                ViewState["PaginaActual"] = Convert.ToInt32(e.CommandArgument);
            }

            ListarFichas();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ListarFichas();
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ListarFichas();
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            ListarFichas();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigoFicha.Text.Trim();
                DateTime fInicio = Convert.ToDateTime(txtFechaInicio.Text);
                DateTime fFinal = Convert.ToDateTime(txtFechaFinal.Text);
                string jornada = ddlJornada.SelectedValue;
                string estado = ddlEstado.SelectedValue;
                int idPrograma = Convert.ToInt32(ddlPrograma.SelectedValue);

                bool resultado = false;
                bool esNuevo = string.IsNullOrEmpty(hfIdFicha.Value);

                if (esNuevo)
                {
                    if (oFichaL.MtExisteFicha(codigo))
                    {
                        SetMensaje("warning", "Ya existe una ficha registrada con ese código.");
                        return;
                    }

                    resultado = oFichaL.MtCrearFicha(codigo, fInicio, fFinal, jornada, estado, idPrograma);

                    if (!resultado)
                        SetMensaje("error", "El programa seleccionado no tiene un centro asignado.");
                }
                else
                {
                    int idFicha = Convert.ToInt32(hfIdFicha.Value);

                    if (oFichaL.MtExisteFichaEditar(idFicha, codigo))
                    {
                        SetMensaje("warning", "Ya existe otra ficha registrada con ese código.");
                        return;
                    }

                    resultado = oFichaL.MtActualizarFicha(idFicha, codigo, fInicio, fFinal, jornada, estado, idPrograma);
                }

                if (resultado)
                {
                    LimpiarFormulario();
                    ListarFichas();
                    SetMensaje("success", esNuevo ? "¡Ficha registrada con éxito!" : "¡Ficha actualizada con éxito!");
                }
                else if (esNuevo == false)
                {
                    SetMensaje("error", "No se pudo completar la operación en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error en el proceso: " + ex.Message);
            }
        }

        protected void gvFichas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString())) return;

            int idFicha = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                GridViewRow fila = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

                hfIdFicha.Value = idFicha.ToString();
                txtCodigoFicha.Text = fila.Cells[1].Text.Trim();

                string jornadaTabla = Server.HtmlDecode(fila.Cells[3].Text).Trim();
                ListItem itemJornada = ddlJornada.Items.FindByValue(jornadaTabla);
                if (itemJornada != null) ddlJornada.SelectedValue = itemJornada.Value;
                else
                {
                    ListItem itemJornadaTexto = ddlJornada.Items.FindByText(jornadaTabla);
                    if (itemJornadaTexto != null) ddlJornada.SelectedValue = itemJornadaTexto.Value;
                }

                DateTime fInicio = Convert.ToDateTime(fila.Cells[4].Text);
                DateTime fFin = Convert.ToDateTime(fila.Cells[5].Text);
                txtFechaInicio.Text = fInicio.ToString("yyyy-MM-dd");
                txtFechaFinal.Text = fFin.ToString("yyyy-MM-dd");

                string estadoTabla = Server.HtmlDecode(fila.Cells[6].Text).Trim();
                ListItem itemEstado = ddlEstado.Items.FindByValue(estadoTabla);
                if (itemEstado != null) ddlEstado.SelectedValue = itemEstado.Value;
                else
                {
                    ListItem itemEstadoTexto = ddlEstado.Items.FindByText(estadoTabla);
                    if (itemEstadoTexto != null) ddlEstado.SelectedValue = itemEstadoTexto.Value;
                }

                string nombreProgTabla = Server.HtmlDecode(fila.Cells[2].Text).Trim();
                ListItem itemProg = ddlPrograma.Items.FindByText(nombreProgTabla);
                if (itemProg != null) ddlPrograma.SelectedValue = itemProg.Value;

                lblTituloForm.Text = "Modificar Ficha: " + fila.Cells[1].Text;
                btnCancelar.Visible = true;
                btnGuardar.Text = "Actualizar Ficha";
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    bool eliminado = oFichaL.MtEliminarFicha(idFicha);
                    if (eliminado)
                    {
                        LimpiarFormulario();
                        ListarFichas();
                        SetMensaje("success", "¡Ficha eliminada correctamente!");
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar la ficha seleccionada.");
                    }
                }
                catch (Exception ex)
                {
                    SetMensaje("error", "Error al intentar eliminar: " + ex.Message);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }

        private void LimpiarFormulario()
        {
            hfIdFicha.Value = "";
            txtCodigoFicha.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFinal.Text = "";
            ddlJornada.SelectedIndex = 0;
            ddlPrograma.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Ficha";
            btnGuardar.Text = "Guardar Ficha";
            btnCancelar.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}