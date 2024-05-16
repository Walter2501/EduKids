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

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt(MERITOS_KEY, 0);
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

    public void CambiarEscena(int IdEscena)
    {
        SceneManager.LoadScene(IdEscena);
    }

    public void CambiarEscena(string NombreEscena)
    {
        SceneManager.LoadScene(NombreEscena);
    }
}
