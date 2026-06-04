# Sistema de Planes de Mejoramiento

Sistema web desarrollado en ASP.NET Web Forms para la gestión de planes de mejoramiento académicos, aprendices, instructores, fichas, programas de formación y evidencias dentro de un entorno de formación similar al SENA.

---

# Descripción

El proyecto **Sistema de Planes de Mejoramiento** permite administrar y realizar seguimiento a los procesos académicos relacionados con aprendices que requieren actividades de mejoramiento para alcanzar los resultados de aprendizaje establecidos en su programa de formación.

La plataforma facilita la interacción entre administradores, instructores y aprendices mediante módulos especializados para cada rol.

---

# Tecnologías Utilizadas

## Backend

* ASP.NET Web Forms
* C#
* .NET Framework

## Frontend

* HTML5
* CSS3
* JavaScript
* Bootstrap

## Base de Datos

* SQL Server

## Herramientas

* Visual Studio
* SQL Server Management Studio (SSMS)
* Git
* GitHub

---

# Características Principales

## Administración de Usuarios

* Inicio de sesión.
* Recuperación de contraseña.
* Gestión de usuarios.
* Administración de roles.
* Gestión de perfiles.

## Gestión Académica

* Gestión de centros de formación.
* Gestión de programas de formación.
* Gestión de competencias.
* Gestión de resultados de aprendizaje.
* Gestión de fichas.
* Gestión de aprendices.
* Gestión de instructores.
* Asignación de instructores.
* Carga masiva de aprendices.

## Planes de Mejoramiento

* Creación de planes de mejoramiento.
* Asignación de resultados de aprendizaje.
* Seguimiento de actividades.
* Registro de observaciones.
* Control de estados.

## Evidencias

* Carga de evidencias.
* Evaluación por parte del instructor.
* Registro de observaciones.
* Seguimiento del estado de aprobación.

## Históricos

* Consulta de registros históricos.
* Seguimiento de procesos académicos.
* Historial de evaluaciones.
* Historial de planes de mejoramiento.

---

# Estructura General del Proyecto

text
sistemaPlanMejoramientos/
│
├── Datos/
├── Logica/
├── Modelo/
├── Vista/
│   ├── Dashboard.aspx
│   ├── DashboardInstructor.aspx
│   ├── DashboardAprendiz.aspx
│   ├── FrmLogin.aspx
│   ├── FrmRecuperar.aspx
│   ├── resetPassword.aspx
│   ├── FrmUsuarios.aspx
│   ├── GestionCentros.aspx
│   ├── GestionProgramas.aspx
│   ├── GestionCompetencias.aspx
│   ├── ResultadosAprendizaje.aspx
│   ├── GestionFichas.aspx
│   ├── FrmConsultarFichas.aspx
│   ├── GestionAprendices.aspx
│   ├── Cargamasivaaprendices.aspx
│   ├── GestionInstructores.aspx
│   ├── AsignacionInstructores.aspx
│   ├── FrmCrearPlan.aspx
│   ├── FrmMisPlanes.aspx
│   ├── FrmSubirEvidencia.aspx
│   ├── FrmEvaluarEvidencias.aspx
│   ├── FrmHistoricoComite.aspx
│   ├── FrmHistoricoInternos.aspx
│   ├── FrmMiPerfil.aspx
│   └── ...
│
└── Web.config


---

# Requisitos del Sistema

## Software Necesario

* Visual Studio 2019 o superior.
* SQL Server.
* SQL Server Management Studio (SSMS).
* .NET Framework.
* Navegador web moderno.

---

# Instalación del Proyecto

## 1. Clonar el repositorio

bash
git clone https://github.com/juliandreyes23/SistemaPlanMejoramientos.git


---

## 2. Abrir la solución

Abrir el archivo:

text
sistemaPlanMejoramientos.sln


desde Visual Studio.

---

## 3. Configurar la Base de Datos

### Crear la Base de Datos

Crear una base de datos llamada:

sql
planMejoramiento


### Ejecutar Scripts

Ejecutar los scripts SQL incluidos en el proyecto para crear:

* Tablas.
* Procedimientos almacenados.
* Relaciones.
* Datos iniciales (si aplica).

### Configurar la Conexión

Ubicar la clase:

text
Datos/ClConexion.cs


Verificar la cadena de conexión:

SqlConnection oConex = new SqlConnection(
    "Data Source=.;Initial Catalog=planMejoramiento;Integrated Security=True;Encrypt=False;"
);


### Importante

Si el servidor SQL o el nombre de la base de datos es diferente, se deberá modificar la cadena de conexión en:

text
Datos/ClConexion.cs


---

# Roles del Sistema

## Administrador

Responsable de la administración general del sistema.

### Funciones

* Gestionar usuarios.
* Gestionar centros de formación.
* Gestionar programas.
* Gestionar competencias.
* Gestionar resultados de aprendizaje.
* Gestionar fichas.
* Gestionar aprendices.
* Gestionar instructores.
* Asignar instructores.
* Realizar carga masiva de aprendices.
* Consultar información académica.

---

## Instructor

Responsable del seguimiento académico y evaluación de evidencias.

### Funciones

* Consultar aprendices asignados.
* Crear planes de mejoramiento.
* Evaluar evidencias.
* Registrar observaciones.
* Consultar históricos.
* Realizar seguimiento académico.

---

## Aprendiz

Responsable de cumplir las actividades establecidas en el plan de mejoramiento.

### Funciones

* Consultar planes asignados.
* Revisar observaciones.
* Subir evidencias.
* Consultar estados del plan.
* Actualizar información personal.

---

# Módulos Principales

| Módulo                     | Función                            |
| -------------------------- | ---------------------------------- |
| Login                      | Inicio de sesión                   |
| Recuperación de Contraseña | Restablecimiento de acceso         |
| Dashboard Administrador    | Administración general             |
| Dashboard Instructor       | Seguimiento y evaluación           |
| Dashboard Aprendiz         | Consulta de planes                 |
| Gestión de Usuarios        | Administración de usuarios         |
| Gestión de Centros         | Administración de centros          |
| Gestión de Programas       | Administración de programas        |
| Gestión de Competencias    | Administración de competencias     |
| Resultados de Aprendizaje  | Administración de resultados       |
| Gestión de Fichas          | Administración de fichas           |
| Gestión de Aprendices      | Administración de aprendices       |
| Carga Masiva de Aprendices | Importación masiva                 |
| Gestión de Instructores    | Administración de instructores     |
| Asignación de Instructores | Asociación instructor-ficha        |
| Crear Plan                 | Registro de planes                 |
| Mis Planes                 | Consulta de planes asignados       |
| Evidencias                 | Carga y evaluación                 |
| Históricos                 | Consulta de registros              |
| Perfil de Usuario          | Administración de datos personales |

---

# Seguridad

El sistema implementa mecanismos de seguridad para proteger la información y controlar el acceso a los diferentes módulos.

## Funcionalidades de Seguridad

* Validación de credenciales.
* Control de acceso basado en roles.
* Recuperación de contraseña mediante correo electrónico.
* Restricción de acceso por perfil.
* Protección de funcionalidades según permisos del usuario.

---

# Flujo General del Sistema

1. El usuario inicia sesión.
2. El sistema identifica el rol asignado.
3. Se redirecciona al dashboard correspondiente.
4. El administrador configura la estructura académica.
5. Los instructores crean planes de mejoramiento.
6. Los aprendices cargan evidencias.
7. Los instructores evalúan evidencias.
8. El sistema almacena el historial de actividades realizadas.

---

# Capturas del Sistema

## Página de Inicio

Agregar captura de la página principal.

## Inicio de Sesión

Agregar captura del formulario de autenticación.

## Dashboard Administrador

Agregar captura del panel administrativo.

## Dashboard Instructor

Agregar captura del panel del instructor.

## Dashboard Aprendiz

Agregar captura del panel del aprendiz.

## Gestión de Aprendices

Agregar captura del módulo.

## Gestión de Instructores

Agregar captura del módulo.

## Creación de Planes de Mejoramiento

Agregar captura del formulario.

## Subida de Evidencias

Agregar captura del módulo.

---

# Autor

**Julian Reyes**

Proyecto académico desarrollado para la gestión de planes de mejoramiento y seguimiento académico dentro de entornos de formación.
