using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

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

        public List<ClCompetenciasM> MtListarCompetencias()
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

        public List<ClCompetenciasM> MtCargarCompetencias(int idPrograma)
        {
            if (idPrograma <= 0)
                return new List<ClCompetenciasM>();

            return oCompetenciaD.MtCargarCompetencias(idPrograma);
        }
        public List<ClCompetenciasM> MtListaCompetencia()
        {
            return oCompetenciaD.MtListaCompetencia();
        }

        public List<ClCompetenciasM> MtBuscarCompetencias(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return new List<ClCompetenciasM>();

            return oCompetenciaD.MtBuscarCompetencias(filtro);
        }
    }
}