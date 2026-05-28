<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMiPerfil.aspx.cs" Inherits="sistemaPlanMejoramientos.Aprendiz.FrmMiPerfil" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mi Perfil | SENA Aprendiz</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
            --sena-dark: #042940;
            --sena-mid: #005C53;
            --sena-accent: #9FC131;
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
            max-width: 860px;
            margin: 0 auto;
        }

        .profile-card {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 18px;
            overflow: hidden;
            margin-bottom: 20px;
        }

        .profile-header {
            background: linear-gradient(135deg, var(--sena-dark) 0%, var(--sena-mid) 100%);
            padding: 28px 28px 24px;
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .avatar {
            width: 72px;
            height: 72px;
            border-radius: 50%;
            background: rgba(159,193,49,.2);
            border: 2px solid rgba(159,193,49,.4);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 32px;
            color: var(--sena-accent);
            flex-shrink: 0;
        }

        .profile-name {
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 20px;
            color: #fff;
            margin-bottom: 4px;
        }

        .profile-sub {
            font-size: 12px;
            color: rgba(255,255,255,.5);
            margin-bottom: 10px;
        }

        .estado-pill {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            background: rgba(159,193,49,.15);
            border: 1px solid rgba(159,193,49,.35);
            color: var(--sena-accent);
            font-size: 11px;
            font-weight: 600;
            padding: 3px 12px;
            border-radius: 20px;
        }

            .estado-pill.cancelado {
                background: rgba(220,53,69,.15);
                border-color: rgba(220,53,69,.35);
                color: #f87171;
            }

            .estado-pill.aplazado {
                background: rgba(255,193,7,.15);
                border-color: rgba(255,193,7,.35);
                color: #ffc107;
            }

        .section-title {
            font-family: 'Syne', sans-serif;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1.5px;
            color: var(--text-muted);
            padding: 18px 24px 12px;
            border-bottom: 1px solid var(--border);
        }

        .info-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 0;
        }

        .info-item {
            padding: 16px 24px;
            border-bottom: 1px solid var(--border);
            border-right: 1px solid var(--border);
        }

            .info-item:nth-child(even) {
                border-right: none;
            }

            .info-item:nth-last-child(-n+2) {
                border-bottom: none;
            }

        .info-label {
            font-size: 10px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: var(--text-muted);
            margin-bottom: 5px;
        }

        .info-val {
            font-size: 14px;
            font-weight: 500;
            color: var(--sena-dark);
        }

        .ficha-card {
            background: var(--card);
            border: 1px solid var(--border);
            border-radius: 18px;
            overflow: hidden;
        }

        .ficha-header {
            background: var(--sena-dark);
            padding: 16px 24px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .ficha-header-title {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 14px;
            color: #fff;
        }

        .ficha-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 0;
        }

        .ficha-item {
            padding: 16px 24px;
            border-right: 1px solid var(--border);
        }

            .ficha-item:last-child {
                border-right: none;
            }

        @media (max-width: 640px) {
            .info-grid {
                grid-template-columns: 1fr;
            }

            .info-item {
                border-right: none;
            }

                .info-item:nth-last-child(-n+2) {
                    border-bottom: 1px solid var(--border);
                }

                .info-item:last-child {
                    border-bottom: none;
                }

            .ficha-grid {
                grid-template-columns: 1fr;
            }

            .ficha-item {
                border-right: none;
                border-bottom: 1px solid var(--border);
            }

                .ficha-item:last-child {
                    border-bottom: none;
                }

            .page-body {
                padding: 16px;
            }
        }

        .nav {
            height: 60px;
            padding: 0 28px;
        }

        .hero {
            padding: 32px 28px 28px;
        }

            .hero::before {
                width: 280px;
                height: 280px;
                top: -60px;
                right: -60px;
            }

        .page-body {
            max-width: 860px;
            padding: 28px;
        }

        .profile-card,
        .ficha-card {
            border-radius: 18px;
        }

        .profile-header {
            padding: 28px 28px 24px;
            gap: 20px;
        }

        .avatar {
            width: 72px;
            height: 72px;
            font-size: 32px;
        }

        .info-grid {
            grid-template-columns: repeat(2, 1fr);
        }

        .info-item {
            padding: 16px 24px;
        }

        .ficha-grid {
            grid-template-columns: repeat(3, 1fr);
        }

        .ficha-item {
            padding: 16px 24px;
        }

        .hero-title {
            font-size: 24px;
        }

        .profile-name {
            font-size: 20px;
        }

        .info-val {
            font-size: 14px;
        }

        @media (max-width: 640px) {
            .page-body {
                padding: 16px;
            }

            .avatar {
                width: 60px;
                height: 60px;
                font-size: 26px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="nav">
            <a href="DashboardAprendiz.aspx" class="nav-brand">
                <div class="nav-dot"></div>
                SENA Aprendiz
            </a>
            <a href="DashboardAprendiz.aspx" class="nav-btn">
                <i class="bi bi-arrow-left"></i>Volver al Dashboard
            </a>
        </nav>

        <div class="hero">
            <div class="hero-inner">
                <div class="hero-badge">
                    <i class="bi bi-person-badge-fill"></i>Información Personal
                </div>
                <h1 class="hero-title">Mi <span>Perfil</span></h1>
                <p class="hero-sub">Consulta tu información personal, ficha asignada y estado académico actual.</p>
            </div>
        </div>

        <div class="page-body">

            <div class="profile-card">
                <div class="profile-header">
                    <div class="avatar">
                        <i class="bi bi-person-fill"></i>
                    </div>
                    <div>
                        <div class="profile-name">
                            <asp:Label ID="lblNombre" runat="server"></asp:Label>
                        </div>
                        <div class="profile-sub">Aprendiz SENA</div>
                        <asp:Label ID="lblEstado" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="section-title">
                    <i class="bi bi-person-vcard" style="margin-right: 6px;"></i>Datos Personales
                </div>

                <div class="info-grid">
                    <div class="info-item">
                        <div class="info-label">Tipo de Documento</div>
                        <div class="info-val">
                            <asp:Label ID="lblTipoDoc" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Número de Documento</div>
                        <div class="info-val">
                            <asp:Label ID="lblDocumento" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Correo Electrónico</div>
                        <div class="info-val">
                            <asp:Label ID="lblCorreo" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Teléfono</div>
                        <div class="info-val">
                            <asp:Label ID="lblTelefono" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="ficha-card">
                <div class="ficha-header">
                    <i class="bi bi-journal-bookmark-fill" style="color: var(--sena-accent); font-size: 16px;"></i>
                    <div class="ficha-header-title">Información Académica</div>
                </div>
                <div class="ficha-grid">
                    <div class="ficha-item">
                        <div class="info-label">Ficha</div>
                        <div class="info-val">
                            <asp:Label ID="lblFicha" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="ficha-item">
                        <div class="info-label">Programa</div>
                        <div class="info-val">
                            <asp:Label ID="lblPrograma" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="ficha-item">
                        <div class="info-label">Jornada</div>
                        <div class="info-val">
                            <asp:Label ID="lblJornada" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
