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
    List<Question> records;
    public Button next;
    public Button back;
    public GameObject page;
    public Button submit;
    public GameObject result;

    //counting variables
    int count;
    Answer current; //the selected Button on the page
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
                int len = Int32.Parse(temp[temp.Length - 1]) - 1;
                a[len].correct = true;

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
        //quiz setup
        List<Question> t = new List<Question>();
        global = new Quiz("searching", t);
        build_questions(global.questions);

        //chosen answer setup
        records = new List<Question>();

        //button setup
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
        submit.interactable = false;

        result.GetComponent<Text>().text = "";

        //update display
        UpdatePage();

        //display first question and answer text
        count = 0;
        UpdateQA();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        try
        {
            if (current.text != "")
            {
                //record answer
                UpdateRecord();

                //increase count
                Increment();

                //update display
                UpdateQA();
                UpdatePage();
            }
        }
        catch (NullReferenceException)
        {   
            warning.GetComponent<Text>().text = "Please select an answer before pressing next";
        }
    }

    public void Back()
    {
        //decrease count
        Decrement();

        if (count < 0)
        {
            count = 0;
        }
        if (count > 0)
        { 
            UpdateRecord();
        }

        UpdateQA();
        UpdatePage();
    }

    public void UpdateQA()
    {
        warning.GetComponent<Text>().text = "";
        questiontext.GetComponent<Text>().text = global.questions[count].question;
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = global.questions[count].answers[i].text;
            //Debug.Log("break");
        }
    }

    public void Increment()
    {
        count++;
    }
    public void Decrement()
    {
        count--;
    }

    public void UpdatePage()
    {
        int c1 = count + 1;
        string temp = c1.ToString();
        page.GetComponent<Text>().text = temp + "/" + global.questions.Count.ToString();
        //Debug.Log(global.questions.Count);

        // next button active
        if (count == global.questions.Count - 1)
        {
            next.gameObject.SetActive(false);
            submit.interactable = true;
        }
        if (count < global.questions.Count - 1)
        {
            submit.interactable = false;
            next.gameObject.SetActive(true);
        }

        //back button active
        
    }

    public void UpdateRecord()
    {

        if (count == 0 && records.Count < 1)//check if new record
        {
            records.Add(new Question(global.questions[count].question, new List<Answer>()));//add current question text, empty Answer List
            records[count].answers.Add(current);//add current to empty Answer List
        }
        else if (records.Count > 0)
        {
            if (count > 0)
            { 
                if (!(records[count - 1].question.Equals(global.questions[count].question)))
                {
                    records.Add(new Question(global.questions[count].question, new List<Answer>()));
                    records[count].answers.Add(current);
                    Debug.Log("working " + global.questions[count].question);
                }
            }
        }

/*        Debug.Log(records.Count);
        foreach (Question q in records)
        {
            Debug.Log(q.question);
            foreach (Answer a in q.answers)
            {
                Debug.Log(a.text);
            }
        }*/
    }

    public void Submit()
    {
        int sum = 0;
        foreach (Question q in records)
        {
            foreach (Answer a in q.answers)
            {
                if (a.correct)
                {
                    sum++;
                }
            }
        }
        if (current.correct)
        {
            sum++;
        }
        result.GetComponent<Text>().text = sum.ToString() + " out of " + global.questions.Count;
        Debug.Log("Total = " + sum.ToString());
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