using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharController : MonoBehaviour {
	[SerializeField]
	float moveSpeed = 6f;
	Vector3 forward, right;
    bool needToCheckMovement;
    public GameObject[] groundObjects;

    void Start () {
        needToCheckMovement = false;
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize (forward);
		right = Quaternion.Euler (new Vector3 (0, 90, 0)) * forward;
	}
	
	void Update () {
		if (Input.anyKey) {
			Move ();
			needToCheckMovement = true;
		} else if (needToCheckMovement == true) {
            GameObject closest = findClosestGroundObject();
            moveTowards(closest);
            //Debug.Log(closest.name);
			needToCheckMovement = false;
		}
	}

	void Move(){
		Vector3 direction = new Vector3 (Input.GetAxis ("HorizontalKey"), 0, Input.GetAxis ("VerticalKey"));
		Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis ("HorizontalKey");
		Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis ("VerticalKey");

		Vector3 heading = Vector3.Normalize (rightMovement + upMovement);

		transform.forward = heading;
		transform.position += rightMovement;
		transform.position += upMovement;
	}

	double distanceCalc(GameObject g1, GameObject g2){
        //return Math.Sqrt (Math.Pow (g2.transform.position.x - g1.transform.position.x, 2) + Math.Pow (g2.transform.position.z - g1.transform.position.z, 2));
        Renderer rend1 = g1.GetComponent<Renderer>();
        Renderer rend2 = g2.GetComponent<Renderer>();
        return Math.Sqrt(Math.Pow(rend2.bounds.center.x - rend1.bounds.center.x, 2) + Math.Pow(rend2.bounds.center.z - rend1.bounds.center.z, 2));
    }

	GameObject findClosestGroundObject(){
        //Debug.Log(groundObjects.Length);
		if (groundObjects.Length == 0) {
			groundObjects = GameObject.FindGameObjectsWithTag ("GroundObject");
		}
		double distance;
		double bestDistance = distanceCalc(this.gameObject, groundObjects[0]);
		GameObject closest = groundObjects[0];
        CapsuleCollider playerCollider = this.gameObject.GetComponent<CapsuleCollider>();
        for (int i = 1; i < groundObjects.Length; i++) {
            //if (playerCollider.IsTouching(groundObjects[i].GetComponent<Collider2D>())) {
            distance = distanceCalc(this.gameObject, groundObjects[i]);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                closest = groundObjects[i];
            }
		}
		return closest;
	}

    void moveTowards(GameObject target) {
        float playerRad = this.gameObject.transform.position.z / 2;
        float targetRad = target.transform.position.z / 2;
        Renderer playerRend = this.gameObject.GetComponent<Renderer>();
        Renderer targetRend = target.GetComponent<Renderer>();
        //Vector3 desiredPosition = new Vector3(target.transform.position.x, this.gameObject.transform.position.y, target.transform.position.z);'
        Vector3 desiredPosition = new Vector3(targetRend.bounds.center.x, this.gameObject.transform.position.y, targetRend.bounds.center.z);
        while (Input.anyKey == false && this.gameObject.transform.position != desiredPosition) {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            //rend1.bounds.center = Vector3.MoveTowards(rend1.bounds.center, rend2.bounds.center, moveSpeed * Time.deltaTime);
            //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, new Vector3(0,this.gameObject.transform.position.y ,target.transform.position.z), moveSpeed * Time.deltaTime);
        }
    }
}
