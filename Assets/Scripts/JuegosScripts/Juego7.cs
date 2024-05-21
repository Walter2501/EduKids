using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Juego7 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textoBotones;
    [SerializeField] private TextMeshProUGUI textoSuma;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private int puntosPorRespuestaCorrecta;

    private int puntosTotales = 0;
    private int vecesJugado = 1;
    private bool enEspera = false;
    private AudioSource audioSource;
    private int num1 = 0;
    private int num2 = 0;
    private int suma = 0;

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
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        suma = num1 + num2;

        textoSuma.text = $"{num1} + {num2}";

        GenerarRespuestas();
        enEspera = false;
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

    public void PresionarBoton(TextMeshProUGUI texto)
    {
        if (enEspera) return;

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

    private IEnumerator TerminarJuego()
    {
        GameManager.Instance.AddMeritos(puntosTotales);
        Debug.Log($"Terminado: {GameManager.Instance.cantidadMeritos}");
        yield return new WaitForSeconds(1);
        Debug.Log(puntosTotales);
        GameManager.Instance.CambiarEscena(0);
    }
}