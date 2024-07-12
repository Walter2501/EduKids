using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarcadorItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombre;
    [SerializeField] private TextMeshProUGUI puntuacion;

    public void SetText(Marcador marcador)
    {
        nombre.text = marcador.nombreCompleto;
        puntuacion.text = marcador.meritosTotal.ToString();
    }
}