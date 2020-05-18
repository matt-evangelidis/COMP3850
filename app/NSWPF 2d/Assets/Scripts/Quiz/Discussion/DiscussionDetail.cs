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
    public GameObject thread_heading;
    public GameObject thread_nameRole;
    public GameObject thread_content;

    public GameObject entry;
    public GameObject warning;
    public GameObject reply;

    public Discussion discussion;

    public static Thread post = null;
    private List<Reply> replies = null;

    public void back()
    {
        SceneManager.LoadScene("Searching Discussion");
    }

    public void createReply()
    {
        SceneManager.LoadScene("Discussion Reply");
    }


    // Start is called before the first frame update
    void Start()
    {
        discussion = Discussion.getDiscussion();

        if (SearchingDiscussion.postDetail == null)
        {
            warning.GetComponent<Text>().text = "no post found";
            return;
        }

        post = discussion.getThread(SearchingDiscussion.postDetail.name);
        if (post == null)
        {
            warning.GetComponent<Text>().text = "no post found";
            return;
        }

        // Display thread
        thread_heading.GetComponent<Text>().text = post.heading;
        thread_nameRole.GetComponent<Text>().text = post.username + " - " + post.role;
        thread_content.GetComponent<Text>().text = post.content;

        replies = post.replies;
        
        foreach (Reply re in replies)
        {
            GameObject go = (GameObject)Instantiate(entry);
            go.transform.SetParent(reply.transform);

            go.transform.Find("Header").Find("Heading").GetComponentInChildren<Text>().text = "Re: " + post.heading;
            go.transform.Find("Header").Find("name-role").GetComponentInChildren<Text>().text = re.username + " - "+ re.role;
            go.transform.Find("Content").GetComponentInChildren<Text>().text = re.content;

            go.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        entry.SetActive(false);

        if (replies.Count == 0)
        {
            reply.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
