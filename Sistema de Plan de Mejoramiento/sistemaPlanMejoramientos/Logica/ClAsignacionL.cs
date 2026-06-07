using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClAsignacionL
    {
        ClAsignacionD oAsignacionD = new ClAsignacionD();

        public List<ClInstructoresM> MtListarInstructores()
        {
            return oAsignacionD.MtListarInstructoresCombo();
        }

        public List<ClFichasM> MtListarFichas()
        {
            return oAsignacionD.MtListarFichasCombo();
        }

        public List<ClFichaInstructorM> MtListarAsignaciones()
        {
            return oAsignacionD.MtListarAsignaciones();
        }

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