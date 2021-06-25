using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{



    //void OnCollisionEnter(Collider col)
    //{
    //    Debug.Log("触碰");

    //    if (col.tag == "Player")
    //    {
    //        //TODO 增加背包里的数据
    //        Destroy(gameObject);
    //    }
    //}

    public void PickUp()
    {
        GameObject.Find("GameManager").GetComponent<ToolsManager>().GetItem(gameObject);
        Destroy(gameObject);

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
