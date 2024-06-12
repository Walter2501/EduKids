using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Rol : MonoBehaviour
{
    [SerializeField] private GameObject prefabUsuario;
    [SerializeField] private Transform container;
    private FirebaseManager firebaseManager;

    private List<UsuarioBase> usuariosList = new List<UsuarioBase>();

    private void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager no encontrado en la escena.");
        }
    }

    public void OnUsuariosLoaded(List<UsuarioBase> usuarios)
    {
        usuariosList = usuarios;
        DibujarUsuarios();
    }

    private void DibujarUsuarios()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (var usuario in usuariosList)
        {
            GameObject usuarioGO = Instantiate(prefabUsuario, container);
            TextMeshProUGUI nombreText = usuarioGO.GetComponentInChildren<TextMeshProUGUI>();
            nombreText.text = $"{usuario.Nombre} {usuario.Apellido1} {usuario.Apellido2}";

            TMP_Dropdown rolesDropdown = usuarioGO.GetComponentInChildren<TMP_Dropdown>();
            rolesDropdown.ClearOptions();
            rolesDropdown.AddOptions(new List<string> { "Estudiante", "Padre" });
            rolesDropdown.value = usuario.Rol;

            // Set callback for when the dropdown value changes
            rolesDropdown.onValueChanged.AddListener((index) => OnRolesDropdownValueChanged(rolesDropdown, usuario.Nombre));


            Button eliminarButton = usuarioGO.GetComponentInChildren<Button>();
            eliminarButton.onClick.AddListener(() => btnEliminar(usuario.Nombre));
        }
    }

    // Método que se llama cuando cambia la selección del Dropdown
    private void OnRolesDropdownValueChanged(TMP_Dropdown dropdown, string nombreUsuario)
    {
        if (firebaseManager != null)
        {
            int nuevoRol = dropdown.value;
            firebaseManager.CambiarRolUsuario(nombreUsuario, nuevoRol);
        }
        else
        {
            Debug.LogError("FirebaseManager es nulo.");
        }
    }

    private void btnEliminar(string nombreUsuario)
    {
        try
        {
            if (firebaseManager != null)
            {
                firebaseManager.ElimincarUsuario(nombreUsuario);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"El error es: {e}");
        }
    }
}
