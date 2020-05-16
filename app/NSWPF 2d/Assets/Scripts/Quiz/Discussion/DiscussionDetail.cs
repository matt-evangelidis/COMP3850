using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class DiscussionDetail : MonoBehaviour
{
    public GameObject detail;
    public GameObject Reply;
    public GameObject dividerContent;
    public GameObject dividerReply;
    public GameObject warning;
    public GameObject content;

    public static Thread post = null;
    private List<Reply> replies = null;

    public void back() {
        SceneManager.LoadScene("Searching Discussion");
    }

    public void createReply() {
        SceneManager.LoadScene("Discussion Reply");
    }


    // Start is called before the first frame update
    void Start()
    {
        post = SearchingDiscussion.postDetail;
        if (post == null) {
            warning.GetComponent<Text>().text = "no post found";
            return;
        }
        
        detail.GetComponent<Text>().text = post.username + " (" + post.role + ") " + ": " + post.content;

        replies = post.replies;

        foreach (Reply reply in replies)
        {
            GameObject go = (GameObject)Instantiate(Reply);
            go.transform.SetParent(content.transform);
            go.GetComponent<Text>().text = reply.username + " (" +reply.role+ ") : " + reply.content;

            GameObject divider = (GameObject)Instantiate(dividerReply);
            divider.transform.SetParent(content.transform);
        }


        Reply.SetActive(false);
        dividerReply.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
