using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLightening : MonoBehaviour {

    //  [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
    public List<GameObject> enemysPink = new List<GameObject>();
    public float rotateSpeed;
    public Transform topRotate;
    public float TopRotateSpeed;
    public float PinkAttackPercent;
    public Transform topFirePosition;
    private bool haveEnemy=false;
    private float normalVol;
    AttackData attackData;
    // public GameObject elecEffect;
    //  public GameObject[] elecEffects;
    public List<GameObject> elecEffects = new List<GameObject>();
    public List<GameObject> elecEffectsPink = new List<GameObject>();
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
        attackData = this.GetComponent<AttackDataManager>().attackData;
        enemysPink = GetComponentInChildren<BulletChildColider>().enemys;
        audioSource = GetComponent<AudioSource>();
        normalVol = audioSource.volume;
    }

    void Update()
    {
        attackData.head.transform.Rotate(Random.value * rotateSpeed * Time.deltaTime, rotateSpeed * Time.deltaTime, Random.value * rotateSpeed * Time.deltaTime);
        topRotate.transform.Rotate(0, TopRotateSpeed * Time.deltaTime, 0);
        if (enemysPink.Count > 0 )
        {
            if (haveEnemy == false)
            {
                haveEnemy = true;
                audioSource.Play();
                audioSource.volume = normalVol;
            }
        }
        else
        {
            haveEnemy = false;
            audioSource.volume -= Time.deltaTime;
            if(audioSource.volume<0.1)
                 audioSource.Stop();
        }

        for (int index = 0; index < attackData.attackNumber ; index++)
        {
            bool haveEmpty = false;
            for (int i = 0; i < enemysPink.Count; i++)
            {
                if (enemysPink[i] == null)
                {
                    enemysPink.RemoveAt(i--);
                    haveEmpty = true;
                }
            }
            if (haveEmpty == true)
            {
                index = -1;
                continue;
            }

            if (index < enemysPink.Count)
            {

                enemysPink[index].GetComponent<EnemyBehaviour>().TakeDamager((attackData.attack + attackData.greenData.greenAttack)*PinkAttackPercent* Time.deltaTime, attackData.attackType);
                elecEffectsPink[index].GetComponent<UVChainLightning>().setPosition(topFirePosition, enemysPink[index].transform);
                float tempR = gameObject.transform.FindChild("ChildColider").GetComponent<SphereCollider>().radius;
                elecEffectsPink[index].GetComponent<UVChainLightning>().setRadius(tempR);        
               
            }
            else
            {
                elecEffectsPink[index].GetComponent<UVChainLightning>().setPosition(topFirePosition, null);
            }
        }

        for (int index = 0; index < attackData.attackNumber ; index++)
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
                elecEffects[index].GetComponent<UVChainLightning>().setPosition(attackData.firePosition, enemys[index].transform);
                elecEffects[index].GetComponent<UVChainLightning>().setRadius(transform.GetComponent<SphereCollider>().radius);
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
