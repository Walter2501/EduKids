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
    protected int vecesJugado = 1;
    protected bool enEspera = false;
    protected AudioSource audioSource;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void Start()
    {
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


    protected IEnumerator TerminarJuego()
    {
        panelCargando.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        GameManager.Instance.estudiante.Meritos += puntosTotales;
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
