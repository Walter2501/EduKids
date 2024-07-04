using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelElegirDificultad : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] private Button Dificultad1Boton;
    [SerializeField] private Button Dificultad2Boton;
    [SerializeField] private Button Dificultad3Boton;
    [SerializeField] private Button CancelarBoton;

    [Header("Bloqueos")]
    [SerializeField] private GameObject Bloqueo2;
    [SerializeField] private GameObject Bloqueo3;

    private string gameScene = "";

    private void Start()
    {
        Dificultad1Boton.onClick.AddListener(SetDificultad1);
        Dificultad2Boton.onClick.AddListener(SetDificultad2);
        Dificultad3Boton.onClick.AddListener(SetDificultad3);
        CancelarBoton.onClick.AddListener(Cancelar);
    }

    public void SetGame(int numeroJuego, string nombreEscena, string nombreJuego)
    {
        gameScene = nombreEscena;
        int dificultad = GameManager.Instance.estudiante.progreso.GetDificultadMax(numeroJuego);

        if (dificultad == 1)
        {
            Dificultad2Boton.interactable = false;
            Bloqueo2.SetActive(true);
            Dificultad3Boton.interactable = false;
            Bloqueo3.SetActive(true);
        }
        else if (dificultad == 2)
        {
            Dificultad3Boton.interactable = false;
            Bloqueo3.SetActive(true);
        }
        else
        {
            Dificultad2Boton.interactable = true;
            Dificultad3Boton.interactable = true;
        }

        GameManager.Instance.nombreJuego = nombreJuego;
    }

    private void SetDificultad1()
    {
        GameManager.Instance.dificultadJuego = 1;
        GameManager.Instance.CambiarEscena(gameScene);
    }

    private void SetDificultad2()
    {
        GameManager.Instance.dificultadJuego = 2;
        GameManager.Instance.CambiarEscena(gameScene);
    }

    private void SetDificultad3()
    {
        GameManager.Instance.dificultadJuego = 3;
        GameManager.Instance.CambiarEscena(gameScene);
    }

    private void Cancelar()
    {
        gameScene = "";
        GameManager.Instance.nombreJuego = "";
        Bloqueo2.SetActive(false);
        Bloqueo3.SetActive(false);
        gameObject.SetActive(false);
    }
}