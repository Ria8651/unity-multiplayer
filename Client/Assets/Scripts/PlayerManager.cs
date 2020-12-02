using UnityEngine;
using TMPro;

public enum PlayerStates {
    none,
    waiting,
    ready,
    bunny,
    human
}

public class PlayerManager : MonoBehaviour {
    public int id;
    public string username;
    public PlayerStates playerState;
    public float predictionAcceleration = 0.3f;
    public TMP_Text usernameFeild;

    [HideInInspector]
    public Vector3 velocity;

    Vector3 position;

    void Awake() {
        UpdatePlayerState(playerState);
    }

    void FixedUpdate() {
        if (id != Client.instance.myId) {
            Vector3 predictedPosition = position + velocity * Time.deltaTime;
            transform.position += (predictedPosition - transform.position) * predictionAcceleration;
        }
    }

    public void UpdatePlayerData(Vector3 _position, Vector3 _velocity) {
        position = _position;
        velocity = _velocity;
    }

    public void Teleport(Vector3 position) {
        transform.position = position;

        if (GetComponent<Rigidbody>() != null) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void UpdatePlayerState(PlayerStates newState) {
        if (playerState == newState) {
            return;
        }

        switch (newState) {
            case PlayerStates.waiting:
                if (id == Client.instance.myId) {
                    UIManager.instance.SetState(UIState.waiting);
                }

                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.gray;
                break;

            case PlayerStates.ready:
                if (id == Client.instance.myId) {
                    UIManager.instance.SetState(UIState.ready);
                }

                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.cyan;
                break;

            case PlayerStates.bunny:
                if (id == Client.instance.myId) {
                    UIManager.instance.SetState(UIState.game);
                }

                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                break;

            case PlayerStates.human:
                if (id == Client.instance.myId) {
                    UIManager.instance.SetState(UIState.game);
                }

                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
                break;

            case PlayerStates.none:
                if (id == Client.instance.myId) {
                    UIManager.instance.SetState(UIState.menu);
                }

                break;
        }

        playerState = newState;
    }
}