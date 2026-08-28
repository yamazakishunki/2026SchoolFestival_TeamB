using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private float slow = 0.5f;
    private bool trigger = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (trigger)
        {
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger=true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Chara")
        {
        }
    }

   private void slowdown(GameObject player)
    {
       PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
        if (playerCtrl != null)
        {
            playerCtrl.movespeed--;
        }
        else
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                
            }

        }
    }
}
