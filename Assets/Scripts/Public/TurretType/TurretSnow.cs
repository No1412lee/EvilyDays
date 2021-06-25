using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSnow : MonoBehaviour {



    public List<GameObject> enemys = new List<GameObject>();
    public List<GameObject> enemysNear = new List<GameObject>();
    private float timer = 0;           // 计时器
    private float timerBuff = 0;
    public GameObject fireEffect;
    [HideInInspector]
    public AttackData attackData;
    public float giveBuffSpeed=0.5f;
    public Transform EffectPlace;
    public DeBuff debuff;
    private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
        attackData = this.GetComponent<AttackDataManager>().attackData;
        timer = attackData.attackSpeed;
    }
    void Update()
    {
        timer += Time.deltaTime;
        timerBuff += Time.deltaTime;
        if (enemys.Count > 0 && timerBuff > giveBuffSpeed)
        {
            timerBuff = 0;
            GiveBuff();
        }

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
    void GiveBuff()
    {

        //TODO 给周围塔上debuff
        enemysNear = GetComponentInChildren<BulletChildColider>().enemys;
        UpdateEnemys(enemysNear);
        if (enemysNear.Count == 0)
            return;
        for (int index = 0; index < enemysNear.Count; index++)
        {

            if (enemysNear[index] == null)
            {
                continue;
            }
            enemysNear[index].GetComponent<EnemyDataManager>().SetBuffData(debuff);
        }
    }
    void Attack()
    {
        UpdateEnemys(enemys);
        if (enemys.Count == 0)
            return;

        audioSource.Play();
       // Vector3 temp = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        GameObject.Instantiate(fireEffect, EffectPlace.position, EffectPlace.rotation);

        GameObject bullet = GameObject.Instantiate(attackData.bulletData.bulletPrefab, attackData.firePosition.position, attackData.firePosition.rotation);
        Vector3 tempDropPosition = new Vector3(0, 10,0);
        bullet.GetComponent<Rigidbody>().AddForce(tempDropPosition, ForceMode.Impulse);
        bullet.GetComponent<BulletIce>().SetAttackData(attackData);
        bullet.GetComponent<BulletIce>().SetTarget(enemys[0].transform); //目标为队列里的第一个怪
    }

    void UpdateEnemys(List<GameObject> enemys)
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
