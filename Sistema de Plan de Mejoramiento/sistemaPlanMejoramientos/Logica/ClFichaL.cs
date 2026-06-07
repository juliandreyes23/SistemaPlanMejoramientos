using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
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

        public List<ClFichasM> MtListarFichas()
        {
            return oFichaD.MtListarFichas();
        }

        public List<ClFichasM> MtListarFichas(string filtro)
        {
            return oFichaD.MtListarFichas(filtro ?? "");
        }

        public bool MtActualizarFicha(int idFicha, string codigoFicha, DateTime fechaInicio,
            DateTime fechaFinalizacion, string jornada, string estado, int idPrograma)
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

        public bool MtExisteFicha(string codigoFicha)
        {
            if (string.IsNullOrWhiteSpace(codigoFicha)) return false;
            return oFichaD.MtExisteFicha(codigoFicha);
        }

        public bool MtExisteFichaEditar(int idFicha, string codigoFicha)
        {
            if (idFicha <= 0 || string.IsNullOrWhiteSpace(codigoFicha)) return false;
            return oFichaD.MtExisteFichaEditar(idFicha, codigoFicha);
        }

        public int MtObtenerIdCentroPorPrograma(int idPrograma)
        {
            if (idPrograma <= 0) return 0;
            return oFichaD.MtObtenerIdCentroPorPrograma(idPrograma);
        }

        public List<ClFichasM> MtListarFichasPorInstructor(int idInstructor)
        {
            if (idInstructor <= 0)
                return new List<ClFichasM>();

            return oFichaD.MtListarFichasPorInstructor(idInstructor);
        }

        public List<ClAprendizM> MtListarAprendicesPorFicha(int idFicha)
        {
            if (idFicha <= 0)
                return new List<ClAprendizM>();

            return oFichaD.MtListarAprendicesPorFicha(idFicha);
        }

        public int MtContarFichasPorInstructor(int idInstructor)
        {
            if (idInstructor <= 0) return 0;
            return oFichaD.MtContarFichasPorInstructor(idInstructor);
        }

        public int MtObtenerIdCentroPorFicha(int idFicha)
        {
            if (idFicha <= 0) return 0;
            return oFichaD.MtObtenerIdCentroPorFicha(idFicha);
        }
    }
}