using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public GameObject storeItemPrefab;  // Prefab del ItemContainer
    public Transform ownerContainer;    // Contenedor para el dueño
    public Transform customerContainer; // Contenedor para el cliente

    public TMP_InputField itemNameInput; // Campo de texto para el nombre del ítem
    public TMP_InputField itemPriceInput; // Campo de texto para el precio del ítem

    private List<GameObject> items = new List<GameObject>();

    // Método para añadir ítem desde los campos de texto
    public void AddItemToStore()
    {
        string itemName = itemNameInput.text;
        string itemPrice = itemPriceInput.text;
        AddItem(itemName, itemPrice);
    }

    // Método para agregar un nuevo ítem
    public void AddItem(string name, string price)
    {
        // Crear ítem en el panel del dueño
        GameObject ownerItem = Instantiate(storeItemPrefab, ownerContainer);
        SetupItem(ownerItem, name, price, true);

        // Crear ítem en el panel del cliente
        GameObject customerItem = Instantiate(storeItemPrefab, customerContainer);
        SetupItem(customerItem, name, price, false);

        items.Add(ownerItem);
        items.Add(customerItem);
    }

    // Configuración del ítem
    private void SetupItem(GameObject item, string name, string price, bool isOwner)
    {
        TextMeshProUGUI nameText = item.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceText = item.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>();
        nameText.text = name;
        priceText.text = price;

        if (isOwner)
        {
            Button deleteButton = item.transform.Find("btnDelet").GetComponent<Button>();
            Button editButton = item.transform.Find("btnEdit").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteItem(item));
            editButton.onClick.AddListener(() => EditItem(nameText, priceText));
        }
        else
        {
            Button buyButton = item.transform.Find("btnBuy").GetComponent<Button>();
            buyButton.onClick.AddListener(() => BuyItem(item));
        }
    }

    // Método para eliminar un ítem
    private void DeleteItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    // Método para editar un ítem
    private void EditItem(TextMeshProUGUI nameText, TextMeshProUGUI priceText)
    {
        nameText.text = itemNameInput.text;
        priceText.text = itemPriceInput.text;
    }

    // Método para comprar un ítem
    private void BuyItem(GameObject item)
    {
        Debug.Log("Item bought: " + item.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text);
        // Lógica adicional para la compra del ítem
    }
}
