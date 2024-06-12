using System;
using System.Collections.Generic;

[Serializable]
public class Maestro : UsuarioBase
{
    public List<RecompensaData> Recompensas;
    public List<CanjeoData> Canjeos;
    public List<string> EstudiantesIDs;
    public string CodigoMaestro;

    public Maestro(string nombre, string apellido1, string apellido2, string password, string codigoMaestro) : base(nombre, apellido1, apellido2, password)
    {
        Recompensas = new List<RecompensaData>();
        Canjeos = new List<CanjeoData>();
        EstudiantesIDs = new List<string>();
        Rol = 2;
        CodigoMaestro = codigoMaestro;
    }
}
