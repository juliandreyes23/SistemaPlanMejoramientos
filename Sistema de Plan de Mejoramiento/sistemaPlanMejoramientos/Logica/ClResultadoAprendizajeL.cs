using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClResultadoAprendizajeL
    {
        ClResultadoAprendizajeD oResultadoD = new ClResultadoAprendizajeD();

        public bool MtCrearResultado(string descripcion, int idCompetencia)
        {
            if (string.IsNullOrWhiteSpace(descripcion) || idCompetencia <= 0)
            {
                return false;
            }
            return oResultadoD.MtCrearResultado(descripcion, idCompetencia);
        }

        public DataTable MtListarResultadoAprendizaje()
        {
            return oResultadoD.MtListarResultadoAprendizaje();
        }

        public DataTable MtCargarPrograma()
        {
            return oResultadoD.MtCargarPrograma();
        }
        

        public bool MtActualizarResultado(int idResultadoAprendizaje, string descripcion, int idCompetencia)
        {
            if (idResultadoAprendizaje <= 0 || string.IsNullOrWhiteSpace(descripcion) || idCompetencia <= 0)
            {
                return false;
            }
            return oResultadoD.MtActualizarResultado(idResultadoAprendizaje, descripcion, idCompetencia);
        }

        public bool MtEliminarResultado(int idResultadoAprendizaje)
        {
            if (idResultadoAprendizaje <= 0)
            {
                return false;
            }
            return oResultadoD.MtEliminarResultado(idResultadoAprendizaje);
        }
    }
}