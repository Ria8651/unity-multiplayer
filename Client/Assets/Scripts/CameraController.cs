using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;
    
    public PlayerManager target;
    [Range(0, 1)]
    public float acceleration = 0.1f;
    public float prediction;
    public float fovChange;
    [Range(0, 1)]
    public float dampedVelocityAcceleration;
    public Vector3 offset;

    Vector3 dampedVelocity;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    void FixedUpdate() {
        if (target != null) {
            Vector3 targetVelocity = target.transform.position + offset +
                dampedVelocity.sqrMagnitude * fovChange * -transform.forward +
                dampedVelocity * prediction;

            Vector3 difference = (targetVelocity - transform.position) * acceleration;
            transform.position += difference;

            dampedVelocity += (target.velocity - dampedVelocity) * dampedVelocityAcceleration;
        }
    }
}
