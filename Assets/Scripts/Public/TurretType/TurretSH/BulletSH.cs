using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSH : MonoBehaviour {

    public GameObject airBullet;
    void OnCollisionEnter(Collision collisionInfo)
    {
        GameObject.Instantiate(airBullet, transform.position, transform.rotation);
        Destroy(gameObject);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
