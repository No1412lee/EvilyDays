using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class ToolData
{
    public string id;
    public Sprite ico;
    public Buff buff;
    //public TurretGrade grade;
    public List<int> effectTurretID;
}

[System.Serializable]
public class FoodData
{
    public FoodType foodType;
    public string id;
    //public TurretGrade grade;
    public Sprite ico;
    public int recover;
}

[System.Serializable]
public enum FoodType
{
    hungerRecover,
    hpRecover,
    iqRecover,
}

[System.Serializable]
public class StuffData
{
    public string id;
    public Sprite ico;
}

[System.Serializable]
public class QuestData
{
    public string id;
    public Sprite ico;
}

[SerializeField]
public class ToolDataOwn
{
    public int count = 0;
    public int num;
    public Text countText;
    public GameObject UI;
    public ToolData tool;
}

[SerializeField]
public class FoodDataOwn
{
    public int count = 0;
    public int num;
    public Text countText;
    public GameObject UI;
    public FoodData food;
}

[SerializeField]
public class StuffDataOwn
{
    public int count = 0;
    public int num;
    public Text countText;
    public GameObject UI;
    public StuffData stuff;
}

[SerializeField]
public class QuestDataOwn
{
    public int count = 0;
    public int num;
    public Text countText;
    public GameObject UI;
    public QuestData quest;
}

//  _______________________________________________________________________________________________________________________

[System.Serializable]
public class QuestionData
{
    public string question;
    public string answerA;
    public string answerB;
    public string answerC;
    public string answerD;
    public string rightChose;
    public string itemId;
}

[System.Serializable]
public class BossData
{
    [System.Serializable]
    public class Need
    {
        public string questId;
        public int needCount;
    }
    public List<Need> needs;
    public GameObject bossPrefabs;
}

[System.Serializable]
public class GoodData
{
        public string itemId;
        public float prob;//概率
        public int price;
}
  

