using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;
using UnityEngine.SceneManagement;

public class learnerManagement : MonoBehaviour
{
    public void backtoAdminMenu() {
        SceneManager.LoadScene("Admin Menu");
    }

    public void toLearnerInfo() {
        SceneManager.LoadScene("Learner Info");
    }

    public void toLeaderboard() {
        SceneManager.LoadScene("Learderboard");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
