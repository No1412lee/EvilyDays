//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DrugManager : MonoBehaviour
//{
//    public float odds;
//    public List<DrugData> drugs;

//    public void scaterDrug()
//    {

//        // 随机药剂
//        int i;
//        float total = 0;
//        foreach (DrugData drug in drugs)
//        {
//            total += drug.value;
//        }
//        float randomPoint = Random.value * total;
//        for (i = 0; i < drugs.Count; i++)
//        {
//            if (randomPoint < drugs[i].value)
//                break;
//            else
//                i++;
//        }
//        Vector3 pos = new Vector3();
//        pos.x = Random.Range(gameObject.transform.position.x - drugs[i].scopen, gameObject.transform.position.x + drugs[i].scopen);
//        pos.y = gameObject.transform.position.y;
//        pos.z = Random.Range(gameObject.transform.position.z - drugs[i].scopen, gameObject.transform.position.z + drugs[i].scopen);
//        if (pos.x < gameObject.transform.position.x + 0.5 && pos.x > gameObject.transform.position.x)
//            pos.x += (float)0.5;
//        if (pos.x > gameObject.transform.position.x - 0.5 && pos.x < gameObject.transform.position.x)
//            pos.x -= (float)0.5;
//        if (pos.y < gameObject.transform.position.y + 0.5 && pos.y > gameObject.transform.position.y)
//            pos.y += (float)0.5;
//        if (pos.y > gameObject.transform.position.y - 0.5 && pos.y < gameObject.transform.position.y)
//            pos.y -= (float)0.5;
//        Instantiate(drugs[i].drug, pos, Quaternion.identity);

//    }

//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        float randomPoint = Random.value;
//        Debug.Log(randomPoint);
//        if (randomPoint < odds)
//        {
//            scaterDrug();
//        }
//    }
//}
