using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Juego1 : JuegoBase
{
    [SerializeField] private GameObject[] Manzanas;
    [SerializeField] private TextMeshProUGUI[] textosBotones;
    private int[] numeros = new int[6];
    private int cantidadAleatoria = 0;

    protected override void IniciarJuego()
    {
        cantidadAleatoria = Random.Range(1, 11);
        EncenderManzanas(cantidadAleatoria);
        GenerarNumeros();
        AsignarNumeros();
        base.IniciarJuego();
    }

    private void EncenderManzanas(int cantidad)
    {
        for (int i = 0; i < Manzanas.Length; i++)
        {
            Manzanas[i].SetActive(i < cantidadAleatoria);
        }
    }

    private void GenerarNumeros()
    {
        List<int> numerosDisponibles = new List<int>(Enumerable.Range(1, 10));
        numerosDisponibles.Remove(cantidadAleatoria);

        numerosDisponibles = numerosDisponibles.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < numeros.Length; i++)
        {
            numeros[i] = numerosDisponibles[i];
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

    public override void BotonOpcion(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        base.BotonOpcion(texto);
        for (int i = 0; i < textosBotones.Length; i++)
        {
            if (textosBotones[i] == texto)
            {
                if (numeros[i] == cantidadAleatoria)
                {
                    audioSource.clip = win;
                    audioSource.Play();
                    respuestasCorrectas++;
                    puntosTotales += puntosPorRespuestaCorrecta;
                }
                else
                {
                    audioSource.clip = wrong;
                    audioSource.Play();
                }
                vecesJugado++;
                if (vecesJugado > maxVecesJugado)
                {
                    CheckIfSubirDificultad(1);
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
}
