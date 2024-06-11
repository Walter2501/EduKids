using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuProfesor : MonoBehaviour
{
    public TextMeshProUGUI nombreText;
    public Button crearRecompensas;
    public Button canjeos;
    public Button gestionAlumnos;

    private void Start()
    {
        //Asigno las funciones a cada boton
        crearRecompensas.onClick.AddListener(IrRecompensas);
        canjeos.onClick.AddListener(IrCanjeos);
        gestionAlumnos.onClick.AddListener(IrGestionAlumnos);
        //Pongo el nombre del maestro
        nombreText.text = $"{GameManager.Instance.maestro.Nombre} {GameManager.Instance.maestro.Apellido1}";
    }

    public void IrRecompensas()
    {
        GameManager.Instance.CambiarEscena("RecompensasVista");
    }

    public void IrCanjeos()
    {
        GameManager.Instance.CambiarEscena("Canjeos");
    }

    public void IrGestionAlumnos()
    {
        GameManager.Instance.CambiarEscena("GestionarRoles");
    }
}
