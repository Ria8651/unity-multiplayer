using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;
    
    public Transform target;
    public float acceleration = 0.1f;
    public Vector3 offset;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    void FixedUpdate() {
        if (target != null) {
            Vector3 difference = (target.position + offset - transform.position) * acceleration;
            transform.position += difference;
        }
    }
}
