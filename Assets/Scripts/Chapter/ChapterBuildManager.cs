using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterBuildManager : MonoBehaviour
{
    public static ChapterBuildManager instance;
    public List<TurretData> turretDatas;
    //基础塔：
    public GameObject TurretObstacle;
    public GameObject JudgePanel;
    //public GameObject TurretZSQ;

    //升级塔：
    //public GameObject TurretEric;
    //当前选择的炮台（准备建造的）
    //public TurretData selectedTurretDate;

    //public GameObject barrier;
    //Seeker seeker;
    //GameObject ob;
    //基础建设按钮：
    //public Button ZsqButton;
    //public Button ObButton;
    //
    [HideInInspector]
    public MapData mapData;
    //public MapCube mapCube;
    [HideInInspector]
    public List<MapCube> mapCubes;
    public MapCube lastMapCube;
    [HideInInspector]
    public UpgradeData upgradeData;
    public ChapterUIManager UI;
    //功能按钮：
    //public Button NextButton;
    //public Button HideButton;
    //public Button RecommendButton;
    //public Button MergeButton;
    //功能UI：
    //public GameObject MergeButtonUI;
    //public GameObject BuildPanel;
    public GameObject FunctionPanel;
    public GameObject buildSuccessEffect;
    //多脚本调用数据：
    public static Slider BaseHpSlider;
    public static int money = 500;
    public static int moneyMax = 500;
    public bool isBuildStage;
    public static Text hpText;
    public static Text moneyText;
    public AudioClip musicMapCubeSelect;
    //
    // 自锁cd:
    private bool mouseDown = false;

    //private bool flag;//记录是否能够合成
    void Awake()
    {
        instance = this;
    }
    void upgradePath()
    {
        //TurretEric.downTurret = TurretZSQ;
        //TurretZSQ.downTurret = null;
        ////TODO
    }
    void Start()
    {
        //  Debug.Log("建造");
        //ob = null;
        //seeker = GetComponent<Seeker>();
        mapData = GameObject.Find("Map").GetComponent<MapData>();
        UI = GameObject.Find("MainCanvas").GetComponent<ChapterUIManager>();
        upgradeData = GetComponent<UpgradeData>();
        Vec2Int.xSize = mapData.xSize;
        Vec2Int.ySize = mapData.zSize;
        moneyText = GameObject.Find("MainCanvas/MoneyPanel/MoneyText").GetComponent<Text>();
        hpText = GameObject.Find("MainCanvas/MoneyPanel/HpText").GetComponent<Text>();
        BaseHpSlider = GameObject.Find("BaseHpCanvas/BaseHpSlider").GetComponent<Slider>();
        mapCubes = new List<MapCube>();
        for (int i = 0; i < turretDatas.Count; i++)
        {
            turretDatas[i].ID = i;
            turretDatas[i].turretPrefab.GetComponent<TurretDataLink>().data = turretDatas[i];
        }
    }

    public static void ChangeMoney(int change)
    {
        money = money - change;
        if (money >= moneyMax)
            money = moneyMax;
        if (money < 0)
            money = 0;
        moneyText.text = money + "/" + moneyMax;
    }


    public static void BaseHpChange(float attack)
    {
        BaseHpSlider.value -= attack / 100;
        GameObject.Find("BaseHpCanvas/BaseHpSlider").GetComponent<Slider>().value = BaseHpSlider.value;
        hpText.text =BaseHpSlider.value * 100 + "/100";
        if (BaseHpSlider.value <= 0)
        {
            GameManager.instance.Failed();
        }
    }

    public void BFS(Queue q, bool[][] found, Vec2Int k)
    {
        if (k.legal() && mapData.map[k.x][k.y] != null)
        {
            if (!found[k.x][k.y]
                && mapData.map[k.x][k.y].gameObject.activeInHierarchy
                && mapData.map[k.x][k.y].turretGo == null)
                q.Enqueue(k);
            found[k.x][k.y] = true;
        }
    }
    public bool Buildable(MapCube mapCube)
    {
        if (!mapCube.Buildable)
            return false;

        Queue q = new Queue();
        Vec2Int k = new Vec2Int();
        Vec2Int end = new Vec2Int();

        int nowPoint = mapData.startPoint;
        while (nowPoint + 1 < mapData.wayPoints.Count)
        {
            q.Clear();
            k.x = (int)mapData.wayPoints[nowPoint].x + mapData.xDiff;
            k.y = (int)mapData.wayPoints[nowPoint].z + mapData.zDiff;
            q.Enqueue(k);
            end.x = (int)mapData.wayPoints[nowPoint + 1].x + mapData.xDiff;
            end.y = (int)mapData.wayPoints[nowPoint + 1].z + mapData.zDiff;

            bool[][] found = new bool[mapData.xSize][];
            for (int i = 0; i < mapData.xSize; i++)
            {
                found[i] = new bool[mapData.zSize];
                for (int j = 0; j < mapData.zSize; j++)
                {
                    found[i][j] = false;
                }
            }
            found[(int)mapCube.transform.position.x + mapData.xDiff][(int)mapCube.transform.position.z + mapData.zDiff] = true;
            if (found[k.x][k.y])
                return false;
            found[k.x][k.y] = true;

            bool flag = false;
            while (q.Count > 0)
            {
                k = (Vec2Int)q.Dequeue();
                if (k.Equals(end))
                {
                    flag = true;
                    break;
                }

                BFS(q, found, new Vec2Int((int)(k.x - 1), (int)k.y));
                BFS(q, found, new Vec2Int((int)(k.x + 1), (int)k.y));
                BFS(q, found, new Vec2Int((int)k.x, (int)(k.y - 1)));
                BFS(q, found, new Vec2Int((int)k.x, (int)(k.y + 1)));
            }
            if (!flag)
                return false;
            nowPoint++;
        }
        return true;
    }

    //public void onPathComplete(Path p)
    public void buildTurret(GameObject turret)
    {
        TurretData turretData = turret.GetComponent<TurretDataLink>().data;
        if (money >= turretData.buildCost)
        {
            Vector3 tempV3 = new Vector3(mapCubes[0].transform.position.x, mapCubes[0].transform.position.y + 0.15f, mapCubes[0].transform.position.z);
            GameObject.Instantiate(buildSuccessEffect, tempV3, mapCubes[0].transform.rotation);
            GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioSuccess();
            ChangeMoney(turretData.buildCost);
            int i = mapCubes.Count - 1;
            while (i > 0)
            {
                mapCubes[i].ReBuild(TurretObstacle);
                i--;
            }

            if (mapCubes[0].turretGo == null)
                mapCubes[0].BuildTurret(turret);
            else
                mapCubes[0].ReBuild(turret);
        }
        else
        {
            GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioMergeFail();
            // TODO: 不可建塔，金钱不足

        }
    }

    public void mapCubesAdd(MapCube x)
    {
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioSelect();
        x.highLight();
        mapCubes.Add(x);


        //TODO 隐藏原攻击范围，隐藏数值
        if (lastMapCube != null)
            lastMapCube.HideFanwei();

        lastMapCube = x;

        //TODO 显示新攻击范围，显示数值
        lastMapCube.ShowFanwei();
    }

    public void mapCubesRemove(MapCube x)
    {
        x.normalLight();
        mapCubes.Remove(x);
        if (lastMapCube == x)
        {
            //TODO 隐藏攻击范围，隐藏数值
            if (lastMapCube != null)
                lastMapCube.HideFanwei();

            lastMapCube = null;

        }
    }

    public void mapCubesClear()
    {
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioStopSelect();
        foreach (MapCube x in mapCubes)
        {
            x.normalLight();
        }
        mapCubes.Clear();
        //TODO 隐藏攻击范围，隐藏数值
        if (lastMapCube != null)
            lastMapCube.HideFanwei();
        lastMapCube = null;
    }

    public void onSelectMapCube(Vector3 clickPosition)
    {
        //  Debug.Log(SceneManager.GetActiveScene().name);

        //无尽模式下的点击判定

        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        RaycastHit hit;
        bool isColider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Plane"));
        if (isColider)
        {
            MapCube mapCube = hit.collider.GetComponent<MapCube>();//点击的是哪个方格
            if (mapCubes.Contains(mapCube))
            {
                mapCubesRemove(mapCube);
            }
            else if (mapCube.turretGo == null)
            {
                mapCubesClear();

                if (isBuildStage == false)
                {
                    GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioError();
                    // TODO: 战斗阶段不可建塔
                }
                else if (!Buildable(mapCube))
                {
                    GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioError();
                    // TODO: 阻挡
                }
                else
                {
                    GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioSuccess();
                    mapCubesAdd(mapCube);
                }
            }
            else
            {
                if (mapCubes.Count == 1 && mapCubes[0].turretGo == null)
                    mapCubesClear();

                mapCubesAdd(mapCube);
            }
            UI.updateUI();
        }
    }

    // Update is called once per frame
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    void Update()
    {
        MapCube.isBuildStage = isBuildStage;
        if (Input.GetMouseButton(0))     //鼠标左键点击
        {
            if (!mouseDown)
            {
                mouseDown = true;
                if (EventSystem.current.IsPointerOverGameObject() == false) //是否点在UI里
                {
                    onSelectMapCube(Input.mousePosition);
                }
            }
        }
        else
        {
            mouseDown = false;
        }
    }
#endif

    public void OnNextDown()
    {
        JudgePanel.SetActive(true);
    }
}
