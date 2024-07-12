using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuEstudiante : MonoBehaviour
{
    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI meritosText;
    public Button actividades;
    public Button recompensas;
    public Button desafios;
    public Button progreso;
    public Button marcador;

    private void Start()
    {
        //Asigno las funciones a cada boton
        recompensas.onClick.AddListener(IrRecompensas);
        actividades.onClick.AddListener(IrActividades);
        progreso.onClick.AddListener(IrProgreso);
        marcador.onClick.AddListener(IrMarcador);
        //Pongo el nombre del maestro
        nombreText.text = $"{GameManager.Instance.estudiante.Nombre} {GameManager.Instance.estudiante.Apellido1}";
        meritosText.text = $"Méritos: {GameManager.Instance.estudiante.Meritos}";
        if (GameManager.Instance.desafio != null)
        {
            if (GameManager.Instance.desafio.completado == false)
            {
                desafios.interactable = true;
                desafios.onClick.AddListener(EmpezarDesafio);
            }
        }
    }

    public void IrActividades()
    {
        GameManager.Instance.CambiarEscena("MenuTemp");
    }

    public void IrRecompensas()
    {
        GameManager.Instance.CambiarEscena("tienda");
    }

    public void IrProgreso()
    {
        GameManager.Instance.CambiarEscena("ProgresoEstudiante");
    }

    public void IrMarcador()
    {
        GameManager.Instance.CambiarEscena("Marcador");
    }

    public void EmpezarDesafio()
    {
        GameManager.Instance.jugarDesafio = true;
        GameManager.Instance.juegosJugados++;
        GameManager.Instance.CambiarEscena(GameManager.Instance.desafio.juegosName[GameManager.Instance.juegosJugados-1]);
    }
}
