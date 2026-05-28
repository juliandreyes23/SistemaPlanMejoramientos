<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConsultarFichas.aspx.cs"
    Inherits="sistemaPlanMejoramientos.Instructor.FrmConsultarFichas" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mis Fichas & Aprendices | SENA</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />

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

        .nav-btn {
            border: 1px solid rgba(255,255,255,.25);
            background: transparent;
            color: rgba(255,255,255,.85);
            padding: 7px 16px;
            border-radius: 20px;
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
            max-width: 650px;
        }

        .body-wrap {
            padding: 28px;
        }

        .search-box {
            position: relative;
            margin-bottom: 24px;
        }

            .search-box i {
                position: absolute;
                left: 14px;
                top: 50%;
                transform: translateY(-50%);
                color: #adb5bd;
                font-size: 15px;
            }

            .search-box input {
                width: 100%;
                border: 1px solid #dbe3ea;
                border-radius: 14px;
                background: #fff;
                padding: 13px 16px 13px 42px;
                font-size: 13px;
                color: #042940;
                outline: none;
                transition: .2s;
            }

                .search-box input:focus {
                    border-color: #9FC131;
                    box-shadow: 0 0 0 4px rgba(159,193,49,.1);
                }

        .section-head {
            display: flex;
            align-items: center;
            gap: 14px;
            margin-bottom: 18px;
        }

        .section-rule {
            flex: 1;
            height: 1px;
            background: #dbe3ea;
        }

        .section-label {
            font-family: 'Syne', sans-serif;
            font-size: 13px;
            font-weight: 700;
            color: #042940;
            letter-spacing: 1px;
            text-transform: uppercase;
            white-space: nowrap;
        }

        .ficha-card {
            background: #fff;
            border: 1px solid #e0e6ed;
            border-radius: 18px;
            margin-bottom: 18px;
            overflow: hidden;
            transition: .25s;
        }

            .ficha-card:hover {
                box-shadow: 0 10px 28px rgba(0,0,0,.06);
                border-color: #cfd9e2;
            }

        .ficha-header {
            padding: 20px 22px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            cursor: pointer;
            transition: .2s;
        }

            .ficha-header:hover {
                background: #f8fafc;
            }

        .ficha-left {
            display: flex;
            align-items: center;
            gap: 14px;
        }

        .ficha-icon {
            width: 48px;
            height: 48px;
            border-radius: 14px;
            background: #E1F5EE;
            color: #0F6E56;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 22px;
            flex-shrink: 0;
        }

        .ficha-codigo {
            font-family: 'Syne', sans-serif;
            font-size: 15px;
            font-weight: 700;
            color: #042940;
        }

        .ficha-programa {
            font-size: 12px;
            color: #6c757d;
            margin-top: 4px;
        }

        .ficha-right {
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }

        .pill {
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
            border: 1px solid transparent;
        }

        .pill-jornada {
            background: #E6F1FB;
            color: #185FA5;
            border-color: #c6ddf5;
        }

        .pill-activa {
            background: #E1F5EE;
            color: #0F6E56;
            border-color: #b7e3d0;
        }

        .pill-finalizada {
            background: #f0f0f0;
            color: #6c757d;
            border-color: #ddd;
        }

        .pill-count {
            background: #042940;
            color: #9FC131;
            border-color: #042940;
        }

        .chevron {
            color: #9FC131;
            font-size: 18px;
            transition: .3s;
        }

            .chevron.open {
                transform: rotate(180deg);
            }

        .aprendices-wrap {
            display: none;
            padding: 0 22px 18px;
            border-top: 1px solid #edf2f6;
        }

            .aprendices-wrap.open {
                display: block;
            }

        .aprendiz-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 14px 0;
            border-bottom: 1px solid #f0f4f7;
        }

            .aprendiz-row:last-child {
                border-bottom: none;
            }

        .aprendiz-left {
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .aprendiz-avatar {
            width: 38px;
            height: 38px;
            border-radius: 50%;
            background: #042940;
            color: #9FC131;
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: 'Syne', sans-serif;
            font-size: 13px;
            font-weight: 700;
            flex-shrink: 0;
        }

        .aprendiz-name {
            font-size: 13px;
            font-weight: 600;
            color: #042940;
        }

        .aprendiz-doc {
            font-size: 11px;
            color: #6c757d;
            margin-top: 2px;
        }

        .estado {
            font-size: 11px;
            font-weight: 600;
            padding: 5px 12px;
            border-radius: 20px;
        }

        .estado-formacion {
            background: #E1F5EE;
            color: #0F6E56;
        }

        .estado-condicionado {
            background: #fff3cd;
            color: #856404;
        }

        .estado-cancelado {
            background: #fde8e8;
            color: #c0392b;
        }

        .estado-aplazado {
            background: #E6F1FB;
            color: #185FA5;
        }

        .estado-default {
            background: #f0f0f0;
            color: #6c757d;
        }

        .empty-state {
            background: #fff;
            border: 1px solid #e0e6ed;
            border-radius: 18px;
            padding: 70px 20px;
            text-align: center;
            color: #adb5bd;
        }

            .empty-state i {
                font-size: 56px;
                margin-bottom: 14px;
                display: block;
            }

            .empty-state h3 {
                font-family: 'Syne', sans-serif;
                font-size: 18px;
                color: #042940;
                margin-bottom: 6px;
            }

            .empty-state p {
                font-size: 13px;
            }

        @media (max-width: 768px) {

            .body-wrap {
                padding: 18px;
            }

            .ficha-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 16px;
            }

            .ficha-right {
                width: 100%;
            }

            .aprendiz-row {
                flex-direction: column;
                align-items: flex-start;
                gap: 10px;
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

            <a href="DashboardInstructor.aspx" class="nav-btn">
                <i class="bi bi-arrow-left"></i>
                Volver
            </a>
        </nav>

        <div class="hero">
            <div class="hero-inner">

                <div class="hero-badge">
                    <i class="bi bi-folder2-open"></i>
                    Gestión Académica
                </div>

                <h1 class="hero-title">Mis Fichas &
                    <br />
                    <span>Aprendices</span>
                </h1>

                <p class="hero-sub">
                    Consulta los grupos asignados, revisa aprendices vinculados y verifica rápidamente su estado académico actual.
                </p>

            </div>
        </div>

        <div class="body-wrap">

            <div class="search-box">
                <i class="bi bi-search"></i>

                <asp:TextBox
                    ID="txtBuscar"
                    runat="server"
                    placeholder="Buscar por código de ficha o programa..."
                    AutoPostBack="true"
                    OnTextChanged="txtBuscar_TextChanged">
                </asp:TextBox>
            </div>

            <div class="section-head">
                <div class="section-rule"></div>
                <span class="section-label">Fichas Asignadas</span>
                <div class="section-rule"></div>
            </div>

            <asp:Panel ID="pnlFichas" runat="server"></asp:Panel>

            <asp:Panel ID="pnlVacio" runat="server" Visible="false">

                <div class="empty-state">
                    <i class="bi bi-folder-x"></i>

                    <h3>Sin fichas registradas</h3>

                    <p>
                        Actualmente no tienes fichas o grupos asociados a tu cuenta.
                    </p>
                </div>

            </asp:Panel>

        </div>

    </form>

    <script>
        function toggleFicha(id) {

            var wrap = document.getElementById('aprendices_' + id);
            var chev = document.getElementById('chev_' + id);

            if (wrap.classList.contains('open')) {

                wrap.classList.remove('open');
                chev.classList.remove('open');

            } else {

                wrap.classList.add('open');
                chev.classList.add('open');
            }
        }
    </script>

</body>
</html>
