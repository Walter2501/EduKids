using System;
using System.Collections.Generic;

//Roles:
//0 = Estudiante
//1 = Padre
//2 = Maestro
//3 = Admin
[Serializable]
public class UsuarioBase
{
    public string Nombre;
    public string Apellido1;
    public string Apellido2;
    public string Password;
    public int Rol;
    public Progreso Progreso;

    public UsuarioBase() { }

    public UsuarioBase(string nombre, string apellido1, string apellido2, string password)
    {
        Nombre = nombre;
        Apellido1 = apellido1;
        Apellido2 = apellido2;
        Password = password;
    }

    public UsuarioBase(string nombre, string apellido1, string apellido2, string password, int rol, Progreso progreso)
    {
        Nombre = nombre;
        Apellido1 = apellido1;
        Apellido2 = apellido2;
        Password = password;
        Rol = rol;
        Progreso = progreso;
    }
}

[Serializable]
public class Progreso
{
    public int dificultad;
    public List<Nivel> niveles;
}