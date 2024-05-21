using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Juego1 : MonoBehaviour
{
    [SerializeField] private GameObject[] Manzanas;
    [SerializeField] private TextMeshProUGUI[] textosBotones;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private int puntosPorRespuestaCorrecta;

    private int puntosTotales = 0;
    private int vecesJugado = 1;
    private bool enEspera = false;
    private AudioSource audioSource;
    private int[] numeros = new int[6];
    private int cantidadAleatoria = 0; //tambien la respuesta correcta

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
        cantidadAleatoria = Random.Range(1, 11);
        EncenderManzanas(cantidadAleatoria);
        GenerarNumeros();
        AsignarNumeros();
        enEspera = false;
    }

    private void EncenderManzanas(int cantidad)
    {
        
        for (int i = 0; i < Manzanas.Length; i++)
        {
            if (i < cantidadAleatoria) Manzanas[i].SetActive(true);
            else Manzanas[i].SetActive(false);
        }
    }

    private void GenerarNumeros()
    {
        List<int> numerosDisponibles = Enumerable.Range(1, 10).ToList();
        numerosDisponibles.Remove(cantidadAleatoria);

        List<int> numerosGenerados = new List<int>();


        //Lambda
        numerosDisponibles = numerosDisponibles.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < numeros.Length; i++)
        {
            numerosGenerados.Add(numerosDisponibles[i]);
        }

        for (int i = 0; i < numerosGenerados.Count; i++)
        {
            numeros[i] = numerosGenerados[i];
        }

        numeros[Random.Range(0, numeros.Length)] = cantidadAleatoria;
    }

    private void AsignarNumeros()
    {
        for (int i = 0; i < textosBotones.Length; i++)
        {
            textosBotones[i].text = numeros[i].ToString();
        }
    }

    public void BotonRespuestaCorrecta(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        for (int i = 0; i < textosBotones.Length; i++)
        {
            if (textosBotones[i] == texto)
            {
                if (numeros[i] == cantidadAleatoria)
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