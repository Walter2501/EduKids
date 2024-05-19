

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    public GameObject productoPrefab; // El prefab que contiene los textos y el botón
    public Transform parentTransform; // El padre donde se instanciarán los prefabs

    private Dictionary<string, int> productos = new Dictionary<string, int>();

    void Start()
    {
        productos.Add("Producto1", 10);
        productos.Add("Producto2", 20);

        CrearPrefabs();
    }

    void CrearPrefabs()
    {
        foreach (var item in productos)
        {
            GameObject productoInstance = Instantiate(productoPrefab, parentTransform);

            TextMeshProUGUI nombreText = productoInstance.transform.Find("ProductNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI precioText = productoInstance.transform.Find("ProductPriceText").GetComponent<TextMeshProUGUI>();
            Button buyButton = productoInstance.transform.Find("BuyButton").GetComponent<Button>();

            nombreText.text = item.Key;
            precioText.text = item.Value.ToString();

            buyButton.onClick.AddListener(() => ComprarProducto(item.Key));
        }
    }

    void ComprarProducto(string productName)
    {
        Debug.Log("Compraste: " + productName);
        // Aquí puedes agregar la lógica para comprar el producto
    }
}
