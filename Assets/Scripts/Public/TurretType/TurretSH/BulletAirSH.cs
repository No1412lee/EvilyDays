using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrugType
{
    dot,
    slow,
    fire
}
public class BulletAirSH : MonoBehaviour {

    public DrugType drugType;
    public DeBuff debuff;
    public float attack;
    public AttackType attackType;
    public float desTime = 5.0f;
    public GameObject bombEffect;
    private float timer;
    public List<GameObject> enemys = new List<GameObject>();
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            if(drugType == DrugType.dot)
                col.GetComponent<EnemyDataManager>().SetBuffData(debuff);
            if(drugType == DrugType.slow)
                col.GetComponent<EnemyDataManager>().SetBuffData(debuff);
            if (drugType == DrugType.fire)
            {
                col.GetComponent<EnemyBehaviour>().TakeDamager(attack, attackType);
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                GameObject.Instantiate(bombEffect, position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }



	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > desTime)
            Destroy(gameObject);
	}
}
