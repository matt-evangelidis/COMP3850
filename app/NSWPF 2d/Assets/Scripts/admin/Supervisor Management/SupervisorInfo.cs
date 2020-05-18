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
    //UI element
    public GameObject scrollView;
    public GameObject heading;

    //UI alignment
    float scrollWidth;

    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;
    public GameObject inputUsername;
    public GameObject nextBtn;
    private string Username;

    //singleton 
    Cohort cohort = Cohort.getCohort();

    public void backToManagement()
    {
        SceneManager.LoadScene("Supervisor Management");
    }

    public void nextButton()
    {
        if (Username != "")
        {
            User foundUser = cohort.getUser(Username);

            if (foundUser == null) 
            {
                warning.GetComponent<Text>().text = "User not found!";
                Debug.LogWarning("User not found");
                return;
            }

            AdminEdit.adminEditUsername = Username;
            AdminEdit.adminEditRole = cohort.getIndexToRole(foundUser.role);
            SceneManager.LoadScene("Admin Edit User");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //UI alignment

        //the width of scroll view. This is used to control the size of user entry.
        RectTransform rt = scrollView.GetComponent<RectTransform>();
        scrollWidth = rt.rect.width;

        // set heading alignment
        RectTransform headingRT = heading.GetComponent<RectTransform>();
        headingRT.sizeDelta = new Vector2(scrollWidth, headingRT.rect.height);
        headingRT.position = new Vector3(rt.position.x, headingRT.position.y, headingRT.position.z);
        for (int i = 0; i < heading.transform.childCount; i++)
        {
            GameObject child = heading.transform.GetChild(i).gameObject;
            RectTransform childRT = child.GetComponent<RectTransform>();

            childRT.sizeDelta = new Vector2(scrollWidth / heading.transform.childCount, childRT.rect.height);
        }

        Cohort cohort = Cohort.getCohort();
        foreach (User supervisor in cohort.getSupervisors())
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(content.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = supervisor.username;
            go.transform.Find("Firstname").GetComponent<InputField>().text = supervisor.firstname;
            go.transform.Find("Lastname").GetComponent<InputField>().text = supervisor.lastname;
            go.transform.Find("Email").GetComponent<InputField>().text = supervisor.email;

            //UI alignemnt
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                RectTransform childRT = child.GetComponent<RectTransform>();

                childRT.sizeDelta = new Vector2(scrollWidth / go.transform.childCount, childRT.rect.height);
            }
            go.transform.localScale = new Vector3(1f, 1f, 1f);
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
