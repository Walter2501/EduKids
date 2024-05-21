using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class recompensas : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform container;

    private List<GameObject> items = new List<GameObject>();

    public TMP_InputField inputField, inputField1;

    public void AsignarTextoDesdeCampo()
    {
        string textoIngresado = inputField.text;
        string textoIngresado2 = inputField1.text;
        AddItem(textoIngresado, textoIngresado2);
    }

    public void AddItem(string value, string value2)
    {
        GameObject newItem = Instantiate(itemPrefab, container);

        TextMeshProUGUI nombreText= newItem.transform.Find("Nombre_Text").GetComponent<TextMeshProUGUI>();
        nombreText.text = value;

        TextMeshProUGUI costoText = newItem.transform.Find("Costo_Text").GetComponent<TextMeshProUGUI>();
        costoText.text = value2;

        items.Add(newItem);
    }
}
