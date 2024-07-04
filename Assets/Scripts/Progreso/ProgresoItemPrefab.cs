using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgresoItemPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombre;
    [SerializeField] private TextMeshProUGUI dificultad;
    [SerializeField] private TextMeshProUGUI respuestas;

    public void SetTexts(string nombreJuego, int dificultadJuego, string respuestasCorrectas)
    {
        nombre.text = nombreJuego;
        dificultad.text = $"Dificultad: {dificultadJuego}";
        respuestas.text = respuestasCorrectas;
    }
}
