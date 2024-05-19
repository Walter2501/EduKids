using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        string textoIngresado = inputField.text;
        string textoIngresado2 = inputField1.text;
        AddItem(textoIngresado, textoIngresado2);
    }

    public void AddItem(string value, string value2)
    {
        GameObject newItem = Instantiate(itemPrefab, container);

        TMP_InputField inputComponent = newItem.transform.Find("ProductNameInput").GetComponent<TMP_InputField>();
        inputComponent.text = value;

        TMP_InputField inputComponent2 = newItem.transform.Find("ProductPriceInput").GetComponent<TMP_InputField>();
        inputComponent2.text = value2;

        Button deleteButton = newItem.transform.Find("btnEliminar").GetComponent<Button>();
        Button editButton = newItem.transform.Find("btnEditar").GetComponent<Button>();

        deleteButton.onClick.AddListener(() => DeleteItem(newItem));
        editButton.onClick.AddListener(() => EditItem(inputComponent, inputComponent2));

        items.Add(newItem);
    }

    private void DeleteItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    private void EditItem(TMP_InputField inputComponent, TMP_InputField inputComponent2)
    {


        inputComponent.text = "Nuevo Texto Editado";
        inputComponent2.text = "Nuevo Precio Editado";
    }
}
