using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntroPanel
{
    public CanvasGroup panelGroup;
    public float displayDuration = 3f;
}

public class IntroManager : MonoBehaviour
{
    [SerializeField] private IntroPanel[] panels;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private KeyCode skipKey = KeyCode.Escape;

    [Header("Same-Scene Transition")]
    [SerializeField] private GameObject introContainer;  // parent holding all intro panels
    [SerializeField] private GameObject mainMenuContainer; // parent holding your main menu buttons

    private bool skipped = false;

    private void Start()
    {
        mainMenuContainer.SetActive(false); // hidden until intro finishes

        foreach (var panel in panels)
        {
            panel.panelGroup.alpha = 0f;
        }

        StartCoroutine(PlaySequence());
    }

    private void Update()
    {
        if (!skipped && (Input.GetKeyDown(skipKey) || Input.GetButtonDown("Submit")))
        {
            SkipIntro();
        }
    }

    private IEnumerator PlaySequence()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (skipped) yield break;

            yield return StartCoroutine(FadeCanvasGroup(panels[i].panelGroup, 0f, 1f, fadeDuration));

            float elapsed = 0f;
            while (elapsed < panels[i].displayDuration && !skipped)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (skipped) yield break;

            yield return StartCoroutine(FadeCanvasGroup(panels[i].panelGroup, 1f, 0f, fadeDuration));
        }

        ShowMainMenu();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        group.alpha = to;
    }

    private void SkipIntro()
    {
        skipped = true;
        StopAllCoroutines();
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        introContainer.SetActive(false);
        mainMenuContainer.SetActive(true);
    }
}