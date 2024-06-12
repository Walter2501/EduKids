using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuJuegos : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI meritosText;
    [SerializeField] private Button botonVolver;

    private void Start()
    {
        nombreText.text = $"{GameManager.Instance.estudiante.Nombre} {GameManager.Instance.estudiante.Apellido1}";
        meritosText.text = $"Méritos: {GameManager.Instance.estudiante.Meritos}";
        botonVolver.onClick.AddListener(Volver);
    }

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuEstudiante");
    }
}
