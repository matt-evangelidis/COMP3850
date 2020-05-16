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

    private string filePath = "database/discussion/";

    private string content;

    // Start is called before the first frame update
    void Start()
    {
        Reply.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline;
        Reply.GetComponent<InputField>().text = "";

        if (Login.globalUsername == null)
        {
            warning.GetComponent<Text>().text = "cannot verify username! Please contact admin";
            return;
        }

        if (Login.globalRole == null) {
            warning.GetComponent<Text>().text = "cannot verify role! Please contact admin";
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
    public void uploadReply() {
        if (content.Equals("")) {
            warning.GetComponent<Text>().text = "Nothing to upload";
            return;
        }

        if (content.Contains(";") || content.Contains("<END>")) {
            warning.GetComponent<Text>().text = "Reply cannot contain ';' or '<END>'";
            return;
        }

        if (!System.IO.Directory.Exists(@filePath))
        {
            warning.GetComponent<Text>().text = "Error: no database found! Please contact admin";
            return;
        }

        if (!System.IO.File.Exists(@filePath + DiscussionDetail.post.name + ".txt"))
        {
            warning.GetComponent<Text>().text = "Error: no post found! Please contact admin";
            return;
        }

        string form = Login.globalUsername + ";" + Login.globalRole + ";" + content + "<END>"; 

        File.AppendAllText(@filePath + DiscussionDetail.post.name + ".txt", form);

        SearchingDiscussion.postDetail.noReplies += 1;
        Reply newReply = new Reply(Login.globalUsername, content, Login.globalRole);
        SearchingDiscussion.postDetail.replies.Add(newReply);

        SceneManager.LoadScene("Searching Discussion Detail");
    }
}
