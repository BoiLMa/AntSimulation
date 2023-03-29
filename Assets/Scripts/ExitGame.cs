using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public TMP_Text UI;
    public GameObject antHill;
    public string food, antsAmount;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        UI.text = "Escape to Quit\n" + "Amount of food: " + food + "\nAmount of ants: " + antsAmount;
        food = antHill.GetComponent<AntBehaviour>().totalFood.ToString();
        antsAmount = antHill.GetComponent<AntBehaviour>().antList.Count.ToString();
    }
}
