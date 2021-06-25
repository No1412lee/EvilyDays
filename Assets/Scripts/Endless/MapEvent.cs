using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapEvent : MonoBehaviour {


    [System.Serializable]public class ColliderEvent : UnityEvent { }
    public ColliderEvent WhenCollider;
    public ColliderEvent WhenLeave;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            WhenCollider.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhenLeave.Invoke();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
