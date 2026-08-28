using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    public UnityEvent<int> OnScoreChanged;

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }
}