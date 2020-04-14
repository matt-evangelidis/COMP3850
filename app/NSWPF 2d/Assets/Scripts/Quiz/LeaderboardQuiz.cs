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

public class LeaderboardQuiz : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public GameObject achievementBtn;
    public GameObject backBtn;
    public GameObject backModuleBtn;


    private string filePath = "database/leaderboard/quiz/";

    public void backToModule()
    {
        SceneManager.LoadScene("Module Searching");
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
            go.transform.Find("Attemp").GetComponent<InputField>().text = achievement.noAttemps;
            go.transform.Find("BestAttemp").GetComponent<InputField>().text = achievement.bestAttemp;
            go.transform.Find("Percent").GetComponent<InputField>().text = achievement.percent;
            Debug.Log(achievement.username+ " " + achievement.noAttemps + " " + achievement.bestAttemp + " " + achievement.percent);
        }

        Destroy(userEntry);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
