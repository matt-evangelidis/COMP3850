using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quiz2Handler : MonoBehaviour
{
    //gameObjects
    public GameObject[] boxes;
    public Text textOutput;
    public Button submitButton;
    public Button finishButton;
    public GameObject ellipse;

    //internal variables
    private bool complete = false;
    private int sum;

    // Start is called before the first frame update
    void Start()
    {
        textOutput.text = "";
        finishButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //circle placement
        PlaceEllipse();

        //check if all boxes have been found
        for (int i = 0; i < boxes.Length; i++)
        {
            if(!boxes[i].GetComponent<DisplayScript>().getActive())
            {
                break;
            }
            complete = true;
        }

        //update text field with current results
        sum = 0;
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i].GetComponent<DisplayScript>().getActive())
            {
                sum++;
            }
        }
        //textOutput.text = sum + "/" + boxes.Length;

        //quiz completion, disable script
        if (complete)
        {
            print("complete");
            enabled = false;
        }

    }

    public void PlaceEllipse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(ellipse, worldPosition, Quaternion.identity);
        }
    }

    public void ButtonClick()
    {
        textOutput.text = sum + "/" + boxes.Length;
        finishButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        complete = true;
    }

    public void FinishClick()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
