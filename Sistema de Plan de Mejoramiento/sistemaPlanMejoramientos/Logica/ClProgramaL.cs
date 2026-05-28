using sistemaPlanMejoramientos.Datos;
using System.Data;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClProgramaL
    {
        ClProgramaD oProgramaD = new ClProgramaD();

        public bool MtCrearPrograma(string codigoPrograma, string nombre, string version,
                                    string nivel, string duracion, string estado, int idCentro)
        {
            return oProgramaD.MtCrearPrograma(codigoPrograma, nombre, version, nivel, duracion, estado, idCentro);
        }

        public DataTable MtListarProgramas(string filtro)
        {
            return oProgramaD.MtListarProgramas(filtro);
        }

        public DataTable MtObtenerProgramaPorId(int idPrograma)
        {
            return oProgramaD.MtObtenerProgramaPorId(idPrograma);
        }

        public bool MtActualizarPrograma(int idPrograma, string codigoPrograma, string nombre,
                                         string version, string nivel, string duracion,
                                         string estado, int idCentro)
        {
            return oProgramaD.MtActualizarPrograma(idPrograma, codigoPrograma, nombre, version, nivel, duracion, estado, idCentro);
        }

        public bool MtEliminarPrograma(int idPrograma)
        {
            return oProgramaD.MtEliminarPrograma(idPrograma);
        }
    }
}