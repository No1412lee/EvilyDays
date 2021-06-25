using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    [HideInInspector]
    public List<Vector3> wayPoints;
    public List<GameObject> keyPoints;
    public MapCube[][] map;
    public int xSize;
    public int zSize;
    public int xDiff;
    public int zDiff;

    [HideInInspector]
    public int startPoint = 0;

    public void setKeyPoints()
    {
        wayPoints.Clear();
        // 寻路关键点
        for (int i = 0; i < keyPoints.Count; i++)
        {
            wayPoints.Add(keyPoints[i].transform.position);
        }
    }

    public void Load()
    {
        foreach (SenceTurret turret in GameDataManager.gameData.turrets)
        {
            GameObject newTurret = map[turret.x][turret.y].BuildTurret(BuildManager.instance.turretDatas[turret.ID].turretPrefab);
            AttackDataManager turretAttackData = newTurret.GetComponent<AttackDataManager>();
            foreach (BuffCount buffCount in turret.buffs)
            {
                turretAttackData.SetBuffData(buffCount.buff, buffCount.keepCount);
            }
        }
    }

    public void Save(List<SenceTurret> turrets)
    {
        turrets.Clear();
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
                if (map[i][j] != null && map[i][j].turretGo != null)
                {
                    SenceTurret turret = new SenceTurret();
                    turret.x = i;
                    turret.y = j;
                    turret.ID = map[i][j].turret.ID;
                    turret.buffs = map[i][j].turretGo.GetComponent<AttackDataManager>().buffs;
                    turrets.Add(turret);
                }
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Endless")
            Load();
    }

    // Use this for initialization
    void Awake()
    {
        setKeyPoints();
        map = new MapCube[xSize][];

        for (int i = 0; i < xSize; i++)
        {
            map[i] = new MapCube[zSize];
        }
        Vector3 nowP;
        foreach (Transform childMap in transform)
        {
            foreach (Transform child in childMap.transform)
            {
                nowP = child.position;
                map[(int)nowP.x + xDiff][(int)nowP.z + zDiff] = child.GetComponent<MapCube>();
            }
        }
    }
}
