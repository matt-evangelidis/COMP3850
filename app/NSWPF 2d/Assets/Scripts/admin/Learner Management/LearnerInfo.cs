using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class LearnerInfo : MonoBehaviour
{
    //UI element
    public GameObject scrollView;
    public GameObject heading;
    //UI alignment
    float scrollWidth;

    public GameObject userEntry;
    public void backToLearnerManagement() {
        SceneManager.LoadScene("Learner Management");
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


        Cohort cohort = Cohort.getCohort();
        foreach (User learner in cohort.getLearners())
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(this.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = learner.username;
            go.transform.Find("Firstname").GetComponent<InputField>().text = learner.firstname;
            go.transform.Find("Lastname").GetComponent<InputField>().text = learner.lastname;
            go.transform.Find("Email").GetComponent<InputField>().text = learner.email;

            //UI alignemnt
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                RectTransform childRT = child.GetComponent<RectTransform>();

                childRT.sizeDelta = new Vector2(scrollWidth / go.transform.childCount, childRT.rect.height);
            }
            go.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        Destroy(userEntry);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
