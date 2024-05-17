using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class recompensas : MonoBehaviour
{
    public GameObject itemPrefab;  
    public Transform container;    

    private List<GameObject> items = new List<GameObject>();

    public TMP_InputField inputField, inputField1; 

    public void AsignarTextoDesdeCampo()
    {
        string textoIngresado = inputField.text + inputField1.text;
        AddItem(textoIngresado);
    }

    public void AddItem(string value)
    {
        GameObject newItem = Instantiate(itemPrefab, container);

        TextMeshProUGUI textComponent = newItem.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        textComponent.text = value; 

        Button deleteButton = newItem.transform.Find("DeleteButton").GetComponent<Button>();
        Button editButton = newItem.transform.Find("EditButton").GetComponent<Button>();

        deleteButton.onClick.AddListener(() => DeleteItem(newItem));
        editButton.onClick.AddListener(() => EditItem(textComponent));

        items.Add(newItem);
    }

    private void DeleteItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    private void EditItem(TextMeshProUGUI textComponent)
    {

        textComponent.text = "Nuevo Texto Editado";  
    }
}
