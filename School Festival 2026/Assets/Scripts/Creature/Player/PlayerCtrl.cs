using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GabrielBigardi.SpriteAnimator;

public class PlayerCtrl : MonoBehaviour
{
    public float movespeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool movementLocked = false;
    public bool IsInvincible { get; private set; }
    public bool IsStunned {get; private set;}
    [SerializeField] private float iFrameDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;
    private float speedMultiplier = 1f;

    [SerializeField] private SpriteRenderer spriteRenderer; 
    [SerializeField] public SpriteAnimator spriteAnimator;

    private float baseSpeed; 
    private Coroutine speedBoostRoutine; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteAnimator == null) spriteAnimator = GetComponent<SpriteAnimator>();
        baseSpeed = movespeed;
    }

    void Update()
    {
        if (movementLocked)
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

        if (movement.sqrMagnitude > 0.01f)
        {
            spriteAnimator.PlayIfNotPlaying("Walking");
        }
        else
        {
            spriteAnimator.PlayIfNotPlaying("Idle");
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
        Debug.Log("Stun called. IsStunned=" + IsStunned + " IsInvincible=" + IsInvincible);
        if (IsStunned || IsInvincible)
        {
            return;
        }
        StartCoroutine(StunRoutine(duration));
    }

    public void SetSpeedMultiplier(float multiplier) // NEW — replaces ApplySpeedBoost
    {
    speedMultiplier = multiplier;
    movespeed = baseSpeed * multiplier;
        spriteAnimator.SetCurrentFrame(10);
    }

    public void ApplySpeedBoost(float multiplier, float duration) // NEW
    {
        if (speedBoostRoutine != null) StopCoroutine(speedBoostRoutine);
        speedBoostRoutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        movespeed = baseSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        movespeed = baseSpeed;
        spriteAnimator.SetCurrentFrame(10);
        speedBoostRoutine = null;
    }

    private IEnumerator StunRoutine(float duration)
    {
        IsStunned = true;
        SetMovementLocked(true);
        spriteAnimator.Play("Stun"); 
        yield return new WaitForSeconds(duration);
        SetMovementLocked(false);
        IsStunned = false;

        IsInvincible = true;
        yield return StartCoroutine(BlinkDuringInvincibility());
        IsInvincible = false;
        spriteRenderer.enabled = true;
    }
    public bool IsMovementLocked()
    {
        return movementLocked;
    }

    private IEnumerator BlinkDuringInvincibility() // NEW
    {
        float elapsed = 0f;
        while (elapsed < iFrameDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
    }

}