<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardAprendiz.aspx.cs" Inherits="sistemaPlanMejoramientos.Aprendiz.DashboardAprendiz" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Dashboard Aprendiz | SENA</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
            --sena-dark: #042940;
            --sena-mid: #005C53;
            --sena-accent: #9FC131;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: 'DM Sans', 'Segoe UI', sans-serif;
            background: #f4f7fb;
            min-height: 100vh;
        }

        .nav {
            background: #042940;
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
            background: #9FC131;
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

        .nav-role {
            font-size: 10px;
            color: rgba(255,255,255,.45);
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .nav-name {
            font-size: 13px;
            color: #fff;
            font-weight: 500;
            display: block;
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
        }

            .nav-btn:hover {
                background: rgba(255,255,255,.1);
                color: #fff;
            }

        .hero {
            background: #042940;
            padding: 36px 28px 32px;
            position: relative;
            overflow: hidden;
        }

            .hero::before {
                content: '';
                position: absolute;
                top: -60px;
                right: -60px;
                width: 320px;
                height: 320px;
                background: #005C53;
                border-radius: 50%;
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
            font-weight: 500;
            padding: 4px 12px;
            border-radius: 20px;
            margin-bottom: 14px;
            letter-spacing: .5px;
            text-transform: uppercase;
        }

        .hero-title {
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 26px;
            color: #fff;
            line-height: 1.15;
            margin-bottom: 10px;
        }

            .hero-title span {
                color: #9FC131;
            }

        .hero-sub {
            font-size: 13px;
            color: rgba(255,255,255,.5);
            line-height: 1.6;
        }

        .estado-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: rgba(159,193,49,.12);
            border: 1px solid rgba(159,193,49,.3);
            color: #9FC131;
            font-size: 11px;
            font-weight: 600;
            padding: 4px 12px;
            border-radius: 20px;
            margin-top: 14px;
        }

            .estado-badge.cancelado {
                background: rgba(220,53,69,.12);
                border-color: rgba(220,53,69,.3);
                color: #dc3545;
            }

            .estado-badge.aplazado {
                background: rgba(255,193,7,.12);
                border-color: rgba(255,193,7,.3);
                color: #ffc107;
            }

        .dash-body {
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
            font-size: 13px;
            font-weight: 700;
            color: #042940;
            text-transform: uppercase;
            letter-spacing: 1.5px;
            white-space: nowrap;
        }

        .section-rule {
            flex: 1;
            height: 1px;
            background: #dbe3ea;
        }

        .mod-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 14px;
            margin-bottom: 28px;
        }

        .mod-card {
            background: #fff;
            border: 1px solid #e0e6ed;
            border-radius: 16px;
            padding: 22px 18px 18px;
            cursor: pointer;
            transition: all .25s;
            position: relative;
            overflow: hidden;
            text-decoration: none;
            display: block;
        }

            .mod-card::after {
                content: '';
                position: absolute;
                bottom: 0;
                left: 0;
                right: 0;
                height: 3px;
                background: #005C53;
                transform: scaleX(0);
                transition: transform .25s;
                transform-origin: left;
            }

            .mod-card:hover {
                transform: translateY(-4px);
                border-color: #c5d0db;
                box-shadow: 0 8px 24px rgba(0,0,0,.07);
            }

                .mod-card:hover::after {
                    transform: scaleX(1);
                }

                .mod-card:hover .mod-action {
                    opacity: 1;
                    transform: translateY(0);
                }

        .mod-ico {
            width: 46px;
            height: 46px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 22px;
            margin-bottom: 14px;
        }

            .mod-ico.planes {
                background: #E6F1FB;
                color: #185FA5;
            }

            .mod-ico.evidencia {
                background: #E1F5EE;
                color: #0F6E56;
            }

            .mod-ico.perfil {
                background: #EEEDFE;
                color: #534AB7;
            }

        .mod-title {
            font-family: 'Syne', sans-serif;
            font-size: 14px;
            font-weight: 700;
            color: #042940;
            margin-bottom: 6px;
        }

        .mod-desc {
            font-size: 12px;
            color: #6c757d;
            line-height: 1.5;
            margin-bottom: 14px;
        }

        .mod-action {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-size: 12px;
            font-weight: 500;
            color: #042940;
            opacity: 0;
            transform: translateY(4px);
            transition: all .2s;
        }

        .bot-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 14px;
        }

        .bot-card {
            background: #fff;
            border: 1px solid #e0e6ed;
            border-radius: 16px;
            padding: 22px 20px 18px;
        }

            .bot-card.accent {
                background: #042940;
                border-color: #042940;
            }

        .bot-top {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            margin-bottom: 8px;
        }

        .bot-title {
            font-family: 'Syne', sans-serif;
            font-size: 15px;
            font-weight: 700;
            color: #042940;
        }

        .bot-card.accent .bot-title {
            color: #fff;
        }

        .bot-sub {
            font-size: 12px;
            color: #6c757d;
            margin-top: 4px;
            line-height: 1.5;
        }

        .bot-card.accent .bot-sub {
            color: rgba(255,255,255,.5);
        }

        .metric-badge {
            font-family: 'Syne', sans-serif;
            font-size: 32px;
            font-weight: 800;
            color: #042940;
            line-height: 1;
            margin-top: 10px;
            display: block;
        }

        .bot-card.accent .metric-badge {
            color: #9FC131;
        }

        .bot-link {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-size: 12px;
            font-weight: 600;
            color: #042940;
            border: 1px solid rgba(4,41,64,.2);
            padding: 6px 14px;
            border-radius: 20px;
            margin-top: 16px;
            transition: all .2s;
            text-decoration: none;
        }

        .bot-card:hover .bot-link {
            background: #042940;
            color: #fff;
            border-color: #042940;
        }

        .bot-card.accent .bot-link {
            border-color: rgba(255,255,255,.25);
            color: rgba(255,255,255,.8);
        }

        .bot-card.accent:hover .bot-link {
            background: #9FC131;
            color: #042940;
            border-color: #9FC131;
        }

        @media (max-width: 992px) {
            .mod-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .bot-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 600px) {
            .mod-grid {
                grid-template-columns: 1fr;
            }

            .bot-grid {
                grid-template-columns: 1fr;
            }

            .dash-body {
                padding: 16px;
            }
        }

        @media (max-width: 1100px) {

            .mod-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .bot-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 768px) {

            body {
                overflow-x: hidden;
            }

            .nav {
                flex-direction: column;
                align-items: flex-start;
                justify-content: center;
                height: auto;
                padding: 14px 18px;
                gap: 12px;
            }

            .nav-right {
                width: 100%;
                justify-content: space-between;
            }

            .nav-name {
                font-size: 12px;
            }

            .hero {
                padding: 28px 20px;
            }

            .hero-title {
                font-size: 22px;
            }

            .hero-sub {
                font-size: 12px;
                max-width: 100%;
            }

            .hero-badge,
            .estado-badge {
                font-size: 10px;
            }

            .dash-body {
                padding: 18px;
            }

            .mod-grid {
                grid-template-columns: 1fr;
            }

            .bot-grid {
                grid-template-columns: 1fr;
            }

            .mod-card,
            .bot-card {
                padding: 18px;
            }

            .metric-badge {
                font-size: 26px;
            }

            .section-label {
                font-size: 11px;
            }
        }

        @media (max-width: 480px) {

            .hero-title {
                font-size: 20px;
                line-height: 1.2;
            }

            .hero-sub {
                font-size: 11px;
            }

            .nav-brand {
                font-size: 14px;
            }

            .nav-btn {
                width: 100%;
                text-align: center;
                font-size: 11px;
            }

            .mod-title,
            .bot-title {
                font-size: 13px;
            }

            .mod-desc,
            .bot-sub {
                font-size: 11px;
            }

            .mod-ico {
                width: 42px;
                height: 42px;
                font-size: 18px;
            }

            .metric-badge {
                font-size: 22px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="nav">
            <div class="nav-brand">
                <div class="nav-dot"></div>
                SENA Aprendiz
       
            </div>
            <div class="nav-right">
                <div>
                    <div class="nav-role">Proceso Formativo</div>
                    <asp:Label ID="lblAprendiz" runat="server" CssClass="nav-name"></asp:Label>
                </div>
                <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar sesión"
                    CssClass="nav-btn" OnClick="btnCerrarSesion_Click" />
            </div>
        </nav>

        <div class="hero">
            <div class="hero-inner">
                <div class="hero-badge">
                    <i class="bi bi-mortarboard-fill"></i>Portal del Aprendiz
           
                </div>
                <h1 class="hero-title">Mis Planes &<br>
                    <span>Evidencias</span></h1>
                <p class="hero-sub">Consulta los planes de mejoramiento asignados por tu instructor y sube tus evidencias para ser evaluado.</p>
                <div>
                    <asp:Label ID="lblEstadoBadge" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <div class="dash-body">

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Accesos Rápidos</span>
                <div class="section-rule"></div>
            </div>

            <div class="mod-grid">
                <a href="FrmMisPlanes.aspx" class="mod-card">
                    <div class="mod-ico planes"><i class="bi bi-journal-text"></i></div>
                    <div class="mod-title">Mis Planes</div>
                    <div class="mod-desc">Consulta todos los planes de mejoramiento que te han sido asignados y su estado actual.</div>
                    <span class="mod-action">Ver Planes <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="FrmSubirEvidencia.aspx" class="mod-card">
                    <div class="mod-ico evidencia"><i class="bi bi-cloud-upload-fill"></i></div>
                    <div class="mod-title">Subir Evidencia</div>
                    <div class="mod-desc">Adjunta tus archivos de evidencia en PDF, Word, imágenes o ZIP para un plan pendiente.</div>
                    <span class="mod-action">Subir Ahora <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="FrmMiPerfil.aspx" class="mod-card">
                    <div class="mod-ico perfil"><i class="bi bi-person-badge-fill"></i></div>
                    <div class="mod-title">Mi Perfil</div>
                    <div class="mod-desc">Consulta tu información personal, ficha asignada y estado académico actual.</div>
                    <span class="mod-action">Ver Perfil <i class="bi bi-arrow-right"></i></span>
                </a>
            </div>

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Resumen Académico</span>
                <div class="section-rule"></div>
            </div>

            <div class="bot-grid">
                <div class="bot-card">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Planes Pendientes</div>
                            <div class="bot-sub">Planes activos que aún requieren que entregues evidencias.</div>
                        </div>
                        <span class="metric-badge">
                            <asp:Label ID="lblPlanesPendientes" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <a href="FrmMisPlanes.aspx?filtro=Pendiente" class="bot-link">Ver pendientes <i class="bi bi-arrow-right"></i></a>
                </div>

                <div class="bot-card accent">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Planes Comité</div>
                            <div class="bot-sub">Casos en segunda instancia. Requieren atención urgente.</div>
                        </div>
                        <span class="metric-badge">
                            <asp:Label ID="lblPlanesComite" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <a href="FrmMisPlanes.aspx?filtro=Comite" class="bot-link">Revisar <i class="bi bi-arrow-right"></i></a>
                </div>

                <div class="bot-card">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Planes Aprobados</div>
                            <div class="bot-sub">Planes que superaste satisfactoriamente.</div>
                        </div>
                        <span class="metric-badge">
                            <asp:Label ID="lblPlanesAprobados" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <a href="FrmMisPlanes.aspx?filtro=Aprobado" class="bot-link">Ver historial <i class="bi bi-arrow-right"></i></a>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
