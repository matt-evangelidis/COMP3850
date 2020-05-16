using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DiscussionAddReply : MonoBehaviour
{
    public GameObject warning;
    public GameObject Reply;

    Discussion discussion;

    private string content;

    public Cohort cohort;

    // Start is called before the first frame update
    void Start()
    {
        discussion = Discussion.getDiscussion();
        cohort = Cohort.getCohort();
        Reply.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline;
        Reply.GetComponent<InputField>().text = "";

        if (Login.globalUsername == null)
        {
            warning.GetComponent<Text>().text = "cannot verify username! Please contact admin";
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        content = Reply.GetComponent<InputField>().text;
    }
    public void cancel() {
        Reply.GetComponent<InputField>().text = "";
        SceneManager.LoadScene("Searching Discussion Detail");
    }
    public void uploadReply() 
    {
        string role = cohort.getIndexToRole(Login.globalRole);
        Reply reply = new Reply(Login.globalUsername,content,role);
        Thread th = discussion.getThread(DiscussionDetail.post.name);
        KeyValuePair<int,string>  error = th.addReply(reply);
        if (error.Key == 1) 
        {
            warning.GetComponent<Text>().text = error.Value;
            return;
        }

        SceneManager.LoadScene("Searching Discussion Detail");
    }
}
