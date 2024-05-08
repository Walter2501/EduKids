using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Random;

public class juego3 : MonoBehaviour
{
    public TextMeshProUGUI txtPrin;
    public TextMeshProUGUI[] txtRpt;

    private string respuestaCorrecta = "";

    void Start()
    {
        calcularSuma();
    }

    public void calcularSuma()
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

        dibujarRespuestas(num1, num2);
    }

    public void dibujarRespuestas(int num1, int num2)
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
            } while (numAle1 == num1 || numAle1 == num2 || numAle2 == num1 || numAle2 == num2 || (numAle1 + numAle2) >100);

            txtRpt[availableIndexes[i]].text = $"{numAle1 + numAle2}";
        }
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

    public void PresionarBoton(TextMeshProUGUI textoBoton)
    {
        if (textoBoton.text == respuestaCorrecta) Debug.Log("Respuesta Correcta");
        else Debug.Log("Respuesta Incorrecta");
    }
}
