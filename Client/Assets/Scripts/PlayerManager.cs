using UnityEngine;
using TMPro;

public enum PlayerStates {
    none,
    lobby,
    bunny,
    human
}

public class PlayerManager : MonoBehaviour {
    public int id;
    public string username;
    public PlayerStates state;
    public float predictionAcceleration = 0.3f;
    public TMP_Text usernameFeild;

    [HideInInspector]
    public Vector3 velocity;

    Vector3 position;

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
        if (state == newState) {
            return;
        }

        switch (newState) {
            case PlayerStates.lobby:
                UIManager.instance.SetState(UIState.lobby);
                Debug.Log("You are in the lobby");
                break;

            case PlayerStates.bunny:
                UIManager.instance.SetState(UIState.game);
                Debug.Log("You are the bunny!");
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                break;

            case PlayerStates.human:
                UIManager.instance.SetState(UIState.game);
                Debug.Log("You are a human.");
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case PlayerStates.none:
                UIManager.instance.SetState(UIState.menu);
                Debug.LogError("Player Manager: playerState set to none!");
                break;
        }

        state = newState;
    }
}