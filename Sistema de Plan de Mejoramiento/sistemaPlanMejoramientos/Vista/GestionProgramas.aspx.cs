using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionProgramas : System.Web.UI.Page
    {
        ClProgramaL oProgramaL = new ClProgramaL();
        ClCentroL oCentroL = new ClCentroL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rol"] == null || Session["rol"].ToString().ToUpper() != "ADMINISTRADOR")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                CargarCentrosDropdown();
                CargarProgramas();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void CargarCentrosDropdown()
        {
            var lista = oCentroL.MtListarCentrosActivos();

            ddlCentro.DataSource = lista;
            ddlCentro.DataTextField = "nombre";
            ddlCentro.DataValueField = "idCentro";
            ddlCentro.DataBind();

            ddlCentro.Items.Insert(0, new ListItem("-- Seleccione Centro --", ""));
        }

        private void CargarProgramas()
        {
            var lista = oProgramaL.MtListarProgramas(txtBuscar.Text.Trim());

            gvProgramas.PageIndex = (int)ViewState["PaginaActual"];
            gvProgramas.DataSource = lista;
            gvProgramas.DataBind();

            int totalPaginas = gvProgramas.PageCount;
            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = lista.Count.ToString();

            var paginas = Enumerable.Range(0, totalPaginas).ToList();
            rptPaginacion.DataSource = paginas;
            rptPaginacion.DataBind();
        }

        protected void gvProgramas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarProgramas();
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int paginaActual = (int)ViewState["PaginaActual"];
            int totalPaginas = (int)ViewState["TotalPaginas"];

            if (e.CommandArgument.ToString() == "anterior")
            {
                if (paginaActual > 0)
                    ViewState["PaginaActual"] = paginaActual - 1;
            }
            else if (e.CommandArgument.ToString() == "siguiente")
            {
                if (paginaActual < totalPaginas - 1)
                    ViewState["PaginaActual"] = paginaActual + 1;
            }
            else
            {
                ViewState["PaginaActual"] = Convert.ToInt32(e.CommandArgument);
            }

            CargarProgramas();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarProgramas();
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            CargarProgramas();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string version = txtVersion.Text.Trim();
                string nivel = ddlNivel.SelectedValue;
                string duracion = txtDuracion.Text.Trim();
                string estado = ddlEstado.SelectedValue;

                if (string.IsNullOrEmpty(codigo) ||
                    string.IsNullOrEmpty(nombre) ||
                    string.IsNullOrEmpty(nivel) ||
                    string.IsNullOrEmpty(ddlCentro.SelectedValue))
                {
                    SetMensaje("warning", "Complete los campos obligatorios");
                    return;
                }

                int idCentro = Convert.ToInt32(ddlCentro.SelectedValue);
                bool esNuevo = string.IsNullOrEmpty(hfIdPrograma.Value);

                if (esNuevo)
                {
                    if (oProgramaL.MtObtenerProgramaPorCodigo(codigo))
                    {
                        SetMensaje("warning", "Ya existe ese código");
                        return;
                    }

                    bool ok = oProgramaL.MtCrearPrograma(codigo, nombre, version, nivel, duracion, estado, idCentro);

                    SetMensaje(ok ? "success" : "error",
                        ok ? "Programa creado" : "Error al crear");

                    if (ok)
                    {
                        LimpiarFormulario();
                        CargarProgramas();
                    }
                }
                else
                {
                    int idPrograma = Convert.ToInt32(hfIdPrograma.Value);

                    bool duplicado = oProgramaL.MtObtenerProgramaPorCodigoExcluyendo(codigo, idPrograma);
                    if (duplicado)
                    {
                        SetMensaje("warning", "Código ya existe");
                        return;
                    }

                    bool ok = oProgramaL.MtActualizarPrograma(
                        idPrograma, codigo, nombre, version, nivel, duracion, estado, idCentro);

                    SetMensaje(ok ? "success" : "error",
                        ok ? "Programa actualizado" : "Error al actualizar");

                    if (ok)
                    {
                        LimpiarFormulario();
                        CargarProgramas();
                    }
                }
            }
            catch (Exception ex)
            {
                SetMensaje("error", ex.Message);
            }
        }

        protected void gvProgramas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;

            int idPrograma = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                var prog = oProgramaL.MtObtenerProgramaPorId(idPrograma);

                if (prog != null)
                {
                    hfIdPrograma.Value = prog.idPrograma.ToString();
                    txtCodigo.Text = prog.codigoPrograma;
                    txtNombre.Text = prog.nombre;
                    txtVersion.Text = prog.version;
                    txtDuracion.Text = prog.duracion;

                    ddlNivel.SelectedValue = prog.nivel;
                    ddlEstado.SelectedValue = prog.estado;
                    ddlCentro.SelectedValue = prog.idCentro.ToString();
                }

                lblTituloForm.Text = "Modificar Programa";
                btnGuardar.Text = "Actualizar Programa";
                btnCancelar.Visible = true;
            }
            else if (e.CommandName == "Eliminar")
            {
                bool ok = oProgramaL.MtEliminarPrograma(idPrograma);

                SetMensaje(ok ? "success" : "error",
                    ok ? "Eliminado" : "No se pudo eliminar");

                CargarProgramas();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vista/Dashboard.aspx");
        }
        protected void gvProgramas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                if (btnEliminar != null)
                    btnEliminar.Attributes.Add("onclick", "return confirmarEliminar(this);");

                LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                if (btnEditar != null)
                    btnEditar.Attributes.Add("onclick", "return confirmarEditar(this);");
            }
        }
        private void LimpiarFormulario()
        {
            hfIdPrograma.Value = "";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtVersion.Text = "";
            txtDuracion.Text = "";
            ddlNivel.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            ddlCentro.SelectedIndex = 0;

            lblTituloForm.Text = "Registrar Programa";
            btnGuardar.Text = "Guardar Programa";
            btnCancelar.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}