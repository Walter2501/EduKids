using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JuegoBase : MonoBehaviour
{
    //Todos los juegos utilizan esto, por eso la clase padre nueva
    //también porque el codigo de terminarjuego es largito para no tenerlo en cada script
    [SerializeField] protected GameObject panelCargando;
    [SerializeField] protected AudioClip win;
    [SerializeField] protected AudioClip wrong;
    [SerializeField] protected int puntosPorRespuestaCorrecta;

    protected int puntosTotales = 0;
    protected int maxVecesJugado = 0;
    protected int respuestasCorrectas = 0;
    protected int vecesJugado = 1;
    protected bool enEspera = false;
    protected AudioSource audioSource;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void Start()
    {
        if (GameManager.Instance.jugarDesafio == true)
        {
            maxVecesJugado = 5;
        }
        else
        {
            if (GameManager.Instance.dificultadJuego == 1) maxVecesJugado = 5;
            else if (GameManager.Instance.dificultadJuego == 2) maxVecesJugado = 10;
            else maxVecesJugado = 20;
        }

        IniciarJuego();
    }

    protected virtual void IniciarJuego()
    {
        enEspera = false;
    }

    public virtual void BotonOpcion(TextMeshProUGUI texto)
    {
        enEspera = true;
    }

    protected void CheckIfSubirDificultad(int gameNumber)
    {

        int dificultadMaxAlcanzada = GameManager.Instance.estudiante.progreso.GetDificultadMax(gameNumber);
        if (dificultadMaxAlcanzada > 2) return;
        
        float porcentajeRespuestasCorrectas = (float)respuestasCorrectas / maxVecesJugado;
        Debug.Log(porcentajeRespuestasCorrectas);
        if (porcentajeRespuestasCorrectas > 0.8f || Mathf.Approximately(porcentajeRespuestasCorrectas, 0.8f))
        {
            int dificultadJuego = GameManager.Instance.dificultadJuego;
            if (dificultadMaxAlcanzada == dificultadJuego)
            {
                Debug.Log("Dentro");
                GameManager.Instance.estudiante.progreso.SubirDificultadMaximaAlcanzada(gameNumber);
            }
        }
    }


    protected IEnumerator TerminarJuego()
    {
        panelCargando.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        if (GameManager.Instance.jugarDesafio == true)
        {
            GameManager.Instance.puntosHastaAhora += puntosTotales;
            if (GameManager.Instance.juegosJugados == 3)
            {
                GameManager.Instance.jugarDesafio = false;
                GameManager.Instance.desafio.DesafioCompletado();
                DesafioDiario.SaveDesafio(GameManager.Instance.desafio);
                GameManager.Instance.estudiante.Meritos += GameManager.Instance.puntosHastaAhora;
                GameManager.Instance.estudiante.MeritosTotal += GameManager.Instance.puntosHastaAhora;
                int respuestasDiarias = GameManager.Instance.puntosHastaAhora / 20;
                NivelCompletado newNivelDesafioDiario = new NivelCompletado("Desafio Diario", 1, $"{respuestasDiarias}/{15}");
                //Esto para que la lista no sea más larga que 20
                if (GameManager.Instance.estudiante.nivelesCompletados.Count >= 20)
                {
                    GameManager.Instance.estudiante.nivelesCompletados.RemoveAt(0);
                }
                GameManager.Instance.estudiante.nivelesCompletados.Add(newNivelDesafioDiario);

                var serverData1 = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).GetValueAsync(); //revisa si existe el usuario
                yield return new WaitUntil(predicate: () => serverData1.IsCompleted); //se espera a que termine la consulta

                if (serverData1.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
                {
                    Debug.LogError("Error al verificar el ID: " + serverData1.Exception);
                    yield break;
                }

                DataSnapshot snapshot1 = serverData1.Result;

                if (snapshot1.Exists) //Si la consulta regresa que ya existe el usuarioID en la bdd procede a guardar los datos actualizados
                {
                    string json = JsonConvert.SerializeObject(GameManager.Instance.estudiante);
                    var newDataToSave = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).SetRawJsonValueAsync(json);
                    yield return new WaitUntil(() => newDataToSave.IsCompleted);

                    if (newDataToSave.IsFaulted)
                    {
                        Debug.LogError("Error al guardar los nuevos datos: " + newDataToSave.Exception);
                    }
                    else
                    {
                        Debug.Log("Datos guardados correctamente");
                        GameManager.Instance.juegosJugados = 0;
                        GameManager.Instance.puntosHastaAhora = 0;
                        GameManager.Instance.CambiarEscena("MenuEstudiante");
                    }
                }
                else //Si por alguna razón ya no existe el usuario sale de la app
                {
                    Application.Quit(); //esto solo funciona el el built, no en el editor
                }
            }
            else
            {
                GameManager.Instance.juegosJugados++;
                GameManager.Instance.CambiarEscena(GameManager.Instance.desafio.juegosName[GameManager.Instance.juegosJugados-1]);
            }
        }
        int dificultadJuego = GameManager.Instance.dificultadJuego;
        float puntosFloat = puntosTotales;
        if (respuestasCorrectas == maxVecesJugado)
        {
            switch (dificultadJuego)
            {
                case 1:
                    puntosFloat *= 1.25f;
                    break;
                case 2:
                    puntosFloat *= 1.5f;
                    break;
                case 3:
                    puntosFloat *= 1.75f;
                    break;
            }
            puntosTotales = (int) puntosFloat;
        }
        GameManager.Instance.estudiante.Meritos += puntosTotales;
        GameManager.Instance.estudiante.MeritosTotal += puntosTotales;
        NivelCompletado newNivel = new NivelCompletado(GameManager.Instance.nombreJuego, GameManager.Instance.dificultadJuego, $"{respuestasCorrectas}/{maxVecesJugado}"); 
        //Esto para que la lista no sea más larga que 20
        if (GameManager.Instance.estudiante.nivelesCompletados.Count >= 20)
        {
            GameManager.Instance.estudiante.nivelesCompletados.RemoveAt(0);
        }
        GameManager.Instance.estudiante.nivelesCompletados.Add(newNivel);

        var serverData = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).GetValueAsync(); //revisa si existe el usuario
        yield return new WaitUntil(predicate: () => serverData.IsCompleted); //se espera a que termine la consulta

        if (serverData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
        {
            Debug.LogError("Error al verificar el ID: " + serverData.Exception);
            yield break;
        }

        DataSnapshot snapshot = serverData.Result;

        if (snapshot.Exists) //Si la consulta regresa que ya existe el usuarioID en la bdd procede a guardar los datos actualizados
        {
            string json = JsonConvert.SerializeObject(GameManager.Instance.estudiante);
            var newDataToSave = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => newDataToSave.IsCompleted);

            if (newDataToSave.IsFaulted)
            {
                Debug.LogError("Error al guardar los nuevos datos: " + newDataToSave.Exception);
            }
            else
            {
                Debug.Log("Datos guardados correctamente");
                GameManager.Instance.CambiarEscena("MenuTemp");
            }
        }
        else //Si por alguna razón ya no existe el usuario sale de la app
        {
            Application.Quit(); //esto solo funciona el el built, no en el editor
        }
    }
}
