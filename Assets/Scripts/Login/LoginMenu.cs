using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoginMenu : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject cargandoText;
    [SerializeField] private GameObject panelLogin;
    [SerializeField] private GameObject panelRol;
    [SerializeField] private GameObject panelDatos;
    [Header("Datos Registro")]
    [SerializeField] private TMP_InputField nombreInput;
    [SerializeField] private TMP_InputField apellido1Input;
    [SerializeField] private TMP_InputField apellido2Input;
    [SerializeField] private TMP_InputField PasswordInput;

    private bool login = false;
    private bool register = false;

    private void Start()
    {
        StartCoroutine(CheckUser());
    }

    private IEnumerator CheckUser()
    {
        string userID = GameManager.Instance.usuarioID;

        if (userID != "")
        {
            var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync();
            yield return new WaitUntil(predicate: () => serverData.IsCompleted);

            if (serverData.IsFaulted)
            {
                Debug.LogError("Error al verificar el ID: " + serverData.Exception);
                yield break; // Salir de la corrutina si hay un error
            }

            Debug.Log("Usuario Encontrado");

            DataSnapshot snapshot = serverData.Result;
            string jsonData = snapshot.GetRawJsonValue();

            switch (GameManager.Instance.rol)
            {
                case 0:
                    GameManager.Instance.estudiante = JsonUtility.FromJson<Estudiante>(jsonData);
                    GameManager.Instance.CambiarEscena("MenuEstudiante");
                    break;

                case 1:
                    GameManager.Instance.maestro = JsonUtility.FromJson<Maestro>(jsonData);
                    GameManager.Instance.CambiarEscena("MenuProfesor");
                    break;
            }
        }
        else
        {
            Debug.Log("No hay usuario guardado");
            cargandoText.SetActive(false);
            panelLogin.SetActive(true);
        }
    }

    public void IniciarSesion()
    {

        login = true;
        panelDatos.SetActive(true);
        panelLogin.SetActive(false);
    }

    public void Registrarse()
    {
        register = true;
        panelRol.SetActive(true);
        panelLogin.SetActive(false);
    }

    //Datos para registro
    private string nombre;
    private string apellido1;
    private string apellido2;
    private string password;
    private int rol;

    public void setRol(int newRol)
    {
        rol = newRol;
        panelDatos.SetActive(true);
        panelRol.SetActive(false);
    }

    public void SetDatos()
    {
        nombre = nombreInput.text;
        apellido1 = apellido1Input.text;
        apellido2 = apellido2Input.text;
        password = PasswordInput.text;

        if (nombre == "" || apellido1 == "" || apellido2 == "" || password == "")
        {
            Debug.Log("Llene todos los datos");
            return;
        }

        cargandoText.SetActive(true);
        if (register)
        {
            StartCoroutine(Registrando());
        }
        else if (login)
        {
            StartCoroutine(Login());
        }
        panelDatos.SetActive(false);
    }

    private IEnumerator Registrando()
    {
        if (rol ==  0)
        {
            Estudiante newEstudiante = new Estudiante(nombre, apellido1, apellido2, password);
            string userID = nombre + apellido1 + apellido2;

            var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync();
            yield return new WaitUntil(predicate: () => serverData.IsCompleted);

            if (serverData.IsFaulted)
            {
                Debug.LogError("Error al verificar el ID: " + serverData.Exception);
                yield break;
            }

            DataSnapshot snapshot = serverData.Result;

            if (snapshot.Exists)
            {
                Debug.Log("Este usuario ya existe, iniciando sesion");
                StartCoroutine(Login());
            }
            else
            {
                Debug.Log("Este usuario no existe, registrando");
                string json = JsonUtility.ToJson(newEstudiante);
                var serverToRegister = GameManager.Instance.database.Child("Usuarios").Child(userID).SetRawJsonValueAsync(json);

                if (serverToRegister.IsFaulted)
                {
                    Debug.LogError("Error al registrar usuario: " + serverToRegister.Exception);
                }
                else
                {
                    Debug.Log("Usuario registrado correctamente");
                    GameManager.Instance.SetUserID(userID);
                    GameManager.Instance.SetRol(rol);
                    GameManager.Instance.estudiante = newEstudiante;
                    GameManager.Instance.CambiarEscena("MenuEstudiante");
                }
            }
        }
        else if (rol == 1)
        {
            Maestro newMaestro = new Maestro(nombre, apellido1, apellido2, password);
            string userID = nombre + apellido1 + apellido2;

            var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync();
            yield return new WaitUntil(predicate: () => serverData.IsCompleted);

            if (serverData.IsFaulted)
            {
                Debug.LogError("Error al verificar el ID: " + serverData.Exception);
                yield break;
            }

            DataSnapshot snapshot = serverData.Result;

            if (snapshot.Exists)
            {
                Debug.Log("Este usuario ya existe, iniciando sesion");
                Login();
            }
            else
            {
                Debug.Log("Este usuario no existe, registrando");
                string json = JsonUtility.ToJson(newMaestro);
                var serverToRegister = GameManager.Instance.database.Child("Usuarios").Child(userID).SetRawJsonValueAsync(json);

                if (serverToRegister.IsFaulted)
                {
                    Debug.LogError("Error al registrar usuario: " + serverToRegister.Exception);
                }
                else
                {
                    Debug.Log("Usuario registrado correctamente");
                    GameManager.Instance.SetUserID(userID);
                    GameManager.Instance.SetRol(rol);
                    GameManager.Instance.maestro = newMaestro;
                    GameManager.Instance.CambiarEscena("MenuProfesor");
                }
            }
        }
    }

    private IEnumerator Login()
    {
        string userID = nombre + apellido1 + apellido2;

        var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync();
        yield return new WaitUntil(() => serverData.IsCompleted);

        if (serverData.IsFaulted)
        {
            Debug.LogError("Error al verificar el ID: " + serverData.Exception);
            yield break; // Salir de la corrutina si hay un error
        }

        Debug.Log("Usuario Encontrado");

        DataSnapshot snapshot = serverData.Result;

        if (!snapshot.Exists)
        {
            Debug.Log("Este usuario no existe, registrese");
            register = false;
            login = false;
            cargandoText.SetActive(false);
            panelLogin.SetActive(true);
            yield break;
        }

        string jsonData = snapshot.GetRawJsonValue();

        UsuarioBase dataTemp = JsonUtility.FromJson<UsuarioBase>(jsonData);

        if (dataTemp.Rol == 0)
        {
            GameManager.Instance.SetUserID(userID);
            GameManager.Instance.SetRol(dataTemp.Rol);
            GameManager.Instance.estudiante = JsonUtility.FromJson<Estudiante>(jsonData);
            GameManager.Instance.CambiarEscena("MenuEstudiante");
        }
        else if (dataTemp.Rol == 1)
        {
            GameManager.Instance.SetUserID(userID);
            GameManager.Instance.SetRol(dataTemp.Rol);
            GameManager.Instance.maestro = JsonUtility.FromJson<Maestro>(jsonData);
            GameManager.Instance.CambiarEscena("MenuProfesor");
        }
    }
}