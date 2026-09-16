using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeverOverlay : MonoBehaviour
{
    [SerializeField] private Image overlayImage;

    [Header("Color Cycling")]
    [SerializeField] private float hueCycleSpeed = 0.15f;
    [SerializeField] private float saturation = 0.8f;
    [SerializeField] private float brightness = 1f;

    [Header("Alpha Pulse")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.35f;

    private Coroutine cycleRoutine;

    private void OnEnable()
    {
        GameStateManager.OnFeverStart += ShowOverlay;
        GameStateManager.OnRainingStart += HideOverlay;
    }

    private void OnDisable()
    {
        GameStateManager.OnFeverStart -= ShowOverlay;
        GameStateManager.OnRainingStart -= HideOverlay;
    }

    private void Awake()
    {
        SetAlpha(0f);
    }

    private void ShowOverlay()
    {
        if (cycleRoutine != null) StopCoroutine(cycleRoutine);
        cycleRoutine = StartCoroutine(CycleRoutine());
    }

    private void HideOverlay()
    {
        if (cycleRoutine != null) StopCoroutine(cycleRoutine);
        SetAlpha(0f);
    }

    private IEnumerator CycleRoutine()
    {
        while (true)
        {
            float hue = Mathf.Repeat(Time.time * hueCycleSpeed, 1f);
            Color rainbowColor = Color.HSVToRGB(hue, saturation, brightness);

            float pulseT = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            rainbowColor.a = Mathf.Lerp(minAlpha, maxAlpha, pulseT);

            overlayImage.color = rainbowColor;
            yield return null;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color c = overlayImage.color;
        c.a = alpha;
        overlayImage.color = c;
    }
}