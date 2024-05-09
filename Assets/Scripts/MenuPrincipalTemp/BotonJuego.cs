using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BotonJuego : MonoBehaviour
{
    [SerializeField] private int escanaCargar;
    [SerializeField] private Image imagen;
    [SerializeField] private string nombre;

    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        textMeshProUGUI.text = nombre;
    }

    public void CargarBoton()
    {
        SceneManager.LoadScene(escanaCargar);
    }
}