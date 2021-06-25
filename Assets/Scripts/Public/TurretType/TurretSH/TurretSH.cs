using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSH : MonoBehaviour {

    public List<GameObject> enemys = new List<GameObject>();
    private float timer = 0;           // 计时器
    private float timerEffect= 3.0f;
    private float timerDrug= 0f;

    public float effectSpeed;
    public float drupSpeed=3.0f;
    public GameObject fireEffect;
    [HideInInspector]
    public AttackData attackData;
 //   public float giveBuffSpeed = 0.5f;
    public Transform EffectPlace;
    public GameObject rotateMiddle;
    public float rotateSpeed1;
    public GameObject rotatePiece;
    public float rotateSpeed2;

    public List<DeBuff> debuffs;
    public int throwMax;
    public List<DrugData> drugs;
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
        //audioSource = GetComponent<AudioSource>();
        attackData = this.GetComponent<AttackDataManager>().attackData;
        timer = attackData.attackSpeed;
    }
    void Update()
    {
        timer += Time.deltaTime;
        timerEffect += Time.deltaTime;
        timerDrug += Time.deltaTime;
        rotateMiddle.transform.Rotate(0, rotateSpeed1 * Time.deltaTime, 0);
        rotatePiece.transform.Rotate(0, rotateSpeed2 * Time.deltaTime, 0);
        if ( enemys.Count>0 &&timerEffect>effectSpeed)
        {
            timerEffect = 0;
            GameObject.Instantiate(fireEffect, EffectPlace.position, EffectPlace.rotation);
            
        }
        if(timerDrug>drupSpeed)
        {
           timerDrug= 0;
            ThrowDrug();
        }
        if (enemys.Count > 0 && timer > attackData.attackSpeed)
        {
            timer = 0;
            Attack();
        }
    }
    void ThrowDrug()
    {
        float total = 0;
        foreach (DrugData drug in drugs)
        {
            total += drug.value;
        }
        //随机数：决定生成几瓶药剂
        int throwNumber=(int)(Random.value * (throwMax-1)+1);
        for(int index=0;index<throwNumber;index++)
        {
            int whichDrug;
            float randomPoint = Random.value * total;
            float tempSum=0;
            for (whichDrug = 0; whichDrug < drugs.Count; whichDrug++)
            {
                tempSum += drugs[whichDrug].value;
                if (randomPoint < tempSum)
                    break;
            }

            Vector3 tempPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            Quaternion RanRota = Quaternion.Euler(new Vector3(Random.value * 60-30, Random.value * 360, Random.value * 60-30));
            GameObject dropItem = Instantiate(drugs[whichDrug].drug, tempPosition, RanRota);
       //     dropItem.GetComponent<BulletBomb>().SetAttackData(attackData);

            Vector3 tempDropPosition = new Vector3(Random.value * 7 - 3.5f, Random.value*5f+2, Random.value * 7 - 3.5f);
            dropItem.GetComponent<Rigidbody>().AddForce(tempDropPosition, ForceMode.Impulse);
        }

    }

    void Attack()
    {
        UpdateEnemys(enemys);
        if (enemys.Count == 0)
            return;

        for (int index = 0; index < enemys.Count; index++)
        {

            if (enemys[index] == null)
            {
                continue;
            }
            foreach (DeBuff tempdebuff in debuffs)
            {
                enemys[index].GetComponent<EnemyDataManager>().SetBuffData(tempdebuff);
            }
            
        }


        //GameObject bullet = GameObject.Instantiate(attackData.bulletData.bulletPrefab, attackData.firePosition.position, attackData.firePosition.rotation);
        //Vector3 tempDropPosition = new Vector3(0, 10, 0);
        //bullet.GetComponent<Rigidbody>().AddForce(tempDropPosition, ForceMode.Impulse);
        //bullet.GetComponent<BulletIce>().SetAttackData(attackData);
        //bullet.GetComponent<BulletIce>().SetTarget(enemys[0].transform); //目标为队列里的第一个怪

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
