using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoginMenu : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject cargandoText;
    [SerializeField] private GameObject panelLogin; //login y registro
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
        StartCoroutine(CheckUser()); //empieza revisando su hay un usuarioID guardado de forma local
    }

    private IEnumerator CheckUser()
    {
        string userID = GameManager.Instance.usuarioID;

        if (userID != "") //si hay un usuarioID guardado revisa si existe
        {
            var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync(); //hace la consulta para conseguir al usuario
            yield return new WaitUntil(predicate: () => serverData.IsCompleted); //espera a que se complete la consulta

            if (serverData.IsFaulted)
            {
                Debug.LogError("Error al verificar el ID: " + serverData.Exception);
                yield break; // Salir de la corrutina si hay un error
            }

            DataSnapshot snapshot = serverData.Result;

            if (!snapshot.Exists) //comprueba si el usuario existe en la base de datos, si no existe pasa a la escena para registrarse y logearse
            {
                //Esto es en caso se borre un usuario en la bdd por cualquier razón
                Debug.Log("El usuario guardado no existe en la BDD");
                cargandoText.SetActive(false);
                panelLogin.SetActive(true);
                //debido a que si se llego hasta aquí habia un userID que ya no existe en la base de datos
                //se borra todo lo que se guardaba de forma local para evitar errores
                PlayerPrefs.DeleteAll();
                yield break; //Se cancela el resto de la corrutina
            }

            Debug.Log("Usuario Encontrado");

            string jsonData = snapshot.GetRawJsonValue();

            switch (GameManager.Instance.rol) //se guarda al usuario encontrado en base a su rol
            {
                case 0:
                    GameManager.Instance.estudiante = JsonConvert.DeserializeObject<Estudiante>(jsonData);
                    DesafioDiario.CheckAndCreateDesafio();
                    GameManager.Instance.CambiarEscena("MenuEstudiante");
                    break;
                case 1:
                    GameManager.Instance.padre = JsonConvert.DeserializeObject<Padre>(jsonData);
                    GameManager.Instance.CambiarEscena("MenuPadre");
                    break;
                case 2:
                    GameManager.Instance.maestro = JsonConvert.DeserializeObject<Maestro>(jsonData);
                    GameManager.Instance.CambiarEscena("MenuProfesor");
                    break;
            }
        }
        else //si no hay un userID pasa a la escena para registrarse y logearse
        {
            Debug.Log("No hay usuario guardado");
            PlayerPrefs.DeleteAll();
            cargandoText.SetActive(false);
            panelLogin.SetActive(true);
        }
    }

    public void IniciarSesion() //el boton de iniciar sesion
    {

        login = true; //ya que el siguiente panel luego de estos botones es el de datos,
                      //debido a eso para que se sepa que hacer con los datos(crear usuario o buscar uno existente)
                      //estan estos booleanos
        panelDatos.SetActive(true);
        panelLogin.SetActive(false);
    }

    public void Registrarse() //el boton de registrarse
    {
        register = true;
        panelRol.SetActive(true);
        panelLogin.SetActive(false);
    }

    //Datos para registro e inicio sesion
    private string nombre = "";
    private string apellido1 = "";
    private string apellido2 = "";
    private string password = "";
    private int rol = -1;

    public void SetDatos() //guarda los datos y inicia el registro o login dependiendo de que eligio el usuario al inicio
    {
        nombre = nombreInput.text;
        apellido1 = apellido1Input.text;
        apellido2 = apellido2Input.text;
        password = PasswordInput.text;

        if (nombre == "" || apellido1 == "" || apellido2 == "" || password == "")
        {
            Debug.Log("Llene todos los datos"); //si hay algun dato vacio no prosigue
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

    //lo de abajo es una region, presiona el + para desplegar el codigo y el - para ocultarlo
    #region registro

    public void setRol(int newRol) //se guarda el rol desde el panel de datos y pasa al panel para ingresar los demas datos
    {
        rol = newRol;
        panelDatos.SetActive(true);
        panelRol.SetActive(false);
    }

    private IEnumerator Registrando()
    {
        string userID = nombre + apellido1 + apellido2; //se crea el userID

        var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync(); //se hace la consulta
        yield return new WaitUntil(predicate: () => serverData.IsCompleted); //se espera a que termine la consulta

        if (serverData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
        {
            Debug.LogError("Error al verificar el ID: " + serverData.Exception);
            yield break;
        }

        DataSnapshot snapshot = serverData.Result;

        if (snapshot.Exists) //Si la consulta regresa que ya existe el usuarioID en la bdd procede a empezar el login
        {
            Debug.Log("Este usuario ya existe, inicie sesion");
            register = false;
            login = false;
            cargandoText.SetActive(false);
            panelLogin.SetActive(true);
            yield break;
        }

        Debug.Log("Este usuario no existe, registrando");

        string code = UniqueCodeGenerator.GenerateCode(userID); //Se genera un codigo unico para el usuario, solo se usa si es maestro o estudiante

        if (rol == 0 || rol == 2) //si es estudiante o maestro tambien se guarda el codigo
        {
            CodigosUsuario newCodigo = new CodigosUsuario(userID);

            string json = JsonConvert.SerializeObject(newCodigo);
            var serverCodeToRegsiter = GameManager.Instance.database.Child("Codigos").Child(code).SetRawJsonValueAsync(json);

            yield return new WaitUntil(() => serverCodeToRegsiter.IsCompleted);

            if (serverCodeToRegsiter.IsFaulted)
            {
                Debug.LogError("Error al guardar el codigo: " + serverCodeToRegsiter.Exception);
            }
            else
            {
                Debug.Log("Codigo guardado correctamente");
            }
        }

        //A partir de aqui se crea al usuario en base a su rol
        if (rol == 0)
        {
            Estudiante newEstudiante = new Estudiante(nombre, apellido1, apellido2, password, code);

            string json = JsonConvert.SerializeObject(newEstudiante);
            var serverToRegister = GameManager.Instance.database.Child("Usuarios").Child(userID).SetRawJsonValueAsync(json);

            yield return new WaitUntil(() => serverToRegister.IsCompleted);

            if (serverToRegister.IsFaulted)
            {
                Debug.LogError("Error al registrar usuario: " + serverToRegister.Exception);
            }
            else
            {
                Debug.Log("Usuario registrado correctamente");
                GameManager.Instance.SetUserID(userID); //guarda el id en el gamemanager para que el siguiente inicio de sesion se logee automaticamente
                GameManager.Instance.SetRol(rol); //se guarda el rol en el gamemanager para que el siguinte inicio de sesion obtenga el usuario en su rol correspondiente
                GameManager.Instance.estudiante = newEstudiante; //guarda la instancia de estudiante para sacar los datos de él
                GameManager.Instance.CambiarEscena("MenuEstudiante"); //cambia a la escena estudiante
            }
        }
        else if (rol == 1)
        {
            Padre newPadre = new Padre(nombre, apellido1, apellido2, password);

            string json = JsonConvert.SerializeObject(newPadre);
            var serverToRegister = GameManager.Instance.database.Child("Usuarios").Child(userID).SetRawJsonValueAsync(json);

            yield return new WaitUntil(() => serverToRegister.IsCompleted);

            if (serverToRegister.IsFaulted)
            {
                Debug.LogError("Error al registrar usuario: " + serverToRegister.Exception);
            }
            else
            {
                Debug.Log("Usuario registrado correctamente");
                GameManager.Instance.SetUserID(userID);
                GameManager.Instance.SetRol(rol);
                GameManager.Instance.padre = newPadre;
                GameManager.Instance.CambiarEscena("MenuPadre");
            }
        }
        else if (rol == 2)
        {
            Maestro newMaestro = new Maestro(nombre, apellido1, apellido2, password, code);

            string json = JsonConvert.SerializeObject(newMaestro);
            var serverToRegister = GameManager.Instance.database.Child("Usuarios").Child(userID).SetRawJsonValueAsync(json);

            yield return new WaitUntil(() => serverToRegister.IsCompleted);

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

    #endregion

    #region login

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

        if (!snapshot.Exists) //Si el usuario no existe lo regresa al inicio para que se registre
        {
            Debug.Log("Este usuario no existe, registrese");
            register = false;
            login = false;
            cargandoText.SetActive(false);
            panelLogin.SetActive(true);
            yield break;
        }

        string jsonData = snapshot.GetRawJsonValue();

        UsuarioBase dataTemp = JsonConvert.DeserializeObject<UsuarioBase>(jsonData); //se guarda el archivo de la consulta como UsuarioBase para comprobar el password y el rol

        if (dataTemp.Password != password) //se revisa si el password es correcto
        {
            Debug.Log("Contraseña Incorrecta");
            cargandoText.SetActive(false);
            panelDatos.SetActive(true);
            yield break; //si es incorrecto cancela el login
        }

        GameManager.Instance.SetUserID(userID); //se guarda el id
        GameManager.Instance.SetRol(dataTemp.Rol); //se guarda el rol

        if (dataTemp.Rol == 0)
        {
            GameManager.Instance.estudiante = JsonConvert.DeserializeObject<Estudiante>(jsonData); // ahora si se guarda como el rol correspondiente
            DesafioDiario.CheckAndCreateDesafio();
            GameManager.Instance.CambiarEscena("MenuEstudiante");
        }
        if (dataTemp.Rol == 1)
        {
            GameManager.Instance.padre = JsonConvert.DeserializeObject<Padre>(jsonData); // ahora si se guarda como el rol correspondiente
            GameManager.Instance.CambiarEscena("MenuPadre");
        }
        else if (dataTemp.Rol == 2)
        {
            GameManager.Instance.maestro = JsonConvert.DeserializeObject<Maestro>(jsonData);
            GameManager.Instance.CambiarEscena("MenuProfesor");
        }
    }

    #endregion

    public void Cancelar()
    {
        login = false;
        register = false;
        rol = -1; //por si acaso
        panelLogin.SetActive(true);
        panelDatos.SetActive(false);
    }
}