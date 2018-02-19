using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharController : MonoBehaviour {
    [SerializeField]
    float moveSpeed = 6f;
    Vector3 forward, right;
    bool needToCheckMovement;
    Vector3 original_position;
    public Dictionary<Vector3, GameObject> touchingObjects;
    public GameObject test;
    
    void Start() {
        needToCheckMovement = false;
        forward = Camera.main.transform.forward;
        touchingObjects = new Dictionary<Vector3, GameObject>();
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void OnMouseUp() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        this.GetComponent<Collider>().enabled = false;

        if(Physics.Raycast(ray, out hit)) {
            if(hit.transform.CompareTag("GroundObject")) {
                GameObject tileHit = hit.collider.gameObject;
                float tileWidth = tileHit.GetComponent<MeshFilter>().mesh.bounds.extents.x;
                float tileHeight = tileHit.GetComponent<MeshFilter>().mesh.bounds.extents.y;
                float tileLength = tileHit.GetComponent<MeshFilter>().mesh.bounds.extents.z;

                float x_coord = tileHit.transform.position.x - tileWidth;
                float z_coord = tileHit.transform.position.z + tileLength;
                float y_coord = tileHit.transform.position.y + 4 * tileHeight;
                this.transform.position = new Vector3(x_coord, y_coord, z_coord);
            }
        } else {
            this.transform.position = original_position;
        }
        this.GetComponent<Collider>().enabled = true;
    }

    void OnMouseDown() {
        original_position = transform.position;
    }

    void OnMouseDrag() {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }
}