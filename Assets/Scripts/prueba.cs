using GraphQL;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

//script para pruebas, cambiar a su gusto solo funciona en la escena pruebas
public class prueba : MonoBehaviour
{
    private void Start()
    {
        string abd = UniqueCodeGenerator.GenerateCode("WalterAristaValqui");

        Debug.Log(abd);
    }
}
