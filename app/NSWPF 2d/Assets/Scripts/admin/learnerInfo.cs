using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using TMPro;

public class LearnerInfo : MonoBehaviour
{
    public GameObject userEntry;

    public void backToLearnerManagement() {
        SceneManager.LoadScene("Learner Management");
    }

    // Start is called before the first frame update
    void Start()
    {

        Cohort cohort = new Cohort();
        foreach (Learner learner in cohort.learners)
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(this.transform);
            go.transform.Find("Username").GetComponentInChildren<InputField>().text = learner.username;
            go.transform.Find("Firstname").GetComponent<InputField>().text = learner.firstname;
            go.transform.Find("Lastname").GetComponent<InputField>().text = learner.lastname;
            go.transform.Find("Email").GetComponent<InputField>().text = learner.email;
        }
        
        //Destroy(userEntry);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
