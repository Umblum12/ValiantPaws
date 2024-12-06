using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenModeDropdown;
    public GameObject warningPanel;

    private int originalResolutionIndex;
    private int originalFullscreenModeIndex;

    private bool hasUnsavedChanges = false;

    private Resolution[] resolutions;

    private void Start()
    {
        // Aseguramos que el panel de advertencia esté oculto al inicio
        warningPanel.SetActive(false);

        // Obtener las resoluciones disponibles
        resolutions = Screen.resolutions;

        // Llenar el Dropdown de resoluciones con las opciones disponibles, evitando duplicados
        resolutionDropdown.ClearOptions();
        HashSet<string> uniqueResolutions = new HashSet<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionString = resolutions[i].width + "x" + resolutions[i].height;
            if (!uniqueResolutions.Contains(resolutionString))
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionString));
                uniqueResolutions.Add(resolutionString);
            }
        }
        // Suscribirse a los cambios en los dropdowns
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        fullscreenModeDropdown.onValueChanged.AddListener(OnFullscreenModeChanged);

        // Guardar valores originales al inicio
        originalResolutionIndex = resolutionDropdown.value;
        originalFullscreenModeIndex = fullscreenModeDropdown.value;

    }

    private void ApplyDisplaySettings()
    {
        Resolution selectedResolution = resolutions[resolutionDropdown.value];
        FullScreenMode mode = fullscreenModeDropdown.value == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        // Aplicar la resolución y el modo de pantalla
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, mode);
    }

    private void OnResolutionChanged(int newValue)
    {
        hasUnsavedChanges = true; // Marca que hay cambios sin guardar
    }

    private void OnFullscreenModeChanged(int newValue)
    {
        hasUnsavedChanges = true; // Marca que hay cambios sin guardar
    }

    // Llamar a este método cuando el jugador intente salir o cambiar de escena
    public void OnTryExit()
    {
        if (hasUnsavedChanges)
        {
            // Mostrar advertencia si hay cambios sin guardar
            ShowUnsavedChangesWarning();
        }
        else
        {
            // No hay cambios, puedes salir directamente
            Exit();
        }
    }

    // Mostrar mensaje de advertencia
    private void ShowUnsavedChangesWarning()
    {
        warningPanel.SetActive(true); // Mostrar el panel de advertencia

        // Asignar los botones de la advertencia
        var applyButton = warningPanel.transform.Find("Btn_AplicarConfirm").GetComponent<Button>();
        var cancelButton = warningPanel.transform.Find("Btn_SalirConfirm").GetComponent<Button>();

        applyButton.onClick.RemoveAllListeners();
        applyButton.onClick.AddListener(ApplyChanges);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(CancelChanges);
    }

    // Aplicar los cambios
    public void ApplyChanges()
    {
        // Cambiar la resolución solo cuando el usuario presiona aplicar
        ApplyDisplaySettings();

        // Guardar los valores actuales
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenMode", fullscreenModeDropdown.value);

        originalResolutionIndex = resolutionDropdown.value;
        originalFullscreenModeIndex = fullscreenModeDropdown.value;
        hasUnsavedChanges = false;

        Debug.Log("Cambios guardados. Nueva resolución: " + resolutions[resolutionDropdown.value].width + "x" + resolutions[resolutionDropdown.value].height);
        warningPanel.SetActive(false); // Ocultar el panel de advertencia después de aplicar cambios
    }

    // Función para salir sin guardar
    public void ExitWithoutSaving()
    {
        // Volver a los valores originales
        resolutionDropdown.value = originalResolutionIndex;
        fullscreenModeDropdown.value = originalFullscreenModeIndex;
        hasUnsavedChanges = false;

        warningPanel.SetActive(false); // Ocultar el panel después de salir
        Exit();
    }

    private void CancelChanges()
    {
        warningPanel.SetActive(false); // Ocultar el panel de advertencia si el usuario cancela
    }

    private void Exit()
    {
        Debug.Log("Saliendo del menu");
        SceneManager.LoadScene("SampleScene");
    }
}
