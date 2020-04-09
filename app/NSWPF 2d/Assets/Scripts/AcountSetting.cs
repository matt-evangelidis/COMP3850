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
    private string form;

    private String[] lines;
    private String DecryptedPassword;
    private bool EmailValid = false;

    private bool edit = false;


    // Start is called before the first frame update
    void Start()
    {
        Username = Login.globalUsername;
        if (System.IO.File.Exists(@"database/login/learner/" + Username + ".txt"))
        {
            lines = System.IO.File.ReadAllLines(@"database/login/learner/" + Username + ".txt");
            username.GetComponent<TextMeshProUGUI>().text = Username;
            firstName.GetComponent<InputField>().text = lines[0];
            lastName.GetComponent<InputField>().text = lines[1];
            email.GetComponent<InputField>().text = lines[3];
            role.GetComponent<TextMeshProUGUI>().text = lines[5];
            Role = lines[5];
        }
        else 
        {
            warning.GetComponent<Text>().text = "There is an internal error, please contact admin";
            Debug.LogWarning("file not exist" + Username);
        }

    }

    void EmailValidation()
    {
        foreach (char c in Email)
        {
            if ((int)c > 32 && (int)c < 126)
                EmailValid = true;
            else
                EmailValid = false;
        }
    }

    public void editModeOn() {
        edit = true;
    }
    public void cancelEdit() {
        edit = false;
    }
    public void backToMenu() {
        SceneManager.LoadScene("Main Menu");
    }
    public void saveChange() {
        FirstName = firstName.GetComponent<InputField>().text;
        LastName = lastName.GetComponent<InputField>().text;
        Email = email.GetComponent<InputField>().text;
        CurPassword = curPassword.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;

        
        //do everything in here
        bool FN = false; //first name
        bool LN = false; //last name
        bool EM = false; //email
        bool newPW = false; // true if only new password
        bool CurPW = false; // current password
        bool PW = false; //password
        bool CPW = false; //Confirm password

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
        if (LastName != "")
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
            newPW = true;
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
            newPW = false;
            Debug.LogWarning("Password field EMPTY!");
        }



        //decrypt password in the database and compare
        int i = 1;
        DecryptedPassword = "";
        foreach (char c in lines[4])
        {
            char Decrypted = (char)(c / i);
            DecryptedPassword += Decrypted.ToString();
            i++;
        }

        if (CurPassword.Equals(DecryptedPassword)) 
        {
            CurPW = true;
        }
        else
        {
            CurPW = false;
            warning.GetComponent<Text>().text = "Current password is INCORRECT!";
            Debug.LogWarning("current password is wrong!");
            return;
        }

        //validate confirm password
        if (newPW == true)
        {
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
        }
        else if (newPW == false) 
        {
            if (ConfPassword != "") 
            {
                warning.GetComponent<Text>().text = "Confirm Password must match the new password";
                Debug.LogWarning("No new password but confirm password is filled");
            }
        }

        if (newPW == true)
        {
            if (FN == true && LN == true && EM == true && CurPW == true && PW == true && CPW == true)
            {
                bool Clear = true;
                i = 1;
                foreach (char c in Password)
                {
                    if (Clear)
                    {
                        Password = "";
                        Clear = false;
                    }
                    char Encrypted = (char)(c * i);
                    Password += Encrypted.ToString();
                    i++;
                }
                form = (FirstName + "\n" + LastName + "\n" + Username + "\n" + Email + "\n" + Password + "\n" + Role);
                System.IO.File.WriteAllText(@"database/login/learner/" + Username + ".txt", form);
                edit = false;
                warning.GetComponent<Text>().text = "Changes are saved!";
                curPassword.GetComponent<InputField>().text = "";
                password.GetComponent<InputField>().text = "";
                confPassword.GetComponent<InputField>().text = "";
                Login.fullName = FirstName + " " + LastName;
            }
        }
        else if (newPW == false)
        {
            if (FN == true && LN == true && EM == true && CurPW == true) 
            {
                bool Clear = true;
                i = 1;
                foreach (char c in CurPassword)
                {
                    if (Clear)
                    {
                        CurPassword = "";
                        Clear = false;
                    }
                    char Encrypted = (char)(c * i);
                    CurPassword += Encrypted.ToString();
                    i++;
                }
                form = (FirstName + "\n" + LastName + "\n" + Username + "\n" + Email + "\n" + CurPassword + "\n" + Role);
                System.IO.File.WriteAllText(@"database/login/learner/" + Username + ".txt", form);
                edit = false;
                warning.GetComponent<Text>().text = "Changes are saved!";
                curPassword.GetComponent<InputField>().text = "";
                password.GetComponent<InputField>().text = "";
                confPassword.GetComponent<InputField>().text = "";
                Login.fullName = FirstName + " " + LastName;
            }
        }
        

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
