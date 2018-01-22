using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 6f;
    Vector3 forward, right;
    bool needToCheckMovement;
    //public GameObject[] groundObjects;
    public Dictionary<Vector3, GameObject> touchingObjects;
    public GameObject test;

    private void OnTriggerEnter(Collider other)
    {
        //touchingObjects[test.GetComponent<Map>().map[new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.z)].transform.position]
        //    = test.GetComponent<Map>().map[new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.z)];
        //Debug.Log(touchingObjects.Count);

        //GameObject g = other.gameObject;
        //touchingObjects[g.transform.position] = g;
        touchingObjects[other.gameObject.transform.position] = other.gameObject;
        //Debug.Log(touchingObjects.Count);
        //Debug.Log(other.gameObject.gameObject.transform.position.x + " " + other.gameObject.gameObject.transform.position.y + " " + other.gameObject.gameObject.transform.position.z);

    }

    private void OnTriggerExit(Collider other)
    {
        touchingObjects.Remove(other.gameObject.transform.position);
        //touchingObjects.Remove(test.GetComponent<Map>().map[new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.z)].transform.position);
    }

    void Start()
    {
        needToCheckMovement = false;
        forward = Camera.main.transform.forward;
        touchingObjects = new Dictionary<Vector3, GameObject>();
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            //GameObject closest = findClosestGroundObject(test.GetComponent<Map>().map);
            Move();
            MoveY();
            needToCheckMovement = true;
        }
        else if (needToCheckMovement == true)
        {
            GameObject closest = findClosestGroundObject(test.GetComponent<Map>().map);
            moveTowards(closest);
            //Debug.Log(closest.name);
            needToCheckMovement = false;
        }
    }

    void MoveY()
    {
        GameObject closest = findClosestGroundObject(test.GetComponent<Map>().map);
        //GameObject closest = findClosestGroundObject(touchingObjects.Values.ToList());
        if (closest)
        {
            //if((closest.transform.position.y + .5)> this.gameObject.transform.position.y)
            //{
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, (float)(closest.transform.position.y + 1.5), transform.position.z), 5);
                //transform.position += new Vector3(0, closest.transform.position.y - this.gameObject.transform.position.y);
            //} else if(transform.position.y > (closest.transform.position.y + .5))
            //{
                //transform.position -= new Vector3(0, this.gameObject.transform.position.y - closest.transform.position.y);
            //}
        } else
        {
            if(transform.position.y != 1.5)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, (float)1.5, transform.position.y), 5);
            }
        }
    }

    void Move()
    {
        //Vector3 direction;
        Vector3 rightMovement;
        Vector3 upMovement;
        if (!Camera.main.GetComponent<CameraFollow>().flip)
        {
            rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey") * Camera.main.GetComponent<CameraFollow>().horiz;
            upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey") * Camera.main.GetComponent<CameraFollow>().vert;
        } else
        {
            rightMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey") * Camera.main.GetComponent<CameraFollow>().horiz;
            upMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey") * Camera.main.GetComponent<CameraFollow>().vert;
        }
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    double distanceCalc(GameObject g1, GameObject g2)
    {
        //return Math.Sqrt (Math.Pow (g2.transform.position.x - g1.transform.position.x, 2) + Math.Pow (g2.transform.position.z - g1.transform.position.z, 2));
        Renderer rend1 = g1.GetComponent<Renderer>();
        Renderer rend2 = g2.GetComponent<Renderer>();
        return Math.Sqrt(Math.Pow(rend2.bounds.center.x - rend1.bounds.center.x, 2) + Math.Pow(rend2.bounds.center.z - rend1.bounds.center.z, 2));
    }

    GameObject findClosestGroundObject(List<GameObject> gameObjectsList)
    {
        //Debug.Log(groundObjects.Length);
        if (gameObjectsList.Count == 0)
        {
            return null;
            //groundObjects = GameObject.FindGameObjectsWithTag("GroundObject");
        }
        double distance;
        double bestDistance = distanceCalc(this.gameObject, gameObjectsList[0]);
        GameObject closest = gameObjectsList[0];
        CapsuleCollider playerCollider = this.gameObject.GetComponent<CapsuleCollider>();
        for (int i = 1; i < gameObjectsList.Count; i++)
        {
            //if (playerCollider.IsTouching(groundObjects[i].GetComponent<Collider2D>())) {
            distance = distanceCalc(this.gameObject, gameObjectsList[i]);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                closest = gameObjectsList[i];
            }
        }
        return closest;
    }
    
    

    void moveTowards(GameObject target)
    {
        float playerRad = this.gameObject.transform.position.z / 2;
        float targetRad = target.transform.position.z / 2;
        Renderer playerRend = this.gameObject.GetComponent<Renderer>();
        Renderer targetRend = target.GetComponent<Renderer>();
        Vector3 desiredPosition = new Vector3(targetRend.bounds.center.x, (float)(target.transform.position.y + 1.5), targetRend.bounds.center.z);
        while (Input.anyKey == false && this.gameObject.transform.position != desiredPosition)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
        }
    }
}