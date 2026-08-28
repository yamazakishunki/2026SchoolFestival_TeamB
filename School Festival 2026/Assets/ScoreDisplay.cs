using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] public Text scoreText;
    [SerializeField] private string prefix = "Score: ";

    private void Start()
    {
        scoreManager.OnScoreChanged.AddListener(UpdateText);
        UpdateText(scoreManager.CurrentScore); // show correct value immediately (handles 0)
    }

    private void OnDestroy()
    {
        scoreManager.OnScoreChanged.RemoveListener(UpdateText);
    }

    private void UpdateText(int newScore)
    {
        scoreText.text = prefix + newScore;
    }
}