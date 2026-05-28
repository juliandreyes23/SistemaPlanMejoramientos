<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSubirEvidencia.aspx.cs" Inherits="sistemaPlanMejoramientos.Aprendiz.FrmSubirEvidencia" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Subir Evidencia</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'DM Sans', sans-serif;
            background: #f4f7fb;
            min-height: 100vh;
        }

        .nav {
            background: #042940;
            height: 60px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 30px;
        }

        .nav-title {
            color: white;
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 17px;
        }

        .btn-volver {
            color: white;
            text-decoration: none;
            border: 1px solid rgba(255,255,255,.3);
            padding: 8px 16px;
            border-radius: 30px;
            font-size: 12px;
        }

        .contenedor {
            width: 95%;
            max-width: 900px;
            margin: 30px auto;
        }

        .card-plan {
            background: #042940;
            border-radius: 18px;
            padding: 25px;
            color: white;
            margin-bottom: 20px;
        }

        .plan-tipo {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: rgba(159,193,49,.15);
            border: 1px solid rgba(159,193,49,.35);
            color: #9FC131;
            padding: 5px 12px;
            border-radius: 30px;
            font-size: 11px;
            margin-bottom: 12px;
            font-weight: 700;
        }

        .titulo-plan {
            font-size: 18px;
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            margin-bottom: 12px;
        }

        .datos-plan {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            font-size: 13px;
            color: rgba(255,255,255,.7);
        }

        .estado-plan {
            margin-top: 15px;
            display: inline-block;
            background: rgba(255,193,7,.15);
            border: 1px solid rgba(255,193,7,.3);
            color: #ffc107;
            padding: 6px 14px;
            border-radius: 30px;
            font-size: 12px;
            font-weight: 700;
        }

        .raps {
            margin-top: 20px;
        }

        .rap-chip {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            background: rgba(255,255,255,.08);
            border: 1px solid rgba(255,255,255,.15);
            color: rgba(255,255,255,.8);
            padding: 5px 12px;
            border-radius: 20px;
            margin: 4px;
            font-size: 11px;
        }

        .card-upload {
            background: white;
            border-radius: 18px;
            padding: 28px;
            border: 1px solid #e0e6ed;
            margin-bottom: 20px;
        }

        .titulo-upload {
            font-family: 'Syne', sans-serif;
            font-size: 17px;
            font-weight: 700;
            color: #042940;
            margin-bottom: 8px;
        }

        .sub-upload {
            font-size: 12px;
            color: #6c757d;
            margin-bottom: 20px;
        }

        .acciones {
            margin-top: 24px;
            display: flex;
            gap: 12px;
            flex-wrap: wrap;
        }

        .btn-subir {
            background: #005C53;
            color: white;
            border: none;
            border-radius: 12px;
            padding: 13px 28px;
            font-size: 14px;
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            cursor: pointer;
        }

            .btn-subir:disabled {
                background: #aaa;
                cursor: not-allowed;
            }

        .btn-cancelar {
            border: 1px solid #c5d0db;
            background: white;
            color: #042940;
            border-radius: 12px;
            padding: 13px 22px;
            text-decoration: none;
            font-size: 13px;
        }

        .historial {
            background: white;
            border-radius: 18px;
            padding: 24px;
            border: 1px solid #e0e6ed;
        }

        .historial-titulo {
            font-family: 'Syne', sans-serif;
            font-size: 16px;
            font-weight: 700;
            color: #042940;
            margin-bottom: 18px;
        }

        .item-historial {
            display: flex;
            align-items: center;
            gap: 14px;
            padding: 14px 0;
            border-bottom: 1px solid #f0f4f8;
        }

            .item-historial:last-child {
                border-bottom: none;
            }

        .icono-historial {
            width: 40px;
            height: 40px;
            background: #E1F5EE;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #0F6E56;
            font-size: 20px;
            flex-shrink: 0;
        }

        .nombre-archivo {
            font-size: 13px;
            font-weight: 700;
            color: #042940;
        }

        .fecha-archivo {
            font-size: 11px;
            color: #6c757d;
        }

        .badge-tipo {
            margin-left: auto;
            background: #E6F1FB;
            color: #185FA5;
            border-radius: 20px;
            padding: 5px 12px;
            font-size: 11px;
            font-weight: 700;
            white-space: nowrap;
        }

        .btn-descargar {
            margin-left: 8px;
            color: #005C53;
            font-size: 18px;
        }

        .sin-evidencias {
            color: #6c757d;
            font-size: 13px;
            text-align: center;
            padding: 20px 0;
        }
        .login-card {
            max-width: 420px;
            padding: 2.5rem 2.2rem;
            border-radius: 20px;
        }

        .card-logo {
            width: 52px;
            height: 52px;
        }

        .field-input {
            padding: 0.7rem 1rem;
            font-size: 0.95rem;
            border-radius: 10px;
        }

        .btn-submit {
            padding: 0.85rem;
            border-radius: 50px;
        }

        .nav {
            height: 60px;
            padding: 0 28px;
        }

        .hero {
            padding: 32px 28px 28px;
        }

        .page-body {
            max-width: 860px;
            padding: 28px;
        }

        .profile-card {
            border-radius: 18px;
        }

        .profile-header {
            padding: 28px 28px 24px;
        }

        .avatar {
            width: 72px;
            height: 72px;
        }

        .info-item {
            padding: 16px 24px;
        }

        .page-header {
            padding: 28px 28px 24px;
        }

        .plans-table-wrap {
            border-radius: 16px;
        }

        .plans-table th {
            padding: 12px 16px;
        }

        .plans-table td {
            padding: 14px 16px;
        }

        .btn-subir {
            padding: 6px 14px;
            border-radius: 20px;
        }

        .auth-card {
            max-width: 420px;
            padding: 2.5rem 2.2rem;
            border-radius: 20px;
        }

        .navbar-top {
            padding: 1rem 2rem;
        }

        .contenedor {
            max-width: 900px;
            width: 95%;
            margin: 30px auto;
        }

        .card-plan {
            padding: 25px;
            border-radius: 18px;
        }

        .card-upload {
            padding: 28px;
            border-radius: 18px;
        }

        .historial {
            padding: 24px;
            border-radius: 18px;
        }

        .btn-cancelar {
            padding: 13px 22px;
            border-radius: 12px;
        }

        .icono-historial {
            width: 40px;
            height: 40px;
            border-radius: 10px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server" enctype="multipart/form-data">

        <nav class="nav">
            <div class="nav-title">SENA Aprendiz</div>
            <a href="FrmMisPlanes.aspx" class="btn-volver">
                <i class="bi bi-arrow-left"></i>Volver
            </a>
        </nav>

        <div class="contenedor">

            <div class="card-plan">
                <asp:Literal ID="litTipoPlan" runat="server" />
                <div class="titulo-plan">
                    <asp:Label ID="lblActividades" runat="server" />
                </div>
                <div class="datos-plan">
                    <span><i class="bi bi-person"></i>
                        <asp:Label ID="lblInstructor" runat="server" /></span>
                    <span><i class="bi bi-calendar-x"></i>
                        <asp:Label ID="lblFechaLimite" runat="server" /></span>
                </div>
                <div class="estado-plan">
                    <asp:Label ID="lblEstadoPlan" runat="server" />
                </div>
                <div class="raps">
                    <asp:Literal ID="litRaps" runat="server" />
                </div>
            </div>

            <div class="card-upload">
                <div class="titulo-upload">
                    <i class="bi bi-cloud-upload-fill"></i>Subir Evidencia
                </div>
                <div class="sub-upload">
                    Formatos permitidos: PDF, DOCX, JPG, PNG y ZIP. Máximo 50 MB.
                </div>
                <label class="form-label fw-semibold">Seleccionar archivo</label>
                <asp:FileUpload ID="fuEvidencia" runat="server" CssClass="form-control" />
                <div class="acciones">
                    <asp:Button ID="btnSubir" runat="server" Text="Subir Evidencia" CssClass="btn-subir" OnClick="btnSubir_Click" />
                    <a href="FrmMisPlanes.aspx" class="btn-cancelar">Cancelar</a>
                </div>
            </div>

            <div class="historial">
                <div class="historial-titulo">
                    <i class="bi bi-clock-history"></i>Evidencias Subidas
                </div>
                <asp:Repeater ID="rptEvidencias" runat="server">
                    <ItemTemplate>
                        <div class="item-historial">
                            <div class="icono-historial">
                                <i class='<%# ObtenerIconoTipo(Eval("tipoArchivo").ToString()) %>'></i>
                            </div>
                            <div style="min-width: 0; flex: 1;">
                                <div class="nombre-archivo"><%# Eval("nombreArchivo") %></div>
                                <div class="fecha-archivo"><%# Convert.ToDateTime(Eval("fechaSubida")).ToString("dd/MM/yyyy HH:mm") %></div>
                            </div>
                            <span class="badge-tipo"><%# Eval("tipoArchivo") %></span>
                            <a href='<%# ObtenerUrlDescarga(Eval("nombreArchivo").ToString()) %>' target="_blank" class="btn-descargar" title="Descargar">
                                <i class="bi bi-download"></i>
                            </a>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%# rptEvidencias.Items.Count == 0 ? "<div class=\"sin-evidencias\"><i class=\"bi bi-inbox\"></i> Aún no has subido evidencias.</div>" : "" %>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

        </div>

        <asp:HiddenField ID="hfIdPlan" runat="server" />
        <asp:HiddenField ID="hfAlerta" runat="server" Value="" />
        <asp:Literal ID="litScript" runat="server" />

    </form>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        window.addEventListener('load', function () {
            var hf = document.getElementById('<%= hfAlerta.ClientID %>');
            if (!hf || hf.value === '') return;

            var datos = JSON.parse(hf.value);
            hf.value = '';

            Swal.fire({
                icon: datos.icon,
                title: datos.title,
                text: datos.text,
                confirmButtonText: 'Aceptar'
            }).then(function () {
                if (datos.reload) {
                    var url = window.location.pathname + '?idPlan=' + document.getElementById('<%= hfIdPlan.ClientID %>').value;
                window.location.replace(url);
            }
        });
        });
    </script>

</body>
</html>
