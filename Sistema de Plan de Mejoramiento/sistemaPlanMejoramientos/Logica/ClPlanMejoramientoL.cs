using System;
using System.Collections.Generic;
using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClPlanMejoramientoL
    {
        ClPlanMejoramientoD oPlanD = new ClPlanMejoramientoD();
        ClEvidenciaD oEvidenciaD = new ClEvidenciaD();

        public int MtCrearPlanMejoramiento(string tipoPlan, DateTime fechaAsignacion, DateTime fechaLimite,
                                           string actividades, string observaciones, string estadoPlan,
                                           int idAprendiz, int idInstructor)
        {
            if (string.IsNullOrWhiteSpace(tipoPlan) || string.IsNullOrWhiteSpace(actividades) ||
                string.IsNullOrWhiteSpace(estadoPlan) || idAprendiz <= 0 || idInstructor <= 0)
                return 0;

            if (fechaLimite < fechaAsignacion)
                return 0;

            return oPlanD.MtCrearPlanMejoramiento(tipoPlan, fechaAsignacion, fechaLimite,
                                                  actividades, observaciones, estadoPlan,
                                                  idAprendiz, idInstructor);
        }

        public bool MtAsociarResultadoAPlan(int idPlanMejoramiento, int idResultadoAprendizaje)
        {
            if (idPlanMejoramiento <= 0 || idResultadoAprendizaje <= 0) return false;
            return oPlanD.MtAsociarResultadoAPlan(idPlanMejoramiento, idResultadoAprendizaje);
        }

        public List<ClPlanMejoramientoM> MtListarPlanes()
        {
            return oPlanD.MtListarPlanes();
        }

        public bool MtActualizarEstadoPlan(int idPlanMejoramiento, string estadoPlan, string observaciones)
        {
            if (idPlanMejoramiento <= 0 || string.IsNullOrWhiteSpace(estadoPlan)) return false;
            return oPlanD.MtActualizarEstadoPlan(idPlanMejoramiento, estadoPlan, observaciones);
        }

        public bool MtEliminarPlan(int idPlanMejoramiento)
        {
            if (idPlanMejoramiento <= 0) return false;
            return oPlanD.MtEliminarPlan(idPlanMejoramiento);
        }

        public int MtContarPlanesPorTipo(int idInstructor, string tipoPlan)
        {
            if (idInstructor <= 0 || string.IsNullOrWhiteSpace(tipoPlan)) return 0;
            return oPlanD.MtContarPlanesPorTipo(idInstructor, tipoPlan);
        }

        public List<ClAprendizM> MtListarAprendicesPorInstructor(int idInstructor)
        {
            if (idInstructor <= 0) return new List<ClAprendizM>();
            return oPlanD.MtListarAprendicesPorInstructor(idInstructor);
        }

        public List<ClResultadoAprendizajeM> MtListarResultadosPorFicha(int idFicha)
        {
            if (idFicha <= 0) return new List<ClResultadoAprendizajeM>();
            return oPlanD.MtListarResultadosPorFicha(idFicha);
        }

        public bool MtExistePlanComitePendiente(int idAprendiz)
        {
            if (idAprendiz <= 0) return false;
            return oPlanD.MtExistePlanComitePendiente(idAprendiz);
        }

        public List<ClPlanMejoramientoM> MtListarPlanesPendientesEvaluacion(int idInstructor)
        {
            if (idInstructor <= 0) return new List<ClPlanMejoramientoM>();
            return oPlanD.MtListarPlanesPendientesEvaluacion(idInstructor);
        }

        public bool MtCancelarAprendiz(int idAprendiz)
        {
            return oPlanD.MtCancelarAprendiz(idAprendiz);
        }

        public string MtEvaluarPlan(int idPlanMejoramiento, int idAprendiz, int idInstructor,
                                    string producto, string conocimiento, string desempeno,
                                    string observaciones, string tipoPlan, DateTime fechaLimite)
        {
            if (idPlanMejoramiento <= 0 || idAprendiz <= 0 || idInstructor <= 0)
                return "Error";

            bool vencido = DateTime.Now.Date > fechaLimite.Date;

            if (vencido)
            {
                producto = "No Aprobado";
                conocimiento = "No Aprobado";
                desempeno = "No Aprobado";
            }

            bool calificado = oEvidenciaD.MtCalificarEvidencia(
                idPlanMejoramiento, producto, conocimiento, desempeno);

            if (!string.IsNullOrWhiteSpace(observaciones))
                oEvidenciaD.MtRegistrarObservacionesEvidencia(idPlanMejoramiento, observaciones);

            if (!calificado) return "Error";

            bool aprobado = producto == "Aprobado" &&
                            conocimiento == "Aprobado" &&
                            desempeno == "Aprobado";

            if (aprobado)
            {
                oPlanD.MtActualizarEstadoPlan(idPlanMejoramiento, "Aprobado", observaciones);
                return "Aprobado";
            }

            oPlanD.MtActualizarEstadoPlan(idPlanMejoramiento, "No Aprobado", observaciones);

            if (tipoPlan == "Interno")
            {
                bool yaExisteComite = oPlanD.MtExistePlanComitePendiente(idAprendiz);

                if (!yaExisteComite)
                {
                    DateTime hoy = DateTime.Now;

                    int idComite = oPlanD.MtCrearPlanMejoramiento(
                        "Comité",
                        hoy,
                        hoy.AddDays(15),
                        "Plan generado automáticamente por no aprobación del plan interno.",
                        "Generado automáticamente por el sistema.",
                        "Pendiente",
                        idAprendiz,
                        idInstructor
                    );

                    if (idComite > 0)
                        MtCopiarResultadosAlComite(idPlanMejoramiento, idComite);
                }

                return "Comite";
            }
            else if (tipoPlan == "Comité")
            {
                oPlanD.MtCancelarAprendiz(idAprendiz);
                return "Cancelado";
            }

            return "Error";
        }

        private void MtCopiarResultadosAlComite(int idPlanInterno, int idPlanComite)
        {

            var resultados = oPlanD.MtListarResultadosPorFicha(idPlanInterno);

            foreach (var r in resultados)
            {
                oPlanD.MtAsociarResultadoAPlan(idPlanComite, r.idResultadoAprendizaje);
            }
        }

        public List<ClPlanMejoramientoM> MtListarPlanesInternosPorInstructor(int idInstructor, string filtroNombre, string filtroEstado)
        {
            if (idInstructor <= 0) return new List<ClPlanMejoramientoM>();
            return oPlanD.MtListarPlanesInternosPorInstructor(idInstructor, filtroNombre, filtroEstado);
        }

        public List<ClPlanMejoramientoM> MtListarPlanesComitePorInstructor(int idInstructor, string filtroNombre, string filtroEstado)
        {
            if (idInstructor <= 0) return new List<ClPlanMejoramientoM>();
            return oPlanD.MtListarPlanesComitePorInstructor(idInstructor, filtroNombre, filtroEstado);
        }
    }
}