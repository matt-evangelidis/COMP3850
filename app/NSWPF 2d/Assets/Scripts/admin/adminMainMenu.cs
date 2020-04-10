using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;
using UnityEngine.SceneManagement;

public class AdminMainMenu : MonoBehaviour
{
    public GameObject adminBtn;
    public GameObject supervisorBtn;
    public GameObject learnerBtn;

    public void toAdminManagement() {
        SceneManager.LoadScene("Admin Management");
    }
    public void toSupervisorManagement()
    {
        SceneManager.LoadScene("Supervisor Management");
    }

    public void toLearnerManagement()
    {
        SceneManager.LoadScene("Learner Management");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
