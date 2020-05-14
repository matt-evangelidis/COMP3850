using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Discussion
{
    private List<Thread> _threads = new List<Thread>();
    public List<Thread> threads { get { return _threads; } set { _threads = value; } }
    public Discussion(string path)
    //---------------------------------------------------
    // Getting all Thread
    //---------------------------------------------------
    {
        DirectoryInfo directory = new DirectoryInfo(@path);
        if (directory == null) {
            Debug.LogWarning("No directory found");
            return;
        }

        //gettin text file
        FileInfo[] Files = directory.GetFiles("*.txt");

        foreach (FileInfo file in Files)
        {

            if (!System.IO.File.Exists(@path + file.Name)) continue;


            // Get date time
            string fileName = file.Name.Substring(0, file.Name.IndexOf("."));

            // Read content
            string text = System.IO.File.ReadAllText(@path + file.Name);
            string[] parts = text.Split(new[] { "<END>" }, StringSplitOptions.None);
            string[] post = parts[0].Split(';');

            if (post.Length != 5) {
                return;
            }

            // Extract data from the post
            string name = post[0];
            string username = post[1];
            string heading = post[2];
            string content = post[4];
            int noReplies = parts.Length - 2;
            string dateTIme = post[3];

            List<Reply> replies = new List<Reply>();

            // extract data from all reply
            if (parts.Length > 2) {
                // get replies
                for (int i = 1; i < parts.Length-1; i++)
                {
                    string[] reply_msg = parts[i].Split(';');
                    string reply_name = reply_msg[0];
                    string reply_role = reply_msg[1];
                    string reply_content = reply_msg[2];
                    Reply reply = new Reply(reply_name, reply_content, reply_role);
                    replies.Add(reply);
                }
            }

            Thread thread = new Thread(name,username,heading,content, dateTIme, replies.Count, replies);
            threads.Add(thread);
        }
    }
}

public class Thread 
{
    private string _name;
    private string _dateTime;
    private string _username;
    private int _noReplies;
    private string _heading;
    private string _content;
    private List<Reply> _replies;

    public string name { get { return _name; } set { _name = value; } }
    public string username { get { return _username; } set { _username = value; } }
    public string dateTime { get { return _dateTime; } set { _dateTime = value; } }
    public int noReplies { get { return _noReplies; } set { _noReplies = value; } }
    public string heading { get { return _heading; } set { _heading = value; } }
    public string content { get { return _content; } set { _content = value; } }
    public List<Reply> replies { get { return _replies; } set { _replies = value; } }

    public Thread(string name, string username, string heading, string content, string dateTime, int noReplies, List<Reply> replies) {
        this.name = name;
        this.username = username;
        this.dateTime = dateTime;
        this.noReplies = noReplies;
        this.heading = heading;
        this.content = content;
        this.replies = replies;
    }
}

public class Reply {
    private string _username;
    private string _content;
    private string _role;
    public string username { get { return _username; } set { _username = value; } }
    public string content { get { return _content; } set { _content = value; } }
    public string role { get { return _role; } set { _role = value; } }
    public Reply(string username, string content, string role)
    {
        this.username = username;
        this.content = content;
        this.role = role;
    }
}
