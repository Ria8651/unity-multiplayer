using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float speed = 8;
    [Range(0, 1)]
    public float acceleration = 0.1f;
    public float gravity = 20;
    public float gravityMultipliyer = 1.5f;
    public float jumpForce = 20;

    Rigidbody rb;
    bool grounded;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space) && grounded) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            grounded = false;
        }

        Vector2 input = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
        ).normalized * speed;

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 currentVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        Vector3 targetVelocity =
            transform.right * input.x +
            transform.forward * input.y;

        Vector3 difference = (targetVelocity - currentVelocity) * acceleration;
        rb.AddForce(difference, ForceMode.VelocityChange);
        
        float multiplier = localVelocity.y < 0 ? gravityMultipliyer : 1;
        rb.AddForce(-transform.up * gravity * multiplier, ForceMode.Acceleration);

        ClientSend.PlayerData(transform.position, rb.velocity);
    }

    void OnTriggerStay(Collider other) {
        grounded = true;
    }

    void OnTriggerExit(Collider other) {
        grounded = false;
    }
}