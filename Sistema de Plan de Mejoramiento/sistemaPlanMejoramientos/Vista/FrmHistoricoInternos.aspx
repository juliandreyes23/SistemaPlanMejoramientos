<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmHistoricoInternos.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.FrmHistoricoInternos" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Historial Planes Internos | SENA Instructor</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
            --sena-dark: #042940;
            --sena-mid: #005C53;
            --sena-accent: #9FC131;
            --sena-danger: #dc3545;
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
                transform: scale(1)
            }

            50% {
                opacity: .5;
                transform: scale(1.4)
            }
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

        .filtros-bar {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 12px;
            padding: 16px 20px;
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 20px;
            flex-wrap: wrap;
        }

            .filtros-bar input, .filtros-bar select {
                border: 1.5px solid var(--border);
                border-radius: 8px;
                padding: 8px 12px;
                font-family: 'DM Sans', sans-serif;
                font-size: 13px;
                outline: none;
                transition: border .2s;
                background: var(--bg);
                color: var(--sena-dark);
            }

                .filtros-bar input:focus, .filtros-bar select:focus {
                    border-color: var(--sena-mid);
                    background: #fff;
                }

            .filtros-bar input {
                flex: 1;
                min-width: 200px;
            }

        .btn-filtrar {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: var(--sena-dark);
            color: #fff;
            font-size: 12px;
            font-weight: 600;
            padding: 8px 18px;
            border-radius: 8px;
            border: none;
            cursor: pointer;
            transition: all .2s;
        }

            .btn-filtrar:hover {
                background: var(--sena-mid);
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

        .badge-pendiente {
            background: #fff3cd;
            color: #856404;
        }

        .badge-aprobado {
            background: #e1f5ee;
            color: #0F6E56;
        }

        .badge-noaprobado {
            background: #fce8e8;
            color: #9b1c1c;
        }

        .btn-detalle {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-size: 12px;
            font-weight: 600;
            background: var(--sena-dark);
            color: #fff;
            padding: 7px 14px;
            border-radius: 20px;
            border: none;
            cursor: pointer;
            transition: all .2s;
        }

            .btn-detalle:hover {
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

        #panelDetalle {
            display: none;
            margin-top: 28px;
        }

        .detail-card {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 16px;
            overflow: hidden;
        }

        .detail-header {
            background: var(--sena-dark);
            padding: 18px 24px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .detail-header-title {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 15px;
            color: #fff;
        }

        .detail-header-sub {
            font-size: 12px;
            color: rgba(255,255,255,.5);
            margin-top: 3px;
        }

        .btn-close {
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

            .btn-close:hover {
                background: var(--sena-danger);
                border-color: var(--sena-danger);
            }

        .detail-body {
            padding: 24px;
        }

        .info-grid {
            display: grid;
            grid-template-columns: repeat(3,1fr);
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

        .rap-list {
            margin-bottom: 24px;
        }

        .rap-item {
            display: flex;
            align-items: flex-start;
            gap: 10px;
            padding: 10px 14px;
            background: var(--bg);
            border: 1px solid var(--border);
            border-radius: 10px;
            margin-bottom: 8px;
            font-size: 13px;
            color: var(--sena-dark);
        }

            .rap-item i {
                color: var(--sena-mid);
                margin-top: 2px;
                flex-shrink: 0;
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

        .eval-grid {
            display: grid;
            grid-template-columns: repeat(3,1fr);
            gap: 12px;
            margin-bottom: 20px;
        }

        .eval-crit {
            border-radius: 12px;
            padding: 14px 16px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

            .eval-crit.aprobado {
                background: #e1f5ee;
                border: 1px solid #a7f3d0;
            }

            .eval-crit.noaprobado {
                background: #fce8e8;
                border: 1px solid #fca5a5;
            }

            .eval-crit.sineval {
                background: var(--bg);
                border: 1px solid var(--border);
            }

        .eval-crit-icon {
            font-size: 22px;
        }

        .eval-crit-label {
            font-size: 10px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: var(--text-muted);
        }

        .eval-crit-val {
            font-size: 13px;
            font-weight: 700;
            color: var(--sena-dark);
            margin-top: 2px;
        }

        .eval-crit.aprobado .eval-crit-val {
            color: var(--sena-success);
        }

        .eval-crit.noaprobado .eval-crit-val {
            color: var(--sena-danger);
        }

        .obs-box {
            background: var(--bg);
            border: 1px solid var(--border);
            border-radius: 10px;
            padding: 14px 16px;
            font-size: 13px;
            color: #334155;
            line-height: 1.6;
        }

        @media (max-width: 768px) {
            .info-grid {
                grid-template-columns: 1fr 1fr;
            }

            .eval-grid {
                grid-template-columns: 1fr;
            }

            .page-body {
                padding: 16px;
            }
        }

        .table-wrap {
            width: 100%;
            overflow-x: auto;
        }

            .table-wrap table {
                min-width: 1100px;
            }

        #panelDetalle {
            width: 100%;
        }

        .detail-card {
            width: 100%;
        }

        @media (min-width: 1400px) {
            .page-body {
                max-width: 1600px;
                margin: 0 auto;
            }
        }

        @media (max-width: 992px) {
            .info-grid {
                grid-template-columns: 1fr 1fr;
            }

            .eval-grid {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 768px) {
            .nav {
                padding: 0 16px;
            }

            .hero {
                padding: 24px 16px;
            }

            .page-body {
                padding: 14px;
            }

            .hero-title {
                font-size: 20px;
            }

            .filtros-bar {
                flex-direction: column;
                align-items: stretch;
            }

                .filtros-bar input,
                .filtros-bar select,
                .btn-filtrar {
                    width: 100%;
                }

            .info-grid {
                grid-template-columns: 1fr;
            }

            .detail-body {
                padding: 16px;
            }

            .table-wrap table {
                min-width: 900px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfIdPlanDetalle" runat="server" Value="0" />

        <nav class="nav">
            <a href="DashboardInstructor.aspx" class="nav-brand">
                <div class="nav-dot"></div>
                SENA Instructor
        </a>
            <a href="DashboardInstructor.aspx" class="nav-btn">
                <i class="bi bi-arrow-left"></i>Volver al Dashboard
        </a>
        </nav>

        <div class="hero">
            <div class="hero-inner">
                <div class="hero-badge"><i class="bi bi-file-earmark-text"></i>Planes Internos</div>
                <h1 class="hero-title"><span>Planes Internos</span></h1>
                <p class="hero-sub">Consulta todos los planes de mejoramiento internos generados, su estado y el detalle de evaluación de cada aprendiz.</p>
            </div>
        </div>

        <div class="page-body">

            <div class="filtros-bar">
                <i class="bi bi-search" style="color: var(--text-muted);"></i>
                <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por nombre o documento del aprendiz..." />
                <asp:DropDownList ID="ddlEstado" runat="server">
                    <asp:ListItem Value="">Todos los estados</asp:ListItem>
                    <asp:ListItem Value="Pendiente">Pendiente</asp:ListItem>
                    <asp:ListItem Value="Aprobado">Aprobado</asp:ListItem>
                    <asp:ListItem Value="No Aprobado">No Aprobado</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar"
                    CssClass="btn-filtrar" OnClick="btnFiltrar_Click" />
            </div>

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Planes de Mejoramiento Internos</span>
                <div class="section-rule"></div>
            </div>

            <div class="table-wrap">
                <asp:Repeater ID="rptPlanes" runat="server" OnItemCommand="rptPlanes_ItemCommand">
                    <HeaderTemplate>
                        <table>
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Aprendiz</th>
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
                            <td style="color: var(--text-muted); font-size: 12px;">#<%# Eval("idPlanMejoramiento") %></td>
                            <td>
                                <strong><%# Eval("nombreAprendiz") %></strong><br />
                                <span style="font-size: 11px; color: var(--text-muted);"><%# Eval("docAprendiz") %></span>
                            </td>
                            <td><%# Eval("codigoFicha") %></td>
                            <td><%# Convert.ToDateTime(Eval("fechaAsignacion")).ToString("dd/MM/yyyy") %></td>
                            <td>
                                <%# Convert.ToDateTime(Eval("fechaLimite")) < DateTime.Now && Eval("estadoPlan").ToString() == "Pendiente"
                                ? "<span class='badge badge-noaprobado'><i class='bi bi-clock-history'></i> " + Convert.ToDateTime(Eval("fechaLimite")).ToString("dd/MM/yyyy") + " (Vencido)</span>"
                                : Convert.ToDateTime(Eval("fechaLimite")).ToString("dd/MM/yyyy") %>
                        </td>
                            <td style="text-align: center;">
                                <strong><%# Eval("totalEvidencias") %></strong>
                            </td>
                            <td>
                                <%# GetBadgeEstado(Eval("estadoPlan").ToString()) %>
                        </td>
                            <td>
                                <asp:Button ID="btnVerDetalle" runat="server"
                                    Text="Ver Detalle"
                                    CssClass="btn-detalle"
                                    CommandName="Detalle"
                                    CommandArgument='<%# Eval("idPlanMejoramiento") %>' />
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
                        <p>No hay planes internos registrados.</p>
                    </div>
                </asp:Panel>
            </div>

            <div id="panelDetalle" runat="server" clientidmode="Static">
                <div class="detail-card">
                    <div class="detail-header">
                        <div>
                            <div class="detail-header-title">
                                <i class="bi bi-file-earmark-text" style="color: var(--sena-accent); margin-right: 8px;"></i>
                                Plan Interno #<asp:Label ID="lblIdPlan" runat="server"></asp:Label>
                                —
                                <asp:Label ID="lblAprendizHeader" runat="server"></asp:Label>
                            </div>
                            <div class="detail-header-sub">
                                Estado:
                                <asp:Label ID="lblEstadoHeader" runat="server"></asp:Label>
                                &nbsp;|&nbsp; Ficha:
                                <asp:Label ID="lblFichaHeader" runat="server"></asp:Label>
                            </div>
                        </div>
                        <button type="button" class="btn-close" onclick="cerrarDetalle()">
                            <i class="bi bi-x"></i>
                        </button>
                    </div>

                    <div class="detail-body">

                        <div class="info-grid">
                            <div class="info-item">
                                <label>Fecha de Asignación</label>
                                <span>
                                    <asp:Label ID="lblFechaAsig" runat="server"></asp:Label></span>
                            </div>
                            <div class="info-item">
                                <label>Fecha Límite</label>
                                <span>
                                    <asp:Label ID="lblFechaLimite" runat="server"></asp:Label></span>
                            </div>
                            <div class="info-item">
                                <label>Instructor</label>
                                <span>
                                    <asp:Label ID="lblInstructor" runat="server"></asp:Label></span>
                            </div>
                        </div>

                        <div class="section-head" style="margin-bottom: 10px;">
                            <div class="section-rule"></div>
                            <span class="section-label">Actividades del Plan</span>
                            <div class="section-rule"></div>
                        </div>
                        <div class="obs-box" style="margin-bottom: 24px;">
                            <asp:Label ID="lblActividades" runat="server"></asp:Label>
                        </div>

                        <div class="section-head" style="margin-bottom: 10px;">
                            <div class="section-rule"></div>
                            <span class="section-label">Resultados de Aprendizaje Asociados</span>
                            <div class="section-rule"></div>
                        </div>
                        <div class="rap-list">
                            <asp:Repeater ID="rptResultados" runat="server">
                                <ItemTemplate>
                                    <div class="rap-item">
                                        <i class="bi bi-check2-circle"></i>
                                        <%# Eval("descripcion") %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Panel ID="pnlSinResultados" runat="server" Visible="false">
                                <div class="rap-item" style="color: var(--text-muted);">
                                    <i class="bi bi-dash-circle"></i>Sin resultados asociados.
                           
                                </div>
                            </asp:Panel>
                        </div>

                        <div class="section-head" style="margin-bottom: 10px;">
                            <div class="section-rule"></div>
                            <span class="section-label">Evidencias Subidas</span>
                            <div class="section-rule"></div>
                        </div>
                        <div style="margin-bottom: 24px;">
                            <asp:Repeater ID="rptEvidencias" runat="server">
                                <ItemTemplate>
                                    <div class="ev-item">
                                        <div class='ev-icon <%# GetIconClass(Eval("tipoArchivo").ToString()) %>'>
                                            <i class='bi <%# GetIconBi(Eval("tipoArchivo").ToString()) %>'></i>
                                        </div>
                                        <div class="ev-name"><%# Eval("nombreArchivo") %></div>
                                        <div class="ev-date"><%# Convert.ToDateTime(Eval("fechaSubida")).ToString("dd/MM/yyyy HH:mm") %></div>
                                        <a href='<%# ResolveUrl(Eval("rutaArchivo").ToString()) %>' target="_blank" class="ev-download">
                                            <i class="bi bi-download"></i>Descargar
                                    </a>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Panel ID="pnlSinEvidencias" runat="server" Visible="false">
                                <div class="ev-item" style="color: var(--text-muted); font-size: 13px;">
                                    <i class="bi bi-folder2-open" style="font-size: 20px; margin-right: 8px; opacity: .4;"></i>
                                    El aprendiz no ha subido evidencias.
                           
                                </div>
                            </asp:Panel>
                        </div>

                        <div class="section-head" style="margin-bottom: 10px;">
                            <div class="section-rule"></div>
                            <span class="section-label">Resultado de Evaluación</span>
                            <div class="section-rule"></div>
                        </div>

                        <asp:Panel ID="pnlSinEvaluacion" runat="server" Visible="false">
                            <div class="obs-box" style="margin-bottom: 16px; color: var(--text-muted);">
                                <i class="bi bi-hourglass-split"></i>Este plan aún no ha sido evaluado.
                       
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlConEvaluacion" runat="server" Visible="false">
                            <div class="eval-grid">
                                <div id="divProducto" runat="server" class="eval-crit sineval">
                                    <div class="eval-crit-icon">
                                        <asp:Label ID="lblIconProducto" runat="server" Text="📦"></asp:Label>
                                    </div>
                                    <div>
                                        <div class="eval-crit-label">Producto</div>
                                        <div class="eval-crit-val">
                                            <asp:Label ID="lblProducto" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="divConocimiento" runat="server" class="eval-crit sineval">
                                    <div class="eval-crit-icon">
                                        <asp:Label ID="lblIconConocimiento" runat="server" Text="💡"></asp:Label>
                                    </div>
                                    <div>
                                        <div class="eval-crit-label">Conocimiento</div>
                                        <div class="eval-crit-val">
                                            <asp:Label ID="lblConocimiento" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="divDesempeno" runat="server" class="eval-crit sineval">
                                    <div class="eval-crit-icon">
                                        <asp:Label ID="lblIconDesempeno" runat="server" Text="📊"></asp:Label>
                                    </div>
                                    <div>
                                        <div class="eval-crit-label">Desempeño</div>
                                        <div class="eval-crit-val">
                                            <asp:Label ID="lblDesempeno" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="section-head" style="margin-bottom: 10px;">
                                <div class="section-rule"></div>
                                <span class="section-label">Observaciones del Instructor</span>
                                <div class="section-rule"></div>
                            </div>
                            <div class="obs-box">
                                <asp:Label ID="lblObsEvaluacion" runat="server"></asp:Label>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </div>

        </div>

    </form>
    <script>
        function cerrarDetalle() {
            document.getElementById('panelDetalle').style.display = 'none';
        }
        window.onload = function () {
            var hf = document.getElementById('<%= hfIdPlanDetalle.ClientID %>');
            if (hf && hf.value && hf.value !== '0')
                document.getElementById('panelDetalle').style.display = 'block';
        };
</script>
</body>
</html>
