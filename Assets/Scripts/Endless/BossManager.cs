using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShowBoss : MonoBehaviour {

    List<BossData> bosses;
    public static ShowBoss instance;


    void Awake()
    {
        instance = this;
    }

    public void ShowMeBoss(BossData boss)
    {

        int num = bosses.IndexOf(boss);
        for(int i = 0;i<=boss.needs.Count;i++)
        { 
            int count = ToolsManager.instance.FindItem(boss.needs[i].questId);
            if (count < boss.needs[i].needCount)
                return;
        }
        for (int j = 0; j < boss.needs.Count; j++)
        {
            ToolsManager.instance.UseItem(boss.needs[j].questId, boss.needs[j].needCount);
        }
           //出现boss
        return;
    }
}
