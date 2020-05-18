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
    //UI element
    public GameObject scrollView;
    public GameObject heading;

    //UI alignment
    float scrollWidth;

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
        if (Login.globalRole == 1)
        {
            backToSupervisorMainBtnTxt.GetComponent<Text>().text = "Supervisor Menu"; // override button text
            warning.GetComponent<Text>().text = "This functionality is currently unavailable for non-learner-role user";
            return;
        }

        //UI alignment

        //the width of scroll view. This is used to control the size of user entry.
        RectTransform rt = scrollView.GetComponent<RectTransform>();
        scrollWidth = rt.rect.width;

        // set heading alignment
        RectTransform headingRT = heading.GetComponent<RectTransform>();
        headingRT.sizeDelta = new Vector2(scrollWidth, headingRT.rect.height);
        headingRT.position = new Vector3(rt.position.x, headingRT.position.y, headingRT.position.z);



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
            go.transform.Find("Attempt").GetComponentInChildren<InputField>().text = "#"+(myAttempts.IndexOf(attempt)+1).ToString();
            go.transform.Find("Correct").GetComponent<InputField>().text = attempt.noCorrects.ToString();
            go.transform.Find("Total").GetComponent<InputField>().text = attempt.noQuestion.ToString();
            go.transform.Find("Percent").GetComponent<InputField>().text = attempt.percent.ToString()+"%";

            //UI alignemnt
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                RectTransform childRT = child.GetComponent<RectTransform>();

                childRT.sizeDelta = new Vector2(scrollWidth / go.transform.childCount, childRT.rect.height);
            }
            go.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        userEntry.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
