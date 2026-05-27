# Sistema de Planes de Mejoramiento

Sistema web desarrollado en ASP.NET para la gestión de planes de mejoramiento académicos, aprendices, instructores, fichas y evidencias dentro de un entorno de formación.

---

# Descripción

El proyecto **sistemaPlanMejoramientos** permite administrar procesos académicos relacionados con:

* Gestión de aprendices.
* Gestión de instructores.
* Administración de fichas.
* Gestión de programas de formación.
* Creación de planes de mejoramiento.
* Subida y evaluación de evidencias.
* Control de usuarios y roles.
* Históricos y seguimiento.

El sistema está orientado a instituciones de formación similares al SENA.

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
* SQL Server Management Studio
* Git
* GitHub

---

# Características Principales

## Administración de Usuarios

* Inicio de sesión.
* Recuperación de contraseña.
* Roles de usuario.
* Gestión de perfiles.

## Gestión Académica

* Registro de aprendices.
* Registro de instructores.
* Gestión de programas.
* Gestión de centros.
* Gestión de fichas.

## Planes de Mejoramiento

* Creación de planes.
* Seguimiento de actividades.
* Asignación de resultados de aprendizaje.
* Control de estados.

## Evidencias

* Subida de archivos.
* Evaluación de evidencias.
* Observaciones del instructor.

## Históricos

* Consulta de registros.
* Seguimiento de procesos.
* Historial de evaluaciones.

---

# Estructura General del Proyecto


sistemaPlanMejoramientos/
│
├── Datos/
├── Logica/
├── Modelo/
├── Vista/
│   ├── Dashboard.aspx
│   ├── FrmLogin.aspx
│   ├── GestionAprendices.aspx
│   ├── GestionInstructores.aspx
│   ├── GestionFichas.aspx
│   ├── FrmCrearPlan.aspx
│   ├── FrmSubirEvidencia.aspx
│   └── ...
└── Web.config


---

# Requisitos del Sistema

## Software Necesario

* Visual Studio 2019 o superior.
* SQL Server.
* .NET Framework.
* Navegador web moderno.

---

# Instalación del Proyecto

## 1. Clonar el repositorio


git clone https://github.com/juliandreyes23/SistemaPlanMejoramientos.git




## 2. Abrir la solución

Abrir el archivo:


sistemaPlanMejoramientos.sln


Desde Visual Studio.

---

## 3. Configurar la base de datos
Abrir SQL Server Management Studio.
Crear la base de datos llamada:

planMejoramiento

Ejecutar el script SQL del proyecto para crear las tablas.
Abrir la solución en Visual Studio.
Ubicar la clase de conexión:

Datos/ClConexion.cs

Verificar la cadena de conexión utilizada por el sistema.

Ejemplo utilizado en el proyecto:

SqlConnection oConex = new SqlConnection(
    "Data Source=.;Initial Catalog=planMejoramiento;Integrated Security=True;Encrypt=False;"
);

Importante

Si el nombre del servidor o la base de datos es diferente en otro equipo, se debe modificar la cadena de conexión dentro de:

ClConexion.cs


# Roles del Sistema

## Administrador

Puede:

* Gestionar usuarios.
* Gestionar centros.
* Gestionar programas.
* Gestionar fichas.
* Consultar históricos.

## Instructor

Puede:

* Evaluar evidencias.
* Gestionar aprendices.
* Crear planes de mejoramiento.
* Consultar fichas.

## Aprendiz

Puede:

* Consultar planes.
* Subir evidencias.
* Revisar observaciones.
* Consultar estados.

---

# Módulos Principales

| Módulo                  | Función                            |
| ----------------------- | ---------------------------------- |
| Login                   | Inicio de sesión                   |
| Dashboard               | Panel principal                    |
| Gestión de Aprendices   | Administración de aprendices       |
| Gestión de Instructores | Administración de instructores     |
| Gestión de Fichas       | Administración de fichas           |
| Gestión de Programas    | Administración de programas        |
| Crear Plan              | Registro de planes de mejoramiento |
| Evidencias              | Subida y evaluación de archivos    |
| Históricos              | Consulta de registros              |

---

# Seguridad

El sistema cuenta con:

* Validación de usuarios.
* Control de roles.
* Recuperación de contraseña.
* Restricción de acceso por módulos.

---

# Capturas del Sistema

## Página de Inicio

Agregar captura aquí.

## Dashboard

Agregar captura aquí.

## Gestión de Aprendices

Agregar captura aquí.

## Planes de Mejoramiento

Agregar captura aquí.

---

# Autor

Proyecto desarrollado por:

**Julian Reyes**
