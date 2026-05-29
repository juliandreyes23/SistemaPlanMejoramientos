using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
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
            DataTable dt = oCentroL.MtListarCentros("");
            ddlCentro.DataSource = dt;
            ddlCentro.DataTextField = "nombre";
            ddlCentro.DataValueField = "idCentro";
            ddlCentro.DataBind();
            ddlCentro.Items.Insert(0, new ListItem("-- Seleccione Centro --", ""));
        }

        private void CargarProgramas()
        {
            DataTable dt = oProgramaL.MtListarProgramas(txtBuscar.Text.Trim());

            gvProgramas.PageIndex = (int)ViewState["PaginaActual"];
            gvProgramas.DataSource = dt;
            gvProgramas.DataBind();

            int totalPaginas = gvProgramas.PageCount;
            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = dt.Rows.Count.ToString();

            var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
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

                if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(nombre) ||
                    string.IsNullOrEmpty(nivel) || string.IsNullOrEmpty(ddlCentro.SelectedValue))
                {
                    SetMensaje("warning", "Por favor complete todos los campos obligatorios.");
                    return;
                }

                int idCentro = Convert.ToInt32(ddlCentro.SelectedValue);
                bool esNuevo = string.IsNullOrEmpty(hfIdPrograma.Value);

                if (esNuevo)
                {

                    bool dt = oProgramaL.MtObtenerProgramaPorCodigo(codigo);
                    if (dt = false)
                    {
                            bool insertado = oProgramaL.MtCrearPrograma(codigo, nombre, version, nivel, duracion, estado, idCentro);

                        if (insertado)
                        {
                            SetMensaje("success", "¡Programa registrado exitosamente!");
                            LimpiarFormulario();
                            CargarProgramas();
                        }
                        else
                        {
                            SetMensaje("error", "Error al registrar el programa.");
                        }
                    }
                    else
                    {
                        SetMensaje("error", "No se puede crear un Programa con Codigo ya Registrado.");
                    }

                    
                }
                else
                {
                    int idPrograma = Convert.ToInt32(hfIdPrograma.Value);
                    bool actualizado = oProgramaL.MtActualizarPrograma(idPrograma, codigo, nombre, version, nivel, duracion, estado, idCentro);
                    if (actualizado)
                    {
                        SetMensaje("success", "¡Programa modificado correctamente!");
                        LimpiarFormulario();
                        CargarProgramas();
                    }
                    else
                    {
                        SetMensaje("error", "Error al modificar el programa.");
                    }
                }
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Ocurrió un error: " + ex.Message);
            }
        }

        protected void gvProgramas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString())) return;

            if (e.CommandName == "Editar")
            {
                //int idPrograma = Convert.ToInt32(gvProgramas.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                int idPrograma = Convert.ToInt32(e.CommandArgument);
                hfIdPrograma.Value = idPrograma.ToString();

                DataTable dt = oProgramaL.MtObtenerProgramaPorId(idPrograma);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    txtCodigo.Text = r["codigoPrograma"].ToString();
                    txtNombre.Text = r["nombre"].ToString();
                    txtVersion.Text = r["version"].ToString();
                    txtDuracion.Text = r["duracion"].ToString();

                    if (ddlNivel.Items.FindByValue(r["nivel"].ToString()) != null)
                        ddlNivel.SelectedValue = r["nivel"].ToString();

                    if (ddlEstado.Items.FindByValue(r["estado"].ToString()) != null)
                        ddlEstado.SelectedValue = r["estado"].ToString();

                    if (ddlCentro.Items.FindByValue(r["idCentro"].ToString()) != null)
                        ddlCentro.SelectedValue = r["idCentro"].ToString();
                }

                lblTituloForm.Text = "Modificar Programa";
                btnGuardar.Text = "Actualizar Programa";
                btnCancelar.Visible = true;
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int idPrograma = Convert.ToInt32(e.CommandArgument);
                    bool eliminado = oProgramaL.MtEliminarPrograma(idPrograma);

                    if (eliminado)
                    {
                        SetMensaje("success", "¡Programa eliminado correctamente!");
                        CargarProgramas();
                        if (hfIdPrograma.Value == idPrograma.ToString()) LimpiarFormulario();
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar el programa. Puede tener fichas asociadas.");
                    }
                }
                catch (Exception ex)
                {
                    SetMensaje("error", "Error al intentar eliminar: " + ex.Message);
                }
            }
        }

        protected void gvProgramas_RowDataBound(object sender, GridViewRowEventArgs e) { }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vista/Dashboard.aspx");
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