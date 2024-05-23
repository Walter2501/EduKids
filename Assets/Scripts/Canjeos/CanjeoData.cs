using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanjeoData
{
    public string nombreEstudiante = "";
    public string nombre = "";

    public CanjeoData(string nombreEstudiante, string nombre)
    {
        this.nombreEstudiante = nombreEstudiante;
        this.nombre = nombre;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        CanjeoData other = (CanjeoData)obj;
        return nombreEstudiante == other.nombreEstudiante && nombre == other.nombre;
    }

    public override int GetHashCode()
    {
        return nombreEstudiante.GetHashCode() ^ nombre.GetHashCode();
    }
}
