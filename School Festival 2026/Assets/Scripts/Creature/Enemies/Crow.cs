using UnityEngine;
using System.Collections.Generic;

public class Crow : MonoBehaviour
{
    public float flyDownSpeed = 4f;
    public float flyUpSpeed = 4f;
    public float groundedDuration = 3f;
    public float arrivalThreshold = 0.05f;

    private enum CrowState { FlyingDown, Grounded, FlyingUp }
    private CrowState state = CrowState.FlyingDown;

    private RiceCrop targetCrop;
    private Vector2 targetPosition;
    private Vector2 flyAwayPosition;
    private float groundedTimer;

    private static readonly List<Crow> activeCrows = new List<Crow>();

    public void Initialize(RiceCrop crop, float spawnHeightAboveScreen)
    {
        targetCrop = crop;
        targetPosition = crop.transform.position;
        flyAwayPosition = new Vector2(targetPosition.x, targetPosition.y + spawnHeightAboveScreen);
    }

    private void Update()
    {
        switch (state)
        {
            case CrowState.FlyingDown:
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, flyDownSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, targetPosition) < arrivalThreshold)
                {
                    Land();
                }
                break;

            case CrowState.Grounded:
                groundedTimer -= Time.deltaTime;
                if (groundedTimer <= 0f)
                {
                    FlyAway();
                }
                break;

            case CrowState.FlyingUp:
                transform.position = Vector2.MoveTowards(transform.position, flyAwayPosition, flyUpSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, flyAwayPosition) < arrivalThreshold)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void Land()
    {
        state = CrowState.Grounded;
        groundedTimer = groundedDuration;

        if (targetCrop != null)
        {

            targetCrop.DestroyAndRegrow();
        }
    }

    private void FlyAway()
    {
        state = CrowState.FlyingUp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (state != CrowState.Grounded) return; // only scarable while landed

        if (other.CompareTag("Player"))
        {
            FlyAway(); // scared off early, cuts the grounded timer short
        }
    }

    private void OnEnable() // NEW
    {
        activeCrows.Add(this);
    }

    private void OnDisable() // NEW
    {
        activeCrows.Remove(this);
    }

    public static void DestroyAll() // NEW
    {
        var copy = new List<Crow>(activeCrows);
        foreach (var crow in copy)
        {
            Destroy(crow.gameObject);
        }
    }

    public static void DestroyCrowsInArea(int areaId) // NEW ? used by Scarecrow
    {
        var copy = new List<Crow>(activeCrows);
        foreach (var crow in copy)
        {
            if (crow.targetCrop != null && crow.targetCrop.AreaId == areaId)
            {
                Destroy(crow.gameObject);
            }
        }
    }
}