using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [HideInInspector]
    public string id;

    private bool down = true;

    private bool mouse = false;

    public void use()
    {
        if (Convert.ToInt32(id.Substring(0, 1)) == 3)
            return;
        ToolsManager.instance.UseItem(id);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouse = true;
        StartCoroutine("time");
        return;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
            mouse = false;
        StopCoroutine("time");
        if (mouse)
            return;
        if (down)
            use();
        down = true;
        return;
    }

    public IEnumerator time()
    {
        yield return new WaitForSeconds((float)0.5);
            down = false;
    }

}
