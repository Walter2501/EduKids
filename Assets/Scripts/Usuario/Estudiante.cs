using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//los meritos son puntos
[Serializable]
public class Estudiante : UsuarioBase
{
    public int Meritos;
    public List<RecompensaData> Recompensas;

    public Estudiante(string nombre, string apellido1, string apellido2, string password) : base(nombre, apellido1, apellido2, password)
    {
        Meritos = 0;
        Recompensas = new List<RecompensaData>();
        Rol = 0;
    }
}