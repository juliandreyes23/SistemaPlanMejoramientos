using sistemaPlanMejoramientos.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Modelo;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionAprendices : System.Web.UI.Page
    {
        ClAprendizL oAprendizL = new ClAprendizL();
        ClFichaL oFichaL = new ClFichaL();
        ClUsuarioL oUsuarioL = new ClUsuarioL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                CargarFichas();
                ListarAprendices();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void ListarAprendices()
        {
            try
            {
                List<ClAprendizM> lista = oAprendizL.MtListarAprendices(txtBuscar.Text.Trim());

                gvAprendices.PageIndex = (int)ViewState["PaginaActual"];
                gvAprendices.DataSource = lista;
                gvAprendices.DataBind();

                int totalPaginas = gvAprendices.PageCount;
                ViewState["TotalPaginas"] = totalPaginas;

                litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
                litTotalPaginas.Text = totalPaginas.ToString();
                litTotalRegistros.Text = lista.Count.ToString();

                var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
                rptPaginacion.DataSource = paginas;
                rptPaginacion.DataBind();
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error al listar aprendices: " + ex.Message);
            }
        }

        private void CargarFichas()
        {
            try
            {
                var dtFichas = oFichaL.MtListarFichas();
                ddlFicha.DataSource = dtFichas;
                ddlFicha.DataTextField = "codigoFicha";
                ddlFicha.DataValueField = "idFicha";
                ddlFicha.DataBind();
                ddlFicha.Items.Insert(0, new ListItem("-- Seleccione Ficha --", ""));
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error cargando fichas: " + ex.Message);
            }
        }

        protected void gvAprendices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            ListarAprendices();
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

            ListarAprendices();
        }

        protected void gvAprendices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;

            if (e.CommandName == "Editar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idAprendiz = Convert.ToInt32(gvAprendices.DataKeys[index]["idAprendiz"]);

                var lista = oAprendizL.MtListarAprendices("");
                var a = lista.FirstOrDefault(x => x.idAprendiz == idAprendiz);
                if (a == null) return;

                CargarFichas();

                hfIdAprendiz.Value = a.idAprendiz.ToString();
                txtDocumento.Text = a.numeroDocumento;
                txtNombres.Text = a.nombres;
                txtApellidos.Text = a.apellidos;
                txtCorreo.Text = a.correo;
                txtTelefono.Text = a.telefono;

                if (ddlTipoDoc.Items.FindByValue(a.tipoDocumento) != null)
                    ddlTipoDoc.SelectedValue = a.tipoDocumento;

                if (ddlEstadoAcademico.Items.FindByValue(a.estadoAcademico) != null)
                    ddlEstadoAcademico.SelectedValue = a.estadoAcademico;

                if (ddlFicha.Items.FindByValue(a.idFicha.ToString()) != null)
                    ddlFicha.SelectedValue = a.idFicha.ToString();

                lblTituloForm.Text = "Editar Aprendiz";
                btnGuardar.Text = "Actualizar Aprendiz";
                hfAbrirModal.Value = "editar";

                ListarAprendices();
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int idAprendiz = Convert.ToInt32(e.CommandArgument);
                    int idUsuario = oAprendizL.MtObtenerIdUsuarioPorAprendiz(idAprendiz);
                    bool eliminado = oAprendizL.MtEliminarAprendiz(idAprendiz);

                    if (eliminado)
                    {
                        if (idUsuario > 0) oUsuarioL.MtEliminarUsuario(idUsuario);
                        ListarAprendices();
                        SetMensaje("success", "Aprendiz eliminado correctamente");
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar el aprendiz");
                    }
                }
                catch (Exception ex)
                {
                    SetMensaje("error", ex.Message);
                }
            }
        }

        protected void gvAprendices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                if (btnEliminar != null)
                    btnEliminar.Attributes.Add("onclick", "return confirmarEliminar(this);");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ListarAprendices();
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ListarAprendices();
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            ListarAprendices();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string tipoDoc = ddlTipoDoc.SelectedValue;
                string documento = txtDocumento.Text.Trim();
                string nombres = txtNombres.Text.Trim();
                string apellidos = txtApellidos.Text.Trim();
                string correo = txtCorreo.Text.Trim();
                string telefono = txtTelefono.Text.Trim();

                if (!System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d+$"))
                {
                    hfAbrirModal.Value = string.IsNullOrEmpty(hfIdAprendiz.Value) ? "crear" : "editar";
                    SetMensaje("warning", "El teléfono solo puede contener números.");
                    return;
                }

                if (telefono.Length != 10)
                {
                    hfAbrirModal.Value = string.IsNullOrEmpty(hfIdAprendiz.Value) ? "crear" : "editar";
                    SetMensaje("warning", "El teléfono debe tener exactamente 10 dígitos.");
                    return;
                }

                string estado = ddlEstadoAcademico.SelectedValue;

                if (string.IsNullOrEmpty(ddlFicha.SelectedValue))
                {
                    hfAbrirModal.Value = string.IsNullOrEmpty(hfIdAprendiz.Value) ? "crear" : "editar";
                    SetMensaje("warning", "Seleccione una ficha válida");
                    return;
                }

                int idFicha = Convert.ToInt32(ddlFicha.SelectedValue);
                bool esNuevo = string.IsNullOrEmpty(hfIdAprendiz.Value);
                bool resultado = false;

                if (esNuevo)
                {
                    if (oUsuarioL.MtExisteCorreo(correo))
                    {
                        hfAbrirModal.Value = "crear";
                        SetMensaje("warning", "El correo ya existe");
                        return;
                    }

                    int idUsuario = oUsuarioL.MtCrearUsuarioConRetorno(correo, documento, 3);
                    if (idUsuario <= 0)
                    {
                        hfAbrirModal.Value = "crear";
                        SetMensaje("error", "No se pudo crear usuario");
                        return;
                    }

                    int idCentro = oFichaL.MtObtenerIdCentroPorFicha(idFicha);
                    int idAprendiz = oAprendizL.MtCrearAprendizConRetorno(
                        tipoDoc, documento, nombres, apellidos,
                        correo, telefono, estado, idUsuario, idFicha, idCentro);

                    if (idAprendiz <= 0)
                    {
                        oUsuarioL.MtEliminarUsuario(idUsuario);
                        hfAbrirModal.Value = "crear";
                        SetMensaje("error", "No se pudo crear el aprendiz");
                        return;
                    }

                    resultado = true;
                }
                else
                {
                    int idAprendiz = Convert.ToInt32(hfIdAprendiz.Value);
                    resultado = oAprendizL.MtActualizarAprendiz(
                        idAprendiz, tipoDoc, documento, nombres,
                        apellidos, correo, telefono, estado, idFicha);
                }

                if (resultado)
                {
                    LimpiarFormulario();
                    ListarAprendices();
                    SetMensaje("success", esNuevo ? "Aprendiz creado" : "Aprendiz actualizado");
                }
                else
                {
                    hfAbrirModal.Value = "editar";
                    SetMensaje("error", "No se pudo guardar");
                }
            }
            catch (Exception ex)
            {
                SetMensaje("error", ex.Message);
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblTituloForm.Text = "Registrar Aprendiz";
            btnGuardar.Text = "Guardar Aprendiz";
            hfAbrirModal.Value = "crear";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            hfAbrirModal.Value = "";
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            hfIdAprendiz.Value = "";
            txtDocumento.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            ddlTipoDoc.SelectedIndex = 0;
            ddlEstadoAcademico.SelectedIndex = 0;
            ddlFicha.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Aprendiz";
            btnGuardar.Text = "Guardar Aprendiz";
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}