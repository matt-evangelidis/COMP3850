using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;

public class Cohort
{
    public Cohort cohort;
    private List<Learner> _learners = new List<Learner>();
    public List<Learner> learners { get { return _learners; } set { _learners = value; } }
    public Cohort(List<Learner> learners) {
        this.learners = learners;
    }
    public Learner getLearner(string username) {
        foreach (Learner learner in learners) {
            if (learner.username.Equals(username))
                return learner;
        }
        return null;
    }

    public Cohort() {
        // Getting alll learners
        DirectoryInfo d = new DirectoryInfo(@"database/login/learner/");
        if (d == null) return;

        // Getting Text files
        FileInfo[] Files = d.GetFiles("*.txt");
        string filename = "";
        foreach (FileInfo file in Files)
        {
            filename = file.Name.Substring(0, file.Name.IndexOf(".")); ;
            if (!System.IO.File.Exists(@"database/login/learner/" + file.Name)) continue;
            string[] lines = System.IO.File.ReadAllLines(@"database/login/learner/" + file.Name);
            string firstName = lines[0];
            string lastName = lines[1];
            string userName = lines[2];
            string email = lines[3];
            string password = lines[4];
            string role = lines[5];
            Learner learner = new Learner(userName, firstName, lastName, email, password, role);
            learners.Add(learner);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Learner
{
    private string _username;
    private string _firstname;
    private string _lastname;
    private string _fullname;
    private string _email;
    private string _encryptedPassword;
    private string _role;

    public string username { get { return _username; } set { _username = value; } }
    public string firstname { get { return _firstname; } set { _firstname = value; } }
    public string lastname { get { return _lastname; } set { _lastname = value; } }
    public string fullname { get { return _fullname; } set { _fullname = value; } }
    public string email { get { return _email; } set { _email = value; } }
    public string encryptedPassword { get { return _encryptedPassword; } set { _encryptedPassword = value; } }
    public string role { get { return _role; } set { _role = value; } }
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

    public Learner(string username, string firstname, string lastname, string email, string password, string role) {
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.encryptedPassword = password;
        this.role = role;
        this.fullname = firstname + " " + lastname;
    }
}
