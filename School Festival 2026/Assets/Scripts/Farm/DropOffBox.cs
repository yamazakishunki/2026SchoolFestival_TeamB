using UnityEngine;

public class DropOffBox : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int baseScorePerRice = 100;
    [SerializeField] private int flatBonusPerExtraRice = 50;
    [SerializeField] private int feverBonusPerRice = 100; // NEW

    [SerializeField] private ScoreManager scoreManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out FarmerInventory inventory)) return;

        int riceCount = inventory.CarriedRice;
        if (riceCount <= 0) return;

        int score = CalculateScore(riceCount);
        scoreManager.AddScore(score);

        inventory.RemoveAllRice();
    }

    private int CalculateScore(int riceCount)
    {
        var state = GameStateManager.Instance != null
            ? GameStateManager.Instance.CurrentState
            : GameStateManager.GameState.Normal;

        // Base: doubled during Raining, unaffected during Fever/Normal
        int baseTotal = riceCount * baseScorePerRice;
        if (state == GameStateManager.GameState.Raining)
        {
            baseTotal *= 2;
        }

        // Multi-crop bonus: unchanged by either state
        int extraRice = Mathf.Max(0, riceCount - 1);
        int bonus = extraRice * flatBonusPerExtraRice;

        // Fever bonus: flat +100 added per rice, only during Fever
        int feverBonus = state == GameStateManager.GameState.Fever
            ? riceCount * feverBonusPerRice
            : 0;

        return baseTotal + bonus + feverBonus;
    }
}