using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public List<GameObject> enemyPrefabs;
    public List<float> rates;
    //    public int goldGiven;
}

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves;
    public int nextWaveRate;
    public GameObject nextButton;
    private int currentWave;
    private int killedEnemies;
    private FindPath findPath;
    private MapData mapData;
    private ChapterBuildManager build;
    public Text txtTimer;

    void OnEnemyDestroy(GameObject enemy)
    {
        killedEnemies++;
        if (killedEnemies >= waves[currentWave].enemyPrefabs.Count)
        {
            currentWave++;
            if (currentWave >= waves.Count)
            {
                GameManager.instance.Win();
                return;
            }
            killedEnemies = 0;
            build.isBuildStage = true;
            GameObject.Find("AudioSource/Bgm").GetComponent<AudioManager>().BGMAudioRandomNormalTime();
            if (nextButton != null)
                nextButton.SetActive(true);
            //Invoke("nextWave", nextWaveRate);
            StartCoroutine("Timer2");
        }
    }

    // Use this for initialization
    void Start()
    {
        build = GetComponent<ChapterBuildManager>();
        mapData = GameObject.Find("Map").GetComponent<MapData>();
        findPath = GameObject.Find("A*").GetComponent<FindPath>();
        findPath.pathsDelegate += SpawnEnemy;
        currentWave = 0;
        killedEnemies = 0;
        build.isBuildStage = true;
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
        txtTimer.gameObject.SetActive(false);
        findPath.findNewPath(0);
        //StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
    {
        if (nextButton != null)
            nextButton.SetActive(false);
        Wave wave = waves[currentWave];
        for (int i = 0; i < wave.enemyPrefabs.Count; i++)
        {
            yield return new WaitForSeconds(wave.rates[i]);
            GameObject newEnemy = Instantiate(wave.enemyPrefabs[i]);
            Vector3 startPosition = mapData.wayPoints[mapData.startPoint];
            startPosition.y = newEnemy.transform.position.y;
            newEnemy.transform.position = startPosition;
            EnemyDestructionDelegate del = newEnemy.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    public IEnumerator Timer2()
    {
        txtTimer.gameObject.SetActive(true);
        for (int i = nextWaveRate - 1; i >= 0; i--)
        {
            txtTimer.text = string.Format("{0:d2}:{1:d2}", i / 60, i % 60);
            yield return new WaitForSeconds(1);
        }
        nextWave();
    }
}
