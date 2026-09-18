using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string explanationSceneName = "Explanation";
    [SerializeField] private string gameSceneName = "Farm";
    [SerializeField] private string scoreSceneName = "ScoreBoard";

    public void OnTutorialButton()
    {
        SceneManager.LoadScene(explanationSceneName);
    }
    public void OnPlayButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnScoreButton()
    {
        SceneManager.LoadScene(scoreSceneName);
    }
}