using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float testFloat = 4f;
    public float testD2 = 0;
    
    public float horiz = 1;
    public float vert = 1;
    public bool flip = false;

    Camera mycam;
    // Use this for initialization
    void Start()
    {
        mycam = GetComponent<Camera>();
    }
    bool canRotate = true;
    float rotateAxis = 0;
    int curRotate;
    int curState = 0;
    // Update is called once per frame
    void Update()
    {
        mycam.orthographicSize = (Screen.height / 100f) / testFloat;
        if (target)
        {

            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f) + new Vector3(0, testD2, 0);
            if (canRotate && Input.GetButton("RotateCam"))
            {
                rotateAxis = Input.GetAxis("RotateCam");
                canRotate = false;
                curRotate = 0;
            } else if (!canRotate)
            {
                if(curRotate == 0)
                {
                    curState+=(int)rotateAxis * -1;
                    if (curState == -1) curState = 3;
                    if (curState % 4 == 0)
                    {
                        flip = false;
                        horiz = 1;
                        vert = 1;
                    } else if(curState % 4 == 1)
                    {
                        flip = true;
                        horiz = 1;
                        vert = -1;
                    } else if(curState % 4 == 2)
                    {
                        flip = false;
                        horiz = -1;
                        vert = -1;
                    } else
                    {
                        flip = true;
                        horiz = -1;
                        vert = 1;
                    }
                }
                transform.eulerAngles += new Vector3(0, rotateAxis * 3, 0);
                curRotate += 3;
                if (curRotate == 90)
                {
                    canRotate = true;
                }
            }
        }
    }
}