using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class FrmUsuarios : System.Web.UI.Page
    {
        ClUsuarioL oUsuarioL = new ClUsuarioL();

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
                CargarUsuarios();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void CargarUsuarios()
        {
            DataTable dt = oUsuarioL.MtListarUsuarios(txtBuscar.Text.Trim());

            gvUsuarios.PageIndex = (int)ViewState["PaginaActual"];
            gvUsuarios.DataSource = dt;
            gvUsuarios.DataBind();

            int totalPaginas = gvUsuarios.PageCount;
            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = dt.Rows.Count.ToString();

            var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
            rptPaginacion.DataSource = paginas;
            rptPaginacion.DataBind();
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarUsuarios();
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

            CargarUsuarios();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarUsuarios();
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarUsuarios();
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            CargarUsuarios();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string correo = txtCorreo.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(ddlRol.SelectedValue))
                {
                    SetMensaje("warning", "Por favor, seleccione un rol para el usuario.");
                    return;
                }

                int idRol = Convert.ToInt32(ddlRol.SelectedValue);
                bool esNuevo = string.IsNullOrEmpty(hfIdUsuario.Value);

                if (esNuevo)
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        SetMensaje("warning", "Por favor, digite una contraseña.");
                        return;
                    }

                    if (oUsuarioL.MtExisteCorreo(correo))
                    {
                        SetMensaje("warning", "El correo electrónico ya se encuentra registrado.");
                        return;
                    }

                    bool registrado = oUsuarioL.MtCrearUsuario(correo, password, idRol);

                    if (registrado)
                    {
                        SetMensaje("success", "¡Usuario creado con éxito!");
                        LimpiarFormulario();
                        CargarUsuarios();
                    }
                    else
                    {
                        SetMensaje("error", "Error al registrar el usuario.");
                    }
                }
                else
                {
                    int idUsuario = Convert.ToInt32(hfIdUsuario.Value);

                    bool actualizado = oUsuarioL.MtActualizarUsuario(idUsuario, correo, password, idRol);

                    if (actualizado)
                    {
                        SetMensaje("success", "¡Usuario actualizado con éxito!");
                        LimpiarFormulario();
                        CargarUsuarios();
                    }
                    else
                    {
                        SetMensaje("error", "Error al actualizar el usuario.");
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    SetMensaje("warning", "No se puede guardar el usuario porque existen datos duplicados.");
                }
                else
                {
                    SetMensaje("error", "Ocurrió un error en la base de datos.");
                }
            }
            catch (Exception)
            {
                SetMensaje("error", "Ocurrió un error inesperado.");
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idUsuario = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                DataTable dt = oUsuarioL.MtBuscarUsuarioPorId(idUsuario);
                if (dt.Rows.Count > 0)
                {
                    DataRow fila = dt.Rows[0];
                    hfIdUsuario.Value = fila["idUsuario"].ToString();
                    txtCorreo.Text = fila["correo"].ToString();
                    ddlRol.SelectedValue = fila["idRol"].ToString();
                    lblTituloForm.Text = "Modificar Usuario";
                    btnGuardar.Text = "Actualizar Cambios";
                    btnCancelar.Visible = true;
                    lblInfoPassword.Visible = true;
                    txtPassword.Attributes.Remove("required");
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int idAprendizVinculado = oUsuarioL.MtObtenerIdAprendiz(idUsuario);
                bool eliminado = oUsuarioL.MtEliminarUsuario(idUsuario);

                if (eliminado)
                {
                    if (idAprendizVinculado > 0)
                    {
                        ClAprendizL oAprendizL = new ClAprendizL();
                        oAprendizL.MtEliminarAprendiz(idAprendizVinculado);
                    }

                    SetMensaje("success", "¡Usuario y su aprendiz vinculado eliminados correctamente!");
                    CargarUsuarios();
                    if (hfIdUsuario.Value == idUsuario.ToString()) LimpiarFormulario();
                }
                else
                {
                    SetMensaje("error", "No se puede eliminar el usuario (puede tener datos asociados).");
                }
            }
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
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
            hfIdUsuario.Value = "";
            txtCorreo.Text = "";
            txtPassword.Text = "";
            ddlRol.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Usuario";
            btnGuardar.Text = "Guardar Usuario";
            btnCancelar.Visible = false;
            lblInfoPassword.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}