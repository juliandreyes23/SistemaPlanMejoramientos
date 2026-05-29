using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class ResultadosAprendizaje : Page
    {
        ClResultadoAprendizajeL oLogica = new ClResultadoAprendizajeL();
        ClCompetenciaL oCompetencia = new ClCompetenciaL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PaginaActual"] = 0;
                ViewState["FiltroActual"] = "";
                CargarProgramas();
                CargarGrilla(null);
            }
            else
            {
                hfMensajeTipo.Value = "";
                hfMensajeTxt.Value = "";

                int idPrograma;
                if (int.TryParse(ddlPrograma.SelectedValue, out idPrograma) && idPrograma > 0)
                {
                    DataTable tb = oCompetencia.MtCargarCompetencia(idPrograma);
                    string valorCompetencia = Request.Form[ddlCompetencia.UniqueID];
                    if (!string.IsNullOrEmpty(valorCompetencia))
                        ddlCompetencia.SelectedValue = valorCompetencia;
                }
            }
        }



        private void CargarProgramas()
        {
            ClConexion oConex = new ClConexion();
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = "SELECT idPrograma, nombre FROM programas ORDER BY nombre";
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();

            ddlPrograma.Items.Clear();
            ddlPrograma.Items.Add(new ListItem("-- Seleccione un programa --", "0"));
            foreach (DataRow row in dt.Rows)
                ddlPrograma.Items.Add(new ListItem(row["nombre"].ToString(), row["idPrograma"].ToString()));
        }

        protected void ddlPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPrograma = int.Parse(ddlPrograma.SelectedValue);

            ddlCompetencia.Items.Clear();
            ddlCompetencia.Items.Add(new ListItem("-- Seleccione una competencia --", "0"));

            if (idPrograma > 0)
            {
                DataTable tb = oCompetencia.MtCargarCompetencia(idPrograma);

                foreach (DataRow row in tb.Rows)
                {
                    ddlCompetencia.Items.Add(
                        new ListItem(
                            row["descripcion"].ToString(),
                            row["idCompetencia"].ToString()
                        )
                    );
                }
            }
        }



        private void CargarGrilla(string filtro)
        {
            DataTable dt = oLogica.MtListarResultadoAprendizaje();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                string f = filtro.ToLower();
                DataTable dtFiltrado = dt.Clone();
                foreach (DataRow row in dt.Rows)
                {
                    string desc = row["DescripcionResultado"].ToString().ToLower();
                    string comp = row["DescripcionCompetencia"].ToString().ToLower();
                    if (desc.Contains(f) || comp.Contains(f))
                        dtFiltrado.ImportRow(row);
                }
                dt = dtFiltrado;
            }

            int pageSize = gvResultados.PageSize;
            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int totalRegistros = dt.Rows.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);
            if (totalPaginas == 0) totalPaginas = 1;

            if (paginaActual >= totalPaginas) paginaActual = totalPaginas - 1;
            ViewState["PaginaActual"] = paginaActual;
            ViewState["TotalPaginas"] = totalPaginas;

            gvResultados.PageIndex = paginaActual;
            gvResultados.DataSource = dt;
            gvResultados.DataBind();

            litPaginaActual.Text = (paginaActual + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = totalRegistros.ToString();

            int[] paginas = new int[totalPaginas];
            for (int i = 0; i < totalPaginas; i++) paginas[i] = i;
            rptPaginacion.DataSource = paginas;
            rptPaginacion.DataBind();
        }

        protected void gvResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarGrilla(ViewState["FiltroActual"] as string);
        }

        protected void rptPaginacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Pagina") return;

            int paginaActual = Convert.ToInt32(ViewState["PaginaActual"]);
            int totalPaginas = Convert.ToInt32(ViewState["TotalPaginas"]);
            string arg = e.CommandArgument.ToString();

            if (arg == "anterior")
            {
                if (paginaActual > 0) paginaActual--;
            }
            else if (arg == "siguiente")
            {
                if (paginaActual < totalPaginas - 1) paginaActual++;
            }
            else
            {
                paginaActual = int.Parse(arg);
            }

            ViewState["PaginaActual"] = paginaActual;
            CargarGrilla(ViewState["FiltroActual"] as string);
        }

        protected void gvResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idPrograma = int.Parse(ddlPrograma.SelectedValue);
            int idCompetencia = int.Parse(ddlCompetencia.SelectedValue);
            string descripcion = txtDescripcion.Text.Trim();
            int idResultado = int.Parse(hfIdResultado.Value);

            if (idPrograma <= 0 || idCompetencia <= 0 || string.IsNullOrWhiteSpace(descripcion))
            {
                hfMensajeTipo.Value = "warning";
                hfMensajeTxt.Value = "Complete todos los campos obligatorios.";
                CargarGrilla(ViewState["FiltroActual"] as string);
                return;
            }

            bool ok;
            string msg;

            if (idResultado == 0)
            {
                ok = oLogica.MtCrearResultado(descripcion, idCompetencia);
                msg = ok ? "Resultado registrado correctamente." : "Error al registrar. Verifique los datos.";
            }
            else
            {
                ok = oLogica.MtActualizarResultado(idResultado, descripcion, idCompetencia);
                msg = ok ? "Resultado actualizado correctamente." : "Error al actualizar.";
            }

            hfMensajeTipo.Value = ok ? "success" : "error";
            hfMensajeTxt.Value = msg;

            if (ok) LimpiarFormulario();
            ViewState["PaginaActual"] = 0;
            CargarGrilla(null);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            ViewState["FiltroActual"] = filtro;
            ViewState["PaginaActual"] = 0;
            CargarGrilla(filtro);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ViewState["FiltroActual"] = "";
            ViewState["PaginaActual"] = 0;
            CargarGrilla(null);
        }

        protected void gvResultados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Eliminar")
            {
                try
                {
                    ClConexion oConex = new ClConexion();
                    SqlConnection cn = oConex.MtAbrirConexion();

                    SqlCommand cmdDep = new SqlCommand(
                        "DELETE FROM planResultados WHERE idResultadoAprendizaje = @id", cn);
                    cmdDep.Parameters.AddWithValue("@id", id);
                    cmdDep.ExecuteNonQuery();

                    oConex.MtCerrarConexion();

                    bool ok = oLogica.MtEliminarResultado(id);
                    hfMensajeTipo.Value = ok ? "success" : "error";
                    hfMensajeTxt.Value = ok ? "Resultado eliminado correctamente." : "Error al eliminar.";
                }
                catch (Exception ex)
                {
                    hfMensajeTipo.Value = "error";
                    hfMensajeTxt.Value = "No se pudo eliminar: " + ex.Message;
                }

                CargarGrilla(ViewState["FiltroActual"] as string);
            }
            else if (e.CommandName == "Editar")
            {
                DataTable dt = oLogica.MtListarResultadoAprendizaje();
                DataRow[] rows = dt.Select("idResultadoAprendizaje = " + id);
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    hfIdResultado.Value = id.ToString();
                    txtDescripcion.Text = row["DescripcionResultado"].ToString();

                    string idProg = row["idPrograma"].ToString();
                    CargarProgramas();
                    ddlPrograma.SelectedValue = idProg;
                    DataTable tb = oCompetencia.MtCargarCompetencia(int.Parse(idProg));
                    ddlCompetencia.SelectedValue = row["idCompetencia"].ToString();

                    lblTituloForm.Text = "Actualizar Resultado";
                    btnCancelar.Visible = true;
                    btnGuardar.Text = "Actualizar Resultado";
                }

                CargarGrilla(ViewState["FiltroActual"] as string);
            }
        }

        private void LimpiarFormulario()
        {
            hfIdResultado.Value = "0";
            txtDescripcion.Text = "";
            ddlPrograma.SelectedIndex = 0;
            ddlCompetencia.Items.Clear();
            ddlCompetencia.Items.Add(new ListItem("-- Seleccione una competencia --", "0"));
            lblTituloForm.Text = "Registrar Resultado";
            btnCancelar.Visible = false;
            btnGuardar.Text = "Guardar Resultado";
        }
    }
}