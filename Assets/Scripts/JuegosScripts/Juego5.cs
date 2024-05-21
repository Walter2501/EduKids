using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Juego5 : MonoBehaviour
{
    [SerializeField] private GameObject[] manzanas;
    [SerializeField] private GameObject[] manzanasT; //las manzanas tachadas
    [SerializeField] private TextMeshProUGUI[] textoBotones;
    [SerializeField] private TextMeshProUGUI resta;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private int puntosPorRespuestaCorrecta;

    private int puntosTotales = 0;
    private int vecesJugado = 1;
    private bool enEspera = false;
    private AudioSource audioSource;
    private int[] respuestas = new int[5];
    private int numManzanas;
    private int numManzanasT;
    private int totalManzanas; 
    private int respuestaCorrecta;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        IniciarJuego();
    }

    private void IniciarJuego()
    {
        DecidirManzanasParaUsar();
        EncenderManzanas();
        GeneracionRespuestas();
        Escritura();
        enEspera = false;
    }

    private void DecidirManzanasParaUsar()
    {
        numManzanas = Random.Range(1, 10); //decide cuantas manzanas normales encendera
        numManzanasT = (numManzanas == 9) ? 1 : Random.Range(1, 11 - numManzanas); //decide cuantas manzanas tachadas encendera, sumando con las normales no superara a 10
        totalManzanas = numManzanas + numManzanasT; //el total de manzanas
        respuestaCorrecta = numManzanas; // el numero de manzanas normales es la respuesta correcta
    }

    private void EncenderManzanas() // encenderá las manzanas
    {
        for (int i = 0; i < manzanas.Length; i++)
        {
            manzanas[i].SetActive(false);
            manzanasT[i].SetActive(false);
        }
        for (int i = 0; i < numManzanas; i++)
        {
            manzanas[i].SetActive(true);
        }
        for (int i = 0; i < numManzanasT; i++)
        {
            manzanasT[i].SetActive(true);
        }
    }

    private void GeneracionRespuestas() //genera las respuestas
    {
        List<int> numerosDisponibles = Enumerable.Range(1, 10).ToList(); //Crea lista del 1 al 10
        numerosDisponibles.Remove(respuestaCorrecta); //remueve la respuesta correcta de la lista (para evitar repetidos)

        //Lambda
        numerosDisponibles = numerosDisponibles.OrderBy(x => Random.value).ToList(); //se mezclan los numeros en la lista

        for (int i = 0; i < respuestas.Length; i++) //asigna a cada boton un numero
        {
            respuestas[i] = numerosDisponibles[i];
        }

        respuestas[Random.Range(0, respuestas.Length)] = respuestaCorrecta; //cambia uno de los numeros por la respuesta correcta
    }

    private void Escritura()
    {
        resta.text = $"{totalManzanas} - {numManzanasT}";

        for (int i = 0; i < textoBotones.Length; i++)
        {
            textoBotones[i].text = $"{respuestas[i]}";
        }
    }

    public void PresionarBoton(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        for (int i = 0; i < textoBotones.Length; i++)
        {
            if (texto == textoBotones[i])
            {
                if (respuestas[i] == respuestaCorrecta)
                {
                    audioSource.clip = win;
                    audioSource.Play();
                    puntosTotales += puntosPorRespuestaCorrecta;
                }

                else
                {
                    audioSource.clip = wrong;
                    audioSource.Play();
                }
                enEspera = true;
                vecesJugado++;
                if (vecesJugado > 5)
                {
                    StartCoroutine(TerminarJuego());
                }
                else
                {
                    Invoke("IniciarJuego", 1f);
                }
                return;
            }
        }
    }

    private IEnumerator TerminarJuego()
    {
        GameManager.Instance.AddMeritos(puntosTotales);
        Debug.Log($"Terminado: {GameManager.Instance.cantidadMeritos}");
        yield return new WaitForSeconds(1);
        Debug.Log(puntosTotales);
        GameManager.Instance.CambiarEscena(0);
    }
}
