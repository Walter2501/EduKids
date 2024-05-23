using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI meritosText;
    [SerializeField] private TextMeshProUGUI nombreText;

    private void Start()
    {
        if (GameManager.Instance.rol == 0) meritosText.text = $"Méritos: {GameManager.Instance.cantidadMeritos}";
        nombreText.text = $"{GameManager.Instance.nombre} {GameManager.Instance.apellido}";
    }

    public void CambiarEscena(string NombreEscena)
    {
        GameManager.Instance.CambiarEscena(NombreEscena);
    }
}
