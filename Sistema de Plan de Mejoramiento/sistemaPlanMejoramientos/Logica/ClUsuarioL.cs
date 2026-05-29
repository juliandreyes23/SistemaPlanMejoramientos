using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using sistemaPlanMejoramientos.Datos;

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

        public bool MtCrearUsuario(string correo, string password, int idRol)
        {
            if (!ValidarFormatoCorreo(correo) || string.IsNullOrWhiteSpace(password) || idRol <= 0)
            {
                return false;
            }

            if (oUsuarioD.MtExisteCorreo(correo))
            {
                return false;
            }

            return oUsuarioD.MtCrearUsuario(correo, password, idRol);
        }

        public DataTable MtListarUsuarios()
        {
            DataTable dt = oUsuarioD.MtListarUsuarios();
            return dt ?? new DataTable(); 
        }

        public bool MtActualizarUsuario(int idUsuario, string correo, string password, int idRol)
        {
            if (idUsuario <= 0 || !ValidarFormatoCorreo(correo) || string.IsNullOrWhiteSpace(password) || idRol <= 0)
            {
                return false;
            }

            return oUsuarioD.MtActualizarUsuario(idUsuario, correo, password, idRol);
        }

        public bool MtEliminarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return false;
            }
            return oUsuarioD.MtEliminarUsuario(idUsuario);
        }

        public DataTable MtLogin(string correo, string password)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
            {
                return new DataTable();
            }
            return oUsuarioD.MtLogin(correo, password);
        }

        public DataTable MtBuscarUsuarioPorId(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return new DataTable();
            }
            return oUsuarioD.MtBuscarUsuarioPorId(idUsuario);
        }

        public bool MtSolicitarRecuperacion(string correo)
        {
            if (!ValidarFormatoCorreo(correo))
            {
                return false;
            }
            return oUsuarioD.MtSolicitarRecuperacion(correo);
        }

        public bool MtRestablecerContrasena(string token, string nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Length < 4)
            {
                return false;
            }
            return oUsuarioD.MtRestablecerContrasena(token, nuevaPassword);
        }
    }
}