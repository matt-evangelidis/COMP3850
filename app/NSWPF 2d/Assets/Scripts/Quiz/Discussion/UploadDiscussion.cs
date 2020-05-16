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

public class UploadDiscussion : MonoBehaviour
{
    public GameObject warning;
    public GameObject Heading;
    public GameObject Content;

    private string heading;
    private string content;
    private string form;

    public Discussion discussion;
    public Cohort cohort;

    public void back() {
        SceneManager.LoadScene("Searching Discussion");
    }

    public void upload() {

        string now = DateTime.Now.ToString("yyyy-MM-dd,HH-mm-ss");
        string dateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        string role = cohort.getIndexToRole(Login.globalRole);
        Thread newThread = new Thread(now,Login.globalUsername,role,heading,content,dateTime,0, new List<Reply>());


        KeyValuePair<int,string> error = discussion.addThread(newThread);

        if (error.Key == 1) 
        {
            warning.GetComponent<Text>().text = error.Value;
            return;
        }

        warning.GetComponent<Text>().text = error.Value;

        Heading.GetComponent<InputField>().text = "";
        Content.GetComponent<InputField>().text = "";
    }


    // Start is called before the first frame update
    void Start()
    {
        discussion = Discussion.getDiscussion();
        cohort = Cohort.getCohort();
        Content.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline;
        Heading.GetComponent<InputField>().text = "";
        Content.GetComponent<InputField>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (Heading.GetComponent<InputField>().isFocused)
            {
                Content.GetComponent<InputField>().Select();
            }

        }
        heading = Heading.GetComponent<InputField>().text;
        content = Content.GetComponent<InputField>().text;
    }
}
