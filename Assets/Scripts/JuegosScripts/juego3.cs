using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Random;

public class juego3 : JuegoBase
{
    [SerializeField] private TextMeshProUGUI txtPrin;
    [SerializeField] private TextMeshProUGUI[] txtRpt;

    private string respuestaCorrecta = "";

    protected override void IniciarJuego()
    {
        CalcularSuma();
        base.IniciarJuego();
    }

    public void CalcularSuma()
    {
        int num1 = Range(10, 100);
        int num2 = Range(10, 100);
        while ((num1 + num2) > 100)
        {
            // Si es mayor que 100, generar nuevos números
            num1 = Range(10, 100);
            num2 = Range(10, 100);
        }


        txtPrin.text = $"{num1} +\n<u>{num2}</u>\n??";

        DibujarRespuestas(num1, num2);
    }

    public void DibujarRespuestas(int num1, int num2)
    {
        int ramdonIndex = Range(0, txtRpt.Length);
        txtRpt[ramdonIndex].text = $"{num1 + num2}";

        respuestaCorrecta = txtRpt[ramdonIndex].text;

        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < txtRpt.Length; i++)
        {
            if (i != ramdonIndex)
            {
                availableIndexes.Add(i);
            }
        }

        Shuffle(availableIndexes);

        for (int i = 0; i < txtRpt.Length - 1; i++)
        {
            int numAle1, numAle2;
            do
            {
                numAle1 = Range(0, 100);
                numAle2 = Range(0, 100);
            } while (numAle1 == num1 || numAle1 == num2 || numAle2 == num1 || numAle2 == num2 || (numAle1 + numAle2) > 100);

            txtRpt[availableIndexes[i]].text = $"{numAle1 + numAle2}";
        }
        enEspera = false;
    }

    // Método Shuffle() para mezclar una lista
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public override void BotonOpcion(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        base.BotonOpcion(texto);
        if (texto.text == respuestaCorrecta)
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
            CheckIfSubirDificultad(3);
            StartCoroutine(TerminarJuego());
        }
        else
        {
            Invoke("IniciarJuego", 1f);
        }
    }
}
