using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TruckPlayButton : MonoBehaviour
{
    [SerializeField] private RectTransform truckCoord;
    [SerializeField] private Button playButton;
    [SerializeField] private Image truck;
    [SerializeField] private float driveOnDistance = 1200f; 
    [SerializeField] private float driveOnDuration = 1.2f; // NEW
    [SerializeField] private float driveOffDistance = 1200f;
    [SerializeField] private float driveOffDuration = 1f;
    [SerializeField] private string gameSceneName = "Farm";

    private bool isDriving = false;

    private void Start() // NEW
    {
        StartCoroutine(DriveOnSequence());
    }

    private IEnumerator DriveOnSequence() // NEW
    {
        playButton.interactable = false; // can't select it while it's still driving in

        Vector2 restingPos = truckCoord.anchoredPosition; // wherever you placed it in the Editor = final resting spot
        Vector2 startPos = restingPos + new Vector2(-driveOnDistance, 0f); // starts off-screen to the left

        truckCoord.anchoredPosition = startPos;

        float elapsed = 0f;
        while (elapsed < driveOnDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / driveOnDuration);
            t = t * t * (3f - 2f * t); // smoothstep easing
            truckCoord.anchoredPosition = Vector2.Lerp(startPos, restingPos, t);
            yield return null;
        }

        truckCoord.anchoredPosition = restingPos;
        playButton.interactable = true; // NEW ? now selectable
    }

    public void OnTruckSelected()
    {
        if (isDriving) return;
        isDriving = true;
        StartCoroutine(DriveOffThenLoad());
    }

    private IEnumerator DriveOffThenLoad()
    {
        Vector2 startPos = truckCoord.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(driveOffDistance, 0f);

        float elapsed = 0f;
        while (elapsed < driveOffDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / driveOffDuration);
            truckCoord.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}