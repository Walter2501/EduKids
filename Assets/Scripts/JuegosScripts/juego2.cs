using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class juego2 : MonoBehaviour
{
    public TextMeshProUGUI textPrin;
    public TextMeshProUGUI[] textRptIArray;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private int puntosPorRespuestaCorrecta;

    private int puntosTotales = 0;
    private int vecesJugado = 1;
    private bool enEspera = false;
    private AudioSource audioSource;
    int num;
    int[] inferior;
    int[] superior;
    string respuestaCorrecta = "";

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        calcularNum();
    }


    public void calcularNum()
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
        generarRespuestas(num);
    }

    public void dibujarRespuestas()
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

    public void generarRespuestas(int numero)
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

        dibujarRespuestas();
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

    public void PresionarBoton(TextMeshProUGUI textoBoton)
    {
        if (enEspera) return;

        if (textoBoton.text == respuestaCorrecta)
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
            Invoke("calcularNum", 1f);
        }
        return;
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
