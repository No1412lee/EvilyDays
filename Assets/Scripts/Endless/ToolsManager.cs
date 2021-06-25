using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToolsManager : MonoBehaviour {


    public GameObject bagPanel;

    public GameObject bagGrid;

    public GameObject itemButton;

    public List<GameObject> itemPrefabs;

    public List<GameObject> foodPrefabs;
//
    public List<ToolData> tools;

    public List<FoodData> foods;

    public List<StuffData> stuffs;

    public List<QuestData> quests;

    public static ToolsManager instance;

    public static List<ToolDataOwn> toolsOwn;

    public static List<FoodDataOwn> foodsOwn;

    public static List<StuffDataOwn> stuffsOwn;

    public static List<QuestDataOwn> questsOwn;

    // type  0: 工具 tool 1:食物 food  2:材料  stuff 3.事件道具 quest
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        toolsOwn = new List<ToolDataOwn>();
        foodsOwn = new List<FoodDataOwn>();
        stuffsOwn = new List<StuffDataOwn>();
        questsOwn = new List<QuestDataOwn>();
        Load();
    }
    public int Num(GameObject Item)
    {
        int sum = 0;
        for(int i = 1;i <= 4;i++)
        {
            sum = sum * 10 + Convert.ToInt32(Item.name.Substring(i, 1));
        }
        return (sum);
    }

    public int Num(string id)
    {
        string temp = id.Substring(1, 4);
        int sum = 0;
        for (int i = 0; i <= 3; i++)
        {
            sum = sum * 10 + Convert.ToInt32(temp.Substring(i, 1));
        }
        return (sum);
    }

    public GameObject InstantiateItem(string id,Vector3 postion,Quaternion angle)
    {
        if(Convert.ToInt32(id.Substring(0,1)) == 1)
        {
            GameObject food= Instantiate(foodPrefabs[0], postion, angle);
            food.name = id;
            return food;
        }
        int rare = Convert.ToInt32(id.Substring(5, 1));
        GameObject item = Instantiate(itemPrefabs[rare], postion,angle);
        item.name = id;
        return item;
    }

    public int FindItem(string id)
    {
        int num = Num(id);
        int type = Convert.ToInt32(id.Substring(0, 1));
        if (type == 0)
        {
            int temp = FindItem(tools[num]);
            if (temp == -1)
                return 0;
            return toolsOwn[temp].count;
        }
        if(type == 1)
        {
            int temp = FindItem(foods[num]);
            if (temp == -1)
                return 0;
            return foodsOwn[temp].count;

        }
        if(type == 2)
        {
            int temp = FindItem(stuffs[num]);
            if (temp == -1)
                return 0;
            return stuffsOwn[temp].count;
        }
        if(type == 3)
        {
            int temp = FindItem(quests[num]);
            if (temp == -1)
                return 0;
            return questsOwn[temp].count;
        }
        return 0;
    }

    public int FindItem(ToolData tool)
    {
        if (toolsOwn.Count == 0)
            return -1;
        for(int i = 0;i< toolsOwn.Count;i++)
        {
            if(toolsOwn[i].num == Num(tool.id))
                return i; 
        }
        return -1;
    }

    public int FindItem(FoodData food)
    {
        if (foodsOwn.Count == 0)
            return -1;
       for(int i = 0;i< foodsOwn.Count;i++)
        {
            if(foodsOwn[i].num == Num(food.id))
                return i;
        }
        return -1;
    }

    public int FindItem(StuffData stuff)
    {
        if (stuffsOwn.Count == 0)
            return -1;
        for (int i = 0; i < stuffsOwn.Count; i++)
        {
            if (stuffsOwn[i].num == Num(stuff.id))
                return i;
        }
        return -1;
    }

    public int FindItem(QuestData quest)
    {
        if (questsOwn.Count == 0)
            return -1;
        for(int i =0;i< questsOwn.Count;i++)
        {
            if(questsOwn[i].num == Num(quest.id))
                return i;
        }
        return -1;
    }

    ToolDataOwn NewItem(ToolData tool)
    {
        int num = Num(tool.id);
        GameObject button = Instantiate(itemButton);
        button.GetComponent<Image>().sprite = tool.ico;
        button.GetComponent<BagButtonManager>().id = tool.id;
        button.transform.parent = bagGrid.transform;
        button.transform.localScale = new Vector3(1, 1, 1);
        button.SetActive(false);
        ToolDataOwn toolTemp = new ToolDataOwn();
        toolTemp.tool= tool;
        toolTemp.num = num;
        toolTemp.countText = button.GetComponentInChildren<Text>();
        toolTemp.UI = button;
        return toolTemp;
    }

    FoodDataOwn NewItem(FoodData food)
    {
        int num = Num(food.id);
        GameObject button = Instantiate(itemButton);
        button.GetComponent<Image>().sprite = food.ico;
        button.GetComponent<BagButtonManager>().id = food.id;
        button.transform.parent = bagGrid.transform;
        button.transform.localScale = new Vector3(1, 1, 1);
        button.SetActive(false);
        FoodDataOwn foodTemp = new FoodDataOwn();
        foodTemp.food = food;
        foodTemp.num = num;
        foodTemp.countText = button.GetComponentInChildren<Text>();
        foodTemp.UI = button;
        return foodTemp;
    }

    StuffDataOwn NewItem(StuffData stuff)
    {
        int num = Num(stuff.id);
        GameObject button = Instantiate(itemButton);
        button.GetComponent<Image>().sprite = stuff.ico;
        button.GetComponent<BagButtonManager>().id = stuff.id;
        button.transform.parent = bagGrid.transform;
        button.transform.localScale = new Vector3(1, 1, 1);
        button.SetActive(false);
        StuffDataOwn stuffTemp = new StuffDataOwn();
        stuffTemp.stuff = stuff;
        stuffTemp.num = num;
        stuffTemp.countText = button.GetComponentInChildren<Text>();
        stuffTemp.UI = button;
        return stuffTemp;
    }

    QuestDataOwn NewItem(QuestData quest)
    {
        int num = Num(quest.id);
        GameObject button = Instantiate(itemButton);
        button.GetComponent<Image>().sprite = quest.ico;
        button.GetComponent<BagButtonManager>().id = quest.id;
        button.transform.parent = bagGrid.transform;
        button.transform.localScale = new Vector3(1, 1, 1);
        button.SetActive(false);
        QuestDataOwn questTemp = new QuestDataOwn();
        questTemp.quest = quest;
        questTemp.num = num;
        questTemp.countText = button.GetComponentInChildren<Text>();
        questTemp.UI = button;
        return questTemp;
    }

    public void GetItem(GameObject Item)
    {
        int type = Convert.ToInt32(Item.name.Substring(0, 1));
        int num = Num(Item);
        if (type == 0)//实用性道具
        {
            GetItem(tools[num]);
            return;
        }
        else if (type == 1) //食物
        {
            GetItem(foods[num]);
            return;
        }
        else if (type == 2)
        {
            GetItem(stuffs[num]);
        }
        else if (type == 3)
        {
            GetItem(quests[num]);
        }
        return;
    }

    public void GetItem(string id, int count)
    {
        int type = id[0] - '0';
        int num = Convert.ToInt32(id.Substring(1, 4));
        if (type == 0)//实用性道具
        {
            GetItem(tools[num], count);
            return;
        }
        else if (type == 1) //食物
        {
            GetItem(foods[num], count);
            return;
        }
        else if (type == 2)
        {
            GetItem(stuffs[num], count);
        }
        else if (type == 3)
        {
            GetItem(quests[num], count);
        }
        return;
    }

    public void GetItem(ToolData tool, int count = 1)
    {
        int num = FindItem(tool);
        if (num == -1)
        {
            ToolDataOwn toolTemp = NewItem(tool);
            toolsOwn.Add(toolTemp);
            num = FindItem(tool);
        }

        toolsOwn[num].count += count;
        toolsOwn[num].countText.text = "" + toolsOwn[num].count;

        if (UIManager.i == 0)
            toolsOwn[toolsOwn.Count - 1].UI.SetActive(true);
        return;
    }

    public void GetItem(FoodData food, int count = 1)
    {
        int num = FindItem(food);
        if( num == -1)
        {
            FoodDataOwn foodTemp = NewItem(food);
            foodsOwn.Add(foodTemp);
            num = FindItem(food);
        }

        foodsOwn[num].count += count;
        foodsOwn[num].countText.text = "" + foodsOwn[num].count;

        if (UIManager.i == 1)
            foodsOwn[foodsOwn.Count - 1].UI.SetActive(true);
        return;
    }

    public void GetItem(StuffData stuff, int count = 1)
    {
        int num = FindItem(stuff);
        if (num == -1)
        {
            StuffDataOwn stuffTemp = NewItem(stuff);
            stuffsOwn.Add(stuffTemp);
            num = FindItem(stuff);
        }

        stuffsOwn[num].count += count;
        stuffsOwn[num].countText.text = "" + stuffsOwn[num].count;

        if (UIManager.i == 2)
            stuffsOwn[stuffsOwn.Count - 1].UI.SetActive(true);
        return;
    }

    public void GetItem(QuestData quest, int count = 1)
    {
        int num = FindItem(quest);
        if(num == -1)
        {
            QuestDataOwn questTemp = NewItem(quest);
            questsOwn.Add(questTemp);
            num = FindItem(quest);
        }
        questsOwn[num].count += count;
        questsOwn[num].countText.text = "" + questsOwn[num].count;
        if (UIManager.i == 3)
            questsOwn[questsOwn.Count - 1].UI.SetActive(true);
        return;
    }


    public void UseItem(string id,int count =1)
    {
        int type = Convert.ToInt32(id.Substring(0, 1));
        int num = Num(id);
        if( type == 0)
        {
            UseItem(tools[num],count);
            return;
        }
        if(type == 1)
        {
            UseItem(foods[num],count);
            return;
        }
        if(type ==2)
        {
            UseItem(stuffs[num],count);
            return;
        }
        if(type == 3)
        {
            UseItem(quests[num],count);
            return;
        }
    }

    public void UseItem(ToolData tool,int count)
    {
        int num = FindItem(tool);
        if (num == -1)
            return;
        if (toolsOwn[num].count < count)
            return;
        if(BuildManager.instance.mapCubes.Count > 1|| BuildManager.instance.mapCubes.Count == 0)
        {
         //   Debug.Log("!111");
            UIManager.instance.Warning("请选中一个需要作用的塔");
            //错误音效
            return;
        }
        if(BuildManager.instance.mapCubes[0].turretGo.GetComponent<AttackDataManager>().Exist(tool.buff))
        {
            UIManager.instance.Warning("已经对该塔使用过此道具");
            return;
        }
        BuildManager.instance.mapCubes[0].turretGo.GetComponent<AttackDataManager>().SetBuffData(tool.buff);
        toolsOwn[num].count -= count;
        toolsOwn[num].countText.text = "" + toolsOwn[num].count;
        if (toolsOwn[num].count <= 0)
        {
            toolsOwn[num].UI.SetActive(false);
            toolsOwn.RemoveAt(num);
        }          
        return;
    }

    public void UseItem(FoodData food,int count)
    {
        int num = FindItem(food);
        if (num == -1)
            return;
        if (foodsOwn[num].count < count)
            return;
        if (food.foodType == FoodType.hungerRecover)
        {
            if (StateManager.major.hunger >= 100)
            {
                UIManager.instance.Warning("你已经很饱了");
                return;
            }
            StateManager.instance.ChangeHunger(food.recover * count);
        }
        else if (food.foodType == FoodType.hpRecover)
        {
            if (StateManager.major.hp >= 100)
            {
                //todo
                return;
            }
            StateManager.instance.ChangeHunger(food.recover * count);
        }
        else if(food.foodType == FoodType.iqRecover)
        {
            if(StateManager.major.iq >= StateManager.major.iqMax)
            {
                //todo
                return;
            }
            StateManager.instance.ChangeIQ((StateManager.major.iq + food.recover)*count);
        }              
        foodsOwn[num].count -= count;
        foodsOwn[num].countText.text = "" + foodsOwn[num].count;
        if (foodsOwn[num].count <= 0)
        {
            Destroy(foodsOwn[num].UI);
            foodsOwn.RemoveAt(num);
            return;
        }
    }

    public void UseItem(StuffData stuff,int count)
    {
        int num = FindItem(stuff);
        if (num == -1)
            return;
        if (stuffsOwn[num].count < count)
            return;
        stuffsOwn[num].count -= count;
        stuffsOwn[num].countText.text = "" + stuffsOwn[num].count;
        if (stuffsOwn[num].count <= 0)
            stuffsOwn.RemoveAt(num);
        return;
    }

    public void UseItem(QuestData quest,int count)
    {
        int num = FindItem(quest);
        if (num == -1)
            return;
        if (questsOwn[num].count < count)
            return;
        questsOwn[num].count -= count;
        questsOwn[num].countText.text = "" + questsOwn[num].count;
        if (questsOwn[num].count <= 0)
        {
            questsOwn[num].UI.SetActive(false);
            questsOwn.RemoveAt(num);
        }
        return;
    }

    public void Load()
    {
        foreach (SenceTool temp in GameDataManager.gameData.tools)
        {
            GetItem(temp.ID, temp.count);
        }
    }

    public static void Save(List<SenceTool> tools)
    {
        tools.Clear();
        foreach (ToolDataOwn temp in toolsOwn)
        {
            SenceTool tool = new SenceTool();
            tool.ID = temp.tool.id;
            tool.count = temp.count;
            tools.Add(tool);
        }
        foreach (FoodDataOwn temp in foodsOwn)
        {
            SenceTool tool = new SenceTool();
            tool.ID = temp.food.id;
            tool.count = temp.count;
            tools.Add(tool);
        }
        foreach (QuestDataOwn temp in questsOwn)
        {
            SenceTool tool = new SenceTool();
            tool.ID = temp.quest.id;
            tool.count = temp.count;
            tools.Add(tool);
        }
        foreach (StuffDataOwn temp in stuffsOwn)
        {
            SenceTool tool = new SenceTool();
            tool.ID = temp.stuff.id;
            tool.count = temp.count;
            tools.Add(tool);
        }
    }
}
