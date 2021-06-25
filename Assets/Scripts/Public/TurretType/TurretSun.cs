using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretSun : MonoBehaviour {

    //   public List<GameObject> enemys = new List<GameObject>();

    public Transform SunPosition;
    public float revolutionSpeed;

    public Transform EarthPosition;
    public float rotationSpeed;
    private float timer = 0;           // 计时器
    private AttackData attackData;

    public List<GameObject> enemys = new List<GameObject>();
    public GameObject fireEffect;
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
        GameObject.Find("AudioSource/Bgm").GetComponent<AudioManager>().BGMUniversy();
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
            SunPosition.Rotate(0, revolutionSpeed* Time.deltaTime, 0);
            EarthPosition.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            
    }
    void Attack()
    {
            GameObject.Instantiate(fireEffect, SunPosition.position, transform.rotation);
            for (int index = 0; index < enemys.Count; index++)
            {

                if (enemys[index] == null)
                {
                    continue;
                }
                float distance = Vector3.Distance(transform.position, enemys[index].transform.position);
                // 修改公式为除以半径，而不是半径的平方
                enemys[index].GetComponent<EnemyBehaviour>().TakeDamager((attackData.attack + attackData.greenData.greenAttack) / (distance), attackData.attackType);
               

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
