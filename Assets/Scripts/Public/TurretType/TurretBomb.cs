using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBomb : MonoBehaviour
{


    public List<GameObject> enemys = new List<GameObject>();
    private float timer = 0;           // 计时器
    public GameObject fireEffect;
    [HideInInspector]
    public AttackData attackData;
    public Transform effectPosition;
    private AudioSource audioSource;
    public int randBulletMax;

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
     //   turretTransform.localPosition = gameObject.transform.position + Random.insideUnitSphere ;
    
        timer += Time.deltaTime;
        if (enemys.Count > 0 && timer > (1 / (attackData.attackSpeed + attackData.greenData.greenSpeed)))
        {
            timer = 0;
            Attack();
        }

    }
    void Attack()
    {
        UpdateEnemys();
        if (enemys.Count == 0)
            return;
        int bulletNumber=(int)(Random.value*(randBulletMax-1)+1);
   
        for (int j = 0; j < bulletNumber; j++)
        {
            
            Vector3 tempPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);        
            Quaternion RanRota = Quaternion.Euler (new Vector3(Random.value * -180,Random.value * 360,Random.value * 360));         
            GameObject dropItem = Instantiate(attackData.bulletData.bulletPrefab, tempPosition, RanRota);
            dropItem.GetComponent<BulletBomb>().SetAttackData(attackData);

            Vector3 tempDropPosition = new Vector3(Random.value * 5- 2.5f, 3, Random.value * 5 - 2.5f);
            dropItem.GetComponent<Rigidbody>().AddForce(tempDropPosition, ForceMode.Impulse);
        }

        //Vector3 temp = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z-0.5f);
        audioSource.Play();
        GameObject.Instantiate(fireEffect, effectPosition.position, effectPosition.rotation);
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                continue;
            }
            enemys[index].GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack+attackData.greenData.greenAttack, attackData.attackType);
        }
        
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


        //// 找出第一个空元素 O(n)
        //int count = enemys.Count;
        //for (int i = 0; i < count; i++)
        //    if (enemys[i] == null)
        //    {
        //        // 记录当前位置
        //        int newCount = i++;

        //        // 对每个非空元素，复制至当前位置 O(n)
        //        for (; i < count; i++)
        //            if (enemys[i] != null)
        //                enemys[newCount++] = enemys[i];

        //        // 移除多余的元素 O(n)
        //        enemys.RemoveRange(newCount, count - newCount);
        //        break;
        //    }
    }
}
