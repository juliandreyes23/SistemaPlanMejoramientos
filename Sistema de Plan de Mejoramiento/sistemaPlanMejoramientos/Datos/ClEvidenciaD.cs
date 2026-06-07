using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using sistemaPlanMejoramientos.Modelo;

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

        public List<ClEvidenciaM> MtListarEvidenciaPorPlan(int idPlanMejoramiento)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT idEvidencia,
                                    idPlanMejoramiento,
                                    nombreArchivo,
                                    rutaArchivo,
                                    fechaSubida,
                                    tipoArchivo
                             FROM evidencias
                             WHERE idPlanMejoramiento = @idPlanMejoramiento";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlanMejoramiento", idPlanMejoramiento);

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClEvidenciaM> lista = new List<ClEvidenciaM>();

            while (rd.Read())
            {
                lista.Add(new ClEvidenciaM
                {
                    idEvidencia = Convert.ToInt32(rd["idEvidencia"]),
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    nombreArchivo = rd["nombreArchivo"].ToString(),
                    rutaArchivo = rd["rutaArchivo"].ToString(),
                    fechaSubida = Convert.ToDateTime(rd["fechaSubida"]),
                    tipoArchivo = rd["tipoArchivo"].ToString()
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public bool MtSobrescribirEvidencia(int idPlanMejoramiento, string nombreArchivo, string rutaArchivo, DateTime fechaSubida, string tipoArchivo)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"UPDATE evidencias
                             SET nombreArchivo = @nombreArchivo,
                                 rutaArchivo = @rutaArchivo,
                                 fechaSubida = @fechaSubida,
                                 tipoArchivo = @tipoArchivo
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
                                 INSERT INTO evaluaciones
                                 (idPlanMejoramiento, criterioProducto, criterioConocimiento, criterioDesempeno, observaciones)
                                 VALUES
                                 (@idPlanMejoramiento, @criterioProducto, @criterioConocimiento, @criterioDesempeno, NULL)
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
                                 INSERT INTO evaluaciones
                                 (idPlanMejoramiento, criterioProducto, criterioConocimiento, criterioDesempeno, observaciones)
                                 VALUES
                                 (@idPlanMejoramiento, NULL, NULL, NULL, @observaciones)
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

        public ClAprendizM MtObtenerAprendizPorUsuario(int idUsuario)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT a.idAprendiz,
                                    a.nombres,
                                    a.apellidos,
                                    a.estadoAcademico,
                                    a.tipoDocumento,
                                    a.numeroDocumento,
                                    a.correo,
                                    a.telefono,
                                    f.idFicha,
                                    f.codigoFicha,
                                    f.jornada,
                                    p.idPrograma,
                                    p.nombre
                             FROM aprendices a
                             LEFT JOIN fichas f ON a.idFicha = f.idFicha
                             LEFT JOIN programas p ON f.idPrograma = p.idPrograma
                             WHERE a.idUsuario = @idUsuario";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            SqlDataReader rd = cmd.ExecuteReader();

            ClAprendizM aprendiz = null;

            if (rd.Read())
            {
                aprendiz = new ClAprendizM
                {
                    idAprendiz = Convert.ToInt32(rd["idAprendiz"]),
                    nombres = rd["nombres"].ToString(),
                    apellidos = rd["apellidos"].ToString(),
                    estadoAcademico = rd["estadoAcademico"].ToString(),
                    tipoDocumento = rd["tipoDocumento"].ToString(),
                    numeroDocumento = rd["numeroDocumento"].ToString(),
                    correo = rd["correo"].ToString(),
                    telefono = rd["telefono"].ToString(),
                    idFicha = rd["idFicha"] != DBNull.Value ? Convert.ToInt32(rd["idFicha"]) : 0,
                    ficha = rd["idFicha"] != DBNull.Value
                        ? new ClFichasM
                        {
                            idFicha = Convert.ToInt32(rd["idFicha"]),
                            codigoFicha = rd["codigoFicha"].ToString(),
                            jornada = rd["jornada"].ToString(),
                            programa = new ClProgramasM
                            {
                                idPrograma = rd["idPrograma"] != DBNull.Value ? Convert.ToInt32(rd["idPrograma"]) : 0,
                                nombre = rd["nombre"].ToString()
                            }
                        }
                        : null
                };
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return aprendiz;
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

        public List<ClPlanMejoramientoM> MtListarPlanesPorAprendiz(int idAprendiz)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT pm.idPlanMejoramiento,
                        pm.tipoPlan,
                        pm.fechaAsignacion,
                        pm.fechaLimite,
                        pm.actividades,
                        pm.observaciones,
                        pm.estadoPlan,
                        i.idInstructor,
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
                    observaciones = rd["observaciones"] == DBNull.Value ? null : rd["observaciones"].ToString(),
                    estadoPlan = rd["estadoPlan"].ToString(),
                    nombreInstructor = rd["nombreInstructor"].ToString(),
                    totalEvidencias = Convert.ToInt32(rd["totalEvidencias"]),
                    criterioProducto = rd["criterioProducto"] == DBNull.Value ? null : rd["criterioProducto"].ToString(),
                    criterioConocimiento = rd["criterioConocimiento"] == DBNull.Value ? null : rd["criterioConocimiento"].ToString(),
                    criterioDesempeno = rd["criterioDesempeno"] == DBNull.Value ? null : rd["criterioDesempeno"].ToString(),
                    observacionesEvaluacion = rd["observacionesEvaluacion"] == DBNull.Value ? null : rd["observacionesEvaluacion"].ToString(),
                    instructor = new ClInstructoresM
                    {
                        idInstructor = Convert.ToInt32(rd["idInstructor"]),
                        nombres = rd["nombreInstructor"].ToString()
                    }
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }

        public ClPlanMejoramientoM MtObtenerPlanPorId(int idPlan)
        {
            SqlConnection cn = oConex.MtAbrirConexion();

            string query = @"SELECT pm.idPlanMejoramiento,
                                    pm.tipoPlan,
                                    pm.actividades,
                                    pm.fechaLimite,
                                    pm.estadoPlan,
                                    pm.observaciones,
                                    i.idInstructor,
                                    i.nombres,
                                    i.apellidos
                             FROM planesMejoramiento pm
                             INNER JOIN instructores i
                             ON pm.idInstructor = i.idInstructor
                             WHERE pm.idPlanMejoramiento = @idPlan";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@idPlan", idPlan);

            SqlDataReader rd = cmd.ExecuteReader();

            ClPlanMejoramientoM plan = null;

            if (rd.Read())
            {
                plan = new ClPlanMejoramientoM
                {
                    idPlanMejoramiento = Convert.ToInt32(rd["idPlanMejoramiento"]),
                    tipoPlan = rd["tipoPlan"].ToString(),
                    actividades = rd["actividades"].ToString(),
                    fechaLimite = Convert.ToDateTime(rd["fechaLimite"]),
                    estadoPlan = rd["estadoPlan"].ToString(),
                    observaciones = rd["observaciones"].ToString(),
                    instructor = new ClInstructoresM
                    {
                        idInstructor = Convert.ToInt32(rd["idInstructor"]),
                        nombres = rd["nombres"].ToString(),
                        apellidos = rd["apellidos"].ToString()
                    }
                };
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return plan;
        }

        public List<ClResultadoAprendizajeM> MtListarResultadosPorPlan(int idPlan)
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

            SqlDataReader rd = cmd.ExecuteReader();

            List<ClResultadoAprendizajeM> lista = new List<ClResultadoAprendizajeM>();

            while (rd.Read())
            {
                lista.Add(new ClResultadoAprendizajeM
                {
                    idResultadoAprendizaje = Convert.ToInt32(rd["idResultadoAprendizaje"]),
                    descripcion = rd["descripcion"].ToString()
                });
            }

            rd.Close();
            oConex.MtCerrarConexion();

            return lista;
        }
    }
}