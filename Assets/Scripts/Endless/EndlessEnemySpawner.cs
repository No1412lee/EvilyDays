using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BornPoint
{
    public int bornKeyPoint;
    public List<Wave> waves;
    public int minWavesUnlock;
    public int rewardMoney;
    public EndlessAddEnemy addData;
    public GameObject obstacles;
    public GameObject hiddenMapCubes;
    //[HideInInspector]
    public int currentWave;
    public GameObject NextMapTrigger;
}
[System.Serializable]
public class EndlessAddEnemy
{
    public float addHp;
    public float addarmor;
    public float addspell;
    public float addspeed;
    public float hot;
}


public class EndlessEnemySpawner : MonoBehaviour
{
    public List<BornPoint> bornPoints;
    public int nextWaveRate;
    public GameObject nextButton;
    private int totalWave;
    [HideInInspector]
    public int currentBornPoint;
    [HideInInspector]
    public List<SelectBornPoint> selectBornPoints;
    private int sumBornPoint;
    private int killedEnemies;
    private FindPath findPath;
    private MapData mapData;
    private BuildManager build;
    public Text txtTimer;
    public GameObject timePanel;
    public void setBornPoint(int x)
    {
        if (x >= sumBornPoint)
            return;
        if (currentBornPoint != -1)
        {
            selectBornPoints[currentBornPoint].bornProcess.SetActive(false);
            selectBornPoints[currentBornPoint].keyPointProcess.SetActive(true);
        }
        currentBornPoint = x;
        selectBornPoints[currentBornPoint].bornProcess.SetActive(true);
        selectBornPoints[currentBornPoint].keyPointProcess.SetActive(false);
    }

    void OnEnemyDestroy(GameObject enemy)
    {
        killedEnemies++;
        BornPoint born = bornPoints[currentBornPoint];
        Wave wave = born.waves[Mathf.Min(born.waves.Count - 1, born.currentWave)];
        if (killedEnemies >= wave.enemyPrefabs.Count)
        {
            totalWave++;
            born.currentWave++;
            //---测试---
            //if (born.currentWave >= born.minWavesUnlock)
            //    nextBornPoint();
            //---测试---
            killedEnemies = 0;
            GameObject.Find("AudioSource/Bgm").GetComponent<AudioManager>().BGMAudioRandomNormalTime();
            build.isBuildStage = true;
            if (nextButton != null)
                nextButton.SetActive(true);
            //Invoke("nextWave", nextWaveRate);
            StartCoroutine("Timer2");
        }
    }

    private void Awake()
    {
        build = GetComponent<BuildManager>();
        mapData = GameObject.Find("Map").GetComponent<MapData>();
        findPath = GameObject.Find("A*").GetComponent<FindPath>();
        findPath.pathsDelegate += SpawnEnemy;
        currentBornPoint = -1;
    }

    // Use this for initialization
    void Start()
    {
        foreach (Transform child in GameObject.Find("MapBornPoint").transform)
        {
            selectBornPoints.Add(child.GetComponent<SelectBornPoint>());
        }

        Load();

        mapData.startPoint = bornPoints[sumBornPoint - 1].bornKeyPoint;
        build.isBuildStage = true;
        killedEnemies = 0;

        //  Invoke("nextWave", nextWaveRate);
        StartCoroutine("Timer2");
    }

    public void OnNextButtonDown()
    {
        if (build.mapCubes.Count == 1 && build.mapCubes[0].turretGo == null)
        {
            build.mapCubesClear();
            build.UI.updateUI();
        }
        nextWave();
    }

    public void nextWave()
    {
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioComing();
        GameObject.Find("AudioSource/Bgm").GetComponent<AudioManager>().BGMAudioRandomEnemyTime();
        build.isBuildStage = false;
        StopCoroutine("Timer2");
        timePanel.gameObject.SetActive(false);
        findPath.findNewPath(bornPoints[currentBornPoint].bornKeyPoint);
        //StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
    {
        if (nextButton != null)
            nextButton.SetActive(false);
        BornPoint born = bornPoints[currentBornPoint];
        Wave wave = born.waves[Mathf.Min(born.waves.Count - 1, born.currentWave)];
        for (int i = 0; i < wave.enemyPrefabs.Count; i++)
        {
            yield return new WaitForSeconds(wave.rates[i]);
            GameObject newEnemy = Instantiate(wave.enemyPrefabs[i]);
            if (born.currentWave > (born.waves.Count - 1))
            {
                newEnemy.GetComponent<EnemyBehaviour>().endlessADD(bornPoints[currentBornPoint].addData, born.currentWave - (born.waves.Count - 1));

            }
            Vector3 startPosition = mapData.wayPoints[bornPoints[currentBornPoint].bornKeyPoint];
            startPosition.y = newEnemy.transform.position.y;
            newEnemy.transform.position = startPosition;
            EnemyDestructionDelegate del = newEnemy.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    public void nextBornPoint()
    {
        if (sumBornPoint == bornPoints.Count)
            return;

        bornPoints[sumBornPoint - 1].hiddenMapCubes.SetActive(true);
        bornPoints[sumBornPoint - 1].obstacles.SetActive(false);
        BuildManager.ChangeMoney(bornPoints[sumBornPoint].rewardMoney);
        AstarPath.active.Scan();

        sumBornPoint++;
        setBornPoint(sumBornPoint - 1);
        mapData.startPoint = bornPoints[sumBornPoint - 1].bornKeyPoint;
    }
    public void onNextMapYesDown()
    {
        if (sumBornPoint == bornPoints.Count)
        {
            // TODO: 地图已开最大

            return;
        }

        BornPoint born = bornPoints[sumBornPoint - 1];
        if (born.currentWave < born.minWavesUnlock)
        {
            // TODO: 小于开图最小波数的报错
            GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioWrong();
            Debug.Log("当前波数小于开图最小波数");
            return;
        }

        GameObject.Find("MapBornPoint/nextMap").GetComponent<OpenMapManager>().fireTheWall();
    }

    public IEnumerator Timer2()
    {
        timePanel.gameObject.SetActive(true);
        for (int i = nextWaveRate - 1; i >= 0; i--)
        {
            txtTimer.text = string.Format("{0:d2}:{1:d2}", i / 60, i % 60);
            yield return new WaitForSeconds(1);
        }
        nextWave();
    }

    public void Save(SenceBornPointsWave senceBornPoints)
    {
        senceBornPoints.currentBornPoint = currentBornPoint;
        senceBornPoints.sumBornPoint = sumBornPoint;
        senceBornPoints.totalWave = totalWave;
        for (int i = 0; i < bornPoints.Count; i++)
            senceBornPoints.currentWaves[i] = bornPoints[i].currentWave;
    }

    public void Load()
    {
        SenceBornPointsWave senceBornPoints = GameDataManager.gameData.bornPoints;
        sumBornPoint = 1;
        while (sumBornPoint < senceBornPoints.sumBornPoint)
            nextBornPoint();
        setBornPoint(senceBornPoints.currentBornPoint);
        totalWave = senceBornPoints.totalWave;
        for (int i = 0; i < bornPoints.Count; i++)
            bornPoints[i].currentWave = senceBornPoints.currentWaves[i];
    }
}
