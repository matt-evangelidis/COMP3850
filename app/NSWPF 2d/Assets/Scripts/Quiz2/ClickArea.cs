using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickArea : MonoBehaviour
{
    //script attached to pref
    private GameObject self;
    private float alpha = 1f;
    SpriteRenderer temp;

    // Start is called before the first frame update
    void Start()
    {
        self = gameObject;
        temp = self.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //fade alpha to zero
        temp.color = new Color(1, 1, 1, alpha);
        alpha = alpha - 0.3f * Time.deltaTime;

        //destroy instance
        if (alpha <= 0)
        {
            Destroy(self);
        }
    }
}