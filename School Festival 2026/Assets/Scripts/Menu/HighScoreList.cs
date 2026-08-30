using System.Collections.Generic;
using UnityEngine;

public class HighScoreList : MonoBehaviour
{
    public HighScoreDIsplay[] highScoreDisplay;
    List<HighScoreEntry> scores = new List<HighScoreEntry>();

    void Start()
    {
        scores = XMLManager.instance.LoadScores(); // load real saved scores instead of hardcoded test data
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        scores.Sort((HighScoreEntry x, HighScoreEntry y) => y.score.CompareTo(x.score));

        for (int i = 0; i < highScoreDisplay.Length; i++)
        {
            if (i < scores.Count)
            {
                highScoreDisplay[i].DisplayHighScore(scores[i].name, scores[i].score);
            }
            else
            {
                highScoreDisplay[i].HideEntryDisplay();
            }
        }
    }
}