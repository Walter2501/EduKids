using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JuegoFiguras : JuegoBase
{
    [SerializeField] private FigurasGeometricas[] figuras;
    [SerializeField] private TextMeshProUGUI[] textosBotones;
    [SerializeField] private Image figuraImg;

    private string ladosCorrectos = "";
    private FigurasGeometricas figuraElegida;

    protected override void IniciarJuego()
    {
        ElegirFiguras();
        base.IniciarJuego();
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

    public override void BotonOpcion(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        base.BotonOpcion(texto);
        if (texto.text == ladosCorrectos)
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
            CheckIfSubirDificultad(4);
            StartCoroutine(TerminarJuego());
        }
        else
        {
            Invoke("ElegirFiguras", 1f);
        }
    }
}
