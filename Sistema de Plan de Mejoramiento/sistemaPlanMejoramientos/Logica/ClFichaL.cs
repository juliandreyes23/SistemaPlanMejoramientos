using sistemaPlanMejoramientos.Datos;
using System;
using System.Data;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClFichaL
    {
        ClFichaD oFichaD = new ClFichaD();

        public bool MtCrearFicha(string codigoFicha, DateTime fechaInicio, DateTime fechaFinalizacion,
                          string jornada, string estado, int idPrograma)
        {
            if (string.IsNullOrWhiteSpace(codigoFicha) || string.IsNullOrWhiteSpace(jornada) ||
                string.IsNullOrWhiteSpace(estado) || idPrograma <= 0)
                return false;

            int idCentro = oFichaD.MtObtenerIdCentroPorPrograma(idPrograma);
            if (idCentro <= 0) return false;

            return oFichaD.MtCrearFicha(codigoFicha, fechaInicio, fechaFinalizacion, jornada, estado, idPrograma, idCentro);
        }

        public DataTable MtListarFichas()
        {
            return oFichaD.MtListarFichas();
        }

        public bool MtActualizarFichas(int idFicha, string codigoFicha, DateTime fechaInicio, DateTime fechaFinalizacion, string jornada, string estado, int idPrograma)
        {
            if (idFicha <= 0 || string.IsNullOrWhiteSpace(codigoFicha) ||
                string.IsNullOrWhiteSpace(jornada) || string.IsNullOrWhiteSpace(estado) || idPrograma <= 0)
                return false;
            return oFichaD.MtActualizarFicha(idFicha, codigoFicha, fechaInicio, fechaFinalizacion, jornada, estado, idPrograma);
        }

        public bool MtEliminarFicha(int idFicha)
        {
            if (idFicha <= 0) return false;
            return oFichaD.MtEliminarFicha(idFicha);
        }

        public DataTable MtListarFichasPorInstructor(int idInstructor)
        {
            if (idInstructor <= 0) return new DataTable();
            return oFichaD.MtListarFichasPorInstructor(idInstructor);
        }

        public DataTable MtListarAprendicesPorFicha(int idFicha)
        {
            if (idFicha <= 0) return new DataTable();
            return oFichaD.MtListarAprendicesPorFicha(idFicha);
        }

        public int MtContarFichasPorInstructor(int idInstructor)
        {
            if (idInstructor <= 0) return 0;
            return oFichaD.MtContarFichasPorInstructor(idInstructor);
        }
    }
}