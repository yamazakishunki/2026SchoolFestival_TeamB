using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCtrl : MonoBehaviour
{
    public float movespeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool movementLocked = false;
    public bool IsStunned { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
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

        
        if (x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (x > 0)
        {
            spriteRenderer.flipX = false;
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
    void FixedUpdate()
    {
        Vector2 targetVelocity = movement * movespeed;
        rb.linearVelocity = targetVelocity;
    }

    public void SetMovementLocked(bool locked)
    {
        movementLocked = locked;
        if (locked)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }
}