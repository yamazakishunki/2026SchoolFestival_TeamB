using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMenuManager : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu";

    public void OnBackButton()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
