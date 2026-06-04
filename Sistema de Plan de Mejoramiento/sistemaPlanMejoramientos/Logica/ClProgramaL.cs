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
            if (string.IsNullOrWhiteSpace(codigoPrograma) || string.IsNullOrWhiteSpace(nombre))
                return false;

            return oProgramaD.MtCrearPrograma(codigoPrograma, nombre, version, nivel, duracion, estado, idCentro);
        }

        public DataTable MtListarProgramas()
        {
            return oProgramaD.MtListarProgramas();
        }

        public DataTable MtListarProgramas(string filtro)
        {
            return oProgramaD.MtListarProgramas(filtro ?? "");
        }

        public DataTable MtObtenerProgramaPorId(int idPrograma)
        {
            return oProgramaD.MtObtenerProgramaPorId(idPrograma);
        }

        public bool MtObtenerProgramaPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return false;

            return oProgramaD.MtObtenerProgramaPorCodigo(codigo);
        }

        public bool MtActualizarPrograma(int idPrograma, string codigoPrograma, string nombre,
                                         string version, string nivel, string duracion,
                                         string estado, int idCentro)
        {
            if (idPrograma <= 0 || string.IsNullOrWhiteSpace(codigoPrograma) || string.IsNullOrWhiteSpace(nombre))
                return false;

            return oProgramaD.MtActualizarPrograma(idPrograma, codigoPrograma, nombre, version, nivel, duracion, estado, idCentro);
        }

        public bool MtEliminarPrograma(int idPrograma)
        {
            if (idPrograma <= 0)
                return false;

            return oProgramaD.MtEliminarPrograma(idPrograma);
        }

        public bool MtObtenerProgramaPorCodigoExcluyendo(string codigo, int idPrograma)
        {
            if (string.IsNullOrWhiteSpace(codigo) || idPrograma <= 0) return false;
            return oProgramaD.MtObtenerProgramaPorCodigoExcluyendo(codigo, idPrograma);
        }
    }
}