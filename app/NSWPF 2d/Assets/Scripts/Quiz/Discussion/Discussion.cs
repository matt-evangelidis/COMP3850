using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Discussion
{
    private string path = "database/discussion/";

    //singleton implementation
    private static Discussion instance = null;
    public static Discussion getDiscussion()
    {
        if (instance == null)
        {
            instance = new Discussion();
        }
        return instance;
    }


    private List<Thread> _threads = new List<Thread>();
    public List<Thread> threads { get { return _threads; } set { _threads = value; } }


    public KeyValuePair<int, string> addThread(Thread thread)
    //------------------------------------------------------
    //------------------------------------------------------
    {
        int errorCode = 0;
        string error_message = "";
        KeyValuePair<int, string>  error_return;

        if (thread.name == "" || thread.dateTime == "") 
        {
            errorCode = 1;
            error_message = "ERROR: cannot verify date time!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (thread.username == "") 
        {
            errorCode = 1;
            error_message = "ERROR: cannot verify username!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (thread.role == "") 
        {
            errorCode = 1;
            error_message = "ERROR: cannot verify role!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (thread.heading == "") 
        {
            errorCode = 1;
            error_message = "ERROR: heading is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }


        if (thread.content == "") 
        {
            errorCode = 1;
            error_message = "ERROR: content is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (thread.heading.Contains(";") || thread.heading.Contains("<END>")) 
        {
            errorCode = 1;
            error_message = "ERROR: Heading must not contain ';' or '<END>'!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }
        if (thread.content.Contains(";") || thread.content.Contains("<END>"))
        {
            errorCode = 1;
            error_message = "ERROR: content must not contain ';' or '<END>'!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        // no error then add it
        threads.Add(thread);

        //write to database:
        string form = thread.name + ";" + thread.username + ";" + thread.role + ";" + thread.heading + ";" + thread.dateTime + ";" + thread.content + "<END>";
        System.IO.File.WriteAllText(@path + thread.name + ".txt", form);

        errorCode = 0;
        error_message = "New Thread is added successfully!";
        error_return = new KeyValuePair<int, string>(errorCode, error_message);

        return error_return;
    }


    public Thread getThread(string threadName) 
    {
        foreach (Thread thread in threads)
        {
            if (thread.name.Equals(threadName))
            {
                return thread;
            }
        }
        return null;
    }
    private Discussion()
    //---------------------------------------------------
    // Getting all Threads
    //---------------------------------------------------
    {
        if (!System.IO.Directory.Exists(@path))
        {
            System.IO.Directory.CreateDirectory(@path);
        }
        DirectoryInfo directory = new DirectoryInfo(@path);

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

            if (post.Length != 6) {
                return;
            }

            // Extract data from the post
            string name = post[0];
            string username = post[1];
            string role = post[2];
            string heading = post[3];
            string dateTIme = post[4];
            string content = post[5];
            int noReplies = parts.Length - 2;

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
            Thread thread = new Thread(name,username,role,heading, content, dateTIme, replies.Count,replies);
            threads.Add(thread);
        }
    }
}

public class Thread 
{
    private string _name;
    private string _dateTime;
    private string _username;
    private string _role;
    private int _noReplies;
    private string _heading;
    private string _content;
    private List<Reply> _replies;

    private string path = "database/discussion/";

    public string name { get { return _name; } set { _name = value; } }
    public string username { get { return _username; } set { _username = value; } }
    public string role { get { return _role; } set { _role = value; } }
    public string dateTime { get { return _dateTime; } set { _dateTime = value; } }
    public int noReplies { get { return _noReplies; } set { _noReplies = value; } }
    public string heading { get { return _heading; } set { _heading = value; } }
    public string content { get { return _content; } set { _content = value; } }
    public List<Reply> replies { get { return _replies; } set { _replies = value; } }


    public KeyValuePair<int, string> addReply(Reply reply)
    //------------------------------------------------------
    // Add reply to a thread
    //------------------------------------------------------
    {
        int errorCode = 0;
        string error_message = "";
        KeyValuePair<int, string> error_return;

        if (reply.username == "") 
        {
            errorCode = 1;
            error_message = "ERROR: cannot verify username!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }
        if (reply.role == "")
        {
            errorCode = 1;
            error_message = "ERROR: cannot verify username!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }
        if (reply.content == "") 
        {
            errorCode = 1;
            error_message = "ERROR: your reply is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (reply.content.Contains(";") || reply.content.Contains("<END>"))
        {

            errorCode = 1;
            error_message = "Reply cannot contain ';' or '<END>'";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        // no error add reply
        replies.Add(reply);

        // write to database
        if (!System.IO.Directory.Exists(@path))
        {
            errorCode = 1;
            error_message = "Error: no database found! Please contact admin";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        if (!System.IO.File.Exists(@path + this.name + ".txt"))
        {
            errorCode = 1;
            error_message = "Error: no post found! Please contact admin";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        string form = reply.username + ";" + reply.role + ";" + reply.content + "<END>";

        File.AppendAllText(@path + this.name + ".txt", form);

        this.noReplies = this.replies.Count;

        errorCode = 0;
        error_message = "Reply is added successfully!";
        return error_return;
    }

    public Thread(string name, string username, string role, string heading, string content, string dateTime, int noReplies, List<Reply> replies)
    {
        this.name = name;
        this.username = username;
        this.dateTime = dateTime;
        this.noReplies = noReplies;
        this.heading = heading;
        this.content = content;
        this.replies = replies;
        this.role = role;
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
