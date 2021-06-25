using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AnswerManager : MonoBehaviour, IPointerDownHandler
{
    public string answer;

    public void OnPointerDown(PointerEventData eventData)
    {
        QuestionManager.isAnswer = true;
        UIManager.instance.CleanQuestion();
        UIManager.instance.HideQuestionPanel();
        bool flag = QuestionManager.instance.Answer(answer);
        if (flag)
        {
            Debug.Log("正确");
        }
        else if (!flag)
        {
            Debug.Log("错误");
        }
    }
}
