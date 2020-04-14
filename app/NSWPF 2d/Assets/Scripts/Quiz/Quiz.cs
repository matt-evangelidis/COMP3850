using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;

public class Quiz : MonoBehaviour
{
    // variable to control the flow of game:
    private int currentIndex;

    // variable to save achievement 
    private string filePath = "database/leaderboard/quiz/";

    // variable for result
    public static int totalQuestion;
    public static int noCorrects;

    //gameobject variables
    public GameObject[] buttons;
    public GameObject questiontext;
    public GameObject warning;

    // interaction variable
    public GameObject page;
    public Button next;
    public Button back;
    public Button submit;

    // variable to remember the selection
    private Answer selectedAnswer = null;
    private GameObject selectedButton;
    private List<Record> records;

    // List of question
    private List<Question> questions = new List<Question>();

    public void build_questions() 
    //----------------------------------------------------------
    // Get the txt file and build question bank from txt content
    //----------------------------------------------------------
    {
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
                questions.Add(te);
            }
        }
        else 
        {
            warning.GetComponent<Text>().text = "Internal Error, please contact admin";
            Debug.LogWarning("No searching.txt");
        }
    }

    private void showQA()
    //----------------------------------------------------------
    // Load the question and 4 answer for each question in list
    //----------------------------------------------------------
    {
        // warning msg
        warning.GetComponent<Text>().text = "";

        // question display
        questiontext.GetComponent<Text>().text = questions[currentIndex].question;

        //answer chosen display
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = questions[currentIndex].answers[i].text;
        }
    }

    private void sceneControl()
    //----------------------------------------------------------
    // Control the page number and visibility of buttons
    //----------------------------------------------------------
    {
        // show question number
        int pageNo = currentIndex + 1;
        page.GetComponent<Text>().text = "Question " + pageNo.ToString() + "/" + questions.Count.ToString();

        // Show next, nack and submit button
        if (currentIndex >= questions.Count - 1) //last question
        {
            next.gameObject.SetActive(false);
            submit.gameObject.SetActive(true);
            back.gameObject.SetActive(true);
        }
        if (currentIndex < questions.Count - 1) //not the last question
        {
            next.gameObject.SetActive(true);
            submit.gameObject.SetActive(false);
            back.gameObject.SetActive(true);
        }
        if (currentIndex <= 0) 
        {
            back.gameObject.SetActive(false);
            next.gameObject.SetActive(true);
            submit.gameObject.SetActive(false);
        }

        // display the selected answer;
        if (selectedAnswer != null) {
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }
    }

    private void updateRecord()
    //---------------------------------------------------------------
    // keep track with the selected answer
    //---------------------------------------------------------------
    {
        if (records.Count < currentIndex+1)
        {
            //Record record = new Record(questions[currentIndex],selectedAnswer);
            Record record = new Record(questions[currentIndex], selectedAnswer, selectedButton);
            records.Add(record);
        }
        else if (records.Count >= currentIndex+1)
        {
            records[currentIndex].answer = selectedAnswer;
        }
    }

    public void increment()
    //---------------------------------------------------------------
    // got to next question, then increase count to keep up the index
    //---------------------------------------------------------------
    {
        currentIndex++;
    }
    public void decrement()
    //---------------------------------------------------------------
    // got to back question, then decrease count to keep up the index
    //---------------------------------------------------------------
    {
        currentIndex--;
    }

    private void nextQuestion()
    //----------------------------------------------------------
    // When next button is clicked, show next question
    //----------------------------------------------------------
    {
        print(selectedAnswer);
        if (selectedAnswer == null)
        {
            warning.GetComponent<Text>().text = "Please choose your answer before continue";
            return;
        }

        // update the index
        increment();

        if (records.Count < currentIndex+1)
        {
            selectedAnswer = null;
            selectedButton = null;
        }
        else
        {
            selectedAnswer = records[currentIndex].answer;
            selectedButton = records[currentIndex].selectedButton;
        }

        // get new question
        showQA();

        //update scene
        sceneControl();
    }

    private void previousQuestion()
    //----------------------------------------------------------
    // When next button is clicked, show next question
    //----------------------------------------------------------
    {
        // update the index
        decrement();

        if (records.Count < currentIndex + 1)
        {
            selectedAnswer = null;
            selectedButton = null;
        }
        else
        {
            selectedAnswer = records[currentIndex].answer;
            selectedButton = records[currentIndex].selectedButton;
        }

        // get new question
        showQA();

        //update scene
        sceneControl();
    }

    private void select(GameObject selectedButton, Answer a)
    //----------------------------------------------------------
    // select an answer
    //----------------------------------------------------------
    {
        selectedAnswer = a;
        this.selectedButton = selectedButton;
        updateRecord();
    }

    public void Select1()
    {
        select(buttons[0],questions[currentIndex].answers[0]);
    }

    public void Select2()
    {
        select(buttons[1],questions[currentIndex].answers[1]);
    }

    public void Select3()
    {
        select(buttons[2],questions[currentIndex].answers[2]);
    }

    public void Select4()
    {
        select(buttons[3],questions[currentIndex].answers[3]);
    }

    private void saveAchievement()
    //-------------------------------------------------------------------
    // Save result to file 
    //-------------------------------------------------------------------
    {
        string finalResult = noCorrects.ToString() + "," + totalQuestion.ToString() + ";"; ;

        if (!System.IO.Directory.Exists(@filePath))
        {
            System.IO.Directory.CreateDirectory(@filePath);
        }

        if (!System.IO.File.Exists(@filePath + Login.globalUsername + ".txt"))
        {
            System.IO.File.WriteAllText(@filePath + Login.globalUsername + ".txt", finalResult);
        }
        else
        {
            File.AppendAllText(@filePath+ Login.globalUsername + ".txt", finalResult);
        }
    }

    public void finish()
    //-------------------------------------------------------------------
    // when submit, calculate the correct points and send to result scene
    //-------------------------------------------------------------------
    {
        if (selectedAnswer == null) 
        {
            warning.GetComponent<Text>().text = "Please finish you answer before submiting";
            return;
        }
        int sum = 0;
        foreach (Record record in records)
        {
            if (record.answer.correct == true) 
            {
                sum++;
            }
        }
        totalQuestion = questions.Count;
        noCorrects = sum;
        SceneManager.LoadScene("Quiz Result");
        saveAchievement();

    }

    // Start is called before the first frame update
    void Start()
    {
        build_questions();
        records = new List<Record>();
        currentIndex = 0;
        //button setup
        next.onClick.AddListener(nextQuestion);
        back.onClick.AddListener(previousQuestion);
        submit.onClick.AddListener(finish);

        sceneControl();
        showQA();
    }

    private void Update()
    {
        if (selectedAnswer != null)
        {
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }
        else 
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

public class Record
{
    private Question _question;
    private Answer _answer;
    private GameObject _selectedButton;
    public Question question { 
        get { return _question; }
        set { _question = value; } 
    }
    public Answer answer {
        get { return _answer; }
        set { _answer = value; }
    }

    public GameObject selectedButton
    {
        get { return _selectedButton; }
        set { _selectedButton = value; }
    }

    public Record(Question question, Answer answer, GameObject selectedButton) {
        this.question = question;
        this.answer = answer;
        this.selectedButton = selectedButton;
    }

    public Record(Question question) {
        this.question = question;
        this.answer = null;
        selectedButton = null;
    }
}