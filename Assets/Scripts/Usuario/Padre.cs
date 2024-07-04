using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padre : UsuarioBase
{
    public Padre(string nombre, string apellido1, string apellido2, string password) : base(nombre, apellido1, apellido2, password)
    {
        Rol = 1;
    }
}
