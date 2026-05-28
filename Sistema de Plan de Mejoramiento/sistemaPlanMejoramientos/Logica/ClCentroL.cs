using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using sistemaPlanMejoramientos.Datos;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClCentroL
    {
        ClCentroD oCentroD = new ClCentroD();

        public bool MtCrearCentro(string codigoCentro, string nombre, string regional, string municipio, string departamento, string estado)
        {
            if (string.IsNullOrWhiteSpace(codigoCentro))
                throw new Exception("El código del centro es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del centro es obligatorio.");

            if (string.IsNullOrWhiteSpace(regional))
                throw new Exception("La regional es obligatoria.");

            if (string.IsNullOrWhiteSpace(municipio))
                throw new Exception("El municipio es obligatorio.");

            if (string.IsNullOrWhiteSpace(departamento))
                throw new Exception("El departamento es obligatorio.");

            if (string.IsNullOrWhiteSpace(estado))
                throw new Exception("El estado es obligatorio.");

            if (codigoCentro.Trim().Length > 50)
                throw new Exception("El código del centro no puede superar los 50 caracteres.");

            if (nombre.Trim().Length > 150)
                throw new Exception("El nombre del centro no puede superar los 150 caracteres.");

            if (regional.Trim().Length > 100)
                throw new Exception("La regional no puede superar los 100 caracteres.");

            if (municipio.Trim().Length > 100)
                throw new Exception("El municipio no puede superar los 100 caracteres.");

            if (departamento.Trim().Length > 100)
                throw new Exception("El departamento no puede superar los 100 caracteres.");

            if (estado != "Activo" && estado != "Inactivo")
                throw new Exception("El estado debe ser 'Activo' o 'Inactivo'.");

            if (oCentroD.MtExisteCodigoCentro(codigoCentro.Trim()))
                throw new Exception($"Ya existe un centro registrado con el código '{codigoCentro}'.");

            return oCentroD.MtCrearCentro(
                codigoCentro.Trim(),
                nombre.Trim(),
                regional.Trim(),
                municipio.Trim(),
                departamento.Trim(),
                estado.Trim()
            );
        }

        public DataTable MtListarCentros()
        {
            return oCentroD.MtListarCentros();
        }

        public DataTable MtListarCentros(string filtro)
        {
            if (filtro == null) filtro = "";
            return oCentroD.MtListarCentros(filtro.Trim());
        }

        public bool MtActualizarCentro(int idCentro, string codigoCentro, string nombre, string regional, string municipio, string departamento, string estado)
        {
            if (idCentro <= 0)
                throw new Exception("El identificador del centro no es válido.");

            if (string.IsNullOrWhiteSpace(codigoCentro))
                throw new Exception("El código del centro es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del centro es obligatorio.");

            if (string.IsNullOrWhiteSpace(regional))
                throw new Exception("La regional es obligatoria.");

            if (string.IsNullOrWhiteSpace(municipio))
                throw new Exception("El municipio es obligatorio.");

            if (string.IsNullOrWhiteSpace(departamento))
                throw new Exception("El departamento es obligatorio.");

            if (string.IsNullOrWhiteSpace(estado))
                throw new Exception("El estado es obligatorio.");

            if (codigoCentro.Trim().Length > 50)
                throw new Exception("El código del centro no puede superar los 50 caracteres.");

            if (nombre.Trim().Length > 150)
                throw new Exception("El nombre del centro no puede superar los 150 caracteres.");

            if (regional.Trim().Length > 100)
                throw new Exception("La regional no puede superar los 100 caracteres.");

            if (municipio.Trim().Length > 100)
                throw new Exception("El municipio no puede superar los 100 caracteres.");

            if (departamento.Trim().Length > 100)
                throw new Exception("El departamento no puede superar los 100 caracteres.");

            if (estado != "Activo" && estado != "Inactivo")
                throw new Exception("El estado debe ser 'Activo' o 'Inactivo'.");

            DataTable dtExistente = oCentroD.MtObtenerCentroPorId(idCentro);
            if (dtExistente.Rows.Count == 0)
                throw new Exception("El centro que intenta actualizar no existe.");

            string codigoActual = dtExistente.Rows[0]["codigoCentro"].ToString();
            if (!codigoActual.Equals(codigoCentro.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                if (oCentroD.MtExisteCodigoCentro(codigoCentro.Trim()))
                    throw new Exception($"Ya existe otro centro registrado con el código '{codigoCentro}'.");
            }

            return oCentroD.MtActualizarCentro(
                idCentro,
                codigoCentro.Trim(),
                nombre.Trim(),
                regional.Trim(),
                municipio.Trim(),
                departamento.Trim(),
                estado.Trim()
            );
        }

        public bool MtEliminarCentro(int idCentro)
        {
            if (idCentro <= 0)
                throw new Exception("El identificador del centro no es válido.");

            DataTable dt = oCentroD.MtObtenerCentroPorId(idCentro);
            if (dt.Rows.Count == 0)
                throw new Exception("El centro que intenta eliminar no existe.");

            return oCentroD.MtEliminarCentro(idCentro);
        }

        public DataTable MtObtenerCentroPorId(int idCentro)
        {
            if (idCentro <= 0)
                throw new Exception("El identificador del centro no es válido.");

            return oCentroD.MtObtenerCentroPorId(idCentro);
        }

        public DataTable MtListarCentrosActivos()
        {
            return oCentroD.MtListarCentrosActivos();
        }

        public int MtObtenerIdPorCodigo(string codigoCentro)
        {
            if (string.IsNullOrWhiteSpace(codigoCentro))
                throw new Exception("El código del centro es obligatorio.");

            return oCentroD.MtObtenerIdPorCodigo(codigoCentro.Trim());
        }
    }
}