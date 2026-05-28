using System;
using System.Data;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClEvaluacionL
    {
        ClEvaluacionD oEvaluacionD = new ClEvaluacionD();
        ClEvidenciaD oEvidenciaD = new ClEvidenciaD();
        ClPlanMejoramientoD oPlanD = new ClPlanMejoramientoD();

        public DataTable MtConsultarEvaluacionPorPlan(int idPlanMejoramiento)
        {
            if (idPlanMejoramiento <= 0) return new DataTable();
            return oEvaluacionD.MtConsultarEvaluacionPorPlan(idPlanMejoramiento);
        }

        public string MtEvaluarPlan(int idPlanMejoramiento, int idAprendiz, int idInstructor,
                                     string producto, string conocimiento, string desempeno,
                                     string observaciones)
        {
            if (idPlanMejoramiento <= 0 || idAprendiz <= 0 || idInstructor <= 0)
                return "Error";

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
            else
            {
                oPlanD.MtActualizarEstadoPlan(idPlanMejoramiento, "No Aprobado", observaciones);

                bool yaExisteComite = MtYaExistePlanComite(idAprendiz, idPlanMejoramiento);

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
        }

        private bool MtYaExistePlanComite(int idAprendiz, int idPlanInterno)
        {
            return oPlanD.MtExistePlanComitePendiente(idAprendiz);
        }

        private void MtCopiarResultadosAlComite(int idPlanInterno, int idPlanComite)
        {
            ClEvidenciaD ev = new ClEvidenciaD();
            DataTable resultados = ev.MtListarResultadosPorPlan(idPlanInterno);
            foreach (DataRow row in resultados.Rows)
            {
                int idResultado = Convert.ToInt32(row["idResultadoAprendizaje"]);
                oPlanD.MtAsociarResultadoAPlan(idPlanComite, idResultado);
            }
        }
    }
}