using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Juego7 : JuegoBase
{
    [SerializeField] private TextMeshProUGUI[] textoBotones;
    [SerializeField] private TextMeshProUGUI textoSuma;

    private int num1 = 0;
    private int num2 = 0;
    private int suma = 0;

    protected override void IniciarJuego()
    {
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        suma = num1 + num2;

        textoSuma.text = $"{num1} + {num2}";

        GenerarRespuestas();
        base.IniciarJuego();
    }

    private void GenerarRespuestas()
    {
        List<int> numerosDisponibles = Enumerable.Range(1, 18).ToList();
        numerosDisponibles.Remove(suma);

        //Lambda
        numerosDisponibles = numerosDisponibles.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < textoBotones.Length; i++)
        {
            textoBotones[i].text = $"{numerosDisponibles[i]}";
            textoBotones[i].text = $"{numerosDisponibles[i]}";
        }

        textoBotones[Random.Range(0, textoBotones.Length)].text = $"{suma}";
    }

    public override void BotonOpcion(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        base.BotonOpcion(texto);

        if (texto.text == $"{suma}")
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