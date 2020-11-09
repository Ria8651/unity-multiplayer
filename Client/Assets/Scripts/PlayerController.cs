using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float speed = 5;
    [Range(0, 1)]
    public float acceleration = 0.25f;

    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        Vector2 input = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
        ).normalized * speed;
        
        Vector3 currentVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        Vector3 targetVelocity =
            transform.right * input.x +
            transform.forward * input.y;
        
        Vector3 difference = (targetVelocity - currentVelocity) * acceleration;
        rb.velocity += difference;

        ClientSend.PlayerData(transform.position, rb.velocity);
    }
}