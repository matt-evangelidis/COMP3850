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

    private string filePath = "database/discussion/";

    public void back() {
        SceneManager.LoadScene("Searching Discussion");
    }

    public void upload() {

        if (Login.globalUsername == null) {
            warning.GetComponent<Text>().text = "Internal error. Please contact admin";
            Debug.LogWarning("No username detected");
            return;
        }

        if (Login.globalRole == null)
        {
            warning.GetComponent<Text>().text = "Internal error. Please contact admin";
            Debug.LogWarning("No role detected");
            return;
        }

        if (heading.Equals("")) {
            warning.GetComponent<Text>().text = "Heading should not be empty";
            return;
        }

        if(content.Equals("")) {
            warning.GetComponent<Text>().text = "Content should not be empty";
            return;
        }

        if (content.Contains(";")) {
            warning.GetComponent<Text>().text = "Content should not contaim ';'";
            return;
        }

        if (heading.Contains(";")) {
            warning.GetComponent<Text>().text = "Heading should not contaim ';'";
            return;
        }

        if (!System.IO.Directory.Exists(@filePath))
        {
            System.IO.Directory.CreateDirectory(@filePath);
        }
        
        String now = DateTime.Now.ToString("yyyy-MM-dd,HH-mm-ss");
        String dateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

        form = now + ";" + Login.globalUsername + ";" + Login.globalRole + ";" + heading + ";" + dateTime + ";" + content + "<END>";

        System.IO.File.WriteAllText(@filePath + now + ".txt", form);

        warning.GetComponent<Text>().text = "New discussion has been uploaded";

        Heading.GetComponent<InputField>().text = "";
        Content.GetComponent<InputField>().text = "";
    }


    // Start is called before the first frame update
    void Start()
    {
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
