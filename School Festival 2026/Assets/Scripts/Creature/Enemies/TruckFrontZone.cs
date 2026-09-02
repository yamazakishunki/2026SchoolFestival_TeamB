using UnityEngine;

public class TruckFrontZone : MonoBehaviour
{
    [SerializeField] private float stunDuration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerCtrl player))
        {
            player.Stun(stunDuration);
        }
    }
}
