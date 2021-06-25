using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {

    public List<QuestionData> questions;

    public static QuestionManager instance;

    public QuestData answerSheet;
    private int num;

    public static bool isAnswer = true;
    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    public void Question()
    {
        if (isAnswer)
        {
            if (ToolsManager.instance.FindItem(answerSheet) == -1)
                return;
            ToolsManager.instance.UseItem(answerSheet,1);
            QuestionData temp = ChoseQs();
            UIManager.instance.GetQuestion(temp);
            isAnswer = false;
        }
        UIManager.instance.ShowQuestionPanel();
    }

    public QuestionData ChoseQs()
    {
        float ran = UnityEngine.Random.Range(0, questions.Count);
        num = (int) Math.Floor(ran);
        return questions[num];
    }

    public bool Answer (string str)
    {
        isAnswer = true;
        if (str == questions[num].rightChose)
        {
            ToolsManager.instance.GetItem(questions[num].itemId, 1);
            float luck = UnityEngine.Random.value;
            if(luck >= 0.95)
                ToolsManager.instance.GetItem(questions[num].itemId, 1);
            return true;  
        }
        return false;
    }
	
    public void Right()
    {
        //:todo
    }

    public void False()
    {
        //:todo
    }

	// Update is called once per frame
}
