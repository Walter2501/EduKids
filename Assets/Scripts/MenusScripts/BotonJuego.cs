using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BotonJuego : MonoBehaviour
{
    [SerializeField] private string escenaCargar;
    [SerializeField] private Image imagen;
    [SerializeField] private string nombre;
    [SerializeField] private int juego;
    [SerializeField] private GameObject bloqueo;
    [SerializeField] private PanelElegirDificultad panelDificultad;

    private TextMeshProUGUI textMeshProUGUI;
    private Button thisBoton;

    private void Awake()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        thisBoton = GetComponent<Button>();
    }

    private void Start()
    {
        textMeshProUGUI.text = nombre;
        //el juego 1 siempre esta desbloqueado
        if (juego > 1) 
        {
            //el juego se bloquea si el anterior a el no se ha completado la dificultad 2
            if (GameManager.Instance.estudiante.progreso.GetDificultadMax(juego - 1) < 3)
            {
                bloqueo.SetActive(true);
                thisBoton.interactable = false;
            }
        }
    }

    public void CargarBoton()
    {
        panelDificultad.gameObject.SetActive(true);
        panelDificultad.SetGame(juego, escenaCargar, nombre);
    }
}