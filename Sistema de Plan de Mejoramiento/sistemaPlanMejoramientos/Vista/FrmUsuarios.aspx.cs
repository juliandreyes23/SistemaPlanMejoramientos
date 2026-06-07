using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class FrmUsuarios : System.Web.UI.Page
    {
        ClUsuarioL oUsuarioL = new ClUsuarioL();

        private const int PageSize = 10;

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

        private List<ClUsuarioM> ObtenerDatos()
        {
            string filtro = txtBuscar.Text.Trim();
            return oUsuarioL.MtListarUsuarios(filtro) ?? new List<ClUsuarioM>();
        }

        private void CargarUsuarios()
        {
            List<ClUsuarioM> lista = ObtenerDatos();

            var data = lista.Select(u => new
            {
                u.idUsuario,
                u.correo,
                nombreRol = u.rol != null ? u.rol.nombreRol : ""
            }).ToList();

            int paginaActual = (int)ViewState["PaginaActual"];
            int totalRegistros = data.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / PageSize);

            if (paginaActual >= totalPaginas) paginaActual = 0;

            var paginaData = data
                .Skip(paginaActual * PageSize)
                .Take(PageSize)
                .ToList();

            gvUsuarios.DataSource = paginaData;
            gvUsuarios.DataBind();

            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = (paginaActual + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = totalRegistros.ToString();

            var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
            rptPaginacion.DataSource = paginas;
            rptPaginacion.DataBind();
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Pagina") return;

            int paginaActual = (int)ViewState["PaginaActual"];
            int totalPaginas = (int)ViewState["TotalPaginas"];

            if (e.CommandArgument.ToString() == "anterior")
            {
                if (paginaActual > 0) paginaActual--;
            }
            else if (e.CommandArgument.ToString() == "siguiente")
            {
                if (paginaActual < totalPaginas - 1) paginaActual++;
            }
            else
            {
                paginaActual = Convert.ToInt32(e.CommandArgument);
            }

            ViewState["PaginaActual"] = paginaActual;
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
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(ddlRol.SelectedValue))
                {
                    SetMensaje("warning", "Seleccione un rol.");
                    return;
                }

                int idRol = Convert.ToInt32(ddlRol.SelectedValue);
                bool esNuevo = string.IsNullOrEmpty(hfIdUsuario.Value);

                if (esNuevo)
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        SetMensaje("warning", "Digite una contraseña.");
                        return;
                    }

                    if (oUsuarioL.MtExisteCorreo(correo))
                    {
                        SetMensaje("warning", "El correo ya existe.");
                        return;
                    }

                    bool ok = oUsuarioL.MtCrearUsuario(correo, password, idRol);

                    if (ok)
                    {
                        SetMensaje("success", "Usuario creado correctamente.");
                        LimpiarFormulario();
                        CargarUsuarios();
                    }
                    else
                    {
                        SetMensaje("error", "Error al crear usuario.");
                    }
                }
                else
                {
                    int idUsuario = Convert.ToInt32(hfIdUsuario.Value);

                    bool ok = oUsuarioL.MtActualizarUsuario(idUsuario, correo, password, idRol);

                    if (ok)
                    {
                        SetMensaje("success", "Usuario actualizado correctamente.");
                        LimpiarFormulario();
                        CargarUsuarios();
                    }
                    else
                    {
                        SetMensaje("error", "Error al actualizar.");
                    }
                }
            }
            catch
            {
                SetMensaje("error", "Error inesperado.");
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idUsuario = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                ClUsuarioM usuario = oUsuarioL.MtBuscarUsuarioPorId(idUsuario);

                if (usuario != null)
                {
                    hfIdUsuario.Value = usuario.idUsuario.ToString();
                    txtCorreo.Text = usuario.correo;
                    ddlRol.SelectedValue = usuario.idRol.ToString();

                    lblTituloForm.Text = "Modificar Usuario";
                    btnGuardar.Text = "Actualizar";
                    btnCancelar.Visible = true;
                    lblInfoPassword.Visible = true;
                }
            }

            if (e.CommandName == "Eliminar")
            {
                bool ok = oUsuarioL.MtEliminarUsuario(idUsuario);

                if (ok)
                {
                    SetMensaje("success", "Usuario eliminado correctamente.");
                    LimpiarFormulario();
                    CargarUsuarios();
                }
                else
                {
                    SetMensaje("error", "No se pudo eliminar.");
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
        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            CargarUsuarios();
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}