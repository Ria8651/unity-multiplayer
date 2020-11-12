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
    public TMP_Text usernameFeild;

    public void Awake() {
        Debug.Log(state);
    }

    public void Teleport(Vector3 position) {
        transform.position = position;

        if (GetComponent<Rigidbody>() != null) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void UpdatePlayerState(PlayerStates newState) {
        Debug.Log(state);
        Debug.Log(newState);

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