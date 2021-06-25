using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject turret;

    private bool down = true;

    private bool mouse = false;

    public IEnumerator time()
    {
        yield return new WaitForSeconds((float)0.5);
        down = false;
        //出现介绍

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine("time");
        mouse = true;
        if(!down)
        {
            //todo
        }
        return;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("time");
        if (Input.GetMouseButtonUp(0))
            mouse = false;
        if (mouse)
            return;
        if (down)
            build();
        //介绍消失
        down = true;
        return;
    }

    public void build ()
    {
        UIManager manager = GameObject.Find("MainCanvas").GetComponent<UIManager>();
        manager.buildTurretByUI(gameObject);
        manager.activeClear();
        manager.build.mapCubesClear();
    }
}
