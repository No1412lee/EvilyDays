using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretN2 : MonoBehaviour {

    public List<GameObject> enemys = new List<GameObject>();
    public float skillAngle;
    public GameObject N2Effect;
    private float timer=0;           // 计时器
    public DeBuff debuff;
    AttackData attackData;

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
        if (enemys.Count > 0 && timer > (1 / (attackData.attackSpeed + attackData.greenData.greenSpeed)))
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
            UpdateEnemys();  //去除怪物链表中的空元素
            timer += attackData.attackSpeed;
            return;
        }
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                UpdateEnemys();
                continue;
            }
            GameObject tempN2Effect = GameObject.Instantiate(N2Effect, attackData.firePosition.position, attackData.firePosition.rotation);
       //     Debug.Log(transform.FindChild("battery/fireposition"));
            tempN2Effect.transform.parent = transform.FindChild("battery/fireposition");

            Vector3 norVec = attackData.head.rotation * Vector3.forward;
      //      Debug.Log(norVec+"自身向量");
            Vector3 temVec = enemys[index].transform.position - attackData.head.position;
       //     Debug.Log(temVec+"目标连线");
            float angle = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;
      //      Debug.Log(angle);
            if (angle<= skillAngle * 0.5f || index==0)
            {
                enemys[index].GetComponent<EnemyDataManager>().SetBuffData(debuff);
                enemys[index].GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack+attackData.greenData.greenAttack, attackData.attackType);
   
            }
        }
        //GameObject bullet = GameObject.Instantiate(attackData.bulletData.bulletPrefab, attackData.firePosition.position, attackData.firePosition.rotation);
        //bullet.GetComponent<Bullet>().SetAttackData(attackData);
        //bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform); //目标为队列里的第一个怪
    }
    void UpdateEnemys()
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
