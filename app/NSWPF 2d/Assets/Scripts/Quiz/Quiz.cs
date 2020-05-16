using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class Quiz : MonoBehaviour
{
    // variable to control the flow of game:
    private int currentIndex;

    // variable for result
    public static int totalQuestion;
    public static int noCorrects;

    //gameobject variables
    public GameObject[] buttons;
    public GameObject questiontext;
    public GameObject warning;

    // Quiz state:
    bool LEARNER_WORKING = true;

    // interaction variable
    public GameObject page;
    public Button next;
    public Button back;
    public Button submit;

    // variable to remember the selection
    private Answer selectedAnswer = null;
    private GameObject selectedButton;
    public static List<Record> records;

    //Leaderboard object
    public Leaderboard leaderboard;

    //Multiple choice object
    public MultipleChoice mc;

    // List of question
    public static List<Question> questions;
    private int no_questions = 5;

    // shuffle the answer. (NOT A CLEVER WAY)
    public static void ShuffleList<T>(IList<T> objList)
    {
        System.Random rnd = new System.Random();
        int totalItem = objList.Count;
        T obj;
        while (totalItem >= 1)
        {
            totalItem -= 1;
            int nextIndex = rnd.Next(totalItem, objList.Count);
            obj = objList[nextIndex];
            objList[nextIndex] = objList[totalItem];
            objList[totalItem] = obj;
        }
    }

    public void build_questions() 
    //----------------------------------------------------------
    // Get the txt file and build question bank from txt content
    //----------------------------------------------------------
    {
        mc = MultipleChoice.getMultipleChoice();
        if (mc != null)
        {

            if (mc.questions.Count > no_questions) //pick question from pool
            {
                List<int> rndList = new List<int>();
                System.Random rnd = new System.Random();
                for (int i = 0; i < no_questions; i++)
                {
                    int random = rnd.Next(mc.questions.Count);
                    if (!rndList.Contains(random))
                    {
                        rndList.Add(random);
                        continue;
                    }
                    // if the number is duplicated then do this loop again.
                    i--;
                }

                for (int i = 0; i < rndList.Count; i++)
                {
                    Question quesObj = mc.questions[rndList[i]];

                    //temp string array, delimited by ;, first entry is the question, last entry is the correct answer
                    string question = quesObj.question;

                    //temp answer list
                    List<Answer> a = new List<Answer>();

                    //start from first answer, end at last answer
                    foreach (Answer answer in quesObj.answers)
                    {
                        a.Add(answer);
                    }

                    //shuffle answer
                    ShuffleList<Answer>(a);

                    //build question
                    Question te = new Question(question, a);
                    questions.Add(te);
                }
            } 
            else 
            { 

                for (int i = 0; i < mc.questions.Count; i++)
                {
                    Question quesObj = mc.questions[i];

                    //temp string array, delimited by ;, first entry is the question, last entry is the correct answer
                    string question = quesObj.question;

                    //temp answer list
                    List<Answer> a = new List<Answer>();

                    //start from first answer, end at last answer
                    foreach (Answer answer in quesObj.answers)
                    {
                        a.Add(answer);
                    }

                    //shuffle answer
                    ShuffleList<Answer>(a);

                    //build question
                    Question te = new Question(question, a);
                    questions.Add(te);
                }
                ShuffleList<Question>(questions);
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
            if (LEARNER_WORKING == false)
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
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
            if (LEARNER_WORKING == false) {
                submit.gameObject.SetActive(true);
            }
        }
        if (currentIndex <= 0) 
        {
            back.gameObject.SetActive(false);
            next.gameObject.SetActive(true);
            submit.gameObject.SetActive(false);
            if (LEARNER_WORKING == false)
            {
                submit.gameObject.SetActive(true);
            }
        }

        //display feedback
        if (LEARNER_WORKING == false)
        {
            selectedAnswer = records[currentIndex].answer;
            for (int i = 0; i < buttons.Length; i++) {
                ColorBlock colours = buttons[i].GetComponent<Button>().colors;
                colours.disabledColor = new Color32(255,255,255,255);
                colours.selectedColor = new Color32(255,255,255,255);
                buttons[i].GetComponent<Button>().colors = colours;
            }
            if (records[currentIndex].answer.correct == true)
            {
                int rightIndex = records[currentIndex].question.answers.IndexOf(records[currentIndex].answer);
                Button rightButton = buttons[rightIndex].GetComponent<Button>();
                ColorBlock colours = rightButton.colors;
                colours.disabledColor = new Color32(102, 231, 133, 255);
                colours.selectedColor = new Color32(102, 231, 133, 255);
                rightButton.colors = colours;
            }
            else 
            {
                int wrongIndex = records[currentIndex].question.answers.IndexOf(records[currentIndex].answer);
                Button wrongButton = buttons[wrongIndex].GetComponent<Button>();
                ColorBlock wrongColours = wrongButton.colors;
                wrongColours.disabledColor = new Color32(214, 121, 113, 255);
                wrongColours.selectedColor = new Color32(214, 121, 113, 255);
                wrongButton.colors = wrongColours;

                int rightIndex = records[currentIndex].question.answers.IndexOf(records[currentIndex].question.getCorrectAnswer());
                Button rightButton = buttons[rightIndex].GetComponent<Button>();
                ColorBlock rightColours = rightButton.colors;
                rightColours.disabledColor = new Color32(102, 231, 133, 255);
                rightColours.selectedColor = new Color32(102, 231, 133, 255);
                rightButton.colors = rightColours;
            }
        }
        else // if not feebback
        {
            // display the selected answer;
            if (selectedAnswer != null)
            {
                EventSystem.current.SetSelectedGameObject(selectedButton);
            }
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
        if (selectedAnswer == null)
        {
            warning.GetComponent<Text>().text = "Please choose your answer before you continue.";
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
    // Save result to file (database)
    //-------------------------------------------------------------------
    {
        // using singleton
        leaderboard = Leaderboard.getLeaderboard();
        leaderboard.addAchievement(noCorrects, totalQuestion);
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
        if (Login.globalRole==1)
        {
            return;
        }
        if (LEARNER_WORKING == true)
        {
            saveAchievement();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (QuizResult.review == true)
        {
            LEARNER_WORKING = false;
            questions = QuizResult.questions;
            records = QuizResult.records;
            submit.GetComponentInChildren<Text>().text = "Finish Review";
        }
        else 
        {
            LEARNER_WORKING = true;
            questions = new List<Question>();
            build_questions();
            records = new List<Record>();
            submit.GetComponentInChildren<Text>().text = "Submit";
        }
        //button setup
        currentIndex = 0;
        next.onClick.AddListener(nextQuestion);
        back.onClick.AddListener(previousQuestion);
        submit.onClick.AddListener(finish);

        sceneControl();
        showQA();
    }

    private void Update()
    {
        if (LEARNER_WORKING == true)
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
        else if (LEARNER_WORKING == false) 
        {

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
