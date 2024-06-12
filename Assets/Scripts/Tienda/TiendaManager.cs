using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TiendaManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabArticulo;
    [SerializeField] private GameObject panelCargando;
    [SerializeField] private GameObject panelRecompensas;
    [SerializeField] private GameObject panelCodigo;
    [SerializeField] private TMP_InputField codigoText;
    [SerializeField] private Button botonVolver;
    [SerializeField] private Button botonDatos;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI meritos;
    [SerializeField] private TextMeshProUGUI textoAlerta;

    private List<RecompensaData> recompensasList;

    private void Start()
    {
        botonVolver.onClick.AddListener(Volver);
        if (!CheckIfCodeSaved())
        {
            panelCodigo.SetActive(true); //si no hay codigo guardado muestra un panel para introducirlo
            panelCargando.SetActive(false);

            botonDatos.onClick.AddListener(GetInputCode);
            codigoText.onValueChanged.AddListener(RemoveSpaces);
        }
        else //si hay codigo simplemente consigue las recompensa y las dibuja;
        {
            
        }
    }

    private bool CheckIfCodeSaved() //Revisa si el estudiante tiene el userID del profe guardado
    {
        if (GameManager.Instance.maestroID != "")
        {
            return true; //si lo hay regresa verdadero
        }
        return false; //sino regresa falso
    }

    private void ActualizarMeritos()
    {
        meritos.text = $"Méritos: {GameManager.Instance.cantidadMeritos}";
    }

    private void DibujarArticulos()
    {
        if (recompensasList.Count == 0) return;
        for (int i = 0; i < recompensasList.Count; i++)
        {
            GameObject newObj = Instantiate(prefabArticulo, container);
            ArticuloTienda newArticulo = newObj.GetComponent<ArticuloTienda>();
            newArticulo.SetData(recompensasList[i]);
            newArticulo.DibujarInfo();
        }
    }

    private void GetInputCode() //recibe el codigo escrito en el input
    {
        if (codigoText.text.Length != 8) //si el codigo recibido no tiene exactamente 8 digitos manda la alerta en un texto y cancela el resto
        {
            textoAlerta.text = "Ingrese 8 digitos";
            return;
        }
        //si lo anterior no se cumple hara lo siguiente:
        panelCargando.SetActive(true); //activa el cargando mientras revisa la bdd
        panelCodigo.SetActive(false);

        string codigoToCheck = codigoText.text; //guardo una copia del codigo por si acaso
    }

    private void RemoveSpaces(string text)
    {
        // Reemplaza todos los espacios en el texto con una cadena vacía
        // Para que no pongan espacios al poner el codigo
        codigoText.text = text.Replace(" ", "");
    }

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuEstudiante");
    }
}
