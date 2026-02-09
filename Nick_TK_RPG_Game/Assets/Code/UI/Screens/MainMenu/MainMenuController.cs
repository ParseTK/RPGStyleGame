using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI.MainMenu 
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MainMenuSettings settings;   
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private void OnEnable()
        {
            if (startButton) startButton.onClick.AddListener(StartGame);
            if (quitButton)  quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            if (startButton) startButton.onClick.RemoveAllListeners();
            if (quitButton)  quitButton.onClick.RemoveAllListeners();
        }

        private void StartGame()
        {
            SceneManager.LoadScene(settings.nextSceneName);     
        }

        private void QuitGame()
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
    #else
            Application.Quit();
    #endif
        }
    }
}