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

    private String[] lines;
    private String DecryptedPassword;
    private bool EmailValid = false;

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

    private string pathLearner = "database/login/learner/";
    private string pathSupervisor = "database/login/supervisor/";
    private string path = "";
    // Start is called before the first frame update
    void Start()
    {
        if (adminEditUsername.Equals(""))
        {
            warning.GetComponent<Text>().text = "Internal error, please contact develper";
            return;
        } else if (adminEditRole.Equals("")) 
        {
            warning.GetComponent<Text>().text = "Internal error, please contact develper";
            return;
        }

        if (adminEditRole.Equals("Learner"))
        {
            path = pathLearner;
        }
        else if (adminEditRole.Equals("Supervisor")) 
        {
            path = pathSupervisor;
        }
        if (!System.IO.File.Exists(@path + adminEditUsername + ".txt")) {
            warning.GetComponent<Text>().text = "Unexpected error, please contact development team";
            Debug.LogWarning("cannot find user file");
        }

        lines = System.IO.File.ReadAllLines(@path + adminEditUsername + ".txt");
        username.GetComponentInChildren<InputField>().text = adminEditUsername;
        firstName.GetComponentInChildren<InputField>().text = lines[0];
        lastName.GetComponentInChildren<InputField>().text = lines[1];
        email.GetComponentInChildren<InputField>().text = lines[3];
        role.GetComponentInChildren<InputField>().text = lines[5];
        Role = lines[5];
    }

    public void backToUserInfo()
    {
        if (adminEditRole.Equals("Learner"))
        {
            SceneManager.LoadScene("Learner Info");
        }
        else if (adminEditRole.Equals("Supervisor")) 
        {
            SceneManager.LoadScene("Supervisor Info");
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

        bool UN = false; //Username
        bool FN = false; //first name
        bool LN = false; //last name
        bool EM = false; //email
        bool newPW = false; // true if only new password
        bool PW = false; //password
        bool CPW = false; //Confirm password


        //validate Username
        if (Username != "")
        {
            UN = true;
            //warning.GetComponent<Text>().text = "";
        }
        else
        {
            warning.GetComponent<Text>().text = "Username is empty";
            Debug.LogWarning("Username is EMPTY!");
            return;
        }

        if (Username.Length >= 5) {
            if (Username.Substring(0, 5).Equals("admin", StringComparison.OrdinalIgnoreCase)) 
            {
                UN = false;
                warning.GetComponent<Text>().text = "must not start with 'admin'";
                return;
            }
            if (adminEditRole.Equals("Learner"))
            {
                if (Username.Substring(0, 5).Equals("nswpf", StringComparison.OrdinalIgnoreCase))
                {
                    UN = false;
                    warning.GetComponent<Text>().text = "Learners cannot start with 'nswpf'";
                    return;
                }
            }
            else if (adminEditRole.Equals("Supervisor"))
            {
                if (!Username.Substring(0, 5).Equals("nswpf", StringComparison.OrdinalIgnoreCase))
                {
                    UN = false;
                    warning.GetComponent<Text>().text = "Supervisor start with 'nswpf'";
                    return;
                }
                if (Username.Equals("nswpf", StringComparison.OrdinalIgnoreCase)) 
                {
                    UN = false;
                    warning.GetComponent<Text>().text = "Supervisor must not only 'nswpf'";
                    return;
                }
            }
        } 
        else if (Username.Length <= 5)
        {
            if (adminEditRole.Equals("Supervisor"))
            {
                UN = false;
                warning.GetComponent<Text>().text = "Supervisor must start with 'nswpf'";
                return;
            }
        }

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

        //decrypt password in the database and compare
        int i = 1;
        DecryptedPassword = "";
        foreach (char c in lines[4])
        {
            char Decrypted = (char)(c / i);
            DecryptedPassword += Decrypted.ToString();
            i++;
        }
        CurPassword = DecryptedPassword;

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

        //save new detail
        if (newPW == true)
        {
            if (UN == true && FN == true && LN == true && EM == true && PW == true && CPW == true)
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
                if (Username.Equals(adminEditUsername, StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.WriteAllText(@path + adminEditUsername + ".txt", form);
                }
                else if (!Username.Equals(adminEditUsername, StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.Delete(@path + adminEditUsername + ".txt");
                    System.IO.File.WriteAllText(@path + Username + ".txt", form);
                    adminEditUsername = Username;
                }
                edit = false;
                warning.GetComponent<Text>().text = "Changes are saved!";
                password.GetComponentInChildren<InputField>().text = "";
                confPassword.GetComponentInChildren<InputField>().text = "";
            }
        }
        else if (newPW == false)
        {
            if (UN == true && FN == true && LN == true && EM == true)
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
                if (Username.Equals(adminEditUsername, StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.WriteAllText(@path + adminEditUsername + ".txt", form);
                }
                else if (!Username.Equals(adminEditUsername, StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.Delete(@path + adminEditUsername + ".txt");
                    System.IO.File.WriteAllText(@path + Username + ".txt", form);
                    adminEditUsername = Username;
                }
                edit = false;
                warning.GetComponentInChildren<Text>().text = "Changes are saved!";
                password.GetComponentInChildren<InputField>().text = "";
                confPassword.GetComponentInChildren<InputField>().text = "";
                if (Login.globalRole!=0)
                {
                    Login.fullName = FirstName + " " + LastName;
                }
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

        username.GetComponentInChildren<InputField>().interactable = edit;
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

