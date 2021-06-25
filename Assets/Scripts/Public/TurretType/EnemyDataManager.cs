using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropDate
{
    public string itemID;
    public float probability;
}

[System.Serializable]
public class EnemyData
{
    public float maxHitPoint;
    public float hitPoint;
    public float speed;
    public float hot;
    public int attack;
    public float spell;
    public float armor;
    public GameObject enemyPrefab;
    public int reward;

    public float foodPosibility;
    public GameObject foodPrefab;
    //   public GameObject explosionEffect;
    public List<DropDate> dropDates;
    public GreenEnemyData greenEnemyData;
}
[System.Serializable]
public class DeBuffCount
{
    public float keepCount;
    public DeBuff buff;
}
[System.Serializable]
public class GreenEnemyData
{
    public float dot;
    public float speed;
    public float spell;
    public float armor;
}
public class EnemyDataManager : MonoBehaviour
{
    public EnemyData enemyData;
    public List<DeBuffCount> buffs;
    public GameObject dotEffect;
    public GameObject slowEffect;
    public GameObject braekEffect;
    private int dotNumber = 0;
    private int slowNumber = 0;
    private int breakNumber = 0;
    //  private float NormalSpeed;
    private int index;
    public Transform effectPosion1;
    public Transform effectPosion2;
    
    void Start()
    {
        //     NormalSpeed = enemyData.speed;
    }

    void Update()
    {
        if (buffs != null)
        {
            for (index = 0; index < buffs.Count; index++)
            {
                //         Debug.Log(index + "  Buffs.count"+buffs.Count);
                buffs[index].keepCount += Time.deltaTime;
                if (buffs[index].keepCount > buffs[index].buff.keepTime)
                {
                    buffs[index].keepCount = 0;
                    BuffClear();
                    index--;
                }
            }
        }

    }

    //public GameObject explosionEffect; //爆炸特效 
    public void SetBuffData(DeBuff buff)
    {
        if (buffs != null)
        {
            for (int j = 0; j < buffs.Count; j++)
            {
                if (buffs[j].buff.DeBuffId == buff.DeBuffId)
                {
                    //     Debug.Log("刷新" + transform.name);
                    buffs[j].keepCount = 0;
                    return;
                }
            }
        }//同种Buff不可叠加,只刷新持续时间
        DeBuffCount buffCount = new DeBuffCount();
        buffCount.keepCount = 0;
        buffCount.buff = buff;
        //     Debug.Log(buffCount.keepCount+""+buffCount.buff.buffId);

        buffs.Add(buffCount);
        BuffBegin(buffCount);

    }



    public void BuffBegin(DeBuffCount buffCount)
    {
        //BUFF编号规则
        // 第一位的0、1、2、3分别表示：DOT、移速、减防
        // 第二位的0、1 分别表示：固定数值、百分比
        // 第三位及之后，表示该Buff的等级
        if (buffCount.buff.DeBuffId.Length < 3)
            return;

        //dot
        if (buffCount.buff.DeBuffId[0] == '0')
        {
               enemyData.greenEnemyData.dot += buffCount.buff.dotAttack;
        }

        
        if (buffCount.buff.DeBuffId[0] == '1')
        {
            if (buffCount.buff.DeBuffId[1] == '0')
            {
                enemyData.greenEnemyData.speed += buffCount.buff.slowSpeed;
            }
            else if (buffCount.buff.DeBuffId[1] == '1')
            {
                enemyData.greenEnemyData.speed += buffCount.buff.slowSpeed * enemyData.speed;
            }

        }

        if (buffCount.buff.DeBuffId[0] == '2')
        {
            if (buffCount.buff.DeBuffId[1] == '0')
            {
                enemyData.greenEnemyData.armor += buffCount.buff.breakArmor;
                enemyData.greenEnemyData.spell += buffCount.buff.breakArmor;
            }
            else if (buffCount.buff.DeBuffId[1] == '1')
            {
                enemyData.greenEnemyData.armor += buffCount.buff.breakArmor* enemyData.armor;
                enemyData.greenEnemyData.spell += buffCount.buff.breakArmor * enemyData.spell;
            }

        }

        //加特效，同种特效只会存在一个
        if (buffCount.buff.DeBuffId[0] == '0')
        {
            if (dotNumber != 0)
            {
                dotNumber++;
                return;
            }
            GameObject tempDotEffect = GameObject.Instantiate(dotEffect, effectPosion1.position, effectPosion1.rotation);
            tempDotEffect.transform.parent = transform;
            dotNumber++;
        }
        else if (buffCount.buff.DeBuffId[0] == '1')
        {
            if (slowNumber != 0)
            {
                slowNumber++;
                return;
            }
            GameObject tempSlowEffect = GameObject.Instantiate(slowEffect, effectPosion1.position, effectPosion1.rotation);
            tempSlowEffect.transform.parent = transform;
            slowNumber++;
        }

        else if (buffCount.buff.DeBuffId[0] == '2')
        {
            if (breakNumber != 0)
            {
                breakNumber++;
                return;
            }
      //      Vector3 tempPostion = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
            GameObject tempBreakEffect = GameObject.Instantiate(braekEffect, effectPosion2.position, effectPosion2.rotation);
            tempBreakEffect.transform.parent = transform;
            breakNumber++;
        }
    }
    public void BuffClear()
    {

        //enemyData.speed += buffs[index].buff.slowSpeed;
        //enemyData.dot -= buffs[index].buff.dotAttack;
        //enemyData.spell += buffs[index].buff.breakArmor;
        //enemyData.armor += buffs[index].buff.breakArmor;
        if (buffs[index].buff.DeBuffId[0] == '0')
        {
            enemyData.greenEnemyData.dot -= buffs[index].buff.dotAttack;
        }


        if (buffs[index].buff.DeBuffId[0] == '1')
        {
            if (buffs[index].buff.DeBuffId[1] == '0')
            {
                enemyData.greenEnemyData.speed -= buffs[index].buff.slowSpeed;
            }
            else if (buffs[index].buff.DeBuffId[1] == '1')
            {
                enemyData.greenEnemyData.speed -= buffs[index].buff.slowSpeed * enemyData.speed;
            }

        }

        if (buffs[index].buff.DeBuffId[0] == '2')
        {
            if (buffs[index].buff.DeBuffId[1] == '0')
            {
                enemyData.greenEnemyData.armor -= buffs[index].buff.breakArmor;
                enemyData.greenEnemyData.spell -= buffs[index].buff.breakArmor;
            }
            else if (buffs[index].buff.DeBuffId[1] == '1')
            {
                enemyData.greenEnemyData.armor -= buffs[index].buff.breakArmor * enemyData.armor;
                enemyData.greenEnemyData.spell -= buffs[index].buff.breakArmor * enemyData.spell;
            }

        }

        buffs[index].keepCount = 0;

        if (buffs[index].buff.DeBuffId[0] == '0')
        {
            dotNumber--;
            if (dotNumber == 0)
            {
                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name == "Dot(Clone)")
                    {
                        Destroy(t.gameObject);
                    }
                }
            }
        }
        if (buffs[index].buff.DeBuffId[0] == '1')
        {
            slowNumber--;
            if (slowNumber == 0)
            {
                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name == "Slow(Clone)")
                    {
                        Destroy(t.gameObject);
                    }
                }
            }
        }
        if (buffs[index].buff.DeBuffId[0] == '2')
        {
            breakNumber--;
            if (breakNumber == 0)
            {
                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name == "Break(Clone)")
                    {
                        Destroy(t.gameObject);
                    }
                }
            }
        }

        buffs.RemoveAt(index);

    }

}
