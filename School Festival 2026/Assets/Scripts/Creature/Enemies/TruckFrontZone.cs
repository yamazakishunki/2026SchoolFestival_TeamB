using UnityEngine;

public class TruckFrontZone : MonoBehaviour
{
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private int scoreDeduction = 150;
    [SerializeField] private ScoreManager scoreManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerCtrl player))
        {
            player.Stun(stunDuration);
            scoreManager.AddScore(-scoreDeduction);
        }
    }
}
