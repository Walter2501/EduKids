using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonIrMenu : MonoBehaviour
{
    public void VolverMenu()
    {
        SceneManager.LoadScene(0);
    }
}