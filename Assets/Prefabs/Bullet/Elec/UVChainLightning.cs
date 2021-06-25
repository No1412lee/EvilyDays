using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// uv贴图闪电链
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class UVChainLightning : MonoBehaviour
{
    //美术资源中进行调整
    public float detail;//增加后，线条数量会减少，每个线条会更长。
    public float displacement ;//位移量，也就是线条数值方向偏移的最大值

    public Transform target;//链接目标
    public Transform start;
    public float yOffset = 0;
    private bool haveTarget = false;
    private bool haveRadius = false;
    private float radius;
    private LineRenderer _lineRender;
    private List<Vector3> _linePosList;
    private Vector3 targetFix;
    private Vector3 startFix;


    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _linePosList = new List<Vector3>();
        _lineRender.enabled = false;
    }
    public void setPosition(Transform _start,Transform _target)
    {
        if (_target != null)
            haveTarget = true;
        else
        {
            haveTarget = false;
            _lineRender.enabled = false;
            return;
        }
        start = _start;
        target = _target;
        targetFix = new Vector3(target.position.x,target.position.y+0.5f,target.position.z);
        startFix = new Vector3(start.position.x, 0, start.position.z);

    }

    public  void  setRadius(float _radius)
    {
        radius = _radius;
        haveRadius = true;
    }

    private void Update()
    {

        if(Time.timeScale != 0 && haveTarget==true && haveRadius==true)
        {
            _lineRender.enabled = true;
            _linePosList.Clear();
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            if (target != null)
            {
                endPos = targetFix + Vector3.up * yOffset;
            }
            if(start != null)
            {
                startPos = start.position + Vector3.up * yOffset;
            }

            CollectLinPos(startPos, endPos, displacement);
            _linePosList.Add(endPos);

            _lineRender.SetVertexCount(_linePosList.Count);
            for (int i = 0, n = _linePosList.Count; i < n; i++)
            {
                _lineRender.SetPosition(i, _linePosList[i]);
            }

 

            if (target == null ||  Vector3.Distance(startFix, target.position) >= radius)
            {
                haveTarget=false;
                haveRadius = false;
                target = null;
                _lineRender.enabled = false;
            }
        }
    }

    //收集顶点，中点分形法插值抖动
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < detail)
        {
            _linePosList.Add(startPos);
        }
        else
        {

            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            Vector3 midPos = new Vector3(midX,midY,midZ);

            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }


}    
