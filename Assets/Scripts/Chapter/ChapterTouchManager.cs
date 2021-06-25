using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterTouchManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject gameManager;
    public float transRate;
    [HideInInspector]
    public ChapterBuildManager build;

    private CameraControler cameraControler;
    private float oldDistance;
    private Vector2 oldPosition;
    private int oldCount;
    private bool select;
    private bool onUI;

    // Use this for initialization
    void Start()
    {
        build = gameManager.GetComponent<ChapterBuildManager>();
        cameraControler = mainCamera.GetComponent<CameraControler>();
    }

    public Vector2 getAvgPosition()
    {
        Vector2 sum = new Vector2(0.0f, 0.0f);
        for (int i = 0; i < Input.touchCount; i++)
        {
            sum += Input.touches[i].position;
        }
        return sum / Input.touchCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            oldCount = 0;
        else if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                // 手指按下时，要触发的代码
                oldCount = 1;
                select = true;
                onUI = EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
            }
            else if (!onUI)
            {
                if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    select = false;
                    // 手指滑动时，要触发的代码 
                    //float sX = Input.GetAxis("Mouse X");
                    //float sY = Input.GetAxis("Mouse Y");
                    Vector2 dP = oldPosition - getAvgPosition();
                    if (oldCount == 1)
                        cameraControler.moveCamera(dP.x * transRate, dP.y * transRate);
                }
                else if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    // 手指松开时，要触发的代码
                    if (select)
                        build.onSelectMapCube(Input.touches[0].position);
                    oldCount = 0;
                }
            }

            oldPosition = getAvgPosition();
        }
        else if (Input.touchCount == 2)
        {
            if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                Vector2 dP = oldPosition - getAvgPosition();
                if (oldCount == 2)
                    cameraControler.moveCamera(dP.x * transRate, dP.y * transRate);
                cameraControler.scaleFieldOfView(oldDistance / Vector2.Distance(Input.touches[0].position, Input.touches[1].position));
            }

            oldPosition = getAvgPosition();
            oldCount = 2;
            oldDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
        }
    }
}