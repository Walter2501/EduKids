using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Maestro : UsuarioBase
{
    public List<RecompensaData> Recompensas;
    public List<CanjeoData> Canjeos;

    public Maestro(string nombre, string apellido1, string apellido2, string password) : base(nombre, apellido1, apellido2, password)
    {
        Recompensas = new List<RecompensaData>();
        Canjeos = new List<CanjeoData>();
        Rol = 1;
    }
}
