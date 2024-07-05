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
    [SerializeField] private TextMeshProUGUI codigo;
    [SerializeField] private GameObject cargando;

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
        if (estudiantesID.Count == 0)
        {
            cargando.SetActive(false);
            return;
            //si no hay estudiantes no hace nada
        }
        for (int i = 0; i < estudiantesID.Count; i++) //si hay estudiantes hace esto por cada uno
        {
            GameObject newObj = Instantiate(prefab, container); //crea el prefab
            PrefabEstudianteGestion newEstudent = newObj.GetComponent<PrefabEstudianteGestion>(); //consigue su script
            newEstudent.SetName(estudiantesID[i]); //le manda al prefab el userID del estudiante
        }
        cargando.SetActive(false);
    }

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuProfesor");
    }
}
