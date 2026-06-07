using sistemaPlanMejoramientos.Modelo;
using System;
using System.Collections.Generic;

public class ClFichasM
{
    public int idFicha { get; set; }

    public string codigoFicha { get; set; }

    public DateTime fechaInicio { get; set; }

    public DateTime fechaFinalizacion { get; set; }

    public string jornada { get; set; }

    public string estado { get; set; }

    public int idPrograma { get; set; }

    public int idCentro { get; set; }

    public ClProgramasM programa { get; set; }

    public ClCentroM centro { get; set; }

    public List<ClFichaAprendizM> aprendices { get; set; }

    public List<ClFichaInstructorM> instructores { get; set; }

    public string textoFicha
    {
        get
        {
            string nombrePrograma = programa != null ? programa.nombre : "";
            return codigoFicha + " - " + nombrePrograma + " (" + jornada + ")";
        }
    }
}