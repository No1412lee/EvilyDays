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

public class ChapterUIManager : MonoBehaviour
{
    public static ChapterUIManager instance;
    public GameObject scrollRect;
    public GameObject destroyButton;
    public GameObject mergeButton;
    public GameObject buildPanel;
    public GameObject SetPanel;
    public List<GameObject> Judge;

    [HideInInspector]
    public List<GameObject> activeButtons;
    [HideInInspector]
    public ChapterBuildManager build;
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
    void Start()
    {
        build = GameObject.Find("GameManager").GetComponent<ChapterBuildManager>();
        activeButtons = new List<GameObject>();
        gridRect = scrollRect.transform.FindChild("Grid").GetComponent<RectTransform>();
        //   UIAudioSource = GetComponent<AudioSource>();

        //foreach (TurretButtonLink turretButton in turretButtons)
        //{
        //    turretButton.turretPrefab.GetComponent<TurretData>().UI = turretButton.turretButton;
        //}
    }

    public void ClickVoice()
    {
        GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioSuccess();
    }
    public void OnMergeButtonDown()
    {
        ClickVoice();
        if (ChapterBuildManager.money >= build.upgradeData.mergeCost)
        {
            ChapterBuildManager.ChangeMoney(build.upgradeData.mergeCost);
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
        ClickVoice();
        //mapCubes[0].turret.upgradeUI.SetActive(false);
        GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioSuccess();
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnvAudioDestroy();
        ChapterBuildManager.ChangeMoney(-build.mapCubes[build.mapCubes.Count - 1].turret.recover);
        build.mapCubes[0].DestroyTurret();
        activeClear();
        build.mapCubesClear();
    }

    public void OnMenuButtonDown()
    {
        ClickVoice();
        if (flag)
        {
            SetPanel.SetActive(true);
            flag = false;
            return;
        }
        if (!flag)
        {
            SetPanel.SetActive(false);
            flag = true;
        }
        return;
    }


    public void HideNo()
    {
        foreach (GameObject judge in Judge)
        {
            judge.SetActive(false);
        }
        return;
    }

}
