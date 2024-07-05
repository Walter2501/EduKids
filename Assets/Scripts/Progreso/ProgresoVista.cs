using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgresoVista : MonoBehaviour
{
    [SerializeField] private List<Image> imagenes;
    [SerializeField] private ProgresoItemPrefab prefabNivelCompletado;
    [SerializeField] private Transform container;
    [SerializeField] private Sprite desbloqueado;
    [SerializeField] private Sprite noDesbloqueado;
    [SerializeField] private Button volverBoton;
    [SerializeField] private string escenaVolver;
    [SerializeField] private TextMeshProUGUI codigo;

    private List<NivelCompletado> nivelesList = new List<NivelCompletado>();

    private void Start()
    {
        if (codigo != null) codigo.text = $"Código: {GameManager.Instance.estudiante.CodigoEstudiante}";
        volverBoton.onClick.AddListener(Volver);
        nivelesList = GameManager.Instance.estudiante.nivelesCompletados;
        ChooseImages();
        DibujarNivelesCompletados();
    }

    private void ChooseImages()
    {
        for (int i = 0; i < imagenes.Count; i++)
        {
            if (GameManager.Instance.estudiante.progreso.GetDificultadMax(i+1) < 3)
            {
                imagenes[i].sprite = noDesbloqueado;
            }
            else
            {
                imagenes[i].sprite = desbloqueado;
            }
        }
    }

    private void DibujarNivelesCompletados()
    {
        if (nivelesList.Count == 0) return;
        for (int i = nivelesList.Count - 1; i >= 0;  i--)
        {
            ProgresoItemPrefab newItem = Instantiate(prefabNivelCompletado, container);
            newItem.SetTexts(nivelesList[i].nombre, nivelesList[i].dificultad, nivelesList[i].respuestasCorrectas);
        }
    }

    private void Volver()
    {
        if (GameManager.Instance.rol == 2) GameManager.Instance.estudiante = null;
        GameManager.Instance.CambiarEscena(escenaVolver);
    }
}