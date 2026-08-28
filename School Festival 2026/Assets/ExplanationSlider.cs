using UnityEngine;
using System.Collections;

public class ExplanationSlider : MonoBehaviour
{
    [SerializeField] private RectTransform[] pages; // one panel per explanation page, in order
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float pageSpacing = 1200f; // roughly your canvas width

    private int currentIndex = 0;
    private Coroutine slideRoutine;

    private void Awake()
    {
        // Lay pages out side by side: page 0 on-screen, others waiting off-screen to the right
        // Keep each page's existing Y position instead of forcing it to 0
        for (int i = 0; i < pages.Length; i++)
        {
            float originalY = pages[i].anchoredPosition.y;
            pages[i].anchoredPosition = new Vector2(pageSpacing * i, originalY);
        }
    }

    public void NextPage()
    {
        // Loop back to page 0 after the last page
        currentIndex = (currentIndex + 1) % pages.Length;
        SlideAllPages();
    }

    public void PreviousPage()
    {
        // Loop to the last page when going back from page 0
        currentIndex = (currentIndex - 1 + pages.Length) % pages.Length;
        SlideAllPages();
    }

    private void SlideAllPages()
    {
        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideRoutine());
    }

    private IEnumerator SlideRoutine()
    {
        Vector2[] startPositions = new Vector2[pages.Length];
        Vector2[] targetPositions = new Vector2[pages.Length];

        for (int i = 0; i < pages.Length; i++)
        {
            startPositions[i] = pages[i].anchoredPosition;
            // Each page's target X shifts based on distance from currentIndex; Y stays as-is
            targetPositions[i] = new Vector2(pageSpacing * (i - currentIndex), pages[i].anchoredPosition.y);
        }

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = t * t * (3f - 2f * t); // smoothstep easing

            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].anchoredPosition = Vector2.Lerp(startPositions[i], targetPositions[i], t);
            }
            yield return null;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].anchoredPosition = targetPositions[i];
        }
    }
}