using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Juego1 : MonoBehaviour
{
    [SerializeField] private GameObject[] Manzanas;
    [SerializeField] private TextMeshProUGUI[] textosBotones;

    private int[] numeros = new int[6];
    private int cantidadAleatoria = 0; //tambien la respuesta correcta

    private void Awake()
    {
        cantidadAleatoria = Random.Range(1, 11);

        for(int i = 0; i < cantidadAleatoria; i++)
        {
            Manzanas[i].SetActive(true);
        }
    }

    private void Start()
    {
        GenerarNumeros();
        AsignarNumeros();
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
        for (int i = 0; i < textosBotones.Length; i++)
        {
            if (textosBotones[i] == texto)
            {
                if (numeros[i] == cantidadAleatoria) Debug.Log("Respuesta correcta");
                else Debug.Log("Respuesta incorrecta");
                return;
            }
        }
    }
}