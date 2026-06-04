using OfficeOpenXml;
using sistemaPlanMejoramientos.Logica;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace sistemaPlanMejoramientos.Vista
{
    public partial class CargaMasivaAprendices : System.Web.UI.Page
    {
        ClAprendizL oAprendizL = new ClAprendizL();
        ClUsuarioL oUsuarioL = new ClUsuarioL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rol"] == null || Session["rol"].ToString().ToUpper() != "ADMINISTRADOR")
            {
                Response.Redirect("~/Vista/FrmLogin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            byte[] fileBytes = null;

            if (fileUploadExcel.HasFile)
                fileBytes = fileUploadExcel.FileBytes;
            else if (!string.IsNullOrEmpty(hfArchivoBase64.Value))
                fileBytes = Convert.FromBase64String(hfArchivoBase64.Value);
            else
            {
                EstablecerMensajeAlerta("warning", "Seleccione un archivo Excel.");
                return;
            }

            try
            {
                DataTable dtParaBulk = new DataTable();
                dtParaBulk.Columns.Add("tipoDocumento", typeof(string));
                dtParaBulk.Columns.Add("numeroDocumento", typeof(string));
                dtParaBulk.Columns.Add("nombres", typeof(string));
                dtParaBulk.Columns.Add("apellidos", typeof(string));
                dtParaBulk.Columns.Add("correo", typeof(string));
                dtParaBulk.Columns.Add("telefono", typeof(string));
                dtParaBulk.Columns.Add("estadoAcademico", typeof(string));
                dtParaBulk.Columns.Add("correoPersonal", typeof(string));

                DataColumn colUsuario = new DataColumn("idUsuario", typeof(int));
                colUsuario.AllowDBNull = true;
                dtParaBulk.Columns.Add(colUsuario);
                dtParaBulk.Columns.Add("idFicha", typeof(int));
                dtParaBulk.Columns.Add("idCentro", typeof(int));

                int sinFicha = 0;
                int sinDocumento = 0;
                int duplicados = 0;

                using (var stream = new MemoryStream(fileBytes))
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        EstablecerMensajeAlerta("error", "El Excel no contiene hojas.");
                        return;
                    }

                    var worksheet = package.Workbook.Worksheets.First();

                    if (worksheet.Dimension == null)
                    {
                        EstablecerMensajeAlerta("warning", "El Excel está vacío.");
                        return;
                    }

                    int totalFilas = worksheet.Dimension.End.Row;

                    for (int fila = 2; fila <= totalFilas; fila++)
                    {
                        string documento = worksheet.Cells[fila, 2].Value?.ToString().Trim();
                        string ficha = worksheet.Cells[fila, 7].Value?.ToString().Trim();

                        if (string.IsNullOrWhiteSpace(documento)) { sinDocumento++; continue; }
                        if (string.IsNullOrWhiteSpace(ficha)) { sinFicha++; continue; }

                        if (oAprendizL.MtExisteAprendiz(documento)) { duplicados++; continue; }

                        int idFicha = oAprendizL.MtObtenerIdFichaPorCodigo(ficha);
                        if (idFicha <= 0) { sinFicha++; continue; }

                        string correoFila = worksheet.Cells[fila, 5].Value?.ToString().Trim() ?? "";
                        bool correoYaExiste = !string.IsNullOrEmpty(correoFila) && oUsuarioL.MtExisteCorreo(correoFila);

                        DataRow row = dtParaBulk.NewRow();
                        row["tipoDocumento"] = worksheet.Cells[fila, 1].Value?.ToString().Trim() ?? "";
                        row["numeroDocumento"] = documento;
                        row["nombres"] = worksheet.Cells[fila, 3].Value?.ToString().Trim() ?? "";
                        row["apellidos"] = worksheet.Cells[fila, 4].Value?.ToString().Trim() ?? "";
                        row["correo"] = correoFila;
                        row["telefono"] = worksheet.Cells[fila, 6].Value?.ToString().Trim() ?? "";
                        row["estadoAcademico"] = "En formación";
                        row["idUsuario"] = DBNull.Value;
                        row["idFicha"] = idFicha;
                        row["correoPersonal"] = correoYaExiste ? "" : correoFila;
                        row["idCentro"] = Convert.ToInt32(Session["idCentro"]);

                        dtParaBulk.Rows.Add(row);
                    }
                }

                if (dtParaBulk.Rows.Count == 0)
                {
                    EstablecerMensajeAlerta("warning",
                        $"No hay filas válidas. Duplicados: {duplicados}, sin documento: {sinDocumento}, sin ficha: {sinFicha}");
                    return;
                }

                foreach (DataRow r in dtParaBulk.Rows)
                {
                    string correoNuevo = r["correoPersonal"].ToString();
                    string doc = r["numeroDocumento"].ToString();

                    if (!string.IsNullOrEmpty(correoNuevo))
                    {
                        int idUsuarioNuevo = oUsuarioL.MtCrearUsuarioConRetorno(correoNuevo, doc, 3);
                        r["idUsuario"] = idUsuarioNuevo > 0 ? (object)idUsuarioNuevo : DBNull.Value;
                    }
                }

                bool exito = oAprendizL.MtCargaMasivaAprendices(dtParaBulk);

                if (!exito)
                {
                    foreach (DataRow r in dtParaBulk.Rows)
                    {
                        if (r["idUsuario"] != DBNull.Value)
                        {
                            int idU = Convert.ToInt32(r["idUsuario"]);
                            oUsuarioL.MtEliminarUsuario(idU);
                        }
                    }

                    EstablecerMensajeAlerta("error", "Error al insertar en base de datos. No se guardó ningún registro.");
                    return;
                }

                DataTable dtResultado = new DataTable();
                dtResultado.Columns.Add("Fila", typeof(int));
                dtResultado.Columns.Add("numeroDocumento", typeof(string));
                dtResultado.Columns.Add("nombres", typeof(string));
                dtResultado.Columns.Add("apellidos", typeof(string));
                dtResultado.Columns.Add("Estado", typeof(string));
                dtResultado.Columns.Add("Detalle", typeof(string));

                int i = 1;
                foreach (DataRow r in dtParaBulk.Rows)
                {
                    DataRow nr = dtResultado.NewRow();
                    nr["Fila"] = i++;
                    nr["numeroDocumento"] = r["numeroDocumento"];
                    nr["nombres"] = r["nombres"];
                    nr["apellidos"] = r["apellidos"];
                    nr["Estado"] = "Insertado";
                    nr["Detalle"] = "OK";
                    dtResultado.Rows.Add(nr);
                }

                gvResultado.DataSource = dtResultado;
                gvResultado.DataBind();

                string msg = $"Carga exitosa: {dtParaBulk.Rows.Count} registros.";
                if (duplicados > 0) msg += $" {duplicados} omitidos por duplicado.";
                if (sinFicha > 0) msg += $" {sinFicha} omitidos por ficha inválida.";
                if (sinDocumento > 0) msg += $" {sinDocumento} omitidos sin documento.";

                EstablecerMensajeAlerta("success", msg);
            }
            catch (Exception ex)
            {
                EstablecerMensajeAlerta("error", "Error inesperado: " + ex.Message);
            }
        }

        protected void btnDescargarPlantilla_Click(object sender, EventArgs e)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Plantilla");

                ws.Cells[1, 1].Value = "tipoDocumento";
                ws.Cells[1, 2].Value = "numeroDocumento";
                ws.Cells[1, 3].Value = "nombres";
                ws.Cells[1, 4].Value = "apellidos";
                ws.Cells[1, 5].Value = "correo";
                ws.Cells[1, 6].Value = "telefono";
                ws.Cells[1, 7].Value = "codigoFicha";

                ws.Cells[2, 1].Value = "CC";
                ws.Cells[2, 2].Value = "123456";
                ws.Cells[2, 3].Value = "Juan";
                ws.Cells[2, 4].Value = "Perez";
                ws.Cells[2, 5].Value = "correo@gmail.com";
                ws.Cells[2, 6].Value = "3001234567";
                ws.Cells[2, 7].Value = "2978144";

                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                byte[] file = package.GetAsByteArray();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=Plantilla.xlsx");
                Response.BinaryWrite(file);
                Response.End();
            }
        }

        private void EstablecerMensajeAlerta(string tipo, string texto)
        {
            hfMensajeTipo.Value = tipo;
            hfMensajeTxt.Value = texto;
        }
    }
}