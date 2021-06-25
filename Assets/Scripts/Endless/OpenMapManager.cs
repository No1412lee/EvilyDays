using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapManager : MonoBehaviour {
    public GameObject NextMapPanel;
    public List<Transform> transform;
    public int haveOpen = 0;
    public GameObject bullet;
    public Transform firePosition;
    public AudioSource audioSource;
    void OnTriggerEnter(Collider col)
    {
        if (!BuildManager.instance.isBuildStage)
            return;
        if (col.tag == "Player")
        {
            NextMapPanel.SetActive(true);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            NextMapPanel.SetActive(false);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void fireTheWall()
    {
        if (haveOpen >= 3)
            return;
        audioSource.Play();
        GameObject OpenMapBullet = GameObject.Instantiate(bullet, firePosition.position, firePosition.rotation);
        Vector3 tempDropPosition = new Vector3(haveOpen * 3, 15 + haveOpen * 5, 0);
        OpenMapBullet.GetComponent<Rigidbody>().AddForce(tempDropPosition, ForceMode.Impulse);
        OpenMapBullet.GetComponent<OpenMapBullet>().SetTarget(transform[haveOpen]);
        Debug.Log(transform[haveOpen]);
        haveOpen++;

    }
}
