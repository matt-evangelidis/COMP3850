using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question 
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
    public Answer getCorrectAnswer()
    {
        foreach (Answer answer in answers) {
            if (answer.correct == true)
                return answer;
        }
        return null;
    }

    public Answer getAnswer(int i) {
        if (i >= 0 && i <= 3)
            return answers[i];
        return null;
    }
}


public class Answer
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

    public bool isCorrect() 
    {
        return correct;
    }
}
