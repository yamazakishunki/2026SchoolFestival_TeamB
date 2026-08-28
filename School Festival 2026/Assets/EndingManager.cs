using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite lowTierSprite;   // below 3000
    [SerializeField] private Sprite midTierSprite;   // 3000 - 3999
    [SerializeField] private Sprite topTierSprite;   // 4000+

    [Header("Name Submission")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        int score = GameData.score;

        if (score >= 4000)
        {
            resultImage.sprite = topTierSprite;
        }
        else if (score >= 3000)
        {
            resultImage.sprite = midTierSprite;
        }
        else
        {
            resultImage.sprite = lowTierSprite;
        }
    }

    // Wire this to your Submit button
    public void OnSubmitButton()
    {
        string enteredName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(enteredName))
        {
            enteredName = "Unknown Farmer";
        }

        // Load existing saved scores, append the new one, save back
        List<HighScoreEntry> scores = XMLManager.instance.LoadScores();
        scores.Add(new HighScoreEntry { name = enteredName, score = GameData.score });
        XMLManager.instance.SaveScores(scores);

        SceneManager.LoadScene(mainMenuSceneName);
    }
}