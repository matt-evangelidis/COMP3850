using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizHandler : MonoBehaviour
{
    public GameObject[] boxes;
    public Text textOutput;
    private bool complete = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            if(!boxes[i].GetComponent<DisplayScript>().getActive())
            {
                break;
            }
            complete = true;
        }

        int sum = 0;
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i].GetComponent<DisplayScript>().getActive())
            {
                sum++;
            }
        }
        textOutput.text = sum + "/" + boxes.Length;

        if (complete)
        {
            print("complete");
            enabled = false;
        }
    }
}
