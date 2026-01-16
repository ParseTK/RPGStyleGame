using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapper : MonoBehaviour
{
    public void Start()
    {
	// add inits here
	SceneManager.LoadScene("MainMenu");
    }
}
