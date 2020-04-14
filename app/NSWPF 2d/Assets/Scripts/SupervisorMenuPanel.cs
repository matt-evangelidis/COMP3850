using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SupervisorMenuPanel : MenuPanel  // dodgy inheritance
{
    public GameObject viewUsers;
    public GameObject viewLeaderboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toViewUsers()
    {
        SceneManager.LoadScene("Learner Info"); 
    }

    public void toLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard Quiz");
    }

    //public void toAccount()
    //{
    //    SceneManager.LoadScene("Account");
    //}

    public override void toSearching()
    {
        SceneManager.LoadScene("Module Searching Supervisor");
    }

}
