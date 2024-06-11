using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Admin : UsuarioBase
{
    public Admin(string nombre, string apellido1, string apellido2, string password) : base(nombre, apellido1, apellido2, password)
    {
        Rol = 3;
    }
}