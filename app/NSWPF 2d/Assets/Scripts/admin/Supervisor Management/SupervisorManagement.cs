using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SupervisorManagement : MonoBehaviour
{
    public void backtoAdminMenu()
    {
        SceneManager.LoadScene("Admin Menu");
    }

    public void toSupervisorInfo()
    {
        SceneManager.LoadScene("Supervisor Info");
    }
    public void toCreateSupervisor()
    {
        SceneManager.LoadScene("Create Supervisor");
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
