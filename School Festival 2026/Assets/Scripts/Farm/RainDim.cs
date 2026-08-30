using UnityEngine;
using UnityEngine.UI;

public class RainDimOverlay : MonoBehaviour
{
    [SerializeField] private Image overlayImage; // full-screen UI Image, same setup as FeverOverlay
    [SerializeField] private Color dimColor = new Color(0.15f, 0.2f, 0.25f, 0.4f); // dark cool grey-blue

    private void OnEnable()
    {
        GameStateManager.OnRainingStart += ShowDim;
    }

    private void OnDisable()
    {
        GameStateManager.OnRainingStart -= ShowDim;
    }

    private void Awake()
    {
        SetAlpha(0f); // fully transparent until Raining starts
    }

    private void ShowDim()
    {
        overlayImage.color = dimColor;
    }

    private void SetAlpha(float alpha)
    {
        Color c = overlayImage.color;
        c.a = alpha;
        overlayImage.color = c;
    }
}