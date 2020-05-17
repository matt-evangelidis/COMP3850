using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cohort
//--------------------------------------------
// This is singleton implementation for all account
//--------------------------------------------
{
    private string learnerPath = "database/login/learner/";
    private string supervisorPath = "database/login/supervisor/";
    private string adminPath = "database/login/admin/";

    //singleton implementation
    private static Cohort instance = null;
    public static Cohort getCohort()
    {
        if (instance == null)
        {
            instance = new Cohort();
        }
        return instance;
    }

    //list of admin
    private List<User> admins = new List<User>();
    public List<User> getAdmins()
    {
        return admins;
    }

    // list of learners
    private List<User> learners = new List<User>();
    public List<User> getLearners()
    {
        return learners;
    }
    // list of supervisor
    private List<User> supervisors = new List<User>();
    public List<User> getSupervisors() {
        return supervisors;
    }

    public User getUser(string username) {
        foreach (User user in learners) {
            if (user.username.Equals(username, StringComparison.OrdinalIgnoreCase))
                return user;
        }

        foreach (User user in supervisors)
        {
            if (user.username.Equals(username, StringComparison.OrdinalIgnoreCase))
                return user;
        }

        foreach (User user in admins)
        {
            if (user.username.Equals(username, StringComparison.OrdinalIgnoreCase))
                return user;
        }

        return null;
    }

    public KeyValuePair<int, string> addUser(string username, string firstname, string lastname, string email, string password, string confPassword, int role)
    //--------------------------------------------------
    // Add user to database (create new account)
    // Return value for int:
    // ------------ 0 if all good
    // ------------ 1 if error
    // Return value for string: message
    //--------------------------------------------------
    {
        int errorCode = 0;
        string error_message = "";
        KeyValuePair<int, string> error_return;

        bool FN = false; //first name
        bool LN = false; //last name
        bool UN = false; //username
        bool EM = false; //email
        bool PW = false; //password
        bool CPW = false; //Confirm password
        bool R = false; //role

        // Validate First Name
        if (firstname != "")
        {
            FN = true;
        }
        else
        {
            errorCode = 1;
            error_message = "ERROR: The firstname field is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        //Validate Last Name
        if (lastname != "")
        {
            LN = true;
            //warning.GetComponent<Text>().text = "";
        }
        else
        {
            errorCode = 1;
            error_message = "ERROR: The lastname field is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        //validate username
        if (username != "")
        {
            // Check username rule:
            // must not start with nswpf and admin
            if (username.Length >= 5)
            {
                if (username.Substring(0, 5).Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    errorCode = 1;
                    error_message = "ERROR: Username cannot start with 'admin'";
                    error_return = new KeyValuePair<int, string>(errorCode, error_message);
                    return error_return;
                }

                if (role == 2) // learner
                {
                    if (username.Substring(0, 5).Equals("nswpf", StringComparison.OrdinalIgnoreCase)) {
                        errorCode = 1;
                        error_message = "ERROR: Username cannot start with 'admin' or 'nswpf' keywords";
                        error_return = new KeyValuePair<int, string>(errorCode, error_message);
                        return error_return;
                    }
                }
            }

            // Check existance
            if (this.getUser(username) == null)
            {
                UN = true;
            }
            else //username is already exist
            {
                errorCode = 1;
                error_message = "ERROR: the username is already exist!";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                return error_return;
            }
        }
        else // username is empty
        {
            errorCode = 1;
            error_message = "ERROR: username is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        // Validate Email
        if (email != "")
        {
            if (email.Contains("@"))
            {
                if (email.Contains("."))
                {
                    EM = true;
                    //warning.GetComponent<Text>().text = "";
                }
                else
                {
                    errorCode = 1;
                    error_message = "ERROR: invalid email!";
                    error_return = new KeyValuePair<int, string>(errorCode, error_message);
                    return error_return;
                }
            }
            else
            {
                errorCode = 1;
                error_message = "ERROR: invalid email!";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                return error_return;
            }
        }
        else
        {
            errorCode = 1;
            error_message = "ERROR: Email field is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        //validate password

        if (password != "")
        {
            if (password.Length > 5)
            {
                PW = true;
            }
            else
            {
                errorCode = 1;
                error_message = "ERROR: Password must have at least 6 characters!";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                return error_return;
            }
        }
        else
        {
            errorCode = 1;
            error_message = "ERROR: Password must have at least 6 characters!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        //validate confirm password
        if (confPassword != "")
        {
            if (confPassword.Equals(password))
            {
                CPW = true;
                //warning.GetComponent<Text>().text = "";
            }
            else
            {
                errorCode = 1;
                error_message = "ERROR: Confirm password does not match password!";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                return error_return;
            }
        }
        else
        {
            errorCode = 1;
            error_message = "ERROR: Confirm password field is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        // ROlE: admin = 0, supervisor = 1, learner = 2.
        if (role > 0 && role < 3) {
            R = true;
        }

        // Add new user to cohort
        if (FN == true && LN == true && UN == true && EM == true && PW == true && CPW == true && R == true)
        {
            //encrypt password
            bool Clear = true;
            int i = 1;
            foreach (char c in password)
            {
                if (Clear)
                {
                    password = "";
                    Clear = false;
                }
                char Encrypted = (char)(c * i);
                password += Encrypted.ToString();
                i++;
            }

            // Add new user to list
            User user = new User(username, firstname, lastname, email, password, role);
            if (role == 1) //add to supervisor
            {
                // Find position to insert
                int index = findIndexUser(supervisors, username);

                // Insert into list 
                supervisors.Insert(index, user);

                // Add new user to database
                string form = (firstname + "\n" + lastname + "\n" + username + "\n" + email + "\n" + password + "\n" + getIndexToRole(role));
                System.IO.File.WriteAllText(@supervisorPath + username + ".txt", form);

            }
            else if (role == 2)
            {
                // Find position to insert
                int index = findIndexUser(learners, username);

                // Insert into list 
                learners.Insert(index, user);

                // Add new user to database
                string form = (firstname + "\n" + lastname + "\n" + username + "\n" + email + "\n" + password + "\n" + getIndexToRole(role));
                Debug.LogWarning("form: " + form);
                System.IO.File.WriteAllText(@learnerPath + username + ".txt", form);
            }
        }
        error_return = new KeyValuePair<int, string>(errorCode, error_message);
        return error_return;
    }

    public KeyValuePair<int, string> validateLogin(string username, string password)
    //--------------------------------------------------
    // Call this function to check the user if it's valid to login
    //--------------------------------------------------
    {
        int errorCode = 0;
        string error_message = "";
        KeyValuePair<int, string> error_return;

        if (username == "") //username is empty
        {
            errorCode = 1;
            error_message = "Username is empty";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (password == "") // password is empty
        {
            errorCode = 1;
            error_message = "password is empty";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (username.Length >= 5)
        {
            if (username.Substring(0, 5).Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                //admin login
                foreach (User user in admins)
                {
                    if (user.username.Equals(username)) // Found username
                    {
                        // Validate password
                        if (password.Equals(user.decryptPassword()))
                        {
                            errorCode = 0;
                            error_message = "Login Succeeded!";
                            error_return = new KeyValuePair<int, string>(errorCode, error_message);
                            return error_return;
                        }
                        else
                        {
                            errorCode = 1;
                            error_message = "Username or password incorrect";
                            error_return = new KeyValuePair<int, string>(errorCode, error_message);
                            Debug.LogWarning("Password incorrect");
                            return error_return;
                        }
                    }
                }

                // no user found
                errorCode = 1;
                error_message = "Username or password incorrect";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                Debug.LogWarning("Password incorrect");
                return error_return;
            }
            else if (username.Substring(0, 5).Equals("nswpf", StringComparison.OrdinalIgnoreCase))
            {
                // Supervisor login
                foreach (User user in supervisors)
                {
                    if (user.username.Equals(username)) // Found username
                    {
                        // Validate password
                        if (password.Equals(user.decryptPassword()))
                        {
                            errorCode = 0;
                            error_message = "Login Succeeded!";
                            error_return = new KeyValuePair<int, string>(errorCode, error_message);
                            return error_return;
                        }
                        else
                        {
                            errorCode = 1;
                            error_message = "Username or password incorrect";
                            error_return = new KeyValuePair<int, string>(errorCode, error_message);
                            Debug.LogWarning("Password incorrect");
                            return error_return;
                        }
                    }
                }
                // no user found
                errorCode = 1;
                error_message = "Username or password incorrect";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                Debug.LogWarning("Password incorrect");
                return error_return;
            }
            else
            {
                //learner login
                foreach (User user in learners)
                {
                    if (user.username.Equals(username)) // Found username
                    {
                        // Validate password
                        if (password.Equals(user.decryptPassword()))
                        {
                            errorCode = 0;
                            error_message = "Login Succeeded!";
                            error_return = new KeyValuePair<int, string>(errorCode, error_message);
                            return error_return;
                        }
                        else
                        {
                            errorCode = 1;
                            error_message = "Username or password incorrect";
                            error_return = new KeyValuePair<int, string>(errorCode, error_message);
                            Debug.LogWarning("Password incorrect");
                            return error_return;
                        }
                    }
                }
                // no user found
                errorCode = 1;
                error_message = "Username or password incorrect";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                Debug.LogWarning("Password incorrect");
                return error_return;
            }
        }
        else
        {
            //learner login
            foreach (User user in learners)
            {
                if (user.username.Equals(username)) // Found username
                {
                    // Validate password
                    if (password.Equals(user.decryptPassword()))
                    {
                        errorCode = 0;
                        error_message = "Login Succeeded!";
                        error_return = new KeyValuePair<int, string>(errorCode, error_message);
                        return error_return;
                    }
                    else
                    {
                        errorCode = 1;
                        error_message = "Username or password incorrect";
                        error_return = new KeyValuePair<int, string>(errorCode, error_message);
                        Debug.LogWarning("Password incorrect");
                        return error_return;
                    }
                }
            }
            // no user found
            errorCode = 1;
            error_message = "Username or password incorrect";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            Debug.LogWarning("Password incorrect");
            return error_return;
        }
    }

    private int findIndexUser(List<User> users,string username) 
    // Find Index to insert
    {
        int index = 0;
        foreach (User user in users) {

            // Keep traversing
            if (user.username.CompareTo(username) < 0) {
                index++;
                continue;
            }

            // Found the place
            if (user.username.CompareTo(username) > 0) {
                return index;
            }
        }
        return 0;
    }

    public int getRoleToIndex(string role) 
    //------------------------------------------
    // Conver role to index of role
    //------------------------------------------
    {
        if (role.Equals("learner", StringComparison.OrdinalIgnoreCase)) return 2;
        if (role.Equals("supervisor", StringComparison.OrdinalIgnoreCase)) return 1;
        if (role.Equals("admin", StringComparison.OrdinalIgnoreCase)) return 0;
        return -1;
    }

    public string getIndexToRole(int index)
    //------------------------------------------
    // Conver index of role to role
    //------------------------------------------
    {
        if (index == 0) return "admin";
        if (index == 1) return "supervisor";
        if (index == 2) return "learner";
        return null;
    }
    
    private Cohort()
    //---------------------------------------------------
    // Getting alll users
    //---------------------------------------------------
    {
        // Getting alll users

        if (!System.IO.Directory.Exists(@learnerPath))
        {
            System.IO.Directory.CreateDirectory(@learnerPath);
        }
        if (!System.IO.Directory.Exists(@supervisorPath))
        {
            System.IO.Directory.CreateDirectory(@supervisorPath);
        }
        if (!System.IO.Directory.Exists(@adminPath))
        {
            System.IO.Directory.CreateDirectory(@adminPath);
        }

        DirectoryInfo learnerDir = new DirectoryInfo(@learnerPath);
        DirectoryInfo supervisorDir = new DirectoryInfo(@supervisorPath);
        DirectoryInfo adminDir = new DirectoryInfo(@adminPath);
        if (learnerDir == null && supervisorDir == null && adminDir == null) return;

        // Getting Text files for learner
        if (learnerDir != null) { 
            FileInfo[] Files = learnerDir.GetFiles("*.txt");
            string filename = "";
            foreach (FileInfo file in Files)
            {
                filename = file.Name.Substring(0, file.Name.IndexOf(".")); ;
                if (!System.IO.File.Exists(@learnerPath + file.Name)) continue;
                string[] lines = System.IO.File.ReadAllLines(@learnerPath + file.Name);
                string firstName = lines[0];
                string lastName = lines[1];
                string userName = lines[2];
                string email = lines[3];
                string password = lines[4];
                int role = getRoleToIndex(lines[5]);
                User user = new User(userName, firstName, lastName, email, password, role);
                learners.Add(user);
            }
        }

        if (supervisorDir != null)
        {
            FileInfo[] Files = supervisorDir.GetFiles("*.txt");
            string filename = "";
            foreach (FileInfo file in Files)
            {
                filename = file.Name.Substring(0, file.Name.IndexOf(".")); ;
                if (!System.IO.File.Exists(@supervisorPath + file.Name)) continue;
                string[] lines = System.IO.File.ReadAllLines(@supervisorPath + file.Name);
                string firstName = lines[0];
                string lastName = lines[1];
                string userName = lines[2];
                string email = lines[3];
                string password = lines[4];
                int role = getRoleToIndex(lines[5]);
                User user = new User(userName, firstName, lastName, email, password, role);
                supervisors.Add(user);
            }
        }

        if (adminDir != null)
        {
            FileInfo[] Files = adminDir.GetFiles("*.txt");
            string filename = "";
            foreach (FileInfo file in Files)
            {
                filename = file.Name.Substring(0, file.Name.IndexOf(".")); ;
                if (!System.IO.File.Exists(@adminPath + file.Name)) continue;
                string[] lines = System.IO.File.ReadAllLines(@adminPath + file.Name);
                string firstName = lines[0];
                string lastName = lines[1];
                string userName = lines[2];
                string email = lines[3];
                string password = lines[4];
                int role = getRoleToIndex(lines[5]);
                User user = new User(userName, firstName, lastName, email, password, role);
                admins.Add(user);
            }
        }
    }
}

public class User
{
    private string _username;
    private string _firstname;
    private string _lastname;
    private string _fullname;
    private string _email;
    private string _encryptedPassword;
    private int _role; //0 is admin, 1 is supervisor, 2 is learner

    public string username { get { return _username; } set { _username = value; } }
    public string firstname { get { return _firstname; } set { _firstname = value; } }
    public string lastname { get { return _lastname; } set { _lastname = value; } }
    public string fullname { get { return _fullname; } set { _fullname = value; } }
    public string email { get { return _email; } set { _email = value; } }
    public string encryptedPassword { get { return _encryptedPassword; } set { _encryptedPassword = value; } }
    public int role { get { return _role; } set { _role = value; } }
    public string decryptPassword() {
        int i = 1;
        string decryptedPassword = "";
        foreach (char c in encryptedPassword)
        {
            char Decrypted = (char)(c / i);
            decryptedPassword += Decrypted.ToString();
            i++;
        }
        return decryptedPassword;
    }

    public User(string username, string firstname, string lastname, string email, string password, int role) {
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.encryptedPassword = password;
        this.role = role;
        this.fullname = firstname + " " + lastname;
    }
}
