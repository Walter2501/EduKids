using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Random;

public class juego4 : MonoBehaviour
{
    public Texture2D[] imagenes;
    public TextMeshProUGUI[] txtRpt;
    public RawImage imagenUI;

    Dictionary<string, int> imagenValor = new Dictionary<string, int>
    {
        {"circulo", 0 },
        {"triangulo", 3 },
        {"cuadrado", 4 },
        {"pentagono", 5 },
        {"hexagono", 6 },
        {"heptagono", 7 },
        {"octagono", 8 },
        {"eneagono", 9 },
        {"decagono", 10 },
    };

    void Start()
    {
        mostrarImagen();
    }

    public void mostrarImagen()
    {
        if (imagenes.Length == 0 || txtRpt.Length == 0)
        {
            Debug.LogError("No se han asignado imágenes o cajas de respuesta.");
            return;
        }


        int randomImageIndex = Range(0, imagenes.Length);
        Texture2D imagenSeleccionada = imagenes[randomImageIndex];
        imagenUI.texture = imagenSeleccionada;


        txtRpt[0].text = $"{imagenValor[imagenSeleccionada.name]}";


        List<int> respuestasUnicas = new List<int>(imagenValor.Values);
        respuestasUnicas.Remove(imagenValor[imagenSeleccionada.name]); 


        shuffle(respuestasUnicas);


        for (int i = 1; i < txtRpt.Length; i++)
        {
            if (respuestasUnicas.Count > 0)
            {
                int randomIndex = Range(0, respuestasUnicas.Count);
                txtRpt[i].text = $"{respuestasUnicas[randomIndex]}";
                respuestasUnicas.RemoveAt(randomIndex);
            }
        }
    }

    void shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


}
