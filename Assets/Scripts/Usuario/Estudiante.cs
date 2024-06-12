using System;
using System.Collections.Generic;

//los meritos son puntos
[Serializable]
public class Estudiante : UsuarioBase
{
    public int Meritos;
    public List<RecompensaData> Recompensas;
    public string CodigoEstudiante;

    public Estudiante(string nombre, string apellido1, string apellido2, string password, string codigoEstudiante) : base(nombre, apellido1, apellido2, password)
    {
        Meritos = 0;
        Recompensas = new List<RecompensaData>();
        Rol = 0;
        CodigoEstudiante = codigoEstudiante;
    }
}