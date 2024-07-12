using System;
using System.Collections.Generic;

//los meritos son puntos
[Serializable]
public class Estudiante : UsuarioBase
{
    public int Meritos;
    public int MeritosTotal; //Los meritos conseguidos durante todo la vida del usuario, no se restan
    public List<RecompensaData> Recompensas;
    public List<NivelCompletado> nivelesCompletados;
    public Progreso progreso;
    public string CodigoEstudiante;

    public Estudiante(string nombre, string apellido1, string apellido2, string password, string codigoEstudiante) : base(nombre, apellido1, apellido2, password)
    {
        Meritos = 0;
        MeritosTotal = 0;
        Recompensas = new List<RecompensaData>();
        Rol = 0;
        CodigoEstudiante = codigoEstudiante;
        progreso = new Progreso();
        nivelesCompletados = new List<NivelCompletado>();
    }
}

[Serializable]
public class Progreso
{
    public int juego1maxDificultad;
    public int juego2maxDificultad;
    public int juego3maxDificultad;
    public int juego4maxDificultad;
    public int juego5maxDificultad;
    public int juego6maxDificultad;
    public int juego7maxDificultad;

    public Progreso()
    {
        juego1maxDificultad = 1;
        juego2maxDificultad = 1;
        juego3maxDificultad = 1;
        juego4maxDificultad = 1;
        juego5maxDificultad = 1;
        juego6maxDificultad = 1;
        juego7maxDificultad = 1;
    }

    //Consigo la maxima dificultad alcanzada en el juego pedido
    public int GetDificultadMax(int juego)
    {
        switch (juego)
        {
            case 1:
                return juego1maxDificultad;
            case 2:
                return juego2maxDificultad;
            case 3:
                return juego3maxDificultad;
            case 4:
                return juego4maxDificultad;
            case 5:
                return juego5maxDificultad;
            case 6:
                return juego6maxDificultad;
            case 7:
                return juego7maxDificultad;
        }
        return -1;
    }

    //sube la dificultad máxima para ese juego (Max 3)
    public void SubirDificultadMaximaAlcanzada(int juego)
    {
        switch (juego)
        {
            case 1:
                if (juego1maxDificultad < 3) juego1maxDificultad++;
                break;
            case 2:
                if (juego2maxDificultad < 3) juego2maxDificultad++;
                break;
            case 3:
                if (juego3maxDificultad < 3) juego3maxDificultad++;
                break;
            case 4:
                if (juego4maxDificultad < 3) juego4maxDificultad++;
                break;
            case 5:
                if (juego5maxDificultad < 3) juego5maxDificultad++;
                break;
            case 6:
                if (juego6maxDificultad < 3) juego6maxDificultad++;
                break;
            case 7:
                if (juego7maxDificultad < 3) juego7maxDificultad++;
                break;
        }
    }
}


[Serializable]
public class NivelCompletado
{

    public string nombre;
    public int dificultad;
    public string respuestasCorrectas;

    public NivelCompletado(string nombre, int dificultad, string respuestasCorrectas)
    {
        this.nombre = nombre;
        this.dificultad = dificultad;
        this.respuestasCorrectas = respuestasCorrectas;
    }
}