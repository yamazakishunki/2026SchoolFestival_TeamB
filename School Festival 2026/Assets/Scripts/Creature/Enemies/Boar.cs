using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GabrielBigardi.SpriteAnimator;


public class Boar : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 moveDirection;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteAnimator animator;
    private static readonly List<Boar> activeBoars = new List<Boar>();

    [SerializeField] private float boarStunDuration = 3f;
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
        {
            animator = GetComponent<SpriteAnimator>();
        }
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
        animator.PlayIfNotPlaying("Boar");
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

        if (other.TryGetComponent(out Crow crow))
        {
          crow.ScareOff();  
        }

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCtrl player = other.gameObject.GetComponent<PlayerCtrl>();

            if (player != null && player.IsInvincible)
            {
                return;
            }

            if (other.gameObject.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<PlayerCtrl>();

                if (player != null && player.IsInvincible) return; // skip entirely during i-frames

                if (player != null)
                {
                    player.Stun(boarStunDuration);
                }

                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;

        moveDirection = Vector2.zero;

        animator.Play("BoarDeath");
        animator.SetOnComplete(() => Destroy(gameObject));
    }

    private IEnumerator ReleasePlayer(PlayerCtrl player)
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