using sistemaPlanMejoramientos.Datos;
using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sistemaPlanMejoramientos.Logica
{
    public class ClEvidenciaL
    {
        ClEvidenciaD oEvidenciaD = new ClEvidenciaD();

        public bool MtRegistrarEvidencia(int idPlanMejoramiento, string nombreArchivo, string rutaArchivo, DateTime fechaSubida, string tipoArchivo)
        {
            if (idPlanMejoramiento <= 0 || string.IsNullOrWhiteSpace(nombreArchivo) || string.IsNullOrWhiteSpace(rutaArchivo))
            {
                return false;
            }
            return oEvidenciaD.MtRegistrarEvidencia(idPlanMejoramiento, nombreArchivo, rutaArchivo, fechaSubida, tipoArchivo);
        }

        public List<ClEvidenciaM> MtListarEvidenciaPorPlan(int idPlanMejoramiento)
        {
            if (idPlanMejoramiento <= 0)
                return new List<ClEvidenciaM>();

            return oEvidenciaD.MtListarEvidenciaPorPlan(idPlanMejoramiento);
        }

        public bool MtSobrescribirEvidencia(int idPlanMejoramiento, string nombreArchivo, string rutaArchivo, DateTime fechaSubida, string tipoArchivo)
        {
            if (idPlanMejoramiento <= 0 || string.IsNullOrWhiteSpace(nombreArchivo) || string.IsNullOrWhiteSpace(rutaArchivo))
            {
                return false;
            }
            return oEvidenciaD.MtSobrescribirEvidencia(idPlanMejoramiento, nombreArchivo, rutaArchivo, fechaSubida, tipoArchivo);
        }

        public bool MtCalificarEvidencia(int idPlanMejoramiento, string criterioProducto, string criterioConocimiento, string criterioDesempeno)
        {
            if (idPlanMejoramiento <= 0 || string.IsNullOrWhiteSpace(criterioProducto) ||
                string.IsNullOrWhiteSpace(criterioConocimiento) || string.IsNullOrWhiteSpace(criterioDesempeno))
            {
                return false;
            }
            return oEvidenciaD.MtCalificarEvidencia(idPlanMejoramiento, criterioProducto, criterioConocimiento, criterioDesempeno);
        }

        public bool MtRegistrarObservacionesEvidencia(int idPlanMejoramiento, string observaciones)
        {
            if (idPlanMejoramiento <= 0)
            {
                return false;
            }
            return oEvidenciaD.MtRegistrarObservacionesEvidencia(idPlanMejoramiento, observaciones);
        }
        public ClAprendizM MtObtenerAprendizPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            return oEvidenciaD.MtObtenerAprendizPorUsuario(idUsuario);
        }

        public int MtContarPlanesPorEstado(int idAprendiz, string estado)
        {
            if (idAprendiz <= 0 || string.IsNullOrWhiteSpace(estado))
                return 0;

            return oEvidenciaD.MtContarPlanesPorEstado(idAprendiz, estado);
        }

        public List<ClPlanMejoramientoM> MtListarPlanesPorAprendiz(int idAprendiz)
        {
            if (idAprendiz <= 0)
                return new List<ClPlanMejoramientoM>();

            return oEvidenciaD.MtListarPlanesPorAprendiz(idAprendiz);
        }
        public ClPlanMejoramientoM MtObtenerPlanPorId(int idPlan)
        {
            if (idPlan <= 0)
                return null;

            return oEvidenciaD.MtObtenerPlanPorId(idPlan);
        }

        public List<ClResultadoAprendizajeM> MtListarResultadosPorPlan(int idPlan)
        {
            if (idPlan <= 0)
                return new List<ClResultadoAprendizajeM>();

            return oEvidenciaD.MtListarResultadosPorPlan(idPlan);
        }

        public bool MtValidarExtension(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo)) return false;
            string ext = System.IO.Path.GetExtension(nombreArchivo).ToLower().TrimStart('.');
            string[] permitidos = { "pdf", "docx", "jpg", "jpeg", "png", "zip" };
            return System.Array.Exists(permitidos, e => e == ext);
        }

        public string MtObtenerTipoArchivo(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo)) return "PDF";
            string ext = System.IO.Path.GetExtension(nombreArchivo).ToLower().TrimStart('.');
            switch (ext)
            {
                case "pdf": return "PDF";
                case "docx": return "DOCX";
                case "jpg":
                case "jpeg": return "JPG";
                case "png": return "PNG";
                case "zip": return "ZIP";
                default: return "PDF";
            }
        }
    }
}