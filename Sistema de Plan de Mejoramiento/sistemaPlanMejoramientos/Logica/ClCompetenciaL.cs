using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClCompetenciaL
    {
        ClCompetenciaD oCompetenciaD = new ClCompetenciaD();

        public bool MtCrearCompetencia(string descripcion, int idPrograma)
        {
            if (string.IsNullOrWhiteSpace(descripcion) || idPrograma <= 0)
            {
                return false;
            }
            return oCompetenciaD.MtCrearCompetencia(descripcion, idPrograma);
        }

        public DataTable MtListarCompetencias()
        {
            return oCompetenciaD.MtListarCompetencias();
        }

        public bool MtActualizarCompetencia(int idCompetencia, string descripcion, int idPrograma)
        {
            if (idCompetencia <= 0 || string.IsNullOrWhiteSpace(descripcion) || idPrograma <= 0)
            {
                return false;
            }
            return oCompetenciaD.MtActualizarCompetencia(idCompetencia, descripcion, idPrograma);
        }

        public bool MtEliminarCompetencia(int idCompetencia)
        {
            if (idCompetencia <= 0)
            {
                return false;
            }
            return oCompetenciaD.MtEliminarCompetencia(idCompetencia);
        }

        public DataTable MtCargarCompetencia(int idPrograma)
        {
            return oCompetenciaD.MtCargarCompetencias(idPrograma);
        }
        public DataTable MListarCompetencia()
        {
            return oCompetenciaD.MtListaCompetencia();
        }

        public DataTable MtBuscarCompetencias(string filtro)
        {
            return oCompetenciaD.MtBuscarCompetencias(filtro);
        }
    }
}