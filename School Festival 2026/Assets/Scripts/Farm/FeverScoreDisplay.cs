using UnityEngine;
using UnityEngine.UI;

public class FeverScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Text feverText;
    [SerializeField] private string suffix = " more for Fever!";

    private void Start()
    {
        scoreManager.OnScoreChanged.AddListener(UpdateDisplay);
        UpdateDisplay(scoreManager.CurrentScore); // show correct value immediately

        // Hide once the Normal state ends — the threshold no longer matters after that point
        GameStateManager.OnFeverStart += HideDisplay;
        GameStateManager.OnRainingStart += HideDisplay;
    }

    private void OnDestroy()
    {
        scoreManager.OnScoreChanged.RemoveListener(UpdateDisplay);
        GameStateManager.OnFeverStart -= HideDisplay;
        GameStateManager.OnRainingStart -= HideDisplay;
    }

    private void UpdateDisplay(int currentScore)
{
    int threshold = GameStateManager.Instance.feverScoreThreshold;
    int remaining = Mathf.Max(0, threshold - currentScore);

    if (remaining <= 0)
    {
        feverText.text = "Score reached!";
    }
    else
    {
        feverText.text = remaining + suffix;
    }
}

    private void HideDisplay()
    {
        feverText.gameObject.SetActive(false);
    }
}