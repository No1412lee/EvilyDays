using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public List<GameObject> enemys = new List<GameObject>();
    private float timer=0;           // 计时器 
    private AttackData attackData;


    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    void Start()
    {
        attackData = this.GetComponent<AttackDataManager>().attackData;
   
        timer = attackData.attackSpeed;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (enemys.Count > 0 && timer > (1/(attackData.attackSpeed+attackData.greenData.greenSpeed)))
        {
            timer = 0;
            Attack();
        }
        if(enemys.Count>0 && enemys[0]!=null)   //调整方向
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = attackData.head.position.y; //y轴方向一致
            attackData.head.LookAt(targetPosition);           
        }

    }
    void Attack()
    {
        if(enemys[0]==null)
        {
            UpDataEnemys();  //去除怪物链表中的空元素
            timer += attackData.attackSpeed;
            return;
        }
        GameObject bullet = GameObject.Instantiate(attackData.bulletData.bulletPrefab, attackData.firePosition.position, attackData.firePosition.rotation);
        bullet.GetComponent<Bullet>().SetAttackData(attackData);
        bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform); //目标为队列里的第一个怪
    }
    void UpDataEnemys()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i--);
            }
        }
    }
}
