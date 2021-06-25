using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//[System.Serializable]
//public class TurretButtonLink
//{
//    public GameObject turretButton;
//    public GameObject turretPrefab;
//}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public int gridHeight;
    public int buttonHeight;
    public GameObject scrollRect;
    public GameObject destroyButton;
    public GameObject mergeButton;
    public GameObject bagPanel;
    public GameObject bagButtonPanel;
    public GameObject bagButton;
    public GameObject questionPanel;
    public GameObject buildPanel;
    public GameObject setPanel;
    public GameObject NormalControlImage;
    public static int i = 1;//默认食物
    public GameObject shopPanel;
    public Text question;
    public Text answerA;
    public Text answerB;
    public Text answerC;
    public Text answerD;

    public Text warningText;

    public Sprite foodDown;
    public Sprite foodUp;
    public Sprite questDown;
    public Sprite questUp;
    public Sprite stuffDown;
    public Sprite stuffUp;

    public List<GameObject> Judge;

    [HideInInspector]
    public List<GameObject> activeButtons;
    [HideInInspector]
    public BuildManager build;
    private ToolsManager toolsManager;
    
    private RectTransform gridRect;

    private bool flag = true;
    void Awake()
    {
        instance = this;
    }
    public void buildTurretByUI(GameObject UI)
    {
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioBuild();
        build.buildTurret(build.turretDatas.Find(x => x.UI == UI).turretPrefab);
    }

    public void activeAdd(GameObject x)
    {
        activeButtons.Add(x);
        x.SetActive(true);
    }

    public void activeRemove(GameObject x)
    {
        activeButtons.Remove(x);
        x.SetActive(false);
    }

    public void activeClear()
    {
        //scrollRect.SetActive(false);
        buildPanel.SetActive(false);
        foreach (GameObject x in activeButtons)
        {
            x.SetActive(false);
        }
        activeButtons.Clear();
    }

    public void panelBuild()
    {
        foreach (GameObject turret in build.upgradeData.primaryTurret)
        {
            activeAdd(turret.GetComponent<TurretDataLink>().data.UI);
        }
    }

    public void panelUpgrade(GameObject turret)
    {
        if (build.isBuildStage)
            activeAdd(destroyButton);
        TurretData turretData = turret.GetComponent<TurretDataLink>().data;
        foreach (GameObject upgradeTurret in turretData.upgradeTurretPrefab)
        {
            activeAdd(upgradeTurret.GetComponent<TurretDataLink>().data.UI);
        }
    }

    public void panelMerge()
    {
        activeAdd(mergeButton);
    }
    public void updateUI()
    {
        activeClear();
        if (build.mapCubes.Count == 1)
        {
            if (build.mapCubes[0].turretGo == null)
                panelBuild();
            else
                panelUpgrade(build.mapCubes[0].turretGo);
        }
        else if (build.mapCubes.Count > 1)
        {
            panelMerge();
        }

        if (build.mapCubes.Count > 0)
            //scrollRect.SetActive(true);
            buildPanel.SetActive(true);
    }

    // Use this for initialization
    void Start ()
    {
        build = GameObject.Find("GameManager").GetComponent<BuildManager>();
        activeButtons = new List<GameObject>();
        gridRect = scrollRect.transform.FindChild("Grid").GetComponent<RectTransform>();
        toolsManager = GameObject.Find("GameManager").GetComponent<ToolsManager>();
     //   UIAudioSource = GetComponent<AudioSource>();
        
        //foreach (TurretButtonLink turretButton in turretButtons)
        //{
        //    turretButton.turretPrefab.GetComponent<TurretData>().UI = turretButton.turretButton;
        //}
    }


    public void OnMergeButtonDown()
    {
        if (BuildManager.money >= build.upgradeData.mergeCost)
        {
            BuildManager.ChangeMoney(build.upgradeData.mergeCost);
            bool flag = false;
            foreach (Upgrade upgrade in build.upgradeData.upgrades)
            {
                if (upgrade.Equals(build.mapCubes))
                {
                    if (upgrade.possibility >= Random.value)
                    {
                        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioMergeSuccess();
                        build.buildTurret(upgrade.goalTurret);
                        flag = true;
                        break;
                    }
                }
            }

            // 合成失败
            if (!flag)
            {
                GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioMergeFail();
               //TODO
            }
        }
        else
        {
            GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioMergeFail();
            // TODO: 金钱不够合成

        }

        activeClear();
        build.mapCubesClear();
    }

    public void OnDestroyButtonDown()
    {
     
        //mapCubes[0].turret.upgradeUI.SetActive(false);
        GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioSuccess();
        BuildManager.ChangeMoney(-build.mapCubes[build.mapCubes.Count - 1].turret.recover);
        build.mapCubes[0].DestroyTurret();
        activeClear();
        build.mapCubesClear();
    }

    public void ClickVoice()
    {
        GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioSuccess();
    }

    public void ShowToolsPanel()
    {

        for(int i =0;i <= ToolsManager.toolsOwn.Count -1;i++)
        {
            ToolsManager.toolsOwn[i].UI.SetActive(true);
        }
    }

    public void ShowFoodsPanel()
    {
        for (int i = 0; i <= ToolsManager.foodsOwn.Count - 1; i++)
        {
            ToolsManager.foodsOwn[i].UI.SetActive(true);
        }
    }

    public void ShowStuffsPanel()
    {
        for (int i = 0; i <= ToolsManager.stuffsOwn.Count - 1; i++)
        {
            ToolsManager.stuffsOwn[i].UI.SetActive(true);
        }
    }

    public void ShowQuestPanel()
    {
        for(int i =0;i<=ToolsManager.questsOwn.Count - 1;i++)
        {
            ToolsManager.questsOwn[i].UI.SetActive(true);
        }
    }

    public void HideToolsPanel()
    {
        for (int i = 0; i <= ToolsManager.toolsOwn.Count - 1; i++)
        {
            ToolsManager.toolsOwn[i].UI.SetActive(false);
        }
    }

    public void HideFoodsPanel()
    {
    for (int i = 0; i <= ToolsManager.foodsOwn.Count - 1; i++)
    {
        ToolsManager.foodsOwn[i].UI.SetActive(false);
    }
    }

    public void HideStuffsPanel()
    {
        for (int i = 0; i <= ToolsManager.stuffsOwn.Count - 1; i++)
        {
            ToolsManager.stuffsOwn[i].UI.SetActive(false);
        }
    }

    public void HideQuestPanel()
    {
        for (int i = 0; i <= ToolsManager.questsOwn.Count - 1; i++)
        {
            ToolsManager.questsOwn[i].UI.SetActive(false);
        }
    }

    public void OnToolsButtonDown()
    {
        ClickVoice();
        HideFoodsPanel();
        //HideStuffsPanel();
        HideQuestPanel();
        ShowToolsPanel();
        i = 0;
        bagButtonShow();
    }

    public void OnFoodsButtonDown()
    {
        ClickVoice();
        HideToolsPanel();
        //HideStuffsPanel();
        HideQuestPanel();
        ShowFoodsPanel();
        i = 1;
        bagButtonShow();
    }

    public void OnStuffsButtonDown()
    {
        ClickVoice();
        HideToolsPanel();
        HideFoodsPanel();
        HideQuestPanel();
        ShowStuffsPanel();
        i = 2;
        bagButtonShow();
    }

    public void OnQuestButtonDown()
    {
        ClickVoice();
        HideToolsPanel();
        HideFoodsPanel();
        HideStuffsPanel();
        ShowQuestPanel();
        i = 3;
        bagButtonShow();
    }

    public void bagButtonShow()
    {
        GameObject.Find("MainCanvas/BagPanel/BagButtonPanel/stuffButton").GetComponent<Image>().sprite = stuffUp;
        GameObject.Find("MainCanvas/BagPanel/BagButtonPanel/foodButton").GetComponent<Image>().sprite = foodUp;
        GameObject.Find("MainCanvas/BagPanel/BagButtonPanel/questButton").GetComponent<Image>().sprite = questUp;
        if (i == 0)
            GameObject.Find("MainCanvas/BagPanel/BagButtonPanel/stuffButton").GetComponent<Image>().sprite = stuffDown;
        if( i == 1)
            GameObject.Find("MainCanvas/BagPanel/BagButtonPanel/foodButton").GetComponent<Image>().sprite = foodDown;
        if(i == 3)
            GameObject.Find("MainCanvas/BagPanel/BagButtonPanel/questButton").GetComponent<Image>().sprite = questDown;
        return;

    } 

    public void HideBagButtonDown()
    {
        ClickVoice();
        bagPanel.SetActive(false);
        bagButtonPanel.SetActive(false);
        bagButton.SetActive(true);
    }

    public void ShowBagButtonDown()
    {
        ClickVoice();
        bagPanel.SetActive(true);
        bagButtonPanel.SetActive(true);
        bagButton.SetActive(false);
        bagButtonShow();     
    }

    public void ShowQuestionPanel()
    {
       
        questionPanel.SetActive(true);
    }

    public void HideQuestionPanel()
    {
        questionPanel.SetActive(false);
        
    }

    public void GetQuestion(QuestionData questionTemp)
    {
        question.text= questionTemp.question;
        answerA.text = "A: " + questionTemp.answerA;
        answerB.text = "B: " + questionTemp.answerB;
        answerC.text = "C: " + questionTemp.answerC;
        answerD.text = "D: " + questionTemp.answerD;
    }

    public void CleanQuestion()
    {
        question.text = "";
        answerA.text = "";
        answerB.text = "";
        answerC.text = "";
        answerD.text = "";
    }

    public void OnMenuButtonDown()
    {
        ClickVoice();
        if (flag)
        {
            setPanel.SetActive(true);
            flag = false;
            return;
        }
        if (!flag)
        {
            setPanel.SetActive(false);
            flag = true;
        }
        return;
    }
    
    public void ShowShopPanel()
    {
        shopPanel.SetActive(true);
    }

    public void HideShopPane()
    {
        shopPanel.SetActive(false);
    }

    public void HideJudgePanel()
    {
        foreach (GameObject judge in Judge)
        {
            judge.SetActive(false);
        }
        return;
    }

    public void Warning(string warning)
    {
        warningText.text = warning;
        warningText.color = Color.white;
        warningText.gameObject.SetActive(true);
    }

    public void NormalControlShow()
    {
        if (NormalControlImage.activeInHierarchy == false)
            NormalControlImage.SetActive(true);
    }
    public void NormalControlDisapear()
    {
        if (NormalControlImage.activeInHierarchy == true)
            NormalControlImage.SetActive(false);
    }
}

