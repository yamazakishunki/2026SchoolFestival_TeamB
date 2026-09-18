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
    [SerializeField] private Text warningtext;
    [SerializeField] private Text FinalScore;

    [Header("Tier SFX")] // NEW
    [SerializeField] private AudioClip lowTierSfx;
    [Range(0.0f, 1.0f)][SerializeField] private float volume1;
    [SerializeField] private AudioClip midTierSfx;
    [Range(0.0f, 1.0f)][SerializeField] private float volume2;
    [SerializeField] private AudioClip topTierSfx;
    [Range(0.0f, 1.0f)][SerializeField] private float volume3;

    private void Start()
    {
        FinalScore.text = "スコア : " + GameData.score;

        int score = GameData.score;

        if (score >= 4000)
        {
            resultImage.sprite = topTierSprite;
            AudioManager.Instance.PlaySFX(topTierSfx,volume1);
        }
        else if (score >= 3000)
        {
            resultImage.sprite = midTierSprite;
            AudioManager.Instance.PlaySFX(midTierSfx,volume2);
        }
        else
        {
            resultImage.sprite = lowTierSprite;
            AudioManager.Instance.PlaySFX(lowTierSfx,volume3);
        }
    }

    // Wire this to your Submit button
    public void OnSubmitButton()
    {
        string enteredName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(enteredName))
        {
            warningtext.text = "名前を入力してください！";
        }
        else
        {
            // Load existing saved scores, append the new one, save back
            List<HighScoreEntry> scores = XMLManager.instance.LoadScores();
            scores.Add(new HighScoreEntry { name = enteredName, score = GameData.score });
            XMLManager.instance.SaveScores(scores);

            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}