using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending_1 : MonoBehaviour
{
    //public Image endingImage;


    public GameObject ending1;
    public GameObject ending2;
    public GameObject ending3;

    void Start()
    {
        int score = GameData.score;
        
        ending1.SetActive(false);
        ending2.SetActive(false);
        ending3.SetActive(false);


        if (score >= 4000)
        {
            ending3.SetActive(true);
        }
        else if (score >= 3000)
        {
            ending2.SetActive(true);
        }
        else
        {
            ending1.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("ScoreInput");
        }
    }
}
