using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Timer gameTimer;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject gameOverPanel; // disabled by default
    [SerializeField] private Text finalScoreText;
    [SerializeField] private string endingSceneName = "Ending";

    private void Start()
    {
        gameTimer.OnTimeUp.AddListener(HandleTimeUp);
        gameOverPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        gameTimer.OnTimeUp.RemoveListener(HandleTimeUp);
    }

    private void HandleTimeUp()
    {
        finalScoreText.text = "Score: " + scoreManager.CurrentScore;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Wire this to a "Continue" button on the Time's Up panel
    public void OnContinueButton()
    {
        GameData.score = scoreManager.CurrentScore; // save before scene unloads
        Time.timeScale = 1f; // reset, since Ending scene shouldn't start frozen
        SceneManager.LoadScene(endingSceneName);
    }
}