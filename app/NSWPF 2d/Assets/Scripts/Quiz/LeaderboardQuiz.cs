using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LeaderboardQuiz : MonoBehaviour
{
    //UI element
    public GameObject scrollView;
    public GameObject heading;

    //UI alignment
    float scrollWidth;

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
        if (Login.globalRole==2)
        {
            SceneManager.LoadScene("Module Searching");
        }
        else if (Login.globalRole==1)
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
        if (Login.globalRole==1)
        {
        }
        else if (Login.globalRole==0) 
        {
            SceneManager.LoadScene("Learner Management");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Login.globalRole==2)
        {
            achievementBtn.SetActive(true);
            backBtn.SetActive(false);
            backModuleBtn.SetActive(true);
        }
        else if (Login.globalRole==1)
        {
            backModuleBtn.SetActive(true);
            backToSupervisorMainBtnTxt.GetComponent<Text>().text = "Supervisor Menu"; // override button text
            achievementBtn.SetActive(false);
            backBtn.SetActive(false);
        }
        else if (Login.globalRole==0)
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

        //UI alignment

        //the width of scroll view. This is used to control the size of user entry.
        RectTransform rt = scrollView.GetComponent<RectTransform>();
        scrollWidth = rt.rect.width;

        // set heading alignment
        RectTransform headingRT = heading.GetComponent<RectTransform>();
        headingRT.sizeDelta = new Vector2(scrollWidth, headingRT.rect.height);
        headingRT.position = new Vector3(rt.position.x, headingRT.position.y, headingRT.position.z);

        Leaderboard leaderboard = Leaderboard.getLeaderboard();

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
            go.transform.Find("Attempt").GetComponentInChildren<InputField>().text = achievement.noAttempts.ToString();
            go.transform.Find("BestAttempt").GetComponentInChildren<InputField>().text = "#"+achievement.bestAttempt.ToString();
            go.transform.Find("Percent").GetComponentInChildren<InputField>().text = (achievement.bestScore).ToString()+"%";
            

            //UI alignemnt
            for (int i = 0; i < go.transform.childCount; i++) 
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                RectTransform childRT = child.GetComponent<RectTransform>();

                childRT.sizeDelta = new Vector2(scrollWidth / go.transform.childCount, childRT.rect.height);
            }
            go.transform.localScale = new Vector3(1f, 1f, 1f);
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
                return x.noAttempts.CompareTo(y.noAttempts);
            }
            else
            {
                return y.noAttempts.CompareTo(x.noAttempts);
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
                return y.bestAttempt.CompareTo(x.bestAttempt);
            }
            else
            {
                return x.bestAttempt.CompareTo(y.bestAttempt);
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
                return y.bestScore.CompareTo(x.bestScore);
            }
            else
            {
                return x.bestScore.CompareTo(y.bestScore);
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
            go.transform.Find("Attempt").GetComponentInChildren<InputField>().text = achievement.noAttempts.ToString();
            go.transform.Find("BestAttempt").GetComponentInChildren<InputField>().text = "#"+achievement.bestAttempt.ToString();
            go.transform.Find("Percent").GetComponentInChildren<InputField>().text = (achievement.bestScore).ToString()+"%";

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
        sorted = true;
    }
}
