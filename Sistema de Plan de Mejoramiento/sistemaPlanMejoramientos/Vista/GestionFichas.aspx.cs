using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionFichas : System.Web.UI.Page
    {
        ClFichaL oFichaL = new ClFichaL();
        ClProgramaL oProgramaL = new ClProgramaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                LlenarProgramas();
                ListarFichas();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void LlenarProgramas()
        {
            List<ClProgramasM> programas = oProgramaL.MtListarProgramas();

            ddlPrograma.Items.Clear();
            ddlPrograma.Items.Add(new ListItem("-- Seleccione un Programa --", "0"));

            foreach (var p in programas)
            {
                ddlPrograma.Items.Add(new ListItem(
                    p.codigoPrograma + " - " + p.nombre,
                    p.idPrograma.ToString()
                ));
            }
        }

        private void ListarFichas()
        {
            List<ClFichasM> lista = oFichaL.MtListarFichas(txtBuscar.Text.Trim());

            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int pageSize = gvFichas.PageSize;

            int totalRegistros = lista.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

            var datosPaginados = lista
                .Skip(paginaActual * pageSize)
                .Take(pageSize)
                .ToList();

            gvFichas.DataSource = datosPaginados;
            gvFichas.DataBind();

            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = (paginaActual + 1).ToString();
            litTotalPaginas.Text = totalPaginas == 0 ? "1" : totalPaginas.ToString();
            litTotalRegistros.Text = totalRegistros.ToString();

            rptPaginacion.DataSource = Enumerable.Range(0, totalPaginas).ToList();
            rptPaginacion.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoFicha.Text.Trim();
            DateTime fInicio = Convert.ToDateTime(txtFechaInicio.Text);
            DateTime fFinal = Convert.ToDateTime(txtFechaFinal.Text);
            if (fFinal <= fInicio)
            {
                SetMensaje("warning", "La fecha de finalización debe ser posterior a la fecha de inicio");
                return;
            }
            string jornada = ddlJornada.SelectedValue;
            string estado = ddlEstado.SelectedValue;
            int idPrograma = Convert.ToInt32(ddlPrograma.SelectedValue);

            bool esNuevo = string.IsNullOrEmpty(hfIdFicha.Value);
            bool ok = false;

            if (esNuevo)
            {
                if (oFichaL.MtExisteFicha(codigo))
                {
                    SetMensaje("warning", "Ya existe una ficha con ese código");
                    return;
                }

                ok = oFichaL.MtCrearFicha(codigo, fInicio, fFinal, jornada, estado, idPrograma);
            }
            else
            {
                int idFicha = Convert.ToInt32(hfIdFicha.Value);

                if (oFichaL.MtExisteFichaEditar(idFicha, codigo))
                {
                    SetMensaje("warning", "Ya existe otra ficha con ese código");
                    return;
                }

                ok = oFichaL.MtActualizarFicha(idFicha, codigo, fInicio, fFinal, jornada, estado, idPrograma);
            }

            SetMensaje(ok ? "success" : "error",
                ok ? (esNuevo ? "Ficha creada" : "Ficha actualizada") : "Error al guardar");

            LimpiarFormulario();
            ListarFichas();
        }

        protected void gvFichas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idFicha = Convert.ToInt32(e.CommandArgument);

            List<ClFichasM> lista = oFichaL.MtListarFichas("");
            ClFichasM ficha = lista.FirstOrDefault(x => x.idFicha == idFicha);

            if (ficha == null) return;

            if (e.CommandName == "Editar")
            {
                hfIdFicha.Value = ficha.idFicha.ToString();
                txtCodigoFicha.Text = ficha.codigoFicha;

                txtFechaInicio.Text = ficha.fechaInicio.ToString("yyyy-MM-dd");
                txtFechaFinal.Text = ficha.fechaFinalizacion.ToString("yyyy-MM-dd");

                ddlJornada.SelectedValue = ficha.jornada;
                ddlEstado.SelectedValue = ficha.estado;

                ddlPrograma.SelectedValue = ficha.idPrograma.ToString();

                lblTituloForm.Text = "Actualizar Ficha";
                btnGuardar.Text = "Actualizar Ficha";
                btnCancelar.Visible = true;
            }
            else if (e.CommandName == "Eliminar")
            {
                bool ok = oFichaL.MtEliminarFicha(idFicha);

                SetMensaje(ok ? "success" : "error",
                    ok ? "Ficha eliminada" : "Error al eliminar");

                LimpiarFormulario();
                ListarFichas();
            }
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Pagina") return;

            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int totalPaginas = Convert.ToInt32(ViewState["TotalPaginas"]);

            if (e.CommandArgument.ToString() == "anterior")
                paginaActual = Math.Max(0, paginaActual - 1);

            else if (e.CommandArgument.ToString() == "siguiente")
                paginaActual = Math.Min(totalPaginas - 1, paginaActual + 1);

            else
                paginaActual = Convert.ToInt32(e.CommandArgument);

            ViewState["PaginaActual"] = paginaActual;
            ListarFichas();
        }


        private void LimpiarFormulario()
        {
            hfIdFicha.Value = "";
            txtCodigoFicha.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFinal.Text = "";
            ddlJornada.SelectedIndex = 0;
            ddlPrograma.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;

            lblTituloForm.Text = "Registrar Ficha";
            btnGuardar.Text = "Guardar Ficha";
            btnCancelar.Visible = false;
        }
        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ListarFichas();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            ListarFichas();
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            ListarFichas();
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }
    }
}