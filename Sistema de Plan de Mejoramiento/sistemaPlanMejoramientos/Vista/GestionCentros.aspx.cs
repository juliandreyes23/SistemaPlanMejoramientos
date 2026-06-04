using sistemaPlanMejoramientos.Logica;
using System;
using System.Collections.Generic;
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
                Response.Redirect("~/Dashboard.aspx");
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
            DataTable dt = oCentroL.MtListarCentros(txtBuscar.Text.Trim());

            gvCentros.PageIndex = (int)ViewState["PaginaActual"];
            gvCentros.DataSource = dt;
            gvCentros.DataBind();

            int totalPaginas = gvCentros.PageCount;
            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = dt.Rows.Count.ToString();

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

                if (string.IsNullOrEmpty(codigo))
                {
                    SetMensaje("warning", "El código del centro es obligatorio.");
                    return;
                }
                if (string.IsNullOrEmpty(nombre))
                {
                    SetMensaje("warning", "El nombre del centro es obligatorio.");
                    return;
                }

                bool esNuevo = string.IsNullOrEmpty(hfIdCentro.Value);

                if (esNuevo)
                {
                    bool insertado = oCentroL.MtCrearCentro(codigo, nombre, regional, municipio, departamento, estado);
                    SetMensaje(insertado ? "success" : "error",
                               insertado ? "¡Centro registrado exitosamente!" : "Error al registrar el centro.");
                    if (insertado) { LimpiarFormulario(); CargarCentros(); }
                }
                else
                {
                    int idCentro = Convert.ToInt32(hfIdCentro.Value);
                    bool actualizado = oCentroL.MtActualizarCentro(idCentro, codigo, nombre, regional, municipio, departamento, estado);
                    SetMensaje(actualizado ? "success" : "error",
                               actualizado ? "¡Centro modificado correctamente!" : "Error al modificar el centro.");
                    if (actualizado) { LimpiarFormulario(); CargarCentros(); }
                }
            }
            catch (Exception ex)
            {
                SetMensaje("warning", "Ocurrió un error: " + ex.Message);
            }
        }

        protected void gvCentros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString())) return;

            if (e.CommandName == "Editar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                hfIdCentro.Value = gvCentros.DataKeys[index].Value.ToString();

                DataTable dt = oCentroL.MtObtenerCentroPorId(Convert.ToInt32(hfIdCentro.Value));
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    txtCodigo.Text = r["codigoCentro"].ToString();
                    txtNombre.Text = r["nombre"].ToString();
                    txtRegional.Text = r["regional"].ToString();
                    txtMunicipio.Text = r["municipio"].ToString();
                    txtDepartamento.Text = r["departamento"].ToString();

                    if (ddlEstado.Items.FindByValue(r["estado"].ToString()) != null)
                        ddlEstado.SelectedValue = r["estado"].ToString();
                }

                lblTituloForm.Text = "Modificar Centro";
                btnGuardar.Text = "Actualizar Centro";
                btnCancelar.Visible = true;
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int idCentro = Convert.ToInt32(e.CommandArgument);
                    bool eliminado = oCentroL.MtEliminarCentro(idCentro);

                    if (eliminado)
                    {
                        SetMensaje("success", "¡Centro eliminado correctamente!");
                        CargarCentros();
                        if (hfIdCentro.Value == idCentro.ToString()) LimpiarFormulario();
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar. Puede tener programas o fichas asociadas.");
                    }
                }
                catch (Exception ex)
                {
                    SetMensaje("error", "Error al intentar eliminar: " + ex.Message);
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