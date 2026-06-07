using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionInstructores : System.Web.UI.Page
    {
        ClInstructorL oInstructorL = new ClInstructorL();
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
                CargarCentros();
                CargarInstructores();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void CargarCentros()
        {
            try
            {
                var centros = oInstructorL.MtListarCentros();

                ddlCentro.DataSource = centros;
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
                var lista = oInstructorL.MtListarInstructores();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string f = filtro.ToLower();

                    lista = lista.Where(i =>
                        (i.nombres + " " + i.apellidos).ToLower().Contains(f) ||
                        i.numeroDocumento.ToLower().Contains(f) ||
                        i.correo.ToLower().Contains(f) ||
                        i.especialidad.ToLower().Contains(f)
                    ).ToList();

                    litSinResultados.Text = lista.Count == 0
                        ? "<div class='alert alert-warning text-center py-2 mt-2'>No se encontraron instructores.</div>"
                        : "";

                    ViewState["PaginaActual"] = 0;
                }
                else
                {
                    litSinResultados.Text = "";
                }

                

                int pagina = (int)ViewState["PaginaActual"];
                int pageSize = gvInstructores.PageSize;

                gvInstructores.DataSource = lista;
                gvInstructores.PageIndex = pagina;
                gvInstructores.DataBind();

                int totalPaginas = (int)Math.Ceiling((double)lista.Count / pageSize);

                ViewState["TotalPaginas"] = totalPaginas;

                litPaginaActual.Text = (pagina + 1).ToString();
                litTotalPaginas.Text = totalPaginas.ToString();
                litTotalRegistros.Text = lista.Count.ToString();

                rptPaginacion.DataSource = Enumerable.Range(0, totalPaginas).ToList();
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
            CargarInstructores(txtBuscar.Text.Trim());
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

            CargarInstructores(txtBuscar.Text.Trim());
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
                if (string.IsNullOrWhiteSpace(telefono))
                {
                    SetMensaje("warning", "El teléfono es obligatorio.");
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d+$"))
                {
                    SetMensaje("warning", "El teléfono solo puede contener números.");
                    return;
                }

                if (telefono.Length != 10)
                {
                    SetMensaje("warning", "El teléfono debe tener exactamente 10 dígitos.");
                    return;
                }
                string especialidad = txtEspecialidad.Text.Trim();

                bool esNuevo = string.IsNullOrEmpty(hfIdInstructor.Value);

                if (esNuevo)
                {
                    int idCentro = Convert.ToInt32(ddlCentro.SelectedValue);
                    if (idCentro == 0)
                    {
                        SetMensaje("warning", "Seleccione un centro.");
                        return;
                    }

                    if (oUsuarioL.MtExisteCorreo(correo))
                    {
                        SetMensaje("warning", "Correo ya registrado.");
                        return;
                    }

                    int idUsuario = oUsuarioL.MtCrearUsuarioInstructor(correo, documento);

                    if (idUsuario > 0)
                    {
                        bool ok = oInstructorL.MtCrearInstructor(
                            tipoDoc, documento, nombres, apellidos,
                            correo, telefono, especialidad, idUsuario, idCentro);

                        SetMensaje(ok ? "success" : "error",
                            ok ? "Instructor creado" : "Error al crear instructor");
                    }
                }
                else
                {
                    int idInstructor = Convert.ToInt32(hfIdInstructor.Value);
                    int idCentro = Convert.ToInt32(ddlCentro.SelectedValue);

                    bool ok = oInstructorL.MtActualizarInstructor(
                        idInstructor, nombres, apellidos, correo, telefono, especialidad, idCentro);

                    SetMensaje(ok ? "success" : "error",
                        ok ? "Instructor actualizado" : "Error al actualizar");
                }

                LimpiarFormulario();
                CargarInstructores();
            }
            catch (Exception ex)
            {
                SetMensaje("error", ex.Message);
            }
        }

        protected void gvInstructores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;

            int id = Convert.ToInt32(e.CommandArgument);

            var lista = oInstructorL.MtListarInstructores();
            var inst = lista.FirstOrDefault(x => x.idInstructor == id);

            if (inst == null) return;

            if (e.CommandName == "Editar")
            {
                hfIdInstructor.Value = id.ToString();

                if (ddlTipoDoc.Items.FindByValue(inst.tipoDocumento) != null)
                {
                    ddlTipoDoc.SelectedValue = inst.tipoDocumento;
                }
                txtDocumento.Text = inst.numeroDocumento;

                txtNombres.Text = inst.nombres;
                txtApellidos.Text = inst.apellidos;
                txtCorreo.Text = inst.correo;
                txtTelefono.Text = inst.telefono;
                txtEspecialidad.Text = inst.especialidad;

                ddlCentro.SelectedValue = inst.centro.idCentro.ToString();

                btnGuardar.Text = "Actualizar Datos";
                btnCancelar.Visible = true;
            }
            else if (e.CommandName == "Eliminar")
            {
                bool ok = oInstructorL.MtEliminarInstructor(id);

                SetMensaje(ok ? "success" : "error",
                    ok ? "Eliminado correctamente" : "No se pudo eliminar");

                CargarInstructores();
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
            hfIdInstructor.Value = "";

            ddlTipoDoc.SelectedIndex = 0;

            txtDocumento.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtEspecialidad.Text = "";

            ddlCentro.SelectedIndex = 0;

            btnGuardar.Text = "Registrar Instructor";
            btnCancelar.Visible = false;
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}