<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLogin.aspx.cs" Inherits="sistemaPlanMejoramientos.FrmLogin" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar Sesión - Sistema de Planes de Mejoramiento</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap" rel="stylesheet" />
</head>

<body class="login-page">
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        body.login-page {
            background: linear-gradient(135deg, #042940 0%, #005C53 100%) !important;
            min-height: 100vh;
            font-family: 'DM Sans', 'Segoe UI', sans-serif;
            overflow-x: hidden;
            margin: 0;
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
            background: #9FC131;
            top: -200px;
            right: -150px;
        }

        .s2 {
            width: 350px;
            height: 350px;
            background: #DBF227;
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
            background: rgba(4, 41, 64, 0.7);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border-bottom: 1px solid rgba(255,255,255,0.06);
        }

        .back-link {
            color: rgba(255,255,255,0.65);
            text-decoration: none;
            font-size: 0.88rem;
            transition: color 0.2s;
        }

            .back-link:hover {
                color: #ffffff;
            }

        .nav-brand {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 1rem;
            color: #ffffff;
        }

            .nav-brand span {
                color: #9FC131;
            }

        .login-wrapper {
            position: relative;
            z-index: 1;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 6rem 1rem 2rem;
        }

        .login-card {
            background: rgba(255, 255, 255, 0.06) !important;
            backdrop-filter: blur(20px) !important;
            -webkit-backdrop-filter: blur(20px) !important;
            border: 1px solid rgba(255, 255, 255, 0.12) !important;
            border-radius: 20px;
            padding: 2.5rem 2.2rem;
            width: 100%;
            max-width: 420px;
            box-shadow: 0 24px 64px rgba(0,0,0,0.4) !important;
            animation: slideUp 0.5s ease both;
        }

        .card-header-custom {
            text-align: center;
            margin-bottom: 2rem;
        }

        .card-logo {
            width: 52px;
            height: 52px;
            background: #9FC131;
            color: #042940;
            font-family: 'Syne', sans-serif;
            font-weight: 800;
            font-size: 1.5rem;
            border-radius: 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 1rem auto;
            box-shadow: 0 6px 20px rgba(159,193,49,0.45);
        }

        .card-title {
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 1.6rem;
            color: #ffffff;
            margin-bottom: 0.3rem;
        }

        .card-sub {
            font-size: 0.88rem;
            color: rgba(255,255,255,0.5);
            margin: 0;
        }

        .alert-custom {
            background: rgba(220,53,69,0.2);
            border: 1px solid rgba(220,53,69,0.4);
            border-radius: 10px;
            color: #ff8a95;
            padding: 0.75rem 1rem;
            font-size: 0.88rem;
            margin-bottom: 1.2rem;
        }

        .field-group {
            margin-bottom: 1.2rem;
        }

        .field-label {
            display: block;
            font-size: 0.84rem;
            font-weight: 500;
            color: rgba(255,255,255,0.8);
            margin-bottom: 0.45rem;
            letter-spacing: 0.02em;
        }

        body.login-page .field-input,
        body.login-page input.field-input[type="text"],
        body.login-page input.field-input[type="email"],
        body.login-page input.field-input[type="password"] {
            display: block;
            width: 100%;
            background: rgba(255, 255, 255, 0.08) !important;
            border: 1px solid rgba(255, 255, 255, 0.15) !important;
            border-radius: 10px;
            color: #ffffff !important;
            font-size: 0.95rem;
            padding: 0.7rem 1rem;
            outline: none;
            transition: all 0.25s ease;
            font-family: 'DM Sans', sans-serif;
            box-shadow: none !important;
        }

            body.login-page .field-input::placeholder {
                color: rgba(255, 255, 255, 0.4) !important;
            }

            body.login-page .field-input:focus {
                background: rgba(255, 255, 255, 0.15) !important;
                border-color: rgba(159, 193, 49, 0.8) !important;
                box-shadow: 0 0 0 3px rgba(159, 193, 49, 0.25) !important;
                color: #ffffff !important;
            }

        .input-with-btn {
            display: flex;
            gap: 0.5rem;
            align-items: stretch;
            width: 100%;
        }

            .input-with-btn .field-input {
                flex: 1;
            }

        .eye-btn {
            background: rgba(255, 255, 255, 0.08);
            border: 1px solid rgba(255, 255, 255, 0.15);
            border-radius: 10px;
            color: rgba(255, 255, 255, 0.7);
            padding: 0 1rem;
            cursor: pointer;
            font-size: 1.1rem;
            transition: all 0.2s ease;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .eye-btn:hover {
                background: rgba(255, 255, 255, 0.18);
                color: #ffffff;
            }

        .forgot-row {
            text-align: right;
            margin-bottom: 1.4rem;
            margin-top: -0.4rem;
        }

        .forgot-link {
            color: #9FC131;
            font-size: 0.83rem;
            text-decoration: none;
            transition: color 0.2s;
        }

            .forgot-link:hover {
                color: #DBF227;
                text-decoration: underline;
            }

        body.login-page .btn-submit,
        body.login-page input[type="submit"].btn-submit {
            display: block;
            width: 100%;
            background: #9FC131 !important;
            border: none !important;
            border-radius: 50px;
            color: #042940 !important;
            font-family: 'Syne', sans-serif;
            font-weight: 700;
            font-size: 1rem;
            padding: 0.85rem;
            cursor: pointer;
            transition: all 0.25s ease;
            letter-spacing: 0.02em;
            text-align: center;
            box-shadow: 0 4px 15px rgba(159,193,49,0.2) !important;
        }

            body.login-page .btn-submit:hover {
                background: #DBF227 !important;
                color: #042940 !important;
                transform: translateY(-2px);
                box-shadow: 0 8px 24px rgba(159,193,49,0.45) !important;
            }

        .divider {
            text-align: center;
            position: relative;
            margin: 1.4rem 0;
            color: rgba(255,255,255,0.3);
            font-size: 0.82rem;
        }

            .divider::before,
            .divider::after {
                content: '';
                position: absolute;
                top: 50%;
                width: 42%;
                height: 1px;
                background: rgba(255,255,255,0.1);
            }

            .divider::before {
                left: 0;
            }

            .divider::after {
                right: 0;
            }

        .register-row {
            text-align: center;
            font-size: 0.85rem;
            color: rgba(255,255,255,0.45);
            display: flex;
            gap: 0.4rem;
            justify-content: center;
            flex-wrap: wrap;
        }

        .register-link {
            color: #9FC131;
            text-decoration: none;
            font-weight: 500;
            transition: color 0.2s;
        }

            .register-link:hover {
                color: #DBF227;
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

        body.login-page input[type="password"]::-ms-reveal,
        body.login-page input[type="password"]::-ms-clear {
            display: none;
        }

        body.login-page input[type="password"]::-webkit-credentials-auto-fill-button {
            visibility: hidden;
            display: none !important;
            pointer-events: none;
        }

        .login-wrapper {
            padding: 7rem 1rem 2rem;
        }

        .login-card {
            width: 100%;
            max-width: 460px;
        }

        @media (min-width: 1400px) {
            .login-card {
                max-width: 520px;
                padding: 3rem 2.8rem;
            }

            .card-title {
                font-size: 2rem;
            }

            .field-input {
                font-size: 1rem;
                padding: 0.9rem 1rem;
            }
        }

        @media (max-width: 768px) {
            .navbar-top {
                padding: 1rem;
            }

            .login-wrapper {
                padding: 6rem 1rem 2rem;
            }

            .login-card {
                max-width: 100%;
                padding: 2rem 1.4rem;
                border-radius: 18px;
            }

            .card-title {
                font-size: 1.4rem;
            }

            .card-sub {
                font-size: 0.82rem;
            }

            .field-input {
                font-size: 0.92rem;
                padding: 0.75rem 0.9rem;
            }

            .btn-submit {
                font-size: 0.95rem;
                padding: 0.8rem;
            }

            .input-with-btn {
                gap: 0.4rem;
            }

            .eye-btn {
                padding: 0 0.9rem;
            }
        }

        @media (max-width: 480px) {
            .login-wrapper {
                padding: 5.5rem 0.8rem 1.5rem;
            }

            .login-card {
                padding: 1.7rem 1.1rem;
            }

            .card-logo {
                width: 46px;
                height: 46px;
                font-size: 1.3rem;
            }

            .card-title {
                font-size: 1.2rem;
            }

            .register-row {
                flex-direction: column;
                gap: 0.2rem;
            }
        }
    </style>
    <div class="bg-shapes">
        <div class="shape s1"></div>
        <div class="shape s2"></div>
    </div>
    <nav class="navbar-top">
        <a href="Inicio.aspx" class="back-link">← Inicio</a>
        <span class="nav-brand">SENA <span>Mejoramiento</span></span>
    </nav>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <div class="login-card">
                <div class="card-header-custom">
                    <div class="card-logo">S</div>
                    <h2 class="card-title">Bienvenido</h2>
                    <p class="card-sub">Ingresa tus credenciales institucionales</p>
                </div>
                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert-custom" role="alert">
                    <asp:Label ID="lblMensajeError" runat="server"></asp:Label>
                </asp:Panel>
                <div class="field-group">
                    <label class="field-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="field-input" placeholder="ejemplo@sena.edu.co" TextMode="Email"></asp:TextBox>
                </div>
                <div class="field-group">
                    <label class="field-label">Contraseña</label>
                    <div class="input-with-btn">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="field-input" placeholder="••••••••" TextMode="Password"></asp:TextBox>
                        <button type="button" class="eye-btn" onclick="togglePassword()">👁</button>
                    </div>
                </div>
                <div class="forgot-row">
                    <a href="FrmRecuperar.aspx" class="forgot-link">¿Olvidaste tu contraseña?</a>
                </div>
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar al Sistema" CssClass="btn-submit" OnClick="btnIngresar_Click" />
                <div class="divider"><span>o</span></div>
                <div class="register-row">
                    <span>¿Aún no tienes acceso?</span>
                    <a href="#" class="register-link">Solicitar cuenta</a>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function togglePassword() {
            var p = document.getElementById('<%= txtPassword.ClientID %>');
            p.type = p.type === "password" ? "text" : "password";
        }
    </script>
</body>
</html>
