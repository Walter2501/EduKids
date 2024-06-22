using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Rol : MonoBehaviour
{
    [SerializeField] private GameObject prefabUsuario;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject panelCargando;
    private RolManager rolManager;

    private List<UsuarioBase> usuariosList = new List<UsuarioBase>();

    private void Start()
    {
        rolManager = FindObjectOfType<RolManager>();
        if (rolManager == null)
        {
            Debug.LogError("RolManager no encontrado en la escena.");
        }
    }

    public void OnUsuariosLoaded(List<UsuarioBase> usuarios)
    {
        usuariosList = usuarios;
        DibujarUsuarios();
        panelCargando.SetActive(false);
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
            rolesDropdown.AddOptions(new List<string> { "Estudiante", "Padre", "Maestro" });
            rolesDropdown.value = usuario.Rol;

            rolesDropdown.onValueChanged.AddListener((index) => OnRolesDropdownValueChanged(rolesDropdown, usuario));

            Button eliminarButton = usuarioGO.GetComponentInChildren<Button>();
            eliminarButton.onClick.AddListener(() => btnEliminar(usuario));
        }
    }

    private void OnRolesDropdownValueChanged(TMP_Dropdown dropdown, UsuarioBase usuario)
    {
        if (rolManager != null)
        {
            int nuevoRol = dropdown.value;
            panelCargando.SetActive(true);
            GameObject usuarioGO = Instantiate(prefabUsuario, container);
            TextMeshProUGUI nombreText = usuarioGO.GetComponentInChildren<TextMeshProUGUI>();
            nombreText.text = $"{usuario.Nombre}{usuario.Apellido1}{usuario.Apellido2}";
            rolManager.CambiarRolUsuario(nombreText.text, nuevoRol); // Usar Id en lugar de concatenar el nombre
        }
        else
        {
            Debug.LogError("RolManager es nulo.");
        }
    }

    private void btnEliminar(UsuarioBase usuario)
    {
        try
        {
            if (rolManager != null)
            {
                panelCargando.SetActive(true);
                GameObject usuarioGO = Instantiate(prefabUsuario, container);
                TextMeshProUGUI nombreText = usuarioGO.GetComponentInChildren<TextMeshProUGUI>();
                nombreText.text = $"{usuario.Nombre}{usuario.Apellido1}{usuario.Apellido2}";
                rolManager.EliminarUsuario(nombreText.text); // Usar Id en lugar de concatenar el nombre
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"El error es: {e}");
        }
    }
}
