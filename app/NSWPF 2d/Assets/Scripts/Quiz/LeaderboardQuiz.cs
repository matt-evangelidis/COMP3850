﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using TMPro;

public class LeaderboardQuiz : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public GameObject achievementBtn;
    public GameObject backBtn;
    public GameObject backModuleBtn;
    public Text backToSupervisorMainBtnTxt;

    private string filePath = "database/leaderboard/quiz/";

    public void backToModule()
    {
        if (Login.globalRole.Equals("Learner"))
        {
            SceneManager.LoadScene("Module Searching");
        }
        else if (Login.globalRole.Equals("Supervisor"))
        {
            SceneManager.LoadScene("Module Searching Supervisor");
        }

        // dummy field
        // SceneManager.LoadScene("Module Searching");
    }

    public void toAchievement()
    {
        SceneManager.LoadScene("Achievement Quiz");
    }

    public void backToManagement() {
        if (Login.globalRole.Equals("Supervisor"))
        {
        }
        else if (Login.globalRole.Equals("Admin")) 
        {
            SceneManager.LoadScene("Learner Management");
        }
    }
    // Start is called before the first frame update
    void Start()
    { 

        if (Login.globalRole.Equals("Learner"))
        {
            achievementBtn.SetActive(true);
            backBtn.SetActive(false);
            backModuleBtn.SetActive(true);
        }
        else if (Login.globalRole.Equals("Supervisor"))
        {
            backModuleBtn.SetActive(true);
            backToSupervisorMainBtnTxt.GetComponent<Text>().text = "Supervisor Menu"; // override button text
            achievementBtn.SetActive(false);
            backBtn.SetActive(false);
        }
        else if (Login.globalRole.Equals("Admin"))
        {
            achievementBtn.SetActive(false);
            backBtn.SetActive(true);
            backModuleBtn.SetActive(false);
        }
        Leaderboard leaderboard = new Leaderboard(filePath);
        foreach (Achievement achievement in leaderboard.achievements) 
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(content.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = achievement.username;
            go.transform.Find("Attempt").GetComponentInChildren<InputField>().text = achievement.noAttempts;
            go.transform.Find("BestAttempt").GetComponentInChildren<InputField>().text = achievement.bestAttemp;
            go.transform.Find("Percent").GetComponentInChildren<InputField>().text = achievement.percent;
        }

        Destroy(userEntry);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
