using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemPickup : MonoBehaviour
{
    [Header("Common Item Settings")]
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private Sprite icon;

    [Header("Expiry Flash")] 
    [SerializeField] private float flashStartTime = 3f; // start flashing when this many seconds are left
    [SerializeField] private float flashInterval = 0.15f; // how fast it blinks

    private SpriteRenderer spriteRenderer; 

    private static readonly List<ItemPickup> activeItems = new List<ItemPickup>();

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (lifetime > 0f)
        {
            StartCoroutine(LifetimeRoutine()); // CHANGED ? was a plain Destroy() call before
        }
    }

    private IEnumerator LifetimeRoutine() 
    {
        float timeBeforeFlash = Mathf.Max(0f, lifetime - flashStartTime);
        yield return new WaitForSeconds(timeBeforeFlash);

        float remaining = Mathf.Min(flashStartTime, lifetime);
        while (remaining > 0f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // toggle visibility on/off
            yield return new WaitForSeconds(flashInterval);
            remaining -= flashInterval;
        }

        Destroy(gameObject);
    }

    private void OnEnable() 
    {
        activeItems.Add(this);
    }

    private void OnDisable() 
    {
        activeItems.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out FarmerInventory inventory))
        {
            OnPickedUp(inventory);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickedUp(FarmerInventory inventory);

    public static void DestroyAllGroundItems()
    {
        var copy = new List<ItemPickup>(activeItems);
        foreach (var item in copy)
        {
            Destroy(item.gameObject);
        }
    }
}