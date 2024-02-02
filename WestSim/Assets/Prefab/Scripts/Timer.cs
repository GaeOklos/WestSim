using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timerDuration = 0f;

    public float timer;

    public bool backward = false;

    private bool finished = false;
    private bool start = false;

    public TextMeshProUGUI endTimer;
    public TextMeshProUGUI endTimerMs;
    public GameObject gameOver;

    [SerializeField]
    private TextMeshProUGUI textfield;
    [SerializeField]
    private TextMeshProUGUI textMS;

    void Start()
    {
        Application.targetFrameRate = 60;

        ResetTimer();
    }

    void Update()
    {
        if (!finished && start)
        {
            if (backward is false) 
            {
                timer += Time.deltaTime;
            } else if (backward)
            {
                timer -= Time.deltaTime;
            }
        }

        if (backward && timer <= 0f)
        {
            gameOver.SetActive(true);
        }

        UpdateTimerDisplay(timer);
    }

    private void ResetTimer()
    {
        if (backward)
        {
            timer = timerDuration;
        }
        if (backward is false) 
        {
            timer = 0f;
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float ms = Mathf.FloorToInt(((time % 60) - seconds) * 100);


        string currentTime = string.Format("{0:00}: {1:00},", minutes, seconds);
        string currentMS = string.Format("{0:00}", ms);

        textfield.text = currentTime;
        textMS.text = currentMS;

        if (finished is true && backward is false)
        {
            endTimer.text = currentTime;
            endTimerMs.text = currentMS;
            textfield.text = "";
            textMS.text = "";
        }
        if (backward && timer <= 0f)
        {
            gameOver.SetActive(true);
            textfield.text = "Finish";
            textMS.text = "";
        }
    }

    public void StartTimer()
    {
        start = true;
    }

    public void StopTimer()
    {
        finished = true;
    }
}
