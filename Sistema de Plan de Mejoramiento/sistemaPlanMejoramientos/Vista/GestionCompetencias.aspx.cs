using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class GestionCompetencias : Page
    {
        ClCompetenciaL oCompetenciaL = new ClCompetenciaL();
        ClProgramaL oProgramaL = new ClProgramaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProgramas();
                ViewState["PaginaActual"] = 0;
                CargarCompetencias("");
            }
            if (string.IsNullOrEmpty(Request["__EVENTTARGET"]))
            {
                hfMensajeTipo.Value = "";
                hfMensajeTxt.Value = "";
            }

        }

        private void CargarProgramas()
        {
            DataTable dt = oProgramaL.MtListarProgramas();  
            ddlPrograma.Items.Clear();
            ddlPrograma.Items.Add(new ListItem("-- Seleccione un programa --", "0"));
            foreach (DataRow row in dt.Rows)
            {
                ddlPrograma.Items.Add(new ListItem(
                    row["codigoPrograma"] + " - " + row["nombre"],
                    row["idPrograma"].ToString()
                ));
            }
        }

        private void CargarCompetencias(string filtro)
        {
            DataTable dt;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                dt = oCompetenciaL.MtBuscarCompetencias(filtro);
            }
            else
            {
                dt = oCompetenciaL.MtListarCompetencias();
            }

            int pageSize = 10;
            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int totalRegistros = dt.Rows.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

            if (paginaActual >= totalPaginas && totalPaginas > 0)
            {
                paginaActual = totalPaginas - 1;
                ViewState["PaginaActual"] = paginaActual;
            }

            ViewState["TotalPaginas"] = totalPaginas;

            gvCompetencias.PageIndex = paginaActual;
            gvCompetencias.DataSource = dt;
            gvCompetencias.DataBind();

            litPaginaActual.Text = (paginaActual + 1).ToString();
            litTotalPaginas.Text = totalPaginas == 0 ? "1" : totalPaginas.ToString();
            litTotalRegistros.Text = totalRegistros.ToString();

            if (totalPaginas > 1)
            {
                int[] paginas = new int[totalPaginas];

                for (int i = 0; i < totalPaginas; i++)
                {
                    paginas[i] = i;
                }

                rptPaginacion.DataSource = paginas;
                rptPaginacion.DataBind();
            }
            else
            {
                rptPaginacion.DataSource = null;
                rptPaginacion.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text.Trim();
            int idPrograma = Convert.ToInt32(ddlPrograma.SelectedValue);

            if (string.IsNullOrWhiteSpace(descripcion) || idPrograma <= 0)
            {
                hfMensajeTipo.Value = "warning";
                hfMensajeTxt.Value = "Por favor completa todos los campos obligatorios.";
                CargarCompetencias(txtBuscar.Text.Trim());
                return;
            }

            int idCompetencia = 0;
            int.TryParse(hfIdCompetencia.Value, out idCompetencia);

            bool ok;
            string msg;

            if (idCompetencia > 0)
            {
                ok = oCompetenciaL.MtActualizarCompetencia(idCompetencia, descripcion, idPrograma);
                msg = ok ? "Competencia actualizada correctamente." : "Error al actualizar la competencia.";
            }
            else
            {
                ok = oCompetenciaL.MtCrearCompetencia(descripcion, idPrograma);
                msg = ok ? "Competencia registrada correctamente." : "Error al registrar la competencia.";
            }

            hfMensajeTipo.Value = ok ? "success" : "error";
            hfMensajeTxt.Value = msg;

            LimpiarFormulario();
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["PaginaActual"] = 0;
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["PaginaActual"] = 0;
            CargarCompetencias("");
        }

        protected void gvCompetencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                DataTable dt = oCompetenciaL.MtListarCompetencias();
                DataRow[] rows = dt.Select($"idCompetencia = {id}");
                if (rows.Length > 0)
                {
                    hfIdCompetencia.Value = id.ToString();
                    txtDescripcion.Text = rows[0]["DescripcionCompetencia"].ToString();
                    ddlPrograma.SelectedValue = rows[0]["idPrograma"].ToString();
                    lblTituloForm.Text = "Actualizar Competencia";
                    btnGuardar.Text = "Actualizar Competencia";
                    btnCancelar.Visible = true;
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                bool ok = oCompetenciaL.MtEliminarCompetencia(id);
                hfMensajeTipo.Value = ok ? "success" : "error";
                hfMensajeTxt.Value = ok ? "Competencia eliminada correctamente." : "Error al intentar eliminar.";
                LimpiarFormulario();
                ViewState["PaginaActual"] = 0;
                CargarCompetencias(txtBuscar.Text.Trim());
            }
        }

        protected void gvCompetencias_RowDataBound(object sender, GridViewRowEventArgs e) { }

        protected void gvCompetencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarCompetencias(txtBuscar.Text.Trim());
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
            CargarCompetencias(txtBuscar.Text.Trim());
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vista/Dashboard.aspx");
        }

        private void LimpiarFormulario()
        {
            hfIdCompetencia.Value = "";
            txtDescripcion.Text = "";
            ddlPrograma.SelectedIndex = 0;
            lblTituloForm.Text = "Registrar Competencia";
            btnGuardar.Text = "Guardar Competencia";
            btnCancelar.Visible = false;
        }
    }
}