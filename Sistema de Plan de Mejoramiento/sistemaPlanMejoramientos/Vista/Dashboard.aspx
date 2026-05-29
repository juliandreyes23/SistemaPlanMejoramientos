<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="sistemaPlanMejoramientos.Admin.Dashboard" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Dashboard Administrativo | SENA</title>
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

            .hero::after {
                content: '';
                position: absolute;
                bottom: 20px;
                right: 80px;
                width: 160px;
                height: 160px;
                background: #9FC131;
                border-radius: 50%;
                opacity: .08;
            }

        .hero-inner {
            position: relative;
            z-index: 1;
            display: flex;
            align-items: flex-end;
            justify-content: space-between;
        }

        .hero-text {
            max-width: 380px;
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

        .hero-visual {
            display: flex;
            gap: 8px;
            align-items: flex-end;
            opacity: .7;
        }

        .hero-bar {
            width: 18px;
            border-radius: 4px 4px 0 0;
            background: #9FC131;
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

        .main-grid {
            display: grid;
            grid-template-columns: repeat(5, 1fr);
            gap: 14px;
            margin-bottom: 28px;
            align-items: stretch;
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
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            min-height: 180px;
        }

            .mod-card::after {
                content: '';
                position: absolute;
                bottom: 0;
                left: 0;
                right: 0;
                height: 3px;
                background: #042940;
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
            width: 44px;
            height: 44px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
            margin-bottom: 12px;
            flex-shrink: 0;
        }

            .mod-ico.ct {
                background: #FFF3E0;
                color: #E65100;
            }

            .mod-ico.u {
                background: #042940;
                color: #9FC131;
            }

            .mod-ico.p {
                background: #E1F5EE;
                color: #0F6E56;
            }

            .mod-ico.f {
                background: #FAEEDA;
                color: #854F0B;
            }

            .mod-ico.ap {
                background: #EEEDFE;
                color: #534AB7;
            }

        .mod-body {
            flex: 1;
        }

        .mod-title {
            font-family: 'Syne', sans-serif;
            font-size: 13px;
            font-weight: 700;
            color: #042940;
            margin-bottom: 5px;
        }

        .mod-desc {
            font-size: 11px;
            color: #6c757d;
            line-height: 1.5;
        }

        .mod-action {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            font-size: 11px;
            font-weight: 600;
            color: #042940;
            opacity: 0;
            transform: translateY(4px);
            transition: all .2s;
            text-decoration: none;
            margin-top: 12px;
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
            cursor: pointer;
            transition: all .25s;
        }

            .bot-card:hover {
                transform: translateY(-4px);
                border-color: #c5d0db;
                box-shadow: 0 8px 24px rgba(0,0,0,.07);
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
            font-family: 'Syne',sans-serif;
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

        .mini-bars {
            display: flex;
            gap: 4px;
            align-items: flex-end;
            height: 40px;
            margin-top: 12px;
        }

        .mini-bar {
            flex: 1;
            border-radius: 3px 3px 0 0;
            background: rgba(159,193,49,.3);
        }

            .mini-bar.hi {
                background: #9FC131;
            }

        .mod-ico.in {
            background: #FAECE7;
            color: #993C1D;
        }

        .mod-ico.as {
            background: #042940;
            color: #9FC131;
        }

        .mod-ico.ex {
            background: #E6F1FB;
            color: #185FA5;
        }

        @media (max-width: 1100px) {
            .main-grid {
                grid-template-columns: repeat(3, 1fr);
            }
        }

        @media (max-width: 768px) {
            .main-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .bot-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 500px) {
            .hero-visual {
                display: none;
            }

            .main-grid {
                grid-template-columns: 1fr;
            }

            .bot-grid {
                grid-template-columns: 1fr;
            }

            .dash-body {
                padding: 16px;
            }
        }
        /* =========================
   RESPONSIVE DASHBOARD
========================= */

        @media (max-width: 1200px) {

            .main-grid {
                grid-template-columns: repeat(3, 1fr);
            }
        }

        @media (max-width: 992px) {

            .hero-inner {
                flex-direction: column;
                align-items: flex-start;
                gap: 25px;
            }

            .hero-text {
                max-width: 100%;
            }

            .main-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .bot-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .nav {
                padding: 0 18px;
            }

            .dash-body {
                padding: 20px;
            }
        }

        @media (max-width: 768px) {

            .nav {
                flex-direction: column;
                height: auto;
                padding: 14px;
                gap: 12px;
                align-items: flex-start;
            }

            .nav-right {
                width: 100%;
                justify-content: space-between;
            }

            .hero {
                padding: 28px 20px;
            }

            .hero-title {
                font-size: 22px;
            }

            .hero-sub {
                font-size: 12px;
            }

            .hero-visual {
                display: none;
            }

            .main-grid {
                grid-template-columns: 1fr;
            }

            .bot-grid {
                grid-template-columns: 1fr;
            }

            .mod-card,
            .bot-card {
                min-height: auto;
            }

            .dash-body {
                padding: 16px;
            }
        }

        @media (max-width: 480px) {

            .hero-title {
                font-size: 20px;
            }

            .hero-badge {
                font-size: 10px;
            }

            .section-label {
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

            .nav-btn {
                width: 100%;
                text-align: center;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="nav">
            <div class="nav-brand">
                <div class="nav-dot"></div>
                SENA Admin
       
            </div>
            <div class="nav-right">
                <div>
                    <div class="nav-role">Administrador</div>
                    <asp:Label ID="lblUsuario" runat="server" CssClass="nav-name"></asp:Label>
                </div>
                <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar sesión"
                    CssClass="nav-btn" OnClick="btnCerrarSesion_Click" />
            </div>
        </nav>

        <div class="hero">
            <div class="hero-inner">
                <div class="hero-text">
                    <div class="hero-badge">
                        <i class="bi bi-layout-sidebar"></i>Panel principal
               
                    </div>
                    <h1 class="hero-title">Planes de<br>
                        <span>Mejoramiento</span></h1>
                    <p class="hero-sub">Administra centros, programas, fichas, instructores y aprendices desde un solo lugar.</p>
                </div>
                <div class="hero-visual">
                    <div class="hero-bar" style="height: 28px"></div>
                    <div class="hero-bar" style="height: 44px"></div>
                    <div class="hero-bar" style="height: 36px"></div>
                    <div class="hero-bar" style="height: 56px"></div>
                    <div class="hero-bar" style="height: 48px"></div>
                    <div class="hero-bar" style="height: 64px"></div>
                    <div class="hero-bar" style="height: 40px"></div>
                </div>
            </div>
        </div>

        <div class="dash-body">

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Módulos principales</span>
                <div class="section-rule"></div>
            </div>

            <%-- 5 cards en un solo grid alineado --%>
            <div class="main-grid">

                <a href="GestionCentros.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico ct"><i class="bi bi-building"></i></div>
                        <div class="mod-body">
                            <div class="mod-title">Centros</div>
                            <div class="mod-desc">Registro y administración de centros de formación.</div>
                        </div>
                    </div>
                    <span class="mod-action">Gestionar <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="FrmUsuarios.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico u"><i class="bi bi-person-gear"></i></div>
                        <div class="mod-body">
                            <div class="mod-title">Usuarios</div>
                            <div class="mod-desc">Credenciales y accesos al sistema.</div>
                        </div>
                    </div>
                    <span class="mod-action">Gestionar <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="GestionProgramas.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico p"><i class="bi bi-journal-bookmark-fill"></i></div>
                        <div class="mod-body">
                            <div class="mod-title">Programas</div>
                            <div class="mod-desc">Tecnologías, técnicos y cursos.</div>
                        </div>
                    </div>
                    <span class="mod-action">Gestionar <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="GestionFichas.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico f"><i class="bi bi-card-list"></i></div>
                        <div class="mod-body">
                            <div class="mod-title">Fichas</div>
                            <div class="mod-desc">Jornadas y fechas de formación.</div>
                        </div>
                    </div>
                    <span class="mod-action">Gestionar <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="GestionAprendices.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico ap"><i class="bi bi-people-fill"></i></div>
                        <div class="mod-body">
                            <div class="mod-title">Aprendices</div>
                            <div class="mod-desc">Registro y estado académico.</div>
                        </div>
                    </div>
                    <span class="mod-action">Gestionar <i class="bi bi-arrow-right"></i></span>
                </a>

            </div>

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Herramientas</span>
                <div class="section-rule"></div>
            </div>

            <div class="bot-grid">

                <div class="bot-card">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Instructores</div>
                            <div class="bot-sub">Gestión y asignación de instructores especializados por área.</div>
                        </div>
                        <div class="mod-ico in"><i class="bi bi-person-video3"></i></div>
                    </div>
                    <div class="mini-bars">
                        <div class="mini-bar" style="height: 30%"></div>
                        <div class="mini-bar" style="height: 60%"></div>
                        <div class="mini-bar" style="height: 45%"></div>
                        <div class="mini-bar hi" style="height: 85%"></div>
                        <div class="mini-bar" style="height: 70%"></div>
                        <div class="mini-bar" style="height: 55%"></div>
                    </div>
                    <a href="GestionInstructores.aspx" class="bot-link">Ver instructores <i class="bi bi-arrow-right"></i></a>
                </div>

                <div class="bot-card accent">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Asignaciones</div>
                            <div class="bot-sub">Asigna instructores a fichas según especialidad y disponibilidad.</div>
                        </div>
                        <div class="mod-ico as"><i class="bi bi-diagram-3-fill"></i></div>
                    </div>
                    <a href="AsignacionInstructores.aspx" class="bot-link">Realizar asignación <i class="bi bi-arrow-right"></i></a>
                </div>

                <div class="bot-card">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Carga masiva</div>
                            <div class="bot-sub">Importa aprendices desde archivos Excel de forma rápida.</div>
                        </div>
                        <div class="mod-ico ex"><i class="bi bi-file-earmark-excel-fill"></i></div>
                    </div>
                    <a href="CargaMasivaAprendices.aspx" class="bot-link">Procesar Excel <i class="bi bi-arrow-right"></i></a>
                </div>

            </div>

        </div>

    </form>
</body>
</html>
