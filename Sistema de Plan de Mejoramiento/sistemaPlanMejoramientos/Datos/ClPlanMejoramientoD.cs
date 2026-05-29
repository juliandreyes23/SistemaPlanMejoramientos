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

            string query = @"INSERT INTO planesMejoramiento (tipoPlan, fechaAsignacion, fechaLimite, actividades, observaciones, estadoPlan, idAprendiz, idInstructor) 
                             VALUES (@tipoPlan, @fechaAsignacion, @fechaLimite, @actividades, @observaciones, @estadoPlan, @idAprendiz, @idInstructor);
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
            {
                idPlanGenerado = Convert.ToInt32(resultado);
            }

            oConex.MtCerrarConexion();
            return idPlanGenerado;
        }

        public bool MtAsociarResultadoAPlan(int idPlanMejoramiento, int idResultadoAprendizaje)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO planResultados (idPlanMejoramiento, idResultadoAprendizaje) 
                             VALUES (@idPlanMejoramiento, @idResultadoAprendizaje)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);
            cmd.Parameters.AddWithValue("@idResultadoAprendizaje", idResultadoAprendizaje);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return filas > 0;
        }

        public DataTable MtListarPlanes()
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT p.idPlanMejoramiento, p.tipoPlan, p.fechaAsignacion, p.fechaLimite, p.actividades, p.estadoPlan,
                                    a.nombres + ' ' + a.apellidos AS NombreAprendiz, a.numeroDocumento AS DocAprendiz,
                                    i.nombres + ' ' + i.apellidos AS NombreInstructor
                             FROM planesMejoramiento p
                             INNER JOIN aprendices a ON p.idAprendiz = a.idAprendiz
                             INNER JOIN instructores i ON p.idInstructor = i.idInstructor";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtActualizarEstadoPlan(int idPlanMejoramiento, string estadoPlan, string observaciones)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE planesMejoramiento 
                             SET estadoPlan = @estadoPlan, observaciones = @observaciones 
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

            string query = "DELETE FROM planesMejoramiento WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();

            return filas > 0;
        }
        public int MtContarPlanesPorTipo(int idInstructor, string tipoPlan)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT COUNT(*) FROM planesMejoramiento 
                     WHERE idInstructor = @idInstructor 
                     AND tipoPlan = @tipoPlan";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@tipoPlan", tipoPlan);
            int total = (int)cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return total;
        }
        public DataTable MtListarAprendicesPorInstructor(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"
        SELECT DISTINCT a.idAprendiz, 
               a.nombres + ' ' + a.apellidos AS nombreCompleto,
               a.nombres,
               a.numeroDocumento, f.codigoFicha, f.idFicha
        FROM aprendices a
        INNER JOIN fichaAprendiz fa   ON a.idAprendiz = fa.idAprendiz
        INNER JOIN fichas f           ON fa.idFicha   = f.idFicha
        INNER JOIN fichaInstructor fi ON f.idFicha    = fi.idFicha
        WHERE fi.idInstructor = @idInstructor
        AND a.estadoAcademico IN ('En formación', 'Condicionado')
        ORDER BY a.nombres";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }

        public DataTable MtListarResultadosPorFicha(int idFicha)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"
        SELECT ra.idResultadoAprendizaje, ra.descripcion,
               c.descripcion AS nombreCompetencia
        FROM resultadoAprendizaje ra
        INNER JOIN competencias c ON ra.idCompetencia = c.idCompetencia
        INNER JOIN programas p    ON c.idPrograma     = p.idPrograma
        INNER JOIN fichas f       ON f.idPrograma     = p.idPrograma
        WHERE f.idFicha = @idFicha
        ORDER BY c.descripcion, ra.descripcion";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idFicha", idFicha);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }
        public bool MtExistePlanComitePendiente(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();
            string query = @"SELECT COUNT(*) FROM planesMejoramiento 
                     WHERE idAprendiz = @idAprendiz 
                     AND tipoPlan = 'Comité' 
                     AND estadoPlan = 'Pendiente'";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            int total = (int)cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return total > 0;
        }
        public DataTable MtListarPlanesPendientesEvaluacion(int idInstructor)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"
        SELECT  pm.idPlanMejoramiento,
                pm.tipoPlan,
                pm.fechaAsignacion,
                pm.fechaLimite,
                pm.actividades,
                pm.estadoPlan,
                a.idAprendiz,
                a.nombres + ' ' + a.apellidos  AS nombreAprendiz,
                a.numeroDocumento              AS docAprendiz,
                f.codigoFicha,
                (SELECT COUNT(*) 
                 FROM evidencias e 
                 WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento) AS totalEvidencias,
                /* ¿Ya tiene evaluación registrada? */
                CASE WHEN ev.idEvaluacion IS NOT NULL THEN 1 ELSE 0 END AS yaEvaluado,
                ev.criterioProducto,
                ev.criterioConocimiento,
                ev.criterioDesempeno
        FROM    planesMejoramiento pm
        INNER JOIN aprendices      a  ON pm.idAprendiz   = a.idAprendiz
        INNER JOIN fichaAprendiz   fa ON fa.idAprendiz   = a.idAprendiz
        INNER JOIN fichas          f  ON fa.idFicha      = f.idFicha
        INNER JOIN fichaInstructor fi ON fi.idFicha      = f.idFicha
        LEFT  JOIN evaluaciones    ev ON ev.idPlanMejoramiento = pm.idPlanMejoramiento
        WHERE  fi.idInstructor = @idInstructor
          AND  pm.estadoPlan   = 'Pendiente'
          AND  EXISTS (
                  SELECT 1 FROM evidencias e2
                  WHERE  e2.idPlanMejoramiento = pm.idPlanMejoramiento
               )
        ORDER BY pm.fechaLimite ASC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
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
        public DataTable MtListarPlanesInternosPorInstructor(int idInstructor, string filtroNombre, string filtroEstado)
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
                a.numeroDocumento             AS docAprendiz,
                f.codigoFicha,
                i.nombres + ' ' + i.apellidos AS nombreInstructor,
                (SELECT COUNT(*) FROM evidencias e
                 WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento) AS totalEvidencias
        FROM    planesMejoramiento pm
        INNER JOIN aprendices      a  ON pm.idAprendiz   = a.idAprendiz
        INNER JOIN fichaAprendiz   fa ON fa.idAprendiz   = a.idAprendiz
        INNER JOIN fichas          f  ON fa.idFicha      = f.idFicha
        INNER JOIN fichaInstructor fi ON fi.idFicha      = f.idFicha
        INNER JOIN instructores    i  ON pm.idInstructor = i.idInstructor
        WHERE  fi.idInstructor = @idInstructor
          AND  pm.tipoPlan     = 'Interno'
          AND  (@filtroEstado  = '' OR pm.estadoPlan = @filtroEstado)
          AND  (@filtroNombre  = '' OR a.nombres + ' ' + a.apellidos LIKE '%' + @filtroNombre + '%'
                                    OR a.numeroDocumento LIKE '%' + @filtroNombre + '%')
        ORDER BY pm.fechaAsignacion DESC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@filtroEstado", filtroEstado ?? "");
            cmd.Parameters.AddWithValue("@filtroNombre", filtroNombre ?? "");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }
        public DataTable MtListarPlanesComitePorInstructor(int idInstructor, string filtroNombre, string filtroEstado)
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
            a.numeroDocumento             AS docAprendiz,
            f.codigoFicha,
            i.nombres + ' ' + i.apellidos AS nombreInstructor,
            (SELECT COUNT(*) FROM evidencias e
             WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento) AS totalEvidencias
    FROM    planesMejoramiento pm
    INNER JOIN aprendices      a  ON pm.idAprendiz   = a.idAprendiz
    INNER JOIN fichaAprendiz   fa ON fa.idAprendiz   = a.idAprendiz
    INNER JOIN fichas          f  ON fa.idFicha      = f.idFicha
    INNER JOIN fichaInstructor fi ON fi.idFicha      = f.idFicha
    INNER JOIN instructores    i  ON pm.idInstructor = i.idInstructor
    WHERE  fi.idInstructor = @idInstructor
      AND  pm.tipoPlan     = 'Comité'
      AND  (@filtroEstado  = '' OR pm.estadoPlan = @filtroEstado)
      AND  (@filtroNombre  = '' OR a.nombres + ' ' + a.apellidos LIKE '%' + @filtroNombre + '%'
                                OR a.numeroDocumento LIKE '%' + @filtroNombre + '%')
    ORDER BY pm.fechaAsignacion DESC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
            cmd.Parameters.AddWithValue("@filtroEstado", filtroEstado ?? "");
            cmd.Parameters.AddWithValue("@filtroNombre", filtroNombre ?? "");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            oConex.MtCerrarConexion();
            return dt;
        }
    }
}
