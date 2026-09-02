using UnityEngine;
using System.Collections;

public class ScoreboardPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(1500f, 0f); // off-screen right
    [SerializeField] private Vector2 shownPosition = new Vector2(0f, 0f);     // on-screen

    private Coroutine slideRoutine;

    private void Awake()
    {
        panel.anchoredPosition = hiddenPosition; // start hidden off-screen
    }

    public void ShowScoreboard()
    {
        StartSlide(shownPosition);
    }

    public void HideScoreboard()
    {
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
            t = t * t * (3f - 2f * t); // smoothstep easing
            panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        panel.anchoredPosition = target;
    }
}