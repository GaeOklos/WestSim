using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI thirdText;
    [SerializeField] private TextMeshProUGUI fourthText;
    [SerializeField] private GameObject complete;
    [SerializeField] private Timer timer;

    public bool firstObjective = false;
    public bool secondObjective = false;
    public bool thirdObjective = false;
    public bool fourthObjective = false;

    public bool firstObjectiveFailed = false;
    public bool secondObjectiveFailed = false;
    public bool thirdObjectiveFailed = false;
    public bool fourthObjectiveFailed = false;

    private int first = 0;
    private int second = 0;
    private int third = 0;
    private int fourth = 0;

    private int objectiveNb;

    void Update()
    {
        TextUpdate();
        objectiveNb = first + second + third + fourth;
        if (objectiveNb == 4)
        {
            firstText.text = "";
            secondText.text = "";
            thirdText.text = "";
            fourthText.text = "";
            complete.SetActive(true);
            StartCoroutine(loadNextScene("MainMenu"));
            timer.StopTimer();
        }
    }

    
    public IEnumerator loadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("MainMenu");
    }

    private void TextUpdate()
    {
        if (firstObjective)
        {
            firstText.color = Color.green;
            first = 1;
        }
        if (secondObjective)
        { 
            secondText.color = Color.green;
            second = 1;
        }
        if (thirdObjective)
        {
            thirdText.color = Color.green;
            third = 1;
        }
        if (fourthObjective)
        {
            fourthText.color = Color.green;
            fourth = 1;
        }

        if (firstObjectiveFailed)
        {
            firstText.color= Color.red;
            first = 1;
        }
        if (secondObjectiveFailed)
        {
            secondText.color= Color.red;
            second = 1;
        }
        if (thirdObjectiveFailed)
        {
            thirdText.color= Color.red;
            third = 1;
        }
        if (fourthObjectiveFailed)
        {
            fourthText.color= Color.red;
            fourth = 1;
        }
    }
}
