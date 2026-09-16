using UnityEngine;
using System.Collections;
using GabrielBigardi.SpriteAnimator;

public class DiscoBallController : MonoBehaviour
{
    [SerializeField] private SpriteAnimator animator;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float goalPosX;
    [SerializeField] private float goalPosY;
    [SerializeField] private float startPosX;
    [SerializeField] private float startPosY;
    [SerializeField] private float timeToReachGoal;
    [SerializeField] private Camera mainCam;

    private Coroutine ballMoveRoutine;

    private void OnEnable()
    {
        GameStateManager.OnFeverStart += StartDescent;
        GameStateManager.OnRainingStart += StartAscent;
    }

    private void OnDisable()
    {
        GameStateManager.OnFeverStart -= StartDescent;
        GameStateManager.OnRainingStart -= StartAscent;
    }

    private void Awake()
    {
        SetPosition(startPosX, startPosY);
    }

    private void StartDescent()
    {
        if (ballMoveRoutine != null) StopCoroutine(ballMoveRoutine);
        ballMoveRoutine = StartCoroutine(MoveBall(startPosX, startPosY, goalPosX, goalPosY));
    }

    private void StartAscent()
    {
        if (ballMoveRoutine != null) StopCoroutine(ballMoveRoutine);
        ballMoveRoutine = StartCoroutine(MoveBall(goalPosX, goalPosY, startPosX, startPosY));
    }

    private IEnumerator MoveBall(float fromX, float fromY, float toX, float toY)
    {
        yield return new WaitForSeconds(startDelay);

        Vector2 startWorldPos = ViewportToWorldPos(fromX, fromY); // CHANGED
        Vector2 goalWorldPos = ViewportToWorldPos(toX, toY);       // CHANGED


        animator.Play("Ball");

        float elapsed = 0f;
        while (elapsed < timeToReachGoal)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / timeToReachGoal);
            transform.position = Vector2.Lerp(startWorldPos, goalWorldPos, t);
            yield return null;
        }

        transform.position = goalWorldPos;
    }

    private void SetPosition(float posX, float posY)
    {
        transform.position = ViewportToWorldPos(posX, posY); // CHANGED
    }

        private Vector2 ViewportToWorldPos(float viewportX, float viewportY) // NEW
    {
        float halfHeight = mainCam.orthographicSize;
        float halfWidth = halfHeight * mainCam.aspect;

        float worldX = mainCam.transform.position.x + (viewportX - 0.5f) * 2f * halfWidth;
        float worldY = mainCam.transform.position.y + (viewportY - 0.5f) * 2f * halfHeight;

        return new Vector2(worldX, worldY);
    }
}