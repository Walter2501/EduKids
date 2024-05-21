using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JuegoFiguras : MonoBehaviour
{
    [SerializeField] private FigurasGeometricas[] figuras;
    [SerializeField] private TextMeshProUGUI[] textosBotones;
    [SerializeField] private Image figuraImg;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private int puntosPorRespuestaCorrecta;

    private int puntosTotales = 0;
    private int vecesJugado = 1;
    private bool enEspera = false;
    private AudioSource audioSource;
    private string ladosCorrectos = "";
    private FigurasGeometricas figuraElegida;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ElegirFiguras();
    }

    private void ElegirFiguras()
    {
        figuraElegida = figuras[Random.Range(0, figuras.Length)];
        figuraImg.sprite = figuraElegida.FiguraImg;
        ladosCorrectos = figuraElegida.Lados;

        GenerarRespuestas();
    }

    private void GenerarRespuestas()
    {
        List<string> listTemp = new List<string>();

        for (int i = 0; i < 11; i++)
        {
            listTemp.Add($"{i}");
        }

        listTemp.Remove(ladosCorrectos);

        listTemp = listTemp.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < textosBotones.Length; i++)
        {
            textosBotones[i].text = listTemp[i];
        }

        textosBotones[Random.Range(0, textosBotones.Length)].text = ladosCorrectos;

        enEspera = false;
    }

    public void PresionarBoton(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        if (texto.text == ladosCorrectos)
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
            Invoke("ElegirFiguras", 1f);
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
