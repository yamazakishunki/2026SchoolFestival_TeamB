using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCtrl : MonoBehaviour
{
    
    private void TryHarvestNearbyCrop(RiceCrop crop, FarmerInventory inventory)
{
    if (crop.State != RiceCrop.CropState.Ready) return;
    if (inventory.IsFull) return; // optionally show a "hands full" UI cue

    if (crop.TryHarvest())
    {
        inventory.TryAddRice();
    }
}

    public float movespeed;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y);

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = movement * movespeed;
        rb.linearVelocity = targetVelocity;
    }
}
