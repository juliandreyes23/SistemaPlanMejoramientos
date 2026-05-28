using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class AsignacionInstructores : System.Web.UI.Page
    {
        ClAsignacionL oAsignacionL = new ClAsignacionL();

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
                CargarCombos();
                CargarAsignaciones();
            }

            hfMensajeTipo.Value = "";
            hfMensajeTxt.Value = "";
        }

        private void CargarCombos()
        {
            ddlInstructores.DataSource = oAsignacionL.MtListarInstructores();
            ddlInstructores.DataValueField = "idInstructor";
            ddlInstructores.DataTextField = "NombreCompleto";
            ddlInstructores.DataBind();
            ddlInstructores.Items.Insert(0, new ListItem("-- Seleccione Instructor --", "0"));

            ddlFichas.DataSource = oAsignacionL.MtListarFichas();
            ddlFichas.DataValueField = "idFicha";
            ddlFichas.DataTextField = "TextoFicha";
            ddlFichas.DataBind();
            ddlFichas.Items.Insert(0, new ListItem("-- Seleccione Ficha --", "0"));
        }

        private void CargarAsignaciones()
        {
            DataTable dt = oAsignacionL.MtListarAsignaciones();

            gvAsignaciones.PageIndex = (int)ViewState["PaginaActual"];
            gvAsignaciones.DataSource = dt;
            gvAsignaciones.DataBind();

            int totalPaginas = gvAsignaciones.PageCount;
            ViewState["TotalPaginas"] = totalPaginas;

            litPaginaActual.Text = ((int)ViewState["PaginaActual"] + 1).ToString();
            litTotalPaginas.Text = totalPaginas.ToString();
            litTotalRegistros.Text = dt.Rows.Count.ToString();

            var paginas = Enumerable.Range(0, totalPaginas).Cast<object>().ToList();
            rptPaginacion.DataSource = paginas;
            rptPaginacion.DataBind();
        }

        protected void gvAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["PaginaActual"] = e.NewPageIndex;
            CargarAsignaciones();
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

            CargarAsignaciones();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            int idIns = Convert.ToInt32(ddlInstructores.SelectedValue);
            int idFic = Convert.ToInt32(ddlFichas.SelectedValue);

            if (idIns == 0 || idFic == 0)
            {
                SetMensaje("warning", "Debe seleccionar un instructor y una ficha válida.");
                return;
            }

            bool asignado = oAsignacionL.MtAsignarInstructorFicha(idIns, idFic);

            if (asignado)
            {
                SetMensaje("success", "El instructor fue asignado a la ficha correctamente.");
                CargarAsignaciones();
                ddlInstructores.SelectedIndex = 0;
                ddlFichas.SelectedIndex = 0;
            }
            else
            {
                SetMensaje("info", "Ese instructor ya se encuentra asignado a la ficha seleccionada.");
            }
        }

        protected void gvAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idFichaInstructor = Convert.ToInt32(e.CommandArgument);
                bool eliminado = oAsignacionL.MtEliminarAsignacion(idFichaInstructor);

                if (eliminado)
                {
                    SetMensaje("success", "Asignación eliminada con éxito.");
                    CargarAsignaciones();
                }
                else
                {
                    SetMensaje("error", "No se pudo remover la asignación.");
                }
            }
        }

        protected void lnkVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vista/Dashboard.aspx");
        }

        private void SetMensaje(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}