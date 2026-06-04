using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sistemaPlanMejoramientos.Logica;

namespace sistemaPlanMejoramientos.Instructor
{
    public partial class DashboardInstructor : System.Web.UI.Page
    {
        ClFichaL oFichaL = new ClFichaL();
        ClPlanMejoramientoL oPlanL = new ClPlanMejoramientoL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["correo"] == null || Session["rol"] == null)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            if (Session["rol"].ToString().ToUpper() != "INSTRUCTOR")
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblInstructor.Text = Session["correo"].ToString();
                mtdCargarMetricas();
            }
        }

        private void mtdCargarMetricas()
        {
            try
            {
                int idInstructor = Convert.ToInt32(Session["idInstructor"]);

                
                lblTotalFichas.Text = oFichaL.MtContarFichasPorInstructor(idInstructor).ToString();

                
                lblPlanesInternos.Text = oPlanL.MtContarPlanesPorTipo(idInstructor, "Interno").ToString();
                lblPlanesComite.Text   = oPlanL.MtContarPlanesPorTipo(idInstructor, "Comité").ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar métricas del instructor: " + ex.Message);
                lblTotalFichas.Text    = "0";
                lblPlanesInternos.Text = "0";
                lblPlanesComite.Text   = "0";
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("FrmLogin.aspx");
        }
    }
}