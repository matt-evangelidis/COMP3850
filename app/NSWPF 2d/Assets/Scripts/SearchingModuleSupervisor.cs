using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SearchingModuleSupervisor : SearchingModule
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toModifyQuiz()
    {
         SceneManager.LoadScene("Add Quiz");
    }

    //public void toDiscussion()
    //{
    //    SceneManager.LoadScene("Searching Discussion");
    //}
}
