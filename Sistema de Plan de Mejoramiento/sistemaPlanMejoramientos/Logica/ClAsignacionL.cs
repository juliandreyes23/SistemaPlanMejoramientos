using System;
using System.Data;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClAsignacionL
    {
        ClAsignacionD oAsignacionD = new ClAsignacionD();

        public DataTable MtListarInstructores() => oAsignacionD.MtListarInstructoresCombo();
        public DataTable MtListarFichas() => oAsignacionD.MtListarFichasCombo();
        public DataTable MtListarAsignaciones() => oAsignacionD.MtListarAsignaciones();

        public bool MtAsignarInstructorFicha(int idInstructor, int idFicha)
        {
            if (idInstructor <= 0 || idFicha <= 0) return false;

            return oAsignacionD.MtRegistrarAsignacion(idInstructor, idFicha);
        }

        public bool MtEliminarAsignacion(int idInstructorFicha)
        {
            if (idInstructorFicha <= 0) return false;
            return oAsignacionD.MtEliminarAsignacion(idInstructorFicha);
        }
    }
}