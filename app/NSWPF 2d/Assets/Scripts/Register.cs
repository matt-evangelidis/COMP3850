using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject confPassword;
    public GameObject warning;

    private string   Username;
    private string   Email;
    private string   Password;
    private string   ConfPassword;
    private string   form;
    private bool     EmailValid = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RegisterButton() {

        bool UN  = false; //username
        bool EM  = false; //email
        bool PW  = false; //password
        bool CPW = false; //Confirm password

        //validate username

        if (Username != "")
        {

            if (!System.IO.File.Exists(@"database/" + Username + ".txt"))
            {
                UN = true;
                warning.GetComponent<Text>().text = "";
            }
            else
            {
                warning.GetComponent<Text>().text = "Username taken";
                Debug.LogWarning("Username taken");
                return;
            }
        } 
        else
        {
            warning.GetComponent<Text>().text = "Username field EMPTY!";
            Debug.LogWarning("Username field EMPTY!");
            return;
        }

        //validate email

        if (Email != "")
        {
            EmailValidation();
            if (EmailValid)
            {
                if (Email.Contains("@"))
                {
                    if (Email.Contains("."))
                    {
                        EM = true;
                        warning.GetComponent<Text>().text = "";
                    }
                    else
                    {
                        warning.GetComponent<Text>().text = "Unvalid Email";
                        Debug.LogWarning("Unvalid Email.");
                        return;
                    }
                }
                else 
                {
                    warning.GetComponent<Text>().text = "Unvalid Email";
                    Debug.LogWarning("Unvalid Email@");
                    return;
                }
            }
            else
            {
                warning.GetComponent<Text>().text = "Unvalid Email";
                Debug.LogWarning("Unvalid Email");
                return;
            }
        }
        else
        {
            warning.GetComponent<Text>().text = "Email field EMPTY!";
            Debug.LogWarning("Email field EMPTY!");
            return;
        }

        //validate password

        if (Password != "")
        {
            if (Password.Length > 5)
            {
                PW = true;
                warning.GetComponent<Text>().text = "";
            }
            else
            {
                warning.GetComponent<Text>().text = "Password must be at least 6 characters long";
                Debug.LogWarning("Password must be at least 6 characters long");
                return;
            }
        }
        else 
        {
            warning.GetComponent<Text>().text = "Password field EMPTY!";
            Debug.LogWarning("Password field EMPTY!");
            return;
        }

        //validate confirm password
        if (ConfPassword != "")
        {
            if (ConfPassword.Equals(Password))
            {
                CPW = true;
                warning.GetComponent<Text>().text = "";
            }
            else
            {
                warning.GetComponent<Text>().text = "Password doesn't match!";
                Debug.LogWarning("Password doesn't match!");
                return;
            }
        }
        else 
        {
            warning.GetComponent<Text>().text = "Confirm Password field EMPTY";
            Debug.LogWarning("Confirm Password field EMPTY");
            return;
        }

        //encrypt password
        if (UN == true && EM == true && PW == true && CPW == true) 
        {
            bool Clear = true;
            int i = 1;
            foreach (char c in Password) {
                if (Clear) {
                    Password = "";
                    Clear = false;
                }
                char Encrypted = (char)(c * i);
                Password += Encrypted.ToString();
                i++;
            }
            form = (Username + "\n" + Email + "\n" + Password);
            System.IO.File.WriteAllText(@"database/"+Username+".txt",form);

            username.GetComponent<InputField>().text = "";
            email.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            confPassword.GetComponent<InputField>().text = "";
            warning.GetComponent<Text>().text = "Registration complete";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {

            if (username.GetComponent<InputField>().isFocused) {
                email.GetComponent<InputField>().Select();
            }
            if (email.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                confPassword.GetComponent<InputField>().Select();
            }

        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (Username != "" && Email != "" && Password != "" && ConfPassword != "") {
                RegisterButton();
            }
        }
        Username = username.GetComponent<InputField>().text;
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;

    }

    void EmailValidation() {
        foreach (char c in Email) 
        {
            if ((int)c > 32 && (int)c < 126)
                EmailValid = true;
            else
                EmailValid = false;
        }
    }
}
