using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScript : MonoBehaviour
{
    private GameObject self;
    private SpriteRenderer temp;
    private Color c;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        //variable setup
        self = gameObject;
        temp = self.GetComponent<SpriteRenderer>();
        c = temp.color;

        //set alpha to zero, so it can be visible in the editor
        temp.color = new Color(c.r, c.g, c.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*        if (active)
                {
                    temp.color = new Color(c.r, c.g, c.b, 1f);
                }*/
    }
    void OnMouseDown()
    {
        //make the shape visible when clicked within its collider

        temp.color = new Color(c.r, c.g, c.b, 1f);
        //active = true;
    }
}