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
    //UI element
    public GameObject scrollView;
    public GameObject heading;

    //UI alignment
    float scrollWidth;

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
        //UI alignment

        //the width of scroll view. This is used to control the size of user entry.
        RectTransform rt = scrollView.GetComponent<RectTransform>();
        scrollWidth = rt.rect.width;

        // set heading alignment
        RectTransform headingRT = heading.GetComponent<RectTransform>();
        headingRT.sizeDelta = new Vector2(scrollWidth, headingRT.rect.height);
        headingRT.position = new Vector3(rt.position.x, headingRT.position.y, headingRT.position.z);
        for (int i = 0; i < heading.transform.childCount; i++)
        {
            GameObject child = heading.transform.GetChild(i).gameObject;
            RectTransform childRT = child.GetComponent<RectTransform>();

            childRT.sizeDelta = new Vector2(scrollWidth / heading.transform.childCount, childRT.rect.height);
        }

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

            //UI alignemnt
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                RectTransform childRT = child.GetComponent<RectTransform>();

                childRT.sizeDelta = new Vector2(scrollWidth / go.transform.childCount, childRT.rect.height);
            }
            go.transform.localScale = new Vector3(1f, 1f, 1f);

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
