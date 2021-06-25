using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBomb : MonoBehaviour {


    public AttackData attackData;
    public GameObject bombEffect;
    public List<GameObject> enemys = new List<GameObject>();
    private float bulletAttack;
	// Use this for initialization
    public void SetAttackData(AttackData _attackData)
    {
        this.attackData = _attackData;
        bulletAttack = _attackData.attack / 2+_attackData.greenData.greenAttack;
    }
   void  OnCollisionEnter( Collision collisionInfo )
    {
      //  Debug.Log("pengzhuang" + gameObject);
        enemys = GetComponentInChildren<BulletChildColider>().enemys;
        UpdateEnemys();

        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                continue;
            }
            enemys[index].GetComponent<EnemyBehaviour>().TakeDamager(bulletAttack, attackData.attackType);
        }
        Vector3 tempPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        GameObject.Instantiate(bombEffect, tempPosition, transform.rotation);
        Destroy(gameObject);
    }


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
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
