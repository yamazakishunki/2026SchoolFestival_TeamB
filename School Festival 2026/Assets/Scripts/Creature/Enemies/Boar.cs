using UnityEngine;
using System.Collections;

public class Boar : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("消滅設定")]
    public Sprite deadSprite;
    public float deadTime = 0.3f;

    private Vector2 moveDirection;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    void Update()
    {
        if (isDead)
            return;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

        // 動きを止める
        moveDirection = Vector2.zero;

        // 消滅用画像に変更
        if (deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite;
        }

        // 少し待つ
        yield return new WaitForSeconds(deadTime);

        // イノシシを消す
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
}