using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Ruta : MonoBehaviour
{
    // Start is called before the first frame update
    int dificultadActual = 0;
    public Button facil, intermedio, dificil;
    void Start()
    {
        loadUser();
    }

    void Update()
    {
        IrNivel();
    }

    private void loadUser()
    {
        dificultadActual = GameManager.Instance.estudiante.Progreso.dificultadActual;

        Debug.Log($"La dificultad actual es: {dificultadActual}");
    }

    private void IrNivel()
    {
        if (dificultadActual <= 1)
        {
            facil.onClick.AddListener(IrMundoFacil);
        } else if (dificultadActual == 2) 
        { 
            intermedio.onClick.AddListener(IrMundoIntermedio);
        }
        else
        {
            dificil.onClick.AddListener(IrMundoDificil);
        }
    }

    public void IrMundoFacil()
    {
        GameManager.Instance.CambiarEscena("");
    }
    public void IrMundoIntermedio()
    {
        GameManager.Instance.CambiarEscena("");
    }
    public void IrMundoDificil()
    {
        GameManager.Instance.CambiarEscena("");
    }
}
