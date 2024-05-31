using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Roles:
//0 = Estudiante
//1 = Maestro
//2 = Padre
//3 = Admin
[Serializable]
public class UsuarioBase
{
    public string Nombre;
    public string Apellido1;
    public string Apellido2;
    public string Password;
    public int Rol;

    public UsuarioBase(string nombre, string apellido1, string apellido2, string password)
    {
        Nombre = nombre;
        Apellido1 = apellido1;
        Apellido2 = apellido2;
        Password = password;
    }
}
