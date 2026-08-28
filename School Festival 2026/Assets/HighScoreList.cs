using System.Collections.Generic;
using UnityEngine;

public class HighScoreList : MonoBehaviour
{
    public HighScoreDIsplay[] highScoreDisplay;
    List<HighScoreEntry> scores = new List<HighScoreEntry>();
   
    void Start()
    {
        AddNewScore("Said", 5498);
        AddNewScore("Sam", 6969);
        AddNewScore("John", 4500);
        AddNewScore("Max", 5520);
        AddNewScore("Dave", 380);
        AddNewScore("Steve", 6654);
        AddNewScore("Mike", 11021);
        AddNewScore("Teddy", 3252);

        UpdateDisplay();
    }

    void AddNewScore(string entryName, int entryScore)
    {
        scores.Add(new HighScoreEntry { name = entryName, score = entryScore });
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


