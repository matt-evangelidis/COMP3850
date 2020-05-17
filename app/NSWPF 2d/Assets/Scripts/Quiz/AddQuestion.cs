using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AddQuestion : MonoBehaviour
{
    public GameObject question;
    public GameObject correctAnswer;
    public GameObject warning;
    public GameObject[] wrongAnswers = new GameObject[3];

    private string questionTxt;
    private string CorrectAnswer;
    private string[] WrongAnswers = new string[3];

    // Singleton 
    MultipleChoice mc;

    // Start is called before the first frame update
    void Start()
    {
        mc = MultipleChoice.getMultipleChoice();

        question.GetComponent<InputField>().text = "";
        correctAnswer.GetComponent<InputField>().text = "";
        for (int i = 0; i < wrongAnswers.Length; i++)
        {
            wrongAnswers[i].GetComponent<InputField>().text = "";
        }
    }

    public void back() 
    {
        if (Login.globalRole == 1) //supervisor
        {
            SceneManager.LoadScene("Module Searching Supervisor");
        }
    }

    public void addQuestion()
    {
        List<Answer> answers = new List<Answer>();

        Answer correct = new Answer(CorrectAnswer, true);
        answers.Add(correct);

        for (int i = 0; i < WrongAnswers.Length; i++) 
        {
            Answer wrong = new Answer(WrongAnswers[i], false);
            answers.Add(wrong);
        }

        Question ques = new Question(questionTxt,answers);

        KeyValuePair<int, string> error = mc.addQuestion(ques);

        warning.GetComponent<Text>().text = error.Value;
        if (error.Key == 1) 
        {
            return;
        }

        //if no error then done
        question.GetComponent<InputField>().text = "";
        correctAnswer.GetComponent<InputField>().text = "";
        for (int i = 0; i < wrongAnswers.Length; i++)
        {
            wrongAnswers[i].GetComponent<InputField>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (question.GetComponent<InputField>().isFocused)
            {
                correctAnswer.GetComponent<InputField>().Select();
            }
            else if (correctAnswer.GetComponent<InputField>().isFocused)
            {
                wrongAnswers[0].GetComponent<InputField>().Select();
            }
            else if (wrongAnswers[0].GetComponent<InputField>().isFocused)
            {
                wrongAnswers[1].GetComponent<InputField>().Select();
            }
            else if (wrongAnswers[1].GetComponent<InputField>().isFocused)
            {
                wrongAnswers[2].GetComponent<InputField>().Select();
            }

        }

        questionTxt = question.GetComponent<InputField>().text;
        CorrectAnswer = correctAnswer.GetComponent<InputField>().text;
        for (int i = 0; i < wrongAnswers.Length; i++)
        {
            WrongAnswers[i] = wrongAnswers[i].GetComponent<InputField>().text;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            addQuestion();
        }
    }

}
