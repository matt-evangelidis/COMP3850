using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public GameObject warning;

    private string Username;
    private string Password;
    private String[] lines;
    private String DecryptedPassword;

    public void forgetPassword() {
        warning.GetComponent<Text>().text = "Please contact your admin for support!";
    }
    public void LoginButton() {

        bool UN = false; //username
        bool PW = false; //password

        //validate username
        if (Username != "")
        {
            if (System.IO.File.Exists(@"database/" + Username + ".txt"))
            {
                UN = true;
                lines = System.IO.File.ReadAllLines(@"database/" + Username + ".txt");
                warning.GetComponent<Text>().text = "";
            }
            else
            {
                warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                Debug.LogWarning("Username doesn't exist");
                return;
            }
        }
        else 
        {
            warning.GetComponent<Text>().text = "Username must not be EMPTY!";
            Debug.LogWarning("Username must not be EMPTY!");
            return;
        }

        // validate password
        if (Password != "")
        {
            if (System.IO.File.Exists(@"database/" + Username + ".txt"))
            {
                int i = 1;
                DecryptedPassword = "";
                foreach (char c in lines[2])
                {
                    char Decrypted = (char)(c / i);
                    DecryptedPassword += Decrypted.ToString();
                    i++;
                }
                if (Password.Equals(DecryptedPassword))
                {
                    PW = true;
                    warning.GetComponent<Text>().text = "";
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
                Debug.LogWarning("Username doesn't exist ");
                return;
            }
        }
        else
        {
            warning.GetComponent<Text>().text = "Password must not be EMPTY!";
            Debug.LogWarning("Password must not be EMPTY!");
            return;
        }

        if (UN == true && PW == true) 
        {
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            print("Login Successful");
            Application.LoadLevel("Main Menu");
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
            if (Username != "" && Password != "" )
            {
                LoginButton();
            }
        }
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
    }
}
