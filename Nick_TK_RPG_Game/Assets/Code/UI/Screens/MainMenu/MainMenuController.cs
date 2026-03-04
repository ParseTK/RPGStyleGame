using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private MainMenuSettings settings;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    public void Play()
    {
        if (string.IsNullOrWhiteSpace(settings.gameplayScene))
        {
            Debug.LogError("Gameplay scene name not set on MainMenuController.");
            return;
        }

        SceneManager.LoadScene(settings.gameplayScene);
    }

    public void OpenOptions()
    {
        if (optionsPanel) optionsPanel.SetActive(true);
        if (mainPanel) mainPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        if (optionsPanel) optionsPanel.SetActive(false);
        if (mainPanel) mainPanel.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
