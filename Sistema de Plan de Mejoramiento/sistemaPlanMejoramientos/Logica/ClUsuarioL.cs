using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClUsuarioL
    {
        ClUsuarioD oUsuarioD = new ClUsuarioD();

        private bool ValidarFormatoCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return false;
            string expresion = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, expresion);
        }

        public bool MtExisteCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return false;
            return oUsuarioD.MtExisteCorreo(correo);
        }

        public int MtCrearUsuarioConRetorno(string correo, string documento, int idRol)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(documento))
                return 0;
            if (oUsuarioD.MtExisteCorreo(correo)) return -1;
            return oUsuarioD.MtCrearUsuarioConRetorno(correo, documento, idRol);
        }

        public bool MtCrearUsuario(string correo, string password, int idRol)
        {
            if (!ValidarFormatoCorreo(correo) || string.IsNullOrWhiteSpace(password) || idRol <= 0)
                return false;
            if (oUsuarioD.MtExisteCorreo(correo))
                return false;
            return oUsuarioD.MtCrearUsuario(correo, password, idRol);
        }

        public int MtCrearUsuarioInstructor(string correo, string documento)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(documento))
                return 0;
            if (oUsuarioD.MtExisteCorreo(correo))
                return -1;
            return oUsuarioD.MtCrearUsuarioInstructor(correo, documento);
        }

        public List<ClUsuarioM> MtListarUsuarios()
        {
            return oUsuarioD.MtListarUsuarios() ?? new List<ClUsuarioM>();
        }

        public List<ClUsuarioM> MtListarUsuarios(string filtro)
        {
            return oUsuarioD.MtListarUsuarios(filtro) ?? new List<ClUsuarioM>();
        }

        public bool MtActualizarUsuario(int idUsuario, string correo, string password, int idRol)
        {
            if (idUsuario <= 0 || !ValidarFormatoCorreo(correo) || idRol <= 0)
                return false;
            return oUsuarioD.MtActualizarUsuario(idUsuario, correo, password, idRol);
        }

        public bool MtEliminarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                return false;
            return oUsuarioD.MtEliminarUsuario(idUsuario);
        }

        public ClUsuarioM MtLogin(string correo, string password)
        {
            if (!ValidarFormatoCorreo(correo) || string.IsNullOrWhiteSpace(password))
                return null;

            return oUsuarioD.MtLogin(correo, password);
        }

        public ClUsuarioM MtBuscarUsuarioPorId(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            return oUsuarioD.MtBuscarUsuarioPorId(idUsuario);
        }

        public bool MtSolicitarRecuperacion(string correo)
        {
            if (!ValidarFormatoCorreo(correo))
                return false;
            return oUsuarioD.MtSolicitarRecuperacion(correo);
        }

        public bool MtRestablecerContrasena(string token, string nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Length < 4)
                return false;
            return oUsuarioD.MtRestablecerContrasena(token, nuevaPassword);
        }

        public int MtObtenerIdCentroAdmin(int idUsuario)
        {
            if (idUsuario <= 0) return 0;
            return oUsuarioD.MtObtenerIdCentroAdmin(idUsuario);
        }

        public int MtObtenerIdInstructor(int idUsuario)
        {
            if (idUsuario <= 0) return 0;
            return oUsuarioD.MtObtenerIdInstructor(idUsuario);
        }

        public int MtObtenerIdAprendiz(int idUsuario)
        {
            if (idUsuario <= 0) return 0;
            return oUsuarioD.MtObtenerIdAprendiz(idUsuario);
        }
    }
}