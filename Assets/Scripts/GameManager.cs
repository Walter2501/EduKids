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

    public string usuarioID => PlayerPrefs.GetString(USUARIOID_KEY, ""); //Se guarda el userID al iniciar sesion, para que ya no tenga que pasar por el login
    private string USUARIOID_KEY = "usuarioID_Key";

    //solo se usa si es estudiante:
    public string maestroID => PlayerPrefs.GetString(MAESTROID_KEY, ""); //Esto es para el estudiante, aqui guarda el codigo de su profe
    private string MAESTROID_KEY = "maestroID_Key";

    //solo se usa si es padre:
    public string estudianteID => PlayerPrefs.GetString(ESTUDIANTEID_KEY, ""); //Esto es para los padres, aqui guarda el codigo de su hijo
    private string ESTUDIANTEID_KEY = "estudianteID_Key";


    public int rol => PlayerPrefs.GetInt(ROL_KEY, -1); //guarda el rol, por la misma razón que el userID se guarda, pero este es para saber a que menu ir
    private string ROL_KEY = "rol_password";

    public DatabaseReference database; //guarda la referencia del database

    public Estudiante estudiante = null; //solo se llenara el del rol, los dem�s se quedaran vacios
    public Maestro maestro = null;

    private void Awake()
    {
        instance = this;
        database = FirebaseDatabase.DefaultInstance.RootReference;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

        Debug.Log($"Id: {usuarioID} - rol: {rol}");
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F10)) //solo funciona en el editor, borra los datos locales, al hacerlo se tiene que volver a iniciar sesion la proxima vez que se ponga play
        {
            PlayerPrefs.DeleteKey(USUARIOID_KEY);
            PlayerPrefs.DeleteKey(ROL_KEY);
        }
#endif
    }

    public void SetUserID(string newID)
    {
        PlayerPrefs.SetString(USUARIOID_KEY, newID);
        PlayerPrefs.Save();
    }

    public void SetProfesorUserID(string profesorID)
    {
        PlayerPrefs.SetString(MAESTROID_KEY, profesorID);
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

    public void ReiniciarEscenaActual()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




}
