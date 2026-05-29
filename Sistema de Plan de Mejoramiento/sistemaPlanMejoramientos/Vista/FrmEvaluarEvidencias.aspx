<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmEvaluarEvidencias.aspx.cs" Inherits="sistemaPlanMejoramientos.Instructor.FrmEvaluarEvidencias" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Evaluar Evidencias | SENA Instructor</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
            --sena-dark: #042940;
            --sena-mid: #005C53;
            --sena-accent: #9FC131;
            --sena-danger: #dc3545;
            --sena-warning: #ffc107;
            --sena-success: #198754;
            --bg: #f4f7fb;
            --card: #ffffff;
            --border: #e0e6ed;
            --text-muted: #6c757d;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: 'DM Sans', 'Segoe UI', sans-serif;
            background: var(--bg);
            min-height: 100vh;
        }

        .nav {
            background: var(--sena-dark);
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 28px;
            height: 60px;
        }

        .nav-brand {
            display: flex;
            align-items: center;
            gap: 10px;
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 16px;
            color: #fff;
            text-decoration: none;
        }

        .nav-dot {
            width: 8px;
            height: 8px;
            background: var(--sena-accent);
            border-radius: 50%;
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

        .nav-right {
            display: flex;
            align-items: center;
            gap: 16px;
        }

        .nav-btn {
            border: 1px solid rgba(255,255,255,.25);
            background: transparent;
            color: rgba(255,255,255,.8);
            font-size: 12px;
            padding: 6px 16px;
            border-radius: 20px;
            cursor: pointer;
            transition: all .2s;
            text-decoration: none;
            display: inline-block;
        }

            .nav-btn:hover {
                background: rgba(255,255,255,.1);
                color: #fff;
            }

        .hero {
            background: var(--sena-dark);
            padding: 32px 28px 28px;
            position: relative;
            overflow: hidden;
        }

            .hero::before {
                content: '';
                position: absolute;
                top: -60px;
                right: -60px;
                width: 280px;
                height: 280px;
                background: var(--sena-mid);
                border-radius: 50%;
                opacity: .3;
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
            color: var(--sena-accent);
            font-size: 11px;
            font-weight: 500;
            padding: 4px 12px;
            border-radius: 20px;
            margin-bottom: 12px;
            letter-spacing: .5px;
            text-transform: uppercase;
        }

        .hero-title {
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 24px;
            color: #fff;
            margin-bottom: 8px;
        }

            .hero-title span {
                color: var(--sena-accent);
            }

        .hero-sub {
            font-size: 13px;
            color: rgba(255,255,255,.5);
            line-height: 1.6;
        }

        .page-body {
            padding: 28px;
        }

        .section-head {
            display: flex;
            align-items: center;
            gap: 14px;
            margin-bottom: 20px;
        }

        .section-label {
            font-family: 'Syne', sans-serif;
            font-size: 12px;
            font-weight: 700;
            color: var(--sena-dark);
            text-transform: uppercase;
            letter-spacing: 1.5px;
            white-space: nowrap;
        }

        .section-rule {
            flex: 1;
            height: 1px;
            background: var(--border);
        }

        .table-wrap {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 16px;
            overflow: hidden;
        }

            .table-wrap table {
                width: 100%;
                border-collapse: collapse;
                font-size: 13px;
            }

            .table-wrap thead tr {
                background: var(--sena-dark);
            }

            .table-wrap thead th {
                padding: 13px 16px;
                color: rgba(255,255,255,.8);
                font-family: 'Syne', sans-serif;
                font-size: 11px;
                font-weight: 700;
                text-transform: uppercase;
                letter-spacing: 1px;
                text-align: left;
            }

            .table-wrap tbody tr {
                border-bottom: 1px solid var(--border);
                transition: background .15s;
            }

                .table-wrap tbody tr:last-child {
                    border-bottom: none;
                }

                .table-wrap tbody tr:hover {
                    background: #f8fafc;
                }

            .table-wrap tbody td {
                padding: 12px 16px;
                color: #334155;
                vertical-align: middle;
            }

        .badge {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            font-size: 11px;
            font-weight: 600;
            padding: 3px 10px;
            border-radius: 20px;
        }

        .badge-pending {
            background: #fff3cd;
            color: #856404;
        }

        .badge-interno {
            background: #e6f1fb;
            color: #185FA5;
        }

        .badge-comite {
            background: #fce8e8;
            color: #9b1c1c;
        }

        .badge-vencido {
            background: #fce8e8;
            color: #9b1c1c;
        }

        .badge-ok {
            background: #e1f5ee;
            color: #0F6E56;
        }

        .btn-eval {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-size: 12px;
            font-weight: 600;
            background: var(--sena-dark);
            color: #fff;
            padding: 7px 16px;
            border-radius: 20px;
            border: none;
            cursor: pointer;
            transition: all .2s;
        }

            .btn-eval:hover {
                background: var(--sena-mid);
                transform: translateY(-1px);
            }

        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: var(--text-muted);
        }

            .empty-state i {
                font-size: 48px;
                opacity: .3;
                display: block;
                margin-bottom: 12px;
            }

            .empty-state p {
                font-size: 14px;
            }

        #panelEvaluar {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 16px;
            overflow: hidden;
            margin-top: 28px;
            display: none;
        }

        .panel-header {
            background: var(--sena-dark);
            padding: 18px 24px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .panel-header-title {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 15px;
            color: #fff;
        }

        .panel-header-sub {
            font-size: 12px;
            color: rgba(255,255,255,.5);
            margin-top: 3px;
        }

        .btn-close-panel {
            background: rgba(255,255,255,.1);
            border: 1px solid rgba(255,255,255,.2);
            color: #fff;
            width: 32px;
            height: 32px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: all .2s;
            font-size: 16px;
        }

            .btn-close-panel:hover {
                background: var(--sena-danger);
                border-color: var(--sena-danger);
            }

        .panel-body {
            padding: 24px;
        }

        .info-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 12px;
            margin-bottom: 24px;
        }

        .info-item {
            background: var(--bg);
            border: 1px solid var(--border);
            border-radius: 10px;
            padding: 12px 14px;
        }

            .info-item label {
                font-size: 10px;
                font-weight: 700;
                text-transform: uppercase;
                letter-spacing: 1px;
                color: var(--text-muted);
                display: block;
                margin-bottom: 4px;
            }

            .info-item span {
                font-size: 13px;
                font-weight: 500;
                color: var(--sena-dark);
            }

        .ev-list {
            margin-bottom: 24px;
        }

        .ev-item {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 10px 14px;
            background: var(--bg);
            border: 1px solid var(--border);
            border-radius: 10px;
            margin-bottom: 8px;
        }

        .ev-icon {
            width: 36px;
            height: 36px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            flex-shrink: 0;
        }

            .ev-icon.pdf {
                background: #fce8e8;
                color: #c0392b;
            }

            .ev-icon.docx {
                background: #e6f1fb;
                color: #185FA5;
            }

            .ev-icon.img {
                background: #e1f5ee;
                color: #0F6E56;
            }

            .ev-icon.zip {
                background: #fff3cd;
                color: #856404;
            }

        .ev-name {
            font-size: 13px;
            font-weight: 500;
            color: var(--sena-dark);
            flex: 1;
        }

        .ev-date {
            font-size: 11px;
            color: var(--text-muted);
        }

        .ev-download {
            font-size: 12px;
            color: var(--sena-mid);
            font-weight: 600;
            text-decoration: none;
            display: flex;
            align-items: center;
            gap: 4px;
        }

            .ev-download:hover {
                color: var(--sena-dark);
            }

        .criteria-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 14px;
            margin-bottom: 20px;
        }

        .criteria-card {
            border: 1px solid var(--border);
            border-radius: 12px;
            padding: 16px;
            background: var(--bg);
        }

        .criteria-title {
            font-family: 'Syne', sans-serif;
            font-size: 12px;
            font-weight: 700;
            color: var(--sena-dark);
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-bottom: 4px;
            display: flex;
            align-items: center;
            gap: 6px;
        }

        .criteria-desc {
            font-size: 11px;
            color: var(--text-muted);
            margin-bottom: 12px;
            line-height: 1.5;
        }

        .radio-group {
            display: flex;
            flex-direction: column;
            gap: 8px;
        }

        .radio-opt {
            display: flex;
            align-items: center;
            gap: 8px;
            padding: 8px 12px;
            border-radius: 8px;
            border: 1.5px solid var(--border);
            cursor: pointer;
            transition: all .2s;
            font-size: 13px;
            font-weight: 500;
            background: #fff;
        }

            .radio-opt:has(input:checked).aprueba {
                border-color: var(--sena-success);
                background: #e1f5ee;
                color: var(--sena-success);
            }

            .radio-opt:has(input:checked).no-aprueba {
                border-color: var(--sena-danger);
                background: #fce8e8;
                color: var(--sena-danger);
            }

            .radio-opt input {
                accent-color: var(--sena-mid);
            }

        .obs-group {
            margin-bottom: 24px;
        }

            .obs-group label {
                font-size: 11px;
                font-weight: 700;
                text-transform: uppercase;
                letter-spacing: 1px;
                color: var(--text-muted);
                display: block;
                margin-bottom: 8px;
            }

            .obs-group textarea {
                width: 100%;
                border: 1.5px solid var(--border);
                border-radius: 10px;
                padding: 12px 14px;
                font-family: 'DM Sans', sans-serif;
                font-size: 13px;
                resize: vertical;
                min-height: 90px;
                outline: none;
                transition: border .2s;
                background: var(--bg);
                color: var(--sena-dark);
            }

                .obs-group textarea:focus {
                    border-color: var(--sena-mid);
                    background: #fff;
                }

        .alerta-vencido {
            display: flex;
            align-items: center;
            gap: 10px;
            background: #fff3cd;
            border: 1px solid #ffc107;
            border-radius: 10px;
            padding: 12px 16px;
            font-size: 13px;
            color: #856404;
            margin-bottom: 20px;
        }

            .alerta-vencido i {
                font-size: 18px;
                flex-shrink: 0;
            }

        .btn-submit {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            background: var(--sena-mid);
            color: #fff;
            font-family: 'Syne', sans-serif;
            font-size: 13px;
            font-weight: 700;
            padding: 12px 28px;
            border-radius: 10px;
            border: none;
            cursor: pointer;
            transition: all .25s;
        }

            .btn-submit:hover {
                background: var(--sena-dark);
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(0,92,83,.3);
            }

            .btn-submit:active {
                transform: translateY(0);
            }

        .toast-wrap {
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 9999;
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .toast {
            min-width: 300px;
            padding: 14px 18px;
            border-radius: 12px;
            display: flex;
            align-items: flex-start;
            gap: 10px;
            box-shadow: 0 8px 24px rgba(0,0,0,.15);
            animation: slideIn .3s ease;
            font-size: 13px;
        }

            .toast.success {
                background: #042940;
                color: #fff;
                border-left: 4px solid var(--sena-accent);
            }

            .toast.comite {
                background: #fff3cd;
                color: #856404;
                border-left: 4px solid #ffc107;
            }

            .toast.cancelado {
                background: #fce8e8;
                color: #9b1c1c;
                border-left: 4px solid var(--sena-danger);
            }

            .toast.error {
                background: #fce8e8;
                color: #9b1c1c;
                border-left: 4px solid var(--sena-danger);
            }

            .toast i {
                font-size: 18px;
                flex-shrink: 0;
                margin-top: 1px;
            }

        .toast-title {
            font-weight: 700;
            margin-bottom: 2px;
        }

        .toast-msg {
            opacity: .8;
            line-height: 1.4;
        }

        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateX(40px);
            }

            to {
                opacity: 1;
                transform: translateX(0);
            }
        }

        @media (max-width: 768px) {
            .info-grid {
                grid-template-columns: 1fr 1fr;
            }

            .criteria-grid {
                grid-template-columns: 1fr;
            }

            .page-body {
                padding: 16px;
            }
        }

        .table-wrap {
            overflow-x: auto;
        }

        @media (max-width: 768px) {

            .table-wrap {
                overflow-x: auto;
            }

            .info-grid {
                grid-template-columns: 1fr;
            }

            .criteria-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="toast-wrap" id="toastWrap"></div>

        <asp:HiddenField ID="hfIdPlan" runat="server" />
        <asp:HiddenField ID="hfIdAprendiz" runat="server" />
        <asp:HiddenField ID="hfTipoPlan" runat="server" />
        <asp:HiddenField ID="hfFechaLimite" runat="server" />
        <asp:HiddenField ID="hfVencido" runat="server" Value="0" />

        <nav class="nav">
            <a href="DashboardInstructor.aspx" class="nav-brand">
                <div class="nav-dot"></div>
                SENA Instructor
        </a>
            <div class="nav-right">
                <a href="DashboardInstructor.aspx" class="nav-btn">
                    <i class="bi bi-arrow-left"></i>Volver al Dashboard
            </a>
            </div>
        </nav>

        <div class="hero">
            <div class="hero-inner">
                <div class="hero-badge">
                    <i class="bi bi-check-all"></i>Evaluación de Evidencias
           
                </div>
                <h1 class="hero-title">Evaluar <span>Evidencias</span></h1>
                <p class="hero-sub">Califica Producto, Conocimiento y Desempeño de cada plan pendiente. Un plan vencido se reprueba automáticamente.</p>
            </div>
        </div>

        <div class="page-body">

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Planes con Evidencias Pendientes de Evaluación</span>
                <div class="section-rule"></div>
            </div>

            <div class="table-wrap">
                <asp:Repeater ID="rptPlanes" runat="server" OnItemCommand="rptPlanes_ItemCommand">
                    <HeaderTemplate>
                        <table>
                            <thead>
                                <tr>
                                    <th>Aprendiz</th>
                                    <th>Tipo Plan</th>
                                    <th>Ficha</th>
                                    <th>Fecha Asignación</th>
                                    <th>Fecha Límite</th>
                                    <th>Evidencias</th>
                                    <th>Estado</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <strong><%# Eval("nombreAprendiz") %></strong><br />
                                <span style="font-size: 11px; color: #6c757d;"><%# Eval("docAprendiz") %></span>
                            </td>
                            <td>
                                <span class='badge <%# Eval("tipoPlan").ToString() == "Interno" ? "badge-interno" : "badge-comite" %>'>
                                    <i class='bi <%# Eval("tipoPlan").ToString() == "Interno" ? "bi-file-earmark-text" : "bi-people-fill" %>'></i>
                                    <%# Eval("tipoPlan") %>
                            </span>
                            </td>
                            <td><%# Eval("codigoFicha") %></td>
                            <td><%# Convert.ToDateTime(Eval("fechaAsignacion")).ToString("dd/MM/yyyy") %></td>
                            <td>
                                <%# Convert.ToDateTime(Eval("fechaLimite")) < DateTime.Now
                                ? "<span class='badge badge-vencido'><i class='bi bi-exclamation-triangle'></i> " + Convert.ToDateTime(Eval("fechaLimite")).ToString("dd/MM/yyyy") + "</span>"
                                : "<span style='font-size:13px;'>" + Convert.ToDateTime(Eval("fechaLimite")).ToString("dd/MM/yyyy") + "</span>" %>
                        </td>
                            <td style="text-align: center;">
                                <strong><%# Eval("totalEvidencias") %></strong>
                            </td>
                            <td><span class="badge badge-pending"><i class="bi bi-clock"></i>Pendiente</span></td>
                            <td>
                                <asp:Button ID="btnEvaluar" runat="server" Text="Evaluar"
                                    CssClass="btn-eval"
                                    CommandName="Evaluar"
                                    CommandArgument='<%# Eval("idPlanMejoramiento") + "|" + Eval("idAprendiz") + "|" + Eval("tipoPlan") + "|" + Convert.ToDateTime(Eval("fechaLimite")).ToString("yyyy-MM-dd") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                    </table>
               
                    </FooterTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlVacio" runat="server" Visible="false">
                    <div class="empty-state">
                        <i class="bi bi-inbox"></i>
                        <p>No hay planes con evidencias pendientes de evaluación.</p>
                    </div>
                </asp:Panel>
            </div>

            <div id="panelEvaluar" runat="server" clientidmode="Static">

                <div class="panel-header">
                    <div>
                        <div class="panel-header-title">
                            <i class="bi bi-clipboard-check" style="color: var(--sena-accent); margin-right: 8px;"></i>
                            Evaluando Plan #<asp:Label ID="lblIdPlanHeader" runat="server"></asp:Label>
                            —
                            <asp:Label ID="lblNombreAprendizHeader" runat="server"></asp:Label>
                        </div>
                        <div class="panel-header-sub">
                            Tipo:
                            <asp:Label ID="lblTipoPlanHeader" runat="server"></asp:Label>
                            &nbsp;|&nbsp; Fecha límite:
                            <asp:Label ID="lblFechaLimiteHeader" runat="server"></asp:Label>
                        </div>
                    </div>
                    <button type="button" class="btn-close-panel" onclick="cerrarPanel()">
                        <i class="bi bi-x"></i>
                    </button>
                </div>

                <div class="panel-body">

                    <div class="info-grid">
                        <div class="info-item">
                            <label>Aprendiz</label>
                            <span>
                                <asp:Label ID="lblAprendizInfo" runat="server"></asp:Label></span>
                        </div>
                        <div class="info-item">
                            <label>Ficha</label>
                            <span>
                                <asp:Label ID="lblFichaInfo" runat="server"></asp:Label></span>
                        </div>
                        <div class="info-item">
                            <label>Actividades del Plan</label>
                            <span>
                                <asp:Label ID="lblActividadesInfo" runat="server"></asp:Label></span>
                        </div>
                    </div>

                    <asp:Panel ID="pnlAlertaVencido" runat="server" Visible="false">
                        <div class="alerta-vencido">
                            <i class="bi bi-exclamation-triangle-fill"></i>
                            <div>
                                <strong>Plan vencido:</strong> la fecha límite ya pasó. Los criterios se marcarán automáticamente como <strong>No Aprobado</strong> y se generará un plan por Comité (o se cancelará el aprendiz si es plan Comité).
                       
                            </div>
                        </div>
                    </asp:Panel>

                    <div class="section-head" style="margin-bottom: 12px;">
                        <div class="section-rule"></div>
                        <span class="section-label">Evidencias Subidas</span>
                        <div class="section-rule"></div>
                    </div>
                    <div class="ev-list">
                        <asp:Repeater ID="rptEvidencias" runat="server">
                            <ItemTemplate>
                                <div class="ev-item">
                                    <div class='ev-icon <%# GetIconClass(Eval("tipoArchivo").ToString()) %>'>
                                        <i class='bi <%# GetIconBi(Eval("tipoArchivo").ToString()) %>'></i>
                                    </div>
                                    <div class="ev-name"><%# Eval("nombreArchivo") %></div>
                                    <div class="ev-date"><%# Convert.ToDateTime(Eval("fechaSubida")).ToString("dd/MM/yyyy HH:mm") %></div>
                                    <a href='<%# ResolveUrl("~/") + Eval("rutaArchivo") %>' target="_blank" class="ev-download">
                                        <i class="bi bi-download"></i>Descargar
                                </a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlSinEvidencias" runat="server" Visible="false">
                            <div class="ev-item" style="color: var(--text-muted); font-size: 13px;">
                                <i class="bi bi-folder2-open" style="font-size: 20px; margin-right: 8px; opacity: .4;"></i>
                                El aprendiz aún no ha subido evidencias.
                       
                            </div>
                        </asp:Panel>
                    </div>

                    <div class="section-head" style="margin-bottom: 12px;">
                        <div class="section-rule"></div>
                        <span class="section-label">Criterios de Evaluación</span>
                        <div class="section-rule"></div>
                    </div>

                    <div class="criteria-grid">
                        <div class="criteria-card">
                            <div class="criteria-title">
                                <i class="bi bi-box-seam"></i>Producto
                       
                            </div>
                            <div class="criteria-desc">Evaluación de la evidencia entregada (archivo, documento, trabajo).</div>
                            <div class="radio-group">
                                <label class="radio-opt aprueba">
                                    <asp:RadioButton ID="rbProductoAprueba" runat="server" GroupName="producto" />
                                    <i class="bi bi-check-circle-fill" style="color: var(--sena-success);"></i>Aprueba
                           
                                </label>
                                <label class="radio-opt no-aprueba">
                                    <asp:RadioButton ID="rbProductoNoAprueba" runat="server" GroupName="producto" />
                                    <i class="bi bi-x-circle-fill" style="color: var(--sena-danger);"></i>No Aprueba
                           
                                </label>
                            </div>
                        </div>

                        <div class="criteria-card">
                            <div class="criteria-title">
                                <i class="bi bi-lightbulb"></i>Conocimiento
                       
                            </div>
                            <div class="criteria-desc">Sustentación o explicación verbal del trabajo realizado.</div>
                            <div class="radio-group">
                                <label class="radio-opt aprueba">
                                    <asp:RadioButton ID="rbConocimientoAprueba" runat="server" GroupName="conocimiento" />
                                    <i class="bi bi-check-circle-fill" style="color: var(--sena-success);"></i>Aprueba
                           
                                </label>
                                <label class="radio-opt no-aprueba">
                                    <asp:RadioButton ID="rbConocimientoNoAprueba" runat="server" GroupName="conocimiento" />
                                    <i class="bi bi-x-circle-fill" style="color: var(--sena-danger);"></i>No Aprueba
                           
                                </label>
                            </div>
                        </div>

                        <div class="criteria-card">
                            <div class="criteria-title">
                                <i class="bi bi-bar-chart-steps"></i>Desempeño
                       
                            </div>
                            <div class="criteria-desc">Capacidad para realizar mejoras o nuevas funcionalidades.</div>
                            <div class="radio-group">
                                <label class="radio-opt aprueba">
                                    <asp:RadioButton ID="rbDesempenoAprueba" runat="server" GroupName="desempeno" />
                                    <i class="bi bi-check-circle-fill" style="color: var(--sena-success);"></i>Aprueba
                           
                                </label>
                                <label class="radio-opt no-aprueba">
                                    <asp:RadioButton ID="rbDesempenoNoAprueba" runat="server" GroupName="desempeno" />
                                    <i class="bi bi-x-circle-fill" style="color: var(--sena-danger);"></i>No Aprueba
                           
                                </label>
                            </div>
                        </div>
                    </div>

                    <div class="obs-group">
                        <label><i class="bi bi-chat-left-text"></i>&nbsp;Observaciones del Instructor</label>
                        <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine"
                            placeholder="Escribe retroalimentación para el aprendiz..." CssClass=""></asp:TextBox>
                    </div>

                    <asp:Button ID="btnGuardarEvaluacion" runat="server"
                        Text="Registrar Evaluación"
                        CssClass="btn-submit"
                        OnClick="btnGuardarEvaluacion_Click"
                        OnClientClick="return validarCriterios();" />

                </div>
            </div>

            <asp:HiddenField ID="hfResultado" runat="server" Value="" />

        </div>

    </form>

    <script>
        function abrirPanel() {
            var p = document.getElementById('panelEvaluar');
            p.style.display = 'block';
            p.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
        function cerrarPanel() {
            document.getElementById('panelEvaluar').style.display = 'none';
        }

        function validarCriterios() {
            var grupos = ['producto', 'conocimiento', 'desempeno'];
            for (var i = 0; i < grupos.length; i++) {
                var radios = document.querySelectorAll('input[name*="' + grupos[i] + '"]');
                var marcado = false;
                for (var r = 0; r < radios.length; r++) {
                    if (radios[r].checked) { marcado = true; break; }
                }
                if (!marcado) {
                    mostrarToast('error', 'Criterio incompleto', 'Debes calificar todos los criterios antes de guardar.');
                    return false;
                }
            }
            return true;
        }

        function mostrarToast(tipo, titulo, mensaje) {
            var wrap = document.getElementById('toastWrap');
            var iconos = { success: 'bi-check-circle-fill', comite: 'bi-people-fill', cancelado: 'bi-person-x-fill', error: 'bi-exclamation-circle-fill' };
            var t = document.createElement('div');
            t.className = 'toast ' + tipo;
            t.innerHTML = '<i class="bi ' + (iconos[tipo] || 'bi-info-circle') + '"></i><div><div class="toast-title">' + titulo + '</div><div class="toast-msg">' + mensaje + '</div></div>';
            wrap.appendChild(t);
            setTimeout(function () { if (t.parentNode) t.parentNode.removeChild(t); }, 5000);
        }

        window.onload = function () {
            var hf = document.getElementById('<%= hfResultado.ClientID %>');
            if (hf && hf.value) {
                var val = hf.value;
                if (val === 'Aprobado')
                    mostrarToast('success', '¡Plan Aprobado!', 'El aprendiz ha superado todos los criterios de evaluación.');
                else if (val === 'Comite')
                    mostrarToast('comite', 'Plan por Comité Generado', 'El aprendiz no aprobó el plan interno. Se generó automáticamente un plan por Comité.');
                else if (val === 'Cancelado')
                    mostrarToast('cancelado', 'Aprendiz Cancelado', 'El aprendiz no aprobó el plan por Comité. Su estado académico cambió a Cancelado automáticamente.');
                else if (val === 'Error')
                    mostrarToast('error', 'Error al evaluar', 'Ocurrió un error al registrar la evaluación. Inténtalo de nuevo.');

                hf.value = '';
            }

            var panelVisible = document.getElementById('<%= hfIdPlan.ClientID %>');
            if (panelVisible && panelVisible.value && panelVisible.value !== '0') {
                abrirPanel();
            }
        };
</script>
</body>
</html>
