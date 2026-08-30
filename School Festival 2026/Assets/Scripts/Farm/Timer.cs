using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events; // NEW

public class Timer : MonoBehaviour
{
    bool timerrunning = false;
    public float timeremaining = 180;
    public Text timeText;

    public UnityEvent OnTimeUp; // NEW

    void Start()
    {
        timerrunning = true;
    }

    void Update()
    {
        if (timerrunning)
        {
            if (timeremaining > 0)
            {
                timeremaining -= Time.deltaTime;
                DisplayTime(timeremaining);
            }
            else
            {
                Debug.Log("Times over");
                timerrunning = false;
                OnTimeUp?.Invoke(); // NEW — fires once, right when the timer hits 0
            }
        }
    }

    void DisplayTime(float timetodisplay)
    {
        timetodisplay += 1;
        float minutes = Mathf.FloorToInt(timetodisplay / 60);
        float seconds = Mathf.FloorToInt(timetodisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}