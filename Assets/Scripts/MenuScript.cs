using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI meritosText;

    private void Start()
    {
        meritosText.text = $"Méritos: {GameManager.Instance.cantidadMeritos}";
    }
}
