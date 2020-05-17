using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AdminEdit : MonoBehaviour
{
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject confPassword;
    public GameObject warning;
    public GameObject firstName;
    public GameObject lastName;
    public GameObject role;

    public GameObject editBtn;
    public GameObject saveBtn;
    public GameObject cancelBtn;
    public GameObject backBtn;

    private bool edit = false;

    public static string adminEditUsername = "";
    public static string adminEditRole = "";
    private string Username;
    private string Email;
    private string Password;
    private string ConfPassword;
    private string CurPassword;
    private string FirstName;
    private string LastName;
    private string Role;
    private string form;

    //singleton object
    Cohort cohort;

    // Start is called before the first frame update
    void Start()
    {
        cohort = Cohort.getCohort();

        if (adminEditUsername.Equals(""))
        {
            warning.GetComponent<Text>().text = "Internal error, please contact develper";
            return;
        } else if (adminEditRole.Equals("")) 
        {
            warning.GetComponent<Text>().text = "Internal error, please contact develper";
            return;
        }

        User editUser = cohort.getUser(adminEditUsername);
        
        if (editUser == null) {
            warning.GetComponent<Text>().text = "Unexpected error, please contact development team";
            Debug.LogWarning("cannot find user object");
        }

        username.GetComponentInChildren<InputField>().text = adminEditUsername;
        firstName.GetComponentInChildren<InputField>().text = editUser.firstname;
        lastName.GetComponentInChildren<InputField>().text = editUser.lastname;
        email.GetComponentInChildren<InputField>().text = editUser.email;
        role.GetComponentInChildren<InputField>().text = adminEditRole;
        Role = adminEditRole;
    }

    public void backToUserInfo()
    {
        int editRole = cohort.getRoleToIndex(adminEditRole);
        if (editRole==2)
        {
            SceneManager.LoadScene("Learner Info");
        }
        else if (editRole==1) 
        {
            SceneManager.LoadScene("Supervisor Info");
        }
    }

    public void editModeOn()
    {
        edit = true;
    }
    public void cancelEdit()
    {
        edit = false;
    }

    public void saveChange() {
        Username = username.GetComponentInChildren<InputField>().text.ToLower();
        FirstName = firstName.GetComponentInChildren<InputField>().text;
        LastName = lastName.GetComponentInChildren<InputField>().text;
        Email = email.GetComponentInChildren<InputField>().text;      
        Password = password.GetComponentInChildren<InputField>().text;
        ConfPassword = confPassword.GetComponentInChildren<InputField>().text;

        User editUser = cohort.getUser(adminEditUsername);

        KeyValuePair<int,string> error =  cohort.editUser(Username,FirstName,LastName,Email,editUser.decryptPassword(),Password,ConfPassword, cohort.getRoleToIndex(Role));

        if (error.Key == 1)
        {
            warning.GetComponent<Text>().text = error.Value;
            return;
        }
        edit = false;
        warning.GetComponent<Text>().text = error.Value;

        password.GetComponentInChildren<InputField>().text = "";
        confPassword.GetComponentInChildren<InputField>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        saveBtn.SetActive(edit);
        cancelBtn.SetActive(edit);
        editBtn.SetActive(!edit);
        backBtn.SetActive(!edit);

        //username.GetComponentInChildren<InputField>().interactable = edit;
        role.GetComponentInChildren<InputField>().interactable = false;
        firstName.GetComponentInChildren<InputField>().interactable = edit;
        lastName.GetComponentInChildren<InputField>().interactable = edit;
        email.GetComponentInChildren<InputField>().interactable = edit;
        password.SetActive(edit);
        confPassword.SetActive(edit);
        password.GetComponentInChildren<InputField>().interactable = edit;
        confPassword.GetComponentInChildren<InputField>().interactable = edit;

        if (edit == false)
        {
            Start();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.GetComponentInChildren<InputField>().isFocused)
            { 
                firstName.GetComponentInChildren<InputField>().Select();
            }
            if (firstName.GetComponentInChildren<InputField>().isFocused)
            {
                lastName.GetComponentInChildren<InputField>().Select();
            }
            if (lastName.GetComponentInChildren<InputField>().isFocused)
            {
                email.GetComponentInChildren<InputField>().Select();
            }
            if (email.GetComponentInChildren<InputField>().isFocused)
            {
                password.GetComponentInChildren<InputField>().Select();
            }
            if (password.GetComponentInChildren<InputField>().isFocused)
            {
                confPassword.GetComponentInChildren<InputField>().Select();
            }
        }

        if (edit == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                saveChange();
            }
        }
    }
}

