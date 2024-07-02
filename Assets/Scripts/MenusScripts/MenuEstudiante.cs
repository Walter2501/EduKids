using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuEstudiante : MonoBehaviour
{
    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI meritosText;
    public Button ruta;
    public Button actividades;
    public Button recompensas;

    private void Start()
    {
        //Asigno las funciones a cada boton
        recompensas.onClick.AddListener(IrRecompensas);
        actividades.onClick.AddListener(IrActividades);
        ruta.onClick.AddListener(IrRuta);
        //Pongo el nombre del maestro
        nombreText.text = $"{GameManager.Instance.estudiante.Nombre} {GameManager.Instance.estudiante.Apellido1}";
        meritosText.text = $"M�ritos: {GameManager.Instance.estudiante.Meritos}";
    }

    public void IrActividades()
    {
        GameManager.Instance.CambiarEscena("MenuTemp");
    }

    public void IrRecompensas()
    {
        GameManager.Instance.CambiarEscena("tienda");
    }

    public void IrRuta()
    {
        GameManager.Instance.CambiarEscena("RutaVista");
    }
}
