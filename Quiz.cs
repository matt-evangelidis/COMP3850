using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    private string _title;
    private List<Question> _questions;
    public string title { get { return _title; } set { _title = value; } }
    public List<Question> questions { get { return _questions; } set { _questions = value; } }
    
    public Quiz(string t, List<Question> q) //constructor
    {
        title = t;
        questions = q;
    }


    public void build_questions(List<Question> q)
    {
        //array for
        String[] lines = System.IO.File.ReadAllLines(@"database/quiz/searching.txt");
        
        for(int i = 0; i < lines.Length; i++)
        {
            //temp string array, delimited by ;, first entry is the question, last entry is the correct answer
            string[] temp = lines[i].Split(';');
            
            //temp answer list
            List<Answer> a = new List<Answer>();
            
            //start from first answer, end at last answer
            for (int j = 1; j < temp.Length-1; j++)
            {
                Answer t = new Answer(temp[j], false);
                a.Add(t);
            }

            //set correct field
            int len = temp.Length - 1;
            len = Int32.Parse(temp[len]);
            a[len].correct = true;
            
            //build question, 
            Question te = new Question(temp[0], a);
            q.Add(te);
        }
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

public class Question : MonoBehaviour
{
    private string _question;
    private List<Answer> _answers;
    public string question { get { return _question; } set { _question = value; } }
    public List<Answer> answers { get { return _answers; } set { _answers = value; } }
    
    public Question(string q, List<Answer> a) //constructor
    {
        question = q;
        answers = a;
    }


}

public class Answer : MonoBehaviour
{
    private string _answer;
    private bool _correct;
    public string answer { get { return _answer; } set { _answer = value; } }
    public bool correct { get { return _correct; } set { _correct = value; } }

    public Answer(string a, bool c) //constructor
    {
        answer = a;
        correct = c;
    }

}