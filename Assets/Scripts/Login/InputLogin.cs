using TMPro;
using UnityEngine;

public class InputLogin : MonoBehaviour
{
    private TMP_InputField input;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    private void Start()
    {
        input.onValueChanged.AddListener(RemoveSpaces);
    }

    private void RemoveSpaces(string text)
    {
        // Reemplaza todos los espacios en el texto con una cadena vacía
        // Para que no pongan espacios al poner el codigo
        input.text = text.Replace(" ", "");
    }
}
