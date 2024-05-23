using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager is null");
            }
            return instance;
        }
    }
    #endregion

    public int cantidadMeritos => PlayerPrefs.GetInt(MERITOS_KEY);
    private string MERITOS_KEY = "meritos_password";

    public int rol => PlayerPrefs.GetInt(ROL_KEY, -1); //0 es estudiante - 1 es maestro
    private string ROL_KEY = "rol_password";

    public string nombre => PlayerPrefs.GetString(NOMBRE_KEY, "");
    private string NOMBRE_KEY = "nombre_password";

    public string apellido => PlayerPrefs.GetString(APELLIDO_KEY, "");
    private string APELLIDO_KEY = "apellido_password";

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Debug.Log($"meritos: {cantidadMeritos} - Nombre: {nombre} - Apellido: {apellido} - Rol: {rol}");
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PlayerPrefs.SetInt(MERITOS_KEY, 0);
            PlayerPrefs.SetInt(ROL_KEY, -1);
            PlayerPrefs.SetString(NOMBRE_KEY, "");
            PlayerPrefs.SetString(APELLIDO_KEY, "");
            PlayerPrefs.Save();
        }
#endif
    }

    public void AddMeritos(int cantidad_to_add)
    {
        int nuevaCantidad = cantidadMeritos + cantidad_to_add;
        PlayerPrefs.SetInt(MERITOS_KEY, nuevaCantidad);
        PlayerPrefs.Save();
    }

    public void SetRol(int rolNumber)
    {
        if (rol != -1) return;
        PlayerPrefs.SetInt(ROL_KEY, rolNumber);
        PlayerPrefs.Save();
    }

    public void SetNombreApellido(string newNombre, string newApellido)
    {
        PlayerPrefs.SetString(NOMBRE_KEY, newNombre);
        PlayerPrefs.SetString(APELLIDO_KEY, newApellido);
        PlayerPrefs.Save();
    }

    public void CambiarEscena(int IdEscena)
    {
        SceneManager.LoadScene(IdEscena);
    }

    public void CambiarEscena(string NombreEscena)
    {
        SceneManager.LoadScene(NombreEscena);
    }
}
