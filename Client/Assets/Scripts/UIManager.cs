using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIState {
    menu,
    waiting,
    ready,
    game
}

public class UIManager: MonoBehaviour {
    public static UIManager instance;

    public GameObject menu;
    public GameObject lobby;
    public GameObject game;
    public TMP_InputField usernameField;
    public TMP_InputField ipField;
    public GameObject readyButton;

    UIState state;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        SetState(UIState.menu);
    }

    public void ConnectToServer() {
        Client.instance.ip = ipField.text;
        Client.instance.ConnectToServer();
    }

    public void SetState(UIState newState) {
        if (newState == state) {
            return;
        }

        switch (newState) {
            case UIState.menu:
                menu.SetActive(true);
                lobby.SetActive(false);
                game.SetActive(false);
                break;

            case UIState.waiting:
                menu.SetActive(false);
                lobby.SetActive(true);
                game.SetActive(false);

                readyButton.SetActive(true);
                break;

            case UIState.ready:
                menu.SetActive(false);
                lobby.SetActive(true);
                game.SetActive(false);

                readyButton.SetActive(false);
                break;

            case UIState.game:
                menu.SetActive(false);
                lobby.SetActive(false);
                game.SetActive(true);
                //usernameField.interactable = false;
                //ipField.interactable = false;
                break;
        }
    }
    
    public void Ready() {
        ClientSend.Ready();
    }
}