using UnityEngine;
using UnityEngine.UI;

public class ScareOffProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject crowprogressContainer; // the Canvas itself, or a child holding both bar sprites
    [SerializeField] private Image crowfillImage; // the bar sprite, Image Type = Filled

    private void Awake()
    {
        crowprogressContainer.SetActive(false);
    }

    public void Show()
    {
        crowprogressContainer.SetActive(true);
        SetProgress(0f);
    }

    public void Hide()
    {
        crowprogressContainer.SetActive(false);
    }

    public void SetProgress(float normalizedValue) // 0 to 1
    {
        crowfillImage.fillAmount = Mathf.Clamp01(normalizedValue);
    }
}
