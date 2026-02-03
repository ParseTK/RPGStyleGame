using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene names")]
    [SerializeField] private string gameplayScene = "URP2DSceneTemplate";

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    public void Play()
    {
        if (string.IsNullOrWhiteSpace(gameplayScene))
        {
            Debug.LogError("Gameplay scene name not set on MainMenuController.");
            return;
        }

        SceneManager.LoadScene(gameplayScene);
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
