using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackDataManager : MonoBehaviour
{
    public AttackData attackData;
    public List<BuffCount> buffs;
    public GameObject fanwei;
    //private float attackNormal;
    //private float attackSpeedNormal;
    
    private int index;
    void Start()
    {
        //attackNormal = attackData.attack;
        //attackSpeedNormal = attackData.attackSpeed;
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
    public void ShowFanwei()
    {
        fanwei.SetActive(true);
        float tempRadious = GetComponent<SphereCollider>().radius;
        fanwei.transform.localScale = new Vector3(tempRadious, tempRadious, tempRadious);
    }
    public void HideFanwei()
    {
        fanwei.SetActive(false);
    }
    
    public bool Exist(Buff buff)
    {
        for (int j = 0; j < buffs.Count; j++)
        {
            if (buffs[j].buff.buffId == buff.buffId)
            {
                return true;
            }
        }
        return false;
    }
    //public GameObject explosionEffect; //爆炸特效 
    public void SetBuffData(Buff buff, float passTime = 0)
    {

        if (buffs != null)
        {
            for (int j = 0; j < buffs.Count; j++)
            {
                if (buffs[j].buff.buffId == buff.buffId)
                {
                    //     Debug.Log("刷新" + transform.name);
                    buffs[j].keepCount = 0;
                    return;
                }
            }
        }//同种Buff不可叠加,只刷新持续时间
        BuffCount buffCount = new BuffCount();
        buffCount.keepCount = passTime;
        buffCount.buff = buff;
        //     Debug.Log(buffCount.keepCount+""+buffCount.buff.buffId);

        buffs.Add(buffCount);
        BuffBegin(buffCount);

    }


    public void BuffBegin(BuffCount buffCount)
    {

        if (buffCount.buff.buffId.Length < 3)
            return;
        //BUFF编号规则
        // 第一位的0、1、2、3分别表示：加攻击、加攻速、加攻击范围、加攻击数量
        // 第二位的0、1 分别表示：固定数值、百分比
        // 第三位及之后，表示该Buff的等级

        //TODO 加伤
        if(buffCount.buff.buffId[0]=='0')
        {
            if(buffCount.buff.buffId[1]=='0')
            {
                attackData.greenData.greenAttack += buffCount.buff.buffAttack;
            }
            else if(buffCount.buff.buffId[1]=='1')
            {
                attackData.greenData.greenAttack += buffCount.buff.buffAttack * attackData.attack;
            }
        }
        //TODO加攻速
        if (buffCount.buff.buffId[0] == '1')
        {
            if (buffCount.buff.buffId[1] == '0')
            {
                attackData.greenData.greenSpeed += buffCount.buff.buffSpeed;
            }
            else if (buffCount.buff.buffId[1] == '1')
            {
                attackData.greenData.greenSpeed += buffCount.buff.buffSpeed * attackData.attackSpeed;
            }
        }

        //   attackData.attack += buffCount.buff.buffAttack;
        //   attackData.attackSpeed -= buffCount.buff.buffSpeed;
    }
    public void BuffClear()
    {
        //  attackData.attack -= buffs[index].buff.buffAttack;
        //   attackData.attackSpeed += buffs[index].buff.buffSpeed;
        if(buffs[index].buff.buffId[0]=='0')
        {
            if(buffs[index].buff.buffId[1]=='0')
            {
                attackData.greenData.greenAttack -= buffs[index].buff.buffAttack;
            }
            else if (buffs[index].buff.buffId[1] == '1')
            {
                attackData.greenData.greenAttack -= buffs[index].buff.buffAttack * attackData.attack;
            }
        }

        if (buffs[index].buff.buffId[0] == '1')
        {
            if (buffs[index].buff.buffId[1] == '0')
            {
                attackData.greenData.greenSpeed -= buffs[index].buff.buffSpeed;
            }
            else if (buffs[index].buff.buffId[1] == '1')
            {
                attackData.greenData.greenSpeed -= buffs[index].buff.buffSpeed * attackData.attackSpeed;
            }
        }


        buffs[index].keepCount = 0;
        //    Debug.Log("清空" + buffs[index].buffId + "   index" + index + "buffs.count" + buffs.Count);
        buffs.RemoveAt(index);

    }
}
