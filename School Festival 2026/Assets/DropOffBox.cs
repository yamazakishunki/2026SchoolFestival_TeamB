using UnityEngine;

public class DropOffBox : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int baseScorePerRice = 100;
    [SerializeField] private int flatBonusPerExtraRice = 50;

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
        int baseTotal = riceCount * baseScorePerRice;
        int extraRice = Mathf.Max(0, riceCount - 1);
        int bonus = extraRice * flatBonusPerExtraRice;
        return baseTotal + bonus;
    }
}