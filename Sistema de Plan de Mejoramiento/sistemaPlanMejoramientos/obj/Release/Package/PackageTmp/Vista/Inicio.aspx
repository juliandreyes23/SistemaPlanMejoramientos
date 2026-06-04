<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.Inicio" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Inicio - Sistema de Planes de Mejoramiento</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap" rel="stylesheet" />
    <link href="Css/inicio.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-custom navbar-dark">
            <div class="container-fluid px-4">
                <a class="navbar-brand d-flex align-items-center gap-2" href="Inicio.aspx">
                    <div class="brand-icon">S</div>
                    <span class="brand-text">SENA <span class="brand-sub">Mejoramiento</span></span>
                </a>
                <button class="navbar-toggler custom-toggler" type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navMain"
                    aria-controls="navMain"
                    aria-expanded="false"
                    aria-label="Toggle navigation">

                    <span class="custom-menu-icon">☰</span>

                </button>
                <div class="collapse navbar-collapse" id="navMain">
                    <ul class="navbar-nav ms-auto align-items-center gap-1">
                        <li class="nav-item">
                            <a class="nav-link nav-link-custom active" href="#inicio" onclick="scrollToSection('secInicio')">Inicio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link nav-link-custom" href="#sobre" onclick="scrollToSection('secSobre')">Sobre el Sistema</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link nav-link-custom" href="#funcionalidades" onclick="scrollToSection('secFuncionalidades')">Funcionalidades</a>
                        </li>
                        <li class="nav-item ms-2">
                            <a class="btn btn-nav-login" href="FrmLogin.aspx">Iniciar Sesión</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <section class="hero-section" id="secInicio">
            <div class="hero-bg-shapes">
                <div class="shape shape-1"></div>
                <div class="shape shape-2"></div>
                <div class="shape shape-3"></div>
            </div>
            <div class="container">
                <div class="row align-items-center min-vh-100 py-5 justify-content-center text-center">
                    <div class="col-lg-7">
                        <div class="hero-badge">Sistema Institucional</div>
                        <h1 class="hero-title">Planes de<br />
                            <span class="hero-accent">Mejoramiento</span><br />
                            SENA</h1>
                        <p class="hero-desc">Plataforma integral para la gestión, seguimiento y evaluación de planes de mejoramiento institucional. Optimiza procesos con trazabilidad y transparencia.</p>
                        <div class="hero-actions d-flex gap-3 flex-wrap justify-content-center">
                            <a href="FrmLogin.aspx" class="btn btn-hero-primary">Acceder al Sistema</a>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="sobre-section" id="secSobre">
            <div class="container py-5">
                <div class="text-center mb-5">
                    <div class="section-tag">¿Qué es?</div>
                    <h2 class="section-title">Sobre el Sistema</h2>
                    <p class="section-desc">El Sistema de Planes de Mejoramiento del SENA es una plataforma institucional diseñada para centralizar, gestionar y hacer seguimiento a los compromisos de mejora derivados de auditorías, evaluaciones y procesos de autoevaluación.</p>
                </div>
                <div class="row g-4 justify-content-center">
                    <div class="col-md-4">
                        <div class="sobre-card">
                            <div class="sobre-icon">🎯</div>
                            <h5 class="sobre-title">Misión</h5>
                            <p class="sobre-desc">Facilitar el cumplimiento de los planes de mejoramiento asegurando trazabilidad y transparencia en cada etapa del proceso institucional.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="sobre-card">
                            <div class="sobre-icon">🏛️</div>
                            <h5 class="sobre-title">Alcance</h5>
                            <p class="sobre-desc">Cubre todas las regionales y centros de formación del SENA a nivel nacional, integrando instructores, coordinadores y directivos.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="sobre-card">
                            <div class="sobre-icon">📈</div>
                            <h5 class="sobre-title">Impacto</h5>
                            <p class="sobre-desc">Mejora continua de la calidad educativa mediante el seguimiento sistemático de acciones correctivas y preventivas.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="features-section" id="secFuncionalidades">
            <div class="container py-5">
                <div class="text-center mb-5">
                    <div class="section-tag">Funcionalidades</div>
                    <h2 class="section-title">Todo lo que necesitas</h2>
                </div>
                <div class="row g-4">
                    <div class="col-md-4">
                        <div class="feature-card">
                            <div class="feature-icon">📊</div>
                            <h5 class="feature-title">Seguimiento en tiempo real</h5>
                            <p class="feature-desc">Monitorea el avance de cada plan con indicadores actualizados y alertas automáticas.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="feature-card">
                            <div class="feature-icon">🔐</div>
                            <h5 class="feature-title">Control de acceso</h5>
                            <p class="feature-desc">Gestión de roles y permisos para instructores, coordinadores y directivos.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="feature-card">
                            <div class="feature-icon">📁</div>
                            <h5 class="feature-title">Gestión documental</h5>
                            <p class="feature-desc">Centraliza y organiza los documentos asociados a cada plan de mejoramiento.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="feature-card">
                            <div class="feature-icon">🔔</div>
                            <h5 class="feature-title">Alertas y notificaciones</h5>
                            <p class="feature-desc">Recibe notificaciones automáticas sobre vencimientos y cambios de estado en los planes asignados.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="feature-card">
                            <div class="feature-icon">📝</div>
                            <h5 class="feature-title">Registro de evidencias</h5>
                            <p class="feature-desc">Adjunta y valida evidencias de cumplimiento directamente desde la plataforma de forma segura.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="feature-card">
                            <div class="feature-icon">📤</div>
                            <h5 class="feature-title">Reportes y exportación</h5>
                            <p class="feature-desc">Genera informes de avance y exporta datos en distintos formatos para auditorías y reportes institucionales.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <footer class="site-footer">
            <div class="container py-3 text-center">
                <span class="footer-text">© 2025 SENA — Sistema de Planes de Mejoramiento</span>
            </div>
        </footer>

    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function scrollToSection(id) {
            var el = document.getElementById(id);
            if (el) {
                el.scrollIntoView({ behavior: 'smooth' });
            }
        }

        window.addEventListener('scroll', function () {
            var sections = ['secInicio', 'secSobre', 'secFuncionalidades'];
            var links = document.querySelectorAll('.nav-link-custom');
            var scrollY = window.scrollY + 100;

            sections.forEach(function (id, i) {
                var el = document.getElementById(id);
                if (el && scrollY >= el.offsetTop && scrollY < el.offsetTop + el.offsetHeight) {
                    links.forEach(function (l) { l.classList.remove('active'); });
                    if (links[i]) links[i].classList.add('active');
                }
            });
        });
    </script>
</body>
</html>
