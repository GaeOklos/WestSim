using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI thirdText;
    [SerializeField] private TextMeshProUGUI fourthText;
    [SerializeField] private GameObject complete;

    public bool firstObjective = false;
    public bool secondObjective = false;
    public bool thirdObjective = false;
    public bool fourthObjective = false;

    private int first = 0;
    private int second = 0;
    private int third = 0;
    private int fourth = 0;

    private int objectiveNb;

    void Start()
    {
        
    }

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
        }

        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            firstObjective = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            secondObjective = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            thirdObjective = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            fourthObjective = true;
        }
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
    }
}
