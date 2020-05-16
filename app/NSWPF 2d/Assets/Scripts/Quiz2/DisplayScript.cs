using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScript : MonoBehaviour
{
    //public variables
    public string message;
    public GameObject messageBox;
    public GameObject panel;

    //internal variables
    private SpriteRenderer temp;
    private Color c;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        //variable setup
        temp = gameObject.GetComponent<SpriteRenderer>();
        c = temp.color;
        messageBox.GetComponent<Text>().text = message;
        messageBox.SetActive(false);

        //set alpha to 0, so it can be visible in the editor
        temp.color = new Color(c.r, c.g, c.b, 0f);

        //set alpha to 0 in background panel
        Color t1 = panel.GetComponent<Image>().color;
        t1 = new Color(t1.r, t1.g, t1.b, 0f);
        panel.GetComponent<Image>().color = t1;
    }

    void OnMouseDown()
    {
        //if not active
        if (!active)
        {
            //make the shape visible when clicked within its collider
            temp.color = new Color(c.r, c.g, c.b, 1f);
            //set to active
            active = true;

            //display message within its assigned box
            messageBox.SetActive(true);

            //set alpha to 1
            Color t1 = panel.GetComponent<Image>().color;
            t1 = new Color(t1.r, t1.g, t1.b, 1f);
            panel.GetComponent<Image>().color = t1;
        }
    }

    //return whether the individual object has been activated
    public bool getActive()
    {
        return active;
    }

    public string getMessage()
    {
        return message;
    }
}