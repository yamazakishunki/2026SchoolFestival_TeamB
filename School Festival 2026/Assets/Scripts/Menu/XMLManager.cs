using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    public Leaderboard leaderboard;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // prevent duplicates if it somehow exists in another scene too
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // NEW — carries this object forward into every scene

        if(!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
        }
    }

    public void SaveScores(List<HighScoreEntry> scoresToSave)
    {
        leaderboard.list = scoresToSave;
        XmlSerializer serializer= new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }

    public List<HighScoreEntry> LoadScores()
{
    if(File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
    {
        XmlSerializer serializer= new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
        leaderboard = serializer.Deserialize(stream) as Leaderboard;
        stream.Close();
    }
    else
    {
        leaderboard = new Leaderboard(); // first-ever run, no file yet — start with an empty list
    }
    return leaderboard.list;
}

    [System.Serializable]
    public class Leaderboard
    {
        public List<HighScoreEntry> list = new List<HighScoreEntry>();
    }
}