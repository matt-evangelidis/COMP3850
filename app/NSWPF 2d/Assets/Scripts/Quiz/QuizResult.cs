using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuizResult : MonoBehaviour
{
    public GameObject percent;
    public GameObject correct;
    public GameObject wrong;
    public GameObject total;

    private int noQuestions = Quiz.totalQuestion;
    private int noCorrects = Quiz.noCorrects;

    public void toAchievement() {
        SceneManager.LoadScene("Achievement Quiz");
    }

    public void toLeaderboard() {
        SceneManager.LoadScene("Leaderboard Quiz");
    }
    public void reAttemp() {
        SceneManager.LoadScene("Quiz Menu");
    }

    public void toSearchingModule()
    {
        SceneManager.LoadScene("Module Searching");
    }


    // Start is called before the first frame update
    void Start()
    {
        int noIncorrects = noQuestions - noCorrects;
        float percentage = ((float)noCorrects / (float)noQuestions)*100;
        total.GetComponent<Text>().text = noQuestions.ToString();
        correct.GetComponent<Text>().text = noCorrects.ToString();
        wrong.GetComponent<Text>().text = noIncorrects.ToString();
        percent.GetComponent<Text>().text = percentage.ToString()+"%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
