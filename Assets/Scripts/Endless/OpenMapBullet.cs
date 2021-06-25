using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapBullet : MonoBehaviour {
    private Transform target;
    public GameObject effect;
    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        GameObject.Find("GameManager").GetComponent<EndlessEnemySpawner>().nextBornPoint();
        GameObject.Find("AudioSource/Environment").GetComponent<AudioManager>().EnAudioOpenMap();
        GameObject.Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);   
        transform.Translate(Vector3.forward * 10 * Time.deltaTime);
	}
}
