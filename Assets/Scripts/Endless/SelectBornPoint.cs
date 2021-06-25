using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBornPoint : MonoBehaviour {
    public GameObject selectBornPointPanel;
    public GameObject bornProcess;
    public GameObject keyPointProcess;
    public int bornPoint;
    public static int selectPoint;
    private static EndlessEnemySpawner spawner;
    void OnTriggerEnter(Collider col)
    {
        if (!GameObject.Find("GameManager").GetComponent<BuildManager>().isBuildStage)
            return;
        if (col.tag == "Player")
        {
            if (bornPoint != spawner.currentBornPoint)
            {
                //TODO 跳出窗口，询问是否切换怪物出生点
                selectBornPointPanel.SetActive(true);
                selectPoint = bornPoint;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            selectBornPointPanel.SetActive(false);
        }
    }

    public void onSelectYesDown()
    {
        if (true)   // 判断资源是否足够
        {
            spawner.setBornPoint(selectPoint);
            // TODO: 扣资源
        }
    }
    private void Awake()
    {
        if (spawner == null)
            spawner = GameObject.Find("GameManager").GetComponent<EndlessEnemySpawner>();
    }
}
