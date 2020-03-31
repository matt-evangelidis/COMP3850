using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WelcomeText : MonoBehaviour
{
    public GameObject welcomeText;
    private string fullName;

    // Start is called before the first frame update
    void Start()
    {
        fullName = Login.fullName;
        welcomeText.GetComponent<TextMeshProUGUI>().text = "Welcome, " + fullName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
