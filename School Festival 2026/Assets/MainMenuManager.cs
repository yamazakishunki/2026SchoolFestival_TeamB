using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string explanationSceneName = "Explanation";

    public void OnPlayButton()
    {
        SceneManager.LoadScene(explanationSceneName);
    }

    public void OnExitButton()
    {
        Debug.Log("Exit pressed"); // shows in Editor since Application.Quit does nothing there
        Application.Quit();
    }
}