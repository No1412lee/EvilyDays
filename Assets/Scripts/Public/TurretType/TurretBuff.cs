using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretBuff : MonoBehaviour
{

   // public List<GameObject> turrets = new List<GameObject>();
    public GameObject[] turrets;
    private float timer = 0;           // 计时器
    private AttackData attackData;
    public float rotateSpeed;
    public List<Buff> buffs;
   // public  Buff buff;
    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.tag == "Turret")
    //    {
    //        turrets.Add(col.gameObject);
    //    }
    //}

    //void OnTriggerExit(Collider col)
    //{
    //    if (col.tag == "Turret")
    //    {
    //        turrets.Remove(col.gameObject);
    //    }
    //}
    void Start()
    {
        attackData = GetComponent<AttackDataManager>().attackData;
        timer = attackData.attackSpeed;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > attackData.attackSpeed)
        {
            timer = 0;
            GiveBuff();
        }
        if (rotateSpeed > 0)
            attackData.head.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

    }

    void GiveBuff()
    {
        turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject turret in turrets)
        {
            float distance = Vector3.Distance(transform.position, turret.transform.position);
            //       Debug.Log("givebuff"+turret.name+" count"+buff.keepCount + "distance" + distance);
            if (distance < transform.GetComponent<SphereCollider>().radius)   //距离小于触发器半径且不为自身
            {
                foreach (Buff tempBuff in buffs)
                    turret.GetComponent<AttackDataManager>().SetBuffData(tempBuff);
            }


        }
    }
  
           

}
