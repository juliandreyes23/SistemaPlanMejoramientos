using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClPlanMejoramientoD
    {
        ClConexion oConex = new ClConexion();

        public int MtCrearPlanMejoramiento(string tipoPlan, DateTime fechaAsignacion, DateTime fechaLimite, string actividades, string observaciones, string estadoPlan, int idAprendiz, int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            int idPlanGenerado = 0;

            string query = @"INSERT INTO planesMejoramiento
                    (tipoPlan, fechaAsignacion, fechaLimite, actividades, observaciones, estadoPlan, idAprendiz, idInstructor)
                     VALUES
                    (@tipoPlan, @fechaAsignacion, @fechaLimite, @actividades, @observaciones, @estadoPlan, @idAprendiz, @idInstructor);
                     SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@tipoPlan", tipoPlan);
            cmd.Parameters.AddWithValue("@fechaAsignacion", fechaAsignacion);
            cmd.Parameters.AddWithValue("@fechaLimite", fechaLimite);
            cmd.Parameters.AddWithValue("@actividades", actividades);

            if (string.IsNullOrEmpty(observaciones))
                cmd.Parameters.AddWithValue("@observaciones", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@observaciones", observaciones);

            cmd.Parameters.AddWithValue("@estadoPlan", estadoPlan);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);

            object resultado = cmd.ExecuteScalar();

            if (resultado != null)
                idPlanGenerado = Convert.ToInt32(resultado);

            oConex.MtCerrarConexion();

            return idPlanGenerado;
        }

        public bool MtAsociarResultadoAPlan(int idPlanMejoramiento, int idResultadoAprendizaje)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO planResultados
                    (idPlanMejoramiento, idResultadoAprendizaje)
                     VALUES
                    (@idPlanMejoramiento, @idResultadoAprendizaje)";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);
            cmd.Parameters.AddWithValue("@idResultadoAprendizaje", idResultadoAprendizaje);

            int filas = cmd.ExecuteNonQuery();

            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public List<ClPlanMejoramientoM> MtListarPlanes()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT p.idPlanMejoramiento,
                            p.tipoPlan,
                            p.fechaAsignacion,
                            p.fechaLimite,
                            p.actividades,
                            p.estadoPlan,
                            a.idAprendiz,
                            a.nombres,
                            a.apellidos,
                            a.numeroDocumento,
                            i.idInstructor,
                            i.nombres AS nombresInstructor,
                            i.apellidos AS apellidosInstructor
                     FROM planesMejoramiento p
                     INNER JOIN aprendices a ON p.idAprendiz = a.idAprendiz
                     INNER JOIN instructores i ON p.idInstructor = i.idInstructor";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataReader rd = cmd.ExecuteReader();
            List<ClPlanMejoramientoM> lista = new List<ClPlanMejoramientoM>();

            while (rd.Read())
            {
                lista.Add(new ClPlanMejoramientoM
                {
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    tipoPlan = rd["tipoPlan"].ToString(),
                    fechaAsignacion = Convert.ToDateTime(rd["fechaAsignacion"]),
                    fechaLimite = Convert.ToDateTime(rd["fechaLimite"]),
                    actividades = rd["actividades"].ToString(),
                    estadoPlan = rd["estadoPlan"].ToString(),
                    nombreAprendiz = rd["nombres"] + " " + rd["apellidos"],
                    docAprendiz = rd["numeroDocumento"].ToString(),
                    nombreInstructor = rd["nombresInstructor"] + " " + rd["apellidosInstructor"],
                    aprendiz = new ClAprendizM
                    {
                        idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                        nombres = rd["nombres"].ToString(),
                        apellidos = rd["apellidos"].ToString(),
                        numeroDocumento = rd["numeroDocumento"].ToString()
                    },
                    instructor = new ClInstructoresM
                    {
                        idInstructor = Convert.ToInt32(rd["idInstructor"]),
                        nombres = rd["nombresInstructor"].ToString(),
                        apellidos = rd["apellidosInstructor"].ToString()
                    }
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();
            return lista;
        }

        public bool MtActualizarEstadoPlan(int idPlanMejoramiento, string estadoPlan, string observaciones)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE planesMejoramiento
                     SET estadoPlan = @estadoPlan,
                         observaciones = @observaciones
                     WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);
            cmd.Parameters.AddWithValue("@estadoPlan", estadoPlan);
            cmd.Parameters.AddWithValue("@observaciones", observaciones);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtEliminarPlan(int idPlanMejoramiento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            SqlTransaction transaction = cn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("", cn, transaction);
                cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

                cmd.CommandText = @"DELETE FROM planResultados
                            WHERE idPlanMejoramiento = @idPlanMejoramiento";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evaluaciones
                            WHERE idPlanMejoramiento = @idPlanMejoramiento";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM evidencias
                            WHERE idPlanMejoramiento = @idPlanMejoramiento";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"DELETE FROM planesMejoramiento
                            WHERE idPlanMejoramiento = @idPlanMejoramiento";
                int filas = cmd.ExecuteNonQuery();

                transaction.Commit();
                return filas > 0;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                oConex.MtCerrarConexion();
            }
        }

        public int MtContarPlanesPorTipo(int idInstructor, string tipoPlan)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(*)
                     FROM planesMejoramiento
                     WHERE idInstructor = @idInstructor
                     AND tipoPlan = @tipoPlan";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@tipoPlan", tipoPlan);

            int total = Convert.ToInt32(cmd.ExecuteScalar());
            oConex.MtCerrarConexion();
            return total;
        }

        public List<ClAprendizM> MtListarAprendicesPorInstructor(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
    SELECT DISTINCT
           a.idAprendiz,
           a.nombres,
           a.apellidos,
           a.numeroDocumento,
           f.codigoFicha,
           f.idFicha
    FROM aprendices a
    INNER JOIN fichas f
        ON a.idFicha = f.idFicha
    INNER JOIN fichaInstructor fi
        ON f.idFicha = fi.idFicha
    WHERE fi.idInstructor = @idInstructor
    AND a.estadoAcademico IN ('En formación','Condicionado')
    ORDER BY a.nombres";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);

            SqlDataReader rd = cmd.ExecuteReader();
            List<ClAprendizM> lista = new List<ClAprendizM>();

            while (rd.Read())
            {
                lista.Add(new ClAprendizM
                {
                    idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                    nombres = rd["nombres"].ToString(),
                    apellidos = rd["apellidos"].ToString(),
                    numeroDocumento = rd["numeroDocumento"].ToString(),
                    idFicha = Convert.ToInt32(rd["idFicha"]),
                    codigoFicha = rd["codigoFicha"].ToString(),
                    fichas = new List<ClFichaAprendizM>
                    {
                        new ClFichaAprendizM
                        {
                            idFicha = Convert.ToInt32(rd["idFicha"]),
                            ficha = new ClFichasM
                            {
                                idFicha = Convert.ToInt32(rd["idFicha"]),
                                codigoFicha = rd["codigoFicha"].ToString()
                            }
                        }
                    }
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();
            return lista;
        }

        public List<ClResultadoAprendizajeM> MtListarResultadosPorFicha(int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
    SELECT ra.idResultadoAprendizaje,
           ra.descripcion,
           c.idCompetencia,
           c.descripcion AS nombreCompetencia
    FROM resultadoAprendizaje ra
    INNER JOIN competencias c
        ON ra.idCompetencia = c.idCompetencia
    INNER JOIN programas p
        ON c.idPrograma = p.idPrograma
    INNER JOIN fichas f
        ON f.idPrograma = p.idPrograma
    WHERE f.idFicha = @idFicha
    ORDER BY c.descripcion, ra.descripcion";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);

            SqlDataReader rd = cmd.ExecuteReader();
            List<ClResultadoAprendizajeM> lista = new List<ClResultadoAprendizajeM>();

            while (rd.Read())
            {
                lista.Add(new ClResultadoAprendizajeM
                {
                    idResultadoAprendizaje = Convert.ToInt32(rd["idResultadoAprendizaje"]),
                    descripcion = rd["descripcion"].ToString(),
                    nombreCompetencia = rd["nombreCompetencia"].ToString(),
                    idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                    competencia = new ClCompetenciasM
                    {
                        idCompetencia = Convert.ToInt32(rd["idCompetencia"]),
                        descripcion = rd["nombreCompetencia"].ToString()
                    }
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();
            return lista;
        }

        public bool MtExistePlanComitePendiente(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(*)
                     FROM planesMejoramiento
                     WHERE idAprendiz = @idAprendiz
                     AND tipoPlan = 'Comité'
                     AND estadoPlan = 'Pendiente'";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);

            int total = Convert.ToInt32(cmd.ExecuteScalar());
            oConex.MtCerrarConexion();
            return total > 0;
        }

        public List<ClPlanMejoramientoM> MtListarPlanesPendientesEvaluacion(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
    SELECT pm.idPlanMejoramiento,
           pm.tipoPlan,
           pm.fechaAsignacion,
           pm.fechaLimite,
           pm.actividades,
           pm.estadoPlan,
           a.idAprendiz,
           a.nombres + ' ' + a.apellidos AS nombreAprendiz,
           a.numeroDocumento AS docAprendiz,
           f.codigoFicha,
           (
               SELECT COUNT(*)
               FROM evidencias e
               WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento
           ) AS totalEvidencias,
           CASE
               WHEN ev.idEvaluacion IS NOT NULL THEN 1
               ELSE 0
           END AS yaEvaluado,
           ev.criterioProducto,
           ev.criterioConocimiento,
           ev.criterioDesempeno
    FROM planesMejoramiento pm
    INNER JOIN aprendices a ON pm.idAprendiz = a.idAprendiz
    INNER JOIN fichas f ON a.idFicha = f.idFicha
    INNER JOIN fichaInstructor fi ON fi.idFicha = f.idFicha
    LEFT JOIN evaluaciones ev ON ev.idPlanMejoramiento = pm.idPlanMejoramiento
    WHERE fi.idInstructor = @idInstructor
    AND pm.estadoPlan = 'Pendiente'
    AND EXISTS
    (
        SELECT 1
        FROM evidencias e2
        WHERE e2.idPlanMejoramiento = pm.idPlanMejoramiento
    )
    ORDER BY pm.fechaLimite ASC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);

            SqlDataReader rd = cmd.ExecuteReader();
            List<ClPlanMejoramientoM> lista = new List<ClPlanMejoramientoM>();

            while (rd.Read())
            {
                lista.Add(new ClPlanMejoramientoM
                {
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    tipoPlan = rd["tipoPlan"].ToString(),
                    fechaAsignacion = Convert.ToDateTime(rd["fechaAsignacion"]),
                    fechaLimite = Convert.ToDateTime(rd["fechaLimite"]),
                    actividades = rd["actividades"].ToString(),
                    estadoPlan = rd["estadoPlan"].ToString(),
                    idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                    nombreAprendiz = rd["nombreAprendiz"].ToString(),
                    docAprendiz = rd["docAprendiz"].ToString(),
                    codigoFicha = rd["codigoFicha"].ToString(),
                    totalEvidencias = Convert.ToInt32(rd["totalEvidencias"]),
                    yaEvaluado = Convert.ToBoolean(rd["yaEvaluado"]),
                    criterioProducto = rd["criterioProducto"] == DBNull.Value ? null : rd["criterioProducto"].ToString(),
                    criterioConocimiento = rd["criterioConocimiento"] == DBNull.Value ? null : rd["criterioConocimiento"].ToString(),
                    criterioDesempeno = rd["criterioDesempeno"] == DBNull.Value ? null : rd["criterioDesempeno"].ToString()
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();
            return lista;
        }

        public List<ClPlanMejoramientoM> MtListarPlanesInternosPorInstructor(int idInstructor, string filtroNombre, string filtroEstado)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
    SELECT  pm.idPlanMejoramiento,
            pm.fechaAsignacion,
            pm.fechaLimite,
            pm.actividades,
            pm.observaciones,
            pm.estadoPlan,
            a.idAprendiz,
            a.nombres + ' ' + a.apellidos AS nombreAprendiz,
            a.numeroDocumento AS docAprendiz,
            f.codigoFicha,
            i.nombres + ' ' + i.apellidos AS nombreInstructor,
            (
                SELECT COUNT(*)
                FROM evidencias e
                WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento
            ) AS totalEvidencias
    FROM planesMejoramiento pm
    INNER JOIN aprendices a ON pm.idAprendiz = a.idAprendiz
    INNER JOIN fichas f ON a.idFicha = f.idFicha
    INNER JOIN fichaInstructor fi ON fi.idFicha = f.idFicha
    INNER JOIN instructores i ON pm.idInstructor = i.idInstructor
    WHERE fi.idInstructor = @idInstructor
    AND pm.tipoPlan = 'Interno'
    AND (@filtroEstado = '' OR pm.estadoPlan = @filtroEstado)
    AND (
            @filtroNombre = ''
            OR a.nombres + ' ' + a.apellidos LIKE '%' + @filtroNombre + '%'
            OR a.numeroDocumento LIKE '%' + @filtroNombre + '%'
        )
    ORDER BY pm.fechaAsignacion DESC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@filtroEstado", filtroEstado ?? "");
            cmd.Parameters.AddWithValue("@filtroNombre", filtroNombre ?? "");

            SqlDataReader rd = cmd.ExecuteReader();
            List<ClPlanMejoramientoM> lista = new List<ClPlanMejoramientoM>();

            while (rd.Read())
            {
                lista.Add(new ClPlanMejoramientoM
                {
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    fechaAsignacion = Convert.ToDateTime(rd["fechaAsignacion"]),
                    fechaLimite = Convert.ToDateTime(rd["fechaLimite"]),
                    actividades = rd["actividades"].ToString(),
                    observaciones = rd["observaciones"] == DBNull.Value ? null : rd["observaciones"].ToString(),
                    estadoPlan = rd["estadoPlan"].ToString(),
                    idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                    nombreAprendiz = rd["nombreAprendiz"].ToString(),
                    docAprendiz = rd["docAprendiz"].ToString(),
                    codigoFicha = rd["codigoFicha"].ToString(),
                    nombreInstructor = rd["nombreInstructor"].ToString(),
                    totalEvidencias = Convert.ToInt32(rd["totalEvidencias"])
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();
            return lista;
        }

        public List<ClPlanMejoramientoM> MtListarPlanesComitePorInstructor(int idInstructor, string filtroNombre, string filtroEstado)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
    SELECT  pm.idPlanMejoramiento,
            pm.fechaAsignacion,
            pm.fechaLimite,
            pm.actividades,
            pm.observaciones,
            pm.estadoPlan,
            a.idAprendiz,
            a.nombres + ' ' + a.apellidos AS nombreAprendiz,
            a.numeroDocumento AS docAprendiz,
            f.codigoFicha,
            i.nombres + ' ' + i.apellidos AS nombreInstructor,
            (
                SELECT COUNT(*)
                FROM evidencias e
                WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento
            ) AS totalEvidencias
    FROM planesMejoramiento pm
    INNER JOIN aprendices a ON pm.idAprendiz = a.idAprendiz
    INNER JOIN fichas f ON a.idFicha = f.idFicha
    INNER JOIN fichaInstructor fi ON fi.idFicha = f.idFicha
    INNER JOIN instructores i ON pm.idInstructor = i.idInstructor
    WHERE fi.idInstructor = @idInstructor
    AND pm.tipoPlan = 'Comité'
    AND (@filtroEstado = '' OR pm.estadoPlan = @filtroEstado)
    AND (
            @filtroNombre = ''
            OR a.nombres + ' ' + a.apellidos LIKE '%' + @filtroNombre + '%'
            OR a.numeroDocumento LIKE '%' + @filtroNombre + '%'
        )
    ORDER BY pm.fechaAsignacion DESC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@filtroEstado", filtroEstado ?? "");
            cmd.Parameters.AddWithValue("@filtroNombre", filtroNombre ?? "");

            SqlDataReader rd = cmd.ExecuteReader();
            List<ClPlanMejoramientoM> lista = new List<ClPlanMejoramientoM>();

            while (rd.Read())
            {
                lista.Add(new ClPlanMejoramientoM
                {
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    fechaAsignacion = Convert.ToDateTime(rd["fechaAsignacion"]),
                    fechaLimite = Convert.ToDateTime(rd["fechaLimite"]),
                    actividades = rd["actividades"].ToString(),
                    observaciones = rd["observaciones"] == DBNull.Value ? null : rd["observaciones"].ToString(),
                    estadoPlan = rd["estadoPlan"].ToString(),
                    idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                    nombreAprendiz = rd["nombreAprendiz"].ToString(),
                    docAprendiz = rd["docAprendiz"].ToString(),
                    codigoFicha = rd["codigoFicha"].ToString(),
                    nombreInstructor = rd["nombreInstructor"].ToString(),
                    totalEvidencias = Convert.ToInt32(rd["totalEvidencias"])
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();
            return lista;
        }

        public bool MtCancelarAprendiz(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE aprendices
                     SET estadoAcademico = 'Cancelado'
                     WHERE idAprendiz = @idAprendiz";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }
    }
}