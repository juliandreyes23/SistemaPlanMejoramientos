using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClInstructorL
    {
        ClInstructorD oInstructorD = new ClInstructorD();

        public bool MtCrearInstructor(string tipoDocumento, string numeroDocumento, string nombres, string apellidos, string correo, string telefono, string especialidad, int idUsuario, int idCentro)
        {
            if (string.IsNullOrWhiteSpace(tipoDocumento) || string.IsNullOrWhiteSpace(numeroDocumento) ||
                string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(correo) || idUsuario <= 0 || idCentro <= 0)
                return false;

            return oInstructorD.MtCrearInstructor(tipoDocumento, numeroDocumento, nombres, apellidos, correo, telefono, especialidad, idUsuario, idCentro);
        }

        public DataTable MtListarInstructores()
        {
            return oInstructorD.MtListarInstructores();
        }

        public bool MtActualizarInstructor(int idInstructor, string nombres, string apellidos, string correo, string telefono, string especialidad, int idCentro)
        {
            if (idInstructor <= 0 || string.IsNullOrWhiteSpace(nombres) ||
                string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(correo) || idCentro <= 0)
                return false;

            return oInstructorD.MtActualizarInstructor(idInstructor, nombres, apellidos, correo, telefono, especialidad, idCentro);
        }

        public bool MtEliminarInstructor(int idInstructor)
        {
            if (idInstructor <= 0) return false;
            return oInstructorD.MtEliminarInstructor(idInstructor);
        }

        public bool MtAsignarInstructorAFicha(int idInstructor, int idFicha)
        {
            if (idInstructor <= 0 || idFicha <= 0) return false;
            return oInstructorD.MtAsignarInstructorAFicha(idInstructor, idFicha);
        }
    }
}