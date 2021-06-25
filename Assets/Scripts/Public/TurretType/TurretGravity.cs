using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretGravity : MonoBehaviour {

    public List<GameObject> enemys = new List<GameObject>();

    public float rotateSpeed;
    private float haveRotate;
    private float timer = 0;           // 计时器
    private AttackData attackData;
    public Transform rotateHead;
    public GameObject fireEffect;
    public Transform effectPosition;

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
        attackData = GetComponent<AttackDataManager>().attackData;
    
    //    StartCoroutine(Attack());
    }
    void Update()
    {

        timer += Time.deltaTime;
        if (enemys.Count > 0 && timer > (1 / (attackData.attackSpeed + attackData.greenData.greenSpeed)))
        {
            timer = 0;
            Attack();
        }
  //   attackData.head.transform.Rotate(0, 0, attackData.rotateSpeed * Time.deltaTime);
    //   Debug.Log(attackData.head +"  "+ attackData.rotateSpeed);
        haveRotate += rotateSpeed * Time.deltaTime;
        if (Math.Abs(haveRotate) >= 120)
        {
            rotateSpeed = -rotateSpeed;
          //  haveRotate = 0;
        }
        if(rotateHead!=null)
            rotateHead.Rotate(0, haveRotate * Time.deltaTime, 0);
    }
    void Attack()
    {
        GameObject.Instantiate(fireEffect, effectPosition.position, transform.rotation);
            for (int index = 0; index < enemys.Count; index++)
            {

                if (enemys[index] == null)
                {
                    continue;
                }
                float distance = Vector3.Distance(transform.position, enemys[index].transform.position);

                //修改公式为除以半径
                enemys[index].GetComponent<EnemyBehaviour>().TakeDamager((attackData.attack + attackData.greenData.greenAttack) / (distance), attackData.attackType);
                //       Debug.Log(distance + "and" + attack / (distance * distance));

            }
            UpdateEnemys();
            //foreach (GameObject enemy in enemys)
            //{

            //    float distance = Vector3.Distance(transform.position, enemy.transform.position);
            //    if (enemy == null)
            //        continue;
            //    enemy.GetComponent<EnemyBehaviour>().TakeDamager((attackData.attack + attackData.greenData.greenAttack) / (distance * distance), attackData.attackType);
            //    //       Debug.Log(distance + "and" + attack / (distance * distance));

            //}
          


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
