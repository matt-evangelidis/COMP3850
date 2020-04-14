using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseUserToEdit : MonoBehaviour
{
    public GameObject inputUsername;
    public GameObject nextBtn;
    public GameObject warning;

    private string Username;

    // Start is called before the first frame update
    void Start()
    {
        nextBtn.GetComponent<Button>().interactable = false;
        warning.GetComponent<Text>().text = "";
        inputUsername.GetComponent<InputField>().text = "";
    }

    public void nextButton() {
        if (Username != "") 
        {
            if (!System.IO.File.Exists(@"database/login/learner/" + Username + ".txt")) 
            {
                warning.GetComponent<Text>().text = "User not found!";
                Debug.LogWarning("User not found");
                return;
            }

            AdminEdit.adminEditUsername = Username;
            AdminEdit.adminEditRole = "Learner";
            SceneManager.LoadScene("Admin Edit User");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputUsername.GetComponent<InputField>().text != "")
        {
            nextBtn.GetComponent<Button>().interactable = true;
        }
        else 
        {
            nextBtn.GetComponent<Button>().interactable = false;
        }

        Username = inputUsername.GetComponent<InputField>().text;

        if (Username != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                nextButton();
            }
        }
    }
}
