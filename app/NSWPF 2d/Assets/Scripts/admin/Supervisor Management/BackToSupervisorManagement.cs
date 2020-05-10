using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSupervisorManagement : BackToLearnerManagement
{
    public void backToSupervisorManagement()
    {
            SceneManager.LoadScene("Supervisor Management");
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
