<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCrearPlan.aspx.cs"
    Inherits="sistemaPlanMejoramientos.Instructor.FrmCrearPlan" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Crear Plan de Mejoramiento | SENA</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />

    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
            --sena-dark: #042940;
            --sena-mid: #005C53;
            --sena-accent: #9FC131;
        }

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
            height: 60px;
            background: #042940;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 28px;
        }

        .nav-brand {
            display: flex;
            align-items: center;
            gap: 10px;
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 16px;
            color: #fff;
        }

        .nav-dot {
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background: #9FC131;
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0%,100% {
                opacity: 1;
                transform: scale(1);
            }

            50% {
                opacity: .5;
                transform: scale(1.4);
            }
        }

        .nav-btn {
            border: 1px solid rgba(255,255,255,.25);
            background: transparent;
            color: rgba(255,255,255,.85);
            border-radius: 20px;
            padding: 7px 16px;
            font-size: 12px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: .2s;
        }

            .nav-btn:hover {
                background: rgba(255,255,255,.1);
                color: #fff;
            }

        .hero {
            background: #042940;
            padding: 38px 28px 34px;
            position: relative;
            overflow: hidden;
        }

            .hero::before {
                content: '';
                position: absolute;
                top: -70px;
                right: -70px;
                width: 320px;
                height: 320px;
                border-radius: 50%;
                background: #005C53;
                opacity: .35;
            }

        .hero-inner {
            position: relative;
            z-index: 1;
        }

        .hero-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: rgba(159,193,49,.15);
            border: 1px solid rgba(159,193,49,.35);
            color: #9FC131;
            font-size: 11px;
            font-weight: 600;
            padding: 5px 12px;
            border-radius: 20px;
            margin-bottom: 14px;
            letter-spacing: .5px;
            text-transform: uppercase;
        }

        .hero-title {
            font-family: 'Syne', sans-serif;
            font-size: 28px;
            font-weight: 800;
            line-height: 1.1;
            color: #fff;
            margin-bottom: 10px;
        }

            .hero-title span {
                color: #9FC131;
            }

        .hero-sub {
            color: rgba(255,255,255,.55);
            font-size: 13px;
            line-height: 1.6;
            max-width: 680px;
        }

        .body-wrap {
            padding: 28px;
            max-width: 950px;
            margin: 0 auto;
        }

        .card-form {
            background: #fff;
            border: 1px solid #e0e6ed;
            border-radius: 20px;
            padding: 30px;
            box-shadow: 0 10px 28px rgba(0,0,0,.04);
        }

        .section-title {
            font-family: 'Syne', sans-serif;
            font-size: 13px;
            font-weight: 700;
            color: #042940;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-bottom: 18px;
            padding-bottom: 12px;
            border-bottom: 1px solid #edf2f6;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .form-label {
            font-size: 12px;
            font-weight: 600;
            color: #495057;
            margin-bottom: 6px;
        }

        .form-control,
        .form-select {
            border: 1px solid #dbe3ea;
            border-radius: 12px;
            padding: 11px 14px;
            font-size: 13px;
            color: #042940;
            transition: .2s;
            background: #fff;
        }

            .form-control:focus,
            .form-select:focus {
                border-color: #9FC131;
                box-shadow: 0 0 0 4px rgba(159,193,49,.12);
                outline: none;
            }

        textarea.form-control {
            resize: none;
        }

        .resultados-box {
            border: 1px solid #dbe3ea;
            border-radius: 14px;
            background: #fafcfd;
            padding: 14px;
            max-height: 280px;
            overflow-y: auto;
        }

        .resultado-item {
            display: flex;
            gap: 12px;
            padding: 12px;
            border-radius: 12px;
            transition: .2s;
            cursor: pointer;
        }

            .resultado-item:hover {
                background: #f1f5f9;
            }

            .resultado-item input[type=checkbox] {
                accent-color: #9FC131;
                margin-top: 3px;
            }

        .resultado-content {
            flex: 1;
        }

        .resultado-comp {
            font-size: 11px;
            font-weight: 700;
            color: #9FC131;
            margin-bottom: 4px;
            text-transform: uppercase;
            letter-spacing: .4px;
        }

        .resultado-label {
            font-size: 12px;
            color: #042940;
            line-height: 1.6;
        }

        .empty-resultados {
            text-align: center;
            padding: 36px 18px;
            color: #adb5bd;
        }

            .empty-resultados i {
                font-size: 34px;
                margin-bottom: 10px;
                display: block;
            }

            .empty-resultados p {
                font-size: 13px;
            }

        .actions {
            display: flex;
            justify-content: flex-end;
            gap: 12px;
            margin-top: 10px;
            flex-wrap: wrap;
        }

        .btn-cancelar {
            border: 1px solid #dbe3ea;
            background: transparent;
            color: #6c757d;
            padding: 11px 20px;
            border-radius: 12px;
            text-decoration: none;
            font-size: 13px;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: .2s;
        }

            .btn-cancelar:hover {
                background: #f4f7fb;
            }

        .btn-guardar {
            border: none;
            background: #042940;
            color: #fff;
            padding: 11px 24px;
            border-radius: 12px;
            font-size: 13px;
            font-weight: 600;
            transition: .2s;
        }

            .btn-guardar:hover {
                background: #005C53;
            }

        @media (max-width: 768px) {

            .body-wrap {
                padding: 18px;
            }

            .card-form {
                padding: 22px;
            }

            .actions {
                flex-direction: column;
            }

            .btn-cancelar,
            .btn-guardar {
                width: 100%;
                justify-content: center;
            }
        }

        textarea.form-control {
            resize: vertical;
            min-height: 100px;
            line-height: 1.6;
            padding: 12px 14px;
            font-family: 'DM Sans', sans-serif;
            font-size: 13px;
            color: #042940;
            background: #fafcfd;
            border: 1.5px solid #dbe3ea;
            border-radius: 12px;
            transition: border-color .2s, box-shadow .2s;
            width: 100%;
        }

            textarea.form-control:focus {
                border-color: #9FC131;
                box-shadow: 0 0 0 4px rgba(159,193,49,.12);
                outline: none;
                background: #fff;
            }

            textarea.form-control::placeholder {
                color: #adb5bd;
                font-style: italic;
            }
    </style>
</head>

<body>

    <form id="form1" runat="server">

        <asp:HiddenField ID="hfMensajeTipo" runat="server" />
        <asp:HiddenField ID="hfMensajeTxt" runat="server" />

        <nav class="nav">

            <div class="nav-brand">
                <div class="nav-dot"></div>
                SENA Instructor
            </div>

            <a href="DashboardInstructor.aspx" class="nav-btn">
                <i class="bi bi-arrow-left"></i>
                Volver
            </a>

        </nav>

        <div class="hero">

            <div class="hero-inner">

                <div class="hero-badge">
                    <i class="bi bi-file-earmark-plus"></i>
                    Gestión de Mejoramiento
                </div>

                <h1 class="hero-title">Crear
                    <span>Plan Interno</span>
                </h1>

                <p class="hero-sub">
                    Registra resultados incumplidos, actividades propuestas y fecha límite para el seguimiento académico del aprendiz.
                </p>

            </div>

        </div>

        <div class="body-wrap">

            <div class="card-form">

                <div class="section-title">
                    <i class="bi bi-person-fill"></i>
                    Datos del Aprendiz
                </div>

                <div class="row g-3 mb-4">

                    <div class="col-md-6">

                        <label class="form-label">Aprendiz</label>

                        <asp:DropDownList
                            ID="ddlAprendiz"
                            runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlAprendiz_SelectedIndexChanged">

                            <asp:ListItem Value="" Text="-- Seleccione un aprendiz --"></asp:ListItem>

                        </asp:DropDownList>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label">Ficha</label>

                        <asp:TextBox
                            ID="txtFicha"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true"
                            placeholder="Se carga automáticamente">
                        </asp:TextBox>

                        <asp:HiddenField ID="hfIdFicha" runat="server" Value="0" />

                    </div>

                </div>

                <div class="section-title">
                    <i class="bi bi-list-check"></i>
                    Resultados Incumplidos
                </div>

                <div class="resultados-box mb-4">

                    <asp:Panel ID="pnlResultados" runat="server">

                        <div class="empty-resultados">

                            <i class="bi bi-arrow-up-circle"></i>

                            <p>
                                Selecciona un aprendiz para visualizar los resultados disponibles.
                            </p>

                        </div>

                    </asp:Panel>

                </div>

                <div class="section-title">
                    <i class="bi bi-card-text"></i>
                    Detalles del Plan
                </div>

                <div class="row g-3 mb-4">

                    <div class="col-md-4">
                        <label class="form-label">Fecha Límite</label>
                        <asp:TextBox
                            ID="txtFechaLimite"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Date">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-8">
                        <label class="form-label">Actividades Propuestas</label>
                        <asp:TextBox
                            ID="txtActividades"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="4"
                            placeholder="Describe las actividades que debe desarrollar el aprendiz...">
                        </asp:TextBox>
                    </div>

                    <div class="col-12">
                        <label class="form-label">Observaciones</label>
                        <asp:TextBox
                            ID="txtObservaciones"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="3"
                            placeholder="Comentarios u observaciones adicionales...">
                        </asp:TextBox>
                    </div>

                </div>

                <div class="actions">

                    <a href="DashboardInstructor.aspx" class="btn-cancelar">
                        <i class="bi bi-x-lg"></i>
                        Cancelar
                    </a>

                    <asp:Button
                        ID="btnGuardar"
                        runat="server"
                        Text="Guardar Plan"
                        CssClass="btn-guardar"
                        OnClick="btnGuardar_Click"
                        OnClientClick="return confirmarGuardar();" />

                </div>

            </div>

        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>

    <script>

        function confirmarGuardar() {

            var aprendiz = document.getElementById('<%= ddlAprendiz.ClientID %>').value;

            if (aprendiz === '') {

                Swal.fire({
                    icon: 'warning',
                    title: 'Atención',
                    text: 'Debes seleccionar un aprendiz.',
                    confirmButtonColor: '#042940'
                });

                return false;
            }

            return true;
        }

        window.addEventListener('DOMContentLoaded', function () {

            var tipo = document.getElementById('<%= hfMensajeTipo.ClientID %>').value;
            var txt = document.getElementById('<%= hfMensajeTxt.ClientID %>').value;

            if (tipo === 'success') {

                Swal.fire({
                    icon: 'success',
                    title: 'Plan registrado',
                    text: txt,
                    confirmButtonColor: '#042940',
                    timer: 2400,
                    showConfirmButton: false
                });

            } else if (tipo === 'error' || tipo === 'warning') {

                Swal.fire({
                    icon: tipo,
                    title: tipo === 'error' ? 'Error' : 'Atención',
                    text: txt,
                    confirmButtonColor: '#042940'
                });
            }

        });

    </script>

</body>
</html>
