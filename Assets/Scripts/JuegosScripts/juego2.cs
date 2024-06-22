using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class juego2 : JuegoBase
{
    [SerializeField] private TextMeshProUGUI textPrin;
    [SerializeField] private TextMeshProUGUI[] textRptIArray;

    private int num;
    private int[] inferior;
    private int[] superior;
    private string respuestaCorrecta = "";



    public ProgresoUser progresoUser; // Referencia a ProgresoUser

    private void EnsureProgresoUserInitialized()
    {
        if (progresoUser == null)
        {
            progresoUser = FindObjectOfType<ProgresoUser>();
            if (progresoUser == null)
            {
                Debug.LogError("ProgresoUser no encontrado en EnsureProgresoUserInitialized.");
            }
        }
    }

    private IEnumerator OnLevelComplete()
    {
        EnsureProgresoUserInitialized();
        yield return new WaitForSeconds(1f);

        if (progresoUser != null)
        {
            progresoUser.AgregarNivel(new Nivel { nombre = "Nivel 2", dificultad = progresoUser.getDificultad() });
            progresoUser.SubirDificultad();
            progresoUser.GuardarProgreso();
        }
        else
        {
            Debug.LogError("ProgresoUser no está asignado en OnLevelComplete.");
        }
    }




    protected override void IniciarJuego()
    {
        CalcularNum();
        base.IniciarJuego();
    }

    public void CalcularNum()
    {
        num = Random.Range(0, 10);

        if (num == 0)
        {
            num++;
        }
        if (num == 10)
        {
            num--;
        }


        textPrin.text = $"_{num}_";
        GenerarRespuestas(num);
    }

    public void DibujarRespuestas()
    {

        int randomIndex = Random.Range(0, textRptIArray.Length);
        textRptIArray[randomIndex].text = $"{inferior[0]} | {superior[0]}";
        respuestaCorrecta = textRptIArray[randomIndex].text;

        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < textRptIArray.Length; i++)
        {
            if (i != randomIndex)
            {
                availableIndexes.Add(i);
            }
        }

        Shuffle(availableIndexes);

        for (int j = 1; j < inferior.Length; j++)
        {
            textRptIArray[availableIndexes[j - 1]].text = $"{inferior[j]} | {superior[j]}";
        }
        enEspera = false;
    }

    public void GenerarRespuestas(int numero)
    {
        // Respuesta correcta
        int respuestaCorrectaInf = Mathf.Max(numero - 1, 0);
        int respuestaCorrectaSup = Mathf.Min(numero + 1, 10);

        // Respuestas incorrectas
        List<int> respuestasIncorrectas = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            if (i != respuestaCorrectaInf && i != respuestaCorrectaSup)
            {
                respuestasIncorrectas.Add(i);
            }
        }

        // Mezclar las respuestas incorrectas
        Shuffle(respuestasIncorrectas);

        // Asignar las respuestas
        inferior = new int[] { respuestaCorrectaInf, respuestasIncorrectas[0], respuestasIncorrectas[1], respuestasIncorrectas[2], respuestasIncorrectas[3] };
        superior = new int[] { respuestaCorrectaSup, respuestasIncorrectas[4], respuestasIncorrectas[5], respuestasIncorrectas[6], respuestasIncorrectas[7] };

        DibujarRespuestas();
    }

    // Función para mezclar una lista
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
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
            StartCoroutine(OnLevelComplete());
        }
        else
        {
            Invoke("IniciarJuego", 1f);
        }
        return;
    }
}
