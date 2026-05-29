<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmHistoricoComite.aspx.cs" Inherits="sistemaPlanMejoramientos.Instructor.FrmHistoricoComite" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Historial Planes por Comité | SENA Instructor</title>
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
                background: #7b1c1c;
                border-radius: 50%;
                opacity: .25;
            }

        .hero-inner {
            position: relative;
            z-index: 1;
        }

        .hero-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: rgba(220,53,69,.15);
            border: 1px solid rgba(220,53,69,.35);
            color: #f87171;
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
                color: #f87171;
            }

        .hero-sub {
            font-size: 13px;
            color: rgba(255,255,255,.5);
            line-height: 1.6;
        }

        .page-body {
            padding: 28px;
        }

        .filter-bar {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 14px;
            padding: 16px 20px;
            display: flex;
            gap: 12px;
            align-items: flex-end;
            flex-wrap: wrap;
            margin-bottom: 20px;
        }

        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 5px;
            flex: 1;
            min-width: 180px;
        }

            .filter-group label {
                font-size: 10px;
                font-weight: 700;
                text-transform: uppercase;
                letter-spacing: 1px;
                color: var(--text-muted);
            }

            .filter-group input, .filter-group select {
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

                .filter-group input:focus, .filter-group select:focus {
                    border-color: var(--sena-mid);
                    background: #fff;
                }

        .btn-filtrar {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: var(--sena-dark);
            color: #fff;
            font-size: 13px;
            font-weight: 600;
            padding: 9px 20px;
            border-radius: 8px;
            border: none;
            cursor: pointer;
            transition: all .2s;
        }

            .btn-filtrar:hover {
                background: var(--sena-mid);
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

        .plan-grid {
            display: flex;
            flex-direction: column;
            gap: 14px;
        }

        .plan-card {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 14px;
            overflow: hidden;
            transition: box-shadow .2s;
        }

            .plan-card:hover {
                box-shadow: 0 6px 20px rgba(0,0,0,.07);
            }

        .plan-card-header {
            background: #7b1c1c;
            padding: 14px 20px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 10px;
        }

            .plan-card-header.aprobado {
                background: var(--sena-mid);
            }

            .plan-card-header.noaprobado {
                background: #7b1c1c;
            }

            .plan-card-header.pendiente {
                background: var(--sena-dark);
            }

        .plan-aprendiz {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 14px;
            color: #fff;
        }

        .plan-doc {
            font-size: 11px;
            color: rgba(255,255,255,.55);
            margin-top: 2px;
        }

        .plan-card-body {
            padding: 16px 20px;
            display: flex;
            flex-wrap: wrap;
            gap: 16px;
            align-items: center;
        }

        .plan-meta {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
            flex: 1;
        }

        .meta-item {
            display: flex;
            flex-direction: column;
            gap: 2px;
        }

        .meta-label {
            font-size: 10px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: var(--text-muted);
        }

        .meta-val {
            font-size: 13px;
            font-weight: 500;
            color: var(--sena-dark);
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

        .badge-comite {
            background: #fce8e8;
            color: #9b1c1c;
        }

        .btn-detalle {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-size: 12px;
            font-weight: 600;
            background: transparent;
            color: var(--sena-dark);
            border: 1.5px solid var(--border);
            padding: 7px 16px;
            border-radius: 20px;
            cursor: pointer;
            transition: all .2s;
        }

            .btn-detalle:hover {
                background: var(--sena-dark);
                color: #fff;
                border-color: var(--sena-dark);
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

        #panelDetalle {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 16px;
            overflow: hidden;
            margin-top: 28px;
            display: none;
        }

        .detail-header {
            background: #7b1c1c;
            padding: 18px 24px;
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
        }

        .detail-title {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 15px;
            color: #fff;
        }

        .detail-sub {
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

        .detail-body {
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
        }

            .rap-item i {
                color: #f87171;
                margin-top: 2px;
                flex-shrink: 0;
            }

            .rap-item span {
                font-size: 13px;
                color: var(--sena-dark);
                line-height: 1.5;
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

        .eval-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 14px;
            margin-bottom: 20px;
        }

        .eval-crit {
            border-radius: 12px;
            padding: 16px;
            text-align: center;
            font-family: 'Syne', sans-serif;
            font-size: 13px;
            font-weight: 700;
        }

            .eval-crit.aprobado {
                background: #e1f5ee;
                color: #0F6E56;
                border: 1px solid #a7f3d0;
            }

            .eval-crit.noaprobado {
                background: #fce8e8;
                color: #9b1c1c;
                border: 1px solid #fca5a5;
            }

            .eval-crit.sineval {
                background: var(--bg);
                color: var(--text-muted);
                border: 1px solid var(--border);
            }

        .obs-box {
            background: var(--bg);
            border: 1px solid var(--border);
            border-radius: 10px;
            padding: 14px 16px;
            font-size: 13px;
            color: var(--sena-dark);
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

        .plan-grid {
            display: flex;
            flex-direction: column;
            gap: 14px;
            max-width: 1400px;
            margin: 0 auto;
        }

        .plan-card {
            width: 100%;
        }

        .page-body {
            padding: 28px;
            max-width: 1500px;
            margin: 0 auto;
        }

        #panelDetalle {
            width: 100%;
            max-width: 1400px;
            margin: 28px auto 0;
        }

        .detail-body {
            padding: 28px;
        }

        .info-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 14px;
        }

        .eval-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 14px;
        }

        .ev-item,
        .rap-item {
            width: 100%;
            overflow-wrap: break-word;
        }

        .ev-name,
        .obs-box,
        .meta-val,
        .info-item span {
            word-break: break-word;
        }

        @media (max-width: 768px) {
            .page-body {
                padding: 16px;
            }

            .detail-body {
                padding: 18px;
            }

            .info-grid,
            .eval-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfIdPlanDetalle" runat="server" Value="" />

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
                <div class="hero-badge">
                    <i class="bi bi-people-fill"></i>Segunda Instancia
                </div>
                <h1 class="hero-title">Planes por <span>Comité</span></h1>
                <p class="hero-sub">Historial de planes en segunda instancia. Un dictamen negativo cancela automáticamente al aprendiz.</p>
            </div>
        </div>

        <div class="page-body">

            <div class="filter-bar">
                <div class="filter-group">
                    <label><i class="bi bi-search"></i>Buscar aprendiz</label>
                    <asp:TextBox ID="txtBuscar" runat="server" placeholder="Nombre o documento..." />
                </div>
                <div class="filter-group" style="max-width: 180px;">
                    <label><i class="bi bi-funnel"></i>Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Value="" Text="Todos los estados" />
                        <asp:ListItem Value="Pendiente" Text="Pendiente" />
                        <asp:ListItem Value="Aprobado" Text="Aprobado" />
                        <asp:ListItem Value="No Aprobado" Text="No Aprobado" />
                    </asp:DropDownList>
                </div>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar"
                    CssClass="btn-filtrar" OnClick="btnFiltrar_Click" />
            </div>

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Historial de Planes por Comité</span>
                <div class="section-rule"></div>
            </div>

            <div class="plan-grid">
                <asp:Repeater ID="rptPlanes" runat="server" OnItemCommand="rptPlanes_ItemCommand">
                    <ItemTemplate>
                        <div class='plan-card'>
                            <div class='plan-card-header <%# Eval("estadoPlan").ToString() == "Aprobado" ? "aprobado" : Eval("estadoPlan").ToString() == "No Aprobado" ? "noaprobado" : "pendiente" %>'>
                                <div>
                                    <div class="plan-aprendiz"><%# Eval("nombreAprendiz") %></div>
                                    <div class="plan-doc"><%# Eval("docAprendiz") %></div>
                                </div>
                                <%# GetBadgeEstado(Eval("estadoPlan").ToString()) %>
                            </div>
                            <div class="plan-card-body">
                                <div class="plan-meta">
                                    <div class="meta-item">
                                        <span class="meta-label">Ficha</span>
                                        <span class="meta-val"><%# Eval("codigoFicha") %></span>
                                    </div>
                                    <div class="meta-item">
                                        <span class="meta-label">Asignado</span>
                                        <span class="meta-val"><%# Convert.ToDateTime(Eval("fechaAsignacion")).ToString("dd/MM/yyyy") %></span>
                                    </div>
                                    <div class="meta-item">
                                        <span class="meta-label">Fecha Límite</span>
                                        <span class="meta-val"><%# Convert.ToDateTime(Eval("fechaLimite")).ToString("dd/MM/yyyy") %></span>
                                    </div>
                                    <div class="meta-item">
                                        <span class="meta-label">Evidencias</span>
                                        <span class="meta-val"><%# Eval("totalEvidencias") %></span>
                                    </div>
                                </div>
                                <asp:Button ID="btnDetalle" runat="server"
                                    Text="Ver Detalle"
                                    CssClass="btn-detalle"
                                    CommandName="Detalle"
                                    CommandArgument='<%# Eval("idPlanMejoramiento") %>' />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <asp:Panel ID="pnlVacio" runat="server" Visible="false">
                <div class="empty-state">
                    <i class="bi bi-people"></i>
                    <p>No hay planes por comité registrados para tus fichas.</p>
                </div>
            </asp:Panel>

            <div id="panelDetalle" runat="server" clientidmode="Static">
                <div class="detail-header">
                    <div>
                        <div class="detail-title">
                            <i class="bi bi-people-fill" style="margin-right: 8px;"></i>
                            Plan Comité #<asp:Label ID="lblIdPlan" runat="server"></asp:Label>
                            —
                            <asp:Label ID="lblAprendizHeader" runat="server"></asp:Label>
                        </div>
                        <div class="detail-sub">
                            Estado:
                            <asp:Label ID="lblEstadoHeader" runat="server"></asp:Label>
                            &nbsp;|&nbsp; Ficha:
                            <asp:Label ID="lblFichaHeader" runat="server"></asp:Label>
                        </div>
                    </div>
                    <button type="button" class="btn-close-panel" onclick="cerrarDetalle()">
                        <i class="bi bi-x"></i>
                    </button>
                </div>

                <div class="detail-body">

                    <div class="info-grid">
                        <div class="info-item">
                            <label>Fecha Asignación</label>
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
                        <div class="info-item" style="grid-column: span 3;">
                            <label>Actividades</label>
                            <span>
                                <asp:Label ID="lblActividades" runat="server"></asp:Label></span>
                        </div>
                    </div>

                    <div class="section-head" style="margin-bottom: 12px;">
                        <div class="section-rule"></div>
                        <span class="section-label">Resultados de Aprendizaje</span>
                        <div class="section-rule"></div>
                    </div>
                    <div class="rap-list">
                        <asp:Repeater ID="rptResultados" runat="server">
                            <ItemTemplate>
                                <div class="rap-item">
                                    <i class="bi bi-bookmarks-fill"></i>
                                    <span><%# Eval("descripcion") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlSinResultados" runat="server" Visible="false">
                            <div class="rap-item" style="color: var(--text-muted);">
                                <i class="bi bi-info-circle"></i>
                                <span>Sin resultados de aprendizaje asociados.</span>
                            </div>
                        </asp:Panel>
                    </div>

                    <div class="section-head" style="margin-bottom: 12px;">
                        <div class="section-rule"></div>
                        <span class="section-label">Evidencias Entregadas</span>
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
                                    <a href='<%# ResolveUrl(Eval("rutaArchivo").ToString()) %>' target="_blank" class="ev-download">
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
                        <span class="section-label">Evaluación del Comité</span>
                        <div class="section-rule"></div>
                    </div>

                    <asp:Panel ID="pnlSinEvaluacion" runat="server" Visible="false">
                        <div class="rap-item" style="color: var(--text-muted);">
                            <i class="bi bi-hourglass-split"></i>
                            <span>Este plan aún no ha sido evaluado.</span>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlConEvaluacion" runat="server" Visible="false">
                        <div class="eval-grid">
                            <div id="divProducto" runat="server" class="eval-crit sineval">
                                <i class="bi bi-box-seam" style="font-size: 20px; display: block; margin-bottom: 6px;"></i>
                                Producto<br />
                                <asp:Label ID="lblProducto" runat="server"></asp:Label>
                            </div>
                            <div id="divConocimiento" runat="server" class="eval-crit sineval">
                                <i class="bi bi-lightbulb" style="font-size: 20px; display: block; margin-bottom: 6px;"></i>
                                Conocimiento<br />
                                <asp:Label ID="lblConocimiento" runat="server"></asp:Label>
                            </div>
                            <div id="divDesempeno" runat="server" class="eval-crit sineval">
                                <i class="bi bi-bar-chart-steps" style="font-size: 20px; display: block; margin-bottom: 6px;"></i>
                                Desempeño<br />
                                <asp:Label ID="lblDesempeno" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="section-head" style="margin-bottom: 10px;">
                            <div class="section-rule"></div>
                            <span class="section-label">Observaciones</span>
                            <div class="section-rule"></div>
                        </div>
                        <div class="obs-box">
                            <asp:Label ID="lblObsEvaluacion" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>

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
            if (hf && hf.value && hf.value !== '') {
                var panel = document.getElementById('panelDetalle');
                if (panel) {
                    panel.style.display = 'block';
                    panel.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            }
        };
    </script>
</body>
</html>
