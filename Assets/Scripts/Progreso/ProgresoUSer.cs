using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgresoUser : MonoBehaviour
{
    private int valorActual = 0;
    private List<Nivel> nivelesCompletados = new List<Nivel>();
    private int dificultadActual = 0;


    private FirebaseManager firebaseManager; // Referencia a FirebaseManager

    public void Inicializar(FirebaseManager manager)
    {
        firebaseManager = manager;
    }


    public int getDificultad()
    {
        return dificultadActual;
    }

    // Método para simular la finalización de un nivel
    private bool FinalizarNivel()
    {
        valorActual++;
        return true;
    }

    public void AgregarNivel(Nivel nivel)
    {
        if (FinalizarNivel())
        {
            nivelesCompletados.Add(nivel);
            GuardarProgreso(); // Guardar el progreso actualizado
        }
    }

    public void SubirDificultad()
    {
        dificultadActual++;
        GuardarProgreso(); // Guardar el progreso actualizado
    }

    // Método para guardar el progreso en Firebase
    private void GuardarProgreso()
    {
        var firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager not found in scene!");
            return;
        }

        firebaseManager.GuardarProgresoUsuario(valorActual, nivelesCompletados, dificultadActual);
    }
}


[Serializable]
public class Nivel
{

    public string nombre;
    public int dificultad;

}
