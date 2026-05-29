<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CargaMasivaAprendices.aspx.cs"
    Inherits="sistemaPlanMejoramientos.Vista.CargaMasivaAprendices"
    EnableViewStateMac="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Carga Masiva de Aprendices</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="Css/Cargamasivaaprendices.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfMensajeTipo" runat="server" Value="" />
        <asp:HiddenField ID="hfMensajeTxt" runat="server" Value="" />
        <asp:HiddenField ID="hfArchivoBase64" runat="server" Value="" />

        <div class="page-header d-flex justify-content-between align-items-center">
            <div class="d-flex align-items-center gap-3">
                <a href="GestionAprendices.aspx" class="btn btn-outline-secondary btn-sm">
                    <i class="bi bi-arrow-left-circle me-1"></i>Volver
                </a>
                <div>
                    <h5 class="mb-0 fw-bold text-white">
                        <i class="bi bi-file-earmark-spreadsheet-fill me-2" style="color: #39b54a;"></i>
                        Carga Masiva de Aprendices
                    </h5>
                    <small class="text-muted">Registra múltiples aprendices desde Excel</small>
                </div>
            </div>
        </div>

        <div class="container-fluid py-4 px-4">
            <div class="row g-4">

                <div class="col-xl-4 col-lg-5">

                    <div class="card-custom p-4 mb-4">
                        <div class="card-title-bar">
                            <span class="step-badge">1</span>Descarga la Plantilla
                       
                        </div>
                        <div class="template-info mb-3">
                            <span class="col-tag">tipoDocumento</span>
                            <span class="col-tag">numeroDocumento</span>
                            <span class="col-tag">nombres</span>
                            <span class="col-tag">apellidos</span>
                            <span class="col-tag">correo</span>
                            <span class="col-tag">telefono</span>
                            <span class="col-tag">codigoFicha</span>
                        </div>
                        <asp:Button ID="btnDescargarPlantilla" runat="server"
                            Text="⬇ Descargar Plantilla"
                            CssClass="btn btn-outline-info btn-sm w-100"
                            OnClick="btnDescargarPlantilla_Click" />
                    </div>

                    <div class="card-custom p-4 mb-4">
                        <div class="card-title-bar">
                            <span class="step-badge">2</span>Selecciona el Archivo
                       
                        </div>

                        <div class="drop-zone" id="fuExcel">
                            <asp:FileUpload ID="fileUploadExcel" runat="server"
                                Style="position: absolute; inset: 0; width: 100%; height: 100%; opacity: 0; cursor: pointer; z-index: 2;" />
                            <div class="dz-content">
                                <i class="bi bi-file-earmark-arrow-up"></i>
                                <p id="dzTexto">Haz clic para seleccionar</p>
                            </div>
                        </div>
                        <div id="dzNombreArchivo" style="margin-top: 8px; font-size: 12px; color: #6ab87a; text-align: center; display: none;"></div>
                    </div>

                    <div class="card-custom p-4">
                        <div class="card-title-bar">
                            <span class="step-badge">3</span>Registrar
                       
                        </div>
                        <asp:Button ID="btnCargar" runat="server"
                            Text="📥 Iniciar Carga Masiva"
                            CssClass="btn btn-sena w-100"
                            OnClick="btnCargar_Click"
                            Enabled="false" />
                    </div>

                </div>

                <div class="col-xl-8 col-lg-7">

                    <div id="panelPreview" style="display: none;">
                        <div class="card-custom p-4 mb-4">
                            <div class="card-title-bar">
                                <i class="bi bi-table me-2"></i>Vista Previa
                           
                            </div>
                            <div class="row g-3 mb-3">
                                <div class="col-3">
                                    <div class="summary-card total">
                                        <div class="number" id="cntTotal">0</div>
                                        <div class="label">Total</div>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="summary-card ok">
                                        <div class="number" id="cntOk">0</div>
                                        <div class="label">Válidos</div>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="summary-card error">
                                        <div class="number" id="cntError">0</div>
                                        <div class="label">Error</div>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="summary-card dup">
                                        <div class="number" id="cntDup">0</div>
                                        <div class="label">Duplicados</div>
                                    </div>
                                </div>
                            </div>
                            <div class="scroll-table">
                                <table class="table-preview w-100">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Tipo</th>
                                            <th>Documento</th>
                                            <th>Nombres</th>
                                            <th>Apellidos</th>
                                            <th>Correo</th>
                                            <th>Tel</th>
                                            <th>Ficha</th>
                                            <th>Estado</th>
                                            <th>Detalle</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbodyPreview"></tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div id="panelResultado" style="display: none;">
                        <div class="card-custom p-4">
                            <div class="card-title-bar">
                                <i class="bi bi-clipboard-check me-2"></i>Resultado
                           
                            </div>
                            <asp:GridView ID="gvResultado" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table-preview w-100">
                                <Columns>
                                    <asp:BoundField DataField="Fila" HeaderText="#" />
                                    <asp:BoundField DataField="numeroDocumento" HeaderText="Documento" />
                                    <asp:BoundField DataField="nombres" HeaderText="Nombres" />
                                    <asp:BoundField DataField="apellidos" HeaderText="Apellidos" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div id="panelVacio" class="card-custom p-5 text-center">
                        <i class="bi bi-file-earmark-arrow-up" style="font-size: 3rem; color: #39b54a40;"></i>
                        <p class="mt-3 text-muted">Selecciona un Excel para comenzar</p>
                    </div>

                </div>
            </div>
        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdn.jsdelivr.net/npm/xlsx@0.18.5/dist/xlsx.full.min.js"></script>

    <script>
        window.addEventListener('DOMContentLoaded', function () {

            const fileInput = document.getElementById('<%= fileUploadExcel.ClientID %>');

            fileInput.addEventListener('change', function () {
                if (this.files.length > 0) {
                    procesarArchivo(this.files[0]);
                }
            });

            const tipo = document.getElementById('<%= hfMensajeTipo.ClientID %>').value;
            const txt = document.getElementById('<%= hfMensajeTxt.ClientID %>').value;

            if (tipo === 'success') {
                Swal.fire('OK', txt, 'success');
                document.getElementById('panelResultado').style.display = 'block';
            } else if (tipo === 'error') {
                Swal.fire('Error', txt, 'error');
            } else if (tipo === 'warning') {
                Swal.fire('Atención', txt, 'warning');
            }
        });

        function procesarArchivo(file) {
            const ext = file.name.split('.').pop().toLowerCase();
            if (ext !== 'xlsx' && ext !== 'xls') {
                Swal.fire('Error', 'Solo se permiten archivos Excel.', 'error');
                return;
            }

            document.getElementById('dzTexto').innerText = file.name;

            const nombreDiv = document.getElementById('dzNombreArchivo');
            nombreDiv.innerText = '📎 ' + file.name;
            nombreDiv.style.display = 'block';

            leerExcel(file);
        }

        function leerExcel(file) {
            const reader = new FileReader();

            reader.onload = function (e) {
                const arrayBuffer = e.target.result;

                const base64 = btoa(
                    new Uint8Array(arrayBuffer)
                        .reduce((data, byte) => data + String.fromCharCode(byte), '')
                );
                document.getElementById('<%= hfArchivoBase64.ClientID %>').value = base64;

                const data = new Uint8Array(arrayBuffer);
                const wb = XLSX.read(data, { type: 'array' });
                const ws = wb.Sheets[wb.SheetNames[0]];
                const rows = XLSX.utils.sheet_to_json(ws, { header: 1 });

                const tbody = document.getElementById('tbodyPreview');
                tbody.innerHTML = '';

                let ok = 0, err = 0, dup = 0;
                let seen = {};

                for (let i = 1; i < rows.length; i++) {
                    const r = rows[i];
                    if (!r || !r.length) continue;

                    const doc = r[1];
                    const isDup = seen[doc];
                    seen[doc] = true;

                    let status = 'OK';
                    if (isDup) { status = 'DUP'; dup++; }
                    else { ok++; }

                    tbody.innerHTML += `
                        <tr>
                            <td>${i}</td>
                            <td>${r[0] || ''}</td>
                            <td>${r[1] || ''}</td>
                            <td>${r[2] || ''}</td>
                            <td>${r[3] || ''}</td>
                            <td>${r[4] || ''}</td>
                            <td>${r[5] || ''}</td>
                            <td>${r[6] || ''}</td>
                            <td>${status}</td>
                            <td>—</td>
                        </tr>`;
                }

                document.getElementById('cntTotal').innerText = ok + err + dup;
                document.getElementById('cntOk').innerText = ok;
                document.getElementById('cntError').innerText = err;
                document.getElementById('cntDup').innerText = dup;

                document.getElementById('panelPreview').style.display = 'block';
                document.getElementById('panelVacio').style.display = 'none';

                document.getElementById('<%= btnCargar.ClientID %>').disabled = (ok === 0);
            };

            reader.readAsArrayBuffer(file);
        }
    </script>
</body>
</html>
