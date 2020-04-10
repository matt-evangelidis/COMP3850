using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public GameObject warning;
    public GameObject register;

    public static string fullName; //we will send this to the main menu welcome text
    public static string globalUsername;
    public static string globalRole;
    private string Username;
    private string Password;
    private string firstName;
    private string lastName;
    private String[] lines;
    private String DecryptedPassword;

    public void registerButton()
    {
        //Application.LoadLevel("Register Menu");
        SceneManager.LoadScene("Register Menu");
    }

    public void forgetPassword() 
    {
        warning.GetComponent<Text>().text = "Please contact your admin for support!";
    }

    private bool adminLogin() {
        if (Username.Length < 5) return false;
        if (!Username.Substring(0, 5).Equals("admin", StringComparison.OrdinalIgnoreCase)) {
            return false;
        }
        bool PW = false; //password
        // validate password
        if (Password != "")
        {
            if (System.IO.File.Exists(@"database/login/admin/" + Username + ".txt"))
            {
                lines = System.IO.File.ReadAllLines(@"database/login/admin/" + Username + ".txt");
                //warning.GetComponent<Text>().text = "";

                //decrypt password in the database and compare
                int i = 1;
                DecryptedPassword = "";
                foreach (char c in lines[4])
                {
                    char Decrypted = (char)(c / i);
                    DecryptedPassword += Decrypted.ToString();
                    i++;
                }
                if (Password.Equals(DecryptedPassword))
                {
                    PW = true;
                }
                else
                {
                    warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                    Debug.LogWarning("password is wrong!");
                    return false;
                }
            }
            else
            {
                warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                Debug.LogWarning("Username doesn't exist");
                return false;
            }
        }
        else //password is empty
        {
            warning.GetComponent<Text>().text = "Password must not be EMPTY!";
            Debug.LogWarning("Password must not be EMPTY!");
            return false;
        }

        if (PW == true) {
            lines = System.IO.File.ReadAllLines(@"database/login/admin/" + Username + ".txt");
            firstName = lines[0];
            lastName = lines[1];
            fullName = firstName + ' ' + lastName;
            globalUsername = Username;
            globalRole = lines[5];

            SceneManager.LoadScene("Admin Menu");
            
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
        }
        return PW;
    }
    public void LoginButton() {

        bool UN = false; //username
        bool PW = false; //password
        string Role = "";

        warning.GetComponent<Text>().text = "";

        if (adminLogin() == true) return;

        //validate username
        if (Username != "")
        {
            if (Username.Length >= 5)
            {
                if (Username.Substring(0, 5).Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    adminLogin();
                }
            }
            // validate password
            if (Password != "")
            {
                if (System.IO.File.Exists(@"database/login/learner/" + Username + ".txt"))
                {
                    UN = true;
                    lines = System.IO.File.ReadAllLines(@"database/login/learner/" + Username + ".txt");
                    //warning.GetComponent<Text>().text = "";

                    //decrypt password in the database and compare
                    int i = 1;
                    DecryptedPassword = "";
                    foreach (char c in lines[4])
                    {
                        char Decrypted = (char)(c / i);
                        DecryptedPassword += Decrypted.ToString();
                        i++;
                    }
                    if (Password.Equals(DecryptedPassword))
                    {
                        PW = true;
                        Role = lines[5];
                        //warning.GetComponent<Text>().text = "";
                    }
                    else
                    {
                        warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                        Debug.LogWarning("password is wrong!");
                        return;
                    }
                }
                else
                {
                    warning.GetComponent<Text>().text = "Username doesn't exist or password is wrong!";
                    Debug.LogWarning("Username doesn't exist");
                    return;
                }
            }
            else //pass word is empty
            {
                warning.GetComponent<Text>().text = "Password must not be EMPTY!";
                Debug.LogWarning("Password must not be EMPTY!");
                return;
            }
        }
        else 
        {
            warning.GetComponent<Text>().text = "Username must not be EMPTY!";
            Debug.LogWarning("Username must not be EMPTY!");
            return;
        }

        if (UN == true && PW == true)
        {
            // edited by Lin: send name to main menu welcome text
            lines = System.IO.File.ReadAllLines(@"database/login/learner/" + Username  + ".txt");
            firstName = lines[0];
            lastName = lines[1];
            fullName = firstName + ' ' + lastName;
            globalUsername = Username;
            if (Role.Equals("Learner"))
            {
                SceneManager.LoadScene("Main Menu");

            } else if (Role.Equals("Supervisor")) {

                SceneManager.LoadScene("Supervisor Menu");
            }
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginButton();
        }
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
    }

}
