using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SupervisorInfo : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public GameObject inputUsername;
    public GameObject nextBtn;
    private string Username;

    private string filePath = "database/login/supervisor/";

    public void backToManagement()
    {
        SceneManager.LoadScene("Supervisor Management");
    }

    public void nextButton()
    {
        if (Username != "")
        {
            if (!System.IO.File.Exists(@filePath + Username + ".txt"))
            {
                warning.GetComponent<Text>().text = "User not found!";
                Debug.LogWarning("User not found");
                return;
            }

            AdminEdit.adminEditUsername = Username;
            AdminEdit.adminEditRole = "Supervisor";
            SceneManager.LoadScene("Admin Edit User");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cohort cohort = new Cohort(filePath);
        Debug.LogWarning(cohort.users.Count + " users");
        foreach (User supervisor in cohort.users)
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(content.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = supervisor.username;
            go.transform.Find("Firstname").GetComponent<InputField>().text = supervisor.firstname;
            go.transform.Find("Lastname").GetComponent<InputField>().text = supervisor.lastname;
            go.transform.Find("Email").GetComponent<InputField>().text = supervisor.email;
        }

        Destroy(userEntry);

        nextBtn.GetComponent<Button>().interactable = false;
        warning.GetComponent<Text>().text = "";
        inputUsername.GetComponent<InputField>().text = "";
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
