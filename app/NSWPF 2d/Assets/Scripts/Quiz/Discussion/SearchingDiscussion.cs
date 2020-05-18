using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SearchingDiscussion : MonoBehaviour
{
    public GameObject userEntry;
    public GameObject warning;
    public GameObject content;

    public static Thread postDetail; // use for the next scene. The next scene load detail from this object
    public bool seeDetail;

    private List<GameObject> entries;

    //Discussion object
    Discussion discussion;

    // Back button
    public void backToModule()
    {
        if (Login.globalRole==2)
        {
            SceneManager.LoadScene("Module Searching");
        }
        else if (Login.globalRole==1)
        {
            SceneManager.LoadScene("Module Searching Supervisor");
        }
    }

    // Create a new discussion
    public void toCreateDiscussion() {
        SceneManager.LoadScene("Searching Discussion Create New");
    }


    // Start is called before the first frame update
    void Start()
    {
        discussion = Discussion.getDiscussion();
        Debug.Log("number of discussion: " + discussion.threads.Count);
        seeDetail = false;
        entries = new List<GameObject>();
        postDetail = null;
        foreach (Thread post in discussion.threads) {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(content.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = post.username;
            go.transform.Find("Heading").GetComponentInChildren<InputField>().text = post.heading;
            go.transform.Find("nReplies").GetComponentInChildren<InputField>().text = post.noReplies.ToString();
            go.transform.Find("Detail").GetComponent<Button>().onClick.AddListener(() => {
                postDetail = post;
                Debug.Log(postDetail.heading);
                seeDetail = true;
            });
            go.transform.localScale = new Vector3(1f,1f,1f);
            entries.Add(go);
        }
        userEntry.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (seeDetail == false) {
            postDetail = null;
            return;
        }
        
        if (seeDetail == true) {
            SceneManager.LoadScene("Searching Discussion Detail");
            seeDetail = false;
        }
        
    }
}
