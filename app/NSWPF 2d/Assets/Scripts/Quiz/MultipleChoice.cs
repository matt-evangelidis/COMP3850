using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using System.Linq;

public class MultipleChoice 
{

    string path = "database/quiz/searching.txt";

    public List<Question> questions;

    //singleton implementation
    private static MultipleChoice instance = null;
    public static MultipleChoice getMultipleChoice()
    //----------------------------------------------------------
    //----------------------------------------------------------
    {
        if (instance == null)
        {
            instance = new MultipleChoice();
        }
        return instance;
    }

    public KeyValuePair<int, string> addQuestion(Question question) 
    //----------------------------------------------------------
    // Add question to the question list
    //----------------------------------------------------------
    {
        int errorCode = 0;
        string error_message = "";
        KeyValuePair<int, string> error_return;

        if (question.question == "") 
        {
            errorCode = 1;
            error_message = "ERROR: Question field is empty!";
            error_return = new KeyValuePair<int, string>(errorCode, error_message);
            return error_return;
        }

        foreach (Answer a in question.answers) {
            if (a.text == "") 
            {
                errorCode = 1;
                error_message = "ERROR: Answer " + (question.answers.IndexOf(a)+1) + " field is empty!";
                error_return = new KeyValuePair<int, string>(errorCode, error_message);
                return error_return;
            }
        }

        //no error:
        this.questions.Add(question);

        return error_return;
    }

    public static void ShuffleList<T>(IList<T> objList)
    //--------------------------------------------------
    // Shuffle list
    //--------------------------------------------------
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

    private MultipleChoice()
    //----------------------------------------------------------
    //----------------------------------------------------------
    {
        if (!System.IO.File.Exists(@path)) 
        {
            return;
        }

        questions = new List<Question>();

        // get all the question:
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

            //shuffle answer
            ShuffleList<Answer>(a);

            //build question
            Question te = new Question(temp[0], a);
            questions.Add(te);
        }
    }
}

