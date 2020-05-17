using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AcountSetting : MonoBehaviour
{
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject confPassword;
    public GameObject passwordtxt;
    public GameObject confPasswordtxt;
    public GameObject warning;
    public GameObject firstName;
    public GameObject lastName;
    public GameObject role;
    public GameObject curPassword;
    public GameObject curPasswordtxt;

    public GameObject editBtn;
    public GameObject saveBtn;
    public GameObject cancelBtn;
    public GameObject backBtn;

    private static string Username;
    private string Email;
    private string Password;
    private string ConfPassword;
    private string CurPassword;
    private string FirstName;
    private string LastName;
    private string Role;

    private bool edit = false;

    //singleton:
    Cohort cohort;

    private string pathLearner = "database/login/learner/";
    private string pathSupervisor = "database/login/supervisor/";
    private string path = "";

    // Start is called before the first frame update
    void Start()
    {
        cohort = Cohort.getCohort();
        if (Login.globalRole==1)
        {
            path = pathSupervisor;
        }
        else if (Login.globalRole==2)
        {
            path = pathLearner;
        }
        Username = Login.globalUsername;

        User user = cohort.getUser(Username);
        if (user == null) 
        {
            warning.GetComponent<Text>().text = "There is an internal error, please contact admin";
            Debug.LogWarning("file not exist" + Username);
            return;

        }

        username.GetComponent<TextMeshProUGUI>().text = user.username;
        firstName.GetComponent<InputField>().text = user.firstname;
        lastName.GetComponent<InputField>().text = user.lastname;
        email.GetComponent<InputField>().text = user.email;
        role.GetComponent<TextMeshProUGUI>().text = cohort.getIndexToRole(user.role);
        Role = cohort.getIndexToRole(user.role);


    }

    public void editModeOn() {
        edit = true;
    }
    public void cancelEdit() {
        edit = false;
    }

    // backToMenu extended by Lin: (instance ref, reusing code from BackToMainMenu.cs)
    public void backToMenu() {
        BackToMainMenu btmm = gameObject.AddComponent<BackToMainMenu>();
        // equiv to BackToMainMenu btmm = new BackToMainMenu();
        // but Unity doesn't like it 
        btmm.ToMain();

        // Or alternatively like this, but 255 code smells :)
        //if (Login.2)
        //    SceneManager.LoadScene("Main Menu");
        //else if (Login.globalRole==1)
        //    SceneManager.LoadScene("Supervisor Menu");
        //else if (Login.globalRole==0
        //{ 
            // To be extended
            // SceneManager.LoadScene("Admin Menu");
        //}
    }
    public void saveChange() {
        FirstName = firstName.GetComponent<InputField>().text;
        LastName = lastName.GetComponent<InputField>().text;
        Email = email.GetComponent<InputField>().text;
        CurPassword = curPassword.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;

        KeyValuePair<int,string> error = cohort.editUser(Username, FirstName, LastName, Email, CurPassword, Password, ConfPassword, cohort.getRoleToIndex(Role));

        if (error.Key != 0) 
        {
            warning.GetComponent<Text>().text = error.Value;
            return;
        }

        warning.GetComponent<Text>().text = error.Value;
        edit = false;

    }

    // Update is called once per frame
    void Update()
    {

        saveBtn.SetActive(edit);
        cancelBtn.SetActive(edit);
        editBtn.SetActive(!edit);
        backBtn.SetActive(!edit);

        firstName.GetComponent<InputField>().interactable = edit;
        lastName.GetComponent<InputField>().interactable = edit;
        email.GetComponent<InputField>().interactable = edit;
        curPasswordtxt.SetActive(edit);
        passwordtxt.SetActive(edit);
        confPasswordtxt.SetActive(edit);
        curPassword.GetComponent<InputField>().interactable = edit;
        password.GetComponent<InputField>().interactable = edit;
        confPassword.GetComponent<InputField>().interactable = edit;

        if (edit == false)
        {
            Start();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (firstName.GetComponent<InputField>().isFocused)
            {
                lastName.GetComponent<InputField>().Select();
            }
            if (lastName.GetComponent<InputField>().isFocused)
            {
                email.GetComponent<InputField>().Select();
            }
            if (email.GetComponent<InputField>().isFocused)
            {
                curPassword.GetComponent<InputField>().Select();
            }
            if (curPassword.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                confPassword.GetComponent<InputField>().Select();
            }
        }

        if (edit == true) {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                saveChange();
            }
        }
    }
}
