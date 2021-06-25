using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretHaiMa : MonoBehaviour {

    private AttackData attackData;
    public Transform rotateHead;
    public float rotateSpeed;
    public float gainTime;
    public int gainBrain;
    private float timer;
    // Use this for initialization

    void Start () {
        attackData = GetComponent<AttackDataManager>().attackData;
        timer = gainTime;
       
    }
	
    //public void Effect()
    //{
    //    if (SceneManager.GetActiveScene().name == "Endless")
    //    {
    //        StateManager.major.iqMax += 100;
    //        BuildManager.ChangeMoney(0);
    //        return;
    //    }
    //    else
    //    {
    //        ChapterBuildManager.moneyMax += 100;
    //        ChapterBuildManager.ChangeMoney(0);
    //        return;
    //    }
    //}

    //public void DisEffect()
    //{
    //    if (SceneManager.GetActiveScene().name == "Endless")
    //    {
    //        StateManager.major.iqMax -= 100;
    //        BuildManager.ChangeMoney(0);
    //        return;
    //    }
    //    else
    //    {
    //        ChapterBuildManager.moneyMax -= 100;
    //        ChapterBuildManager.ChangeMoney(0);
    //        return;
    //    }
    //}
    // Update is called once per frame
    void Update () {

        rotateHead.transform.Rotate(0, rotateSpeed * Time.deltaTime,0 );
        timer += Time.deltaTime;
      
        if (timer>gainTime && MapCube.isBuildStage==false )
        {
            timer = 0;
            getBrain();
        }
    }

    public void getBrain()
    {
        if (SceneManager.GetActiveScene().name == "Endless")
            BuildManager.ChangeMoney(gainBrain);       
        else       
            ChapterBuildManager.ChangeMoney(gainBrain);


    }

}
