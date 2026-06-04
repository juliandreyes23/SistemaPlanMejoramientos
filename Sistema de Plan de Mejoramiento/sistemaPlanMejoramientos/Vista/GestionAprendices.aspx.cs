using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        private void CargarFichas()
        {
            try
            {
                DataTable dtFichas = oFichaL.MtListarFichas();
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

        private void ListarAprendices()
        {
            try
            {
                DataTable dt = oAprendizL.MtListarAprendices(txtBuscar.Text.Trim());

                gvAprendices.PageIndex = (int)ViewState["PaginaActual"];
                gvAprendices.DataSource = dt;
                gvAprendices.DataBind();

                int totalPaginas = gvAprendices.PageCount;
                ViewState["TotalPaginas"] = totalPaginas;

                litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
                litTotalPaginas.Text = totalPaginas.ToString();
                litTotalRegistros.Text = dt.Rows.Count.ToString();

                var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
                rptPaginacion.DataSource = paginas;
                rptPaginacion.DataBind();
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error al listar aprendices: " + ex.Message);
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

        protected void gvAprendices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                if (btnEliminar != null)
                    btnEliminar.Attributes.Add("onclick", "return confirmarEliminar(this);");
            }
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
                string estadoAcademico = ddlEstadoAcademico.SelectedValue;

                if (string.IsNullOrEmpty(ddlFicha.SelectedValue))
                {
                    hfAbrirModal.Value = string.IsNullOrEmpty(hfIdAprendiz.Value) ? "crear" : "editar";
                    SetMensaje("warning", "Por favor, seleccione una Ficha de Formación válida.");
                    return;
                }

                int idFicha = Convert.ToInt32(ddlFicha.SelectedValue);
                bool resultado = false;
                bool esNuevo = string.IsNullOrEmpty(hfIdAprendiz.Value);

                if (esNuevo)
                {
                    if (oUsuarioL.MtExisteCorreo(correo))
                    {
                        hfAbrirModal.Value = "crear";
                        SetMensaje("warning", "El correo ya está registrado como usuario en el sistema.");
                        return;
                    }

                    int idUsuarioNuevo = oUsuarioL.MtCrearUsuarioConRetorno(correo, documento, 3);
                    if (idUsuarioNuevo <= 0)
                    {
                        hfAbrirModal.Value = "crear";
                        SetMensaje("error", "No se pudo crear el usuario para el aprendiz.");
                        return;
                    }

                    int idCentro = oFichaL.MtObtenerIdCentroPorFicha(idFicha);
                    if (idCentro <= 0)
                    {
                        hfAbrirModal.Value = "crear";
                        SetMensaje("error", "No se pudo determinar el centro asociado a la ficha seleccionada.");
                        return;
                    }

                    int idAprendizNuevo = oAprendizL.MtCrearAprendizConRetorno(
                        tipoDoc, documento, nombres, apellidos,
                        correo, telefono, estadoAcademico, idUsuarioNuevo, idFicha, idCentro);

                    if (idAprendizNuevo <= 0)
                    {
                        hfAbrirModal.Value = "crear";
                        SetMensaje("error", "No se pudo registrar el aprendiz.");
                        return;
                    }

                    oAprendizL.MtRegistrarFichaIntermedia(idFicha, idAprendizNuevo);
                    resultado = true;
                }
                else
                {
                    int idAprendiz = Convert.ToInt32(hfIdAprendiz.Value);
                    resultado = oAprendizL.MtActualizarAprendiz(
                        idAprendiz, tipoDoc, documento, nombres, apellidos,
                        correo, telefono, estadoAcademico, idFicha);
                }

                if (resultado)
                {
                    LimpiarFormulario();
                    ListarAprendices();
                    SetMensaje("success", esNuevo
                        ? "¡Aprendiz registrado! Usuario creado con contraseña = número de documento."
                        : "¡Aprendiz actualizado con éxito!");
                }
                else
                {
                    hfAbrirModal.Value = esNuevo ? "crear" : "editar";
                    SetMensaje("error", "La base de datos no sufrió cambios. Verifica los datos ingresados.");
                }
            }
            catch (Exception ex)
            {
                hfAbrirModal.Value = string.IsNullOrEmpty(hfIdAprendiz.Value) ? "crear" : "editar";
                string msg = ex.Message.Contains("UNIQUE KEY") || ex.Message.Contains("duplicate key")
                    ? "El número de documento ya se encuentra registrado."
                    : "Error en el proceso de guardado: " + ex.Message;
                SetMensaje("error", msg);
            }
        }

        protected void gvAprendices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString())) return;

            if (e.CommandName == "Editar")
            {
                LinkButton btnEditar = (LinkButton)e.CommandSource;
                GridViewRow fila = (GridViewRow)btnEditar.NamingContainer;

                int idAprendizKey = Convert.ToInt32(gvAprendices.DataKeys[fila.RowIndex]["idAprendiz"]);
                string idFichaKey = gvAprendices.DataKeys[fila.RowIndex]["idFicha"].ToString();

                hfIdAprendiz.Value = idAprendizKey.ToString();

                string tipoDocTabla = Server.HtmlDecode(fila.Cells[1].Text).Trim();
                ListItem itemTipo = ddlTipoDoc.Items.FindByValue(tipoDocTabla);
                if (itemTipo != null) ddlTipoDoc.SelectedValue = itemTipo.Value;

                txtDocumento.Text = Server.HtmlDecode(fila.Cells[2].Text).Trim();
                txtNombres.Text = Server.HtmlDecode(fila.Cells[3].Text).Trim();
                txtApellidos.Text = Server.HtmlDecode(fila.Cells[4].Text).Trim();
                txtCorreo.Text = Server.HtmlDecode(fila.Cells[5].Text).Trim();
                txtTelefono.Text = Server.HtmlDecode(fila.Cells[6].Text).Trim();

                string estadoTabla = Server.HtmlDecode(fila.Cells[7].Text).Trim();
                ListItem itemEstado = ddlEstadoAcademico.Items.FindByValue(estadoTabla);
                if (itemEstado != null) ddlEstadoAcademico.SelectedValue = itemEstado.Value;

                ListItem itemFicha = ddlFicha.Items.FindByValue(idFichaKey);
                if (itemFicha != null) ddlFicha.SelectedValue = itemFicha.Value;

                lblTituloForm.Text = "Modificar Aprendiz: " + txtNombres.Text;
                btnGuardar.Text = "Actualizar Aprendiz";

                hfAbrirModal.Value = "editar";
                hfMensajeTipo.Value = "";
                hfMensajeTxt.Value = "";
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int idAprendiz = Convert.ToInt32(e.CommandArgument);
                    int idUsuarioVinculado = oAprendizL.MtObtenerIdUsuarioPorAprendiz(idAprendiz);
                    bool eliminado = oAprendizL.MtEliminarAprendiz(idAprendiz);

                    if (eliminado)
                    {
                        if (idUsuarioVinculado > 0)
                            oUsuarioL.MtEliminarUsuario(idUsuarioVinculado);

                        LimpiarFormulario();
                        ListarAprendices();
                        SetMensaje("success", "¡Aprendiz y su usuario eliminados con éxito!");
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar el aprendiz. Intenta de nuevo.");
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

            Control ctrl = sender as Control;
            if (ctrl != null && ctrl.ID == "btnNuevo")
            {
                hfAbrirModal.Value = "crear";
                pnlAvisoUsuario.Visible = true;
            }
        }

        private void LimpiarFormulario()
        {
            hfIdAprendiz.Value = "";
            hfAbrirModal.Value = "";
            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";

            txtDocumento.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";

            if (ddlTipoDoc.Items.Count > 0) ddlTipoDoc.SelectedIndex = 0;
            if (ddlEstadoAcademico.Items.Count > 0) ddlEstadoAcademico.SelectedIndex = 0;
            if (ddlFicha.Items.Count > 0) ddlFicha.SelectedIndex = 0;

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