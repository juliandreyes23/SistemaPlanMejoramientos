using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Datos
{
    public class ClEvidenciaD
    {
        ClConexion oConex = new ClConexion();

        public bool MtRegistrarEvidencia(int idPlanMejoramiento, string nombreArchivo, string rutaArchivo, DateTime fechaSubida, string tipoArchivo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"INSERT INTO evidencias (idPlanMejoramiento, nombreArchivo, rutaArchivo, fechaSubida, tipoArchivo)
                            VALUES (@idPlanMejoramiento, @nombreArchivo, @rutaArchivo, @fechaSubida, @tipoArchivo)";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);
            cmd.Parameters.AddWithValue("@nombreArchivo", nombreArchivo);
            cmd.Parameters.AddWithValue("@rutaArchivo", rutaArchivo);
            cmd.Parameters.AddWithValue("@fechaSubida", fechaSubida);
            cmd.Parameters.AddWithValue("@tipoArchivo", tipoArchivo);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtListarEvidenciaPorPlan(int idPlanMejoramiento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idEvidencia, idPlanMejoramiento, nombreArchivo, rutaArchivo, fechaSubida, tipoArchivo
                            FROM evidencias
                            WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public bool MtSobrescribirEvidencia(int idPlanMejoramiento, string nombreArchivo, string rutaArchivo, DateTime fechaSubida, string tipoArchivo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE evidencias 
                            SET nombreArchivo = @nombreArchivo, rutaArchivo = @rutaArchivo,
                                fechaSubida = @fechaSubida, tipoArchivo = @tipoArchivo
                            WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);
            cmd.Parameters.AddWithValue("@nombreArchivo", nombreArchivo);
            cmd.Parameters.AddWithValue("@rutaArchivo", rutaArchivo);
            cmd.Parameters.AddWithValue("@fechaSubida", fechaSubida);
            cmd.Parameters.AddWithValue("@tipoArchivo", tipoArchivo);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtCalificarEvidencia(int idPlanMejoramiento, string criterioProducto, string criterioConocimiento, string criterioDesempeno)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"IF EXISTS (SELECT 1 FROM evaluaciones WHERE idPlanMejoramiento = @idPlanMejoramiento)
                             BEGIN
                                 UPDATE evaluaciones 
                                 SET criterioProducto = @criterioProducto,
                                     criterioConocimiento = @criterioConocimiento,
                                     criterioDesempeno = @criterioDesempeno
                                 WHERE idPlanMejoramiento = @idPlanMejoramiento
                             END
                             ELSE
                             BEGIN
                                 INSERT INTO evaluaciones (idPlanMejoramiento, criterioProducto, criterioConocimiento, criterioDesempeno, observaciones)
                                 VALUES (@idPlanMejoramiento, @criterioProducto, @criterioConocimiento, @criterioDesempeno, NULL)
                             END";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);
            cmd.Parameters.AddWithValue("@criterioProducto", criterioProducto);
            cmd.Parameters.AddWithValue("@criterioConocimiento", criterioConocimiento);
            cmd.Parameters.AddWithValue("@criterioDesempeno", criterioDesempeno);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public bool MtRegistrarObservacionesEvidencia(int idPlanMejoramiento, string observaciones)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"IF EXISTS (SELECT 1 FROM evaluaciones WHERE idPlanMejoramiento = @idPlanMejoramiento)
                             BEGIN
                                 UPDATE evaluaciones 
                                 SET observaciones = @observaciones 
                                 WHERE idPlanMejoramiento = @idPlanMejoramiento
                             END
                             ELSE
                             BEGIN
                                 INSERT INTO evaluaciones (idPlanMejoramiento, criterioProducto, criterioConocimiento, criterioDesempeno, observaciones)
                                 VALUES (@idPlanMejoramiento, NULL, NULL, NULL, @observaciones)
                             END";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

            if (string.IsNullOrEmpty(observaciones))
                cmd.Parameters.AddWithValue("@observaciones", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@observaciones", observaciones);

            int filas = cmd.ExecuteNonQuery();
            oConex.MtCerrarConexion();
            return filas > 0;
        }

        public DataTable MtObtenerAprendizPorUsuario(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT a.idAprendiz, a.nombres, a.apellidos, a.estadoAcademico,
                                    a.tipoDocumento, a.numeroDocumento, a.correo, a.telefono,
                                    ISNULL(CAST(f.codigoFicha AS NVARCHAR), 'Sin ficha') AS codigoFicha,
                                    ISNULL(p.nombre, 'Sin programa') AS nombrePrograma,
                                    ISNULL(f.jornada, '—') AS jornada
                             FROM aprendices a
                             LEFT JOIN fichas f ON a.idFicha = f.idFicha
                             LEFT JOIN programas p ON f.idPrograma = p.idPrograma
                             WHERE a.idUsuario = @idUsuario";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public int MtContarPlanesPorEstado(int idAprendiz, string estado)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT COUNT(*) 
                            FROM planesMejoramiento
                            WHERE idAprendiz = @idAprendiz 
                              AND estadoPlan = @estado";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);
            cmd.Parameters.AddWithValue("@estado", estado);

            int resultado = (int)cmd.ExecuteScalar();
            oConex.MtCerrarConexion();
            return resultado;
        }

        public DataTable MtListarPlanesPorAprendiz(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT 
                                pm.idPlanMejoramiento,
                                pm.tipoPlan,
                                pm.fechaAsignacion,
                                pm.fechaLimite,
                                pm.actividades,
                                pm.observaciones,
                                pm.estadoPlan,
                                i.nombres + ' ' + i.apellidos AS nombreInstructor,
                                (SELECT COUNT(*) FROM evidencias e 
                                 WHERE e.idPlanMejoramiento = pm.idPlanMejoramiento) AS totalEvidencias,
                                ev.criterioProducto,
                                ev.criterioConocimiento,
                                ev.criterioDesempeno,
                                ev.observaciones AS observacionesEvaluacion
                            FROM planesMejoramiento pm
                            INNER JOIN instructores i ON pm.idInstructor = i.idInstructor
                            LEFT JOIN evaluaciones ev ON ev.idPlanMejoramiento = pm.idPlanMejoramiento
                            WHERE pm.idAprendiz = @idAprendiz
                            ORDER BY pm.fechaAsignacion DESC";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idAprendiz", idAprendiz);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public DataTable MtObtenerPlanPorId(int idPlan)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT pm.idPlanMejoramiento, pm.tipoPlan, pm.actividades,
                                    pm.fechaLimite, pm.estadoPlan, pm.observaciones,
                                    i.nombres + ' ' + i.apellidos AS nombreInstructor
                             FROM planesMejoramiento pm
                             INNER JOIN instructores i ON pm.idInstructor = i.idInstructor
                             WHERE pm.idPlanMejoramiento = @idPlan";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlan", idPlan);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }

        public DataTable MtListarResultadosPorPlan(int idPlan)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT ra.idResultadoAprendizaje,
                                    ra.descripcion
                             FROM planResultados pr
                             INNER JOIN resultadoAprendizaje ra 
                                 ON pr.idResultadoAprendizaje = ra.idResultadoAprendizaje
                             WHERE pr.idPlanMejoramiento = @idPlan";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlan", idPlan);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            oConex.MtCerrarConexion();
            return dt;
        }
    }
}