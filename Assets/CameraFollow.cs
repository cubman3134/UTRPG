using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float testFloat = 4f;
    public float testD1 = 0;
    public float testD2 = 0;
    public float testD3 = 0;

    Camera mycam;
    // Use this for initialization
    void Start()
    {
        mycam = GetComponent<Camera>();
    }
    bool canRotate = true;
    float rotateAxis = 0;
    int curRotate;
    // Update is called once per frame
    void Update()
    {
        mycam.orthographicSize = (Screen.height / 100f) / testFloat;
        if (target)
        {

            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f) + new Vector3(testD1, testD2, testD3);
            if (canRotate && Input.GetButton("RotateCam"))
            {
                rotateAxis = Input.GetAxis("RotateCam");
                canRotate = false;
                curRotate = 0;
            }
            if (!canRotate)
            {
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