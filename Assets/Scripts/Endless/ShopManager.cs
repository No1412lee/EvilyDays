using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public List<GoodData> goods;
	// Use this for initialization
	
    public GoodData ChoseGood()
    {
        List<GoodData> goodsTemp = new List<GoodData>() ;


        foreach (GoodData good in goods)
        {
            if (good.price < StateManager.major.hp)
                goodsTemp.Add(good);
        }
        if (goodsTemp.Count <= 0)
            return null;
        float total = 0;
        foreach(GoodData good in goodsTemp)
        {
            total += good.prob;
        }
        float randomPoint = Random.value * total;
        for(int i =0;i<goodsTemp.Count;i++)
        {
            if (randomPoint < goodsTemp[i].prob)
                return goods[i];
            else
                randomPoint -= goodsTemp[i].prob;
        }
        return goodsTemp[goods.Count - 1];
    }

    public void Buy()
    {
        GoodData good = ChoseGood();
        if (good == null)
            return;
        StateManager.instance.ChangeLife(-good.price);
        ToolsManager.instance.GetItem(good.itemId, 1);
        float luck = Random.value;
        if (luck >= 0.95)
            ToolsManager.instance.GetItem(good.itemId, 1);
        return;
    }
}
