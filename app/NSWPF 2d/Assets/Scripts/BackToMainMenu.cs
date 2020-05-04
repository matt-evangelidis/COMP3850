using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BackToMainMenu : MonoBehaviour
{
    private string _role;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMain()
    {
        _role = Login.globalRole;
        // Debug.Log(Login.globalRole);
        // Debug.Log(_role);
        if (_role == "Learner")
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (_role == "Supervisor")
        {
            SceneManager.LoadScene("Supervisor Menu");
        }
        else if (_role == "Admin")
        {
            // To be extended
            SceneManager.LoadScene("Admin Menu");
        }
        else
        {
            Debug.Log("BOOM! You didn't start the game at login scene.");
            SceneManager.LoadScene("Login Menu");
        }
    }
}
