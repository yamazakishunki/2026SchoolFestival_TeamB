using UnityEngine;
using UnityEngine.UI;

public class HarvestProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject progressContainer; // the Canvas itself, or a child holding both bar sprites
    [SerializeField] private Image fillImage; // the bar sprite, Image Type = Filled

    private void Awake()
    {
        progressContainer.SetActive(false);
    }

    public void Show()
    {
        progressContainer.SetActive(true);
        SetProgress(0f);
    }

    public void Hide()
    {
        progressContainer.SetActive(false);
    }

    public void SetProgress(float normalizedValue) // 0 to 1
    {
        fillImage.fillAmount = Mathf.Clamp01(normalizedValue);
    }
}