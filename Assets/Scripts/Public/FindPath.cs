using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public List<Path> paths;
    public float nextWayPointDistance = 0.1f;
    private Seeker seeker;
    private MapData mapData;
    private int nowPath;
    private Vector3 nowPosition;

    public delegate IEnumerator PathsDelegate();
    public PathsDelegate pathsDelegate = null;

    public bool reachable(Path p, Vector3 tP)
    {
        Vector3 end = p.vectorPath[p.vectorPath.Count - 1];
        end.y = tP.y;
        return Vector3.Distance(tP, end) < nextWayPointDistance;
    }

    public void onPathComplete(Path p)
    {
        nowPath++;
        if (reachable(p, mapData.wayPoints[nowPath]))
        {
            paths.Add(p);
            nowPosition = mapData.wayPoints[nowPath];
        }
        if (nowPath + 1 < mapData.wayPoints.Count)
        {
            seeker.StartPath(nowPosition, mapData.wayPoints[nowPath + 1], onPathComplete);
        }
        else
        {
            if (pathsDelegate != null)
            {
                StartCoroutine(pathsDelegate());
            }
        }
    }

    public void findNewPath(int bornPoint)
    {
        paths.Clear();
        nowPath = bornPoint;
        nowPosition = mapData.wayPoints[nowPath];
        seeker.StartPath(nowPosition, mapData.wayPoints[nowPath + 1], onPathComplete);
    }

    // Use this for initialization
    void Start()
    {
        mapData = GameObject.Find("Map").GetComponent<MapData>();
        seeker = GetComponent<Seeker>();
        paths = new List<Path>();
        //findNewPath();
    }
}
