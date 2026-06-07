using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionCentros : System.Web.UI.Page
    {
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
                CargarCentros();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void CargarCentros()
        {
            var lista = oCentroL.MtListarCentros(txtBuscar.Text.Trim());

            gvCentros.PageIndex = (int)ViewState["PaginaActual"];
            gvCentros.DataSource = lista;
            gvCentros.DataBind();

            int totalPaginas = gvCentros.PageCount;
            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = lista.Count.ToString();

            var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
            rptPaginacion.DataSource = paginas;
            rptPaginacion.DataBind();
        }

        protected void gvCentros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarCentros();
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

            CargarCentros();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarCentros();
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            CargarCentros();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string regional = txtRegional.Text.Trim();
                string municipio = txtMunicipio.Text.Trim();
                string departamento = txtDepartamento.Text.Trim();
                string estado = ddlEstado.SelectedValue;

                bool esNuevo = string.IsNullOrEmpty(hfIdCentro.Value);

                if (esNuevo)
                {
                    bool insertado = oCentroL.MtCrearCentro(
                        codigo, nombre, regional, municipio, departamento, estado
                    );

                    if (insertado)
                    {
                        SetMensaje("success", "¡Centro registrado correctamente!");
                        LimpiarFormulario();
                        CargarCentros();
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo registrar el centro.");
                    }
                }
                else
                {
                    int idCentro = Convert.ToInt32(hfIdCentro.Value);

                    bool actualizado = oCentroL.MtActualizarCentro(
                        idCentro, codigo, nombre, regional, municipio, departamento, estado
                    );

                    if (actualizado)
                    {
                        SetMensaje("success", "¡Centro actualizado correctamente!");
                        LimpiarFormulario();
                        CargarCentros();
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo actualizar el centro.");
                    }
                }
            }
            catch (Exception ex)
            {
                SetMensaje("error", ex.Message);
            }
        }

        protected void gvCentros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString()))
                return;

            if (e.CommandName == "Editar")
            {
                try
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    hfIdCentro.Value = gvCentros.DataKeys[index].Value.ToString();

                    ClCentroM centro = oCentroL.MtObtenerCentroPorId(Convert.ToInt32(hfIdCentro.Value));

                    if (centro != null)
                    {
                        txtCodigo.Text = centro.codigoCentro;
                        txtNombre.Text = centro.nombre;
                        txtRegional.Text = centro.regional;
                        txtMunicipio.Text = centro.municipio;
                        txtDepartamento.Text = centro.departamento;
                        ddlEstado.SelectedValue = centro.estado;
                    }

                    lblTituloForm.Text = "Modificar Centro";
                    btnGuardar.Text = "Actualizar Centro";
                    btnCancelar.Visible = true;
                }
                catch (Exception ex)
                {
                    SetMensaje("error", "Error al cargar centro: " + ex.Message);
                }
            }

            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int idCentro = Convert.ToInt32(e.CommandArgument);

                    bool eliminado = oCentroL.MtEliminarCentro(idCentro);

                    if (eliminado)
                    {
                        SetMensaje("success", "Centro eliminado correctamente.");
                        CargarCentros();

                        if (hfIdCentro.Value == idCentro.ToString())
                            LimpiarFormulario();
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar el centro.");
                    }
                }
                catch (Exception ex)
                {
                    SetMensaje("error", ex.Message);
                }
            }
        }

        protected void gvCentros_RowDataBound(object sender, GridViewRowEventArgs e) { }

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
            hfIdCentro.Value = "";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtRegional.Text = "";
            txtMunicipio.Text = "";
            txtDepartamento.Text = "";
            ddlEstado.SelectedIndex = 0;

            lblTituloForm.Text = "Registrar Centro";
            btnGuardar.Text = "Guardar Centro";
            btnCancelar.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}