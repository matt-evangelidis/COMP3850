using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public GameObject build;
    public GameObject[] buttons;
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
        //external check
        if (System.IO.File.Exists(@"database/quiz/searching.txt"))
        {
            //array for reading
            String[] lines = System.IO.File.ReadAllLines(@"database/quiz/searching.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                //temp string array, delimited by ;, first entry is the question, last entry is the correct answer
                string[] temp = lines[i].Split(';');

                //temp answer list
                List<Answer> a = new List<Answer>();

                //start from first answer, end at last answer
                for (int j = 1; j < temp.Length - 1; j++)
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
    }

    public void print(Quiz quiz)
    {
        Debug.Log(quiz.title.ToString());
        foreach (Question q in quiz.questions)
        {
            Debug.Log(q.question.ToString());
            foreach (Answer a in q.answers)
            {
                Debug.Log(a.text.ToString());
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Question> t = new List<Question>();
        Quiz q = new Quiz("searching", t);
        build_questions(q.questions);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = q.questions[0].answers[i].text;
            //buttons[i].GetComponent<Text>().text = q.questions[0].answers[i].text;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Test()
    {
        List<Question> t = new List<Question>();
        Quiz q = new Quiz("searching", t);
        build_questions(t);
        print(q);
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
    private string _text;
    private bool _correct;
    public string text { get { return _text; } set { _text = value; } }
    public bool correct { get { return _correct; } set { _correct = value; } }

    public Answer(string a, bool c) //constructor
    {
        text = a;
        correct = c;
    }

}