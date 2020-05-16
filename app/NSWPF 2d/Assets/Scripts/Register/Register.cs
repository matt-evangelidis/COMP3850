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

    private string Username;
    private string Email;
    private string Password;
    private string ConfPassword;
    private string FirstName;
    private string LastName;

    public Cohort cohort;

    // Start is called before the first frame update
    void Start()
    {
        cohort = Cohort.getCohort();
        
        firstName.GetComponent<InputField>().text = "";
        lastName.GetComponent<InputField>().text = "";
        username.GetComponent<InputField>().text = "";
        email.GetComponent<InputField>().text = "";
        password.GetComponent<InputField>().text = "";
        confPassword.GetComponent<InputField>().text = "";
    }

    public void CancelRegister() 
    {
        SceneManager.LoadScene("Login Menu");
    }

    public void RegisterButton() {

        KeyValuePair<int, string> error = cohort.addUser(Username,FirstName,LastName,Email,Password,ConfPassword,2);

        if (error.Key == 1) //Error
        {
            warning.GetComponent<Text>().text = error.Value;
            return;
        }
        else if (error.Key == 0)
        { 
            SceneManager.LoadScene("Register Success");
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
        Username = username.GetComponent<InputField>().text.ToLower();
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;

        if (Input.GetKeyDown(KeyCode.Return)) {
            RegisterButton();  
        }
    }
}
