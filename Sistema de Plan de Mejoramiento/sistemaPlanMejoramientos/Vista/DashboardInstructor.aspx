<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardInstructor.aspx.cs" Inherits="sistemaPlanMejoramientos.Instructor.DashboardInstructor" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Dashboard Instructor | SENA</title>

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
            font-family: 'DM Sans', sans-serif;
            background: #f4f7fb;
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
            background: var(--sena-dark);
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
                background: var(--sena-mid);
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
                background: var(--sena-accent);
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
            max-width: 500px;
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
                color: var(--sena-accent);
            }

        .hero-sub {
            font-size: 13px;
            color: rgba(255,255,255,.5);
            line-height: 1.6;
            max-width: 420px;
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
            background: var(--sena-accent);
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
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            min-height: 190px;
        }

            .mod-card::after {
                content: '';
                position: absolute;
                bottom: 0;
                left: 0;
                right: 0;
                height: 3px;
                background: var(--sena-dark);
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

            .mod-ico.crear {
                background: #EEEDFE;
                color: #534AB7;
            }

            .mod-ico.eval {
                background: #E1F5EE;
                color: #0F6E56;
            }

            .mod-ico.cons {
                background: #E6F1FB;
                color: #185FA5;
            }

            .mod-ico.hist {
                background: #FAECE7;
                color: #993C1D;
            }

            .mod-ico.com {
                background: #FFF4D6;
                color: #C98A00;
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
            color: var(--sena-dark);
            opacity: 0;
            transform: translateY(4px);
            transition: all .2s;
            text-decoration: none;
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
            transition: .25s;
        }

            .bot-card:hover {
                transform: translateY(-4px);
                box-shadow: 0 8px 24px rgba(0,0,0,.07);
            }

            .bot-card.accent {
                background: var(--sena-dark);
                border-color: var(--sena-dark);
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
            font-size: 34px;
            font-weight: 800;
            color: #042940;
            line-height: 1;
        }

        .bot-card.accent .metric-badge {
            color: var(--sena-accent);
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
            background: var(--sena-dark);
            color: #fff;
            border-color: var(--sena-dark);
        }

        .bot-card.accent .bot-link {
            border-color: rgba(255,255,255,.25);
            color: rgba(255,255,255,.8);
        }

        .bot-card.accent:hover .bot-link {
            background: var(--sena-accent);
            color: var(--sena-dark);
            border-color: var(--sena-accent);
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

        @media (max-width: 992px) {
            .main-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .bot-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 600px) {
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
    </style>
</head>

<body>

    <form id="form1" runat="server">

        <nav class="nav">
            <div class="nav-brand">
                <div class="nav-dot"></div>
                SENA Instructor
            </div>

            <div class="nav-right">
                <div>
                    <div class="nav-role">Área de Formación</div>
                    <asp:Label ID="lblInstructor" runat="server" CssClass="nav-name"></asp:Label>
                </div>

                <asp:Button ID="btnCerrarSesion" runat="server"
                    Text="Cerrar sesión"
                    CssClass="nav-btn"
                    OnClick="btnCerrarSesion_Click" />
            </div>
        </nav>

        <div class="hero">
            <div class="hero-inner">

                <div class="hero-text">
                    <div class="hero-badge">
                        <i class="bi bi-journal-check"></i>
                        Gestión pedagógica
                    </div>

                    <h1 class="hero-title">Evaluación &<br />
                        <span>Mejoramiento</span>
                    </h1>

                    <p class="hero-sub">
                        Gestiona planes de mejoramiento, revisa evidencias y realiza seguimiento académico a tus fichas asignadas.
                    </p>
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

            <div class="main-grid" style="grid-template-columns: repeat(2, 1fr);">

                <a href="FrmCrearPlan.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico crear">
                            <i class="bi bi-file-earmark-plus-fill"></i>
                        </div>
                        <div class="mod-title">Crear Plan</div>
                        <div class="mod-desc">
                            Registra planes de mejoramiento para aprendices con resultados pendientes.
                        </div>
                    </div>
                    <span class="mod-action">Gestionar <i class="bi bi-arrow-right"></i></span>
                </a>

                <a href="FrmEvaluarEvidencias.aspx" class="mod-card">
                    <div>
                        <div class="mod-ico eval">
                            <i class="bi bi-check2-square"></i>
                        </div>
                        <div class="mod-title">Evaluar Evidencias</div>
                        <div class="mod-desc">
                            Revisa archivos cargados y emite juicios cualitativos por evidencia.
                        </div>
                    </div>
                    <span class="mod-action">Evaluar <i class="bi bi-arrow-right"></i></span>
                </a>

            </div>

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Indicadores</span>
                <div class="section-rule"></div>
            </div>

            <div class="bot-grid">

                <div class="bot-card">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Planes Internos</div>
                            <div class="bot-sub">
                                Total de procesos preventivos registrados.
                            </div>
                        </div>

                        <span class="metric-badge">
                            <asp:Label ID="lblPlanesInternos" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>

                    <div class="mini-bars">
                        <div class="mini-bar" style="height: 35%"></div>
                        <div class="mini-bar" style="height: 60%"></div>
                        <div class="mini-bar hi" style="height: 85%"></div>
                        <div class="mini-bar" style="height: 55%"></div>
                        <div class="mini-bar" style="height: 70%"></div>
                    </div>

                    <a href="FrmHistoricoInternos.aspx" class="bot-link">Ver procesos <i class="bi bi-arrow-right"></i>
                    </a>
                </div>

                <div class="bot-card accent">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Planes Comité</div>
                            <div class="bot-sub">
                                Casos críticos pendientes de dictamen académico.
                            </div>
                        </div>

                        <span class="metric-badge">
                            <asp:Label ID="lblPlanesComite" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>

                    <a href="FrmHistoricoComite.aspx" class="bot-link">Revisar casos <i class="bi bi-arrow-right"></i>
                    </a>
                </div>

                <div class="bot-card">
                    <div class="bot-top">
                        <div>
                            <div class="bot-title">Fichas Asignadas</div>
                            <div class="bot-sub">
                                Total de grupos asociados actualmente al instructor.
                            </div>
                        </div>

                        <span class="metric-badge">
                            <asp:Label ID="lblTotalFichas" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>

                    <a href="FrmConsultarFichas.aspx" class="bot-link">Ver grupos <i class="bi bi-arrow-right"></i>
                    </a>
                </div>

            </div>

        </div>

    </form>

</body>
</html>
