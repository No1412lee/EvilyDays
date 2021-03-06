using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretYJSX : MonoBehaviour {

    //  [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
  
    AttackData attackData;
    // public GameObject elecEffect;
    //  public GameObject[] elecEffects;
    public List<GameObject> elecEffects = new List<GameObject>();
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

    }

    void Update()
    {
        if (enemys.Count > 0 && enemys[0] != null)   //调整方向
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = attackData.head.position.y; //y轴方向一致
            attackData.head.LookAt(targetPosition);
        }
      
        for (int index = 0; index < attackData.attackNumber; index++)
        {
            bool haveEmpty = false;
            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] == null)
                {
                    enemys.RemoveAt(i--);
                    haveEmpty = true;
                }
            }
            if (haveEmpty == true)
            {
                index = -1;
                continue;
            }

            if (index < enemys.Count)
            {
                enemys[index].GetComponent<EnemyBehaviour>().TakeDamager((attackData.attack + attackData.greenData.greenAttack) * Time.deltaTime, attackData.attackType);

                //   GameObject elec = GameObject.Instantiate(elecEffect, attackData.firePosition.position, attackData.firePosition.rotation);
                //    elec.transform.parent = transform;
                elecEffects[index].GetComponent<UVChainLightning>().setPosition(attackData.firePosition, enemys[index].transform);
                elecEffects[index].GetComponent<UVChainLightning>().setRadius(transform.GetComponent<SphereCollider>().radius);
                //       enemys[index].GetComponent<EnemyBehaviour>().TakeDamager(attackData.attack * Time.deltaTime, attackData.attackType);

                //    Debug.Log(index);
            }
            else
            {
                elecEffects[index].GetComponent<UVChainLightning>().setPosition(attackData.firePosition, null);
            }
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
    }
}
