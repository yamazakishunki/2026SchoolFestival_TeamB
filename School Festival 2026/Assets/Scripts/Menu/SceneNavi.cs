using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplanationMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void OnStartButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}