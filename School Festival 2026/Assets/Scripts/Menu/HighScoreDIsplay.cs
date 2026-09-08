using UnityEngine;
using UnityEngine.UI;

public class HighScoreDIsplay : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text scoreText;

    [Header("Rank Tier")]
    [SerializeField] private Image tierImage;
    [SerializeField] private Sprite normieTierSprite;
    [SerializeField] private Sprite lowTierSprite;  
    [SerializeField] private Sprite midTierSprite;  
    [SerializeField] private Sprite topTierSprite;  

    public void DisplayHighScore(string name, int score)
    {
        nameText.text = name;
        scoreText.text = string.Format("{0:000000}", score);

        UpdateTierIcon(score);
    }

    public void HideEntryDisplay()
    {
        nameText.text = "";
        scoreText.text = "";

        if (tierImage != null)
        {
            tierImage.enabled = false; 
        }
    }

    private void UpdateTierIcon(int score) 
    {
        if (tierImage == null) return;

        tierImage.enabled = true;

        if (score >= 4000)
        {
            tierImage.sprite = topTierSprite;
            scoreText.color = new Color32(255,207,108,255);
            nameText.color = new Color32(255, 207, 108, 255);
        }
        else if (score >= 3000)
        {
            tierImage.sprite = midTierSprite;
            scoreText.color = new Color32(217, 217, 217, 255);
            nameText.color = new Color32(217, 217, 217, 255);
        }
        else if (score >= 2000)
        {
            tierImage.sprite = lowTierSprite;
            scoreText.color = new Color32(153, 106, 0, 255);
            nameText.color = new Color32(153, 106, 0, 255);
        }
        else
        {
            tierImage.sprite = normieTierSprite;
            scoreText.color = new Color32(255, 255, 255, 255);
            nameText.color = new Color32(255, 255, 255, 255);
        }
    }
}