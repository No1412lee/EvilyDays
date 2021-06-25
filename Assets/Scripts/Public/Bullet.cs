using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AttackData attackData;

    private Transform target;
    private Vector3 targetCenter;
    public GameObject bombEffect;
    public DeBuff debuff;
    public void SetAttackData(AttackData _attackData)
    {
        this.attackData = _attackData;
       
    }

    public void SetTarget(Transform _target)
    {
        this.target = _target;
        targetCenter = target.transform.position;
        targetCenter.y = 0.5f;
    }
    void Update()
    {
        if (target == null)  //目标消失时
        {
            Die();
            return;
        }

        transform.LookAt(targetCenter);
        transform.Translate(Vector3.forward * attackData.bulletData.bulletSpeed * Time.deltaTime);
        Vector3 dir = targetCenter - transform.position;

        if (dir.magnitude < attackData.bulletData.distanseArrive)
        {
            target.GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack + attackData.greenData.greenAttack, attackData.attackType);
            if(debuff.DeBuffId.Length==3) //上Debuff
            {
                target.GetComponent<EnemyDataManager>().SetBuffData(debuff); 
            }



   //         GameObject tempEffect = GameObject.Instantiate(bombEffect, targetCenter, target.rotation);
            GameObject tempEffect = GameObject.Instantiate(bombEffect,transform.position,transform.rotation);
            //Debug.Log(tempEffect.transform.position);
            tempEffect.transform.parent = target;
            Destroy(this.gameObject);
        }
    }
    void Die()
    {
        //  GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
