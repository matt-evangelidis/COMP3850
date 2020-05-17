using System.Collections;
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

    public static string fullName; //we will send this to the main menu welcome text
    public static string globalUsername;
    public static int globalRole; // 0 = admin, 1 = supervisor, 2 = learner
    private string Username;
    private string Password;

    public Cohort cohort;

    public void registerButton()
    {
        SceneManager.LoadScene("Register Menu");
    }

    public void forgetPassword() 
    {
        warning.GetComponent<Text>().text = "Please contact your admin for support!";
    }

    public void LoginButton()
    {

        warning.GetComponent<Text>().text = "";

        //validate username
        KeyValuePair<int, string> error = cohort.validateLogin(Username, Password);


        if (error.Key != 0) // Validate failed
        {
            warning.GetComponent<Text>().text = error.Value;
            return;
        }

        globalUsername = Username.ToLower();
        globalRole = cohort.getUser(Username).role;
        fullName = cohort.getUser(Username).firstname + " " + cohort.getUser(Username).lastname;

        if (globalRole == 2)
        {
            SceneManager.LoadScene("Main Menu");

        }
        else if (globalRole == 1)
        {

            SceneManager.LoadScene("Supervisor Menu");
        }
        else if (globalRole == 0)
        {
            SceneManager.LoadScene("Admin Menu");
        }
        username.GetComponent<InputField>().text = "";
        password.GetComponent<InputField>().text = "";

    }

    // Start is called before the first frame update
    void Start()
    {
        cohort = Cohort.getCohort();
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
