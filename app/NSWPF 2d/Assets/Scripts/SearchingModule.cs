﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SearchingModule : MonoBehaviour
{
    // Start is called before the first frame update
    public void toQuiz() {
        SceneManager.LoadScene("Quiz Menu");
    }

    public void toAchievement() 
    {
        SceneManager.LoadScene("Achievement Quiz");
    }
    public void toLeaderboard() {
        SceneManager.LoadScene("Leaderboard Quiz");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
