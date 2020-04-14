using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterSupervisorSuccess : MonoBehaviour
{
    public void backToSupervisorManagement()
    {
        SceneManager.LoadScene("Supervisor Management");
    }
    public void BackToRegister()
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
