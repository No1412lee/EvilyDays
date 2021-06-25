using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFanwei : MonoBehaviour {

	// Use this for initialization
    public GameObject rotateHead;
    public float rotateSpeed;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rotateHead.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
	}
}
