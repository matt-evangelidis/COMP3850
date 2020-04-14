using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    public GameObject searching;
    public GameObject account;

    public void toAccount() {
        SceneManager.LoadScene("Account");
    }

    public virtual void toSearching() {
        SceneManager.LoadScene("Module Searching");
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
