using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterSuccess : MonoBehaviour
{
    public GameObject login;
    public GameObject register;

    public void ProceedToLogin()
    {
        SceneManager.LoadScene("Login Menu");
    }
    public void BackToRegister()
    {
        SceneManager.LoadScene("Register Menu");
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
