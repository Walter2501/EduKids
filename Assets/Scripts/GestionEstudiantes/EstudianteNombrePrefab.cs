using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class EstudianteNombrePrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreText;
    private string nombre;

    public void SetName(string estudianteID) //consigo el userID
    {
        nombre = SplitCamelCase(estudianteID); //manda a separar el id
        nombreText.text = $"- {nombre}"; //asigna el nombre en el texto
    }

    private string SplitCamelCase(string input) //Pone separaciones antes de una mayuscula
    {
        //Ejem:
        //JohnDoeDoe que sería el userID se vuelve
        //John Doe Doe
        return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }
}
