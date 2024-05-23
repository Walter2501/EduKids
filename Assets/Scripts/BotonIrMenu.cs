using UnityEngine;

public class BotonIrMenu : MonoBehaviour
{
    public void VolverMenu()
    {
        GameManager.Instance.CambiarEscena("MenuTemp");
    }
}