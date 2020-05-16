using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class AchievementQuiz : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public Text backToSupervisorMainBtnTxt;

    Leaderboard leaderboard;

    public void backToModule() {
        if(Login.globalRole==2)
        {
            SceneManager.LoadScene("Module Searching");
        }
        else if (Login.globalRole==1)
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
        if (Login.globalUsername == null) {
            warning.GetComponent<Text>().text = "Internal Error. Please contact admin!";
            return;
        }
        if (Login.globalRole==1)
        {
            backToSupervisorMainBtnTxt.GetComponent<Text>().text = "Supervisor Menu"; // override button text
        }

        leaderboard = Leaderboard.getLeaderboard();

        Achievement myAchievement = leaderboard.getAchievement(Login.globalUsername);
        if (myAchievement == null)
        {
            warning.GetComponent<Text>().text = "No personal progress yet!";
            return;
        }
        List<Attempt> myAttempts = myAchievement.attempts; 

        foreach (Attempt attempt in myAttempts)
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(content.transform);
            go.transform.Find("Attemp").GetComponentInChildren<InputField>().text = "#"+(myAttempts.IndexOf(attempt)+1).ToString();
            go.transform.Find("Correct").GetComponent<InputField>().text = attempt.noCorrects.ToString();
            go.transform.Find("Total").GetComponent<InputField>().text = attempt.noQuestion.ToString();
            go.transform.Find("Percent").GetComponent<InputField>().text = attempt.percent.ToString()+"%";
        }

        Destroy(userEntry);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
