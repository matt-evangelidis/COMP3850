using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
public class learnerInfo : MonoBehaviour
{
    public GameObject userEntry;

    // Start is called before the first frame update
    void Start()
    {

        Cohort cohort = new Cohort();
        foreach (Learner learner in cohort.learners)
        {
            GameObject go = (GameObject)Instantiate(userEntry);
            go.transform.SetParent(this.transform);
            go.transform.Find("Username").GetComponent<InputField>().text = learner.username;
            go.transform.Find("Fullname").GetComponent<InputField>().text = learner.firstname + " " + learner.lastname;
            //go.transform.Find("Lastname").GetComponent<InputField>().text = learner.lastname;
            go.transform.Find("Email").GetComponent<InputField>().text = learner.email;
        }
        
            Destroy(userEntry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
