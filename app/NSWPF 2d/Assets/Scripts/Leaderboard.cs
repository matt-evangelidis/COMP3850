using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System.Linq;

public class Leaderboard
//--------------------------------------------
// This is singleton implementation for all achievement
//--------------------------------------------
{

    private string path = "database/leaderboard/quiz/";


    //singleton implementation
    private static Leaderboard instance = null;
    public static Leaderboard getLeaderboard()
    {
        if (instance == null)
        {
            instance = new Leaderboard();
        }
        return instance;
    }

    private List<Achievement> _achievements;
    public List<Achievement> achievements { get { return _achievements; } set { _achievements = value; } }

    public Achievement getAchievement(string username)
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.username.Equals(username))
                return achievement;
        }
        return null;
    }

    public void addAchievement(int correct, int total) 
    //-------------------------------------------------
    // save and add achievement to leaderboard
    //-------------------------------------------------
    {
        string finalResult = correct.ToString() + "," + total.ToString() + ";"; ;

        Attempt attempt = new Attempt(correct, total);

        

        if (!System.IO.Directory.Exists(@path))
        {
            System.IO.Directory.CreateDirectory(@path);
        }
        if (!System.IO.File.Exists(@path + Login.globalUsername + ".txt"))
        {
            System.IO.File.WriteAllText(@path + Login.globalUsername + ".txt", finalResult);
            
            List<Attempt> attempts = new List<Attempt>();
            attempts.Add(attempt);
            Achievement achievement = new Achievement(Login.globalUsername, attempts);
            this.achievements.Insert(this.findIndexAchievement(achievements, Login.globalUsername),achievement);
        }
        else
        {
            File.AppendAllText(@path + Login.globalUsername + ".txt", finalResult);
            this.getAchievement(Login.globalUsername).attempts.Add(attempt);
            this.getAchievement(Login.globalUsername).update();
        }

    }

    private int findIndexAchievement(List<Achievement> achievements,string username)

    {
        int index = 0;
        foreach (Achievement a in achievements)
        {
            // Keep traversing
            if (a.username.CompareTo(username) < 0)
            {
                index++;
                continue;
            }

            // Found the place
            if (a.username.CompareTo(username) > 0)
            {
                Debug.Log(index);
                return index;
            }
        }
        return index;
    }
    

    private Leaderboard()
    //---------------------------------------------------
    // Getting all learners achivement
    //---------------------------------------------------
    {
        if (!System.IO.Directory.Exists(@path)) 
        {
            System.IO.Directory.CreateDirectory(@path);
        }

        DirectoryInfo directory = new DirectoryInfo(@path);
        if (directory == null) return;

        achievements = new List<Achievement>();

        //gettin text file
        FileInfo[] Files = directory.GetFiles("*.txt");
        string filename = "";

        foreach (FileInfo file in Files) 
        {
            filename = file.Name.Substring(0, file.Name.IndexOf("."));
            if (!System.IO.File.Exists(@path + file.Name)) continue;
            string[] lines = System.IO.File.ReadAllLines(@path + file.Name);
            string[] tries = lines[0].Split(';');

            List<Attempt> attempts = new List<Attempt>();

            for (int i = 0; i < tries.Length-1; i++) {
                string[] items = tries[i].Split(',');
                Debug.Log(items.Length);
                int correct = Int32.Parse(items[0]);
                int total = Int32.Parse(items[1]);
                Attempt attempt = new Attempt(correct,total);
                attempts.Add(attempt);
            }

            Achievement achievement = new Achievement(filename,attempts);
            achievements.Add(achievement);
        }
    }
}

public class Achievement 
{
    private string _username;
    private int _noAttempts;
    private int _bestAttemp;
    private float _bestScore;

    private List<Attempt> _attempts;

    public List<Attempt> attempts { get { return _attempts; } set { _attempts = value; } }

    public string username { get { return _username; } set { _username = value; } }
    public int noAttempts { get { return _noAttempts; } set { _noAttempts = value; } }
    public int bestAttempt { get { return _bestAttemp; } set { _bestAttemp = value; } }
    public float bestScore { get { return _bestScore; } set { _bestScore = value; } }

    public void update() 
    {
        foreach (Attempt attempt in this.attempts)
        {
            if (attempt.percent > this.bestScore)
            {
                this.bestScore = attempt.percent;
                this.bestAttempt = attempts.IndexOf(attempt) + 1;
            }
        }
        this.noAttempts = attempts.Count;
    }
    public Achievement(string username,List<Attempt> attempts) 
    {
        this.username = username;
        this.noAttempts = attempts.Count;
        this.attempts = attempts;

        this.bestScore = -1;
        this.bestAttempt = -1;

        Debug.Log("attempt size: " + attempts.Count);

        foreach (Attempt a in this.attempts) 
        {
            Debug.Log(a.percent + " " + bestScore);
            if (a.percent > bestScore) 
            {
                bestScore = a.percent;
                bestAttempt = this.attempts.IndexOf(a) + 1;
            }
        }

        Debug.Log("Best score: " + bestScore);
    }
}

public class Attempt
{
    private int _noCorrects;
    private int _noQuestion;
    private float _percent;

    public int noCorrects { get { return _noCorrects; } set { _noCorrects = value; } }
    public int noQuestion { get { return _noQuestion; } set { _noQuestion = value; } }
    public float percent { get { return _percent; } set { _percent = value; } }

    public Attempt(int noCorrects, int noQuestion)
    {
        this.noCorrects = noCorrects;
        this.noQuestion = noQuestion;
        this.percent = (float)noCorrects / (float)noQuestion * 100;
    }
}
