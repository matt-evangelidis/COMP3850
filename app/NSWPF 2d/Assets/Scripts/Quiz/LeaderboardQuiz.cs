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
using System.Linq;

public class LeaderboardQuiz : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public GameObject achievementBtn;
    public GameObject backBtn;
    public GameObject backModuleBtn;
    public Button Username;
    public Button TotalAttempts;
    public Button BestAttemp;
    public Button Result;
    public Text backToSupervisorMainBtnTxt;

    private string filePath = "database/leaderboard/quiz/";

    private List<Achievement> achievementList;
    private List<GameObject> entries;

    private bool sorted;
    private enum mode
    {
        usernameAsc = 0,
        usernameDes = 1,
        totalAttemptsAsc = 3,
        totalAttemptsDes = 4,
        bestAttemptAsc = 5,
        bestAttemptDes = 6,
        resultAsc = 7,
        resultDes = 8
    }

    private int sortMode;


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
        if (Login.globalRole == null) 
        {
            warning.GetComponent<Text>().text = "ERROR: cannot determine user role";
            return;
        }
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
        else 
        {
            warning.GetComponent<Text>().text = "ERROR: cannot determine user role";
            return;
        }

        Leaderboard leaderboard = new Leaderboard(filePath);

        if (leaderboard == null) {
            warning.GetComponent<Text>().text = "ERROR: cannot load leaderboard";
        }

        achievementList = new List<Achievement>(leaderboard.achievements);
        this.achievementList = leaderboard.achievements.ToList();
        entries = new List<GameObject>();

        foreach (Achievement achievement in achievementList) 
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            entries.Add(go);
            go.transform.SetParent(content.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = achievement.username;
            go.transform.Find("Attempt").GetComponentInChildren<InputField>().text = achievement.noAttempts;
            go.transform.Find("BestAttempt").GetComponentInChildren<InputField>().text = achievement.bestAttempt;
            go.transform.Find("Percent").GetComponentInChildren<InputField>().text = achievement.percent;
        }

            sorted = true;
        sortMode = (int) mode.usernameAsc;

        //Destroy(userEntry);
        userEntry.SetActive(false);

        // sorting
        Username.onClick.AddListener(sortByUsername);
        TotalAttempts.onClick.AddListener(sortByTotalAttempts);
        BestAttemp.onClick.AddListener(sortByBestAttempt);
        Result.onClick.AddListener(sortByResult);
    }

    public void sortByUsername() 
    {
        achievementList.Sort(delegate (Achievement x, Achievement y)
        {
            if (sortMode != (int)mode.usernameAsc)
            {
                return x.username.CompareTo(y.username);     
            }
            else 
            {
                return y.username.CompareTo(x.username);
            }
        });
        sorted = false;

        if (sortMode == (int)mode.usernameAsc)
        {
            sortMode = (int)mode.usernameDes;
        }
        else 
        {
            sortMode = (int)mode.usernameAsc;
        }
    }

    public void sortByTotalAttempts() 
    {
        achievementList.Sort(delegate (Achievement x, Achievement y)
        {
            if (sortMode != (int)mode.totalAttemptsAsc)
            {
                return x.totalAttempts.CompareTo(y.totalAttempts);
            }
            else
            {
                return y.totalAttempts.CompareTo(x.totalAttempts);
            }
        });
        sorted = false;
        if (sortMode == (int)mode.totalAttemptsAsc)
        {
            sortMode = (int)mode.totalAttemptsDes;
        }
        else
        {
            sortMode = (int)mode.totalAttemptsAsc;
        }
        foreach (Achievement achievement in achievementList)
        {
            Debug.Log(achievement.noAttempts);
        }
    }

    public void sortByBestAttempt()
    {
        achievementList.Sort(delegate (Achievement x, Achievement y)
        {
            if (sortMode != (int)mode.bestAttemptAsc)
            {
                return y.bestAttemptInt.CompareTo(x.bestAttemptInt);
            }
            else
            {
                return x.bestAttemptInt.CompareTo(y.bestAttemptInt);
            }
        });
        sorted = false;
        if (sortMode == (int)mode.bestAttemptAsc)
        {
            sortMode = (int)mode.bestAttemptDes;
        }
        else
        {
            sortMode = (int)mode.bestAttemptAsc;
        }
    }

    public void sortByResult()
    {
        achievementList.Sort(delegate (Achievement x, Achievement y)
        {
            if (sortMode != (int)mode.resultDes)
            {
                return y.result.CompareTo(x.result);
            }
            else
            {
                return x.result.CompareTo(y.result);
            }
        });
        sorted = false;

        if (sortMode == (int)mode.resultDes)
        {
            sortMode = (int)mode.resultAsc;
        }
        else
        {
            sortMode = (int)mode.resultDes;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sorted == true)
        {
            return;
        }

        foreach (GameObject go in entries) 
        {
            Destroy(go);
        }

        userEntry.SetActive(true);
        foreach (Achievement achievement in achievementList)
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            entries.Add(go);
            go.transform.SetParent(content.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = achievement.username;
            go.transform.Find("Attempt").GetComponentInChildren<InputField>().text = achievement.noAttempts;
            go.transform.Find("BestAttempt").GetComponentInChildren<InputField>().text = achievement.bestAttempt;
            go.transform.Find("Percent").GetComponentInChildren<InputField>().text = achievement.percent;
        }
        userEntry.SetActive(false);
        sorted = true;
    }
}
