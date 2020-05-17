using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLearnerManagement : MonoBehaviour
{
    public void backToLearnerManagement() {
        if (Login.globalRole==0)
            SceneManager.LoadScene("Learner Management");
        else if (Login.globalRole==1)
            SceneManager.LoadScene("Supervisor Menu"); // by Lin: extended to check roles
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
