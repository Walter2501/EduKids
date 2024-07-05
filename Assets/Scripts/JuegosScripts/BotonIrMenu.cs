using UnityEngine;

public class BotonIrMenu : MonoBehaviour
{
    public void VolverMenu()
    {
        if (GameManager.Instance.jugarDesafio == true)
        {
            GameManager.Instance.jugarDesafio = false;
            GameManager.Instance.puntosHastaAhora = 0;
            GameManager.Instance.juegosJugados = 0;
            GameManager.Instance.CambiarEscena("MenuEstudiante");
        }
        else
        {
            GameManager.Instance.CambiarEscena("MenuTemp");
        }
    }
}