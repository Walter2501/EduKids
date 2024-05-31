using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private GameObject panelRegistro;
    [SerializeField] private GameObject buttonEstudiante;
    [SerializeField] private GameObject buttonMaestro;
    [SerializeField] private TMP_InputField nombreInput;
    [SerializeField] private TMP_InputField apellidoInput;

    private void Start()
    {
        DetectRol();
    }

    private void Update()
    {
    }

    private void DetectRol()
    {
        
        if (GameManager.Instance.rol == 0)
        {
            LoadEstudiante();
        }
        else if (GameManager.Instance.rol == 1)
        {
            LoadMaestro();
        }
    }

    public void BotonEstudiante()
    {
        GameManager.Instance.SetRol(0);
        panelRegistro.SetActive(true);
        buttonMaestro.SetActive(false);
        buttonEstudiante.SetActive(false);
    }

    public void BotonMaestro()
    {
        GameManager.Instance.SetRol(1);
        panelRegistro.SetActive(true);
        buttonEstudiante.SetActive(false);
        buttonMaestro.SetActive(false);
    }

    public void BotonListo()
    {
        string nombre = nombreInput.text;
        string apellido = apellidoInput.text;
        if (nombre == "" | apellido == "") return;

        GameManager.Instance.SetNombreApellido(nombre, apellido);
        DetectRol();
    }

    private void LoadEstudiante()
    {
        GameManager.Instance.CambiarEscena("MenuEstudiante");
    }

    private void LoadMaestro()
    {
        GameManager.Instance.CambiarEscena("MenuProfesor");
    }
}