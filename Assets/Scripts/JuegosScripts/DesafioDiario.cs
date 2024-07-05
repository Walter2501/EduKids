using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DesafioDiario
{
    private const string LAST_DAY_KEY = "LastDay";
    private const string DESAFIO_KEY = "DesafioKey";

    private static List<string> juegosEscenaNombres = new List<string>
    {
        "Juego1",
        "Juego2",
        "juego3",
        "juego4",
        "Juego5",
        "Juego6",
        "juego7"
    };

    public static void CheckAndCreateDesafio()
    {
        Desafio desafio = LoadDesafio();
        if (desafio != null)
        {
            Debug.Log("Ya hay desafio creado");
            GameManager.Instance.desafio = desafio;
        }
        else
        {
            // Obtener el día de hoy
            int today = System.DateTime.Now.Day;
            Debug.Log(today);

            // Verificar si hay un día guardado
            if (PlayerPrefs.HasKey(LAST_DAY_KEY))
            {
                int lastDay = PlayerPrefs.GetInt(LAST_DAY_KEY);

                // Si el día guardado es diferente al día de hoy, crear un nuevo desafío
                if (lastDay != today)
                {
                    CrearDesafio();
                    // Actualizar el día guardado a hoy
                    PlayerPrefs.SetInt(LAST_DAY_KEY, today);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                // Si no hay un día guardado, guardar el día de hoy y no crear desafío
                PlayerPrefs.SetInt(LAST_DAY_KEY, today);
                PlayerPrefs.Save();
            }
        }
    }

    private static void CrearDesafio()
    {
        List<string> juegosDisponibles = new List<string>();
        List<string> juegosElegidos = new List<string>();

        // Asumiendo que tienes una referencia al progreso del estudiante
        Progreso progreso = GameManager.Instance.estudiante.progreso;

        // Añadir el primer juego siempre
        juegosDisponibles.Add(juegosEscenaNombres[0]);

        // Añadir otros juegos según la dificultad máxima alcanzada
        for (int juego = 2; juego <= juegosEscenaNombres.Count; juego++)
        {
            if (progreso.GetDificultadMax(juego - 1) >= 3)
            {
                juegosDisponibles.Add(juegosEscenaNombres[juego - 1]);
            }
        }

        //Codigo para eleigr juegos
        if (juegosDisponibles.Count >= 3)
        {
            // Usar un HashSet para evitar juegos repetidos
            HashSet<string> juegosElegidosSet = new HashSet<string>();

            while (juegosElegidosSet.Count < 3)
            {
                int index = Random.Range(0, juegosDisponibles.Count);
                string juegoElegido = juegosDisponibles[index];

                // Añadir solo si no está repetido
                if (!juegosElegidosSet.Contains(juegoElegido))
                {
                    juegosElegidosSet.Add(juegoElegido);
                }
            }

            juegosElegidos.AddRange(juegosElegidosSet);
        }
        else
        {
            // Si hay menos de 3 juegos disponibles, permitir repeticiones
            while (juegosElegidos.Count < 3)
            {
                int index = Random.Range(0, juegosDisponibles.Count);
                juegosElegidos.Add(juegosDisponibles[index]);
            }
        }

        Desafio newDesafio = new Desafio(juegosElegidos);

        SaveDesafio(newDesafio);
        GameManager.Instance.desafio = newDesafio;
    }

    public static void PonerDiaAnterior()
    {
        int today = System.DateTime.Now.Day;
        PlayerPrefs.SetInt(LAST_DAY_KEY, today-1);
        PlayerPrefs.Save();
    }

    public static void SaveDesafio(Desafio newDesafio)
    {
        string json = JsonConvert.SerializeObject(newDesafio);
        PlayerPrefs.SetString(DESAFIO_KEY, json);
        PlayerPrefs.Save();
    }

    public static Desafio LoadDesafio()
    {
        string json = PlayerPrefs.GetString(DESAFIO_KEY, "");
        if (json == "") return null;
        Desafio desafio = JsonConvert.DeserializeObject<Desafio>(json);
        return desafio;
    }
}

[SerializeField]
public class Desafio
{
    public List<string> juegosName;
    public bool completado;

    public Desafio(List<string> juegosName)
    {
        this.juegosName = juegosName;
        completado = false;
    }

    public void DesafioCompletado()
    {
        if (completado) return;
        completado = true;
    }
}