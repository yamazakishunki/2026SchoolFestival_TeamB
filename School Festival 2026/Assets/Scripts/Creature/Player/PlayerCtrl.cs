using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCtrl : MonoBehaviour
{
    public float movespeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool movementLocked = false; // NEW
    public bool IsStunned {get; private set;}

    [SerializeField] private SpriteRenderer spriteRenderer; // NEW

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>(); // NEW
    }

    void Update()
    {
        if (movementLocked) // NEW
        {
            movement = Vector2.zero;
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y);

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // NEW: flip sprite to face the direction of horizontal movement
        if (x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (x > 0)
        {
            spriteRenderer.flipX = false;
        }
        // if x == 0 (moving only vertically or standing still), keep facing whichever way it was last
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = movement * movespeed;
        rb.linearVelocity = targetVelocity;
    }

    // NEW: called externally (by the harvest script) to freeze movement
    public void SetMovementLocked(bool locked)
    {
        movementLocked = locked;
        if (locked)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero; // stop immediately, not just ignore new input
        }
    }
    public void Stun(float duration)
    {
        if (IsStunned) return; 
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        IsStunned = true;
        SetMovementLocked(true);
        yield return new WaitForSeconds(duration);
        SetMovementLocked(false);
        IsStunned = false;
    }
    public bool IsMovementLocked()
    {
        return movementLocked;
    }

    public bool IsInvincible()
    {
        return movementLocked;
    }
}