using UnityEngine;
using System.Collections.Generic;

[SerializeField]
public class SenceTurret
{
    public List<BuffCount> buffs;
    public int ID;
    public int x, y;
}

[SerializeField]
public class SenceBornPointsWave
{
    public List<int> currentWaves;
    public int totalWave;
    public int currentBornPoint;
    public int sumBornPoint;
}

[SerializeField]
public class SenceTool
{
    public string ID;
    public int count;
}

//GameData,储存数据的类，把需要储存的数据定义在GameData之内就行// 
public class GameData
{
    //密钥,用于防止拷贝存档// 
    public string key;

    //下面是添加需要储存的内容// 
    public bool isDu;//点击读档为True，加载场景后为False

    //场景存储内容
    public List<SenceTurret> turrets;
    public SenceBornPointsWave bornPoints;
    public StateData major;
    public List<SenceTool> tools;

    public GameData()
    {
        isDu = false;
    }
}
//管理数据储存的类// 
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager This;

    private string dataFileName = "EndlessData.dat";//存档文件的名称,自己定// 
    private XmlSaver xs = new XmlSaver();

    public static GameData gameData;

    public void Awake()
    {
        This = this;
        gameData = new GameData();
        //设定密钥，根据具体平台设定// 
        gameData.key = SystemInfo.deviceUniqueIdentifier;
        Load();
    }

    public void setup()
    {
        gameData.turrets = new List<SenceTurret>();

        EndlessEnemySpawner spawner = GameObject.Find("GameManager").GetComponent<EndlessEnemySpawner>();
        gameData.bornPoints = new SenceBornPointsWave();
        gameData.bornPoints.currentBornPoint = 0;
        gameData.bornPoints.sumBornPoint = 1;
        gameData.bornPoints.currentWaves = new List<int>();
        for (int i = 0; i < spawner.bornPoints.Count; i++)
            gameData.bornPoints.currentWaves.Add(0);

        gameData.major = new StateData();
        gameData.tools = new List<SenceTool>();
    }

    public void DeleteArchive()
    {
        string gameDataFile = GetDataPath() + "/" + dataFileName;
        xs.DeleteXML(gameDataFile);
    }

    //存档时调用的函数// 
    public void Save()
    {
        if (!GameObject.Find("GameManager").GetComponent<BuildManager>().isBuildStage)
        {
            // 战斗阶段无法存档
            return;
        }

        GameObject.Find("Map").GetComponent<MapData>().Save(gameData.turrets);
        GameObject.Find("GameManager").GetComponent<EndlessEnemySpawner>().Save(gameData.bornPoints);
        ToolsManager.Save(gameData.tools);

        gameData.major = StateManager.major;

        string gameDataFile = GetDataPath() + "/" + dataFileName;
        string dataString = xs.SerializeObject(gameData, typeof(GameData));
        xs.CreateXML(gameDataFile, dataString);
    }

    //读档时调用的函数// 
    public void Load()
    {
        MapData mapData = GameObject.Find("Map").GetComponent<MapData>();
        string gameDataFile = GetDataPath() + "/" + dataFileName;
        if (xs.hasFile(gameDataFile))
        {
            string dataString = xs.LoadXML(gameDataFile);
            GameData gameDataFromXML = xs.DeserializeObject(dataString, typeof(GameData)) as GameData;

            //是合法存档// 
            if (gameDataFromXML.key == gameData.key)
            {
                gameData = gameDataFromXML;
            }
            //是非法拷贝存档// 
            else
            {
                //留空：游戏启动后数据清零，存档后作弊档被自动覆盖// 
                setup();
            }
        }
        else
        {
            setup();
        }
    }

    //获取路径//
#if !UNITY_EDITOR && UNITY_ANDROID
    private static string GetDataPath()
    {
        return Application.persistentDataPath;
    }
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    private static string GetDataPath()
    {
        // Your game has read+write access to /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/Documents 
        // Application.dataPath returns ar/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data              
        // Strip "/Data" from path 
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            // Strip application name 
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents";
        }
        else
            //    return Application.dataPath + "/Resources"; 
            return Application.dataPath;
    }
#endif
}