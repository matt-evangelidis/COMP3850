using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using TMPro;

public class AchievementQuiz : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public Text backToSupervisorMainBtnTxt;

    private string[] lines;
    private string[] attemps;
    private string[] results;
    private string filePath = "database/leaderboard/quiz/";

    public void backToModule() {
        if(Login.globalRole.Equals("Learner"))
        {
            SceneManager.LoadScene("Module Searching");
        }
        else if (Login.globalRole.Equals("Supervisor"))
        {
            
            SceneManager.LoadScene("Supervisor Menu");
        }

        // dummy field
        // SceneManager.LoadScene("Module Searching");
    }

    public void toLeaderboard() {
        SceneManager.LoadScene("Leaderboard Quiz");
    }

    // Start is called before the first frame update
    void Start()
    {
        backToSupervisorMainBtnTxt.GetComponent<Text>().text = "Supervisor Menu"; // override button text

        if (!System.IO.File.Exists(@filePath + Login.globalUsername + ".txt"))
        {
            warning.GetComponent<Text>().text = "Internal error, please contact admin";
            return;
        }

        lines = System.IO.File.ReadAllLines(@filePath + Login.globalUsername + ".txt");
        if (lines[0] == "")
        {
            warning.GetComponent<Text>().text = "No Achievement";
            return;
        }
        attemps = lines[0].Split(';');

        for (int i = 0; i < attemps.Length-1; i++)
        {
            results = attemps[i].Split(',');
            string noCorrects = results[0];
            string noQuestions = results[1];
            string noAttemp = "#"+(i + 1).ToString();
            string percentage = ((float.Parse(noCorrects) / float.Parse(noQuestions)) * 100).ToString() + "%";
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(content.transform);
            go.transform.Find("Attemp").GetComponentInChildren<InputField>().text = noAttemp;
            go.transform.Find("Correct").GetComponent<InputField>().text = noCorrects;
            go.transform.Find("Total").GetComponent<InputField>().text = noQuestions;
            go.transform.Find("Percent").GetComponent<InputField>().text = percentage;
        }

        Destroy(userEntry);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
