using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestionEstudiantes : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform container;
    [SerializeField] private Button botonVolver;
    [SerializeField] private TextMeshProUGUI codigo ;

    private List<string> estudiantesID = new List<string>();

    private void Start()
    {
        codigo.text = $"Código: {GameManager.Instance.maestro.CodigoMaestro}"; //muetra el codigo del profe
        estudiantesID = GameManager.Instance.maestro.EstudiantesIDs; //consigue el id de los estudiantes
        botonVolver.onClick.AddListener(Volver);
        DibujarNombreEstudiantes();
    }

    public void DibujarNombreEstudiantes()
    {
        if (estudiantesID.Count == 0) return; //si no hay estudiantes no hace nada
        for (int i = 0; i < estudiantesID.Count; i++) //si hay estudiantes hace esto por cada uno
        {
            GameObject newObj = Instantiate(prefab, container); //crea el prefab
            EstudianteNombrePrefab newEstudent = newObj.GetComponent<EstudianteNombrePrefab>(); //consigue su script
            newEstudent.SetName(estudiantesID[i]); //le manda al prefab el userID del estudiante
        }
    }

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuProfesor");
    }
}
