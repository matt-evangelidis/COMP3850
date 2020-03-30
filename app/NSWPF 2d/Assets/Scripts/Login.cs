﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public GameObject warning;
    public GameObject register;

    private string Username;
    private string Password;
    private String[] lines;
    private String DecryptedPassword;

    public void registerButton()
    {
        //Application.LoadLevel("Register Menu");
        SceneManager.LoadScene("Register Menu");
    }

    public void forgetPassword() 
    {
        warning.GetComponent<Text>().text = "Please contact your admin for support!";
    }
    public void LoginButton() {

        bool UN = false; //username
        bool PW = false; //password
        string Role = "";

        warning.GetComponent<Text>().text = "";

        //validate username
        if (Username != "")
        {
            // validate password
            if (Password != "")
            {
                if (System.IO.File.Exists(@"database/login/" + Username + ".txt"))
                {
                    UN = true;
                    lines = System.IO.File.ReadAllLines(@"database/login/" + Username + ".txt");
                    //warning.GetComponent<Text>().text = "";

                    //decrypt password in the database and compare
                    int i = 1;
                    DecryptedPassword = "";
                    foreach (char c in lines[4])
                    {
                        char Decrypted = (char)(c / i);
                        DecryptedPassword += Decrypted.ToString();
                        i++;
                    }
                    if (Password.Equals(DecryptedPassword))
                    {
                        PW = true;
                        Role = lines[5];
                        //warning.GetComponent<Text>().text = "";
                    }
                    else
                    {
                        warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                        Debug.LogWarning("password is wrong!");
                        return;
                    }
                }
                else
                {
                    warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                    Debug.LogWarning("Username doesn't exist");
                    return;
                }
            }
            else //pass word is empty
            {
                warning.GetComponent<Text>().text = "Password must not be EMPTY!";
                Debug.LogWarning("Password must not be EMPTY!");
                return;
            }
        }
        else 
        {
            warning.GetComponent<Text>().text = "Username must not be EMPTY!";
            Debug.LogWarning("Username must not be EMPTY!");
            return;
        }

        if (UN == true && PW == true) 
        {
            if (Role.Equals("Learner"))
            {
                SceneManager.LoadScene("Main Menu");

            } else if (Role.Equals("Supervisor")) {

                SceneManager.LoadScene("Supervisor Menu");
            }
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginButton();
        }
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
    }
}
