using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject confPassword;
    public GameObject warning;
    public GameObject firstName;
    public GameObject lastName;
    public GameObject role;

    private string Username;
    private string Email;
    private string Password;
    private string ConfPassword;
    private string FirstName;
    private string LastName;
    private string Role;
    private string form;

    private bool   EmailValid = false;

    // Start is called before the first frame update
    void Start()
    {
        firstName.GetComponent<InputField>().text = "";
        lastName.GetComponent<InputField>().text = "";
        username.GetComponent<InputField>().text = "";
        email.GetComponent<InputField>().text = "";
        password.GetComponent<InputField>().text = "";
        confPassword.GetComponent<InputField>().text = "";
        role.GetComponent<Dropdown>().captionText.text = "Choose your role";
    }

    public void CancelRegister() 
    {
        SceneManager.LoadScene("Login Menu");
    }

    public void RegisterButton() {

        bool FN  = false; //first name
        bool LN  = false; //last name
        bool UN  = false; //username
        bool EM  = false; //email
        bool PW  = false; //password
        bool CPW = false; //Confirm password
        bool R   = false; //role

        warning.GetComponent<Text>().text = "";

        //validate first name
        if (FirstName != "")
        {
            FN = true;
            //warning.GetComponent<Text>().text = "";
        }
        else
        {
            warning.GetComponent<Text>().text = "First Name is EMPTY!";
            Debug.LogWarning("First Name is EMPTY!");
            return;
        }

        //validate lastname
        if(LastName != "")
        {
            LN = true;
            //warning.GetComponent<Text>().text = "";
        }
        else
        {
            warning.GetComponent<Text>().text = "Last Name is EMPTY!";
            Debug.LogWarning("Last Name is EMPTY!");
            return;
        }

        //validate username

        if (Username != "")
        {
            if (!System.IO.Directory.Exists(@"database/login/learner/")) {
                System.IO.Directory.CreateDirectory(@"database/login/learner/");
            }

            if (!System.IO.File.Exists(@"database/login/learner/" + Username + ".txt"))
            {
                UN = true;
                //warning.GetComponent<Text>().text = "";
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
                        //warning.GetComponent<Text>().text = "";
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
                //warning.GetComponent<Text>().text = "";
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
                //warning.GetComponent<Text>().text = "";
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

        //validate role
        if (!Role.Equals("Choose your role"))
        {
            R = true;
        }
        else
        {
            warning.GetComponent<Text>().text = "You must choose your role!";
            Debug.LogWarning("You must choose your role!");
            return;
        }


        
        if (FN == true && LN == true && UN == true && EM == true && PW == true && CPW == true && R == true) 
        {
            //encrypt password
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
            form = (FirstName + "\n" + LastName + "\n" + Username + "\n" + Email + "\n" + Password + "\n" + Role);
            System.IO.File.WriteAllText(@"database/login/learner/" + Username+".txt",form);
            //warning.GetComponent<Text>().text = "Registration complete";
            SceneManager.LoadScene("Register Success");

            /*
            firstName.GetComponent<InputField>().text = "";
            lastName.GetComponent<InputField>().text = "";
            username.GetComponent<InputField>().text = "";
            email.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            confPassword.GetComponent<InputField>().text = "";
            role.GetComponent<Dropdown>().captionText.text = "Choose your role";
            */
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {

            if (firstName.GetComponent<InputField>().isFocused)
            {
                lastName.GetComponent<InputField>().Select();
            }
            if (lastName.GetComponent<InputField>().isFocused)
            {
                username.GetComponent<InputField>().Select();
            }
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

        FirstName = firstName.GetComponent<InputField>().text;
        LastName = lastName.GetComponent<InputField>().text;
        Username = username.GetComponent<InputField>().text;
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;
        Role = role.GetComponent<Dropdown>().captionText.text;

        if (Input.GetKeyDown(KeyCode.Return)) {
            RegisterButton();
            
        }
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
