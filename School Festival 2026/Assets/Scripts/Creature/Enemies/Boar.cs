using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boar : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Death Settings")]
    public Sprite deadSprite;
    public float deadTime = 0.3f;

    private Vector2 moveDirection;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private static readonly List<Boar> activeBoars = new List<Boar>();

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // moving left
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // moving right
        }
    }

    void Update()
    {
        if (isDead)
            return;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) // NEW ? separate from OnCollisionEnter2D
    {
        if (isDead) return;

        if (other.TryGetComponent(out RiceCrop crop))
        {
            crop.DestroyAndRegrow();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // stays as-is, still handles the Player
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCtrl player = collision.gameObject.GetComponent<PlayerCtrl>();

            if (player != null)
            {
                player.SetMovementLocked(true);
                player.StartCoroutine(ReleasePlayer(player));
            }

            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        moveDirection = Vector2.zero;

        if (deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite;
        }

        yield return new WaitForSeconds(deadTime);

        Destroy(gameObject);
    }

    IEnumerator ReleasePlayer(PlayerCtrl player)
    {
        yield return new WaitForSeconds(3f);

        if (player != null)
        {
            player.SetMovementLocked(false);
        }
    }

    private void OnEnable() // NEW
    {
        activeBoars.Add(this);
    }

    private void OnDisable() // NEW
    {
        activeBoars.Remove(this);
    }

    public static void DestroyAll() // NEW
    {
        // Copy the list first since destroying triggers OnDisable, which would modify the list mid-loop
        var copy = new List<Boar>(activeBoars);
        foreach (var boar in copy)
        {
            Destroy(boar.gameObject);
        }
    }

}