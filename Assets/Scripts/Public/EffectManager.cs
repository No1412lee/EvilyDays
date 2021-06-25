using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    public float DieTime=1.0f;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,DieTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
