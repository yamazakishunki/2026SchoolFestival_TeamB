using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ScoreboardPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(1200f, 0f);
    [SerializeField] private Vector2 shownPosition = new Vector2(0f, 0f);
    [SerializeField] private InputAction action;

    private Coroutine slideRoutine;
    private bool isShown = false; 
    private bool justOpened = false; 

    private void Awake()
    {
        panel.anchoredPosition = hiddenPosition;
    }

    private void Update() 
    {
        if (!isShown) return;

        if (justOpened)
        {
            justOpened = false; 
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return))
        {
            HideScoreboard();
        }
    }

    public void ShowScoreboard()
    {
        isShown = true;
        justOpened = true; 
        StartSlide(shownPosition);
    }

    public void HideScoreboard()
    {
        isShown = false; 
        StartSlide(hiddenPosition);
    }

    private void StartSlide(Vector2 target)
    {
        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideTo(target));
    }

    private IEnumerator SlideTo(Vector2 target)
    {
        Vector2 start = panel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = t * t * (3f - 2f * t);
            panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        panel.anchoredPosition = target;
    }
}