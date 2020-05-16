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

    public static List<Record> records;
    public static bool review = false;
    public static List<Question> questions;

    private int noQuestions = Quiz.totalQuestion;
    private int noCorrects = Quiz.noCorrects;

    public void toAchievement() {
        records = null;
        SceneManager.LoadScene("Achievement Quiz");
    }

    public void toFeadback() {
        review = true;
        SceneManager.LoadScene("Quiz Menu");
    }

    public void toLeaderboard() {
        records = null;
        SceneManager.LoadScene("Leaderboard Quiz");
    }
    public void reAttemp() {
        records = null;
        SceneManager.LoadScene("Quiz Menu");
    }

    public void toSearchingModule()
    {
        records = null;
        if (Login.globalRole==2)
        {
            SceneManager.LoadScene("Module Searching");
        }
        if (Login.globalRole==1)
        {
            SceneManager.LoadScene("Module Searching Supervisor");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        records = Quiz.records;
        review = false;
        questions = Quiz.questions;
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
