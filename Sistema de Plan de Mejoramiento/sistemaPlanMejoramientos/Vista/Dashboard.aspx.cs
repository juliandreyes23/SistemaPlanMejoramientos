using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sistemaPlanMejoramientos.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null || Session["rol"] == null)
            {
                Response.Redirect("../FrmLogin.aspx");
                return;
            }

            string rol = Session["rol"].ToString().ToUpper();
            if (rol != "ADMINISTRADOR")
            {
                Response.Redirect("../FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblUsuario.Text = Session["correo"].ToString();
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("FrmLogin.aspx");
        }
    }
}