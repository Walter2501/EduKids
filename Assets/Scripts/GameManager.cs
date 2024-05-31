using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Firebase.Database;

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

    public string usuarioID => PlayerPrefs.GetString(USUARIOID_KEY, "");
    private string USUARIOID_KEY = "usuarioID_Key";

    public int rol => PlayerPrefs.GetInt(ROL_KEY, -1);
    private string ROL_KEY = "rol_password";

    public DatabaseReference database;

    public Estudiante estudiante = null; //solo se llenara el del rol, los demás se quedaran vacios
    public Maestro maestro = null;

    public int cantidadMeritos;
    public string nombre;
    public string apellido;

    private void Awake()
    {
        instance = this;
        database = FirebaseDatabase.DefaultInstance.RootReference;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Debug.Log($"Id: {usuarioID}");
        Debug.Log($"rol: {rol}");
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PlayerPrefs.DeleteKey(USUARIOID_KEY);
            PlayerPrefs.DeleteKey(ROL_KEY);
        }
#endif
    }

    public void AddMeritos(int cantidad_to_add)
    {

    }

    public void SetUserID(string newID)
    {
        PlayerPrefs.SetString(USUARIOID_KEY, newID);
        PlayerPrefs.Save();
    }

    public void SetRol(int newRol)
    {
        PlayerPrefs.SetInt(ROL_KEY, newRol);
        PlayerPrefs.Save();
        //rol seteado
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
