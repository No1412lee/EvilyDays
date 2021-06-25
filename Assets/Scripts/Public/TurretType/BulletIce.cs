using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletIce : MonoBehaviour
{
    public AttackData attackData;

    //public float attack;
    //public float bulletSpeed;
    //public int attactType;
    //public float distanseArrive = 1;

    private Transform target;
    public GameObject bombEffect;
    public DeBuff debuff;
    public List<GameObject> enemys = new List<GameObject>();
    private bool haveTatget;
    private Vector3 final;
    
    public void SetAttackData(AttackData _attackData)
    {
        this.attackData = _attackData;
    }

    public void SetTarget(Transform _target)
    {
        this.target = _target;
        final = new Vector3(target.position.x, target.position.y, target.position.z);
        haveTatget = true;
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        //  Debug.Log("pengzhuang" + gameObject);
        enemys = GetComponentInChildren<BulletChildColider>().enemys;
        UpdateEnemys();

        Vector3 tempPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        GameObject.Instantiate(bombEffect, tempPosition, transform.rotation);
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                //  UpdateEnemys();
                continue;
            }

            if (debuff != null) //上Debuff
            {
                enemys[index].GetComponent<EnemyDataManager>().SetBuffData(debuff);
            }
            enemys[index].GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack+attackData.greenData.greenAttack, attackData.attackType);
        }
        //  audioSource.Play();

        Destroy(gameObject);
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

    void Update()
    {
        if (target == null)  //目标消失时
        {
            //Die();
            haveTatget = false;

        }

        if (haveTatget)
        {
            transform.LookAt(target);
            final = new Vector3(target.position.x, target.position.y, target.position.z);
            transform.Translate(Vector3.forward * attackData.bulletData.bulletSpeed * Time.deltaTime);
        }
        else
        {
            transform.LookAt(final);
            transform.Translate(Vector3.forward * attackData.bulletData.bulletSpeed * Time.deltaTime);
        }
        //Vector3 dir = targetCenter - transform.position;

        //if (dir.magnitude < attackData.bulletData.distanseArrive)
        //{
        //    target.GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack, attackData.attackType);
        //    if (attackData.debuff != null) //上Debuff
        //    {
        //        target.GetComponent<EnemyDataManager>().SetBuffData(attackData.debuff);
        //    }

        //   // GameObject tempEffect = GameObject.Instantiate(bombEffect, transform.position, transform.rotation);  
        // //   tempEffect.transform.parent = target;
        //    Destroy(this.gameObject);
        //}
    }
    void Die()
    {

        Destroy(gameObject);
    }
}
