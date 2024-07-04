using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Juego5 : JuegoBase
{
    [SerializeField] private GameObject[] manzanas;
    [SerializeField] private GameObject[] manzanasT; //las manzanas tachadas
    [SerializeField] private TextMeshProUGUI[] textoBotones;
    [SerializeField] private TextMeshProUGUI resta;

    private int[] respuestas = new int[5];
    private int numManzanas;
    private int numManzanasT;
    private int totalManzanas;
    private int respuestaCorrecta;

    protected override void IniciarJuego()
    {
        DecidirManzanasParaUsar();
        EncenderManzanas();
        GeneracionRespuestas();
        Escritura();
        base.IniciarJuego();
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

    public override void BotonOpcion(TextMeshProUGUI texto)
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
                    respuestasCorrectas++;
                    puntosTotales += puntosPorRespuestaCorrecta;
                }

                else
                {
                    audioSource.clip = wrong;
                    audioSource.Play();
                }
                base.BotonOpcion(texto);
                vecesJugado++;
                if (vecesJugado > maxVecesJugado)
                {
                    CheckIfSubirDificultad(5);
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
