using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Timer gameTimer;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private string endingSceneName = "Ending";

    [Header("Fade Transition")]
    [SerializeField] private Image fadeOverlay; // full-screen black Image, alpha starts at 0
    [SerializeField] private float holdBeforeFade = 2f; // how long the score stays fully visible first
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        gameTimer.OnTimeUp.AddListener(HandleTimeUp);
        gameOverPanel.SetActive(false);
        SetOverlayAlpha(0f);
    }

    private void OnDestroy()
    {
        gameTimer.OnTimeUp.RemoveListener(HandleTimeUp);
    }

    private void HandleTimeUp()
    {
        finalScoreText.text = "ÉXÉRÉA : " + scoreManager.CurrentScore;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        StartCoroutine(FadeAndLoadSequence());
    }

    private IEnumerator FadeAndLoadSequence()
    {
        // Wait using unscaled time since Time.timeScale is 0
        yield return new WaitForSecondsRealtime(holdBeforeFade);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            SetOverlayAlpha(Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }
        SetOverlayAlpha(1f);

        GameData.score = scoreManager.CurrentScore;
        Time.timeScale = 1f; // reset before leaving, so Ending scene doesn't start frozen
        SceneManager.LoadScene(endingSceneName);
    }

    private void SetOverlayAlpha(float alpha)
    {
        Color c = fadeOverlay.color;
        c.a = alpha;
        fadeOverlay.color = c;
    }
}