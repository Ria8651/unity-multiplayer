using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerManager))]
public class PlayerController : MonoBehaviour {
    public float speed = 5;
    [Range(0, 1)]
    public float acceleration = 0.25f;
    public float dashSpeed = 8;
    public float characterRepulsionForce;

    Rigidbody rb;
    Vector2 input;
    PlayerManager playerManager;

    void Start() {
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
    }

    void Update() {
        input = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
        ).normalized * speed;

        if (playerManager.playerState == PlayerStates.bunny) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Dash();
            }
        }
    }

    void FixedUpdate() {
        Vector3 currentVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        Vector3 targetVelocity =
            transform.right * input.x +
            transform.forward * input.y;
        
        Vector3 difference = (targetVelocity - currentVelocity) * acceleration;
        rb.velocity += difference;

        playerManager.velocity = rb.velocity;

        ClientSend.PlayerData(transform.position, rb.velocity);
    }

    public void Dash() {
        if (rb.velocity.sqrMagnitude > 0.01f) {
            rb.velocity = rb.velocity.normalized * dashSpeed;
        }
    }

    void OnTriggerStay(Collider other) {
        PlayerManager otherPlayer = other.GetComponent<PlayerManager>();
        if (otherPlayer != null) {
            if (otherPlayer.playerState == PlayerStates.human && playerManager.playerState == PlayerStates.bunny) {
                GameManager.instance.InfectPlayer(otherPlayer);
            }

            Transform otherTransform = other.transform;
            Vector3 difference = transform.position - otherTransform.position;

            rb.AddForce(difference.normalized * characterRepulsionForce * (0.4f - difference.sqrMagnitude), ForceMode.Impulse);
        }
    }
}