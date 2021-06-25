using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRST : MonoBehaviour {

   // public List<GameObject> turrets = new List<GameObject>();
    public GameObject[] turrets;
    public List<Buff> buffs;
    public float randBuffSpeed=10.0f;
    public float randBuffTime = 20.0f;
    public float randBuffMaxAttack = 0.5f;
    public float randBuffMinAttack = -0.1f;
    public float randBuffMaxSpeed = 0.2f;
    public float randBuffMinSpeed = -0.05f;

    private float timer = 0;           // 计时器
    private float timer2 = 0;
    private AttackData attackData;
    public float rotateSpeed;
    public Transform effectPosition;
    public GameObject effectRandomBuff;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        attackData = GetComponent<AttackDataManager>().attackData;
        timer = attackData.attackSpeed;
        timer2 = randBuffSpeed;
    }
    void Update()
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (timer > attackData.attackSpeed)
        {
            timer = 0;
            GiveBuff();
        }
        if (timer2 > randBuffSpeed)
        {
            timer2 = 0;
            RandomBuff();
        }
        if (rotateSpeed > 0)
            attackData.head.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

    }

    void RandomBuff()
    {
        GameObject.Instantiate(effectRandomBuff, effectPosition.position, effectPosition.rotation);
        audioSource.Play();
        Buff tempBuff = new Buff();
        int id1 =(int) (010000 + (int)(Random.value * 9999));
        tempBuff.buffId = id1.ToString("000000");
        tempBuff.keepTime = randBuffTime;
        tempBuff.buffAttack = Random.value * (randBuffMaxAttack - randBuffMinAttack) + randBuffMinAttack;

        Buff tempBuff2 = new Buff();
        int id2 = (int)(110000 + (int)(Random.value * 9999));
        tempBuff2.buffId = id2.ToString("000000");
        tempBuff2.keepTime = randBuffTime;
        tempBuff2.buffSpeed = Random.value * (randBuffMaxSpeed - randBuffMinSpeed) + randBuffMinSpeed;

        turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject turret in turrets)
        {
            float distance = Vector3.Distance(transform.position, turret.transform.position);

            if (distance < transform.GetComponent<SphereCollider>().radius)
            {
                turret.GetComponent<AttackDataManager>().SetBuffData(tempBuff);
                turret.GetComponent<AttackDataManager>().SetBuffData(tempBuff2);
            }
        }
    }

    void GiveBuff()
    {

        turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject turret in turrets)
        {
            float distance = Vector3.Distance(transform.position, turret.transform.position);
      
            if (distance < transform.GetComponent<SphereCollider>().radius)
            {
                foreach (Buff tempBuffs in buffs)
                    turret.GetComponent<AttackDataManager>().SetBuffData(tempBuffs);
            }
        }
    }
}
