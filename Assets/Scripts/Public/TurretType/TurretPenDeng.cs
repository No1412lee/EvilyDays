using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPenDeng : MonoBehaviour {

    public List<GameObject> enemys = new List<GameObject>();
    private float timer = 0;           // 计时器
    public GameObject fireEffect;
    [HideInInspector]
    public AttackData attackData;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
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
        if (enemys.Count > 0 && enemys[0] != null)   //调整方向
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = attackData.head.position.y; //y轴方向一致
            attackData.head.LookAt(targetPosition);
        }

    }
    void Attack()
    {
    //    Vector3 temp = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        GameObject.Instantiate(fireEffect, attackData.firePosition.position, attackData.firePosition.rotation);
        for (int index = 0; index < enemys.Count; index++)
        {

            if (enemys[index] == null)
            {
                continue;
            }
            enemys[index].GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack + attackData.greenData.greenAttack, attackData.attackType);
        }
        UpdateEnemys();
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
