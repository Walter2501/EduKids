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
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        suma = num1 + num2;

        textoSuma.text = $"{num1} + {num2}";

        GenerarRespuestas();
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
        if (texto.text == $"{suma}")
        {
            Debug.Log("Respuesta correcta");
            audioSource.clip = win;
            audioSource.Play();
            //SceneManager.LoadScene(0);
        }

        else
        {
            Debug.Log("Respuesta incorrecta");
            audioSource.clip = wrong;
            audioSource.Play();
            //SceneManager.LoadScene(0);
        }
    }
}