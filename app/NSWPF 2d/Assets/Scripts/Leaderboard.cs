using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;

public class Leaderboard
{
    private List<Achievement> _achievements = new List<Achievement>();
    public List<Achievement> achievements { get { return _achievements; } set { _achievements = value; } }
    public Leaderboard(List<Achievement> achievements) 
    {
        this.achievements = achievements;
    }

    public Achievement getAchievement(string username)
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.username.Equals(username))
                return achievement;
        }
        return null;
    }

    public Leaderboard(string path)
    //---------------------------------------------------
    // Getting all learners achivement
    //---------------------------------------------------
    {
        DirectoryInfo directory = new DirectoryInfo(@path);
        if (directory == null) return;

        //gettin text file
        FileInfo[] Files = directory.GetFiles("*.txt");
        string filename = "";
        string noCorrects = "";
        string noQuestion = "";
        string noAttempts = "";
        string bestAttemp = "";
        foreach (FileInfo file in Files) 
        {
            filename = file.Name.Substring(0, file.Name.IndexOf("."));
            if (!System.IO.File.Exists(@path + file.Name)) continue;
            string[] lines = System.IO.File.ReadAllLines(@path + file.Name);
            string[] attempts = lines[0].Split(';');
            float bestScore = 0.0f;
            int attemptIndex = 0;
            for (int i = 0; i < attempts.Length-1; i++)
            {
                string[] results = attempts[i].Split(',');
                string correct = results[0];
                string total = results[1];
                if ((float.Parse(correct) / float.Parse(total)) > bestScore) {
                    bestScore = (float.Parse(correct) / float.Parse(total));
                    attemptIndex = i+1;
                    noCorrects = correct.ToString();
                    noQuestion = total.ToString();
                    
                    bestAttemp = "#"+attemptIndex.ToString();
                    

                }
            }
            noAttempts = (attempts.Length-1).ToString();

            Achievement achievement = new Achievement(filename,noCorrects,noQuestion,noAttempts,bestAttemp);
            achievements.Add(achievement);
        }
    }
}

public class Achievement 
{
    private string _username;
    private string _noCorrects;
    private string _noQuestion;
    private string _percent;
    private string _noAttempts;
    private string _bestAttemp;
    public string username { get { return _username; } set { _username = value; } }
    public string noCorrects { get { return _noCorrects; } set { _noCorrects = value; } }
    public string noQuestion { get { return _noQuestion; } set { _noQuestion = value; } }

    public string percent { get { return _percent; } set { _percent = value; } }
    public string noAttempts { get { return _noAttempts; } set { _noAttempts = value; } }
    public string bestAttemp { get { return _bestAttemp; } set { _bestAttemp = value; } }
    public Achievement(string username, string noCorrects, string noQuestion, string noAttempts, string bestAttemp) 
    {
        this.username = username;
        this.noAttempts = noAttempts;
        this.bestAttemp = bestAttemp;
        this.noCorrects = noCorrects;
        this.noQuestion = noQuestion;
        this.percent = ((float.Parse(noCorrects) / float.Parse(noQuestion)) * 100).ToString() + "%";
    }
}
