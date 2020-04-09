using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterSuccess : MonoBehaviour
{
    public GameObject toLogin;
    public GameObject toRegister;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ProceedToLogin()
    {
        SceneManager.LoadScene("Login Menu");
    }
    public void BackToRegister()
    {
        SceneManager.LoadScene("Register Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
