using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClAprendizL
    {
        ClAprendizD oAprendizD = new ClAprendizD();

        public bool MtCrearAprendiz(string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string estadoAcademico, int idUsuario, int idFicha,int idCentro)
        {
            if (string.IsNullOrWhiteSpace(tipoDocumento) || string.IsNullOrWhiteSpace(numeroDocumento) ||
                string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(correo) || idUsuario <= 0 || idFicha <= 0)
            {
                return false;
            }
            return oAprendizD.MtCrearAprendiz(tipoDocumento, numeroDocumento, nombres, apellidos, correo, telefono, estadoAcademico, idUsuario, idFicha,idCentro);
        }

        public DataTable MtListarAprendices()
        {
            return oAprendizD.MtListarAprendices();
        }

        public DataTable MtListarAprendices(string filtro)
        {
            return oAprendizD.MtListarAprendices(filtro);
        }

        public bool MtActualizarAprendiz(int idAprendiz, string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string estadoAcademico, int idFicha)
        {
            if (idAprendiz <= 0 ||
                string.IsNullOrWhiteSpace(tipoDocumento) || string.IsNullOrWhiteSpace(numeroDocumento) ||
                string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(correo) || idFicha <= 0)
            {
                return false;
            }
            return oAprendizD.MtActualizarAprendiz(idAprendiz, tipoDocumento, numeroDocumento, nombres, apellidos, correo, telefono, estadoAcademico, idFicha);
        }

        public bool MtEliminarAprendiz(int idAprendiz)
        {
            if (idAprendiz <= 0) return false;
            return oAprendizD.MtEliminarAprendiz(idAprendiz);
        }

        public bool MtRegistrarFichaIntermedia(int idFicha, int idAprendiz)
        {
            if (idFicha <= 0 || idAprendiz <= 0) return false;
            return oAprendizD.MtRegistrarFichaIntermedia(idFicha, idAprendiz);
        }

        public bool MtCargaMasivaAprendices(DataTable dtAprendicesExcel)
        {
            if (dtAprendicesExcel == null || dtAprendicesExcel.Rows.Count == 0) return false;
            return oAprendizD.MtCargaMasivaAprendices(dtAprendicesExcel);
        }

        public int MtObtenerIdFichaPorCodigo(string codigoFicha)
        {
            return oAprendizD.MtObtenerIdFichaPorCodigo(codigoFicha);
        }


        public bool MtExisteUsuarioPorCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return false;
            return oAprendizD.MtExisteUsuarioPorCorreo(correo);
        }

        public int MtCrearUsuarioAprendiz(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena)) return 0;
            return oAprendizD.MtCrearUsuarioAprendiz(correo, contrasena);
        }
        public bool MtExisteAprendiz(string numeroDocumento)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento)) return false;
            return oAprendizD.MtExisteAprendiz(numeroDocumento);
        }
    }
}