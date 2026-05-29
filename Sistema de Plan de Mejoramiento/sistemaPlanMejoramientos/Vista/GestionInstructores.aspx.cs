using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Datos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionInstructores : System.Web.UI.Page
    {
        ClInstructorL oInstructorL = new ClInstructorL();
        ClConexion oConex = new ClConexion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rol"] == null || Session["rol"].ToString().ToUpper() != "ADMINISTRADOR")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx");
                return;
            }

            CargarCentros();

            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                CargarCentros();
                CargarInstructores();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void CargarCentros()
        {
            if (ddlCentro.Items.Count > 0) return; 

            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                SqlCommand cmd = new SqlCommand("SELECT idCentro, nombre FROM centros WHERE estado = 'Activo'", cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                oConex.MtCerrarConexion();

                ddlCentro.DataSource = dt;
                ddlCentro.DataTextField = "nombre";
                ddlCentro.DataValueField = "idCentro";
                ddlCentro.DataBind();
                ddlCentro.Items.Insert(0, new ListItem("-- Seleccione un centro --", "0"));
            }
            catch (Exception ex)
            {
                SetMensaje("error", "Error al cargar centros: " + ex.Message);
            }
        }

        private void CargarInstructores(string filtro = "")
        {
            try
            {
                DataTable dt = oInstructorL.MtListarInstructores();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string f = filtro.ToLower();
                    var filasFiltradas = dt.AsEnumerable().Where(r =>
                        (r["nombres"].ToString() + " " + r["apellidos"].ToString()).ToLower().Contains(f) ||
                        r["numeroDocumento"].ToString().ToLower().Contains(f) ||
                        r["correo"].ToString().ToLower().Contains(f) ||
                        r["especialidad"].ToString().ToLower().Contains(f)
                    );

                    dt = filasFiltradas.Any()
                        ? filasFiltradas.CopyToDataTable()
                        : dt.Clone();

                    litSinResultados.Text = !filasFiltradas.Any()
                        ? "<div class='alert alert-warning text-center py-2 mt-2'>No se encontraron instructores con ese criterio.</div>"
                        : "";
                }
                else
                {
                    litSinResultados.Text = "";
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                    ViewState["PaginaActual"] = 0;

                gvInstructores.PageIndex = (int)ViewState["PaginaActual"];
                gvInstructores.DataSource = dt;
                gvInstructores.DataBind();

                int totalPaginas = gvInstructores.PageCount;
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
                SetMensaje("error", "Error al listar instructores: " + ex.Message);
            }
        }

        protected void gvInstructores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarInstructores();
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

            CargarInstructores();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string tipoDoc = ddlTipoDoc.SelectedValue;
            string documento = txtDocumento.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string especialidad = txtEspecialidad.Text.Trim();

            bool esNuevo = string.IsNullOrEmpty(hfIdInstructor.Value);

            if (esNuevo)
            {
                int idCentro = Convert.ToInt32(ddlCentro.SelectedValue);
                if (idCentro == 0)
                {
                    SetMensaje("warning", "Por favor seleccione un centro de formación.");
                    return;
                }

                int idUsuarioCreado = CrearUsuarioAutomatico(correo, documento);

                if (idUsuarioCreado > 0)
                {
                    bool registrado = oInstructorL.MtCrearInstructor(
                        tipoDoc, documento, nombres, apellidos,
                        correo, telefono, especialidad, idUsuarioCreado, idCentro);

                    if (registrado)
                    {
                        LimpiarFormulario();
                        CargarCentros();
                        CargarInstructores();
                        SetMensaje("success", "¡Instructor registrado y cuenta de acceso creada correctamente!");
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo asociar el perfil del instructor.");
                    }
                }
                else
                {
                    SetMensaje("warning", "No se pudo crear el usuario de acceso. El correo podría estar duplicado.");
                }
            }
            else
            {
                int idInstructor = Convert.ToInt32(hfIdInstructor.Value);
                int idCentro = Convert.ToInt32(ddlCentro.SelectedValue);

                if (idCentro == 0)
                {
                    SetMensaje("warning", "Por favor seleccione un centro de formación.");
                    return;
                }

                bool actualizado = oInstructorL.MtActualizarInstructor(
                    idInstructor, nombres, apellidos, correo, telefono, especialidad, idCentro);

                if (actualizado)
                {
                    LimpiarFormulario();
                    CargarCentros();
                    CargarInstructores();
                    SetMensaje("success", "¡Datos del instructor actualizados con éxito!");
                }
                else
                {
                    SetMensaje("error", "Error al intentar actualizar el instructor.");
                }
            }
        }

        protected void gvInstructores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString())) return;

            int idInstructor = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                GridViewRow fila = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

                hfIdInstructor.Value = idInstructor.ToString();

                HiddenField hfTipo = (HiddenField)fila.Cells[1].FindControl("hfTipoDoc");
                HiddenField hfNum = (HiddenField)fila.Cells[1].FindControl("hfNumDoc");

                if (hfTipo != null) ddlTipoDoc.SelectedValue = hfTipo.Value;
                if (hfNum != null) txtDocumento.Text = hfNum.Value;

                txtNombres.Text = HttpUtility.HtmlDecode(fila.Cells[2].Text).Trim();
                txtApellidos.Text = HttpUtility.HtmlDecode(fila.Cells[3].Text).Trim();
                txtCorreo.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text).Trim();
                txtTelefono.Text = HttpUtility.HtmlDecode(fila.Cells[5].Text).Trim();
                txtEspecialidad.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text).Trim();

                string centroNombre = HttpUtility.HtmlDecode(fila.Cells[7].Text).Trim();
                ListItem itemCentro = ddlCentro.Items.FindByText(centroNombre);
                if (itemCentro != null)
                    ddlCentro.SelectedValue = itemCentro.Value;

                ddlTipoDoc.Enabled = false;
                txtDocumento.Enabled = false;
                ddlCentro.Enabled = false;

                lblTituloForm.Text = "Modificar Instructor";
                btnGuardar.Text = "Actualizar Datos";
                btnCancelar.Visible = true;
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    bool eliminado = oInstructorL.MtEliminarInstructor(idInstructor);

                    if (eliminado)
                    {
                        LimpiarFormulario();
                        CargarCentros();
                        CargarInstructores();
                        SetMensaje("success", "¡Instructor removido del sistema con éxito!");
                    }
                    else
                    {
                        SetMensaje("error", "No se pudo eliminar al instructor.");
                    }
                }
                catch (Exception ex)
                {
                    SetMensaje("error", "Error al intentar eliminar: " + ex.Message);
                }
            }
        }

        private int CrearUsuarioAutomatico(string correo, string documento)
        {
            int idGenerado = 0;
            try
            {
                SqlConnection cn = oConex.MtAbrirConexion();
                string query = "INSERT INTO usuarios (correo, password, idRol) OUTPUT INSERTED.idUsuario VALUES (@correo, @pass, 2)";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@pass", documento);
                idGenerado = (int)cmd.ExecuteScalar();
                oConex.MtCerrarConexion();
            }
            catch { }
            return idGenerado;
        }

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
            hfIdInstructor.Value = "";
            txtDocumento.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtEspecialidad.Text = "";
            ddlTipoDoc.Enabled = true;
            txtDocumento.Enabled = true;
            ddlCentro.Enabled = true;
            ddlCentro.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Instructor";
            btnGuardar.Text = "Registrar Instructor";
            btnCancelar.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarInstructores(txtBuscar.Text.Trim());
        }

        protected void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            CargarInstructores();
        }
    }
}