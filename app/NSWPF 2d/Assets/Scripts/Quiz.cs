using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Quiz : MonoBehaviour
{
    //gameobject variables
    public GameObject build;
    public GameObject[] buttons;
    public GameObject questiontext;
    public GameObject warning;
    Quiz global;
    public Button next;
    public Button back;

    //counting variables
    int count;
    Answer current;
    public int total;
    
    //class variables
    private string _title;
    private List<Question> _questions = new List<Question>();
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
            string[] lines = System.IO.File.ReadAllLines(@"database/quiz/searching.txt");

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
                int len = Int32.Parse(temp[temp.Length-1]);
                //len = Int32.Parse(temp[len]);
                a[len-1].correct = true;

                //build question
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
        //button setup
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);

        //quiz setup
        List<Question> t = new List<Question>();
        global = new Quiz("searching", t);
        build_questions(global.questions);

        //display first question and answer text
        count = 0;
        questiontext.GetComponent<Text>().text = global.questions[count].question;
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = global.questions[count].answers[i].text;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        if (current.text != "")
        {
            if (current.correct)
            {
                total++;
            }

            count++;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponentInChildren<Text>().text = global.questions[count].answers[i].text;
            }
            questiontext.GetComponent<Text>().text = global.questions[count].question;
        }
        else
        {
            warning.GetComponent<Text>().text = "Please select an answer before pressing next";
        }
    }

    public void Back()
    {
        count--;
        total--;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = global.questions[count].answers[i].text;
        }
        questiontext.GetComponent<Text>().text = global.questions[count].question;
    }

    public void Select(GameObject b, Answer a) // question buttons
    {
        //Debug.Log(b.ToString() + " selected: " + current.text);
        current = a;
        Debug.Log(b.ToString() + " selected: " + current.text);
        Debug.Log(a.correct.ToString());
    }

    public void Select1()
    {
        Select(buttons[0], global.questions[count].answers[0]);
    }

    public void Select2()
    {
        Select(buttons[1], global.questions[count].answers[1]);
    }

    public void Select3()
    {
        Select(buttons[2], global.questions[count].answers[2]);
    }

    public void Select4()
    {
        Select(buttons[3], global.questions[count].answers[3]);
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