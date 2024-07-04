using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JuegoReloj : JuegoBase
{
    [SerializeField] private Reloj[] relojsSO;
    [SerializeField] private TextMeshProUGUI[] textosHoras;
    [SerializeField] private Image relojImg;

    private Reloj relojElegido;
    private string[][] horas_minutos =
    {
        new string[] {"01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00"},
        new string[] {"01:15", "02:15", "03:15", "04:15", "05:15", "06:15", "07:15", "08:15", "09:15", "10:15", "11:15", "12:15"},
        new string[] {"01:30", "02:30", "03:30", "04:30", "05:30", "06:30", "07:30", "08:30", "09:30", "10:30", "11:30", "12:30"},
        new string[] {"01:45", "02:45", "03:45", "04:45", "05:45", "06:45", "07:45", "08:45", "09:45", "10:45", "11:45", "12:45"},
    };
    private string horaCorrecta = "";

    protected override void IniciarJuego()
    {
        relojElegido = relojsSO[Random.Range(0, relojsSO.Length)];

        relojImg.sprite = relojElegido.RelojImg;
        horaCorrecta = relojElegido.Hora;

        GenerarHoras();
        base.IniciarJuego();
    }

    private void GenerarHoras()
    {
        HashSet<string> horasTemp = new HashSet<string>();
        horasTemp.Add(horaCorrecta);

        while (horasTemp.Count < 5)
        {
            horasTemp.Add(horas_minutos[Random.Range(0, 4)][Random.Range(0, 12)]);
        }

        List<string> horasBarajadas = new List<string>();

        foreach (string hora in horasTemp)
        {
            horasBarajadas.Add(hora);
        }

        horasBarajadas = horasBarajadas.OrderBy(x => Random.value).ToList();

        EscribirHoras(horasBarajadas);
    }

    private void EscribirHoras(List<string> horasParaEscribir)
    {
        for (int i = 0; i < textosHoras.Length; i++)
        {
            textosHoras[i].text = horasParaEscribir[i];
        }
    }

    public override void BotonOpcion(TextMeshProUGUI texto)
    {
        if (enEspera) return;
        base.BotonOpcion(texto);

        if (texto.text == horaCorrecta)
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
            CheckIfSubirDificultad(6);
            StartCoroutine(TerminarJuego());
        }
        else
        {
            Invoke("IniciarJuego", 1f);
        }
        return;
    }
}
