<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRecuperar.aspx.cs" Inherits="sistemaPlanMejoramientos.FrmRecuperar" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Recuperar Contraseña - Plan Mejoramientos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap" rel="stylesheet" />
    <link href="Css/frmRecuperar.css" rel="stylesheet" />
</head>
<body>
    <style>
        :root {
            --dark: #042940;
            --teal: #005C53;
            --green: #9FC131;
            --lime: #DBF227;
            --white: #ffffff;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            background: linear-gradient(135deg, #042940 0%, #005C53 100%);
            min-height: 100vh;
            font-family: 'DM Sans', 'Segoe UI', sans-serif;
            overflow-x: hidden;
        }

        .bg-shapes {
            position: fixed;
            inset: 0;
            pointer-events: none;
            z-index: 0;
        }

        .shape {
            position: absolute;
            border-radius: 50%;
            filter: blur(90px);
            opacity: 0.15;
        }

        .s1 {
            width: 500px;
            height: 500px;
            background: var(--green);
            top: -200px;
            right: -150px;
        }

        .s2 {
            width: 350px;
            height: 350px;
            background: var(--lime);
            bottom: -100px;
            left: -80px;
        }

        .navbar-top {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            z-index: 100;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 1rem 2rem;
            background: rgba(4, 41, 64, 0.6);
            backdrop-filter: blur(12px);
            border-bottom: 1px solid rgba(255,255,255,0.06);
        }

        .back-link {
            color: rgba(255,255,255,0.6);
            text-decoration: none;
            font-size: 0.88rem;
            transition: color 0.2s;
        }

            .back-link:hover {
                color: var(--white);
            }

        .nav-brand {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 1rem;
            color: var(--white);
        }

            .nav-brand span {
                color: var(--green);
            }

        .page-wrapper {
            position: relative;
            z-index: 1;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 5rem 1rem 2rem;
        }

        .auth-card {
            background: rgba(255,255,255,0.08);
            backdrop-filter: blur(20px);
            -webkit-backdrop-filter: blur(20px);
            border: 1px solid rgba(255,255,255,0.13);
            border-radius: 20px;
            padding: 2.5rem 2.2rem;
            width: 100%;
            max-width: 420px;
            box-shadow: 0 24px 64px rgba(0,0,0,0.35);
            animation: slideUp 0.5s ease both;
        }

        .card-header-custom {
            text-align: center;
            margin-bottom: 2rem;
        }

        .card-icon {
            font-size: 2.4rem;
            margin-bottom: 1rem;
            display: block;
        }

        .card-title {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 1.6rem;
            color: var(--white);
            margin-bottom: 0.5rem;
        }

        .card-sub {
            font-size: 0.88rem;
            color: rgba(255,255,255,0.5);
            line-height: 1.6;
            margin: 0;
        }

        .msg-panel {
            background: rgba(159,193,49,0.12);
            border: 1px solid rgba(159,193,49,0.3);
            border-radius: 10px;
            color: var(--lime);
            padding: 0.75rem 1rem;
            font-size: 0.88rem;
            margin-bottom: 1.2rem;
        }

        .field-group {
            margin-bottom: 1.4rem;
        }

        .field-label {
            display: block;
            font-size: 0.84rem;
            font-weight: 500;
            color: rgba(255,255,255,0.75);
            margin-bottom: 0.45rem;
            letter-spacing: 0.02em;
        }

        .field-input {
            width: 100%;
            background: rgba(255,255,255,0.1);
            border: 1px solid rgba(255,255,255,0.12);
            border-radius: 10px;
            color: var(--white);
            font-size: 0.95rem;
            padding: 0.7rem 1rem;
            outline: none;
            transition: all 0.25s ease;
            font-family: 'DM Sans', sans-serif;
        }

            .field-input::placeholder {
                color: rgba(255,255,255,0.35);
            }

            .field-input:focus {
                background: rgba(255,255,255,0.15);
                border-color: rgba(159,193,49,0.5);
                box-shadow: 0 0 0 3px rgba(159,193,49,0.12);
                color: var(--white);
            }

        .btn-submit {
            width: 100%;
            background: var(--green);
            border: none;
            border-radius: 50px;
            color: var(--dark);
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 1rem;
            padding: 0.85rem;
            cursor: pointer;
            transition: all 0.25s ease;
            letter-spacing: 0.02em;
            margin-bottom: 1.4rem;
        }

            .btn-submit:hover {
                background: var(--lime);
                transform: translateY(-2px);
                box-shadow: 0 8px 24px rgba(159,193,49,0.35);
            }

        .bottom-link {
            text-align: center;
        }

        .link-back {
            color: rgba(255,255,255,0.45);
            font-size: 0.84rem;
            text-decoration: none;
            transition: color 0.2s;
        }

            .link-back:hover {
                color: var(--white);
            }

        @keyframes slideUp {
            from {
                opacity: 0;
                transform: translateY(24px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .navbar-top {
            padding: 1rem 2rem;
        }

        .page-wrapper {
            padding: 5rem 1rem 2rem;
        }

        .auth-card {
            max-width: 420px;
            padding: 2.5rem 2.2rem;
            border-radius: 20px;
        }

        .card-icon {
            font-size: 2.4rem;
            margin-bottom: 1rem;
        }

        .card-title {
            font-size: 1.6rem;
            margin-bottom: 0.5rem;
        }

        .card-sub {
            font-size: 0.88rem;
            line-height: 1.6;
        }

        .msg-panel {
            padding: 0.75rem 1rem;
            border-radius: 10px;
            margin-bottom: 1.2rem;
        }

        .field-group {
            margin-bottom: 1.4rem;
        }

        .field-label {
            font-size: 0.84rem;
            margin-bottom: 0.45rem;
        }

        .field-input {
            padding: 0.7rem 1rem;
            border-radius: 10px;
            font-size: 0.95rem;
        }

        .btn-submit {
            padding: 0.85rem;
            border-radius: 50px;
            font-size: 1rem;
            margin-bottom: 1.4rem;
        }

        .shape.s1 {
            width: 500px;
            height: 500px;
            top: -200px;
            right: -150px;
        }

        .shape.s2 {
            width: 350px;
            height: 350px;
            bottom: -100px;
            left: -80px;
        }

        @media (max-width: 576px) {
            .auth-card {
                padding: 2rem 1.4rem;
            }

            .navbar-top {
                padding: 1rem;
            }

            .card-title {
                font-size: 1.4rem;
            }
        }
    </style>
    <div class="bg-shapes">
        <div class="shape s1"></div>
        <div class="shape s2"></div>
    </div>
    <nav class="navbar-top">
        <a href="FrmLogin.aspx" class="back-link">← Volver al login</a>
        <span class="nav-brand">SENA <span>Mejoramiento</span></span>
    </nav>
    <form id="form1" runat="server">
        <div class="page-wrapper">
            <div class="auth-card">
                <div class="card-header-custom">
                    <div class="card-icon">🔑</div>
                    <h2 class="card-title">Recuperar Acceso</h2>
                    <p class="card-sub">Ingresa tu correo y te enviaremos un enlace para restablecer tu contraseña</p>
                </div>
                <asp:Panel ID="pnlResultado" runat="server" Visible="false" CssClass="msg-panel" role="alert">
                    <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                </asp:Panel>
                <div class="field-group">
                    <label class="field-label">Correo Institucional</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="field-input" placeholder="ejemplo@sena.edu.co" TextMode="Email"></asp:TextBox>
                </div>
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar Enlace de Recuperación" CssClass="btn-submit" OnClick="btnEnviar_Click" />
                <div class="bottom-link">
                    <a href="FrmLogin.aspx" class="link-back">← Volver al inicio de sesión</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
