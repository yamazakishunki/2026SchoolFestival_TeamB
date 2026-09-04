using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash Instance { get; private set; }

    [SerializeField] private Image flashImage; // full-screen UI Image, same setup pattern as FeverOverlay

    private void Awake()
    {
        Instance = this;
        SetAlpha(0f);
    }

    public void Flash(Color color, float fadeOutDuration = 0.3f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine(color, fadeOutDuration));
    }

    private IEnumerator FlashRoutine(Color color, float fadeOutDuration)
    {
        color.a = 1f;
        flashImage.color = color;

        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration));
            yield return null;
        }
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color c = flashImage.color;
        c.a = alpha;
        flashImage.color = c;
    }
}